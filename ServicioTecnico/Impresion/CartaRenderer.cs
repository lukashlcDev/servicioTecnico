using System;
using System.Drawing;
using System.Drawing.Printing;
using BarcodeLib;

namespace ServicioTecnico;

// Renderer de la Carta para el motor desacoplado.
// Reproduce fielmente el layout estable de ImprimirCarta() de frmOrdenServicio,
// adaptado a OrdenImpresionData + ConfiguracionImpresion y con gestión correcta de recursos GDI+.
// La proporción del barcode (162x40) replica la del barcode de Ticket para mantener
// consistencia visual entre ambas salidas.

internal sealed class CartaRenderer : IRendererImpresion
{
	public const string TipoBarcode = "CODE128";
	public const string Titulo = "ORDEN DE SERVICIO";
	public const int ANCHO_BARCODE_CARTA = 162;
	public const int ALTO_BARCODE_CARTA = 40;

	private readonly OrdenImpresionData _datos;
	private readonly ConfiguracionImpresion _config;

	public CartaRenderer(OrdenImpresionData datos, ConfiguracionImpresion config)
	{
		_datos = datos;
		_config = config;
	}

	public static string ObtenerCodigoBarras(string numeroOrden)
	{
		return numeroOrden.PadLeft(6, '0');
	}

