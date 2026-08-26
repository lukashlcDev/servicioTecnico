using System;
using System.Globalization;

namespace ServicioTecnico.Tests;

internal static class TestsFormatoMoneda
{
	public static TestResult FUN_FMT_001()
	{
		TestResult testResult = new TestResult();
		bool flag = true;
		flag &= Probar("1500", 1500m, "$ 1.500,00");
		flag &= Probar("1500,5", 1500.50m, "$ 1.500,50");
		flag &= Probar("1500,50", 1500.50m, "$ 1.500,50");
		flag &= Probar("1500.50", 1500.50m, "$ 1.500,50");
		flag &= Probar("1.500", 1500m, "$ 1.500,00");
		flag &= Probar("1.50", 1.50m, "$ 1,50");
		flag &= Probar("1.500,50", 1500.50m, "$ 1.500,50");
		flag &= Probar("1.234.567,89", 1234567.89m, "$ 1.234.567,89");
		flag &= Probar("$ 1.500,50", 1500.50m, "$ 1.500,50");
		flag &= Probar("0", 0m, "$ 0,00");
		flag &= Probar("", 0m, "$ 0,00");
		flag &= Probar("1500,555", 1500.56m, "$ 1.500,56");
		flag &= Probar("1500,554", 1500.55m, "$ 1.500,55");
		flag &= Probar("-100", -100m, "-$ 100,00");
		testResult.Ok = flag;
		if (!flag)
		{
			testResult.Detalle = "Algunos casos de parseo/formateo no coinciden";
		}
		return testResult;
	}

	public static TestResult FUN_FMT_002()
	{
		TestResult testResult = new TestResult();
		bool flag = true;
		flag &= !FormatoMoneda.TryParseMonetario("abc", out var _);
		flag &= !FormatoMoneda.TryParseMonetario("abc1500", out var _);
		flag &= !FormatoMoneda.TryParseMonetario("15x00", out var _);
		flag &= !FormatoMoneda.TryParseMonetario("1x500", out var _);
		flag &= !FormatoMoneda.TryParseMonetario("1500x50", out var _);
		flag &= !FormatoMoneda.TryParseMonetario("1a234,50", out var _);
		flag &= !FormatoMoneda.TryParseMonetario("1500abc", out var _);
		flag &= !FormatoMoneda.TryParseMonetario("1,2,3", out var _);
		flag &= !FormatoMoneda.TryParseMonetario("1.2.3,50", out var _);
		flag &= !FormatoMoneda.TryParseMonetario("--100", out var _);
		testResult.Ok = flag;
		if (!flag)
		{
			testResult.Detalle = "Algunos casos inválidos fueron aceptados";
		}
		return testResult;
	}

	public static TestResult FUN_FMT_003()
	{
		TestResult testResult = new TestResult();
		bool flag = true;
		flag &= !FormatoMoneda.TryParseMonetarioPositivo("-100", out var _);
		flag &= FormatoMoneda.TryParseMonetarioPositivo("1500,50", out var valor) && valor == 1500.50m;
		flag &= FormatoMoneda.TryParseMonetarioPositivo("", out var valor2) && valor2 == 0m;
		testResult.Ok = flag;
		if (!flag)
		{
			testResult.Detalle = "Validación de positivos incorrecta";
		}
		return testResult;
	}

	public static TestResult FUN_FMT_004()
	{
		TestResult testResult = new TestResult();
		bool flag = true;
		double valor = 1500.5500000000002;
		string text = FormatoMoneda.FormatearDesdeObjeto(valor);
		flag &= text == "$ 1.500,55";
		flag &= FormatoMoneda.FormatearDesdeObjeto(DBNull.Value) == "$ 0,00";
		flag &= FormatoMoneda.FormatearDesdeObjeto(null) == "$ 0,00";
		testResult.Ok = flag;
		if (!flag)
		{
			testResult.Detalle = "Formateo desde objeto incorrecto";
		}
		return testResult;
	}

