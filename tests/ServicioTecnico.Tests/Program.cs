using System;
using System.Collections.Generic;
using System.IO;

namespace ServicioTecnico.Tests;

internal static class Program
{
	private static int Main()
	{
		Console.WriteLine("Host de pruebas ServicioTecnico.Tests");
		Console.WriteLine("Casos: FUN-041..044, FUN-FMT-001..007, HELPER-001..004, QUICK-001/006/007, ORDER-001..003, CFG-INIT-001, CFG-FOCUS-001, CFG-LAYOUT-001, CFG-FONT-001");
		Console.WriteLine("Entorno: Windows / x86 / .NET Framework 4.0");
		Console.WriteLine();

		List<TestResult> resultados = new List<TestResult>();
		resultados.Add(Ejecutar("FUN-041", TestsModConexion.FUN_041));
		resultados.Add(Ejecutar("FUN-042", TestsModConexion.FUN_042));
		resultados.Add(Ejecutar("FUN-043", TestsModConexion.FUN_043));
		resultados.Add(Ejecutar("FUN-044", TestsModConexion.FUN_044));
		resultados.Add(Ejecutar("HELPER-001", TestsTipoEquipo.HELPER_001));
		resultados.Add(Ejecutar("HELPER-002", TestsTipoEquipo.HELPER_002));
		resultados.Add(Ejecutar("HELPER-003", TestsTipoEquipo.HELPER_003));
		resultados.Add(Ejecutar("HELPER-004", TestsTipoEquipo.HELPER_004));
		resultados.Add(Ejecutar("QUICK-001", TestsTipoEquipo.QUICK_001));
		resultados.Add(Ejecutar("QUICK-006", TestsTipoEquipo.QUICK_006));
		resultados.Add(Ejecutar("QUICK-007", TestsTipoEquipo.QUICK_007));
		resultados.Add(Ejecutar("ORDER-001", TestsTipoEquipo.ORDER_001));
		resultados.Add(Ejecutar("ORDER-002", TestsTipoEquipo.ORDER_002));
		resultados.Add(Ejecutar("ORDER-003", TestsTipoEquipo.ORDER_003));
		resultados.Add(Ejecutar("CFG-INIT-001", TestsConfiguracion.CFG_INIT_001));
		resultados.Add(Ejecutar("CFG-FOCUS-001", TestsConfiguracion.CFG_FOCUS_001));
		resultados.Add(Ejecutar("CFG-LAYOUT-001", TestsConfiguracion.CFG_LAYOUT_001));
		resultados.Add(Ejecutar("CFG-FONT-001", TestsConfiguracion.CFG_FONT_001));
		resultados.Add(Ejecutar("FUN-FMT-001", TestsFormatoMoneda.FUN_FMT_001));
		resultados.Add(Ejecutar("FUN-FMT-002", TestsFormatoMoneda.FUN_FMT_002));
		resultados.Add(Ejecutar("FUN-FMT-003", TestsFormatoMoneda.FUN_FMT_003));
		resultados.Add(Ejecutar("FUN-FMT-004", TestsFormatoMoneda.FUN_FMT_004));
		resultados.Add(Ejecutar("FUN-FMT-005", TestsFormatoMoneda.FUN_FMT_005));
		resultados.Add(Ejecutar("FUN-FMT-006", TestsFormatoMoneda.FUN_FMT_006));
		resultados.Add(Ejecutar("FUN-FMT-007", TestsFormatoMoneda.FUN_FMT_007));

		Console.WriteLine();
		Console.WriteLine("=== RESUMEN ===");
		int fallas = 0;
		foreach (TestResult r in resultados)
		{
			Console.WriteLine(r.Ok ? "  " + r.Id + ": PASS" : "  " + r.Id + ": FAIL - " + r.Detalle);
			if (!r.Ok)
			{
				fallas++;
			}
		}
		Console.WriteLine("Total: " + resultados.Count + " | PASS: " + (resultados.Count - fallas) + " | FAIL: " + fallas);
		return (fallas == 0) ? 0 : 1;
	}

	private static TestResult Ejecutar(string id, Func<TestResult> prueba)
	{
		TestResult r;
		try
		{
			Console.WriteLine("Ejecutando " + id + "...");
			r = prueba();
		}
		catch (Exception ex)
		{
			r = new TestResult();
			r.Ok = false;
			r.Detalle = "Excepción: " + ex.Message;
		}
		r.Id = id;
		return r;
	}
}

internal sealed class TestResult
{
	public string Id;
	public bool Ok;
	public string Detalle = "";
}

internal static class TempDatabase
{
	public static string CrearDirectorio()
	{
		string dir = Path.Combine(Path.GetTempPath(), "ServicioTecnicoTests_" + Guid.NewGuid().ToString("N"));
		Directory.CreateDirectory(dir);
		return dir;
	}

	public static void Eliminar(string dir)
	{
		try
		{
			if (Directory.Exists(dir))
			{
				Directory.Delete(dir, true);
			}
		}
		catch (Exception ex)
		{
			Console.WriteLine("  Aviso: no se pudo eliminar la carpeta temporal " + dir + " - " + ex.Message);
		}
	}
}
