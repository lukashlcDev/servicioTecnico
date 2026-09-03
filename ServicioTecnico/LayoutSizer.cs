using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace ServicioTecnico;

// Escalado proporcional uniforme de un contenedor y todos sus controles (recursivo).
// Invariantes de diseño:
// - El snapshot capturado es inmutable y es la ÚNICA fuente de verdad: toda
//   transformación deriva de snapshot x factor, nunca del estado actual, para
//   evitar acumulación de error (drift) en ciclos repetidos de maximize/restore.
// - El factor es uniforme: Min(anchoDisp/baseAncho, altoDisp/baseAlto) con límites
//   [0.25, 4.0], para preservar exactamente la relación de aspecto del diseño.
// - Identidad exacta en factor 1.0: se restaura la posición/tamaño originales del
//   contenedor y las referencias de fuente originales de cada control (equivalencia
//   byte a byte con el diseño); el centrado proporcional solo aplica con factor != 1.
// - Seguridad de fuentes: en cada cambio de factor se crean las fuentes nuevas,
//   se asignan a los controles y recién después se disponen las anteriores; así
//   ninguna fuente se dispone mientras algún control la siga referenciando.
//
// Limitación DPI conocida (documentada, sin workaround en P2): el cálculo se basa
// exclusivamente en el ClientSize lógico. En una aplicación DPI-Unaware el
// ClientSize en tamaño normal no varía con el DPI del sistema, por lo que el
// factor es 1.0 y este escalado NO corrige el overflow físico a 150% (bitmap
// scaling del OS sobre pantallas chicas, p.ej. 1030x561 -> 1545x842 sobre
// 1366x768). Corregirlo exige DPI-awareness o acotar el tamaño inicial, fuera
// del alcance de P2.

public sealed class LayoutSizer : IDisposable
{
	public const float FactorMinimo = 0.25f;
	public const float FactorMaximo = 4f;

	private readonly Control _contenedor;
	private readonly Size _baseClientSize;
	private readonly Point _baseContenedorLocation;
	private readonly Size _baseContenedorSize;
	private readonly Snapshot[] _snapshots;
	private Dictionary<(string, FontStyle, float), Font> _fuentesEscaladas;
	private float _factorActual = float.NaN;
	private bool _disposed;

	private LayoutSizer(Control contenedor, Size baseClientSize, Snapshot[] snapshots)
	{
		_contenedor = contenedor;
		_baseClientSize = baseClientSize;
		_baseContenedorLocation = contenedor.Location;
		_baseContenedorSize = contenedor.Size;
		_snapshots = snapshots;
	}

	public static LayoutSizer Capture(Control contenedor, Size baseClientSize)
	{
		if (contenedor == null)
		{
			throw new ArgumentNullException("contenedor");
		}
		if (baseClientSize.Width <= 0 || baseClientSize.Height <= 0)
		{
			throw new ArgumentOutOfRangeException("baseClientSize", "baseClientSize debe ser mayor que cero en ambos ejes");
		}
		List<Snapshot> lista = new List<Snapshot>();
		CapturarRecursivo(contenedor, lista);
		return new LayoutSizer(contenedor, baseClientSize, lista.ToArray());
	}

	private static void CapturarRecursivo(Control padre, List<Snapshot> lista)
	{
		foreach (Control control in padre.Controls)
		{
			Font fuente = control.Font;
			bool esLabelAutoSize = control is Label && ((Label)control).AutoSize;
			lista.Add(new Snapshot(control, control.Location, control.Size, fuente, fuente.Name, fuente.Style, fuente.Size, esLabelAutoSize));
			CapturarRecursivo(control, lista);
		}
	}

	public float CurrentFactor => _factorActual;

	public Size BaseClientSize => _baseClientSize;

	public Point BaseContainerLocation => _baseContenedorLocation;

	public Size BaseContainerSize => _baseContenedorSize;

	public int SnapshotCount => _snapshots.Length;

	public float ComputeFactor(Size currentClientSize)
	{
		float fx = (float)currentClientSize.Width / (float)_baseClientSize.Width;
		float fy = (float)currentClientSize.Height / (float)_baseClientSize.Height;
		float factor = Math.Min(fx, fy);
		if (factor < FactorMinimo)
		{
			factor = FactorMinimo;
		}
		if (factor > FactorMaximo)
		{
			factor = FactorMaximo;
		}
		return factor;
	}