	public void Dibujar(PrintPageEventArgs e)
	{
		// Parámetros de layout tomados del baseline ImprimirCarta().
		int margenIzq = 40;
		int margenDer = 120;
		int yInicial = 50;
		int anchoUtil = e.PageBounds.Width - margenIzq - margenDer;
		int anchoTotal = e.PageBounds.Width - margenIzq * 2;
		int caracteresPorLinea = 90;
		int altoLogo = 80;
		int anchoLogo = 100;
		bool dibujarBarcode = true;
		string tipoBarcode = TipoBarcode;

		using (Font fuenteTicket = new Font(_config.FuenteNombre, _config.TamanoFuente, FontStyle.Bold))
		using (Font font = new Font(fuenteTicket.FontFamily, fuenteTicket.Size + 1f, FontStyle.Bold))
		using (Font font2 = new Font(fuenteTicket.FontFamily, fuenteTicket.Size + 2f, FontStyle.Bold))
		using (Font font3 = new Font(fuenteTicket.FontFamily, fuenteTicket.Size + 2f, FontStyle.Bold))
		{
			int num5 = fuenteTicket.Height + 4;
			int y = yInicial;

			// Logo
			if (_datos.Logo != null)
			{
				e.Graphics.DrawImage(_datos.Logo, margenIzq, y, anchoLogo, altoLogo);
			}

			// Datos de empresa a la derecha del logo
			int xEmpresa = margenIzq + anchoLogo + 20;
			e.Graphics.DrawString((_datos.EmpresaNombre ?? "").ToUpper(), font2, Brushes.Black, xEmpresa, y);
			y += font2.Height + 5;
			e.Graphics.DrawString(_datos.EmpresaCorreo ?? "", font3, Brushes.Black, xEmpresa, y);
			y += font3.Height + 2;
			e.Graphics.DrawString(_datos.EmpresaDireccion ?? "", font3, Brushes.Black, xEmpresa, y);

			// Orden N° alineado a la derecha
			string ordenTexto = "ORDEN N°: " + ObtenerCodigoBarras(_datos.NumeroOrden);
			int anchoOrden = (int)Math.Round(e.Graphics.MeasureString(ordenTexto, font).Width);
			e.Graphics.DrawString(ordenTexto, font, Brushes.Black, margenIzq + (anchoUtil - anchoOrden), y);
			y += num5;

			// Reset de Y para continuar debajo del logo
			y = margenIzq + altoLogo + 2;

			// Banner "ORDEN DE SERVICIO"
			ImpresionDrawHelpers.DibujarTextoConFondo(e, Titulo, font, y);
			y += (int)Math.Round(e.Graphics.MeasureString(Titulo, font).Height) + 2;

			// Línea separadora
			ImpresionDrawHelpers.DibujarLinea(e, fuenteTicket, new string('_', caracteresPorLinea), margenIzq, y);
			y += num5;

			// Fila 1: Fecha / Cliente
			ImpresionDrawHelpers.DibujarLineaDosColumnas(e, fuenteTicket, "Fecha:", _datos.Fecha, "Cliente:", _datos.Cliente,
				caracteresPorLinea - margenIzq / 3, margenIzq, y);
			y += num5;

			// Fila 2: Documento / Teléfono
			ImpresionDrawHelpers.DibujarLineaDosColumnas(e, fuenteTicket, "Documento:", _datos.Documento, "Teléfono:", _datos.Telefono,
				caracteresPorLinea - margenIzq / 3, margenIzq, y);
			y += num5;

			// Dir/Correo multilínea
			if (!string.IsNullOrEmpty(_datos.Direccion))
			{
				ImpresionDrawHelpers.DibujarTextoMultilinea(e, fuenteTicket, "Dir/Correo: " + _datos.Direccion, margenIzq, y, anchoUtil);
				y += ImpresionDrawHelpers.ObtenerAlturaTexto(e, "Dir/Correo: " + _datos.Direccion, anchoUtil, fuenteTicket);
			}

			// Línea separadora
			ImpresionDrawHelpers.DibujarLinea(e, fuenteTicket, new string('_', caracteresPorLinea), margenIzq, y);
			y += num5;

			// Equipo / Marca
			ImpresionDrawHelpers.DibujarLineaDosColumnas(e, fuenteTicket, "Equipo:", _datos.TipoEquipo, "Marca:", _datos.Marca,
				caracteresPorLinea - margenIzq / 3, margenIzq, y);
			y += num5;

			// Modelo / IMEI-Serie
			ImpresionDrawHelpers.DibujarLineaDosColumnas(e, fuenteTicket, "Modelo:", _datos.Modelo, "EMEI/Serie:", _datos.IMEI,
				caracteresPorLinea - margenIzq / 3, margenIzq, y);
			y += num5;

			// Accesorios multilínea
			if (!string.IsNullOrEmpty(_datos.Accesorios))
			{
				ImpresionDrawHelpers.DibujarTextoMultilinea(e, fuenteTicket, "Accesorios: " + _datos.Accesorios, margenIzq, y, anchoUtil);
				y += ImpresionDrawHelpers.ObtenerAlturaTexto(e, "Accesorios: " + _datos.Accesorios, anchoUtil, fuenteTicket);
			}
            // y += num5; (aca se deaja espacio para la linea separadora)

            // Línea separadora
            ImpresionDrawHelpers.DibujarLinea(e, fuenteTicket, new string('_', caracteresPorLinea), margenIzq, y);
            y += num5;

            // FALLA REPORTADA
            ImpresionDrawHelpers.DibujarLinea(e, fuenteTicket, "FALLA REPORTADA:", margenIzq, y);
			y += num5;
			ImpresionDrawHelpers.DibujarTextoMultilinea(e, fuenteTicket, _datos.Falla, margenIzq, y, anchoUtil);
			y += ImpresionDrawHelpers.ObtenerAlturaTexto(e, _datos.Falla, anchoUtil, fuenteTicket) + num5;

			// OBSERVACIONES
			if (!string.IsNullOrEmpty(_datos.Observaciones))
			{
				ImpresionDrawHelpers.DibujarLinea(e, fuenteTicket, "OBSERVACIONES:", margenIzq, y);
				y += num5;
				ImpresionDrawHelpers.DibujarTextoMultilinea(e, fuenteTicket, _datos.Observaciones, margenIzq, y, anchoUtil);
				y += ImpresionDrawHelpers.ObtenerAlturaTexto(e, _datos.Observaciones, anchoUtil, fuenteTicket) + num5;
			}

			// REPARACIÓN
			if (!string.IsNullOrEmpty(_datos.Reparacion))
			{
				ImpresionDrawHelpers.DibujarLinea(e, fuenteTicket, "REPARACIÓN:", margenIzq, y);
				y += num5;
				ImpresionDrawHelpers.DibujarTextoMultilinea(e, fuenteTicket, _datos.Reparacion, margenIzq, y, anchoUtil);
				y += ImpresionDrawHelpers.ObtenerAlturaTexto(e, _datos.Reparacion, anchoUtil, fuenteTicket);
			}

			// Línea separadora
			ImpresionDrawHelpers.DibujarLinea(e, fuenteTicket, new string('_', caracteresPorLinea), margenIzq, y);
			y += num5;

			// Estado
			ImpresionDrawHelpers.DibujarTextoMultilinea(e, fuenteTicket, "Estado: " + _datos.Estado, margenIzq, y, anchoUtil);
			y += ImpresionDrawHelpers.ObtenerAlturaTexto(e, "Estado: " + _datos.Estado, anchoUtil, fuenteTicket);

			// Importes alineados a la derecha
			if (_datos.Presupuesto.HasValue && _datos.Presupuesto.Value > 0m)
			{
				ImpresionDrawHelpers.DibujarLineaAlineadaDerecha(e, fuenteTicket, "Presupuesto: " + FormatoMoneda.Formatear(_datos.Presupuesto.Value), anchoUtil, margenIzq, y);
				y += num5;
			}
			if (_datos.Abono.HasValue && _datos.Abono.Value > 0m)
			{
				ImpresionDrawHelpers.DibujarLineaAlineadaDerecha(e, fuenteTicket, "Abono: " + FormatoMoneda.Formatear(_datos.Abono.Value), anchoUtil, margenIzq, y);
				y += num5;
			}
			if (_datos.Total > 0m)
			{
				ImpresionDrawHelpers.DibujarLineaAlineadaDerecha(e, fuenteTicket, "RESTA: " + FormatoMoneda.Formatear(_datos.Total), anchoUtil, margenIzq, y);
				y += num5;
			}

			// Barcode
			if (dibujarBarcode)
			{
string codigo = ObtenerCodigoBarras(_datos.NumeroOrden);
				int altura = ALTO_BARCODE_CARTA;
				int ancho = ANCHO_BARCODE_CARTA;
				int num12 = y - num5 * 3;

				TYPE tipo = string.Equals(tipoBarcode, "CODE128", StringComparison.Ordinal) ? TYPE.CODE128 : TYPE.CODE39;
				ImpresionDrawHelpers.DibujarCodigoBarras(e, tipo, codigo, margenIzq, num12, ancho, altura, fuenteTicket);
				y += num5;
			}

			// Condiciones del servicio
			ImpresionDrawHelpers.DibujarLinea(e, fuenteTicket, _datos.Condiciones ?? "", margenIzq, y);
			y += num5;

			e.HasMorePages = false;
		}
	}
}