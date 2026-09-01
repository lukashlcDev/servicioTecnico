using System.Drawing;

namespace ServicioTecnico;

// Modelo de datos exclusivamente necesario para imprimir una orden de servicio.
// Se construye a partir de los controles de frmOrdenServicio (no reconsulta la BD)
// para conservar la semantica actual de ReImprimir / imprimir al guardar.
//
// Aun NO se utiliza: la impresion actual sigue leyendo directamente los controles.
// Esta clase es la base de la separacion datos/layout prevista en MEJ-004.
public sealed class OrdenImpresionData
{
	public string NumeroOrden { get; set; }
	public string Fecha { get; set; }
	public string Cliente { get; set; }
	public string Documento { get; set; }
	public string Telefono { get; set; }
	public string Direccion { get; set; }
	public string TipoEquipo { get; set; }
	public string Marca { get; set; }
	public string Modelo { get; set; }
	public string IMEI { get; set; }
	public string Accesorios { get; set; }
	public string Falla { get; set; }
	public string Observaciones { get; set; }
	public string Reparacion { get; set; }
	public string Estado { get; set; }
	public string FechaReparado { get; set; }
	public string FechaEntregado { get; set; }
	public string Condiciones { get; set; }
	public string EmpresaNombre { get; set; }
	public string EmpresaCorreo { get; set; }
	public string EmpresaDireccion { get; set; }

	// null = no informado / sin parsear (no se imprime en ese caso).
	public decimal? Presupuesto { get; set; }
	public decimal? Abono { get; set; }

	// Imagen del logo. NO se dispone aqui: pertenece al formulario/resources.
	public Image Logo { get; set; }

	// Total derivado = Presupuesto - Abono, replicando la semantica de calcula()
	// de frmOrdenServicio (txtTotal se mantiene en sync via TextChanged).
	// Solo es distinto de 0 cuando ambos montos estan presentes.
	public decimal Total
	{
		get
		{
			if (Presupuesto.HasValue && Abono.HasValue)
			{
				return Presupuesto.Value - Abono.Value;
			}
			return 0m;
		}
	}
}
