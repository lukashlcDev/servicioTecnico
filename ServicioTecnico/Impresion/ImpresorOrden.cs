using System;
using System.Drawing.Printing;
using System.Windows.Forms;

namespace ServicioTecnico;

// Contrato comun de un renderer de impresion.
// Dibuja sobre PrintPageEventArgs sin conocer frmOrdenServicio ni sus controles.
internal interface IRendererImpresion
{
	void Dibujar(PrintPageEventArgs e);
}

// Orquestador del motor de impresion (MEJ-004).
// Decide Carta/Ticket, crea el PrintDocument, configura la impresora, registra
// PrintPage y ejecuta Print(). Los errores se propagan por excepcion:
// frmOrdenServicio (cuando se conecte el flujo) es quien muestra el MsgBox.
//
// Aun NO esta conectado al flujo real de Imprimir() de frmOrdenServicio.
internal sealed class ImpresorOrden
{
	public const string TipoCarta = "carta";
	public const string TipoTicket = "ticket";

	public void Imprimir(OrdenImpresionData datos, ConfiguracionImpresion config)
	{
		IRendererImpresion renderer = SeleccionarRenderer(config.TipoImpresion, datos, config);
		using (PrintDocument printDocument = new PrintDocument())
		{
			if (!string.IsNullOrEmpty(config.Impresora))
			{
				printDocument.PrinterSettings.PrinterName = config.Impresora;
			}
			printDocument.PrintPage += (sender, e) => renderer.Dibujar(e);
			printDocument.Print();
		}
	}

	public void Previsualizar(OrdenImpresionData datos, ConfiguracionImpresion config)
	{
		IRendererImpresion renderer = SeleccionarRenderer(config.TipoImpresion, datos, config);
		using (PrintDocument printDocument = new PrintDocument())
		{
			if (!string.IsNullOrEmpty(config.Impresora))
			{
				printDocument.PrinterSettings.PrinterName = config.Impresora;
			}
			printDocument.PrintPage += (sender, e) => renderer.Dibujar(e);
			using (PrintPreviewDialog preview = new PrintPreviewDialog())
			{
				preview.Document = printDocument;
				preview.Width = 800;
				preview.Height = 600;
				preview.ShowDialog();
			}
		}
	}

	public static IRendererImpresion SeleccionarRenderer(string tipoImpresion, OrdenImpresionData datos, ConfiguracionImpresion config)
	{
		string tipo = string.IsNullOrEmpty(tipoImpresion) ? "" : tipoImpresion.Trim().ToLowerInvariant();
		if (tipo == TipoCarta)
		{
			return new CartaRenderer(datos, config);
		}
		if (tipo == TipoTicket)
		{
			return new TicketRenderer(datos, config);
		}
		throw new InvalidOperationException("El tipo de impresión configurado no es válido.");
	}
}