	private static bool Probar(string entrada, decimal esperado, string formatoEsperado)
	{
		bool flag = FormatoMoneda.TryParseMonetario(entrada, out var valor);
		if (!flag || valor != esperado)
		{
			Console.WriteLine("  Parseo fallido: '" + entrada + "' -> esperado " + esperado + ", obtenido " + valor + ", ok=" + flag);
			return false;
		}
		string text = FormatoMoneda.Formatear(valor);
		if (text != formatoEsperado)
		{
			Console.WriteLine("  Formateo fallido: '" + entrada + "' -> esperado '" + formatoEsperado + "', obtenido '" + text + "'");
			return false;
		}
		return true;
	}

	public static TestResult FUN_FMT_005()
	{
		TestResult testResult = new TestResult();
		bool flag = true;
		flag &= FormatoMoneda.AgruparEdicion("2") == "2";
		flag &= FormatoMoneda.AgruparEdicion("20") == "20";
		flag &= FormatoMoneda.AgruparEdicion("200") == "200";
		flag &= FormatoMoneda.AgruparEdicion("2000") == "2.000";
		flag &= FormatoMoneda.AgruparEdicion("20000") == "20.000";
		flag &= FormatoMoneda.AgruparEdicion("200000") == "200.000";
		flag &= FormatoMoneda.AgruparEdicion("2000,") == "2.000,";
		flag &= FormatoMoneda.AgruparEdicion("2000,5") == "2.000,5";
		flag &= FormatoMoneda.AgruparEdicion("2000,50") == "2.000,50";
		flag &= FormatoMoneda.AgruparEdicion("") == "";
		flag &= FormatoMoneda.AgruparEdicion(",") == "0,";
		flag &= FormatoMoneda.AgruparEdicion("005") == "5";
		flag &= FormatoMoneda.AgruparEdicion("0") == "0";
		flag &= FormatoMoneda.AgruparEdicion("12.345,67") == "12.345,67";
		flag &= FormatoMoneda.EsInsercionValida("2.000", 5, 0, '5');
		flag &= FormatoMoneda.EsInsercionValida("2.000,5", 7, 0, '0');
		flag &= !FormatoMoneda.EsInsercionValida("2.000,50", 8, 0, '1');
		flag &= !FormatoMoneda.EsInsercionValida("2.000,50", 8, 0, ',');
		flag &= FormatoMoneda.EsInsercionValida("2.000", 5, 0, '.');
		flag &= FormatoMoneda.EsInsercionValida("", 0, 0, ',');
		flag &= !FormatoMoneda.EsInsercionValida("2.000", 0, 0, 'a');
		flag &= !FormatoMoneda.EsInsercionValida("2.000", 0, 0, '-');
		flag &= !FormatoMoneda.EsInsercionValida("2.000", 0, 0, '$');
		flag &= FormatoMoneda.EsInsercionValida("12.345,67", 2, 0, '9');
		flag &= !FormatoMoneda.EsInsercionValida("12.345,67", 8, 0, '9');
		flag &= FormatoMoneda.EsEliminacionInseguraDeComa("12.345,67", 6, 1);
		flag &= !FormatoMoneda.EsEliminacionInseguraDeComa("12.345,67", 5, 1);
		flag &= !FormatoMoneda.EsEliminacionInseguraDeComa("12.345,", 6, 1);
		flag &= !FormatoMoneda.EsEliminacionInseguraDeComa("12.345,67", 6, 3);
		flag &= FormatoMoneda.EsEliminacionInseguraDeComa("12.345,67", 6, 2);
		flag &= !FormatoMoneda.EsEliminacionInseguraDeComa("12.345,67", 2, 1);
		flag &= FormatoMoneda.TryNormalizarEdicion("2000.50", out var edicion1) && edicion1 == "2.000,50";
		flag &= FormatoMoneda.TryNormalizarEdicion("2.000,50", out var edicion2) && edicion2 == "2.000,50";
		flag &= FormatoMoneda.TryNormalizarEdicion("$ 2.000,50", out var edicion3) && edicion3 == "2.000,50";
		flag &= FormatoMoneda.TryNormalizarEdicion("1.234.567,89", out var edicion4) && edicion4 == "1.234.567,89";
		flag &= FormatoMoneda.TryNormalizarEdicion("2000,501", out var edicion5) && edicion5 == "2.000,50";
		flag &= FormatoMoneda.TryNormalizarEdicion("2000", out var edicion6) && edicion6 == "2.000";
		flag &= FormatoMoneda.TryNormalizarEdicion("", out var edicion7) && edicion7 == "";
		flag &= FormatoMoneda.TryNormalizarEdicion("12.345", out var edicion8) && edicion8 == "12.345";
		flag &= !FormatoMoneda.TryNormalizarEdicion("abc", out var _);
		flag &= !FormatoMoneda.TryNormalizarEdicion("abc1500", out var _);
		flag &= !FormatoMoneda.TryNormalizarEdicion("15x00", out var _);
		flag &= !FormatoMoneda.TryNormalizarEdicion("-100", out var _);
		flag &= !FormatoMoneda.TryNormalizarEdicion("1,2,3", out var _);
		flag &= !FormatoMoneda.TryNormalizarEdicion("--100", out var _);
		testResult.Ok = flag;
		if (!flag)
		{
			testResult.Detalle = "Edición dinámica: agrupación/inserción/eliminación/normalización incorrectas";
		}
		return testResult;
	}

