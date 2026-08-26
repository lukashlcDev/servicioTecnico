using System;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

[assembly: InternalsVisibleTo("ServicioTecnico.Tests")]

namespace ServicioTecnico;

internal static class FormatoMoneda
{
	public static readonly CultureInfo Cultura = new CultureInfo("es-AR");

	private const NumberStyles EstilosParseo =
		NumberStyles.AllowDecimalPoint |
		NumberStyles.AllowLeadingSign;

	private static readonly Regex PatronMonetario = new Regex(
		@"^-?([0-9]+|[0-9]+,[0-9]+|[0-9]{1,3}(\.[0-9]{3})*,[0-9]+|[0-9]{1,3}(\.[0-9]{3})+|[0-9]+\.[0-9]{1,2})$",
		RegexOptions.Compiled | RegexOptions.CultureInvariant);

	private static readonly Regex PatronTipeo = new Regex(
		@"^[0-9]*(,[0-9]{0,2})?$",
		RegexOptions.Compiled | RegexOptions.CultureInvariant);

	private static readonly Regex PatronVisualTipeo = new Regex(
		@"^([0-9]{1,3}(\.[0-9]{3})*|[0-9]*)(,[0-9]{0,2})?$",
		RegexOptions.Compiled | RegexOptions.CultureInvariant);

	public static bool TryParseMonetario(string texto, out decimal valor)
	{
		valor = 0m;

		if (string.IsNullOrWhiteSpace(texto))
		{
			valor = 0m;
			return true;
		}

		string limpio = QuitarPresentacion(texto);

		if (string.IsNullOrEmpty(limpio))
		{
			valor = 0m;
			return true;
		}

		if (!PatronMonetario.IsMatch(limpio))
			return false;

		string normalizado = NormalizarSeparadores(limpio);
		normalizado = AsegurarDigitoAntesDeComa(normalizado);

		if (decimal.TryParse(normalizado, EstilosParseo, Cultura, out decimal temp))
		{
			valor = Redondear(temp);
			return true;
		}

		return false;
	}

	public static bool TryParseMonetarioPositivo(string texto, out decimal valor)
	{
		if (TryParseMonetario(texto, out decimal temp) && temp >= 0)
		{
			valor = temp;
			return true;
		}

		valor = 0m;
		return false;
	}

	public static decimal Redondear(decimal valor)
	{
		return decimal.Round(valor, 2, MidpointRounding.AwayFromZero);
	}

	public static string Formatear(decimal valor)
	{
		return Redondear(valor).ToString("C2", Cultura);
	}

	public static string FormatearEditable(decimal valor)
	{
		return Redondear(valor).ToString("0.00", Cultura);
	}

	public static string FormatearDesdeObjeto(object valor)
	{
		if (valor == null || valor == DBNull.Value)
			return Formatear(0m);

		try
		{
			decimal temp = Convert.ToDecimal(valor);
			return Formatear(temp);
		}
		catch
		{
			return Formatear(0m);
		}
	}

	public static string FormatearParaEdicion(decimal valor)
	{
		return Redondear(valor).ToString("#,##0.00", Cultura);
	}

	public static bool EsTextoTipeoValido(string texto)
	{
		return PatronTipeo.IsMatch(texto ?? "");
	}

	public static string AgruparEdicion(string texto)
	{
		if (string.IsNullOrEmpty(texto))
			return "";

		string soloDigitos = "";
		foreach (char c in texto)
		{
			if (c == ',')
				soloDigitos += ',';
			else if (c >= '0' && c <= '9')
				soloDigitos += c;
		}

		int coma = soloDigitos.IndexOf(',');
		string entero = (coma >= 0) ? soloDigitos.Substring(0, coma) : soloDigitos;
		string decimales = (coma >= 0) ? soloDigitos.Substring(coma + 1).Replace(",", "") : "";

		if (decimales.Length > 2)
			decimales = decimales.Substring(0, 2);

		bool teniaEntero = entero.Length > 0;
		entero = entero.TrimStart('0');
		if (entero.Length == 0 && teniaEntero)
			entero = "0";

		string agrupado = "";
		int primero = entero.Length % 3;
		if (primero > 0)
			agrupado = entero.Substring(0, primero);
		for (int i = primero; i < entero.Length; i += 3)
		{
			if (agrupado.Length > 0)
				agrupado += ".";
			agrupado += entero.Substring(i, 3);
		}

		if (coma >= 0)
			return ((agrupado.Length == 0) ? "0" : agrupado) + "," + decimales;

		return agrupado;
	}

