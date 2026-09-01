using System;
using System.Drawing;
using System.Drawing.Printing;
using BarcodeLib;

namespace ServicioTecnico;

// Helpers comunes de dibujo sobre Graphics/PrintPageEventArgs, extraidos de
// frmOrdenServicio (MEJ-004). Reproducen exactamente el comportamiento actual.
// Los helpers que en el codigo original usaban el campo compartido 'fuenteTicket'
// ahora reciben la fuente base como parametro, ya que cada renderer la gestiona.
internal static class ImpresionDrawHelpers
{
	public static void DibujarTextoConFondo(PrintPageEventArgs e, string texto, Font fuente, int y, int padding = 10)
	{
		SizeF sizeF = e.Graphics.MeasureString(texto, fuente);
		int num = (int)Math.Round(sizeF.Width) + padding * 2;
		int num2 = (int)Math.Round(sizeF.Height) + padding / 2;
		int num3 = (e.PageBounds.Width - num) / 2;
		e.Graphics.FillRectangle(Brushes.Black, num3, y, num, num2);
		e.Graphics.DrawString(texto, fuente, Brushes.White, num3 + padding, y + padding / 4);
	}

	public static void DibujarLineaDosColumnas(PrintPageEventArgs e, Font fuenteBase, string texto1, string valor1, string texto2, string valor2, int caracteresPorLinea, int x, int y)
	{
		string s = texto1 + " " + valor1;
		string text = texto2 + " " + valor2;
		int num = (int)Math.Round(e.Graphics.MeasureString(s, fuenteBase).Width);
		int num2 = (int)Math.Round((float)caracteresPorLinea * fuenteBase.Size - (float)num - 25f);
		if (e.Graphics.MeasureString(text, fuenteBase).Width > (float)num2)
		{
			text = text.Substring(0, Math.Max(0, text.Length - 3)) + "..";
		}
		e.Graphics.DrawString(s, fuenteBase, Brushes.Black, x, y);
		e.Graphics.DrawString(text, fuenteBase, Brushes.Black, (float)x + (float)caracteresPorLinea * fuenteBase.Size - e.Graphics.MeasureString(text, fuenteBase).Width, y);
	}

	public static void DibujarLineaAlineadaDerecha(PrintPageEventArgs e, Font fuenteBase, string texto, int anchoMaximo, int x, int y)
	{
		int num = x + anchoMaximo - (int)Math.Round(e.Graphics.MeasureString(texto, fuenteBase).Width);
		e.Graphics.DrawString(texto, fuenteBase, Brushes.Black, num, y);
	}

	public static void DibujarLinea(PrintPageEventArgs e, Font fuenteBase, string texto, int x, int y)
	{
		e.Graphics.DrawString(texto, fuenteBase, Brushes.Black, x, y);
	}

	public static void CentrarTexto(PrintPageEventArgs e, string texto, Font fuente, int y)
	{
		int num = (int)Math.Round(e.Graphics.MeasureString(texto, fuente).Width);
		int num2 = (e.PageBounds.Width - num) / 2;
		e.Graphics.DrawString(texto, fuente, Brushes.Black, num2 - 20, y);
	}

	public static void DibujarTextoMultilinea(PrintPageEventArgs e, Font fuenteBase, string texto, int x, int y, int anchoMaximo)
	{
		RectangleF layoutRectangle = new RectangleF(x, y, anchoMaximo, e.PageBounds.Height - y);
		e.Graphics.DrawString(texto, fuenteBase, Brushes.Black, layoutRectangle);
	}

	public static int ObtenerAlturaTexto(PrintPageEventArgs e, string texto, int anchoMaximo, Font fuente)
	{
		return (int)Math.Round(e.Graphics.MeasureString(texto, fuente, anchoMaximo).Height);
	}

	// Unico punto de integracion con BarcodeLib para el motor nuevo.
	// Reproduce el comportamiento de DibujarCodigoBarras128/39 de frmOrdenServicio
	// (IncludeLabel, centro, etiqueta, Encode x2 + DrawImage) y dispone sus recursos.
	public static void DibujarCodigoBarras(PrintPageEventArgs e, TYPE tipo, string codigo, int x, int y, int ancho, int altura, Font fuenteEtiqueta)
	{
		using (Barcode barcode = new Barcode())
		{
			barcode.IncludeLabel = true;
			barcode.Alignment = AlignmentPositions.CENTER;
			barcode.LabelFont = fuenteEtiqueta;
			barcode.Width = ancho;
			barcode.Height = altura;
			barcode.Encode(tipo, codigo);
			using (Image image = barcode.Encode(tipo, codigo, Color.Black, Color.White, ancho, altura))
			{
				e.Graphics.DrawImage(image, x, y);
			}
		}
	}
}