	public static TestResult FUN_FMT_006()
	{
		TestResult testResult = new TestResult();
		bool flag = true;
		flag &= FormatoMoneda.FormatearParaEdicion(2000m) == "2.000,00";
		flag &= FormatoMoneda.FormatearParaEdicion(2000.5m) == "2.000,50";
		flag &= FormatoMoneda.FormatearParaEdicion(0m) == "0,00";
		flag &= FormatoMoneda.FormatearParaEdicion(1234567.891m) == "1.234.567,89";
		flag &= FormatoMoneda.FormatearParaEdicion(0.005m) == "0,01";
		flag &= FormatoMoneda.FormatearParaEdicion(12345m) == "12.345,00";
		testResult.Ok = flag;
		if (!flag)
		{
			testResult.Detalle = "Formateo para edición incorrecto";
		}
		return testResult;
	}

	public static TestResult FUN_FMT_007()
	{
		TestResult testResult = new TestResult();
		bool flag = true;
		MotorOrigenEdicion motor = new MotorOrigenEdicion();
		motor.ProcesarKeyDown(true);
		motor.ProcesarKeyPress('\b', aceptado: false);
		flag &= motor.Origen == OrigenEdicion.Teclado;
		flag &= motor.ConsumirTextChanged() == OrigenEdicion.Teclado;
		flag &= motor.Origen == OrigenEdicion.Ninguno;
		motor.ProcesarKeyDown(true);
		flag &= motor.ConsumirTextChanged() == OrigenEdicion.Teclado;
		motor.ProcesarKeyDown(false);
		motor.ProcesarKeyPress('\x16', aceptado: false);
		flag &= motor.ConsumirTextChanged() == OrigenEdicion.Ninguno;
		motor.ProcesarKeyDown(false);
		motor.ProcesarKeyPress('\x1a', aceptado: false);
		flag &= motor.ConsumirTextChanged() == OrigenEdicion.Ninguno;
		motor.ProcesarKeyDown(true);
		motor.ProcesarKeyPress('\x18', aceptado: false);
		flag &= motor.ConsumirTextChanged() == OrigenEdicion.Teclado;
		motor.ProcesarKeyDown(false);
		flag &= motor.Origen == OrigenEdicion.Ninguno;
		motor.ProcesarKeyDown(false);
		flag &= motor.ConsumirTextChanged() == OrigenEdicion.Ninguno;
		motor.ProcesarKeyDown(true);
		motor.ProcesarKeyDown(false);
		flag &= motor.Origen == OrigenEdicion.Ninguno;
		motor.ProcesarKeyDown(false);
		motor.ProcesarKeyPress('5', aceptado: true);
		flag &= motor.ConsumirTextChanged() == OrigenEdicion.Teclado;
		motor.ProcesarKeyDown(true);
		motor.Reiniciar();
		flag &= motor.Origen == OrigenEdicion.Ninguno;
		testResult.Ok = flag;
		if (!flag)
		{
			testResult.Detalle = "Máquina de origen de edición con comportamiento incorrecto";
		}
		return testResult;
	}
}
