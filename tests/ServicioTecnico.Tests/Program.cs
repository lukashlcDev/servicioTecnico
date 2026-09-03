using System;
using System.Collections.Generic;
using System.IO;

namespace ServicioTecnico.Tests;

internal static class Program
{
	private static int Main()
	{
		Console.WriteLine("Host de pruebas ServicioTecnico.Tests");
		Console.WriteLine("Casos: FUN-041..044, FUN-FMT-001..007, HELPER-001..004, QUICK-001/006/007, ORDER-001..003, CFG-INIT-001, CFG-FOCUS-001, CFG-LAYOUT-001, CFG-FONT-001, IMP-MODELO-001..007, IMP-ORQ-001..003, IMP-CARTA-001..002, IMP-TICKET-001..002, IMP-RENDER-001..002, LS-FACTOR-001..005, LS-RATIO-001, LS-SNAP-001, LS-RESTORE-001, LS-CYCLE-001/002, LS-CENTER-001/002, LS-FONT-001, LS-RECUR-001, LS-DPI-001");
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
		resultados.Add(Ejecutar("IMP-MODELO-001", TestsImpresion.IMP_MODELO_001));
		resultados.Add(Ejecutar("IMP-MODELO-002", TestsImpresion.IMP_MODELO_002));
		resultados.Add(Ejecutar("IMP-MODELO-003", TestsImpresion.IMP_MODELO_003));
		resultados.Add(Ejecutar("IMP-MODELO-004", TestsImpresion.IMP_MODELO_004));
		resultados.Add(Ejecutar("IMP-MODELO-005", TestsImpresion.IMP_MODELO_005));
		resultados.Add(Ejecutar("IMP-MODELO-006", TestsImpresion.IMP_MODELO_006));
		resultados.Add(Ejecutar("IMP-MODELO-007", TestsImpresion.IMP_MODELO_007));
		resultados.Add(Ejecutar("IMP-ORQ-001", TestsImpresion.IMP_ORQ_001));
		resultados.Add(Ejecutar("IMP-ORQ-002", TestsImpresion.IMP_ORQ_002));
		resultados.Add(Ejecutar("IMP-CARTA-001", TestsImpresion.IMP_CARTA_001));
		resultados.Add(Ejecutar("IMP-CARTA-002", TestsImpresion.IMP_CARTA_002));
		resultados.Add(Ejecutar("IMP-ORQ-003", TestsImpresion.IMP_ORQ_003));
		resultados.Add(Ejecutar("IMP-TICKET-001", TestsImpresion.IMP_TICKET_001));
		resultados.Add(Ejecutar("IMP-TICKET-002", TestsImpresion.IMP_TICKET_002));
		resultados.Add(Ejecutar("IMP-RENDER-001", TestsImpresion.IMP_RENDER_001));
		resultados.Add(Ejecutar("IMP-RENDER-002", TestsImpresion.IMP_RENDER_002));
		resultados.Add(Ejecutar("LS-FACTOR-001", TestsLayoutSizer.LS_FACTOR_001));
		resultados.Add(Ejecutar("LS-FACTOR-002", TestsLayoutSizer.LS_FACTOR_002));
		resultados.Add(Ejecutar("LS-FACTOR-003", TestsLayoutSizer.LS_FACTOR_003));
		resultados.Add(Ejecutar("LS-FACTOR-004", TestsLayoutSizer.LS_FACTOR_004));
		resultados.Add(Ejecutar("LS-FACTOR-005", TestsLayoutSizer.LS_FACTOR_005));
		resultados.Add(Ejecutar("LS-RATIO-001", TestsLayoutSizer.LS_RATIO_001));
		resultados.Add(Ejecutar("LS-SNAP-001", TestsLayoutSizer.LS_SNAP_001));
		resultados.Add(Ejecutar("LS-RESTORE-001", TestsLayoutSizer.LS_RESTORE_001));
		resultados.Add(Ejecutar("LS-CYCLE-001", TestsLayoutSizer.LS_CYCLE_001));
		resultados.Add(Ejecutar("LS-CYCLE-002", TestsLayoutSizer.LS_CYCLE_002));
		resultados.Add(Ejecutar("LS-CENTER-001", TestsLayoutSizer.LS_CENTER_001));
		resultados.Add(Ejecutar("LS-CENTER-002", TestsLayoutSizer.LS_CENTER_002));
		resultados.Add(Ejecutar("LS-FONT-001", TestsLayoutSizer.LS_FONT_001));
		resultados.Add(Ejecutar("LS-RECUR-001", TestsLayoutSizer.LS_RECUR_001));
		resultados.Add(Ejecutar("LS-DPI-001", TestsLayoutSizer.LS_DPI_001));

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
