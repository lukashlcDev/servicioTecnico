using System;
using System.Drawing;
using System.Drawing.Printing;

namespace ServicioTecnico.Tests;

internal static class TestsImpresion
{
	public static TestResult IMP_MODELO_001()
	{
		ConfiguracionImpresion.ParseFuente("Courier New, 9", 10f, out string nombre, out float tamano);
		TestResult r = new TestResult();
		r.Ok = string.Equals(nombre, "Courier New", StringComparison.Ordinal) && tamano == 9f;
		r.Detalle = r.Ok ? "" : "Esperado nombre='Courier New' tamano=9, obtenido nombre='" + nombre + "' tamano=" + tamano;
		return r;
	}

	public static TestResult IMP_MODELO_002()
	{
		ConfiguracionImpresion.ParseFuente("Consolas,12", 10f, out string nombre, out float tamano);
		TestResult r = new TestResult();
		r.Ok = string.Equals(nombre, "Consolas", StringComparison.Ordinal) && tamano == 12f;
		r.Detalle = r.Ok ? "" : "Esperado nombre='Consolas' tamano=12, obtenido nombre='" + nombre + "' tamano=" + tamano;
		return r;
	}

	public static TestResult IMP_MODELO_003()
	{
		ConfiguracionImpresion.ParseFuente("", 10f, out string nombre, out float tamano);
		TestResult r = new TestResult();
		r.Ok = string.Equals(nombre, "Courier New", StringComparison.Ordinal) && tamano == 10f;
		r.Detalle = r.Ok ? "" : "Fallback esperado Courier New/10, obtenido nombre='" + nombre + "' tamano=" + tamano;
		return r;
	}

	public static TestResult IMP_MODELO_004()
	{
		ConfiguracionImpresion.ParseFuente("Courier New, abc", 10f, out string nombre, out float tamano);
		TestResult r = new TestResult();
		r.Ok = string.Equals(nombre, "Courier New", StringComparison.Ordinal) && tamano == 10f;
		r.Detalle = r.Ok ? "" : "Fallback esperado Courier New/10 ante tamano no numerico, obtenido nombre='" + nombre + "' tamano=" + tamano;
		return r;
	}

	public static TestResult IMP_MODELO_005()
	{
		OrdenImpresionData datos = new OrdenImpresionData
		{
			Presupuesto = 1000m,
			Abono = 300m
		};
		TestResult r = new TestResult();
		r.Ok = datos.Total == 700m;
		r.Detalle = r.Ok ? "" : "Esperado Total=700, obtenido " + datos.Total;
		return r;
	}

	public static TestResult IMP_MODELO_006()
	{
		OrdenImpresionData datos = new OrdenImpresionData
		{
			Presupuesto = 1000m,
			Abono = null
		};
		TestResult r = new TestResult();
		r.Ok = datos.Total == 0m;
		r.Detalle = r.Ok ? "" : "Esperado Total=0 al faltar abono, obtenido " + datos.Total;
		return r;
	}

	public static TestResult IMP_MODELO_007()
	{
		ConfiguracionImpresion.ParseFuente("Lucida Console", 10f, out string nombre, out float tamano);
		TestResult r = new TestResult();
		r.Ok = string.Equals(nombre, "Lucida Console", StringComparison.Ordinal) && tamano == 10f;
		r.Detalle = r.Ok ? "" : "Nombre sin tamano esperado 'Lucida Console'/10, obtenido nombre='" + nombre + "' tamano=" + tamano;
		return r;
	}

	public static TestResult IMP_ORQ_001()
	{
		OrdenImpresionData datos = new OrdenImpresionData();
		ConfiguracionImpresion config = new ConfiguracionImpresion { TipoImpresion = "carta" };
		TestResult r = new TestResult();
		try
		{
			ServicioTecnico.IRendererImpresion renderer = ServicioTecnico.ImpresorOrden.SeleccionarRenderer("carta", datos, config);
			r.Ok = renderer is ServicioTecnico.CartaRenderer;
			r.Detalle = r.Ok ? "" : "Se esperaba CartaRenderer para tipo 'carta', obtenido " + (renderer == null ? "null" : renderer.GetType().Name);
		}
		catch (Exception ex)
		{
			r.Ok = false;
			r.Detalle = "Excepcion: " + ex.Message;
		}
		return r;
	}

	public static TestResult IMP_ORQ_002()
	{
		OrdenImpresionData datos = new OrdenImpresionData();
		ConfiguracionImpresion config = new ConfiguracionImpresion { TipoImpresion = "carta" };
		TestResult r = new TestResult();
		try
		{
			ServicioTecnico.ImpresorOrden.SeleccionarRenderer("invalido", datos, config);
			r.Ok = false;
			r.Detalle = "Se esperaba excepcion para tipo de impresion invalido";
		}
		catch (InvalidOperationException)
		{
			r.Ok = true;
			r.Detalle = "";
		}
		catch (Exception ex)
		{
			r.Ok = false;
			r.Detalle = "Se esperaba InvalidOperationException, obtenida " + ex.GetType().Name;
		}
		return r;
	}

	public static TestResult IMP_CARTA_001()
	{
		TestResult r = new TestResult();
		string codigo = ServicioTecnico.CartaRenderer.ObtenerCodigoBarras("42");
		r.Ok = string.Equals(codigo, "000042", StringComparison.Ordinal);
		r.Detalle = r.Ok ? "" : "Esperado '000042', obtenido '" + codigo + "'";
		return r;
	}