	public static bool EsInsercionValida(string texto, int inicioSeleccion, int largoSeleccion, char caracter)
	{
		char efectivo = caracter;
		if (efectivo == '.')
			efectivo = ',';

		bool esDigito = efectivo >= '0' && efectivo <= '9';
		if (!esDigito && efectivo != ',')
			return false;

		if (texto == null)
			texto = "";
		if (inicioSeleccion < 0)
			inicioSeleccion = 0;
		if (inicioSeleccion > texto.Length)
			inicioSeleccion = texto.Length;
		if (largoSeleccion < 0)
			largoSeleccion = 0;
		if (inicioSeleccion + largoSeleccion > texto.Length)
			largoSeleccion = texto.Length - inicioSeleccion;

		string crudo = texto.Remove(inicioSeleccion, largoSeleccion).Insert(inicioSeleccion, efectivo.ToString());
		return EsTextoTipeoValido(crudo.Replace(".", ""));
	}

	public static bool EsEliminacionInseguraDeComa(string texto, int inicio, int longitud)
	{
		if (string.IsNullOrEmpty(texto))
			return false;

		if (inicio < 0)
			inicio = 0;
		if (inicio > texto.Length)
			return false;
		if (longitud < 0)
			longitud = 0;
		if (inicio + longitud > texto.Length)
			longitud = texto.Length - inicio;
		if (longitud == 0)
			return false;

		int coma = texto.IndexOf(',');
		if (coma < 0 || coma < inicio || coma >= inicio + longitud)
			return false;

		for (int i = inicio + longitud; i < texto.Length; i++)
		{
			if (texto[i] >= '0' && texto[i] <= '9')
				return true;
		}

		return false;
	}

	public static bool TryNormalizarEdicion(string texto, out string resultado)
	{
		resultado = "";
		if (texto == null)
			return true;

		texto = texto.Trim();
		if (texto.Length == 0)
			return true;

		if (PatronVisualTipeo.IsMatch(texto))
		{
			resultado = AgruparEdicion(texto);
			return true;
		}

		if (TryParseMonetarioPositivo(texto, out decimal valor))
		{
			resultado = FormatearParaEdicion(Redondear(valor));
			return true;
		}

		return false;
	}

	private static string QuitarPresentacion(string texto)
	{
		return texto.Trim()
			.Replace("$", string.Empty)
			.Replace(" ", string.Empty)
			.Replace("\t", string.Empty)
			.Replace("\u00A0", string.Empty);
	}

	private static string NormalizarSeparadores(string limpio)
	{
		if (limpio.Contains(","))
		{
			return limpio.Replace(".", string.Empty);
		}

		if (limpio.Contains("."))
		{
			string[] partes = limpio.Split('.');
			string ultima = partes[partes.Length - 1];

			if (partes.Length == 2 && ultima.Length >= 1 && ultima.Length <= 2)
				return limpio.Replace(".", ",");

			return limpio.Replace(".", string.Empty);
		}

		return limpio;
	}

	private static string AsegurarDigitoAntesDeComa(string normalizado)
	{
		if (normalizado.StartsWith(","))
			return "0" + normalizado;

		if (normalizado.StartsWith("-,"))
			return "-0" + normalizado.Substring(1);

		return normalizado;
	}
}

internal enum OrigenEdicion
{
	Ninguno,
	Teclado
}

internal sealed class MotorOrigenEdicion
{
	public OrigenEdicion Origen { get; private set; }

	public void ProcesarKeyDown(bool mutacionConfirmada)
	{
		Origen = mutacionConfirmada ? OrigenEdicion.Teclado : OrigenEdicion.Ninguno;
	}

	public void ProcesarKeyPress(char caracter, bool aceptado)
	{
		if (caracter < ' ')
			return;

		if (aceptado)
			Origen = OrigenEdicion.Teclado;
	}

	public OrigenEdicion ConsumirTextChanged()
	{
		OrigenEdicion actual = Origen;
		Origen = OrigenEdicion.Ninguno;
		return actual;
	}

	public void Reiniciar()
	{
		Origen = OrigenEdicion.Ninguno;
	}
}
