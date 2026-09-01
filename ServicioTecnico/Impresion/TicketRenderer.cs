using System;
using System.Drawing;
using System.Drawing.Printing;
using BarcodeLib;

namespace ServicioTecnico;

// Renderer del ticket, desacoplado de frmOrdenServicio.
// Recibe OrdenImpresionData + ConfiguracionImpresion y dibuja sobre
// PrintPageEventArgs reproduciendo fielmente ImprimirTicket().
// NO accede a controles WinForms del formulario.
internal sealed class TicketRenderer : IRendererImpresion
{
	public const string TipoBarcode = "CODE128";
	public const string Titulo = "ORDEN DE SERVICIO";
	public const string DirCorreoPrefijo = "Dir/Correo:";

	private readonly OrdenImpresionData _datos;
	private readonly ConfiguracionImpresion _config;

	public TicketRenderer(OrdenImpresionData datos, ConfiguracionImpresion config)
	{
		_datos = datos;
		_config = config;
	}

	// Numero de la orden usado en el barcode y linea "Orden:" a 6 digitos.
	public static string ObtenerCodigoBarras(string numeroOrden)
	{
		return numeroOrden.PadLeft(6, '0');
	}

	public void Dibujar(PrintPageEventArgs e)
	{
		int num = 24;
		bool flag = true;
		string left = TipoBarcode;

		Font fuente = new Font(_config.FuenteNombre, _config.TamanoFuente, FontStyle.Bold);
		Font fuenteHeader = new Font(fuente.FontFamily, 8f, FontStyle.Bold);
		try
		{
			int num2 = 1;
			int num3 = 0;
			int num4 = fuente.Height + 1;
			int num5 = (int)Math.Round((double)e.PageBounds.Width / 2.5);
			int num6 = (int)Math.Round(e.Graphics.MeasureString(new string('W', num), fuente).Width);
			if (_datos.Logo != null)
			{
				double num7 = (double)_datos.Logo.Width / (double)_datos.Logo.Height;
				int num8 = (int)Math.Round((double)num6 * 0.5);
				int num9 = (int)Math.Round((double)num8 * 1.4);
				int num10 = (int)Math.Round((double)num9 / num7);
				int num11 = num5 - num9 / 2;
				e.Graphics.DrawImage(_datos.Logo, num11, num3, num9, num10);
				num3 += num10 + num4;
			}
			ImpresionDrawHelpers.CentrarTexto(e, _datos.EmpresaNombre?.ToUpper() ?? "", fuenteHeader, num3);
			num3 += num4;
			ImpresionDrawHelpers.CentrarTexto(e, _datos.EmpresaCorreo ?? "", fuenteHeader, num3);
			num3 += num4;
			ImpresionDrawHelpers.CentrarTexto(e, _datos.EmpresaDireccion ?? "", fuenteHeader, num3);
			num3 += num4;
			ImpresionDrawHelpers.CentrarTexto(e, Titulo, fuenteHeader, num3);
			num3 += num4;
			ImpresionDrawHelpers.DibujarLinea(e, fuente, new string('-', num), num2, num3);
			num3 += num4;
			ImpresionDrawHelpers.DibujarLineaDosColumnas(e, fuente, "Orden:", ObtenerCodigoBarras(_datos.NumeroOrden), " ", " ", num, num2, num3);
			num3 += num4;
			ImpresionDrawHelpers.DibujarLineaDosColumnas(e, fuente, "Fecha:", _datos.Fecha, " ", " ", num, num2, num3);
			num3 += num4;
			ImpresionDrawHelpers.DibujarLineaDosColumnas(e, fuente, "Documento:", _datos.Documento, " ", " ", num, num2, num3);
			num3 += num4;
			ImpresionDrawHelpers.DibujarLineaDosColumnas(e, fuente, "Teléfono:", _datos.Telefono, " ", " ", num, num2, num3);
			num3 += num4;
			ImpresionDrawHelpers.DibujarTextoMultilinea(e, fuente, DirCorreoPrefijo + _datos.Direccion, num2, num3, num6);
			num3 += ImpresionDrawHelpers.ObtenerAlturaTexto(e, DirCorreoPrefijo + _datos.Direccion, num6, fuente);
			ImpresionDrawHelpers.DibujarTextoMultilinea(e, fuente, "Cliente: " + _datos.Cliente, num2, num3, num6);
			num3 += ImpresionDrawHelpers.ObtenerAlturaTexto(e, "Cliente: " + _datos.Cliente, num6, fuente);
			ImpresionDrawHelpers.DibujarLinea(e, fuente, new string('-', num), num2, num3);
			num3 += num4;
			ImpresionDrawHelpers.DibujarTextoMultilinea(e, fuente, "Equipo: " + _datos.TipoEquipo, num2, num3, num6);
			num3 += ImpresionDrawHelpers.ObtenerAlturaTexto(e, "Equipo: " + _datos.TipoEquipo, num6, fuente);
			if (!string.IsNullOrEmpty(_datos.Marca))
			{
				ImpresionDrawHelpers.DibujarTextoMultilinea(e, fuente, "Marca: " + _datos.Marca, num2, num3, num6);
				num3 += ImpresionDrawHelpers.ObtenerAlturaTexto(e, "Marca: " + _datos.Marca, num6, fuente);
				if (!string.IsNullOrEmpty(_datos.Modelo))
				{
					ImpresionDrawHelpers.DibujarTextoMultilinea(e, fuente, "Modelo: " + _datos.Modelo, num2, num3, num6);
					num3 += ImpresionDrawHelpers.ObtenerAlturaTexto(e, "Modelo: " + _datos.Modelo, num6, fuente);
				}
			}
			if (!string.IsNullOrEmpty(_datos.IMEI))
			{
				ImpresionDrawHelpers.DibujarTextoMultilinea(e, fuente, "IMEI/Serie: " + _datos.IMEI, num2, num3, num6);
				num3 += ImpresionDrawHelpers.ObtenerAlturaTexto(e, "IMEI/Serie: " + _datos.IMEI, num6, fuente);
			}
			if (!string.IsNullOrEmpty(_datos.Accesorios))
			{
				ImpresionDrawHelpers.DibujarTextoMultilinea(e, fuente, "Accesorios: " + _datos.Accesorios, num2, num3, num6);
				num3 += ImpresionDrawHelpers.ObtenerAlturaTexto(e, "Accesorios: " + _datos.Accesorios, num6, fuente);
			}
			ImpresionDrawHelpers.DibujarLinea(e, fuente, new string('-', num), num2, num3);
			num3 += num4;
			ImpresionDrawHelpers.DibujarLinea(e, fuente, "FALLA REPORTADA:", num2, num3);
			num3 += num4;
			ImpresionDrawHelpers.DibujarTextoMultilinea(e, fuente, _datos.Falla, num2, num3, num6);
			num3 += ImpresionDrawHelpers.ObtenerAlturaTexto(e, _datos.Falla, num6, fuente) + num4;
			if (!string.IsNullOrEmpty(_datos.Observaciones))
			{
				ImpresionDrawHelpers.DibujarLinea(e, fuente, "OBSERVACIONES:", num2, num3);
				num3 += num4;
				ImpresionDrawHelpers.DibujarTextoMultilinea(e, fuente, _datos.Observaciones, num2, num3, num6);
				num3 += ImpresionDrawHelpers.ObtenerAlturaTexto(e, _datos.Observaciones, num6, fuente) + num4;
			}
			if (!string.IsNullOrEmpty(_datos.Reparacion))
			{
				ImpresionDrawHelpers.DibujarLinea(e, fuente, "REPARACIÓN:", num2, num3);
				num3 += num4;
				ImpresionDrawHelpers.DibujarTextoMultilinea(e, fuente, _datos.Reparacion, num2, num3, num6);
				num3 += ImpresionDrawHelpers.ObtenerAlturaTexto(e, _datos.Reparacion, num6, fuente) + num4;
			}
			ImpresionDrawHelpers.DibujarLinea(e, fuente, new string('-', num), num2, num3);
			num3 += num4;
			ImpresionDrawHelpers.DibujarTextoMultilinea(e, fuente, "Estado: " + _datos.Estado, num2, num3, num6);
			num3 += ImpresionDrawHelpers.ObtenerAlturaTexto(e, "Estado: " + _datos.Estado, num6, fuente);
			if (_datos.Abono.HasValue && _datos.Abono.Value > 0m)
			{
				ImpresionDrawHelpers.DibujarLineaAlineadaDerecha(e, fuente, "Abono: " + FormatoMoneda.Formatear(_datos.Abono.Value), num6, num2, num3);
				num3 += num4;
			}
			if (_datos.Presupuesto.HasValue && _datos.Presupuesto.Value > 0m)
			{
				ImpresionDrawHelpers.DibujarLineaAlineadaDerecha(e, fuente, "Presupuesto: " + FormatoMoneda.Formatear(_datos.Presupuesto.Value), num6, num2, num3);
				num3 += num4;
			}
			if (_datos.Total > 0m)
			{
				ImpresionDrawHelpers.DibujarLineaAlineadaDerecha(e, fuente, "TOTAL: " + FormatoMoneda.Formatear(_datos.Total), num6, num2, num3);
				num3 += num4;
			}
			ImpresionDrawHelpers.DibujarLinea(e, fuente, new string('-', num), num2, num3);
			num3 += num4;
			if (!string.IsNullOrEmpty(_datos.FechaReparado) && !string.IsNullOrEmpty(_datos.FechaEntregado) && _datos.FechaReparado != _datos.FechaEntregado)
			{
				ImpresionDrawHelpers.DibujarLinea(e, fuente, "Reparado: " + _datos.FechaReparado, num2, num3);
				num3 += num4;
				ImpresionDrawHelpers.DibujarLinea(e, fuente, "Entregado: " + _datos.FechaEntregado, num2, num3);
				num3 += num4;
			}
			ImpresionDrawHelpers.DibujarLinea(e, fuente, new string('-', num), num2, num3);
			num3 += num4;
			if (flag)
			{
				string codigo = ObtenerCodigoBarras(_datos.NumeroOrden);
				int num12 = 40;
				if (left == "CODE128")
				{
					ImpresionDrawHelpers.DibujarCodigoBarras(e, TYPE.CODE128, codigo, 1, num3, num6 - 20, num12, fuente);
				}
				else
				{
					ImpresionDrawHelpers.DibujarCodigoBarras(e, TYPE.CODE39, codigo, 1, num3, num6 - 20, num12, fuente);
				}
				num3 += num12 + num4;
			}
			ImpresionDrawHelpers.DibujarLinea(e, fuente, _datos.Condiciones ?? "", num2, num3);
			num3 += num4;
			e.HasMorePages = false;
		}
		finally
		{
			fuenteHeader.Dispose();
			fuente.Dispose();
		}
	}
}