	public void Apply(Size currentClientSize)
	{
		if (_disposed)
		{
			throw new ObjectDisposedException("LayoutSizer");
		}
		if (currentClientSize.Width <= 0 || currentClientSize.Height <= 0)
		{
			return;
		}
		if (_contenedor.IsDisposed)
		{
			return;
		}

		float factor = ComputeFactor(currentClientSize);
		bool identidad = factor == 1f;

		int x;
		int y;
		int anchoEscalado;
		int altoEscalado;
		if (identidad)
		{
			// Factor 1.0: identidad exacta con el diseño (posición/tamaño originales).
			x = _baseContenedorLocation.X;
			y = _baseContenedorLocation.Y;
			anchoEscalado = _baseContenedorSize.Width;
			altoEscalado = _baseContenedorSize.Height;
		}
		else
		{
			// Tamaño del contenedor: SIEMPRE base x factor, nunca el estado actual.
			anchoEscalado = Math.Max(1, Redondear((float)_baseContenedorSize.Width * factor));
			altoEscalado = Math.Max(1, Redondear((float)_baseContenedorSize.Height * factor));

			// Centrado determinístico del contenedor en el área cliente disponible.
			x = Math.Max(0, (currentClientSize.Width - anchoEscalado) / 2);
			y = Math.Max(0, (currentClientSize.Height - altoEscalado) / 2);
		}

		bool cambioFactor = factor != _factorActual;
		Dictionary<(string, FontStyle, float), Font> nuevasFuentes = null;
		if (cambioFactor && !identidad)
		{
			nuevasFuentes = new Dictionary<(string, FontStyle, float), Font>();
		}

		Control padre = _contenedor.Parent;
		padre?.SuspendLayout();
		_contenedor.SuspendLayout();
		try
		{
			_contenedor.SetBounds(x, y, anchoEscalado, altoEscalado);

			foreach (Snapshot snap in _snapshots)
			{
				if (snap.Control.IsDisposed)
				{
					continue;
				}
				// Posición/tamaño: SIEMPRE snapshot x factor, nunca el estado actual.
				snap.Control.Location = new Point(Redondear(snap.Location.X * factor), Redondear(snap.Location.Y * factor));
				if (!snap.AutoSizeLabel)
				{
					snap.Control.Size = new Size(Math.Max(1, Redondear(snap.Size.Width * factor)), Math.Max(1, Redondear(snap.Size.Height * factor)));
				}
				if (nuevasFuentes != null)
				{
					snap.Control.Font = ObtenerFuenteEscalada(snap, factor, nuevasFuentes);
				}
				else if (cambioFactor)
				{
					// Factor 1.0: identidad exacta, se restaura la referencia de
					// fuente original capturada (equivalencia byte a byte).
					snap.Control.Font = snap.FontOriginal;
				}
			}

			if (cambioFactor)
			{
				// Todas las referencias ya migraron (al pool nuevo o a las originales):
				// las fuentes del pool anterior quedan sin referencias y se pueden
				// disponer con seguridad.
				if (_fuentesEscaladas != null)
				{
					foreach (Font fuenteVieja in _fuentesEscaladas.Values)
					{
						fuenteVieja.Dispose();
					}
				}
				_fuentesEscaladas = nuevasFuentes;
				_factorActual = factor;
			}
		}
		finally
		{
			_contenedor.ResumeLayout(true);
			padre?.ResumeLayout(false);
		}
	}

	public void Dispose()
	{
		if (_disposed)
		{
			return;
		}
		_disposed = true;
		// Las fuentes del pool activo NO se disponen aquí: los controles siguen
		// referenciándolas y disponerlas violaría la condición de seguridad.
		// El GC las reclamará (mismo patrón que las fuentes de InitializeComponent);
		// los pools intermedios sí se disponen determinísticamente en cada Apply.
		_fuentesEscaladas = null;
	}

	private static Font ObtenerFuenteEscalada(Snapshot snap, float factor, Dictionary<(string, FontStyle, float), Font> pool)
	{
		float tamano = (float)Math.Round((double)(snap.FontSize * factor), 2);
		var clave = (snap.FontName, snap.FontStyle, tamano);
		Font fuente;
		if (!pool.TryGetValue(clave, out Font nueva))
		{
			nueva = new Font(snap.FontName, tamano, snap.FontStyle);
			pool[clave] = nueva;
		}
		fuente = nueva;
		return fuente;
	}

	private static int Redondear(float valor)
	{
		return (int)Math.Round((double)valor, MidpointRounding.AwayFromZero);
	}

	private sealed class Snapshot
	{
		public readonly Control Control;
		public readonly PointF Location;
		public readonly SizeF Size;
		public readonly Font FontOriginal;
		public readonly string FontName;
		public readonly FontStyle FontStyle;
		public readonly float FontSize;
		public readonly bool AutoSizeLabel;

		public Snapshot(Control control, Point location, Size size, Font fontOriginal, string fontName, FontStyle fontStyle, float fontSize, bool autoSizeLabel)
		{
			Control = control;
			Location = new PointF(location.X, location.Y);
			Size = new SizeF(size.Width, size.Height);
			FontOriginal = fontOriginal;
			FontName = fontName;
			FontStyle = fontStyle;
			FontSize = fontSize;
			AutoSizeLabel = autoSizeLabel;
		}
	}
}
