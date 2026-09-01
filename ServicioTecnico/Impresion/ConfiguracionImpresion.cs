using System.Globalization;

namespace ServicioTecnico;

// Configuracion de impresion extraida de la tabla 'configuracion'.
// Contiene solo los datos realmente usados por el motor de impresion.
public sealed class ConfiguracionImpresion
{
	// Impresora configurada (vacia = usar la impresora por defecto).
	public string Impresora { get; set; }

	// "carta" | "ticket". Cualquier otro valor se considera invalido.
	public string TipoImpresion { get; set; }

	// Nombre de la fuente base del ticket (ej. "Courier New").
	public string FuenteNombre { get; set; }

	// Tamano base de la fuente. Los renderers derivan sizes +1/+2 para titulos.
	public float TamanoFuente { get; set; }

	// Parsea el valor de 'fuente_ticket' con formato "NombreFuente, tamano".
	// Si el texto es vacio o invalido, aplica el fallback Courier New + tamanoPorDefecto.
	// Devuelve true si el tamano provino del texto, false si se aplico el fallback.
	public static bool ParseFuente(string textoFuente, float tamanoPorDefecto, out string nombre, out float tamano)
	{
		nombre = "Courier New";
		tamano = tamanoPorDefecto;
		if (string.IsNullOrWhiteSpace(textoFuente))
		{
			return false;
		}
		try
		{
			string[] partes = textoFuente.Split(',');
			string familyName = partes[0].Trim();
			float emSize = (partes.Length > 1) ? float.Parse(partes[1].Trim(), CultureInfo.InvariantCulture) : tamanoPorDefecto;
			nombre = familyName;
			tamano = emSize;
			return partes.Length > 1;
		}
		catch
		{
			nombre = "Courier New";
			tamano = tamanoPorDefecto;
			return false;
		}
	}
}