	public static TestResult IMP_CARTA_002()
	{
		TestResult r = new TestResult();
		r.Ok = string.Equals(ServicioTecnico.CartaRenderer.TipoBarcode, "CODE128", StringComparison.Ordinal);
		r.Detalle = r.Ok ? "" : "Esperado TipoBarcode='CODE128', obtenido '" + ServicioTecnico.CartaRenderer.TipoBarcode + "'";
		return r;
	}

	public static TestResult IMP_ORQ_003()
	{
		OrdenImpresionData datos = new OrdenImpresionData();
		ConfiguracionImpresion config = new ConfiguracionImpresion { TipoImpresion = "ticket" };
		TestResult r = new TestResult();
		try
		{
			ServicioTecnico.IRendererImpresion renderer = ServicioTecnico.ImpresorOrden.SeleccionarRenderer("ticket", datos, config);
			r.Ok = renderer is ServicioTecnico.TicketRenderer;
			r.Detalle = r.Ok ? "" : "Se esperaba TicketRenderer para tipo 'ticket', obtenido " + (renderer == null ? "null" : renderer.GetType().Name);
		}
		catch (Exception ex)
		{
			r.Ok = false;
			r.Detalle = "Excepcion: " + ex.Message;
		}
		return r;
	}

	public static TestResult IMP_TICKET_001()
	{
		TestResult r = new TestResult();
		string codigo = ServicioTecnico.TicketRenderer.ObtenerCodigoBarras("7");
		r.Ok = string.Equals(codigo, "000007", StringComparison.Ordinal);
		r.Detalle = r.Ok ? "" : "Esperado '000007', obtenido '" + codigo + "'";
		return r;
	}

	public static TestResult IMP_TICKET_002()
	{
		TestResult r = new TestResult();
		bool okBarcode = string.Equals(ServicioTecnico.TicketRenderer.TipoBarcode, "CODE128", StringComparison.Ordinal);
		bool okTitulo = string.Equals(ServicioTecnico.TicketRenderer.Titulo, "ORDEN DE SERVICIO", StringComparison.Ordinal);
		r.Ok = okBarcode && okTitulo;
		r.Detalle = r.Ok ? "" : "Esperado TipoBarcode='CODE128' y Titulo='ORDEN DE SERVICIO'";
		return r;
	}

	// Ejercita el renderizado completo de Carta y Ticket sobre un Bitmap en memoria,
	// repitiendo la creacion/destruccion de recursos (Fonts, Barcode, Image) para
	// detectar dobles Dispose, leaks logicos o excepciones de dibujo.
	// No requiere impresora ni reflection.
	public static TestResult IMP_RENDER_001()
	{
		return RenderBucleTipos("carta", 3);
	}

	public static TestResult IMP_RENDER_002()
	{
		return RenderBucleTipos("ticket", 3);
	}

	private static TestResult RenderBucleTipos(string tipo, int iteraciones)
	{
		string dir = TempDatabase.CrearDirectorio();
		Bitmap papel = null;
		Graphics g = null;
		Bitmap logo = null;
		try
		{
			logo = new Bitmap(60, 40);
			OrdenImpresionData datos = new OrdenImpresionData
			{
				NumeroOrden = "7",
				Fecha = "31/08/2026",
				Cliente = "Cliente de prueba",
				Documento = "30111222",
				Telefono = "41112222",
				Direccion = "Calle Falsa 123",
				TipoEquipo = "Laptop",
				Marca = "MarcaX",
				Modelo = "ModeloY",
				IMEI = "123456789012345",
				Accesorios = "Cargador",
				Falla = "No enciende",
				Observaciones = "Reparar bateria",
				Reparacion = "Cambio de bateria",
				Estado = "En proceso",
				FechaReparado = "01/09/2026",
				FechaEntregado = "",
				Condiciones = "1. Condicion uno.\r\n2. Condicion dos.",
				EmpresaNombre = "Hlc Informatica",
				EmpresaCorreo = "correo@test.com",
				EmpresaDireccion = "Sarmiento 1234",
				Presupuesto = 1000m,
				Abono = 300m,
				Logo = logo
			};
			ConfiguracionImpresion config = new ConfiguracionImpresion { FuenteNombre = "Courier New", TamanoFuente = 9f, TipoImpresion = tipo };

			for (int i = 0; i < iteraciones; i++)
			{
				papel?.Dispose();
				if (g != null) { g.Dispose(); g = null; }
				papel = new Bitmap(700, 900);
				g = Graphics.FromImage(papel);
				PrintPageEventArgs e = new PrintPageEventArgs(g, new Rectangle(0, 0, 700, 900), new Rectangle(0, 0, 700, 900), new PageSettings());
				ServicioTecnico.IRendererImpresion renderer = ServicioTecnico.ImpresorOrden.SeleccionarRenderer(tipo, datos, config);
				renderer.Dibujar(e);
				if (e.HasMorePages)
				{
					return Fail("Se esperaba HasMorePages=false para " + tipo);
				}
			}

			TestResult r = new TestResult();
			r.Ok = true;
			r.Detalle = "";
			return r;
		}
		catch (Exception ex)
		{
			return Fail("Render " + tipo + ": excepcion: " + ex.Message);
		}
		finally
		{
			if (g != null) { g.Dispose(); }
			papel?.Dispose();
			logo?.Dispose();
			TempDatabase.Eliminar(dir);
		}
	}

	private static TestResult Fail(string detalle)
	{
		TestResult r = new TestResult();
		r.Ok = false;
		r.Detalle = detalle;
		return r;
	}
}
