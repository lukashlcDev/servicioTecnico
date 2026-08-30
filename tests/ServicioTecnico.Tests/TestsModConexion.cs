using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;

namespace ServicioTecnico.Tests;

internal static class TestsModConexion
{
	public static TestResult FUN_041()
	{
		string dir = TempDatabase.CrearDirectorio();
		try
		{
			string rutaDB = Path.Combine(dir, "ordenes.db");
			ReflectionModConexion.SetRutaDB(rutaDB);
			ReflectionModConexion.VerificarOCrearBD();

			const string clave = "test_k";
			const string valor = "test_v";
			ReflectionModConexion.GuardarConfiguracion(clave, valor);
			string obtenido = ReflectionModConexion.ObtenerConfiguracion(clave);

			TestResult r = new TestResult();
			r.Ok = string.Equals(obtenido, valor, StringComparison.Ordinal);
			r.Detalle = r.Ok ? "" : "Esperado '" + valor + "', obtenido '" + obtenido + "'";
			return r;
		}
		finally
		{
			TempDatabase.Eliminar(dir);
		}
	}

	public static TestResult FUN_042()
	{
		string dir = TempDatabase.CrearDirectorio();
		try
		{
			string rutaDB = Path.Combine(dir, "ordenes.db");
			ReflectionModConexion.SetRutaDB(rutaDB);
			ReflectionModConexion.VerificarOCrearBD();

			const string texto = "CONDICIONES DE PRUEBA FUN-042\nsegunda linea\ttercera";
			ReflectionModConexion.GuardarCondicionesServicio(texto);
			string obtenido = ReflectionModConexion.ObtenerCondicionesServicio();

			TestResult r = new TestResult();
			r.Ok = string.Equals(obtenido, texto, StringComparison.Ordinal);
			r.Detalle = r.Ok ? "" : "El texto recuperado no coincide con el guardado";
			return r;
		}
		finally
		{
			TempDatabase.Eliminar(dir);
		}
	}

	public static TestResult FUN_043()
	{
		string dir = TempDatabase.CrearDirectorio();
		try
		{
			string rutaDB = Path.Combine(dir, "ordenes.db");
			ReflectionModConexion.SetRutaDB(rutaDB);
			ReflectionModConexion.VerificarOCrearBD();

			if (!File.Exists(rutaDB))
			{
				return Fail("No se creo el archivo ordenes.db en " + rutaDB);
			}

			string[] tablas = ObtenerTablas(rutaDB);
			string[] esperadas = new string[] { "clientes", "condiciones_servicio", "configuracion", "estados", "ordenes", "tipos_equipo" };
			if (!ConjuntosIguales(tablas, esperadas))
			{
				return Fail("Tablas esperadas: [" + string.Join(", ", esperadas) + "] - obtenidas: [" + string.Join(", ", tablas) + "]");
			}

			Dictionary<string, string> config = ObtenerConfiguracionTabla(rutaDB);
			string[] errores;
			if (!ValidarConfigInicial(config, out errores))
			{
				return Fail(string.Join("; ", errores));
			}

			ReflectionModConexion.VerificarOCrearBD();

			Dictionary<string, string> config2 = ObtenerConfiguracionTabla(rutaDB);
			if (config2.Count != 3)
			{
				return Fail("VerificarOCrearBD duplico las claves: se esperaban 3, hay " + config2.Count);
			}

			string[] tablas2 = ObtenerTablas(rutaDB);
			if (!ConjuntosIguales(tablas2, esperadas))
			{
				return Fail("Las tablas cambiaron tras la segunda VerificarOCrearBD. Esperadas: [" + string.Join(", ", esperadas) + "] - obtenidas: [" + string.Join(", ", tablas2) + "]");
			}

			TestResult r = new TestResult();
			r.Ok = true;
			r.Detalle = "";
			return r;
		}
		finally
		{
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

	public static TestResult FUN_044()
	{
		string dir = TempDatabase.CrearDirectorio();
		try
		{
			string rutaDB = Path.Combine(dir, "ordenes.db");
			ReflectionModConexion.SetRutaDB(rutaDB);
			ReflectionModConexion.VerificarOCrearBD();

			if (!File.Exists(rutaDB))
			{
				return Fail("No se creo el archivo ordenes.db en " + rutaDB);
			}

			string[] columnas = ObtenerColumnasTiposEquipo(rutaDB);
			string[] esperadasColumnas = new string[] { "activo", "id", "nombre", "orden_visual" };
			if (!ConjuntosIguales(columnas, esperadasColumnas))
			{
				return Fail("Columnas de tipos_equipo esperadas: [" + string.Join(", ", esperadasColumnas) + "] - obtenidas: [" + string.Join(", ", columnas) + "]");
			}

			string[] seed = ObtenerTiposEquipoSeed(rutaDB);
			string[] esperadoSeed = new string[] { "Laptop:1:1", "Impresora:1:2", "PC:1:3", "Otros:1:4" };
			if (!ConjuntosIguales(seed, esperadoSeed))
			{
				return Fail("Seed de tipos_equipo esperado: [" + string.Join(", ", esperadoSeed) + "] - obtenido: [" + string.Join(", ", seed) + "]");
			}

			if (TieneFKDesdeOrdenes(rutaDB))
			{
				return Fail("Se encontro una FK desde ordenes hacia tipos_equipo");
			}

			ReflectionModConexion.VerificarOCrearBD();

			string[] seed2 = ObtenerTiposEquipoSeed(rutaDB);
			int totalFilas = seed2.Length;
			if (totalFilas != 4)
			{
				return Fail("La segunda VerificarOCrearBD duplico el seed: se esperaban 4 filas, hay " + totalFilas);
			}
			if (!ConjuntosIguales(seed2, esperadoSeed))
			{
				return Fail("El seed cambio tras la segunda VerificarOCrearBD: [" + string.Join(", ", seed2) + "]");
			}

			TestResult r = new TestResult();
			r.Ok = true;
			r.Detalle = "";
			return r;
		}
		finally
		{
			ReflectionModConexion.VerificarOCrearBD();
			TempDatabase.Eliminar(dir);
		}
	}

	private static string[] ObtenerColumnasTiposEquipo(string rutaDB)
	{
		List<string> lista = new List<string>();
		using (SQLiteConnection c = new SQLiteConnection("Data Source=" + rutaDB + ";Version=3;"))
		{
			c.Open();
			using (SQLiteCommand cmd = new SQLiteCommand("PRAGMA table_info(tipos_equipo);", c))
			{
				using (SQLiteDataReader rd = cmd.ExecuteReader())
				{
					while (rd.Read())
					{
						lista.Add(rd.GetString(1));
					}
				}
			}
		}
		return lista.ToArray();
	}

	private static string[] ObtenerTiposEquipoSeed(string rutaDB)
	{
		List<string> lista = new List<string>();
		using (SQLiteConnection c = new SQLiteConnection("Data Source=" + rutaDB + ";Version=3;"))
		{
			c.Open();
			using (SQLiteCommand cmd = new SQLiteCommand("SELECT nombre, activo, orden_visual FROM tipos_equipo ORDER BY orden_visual, nombre", c))
			{
				using (SQLiteDataReader rd = cmd.ExecuteReader())
				{
					while (rd.Read())
					{
						lista.Add(rd.GetString(0) + ":" + rd.GetInt32(1) + ":" + rd.GetInt32(2));
					}
				}
			}
		}
		return lista.ToArray();
	}

	private static bool TieneFKDesdeOrdenes(string rutaDB)
	{
		using (SQLiteConnection c = new SQLiteConnection("Data Source=" + rutaDB + ";Version=3;"))
		{
			c.Open();
			using (SQLiteCommand cmd = new SQLiteCommand("PRAGMA foreign_key_list(ordenes);", c))
			{
				using (SQLiteDataReader rd = cmd.ExecuteReader())
				{
					while (rd.Read())
					{
						string tabla = rd["table"].ToString();
						if (string.Equals(tabla, "tipos_equipo", StringComparison.OrdinalIgnoreCase))
						{
							return true;
						}
					}
				}
			}
		}
		return false;
	}

	private static string[] ObtenerTablas(string rutaDB)
	{
		List<string> lista = new List<string>();
		using (SQLiteConnection c = new SQLiteConnection("Data Source=" + rutaDB + ";Version=3;"))
		{
			c.Open();
			using (SQLiteCommand cmd = new SQLiteCommand("SELECT name FROM sqlite_master WHERE type='table' AND name NOT LIKE 'sqlite_%' ORDER BY name", c))
			{
				using (SQLiteDataReader rd = cmd.ExecuteReader())
				{
					while (rd.Read())
					{
						lista.Add(rd.GetString(0));
					}
				}
			}
		}
		return lista.ToArray();
	}

	private static Dictionary<string, string> ObtenerConfiguracionTabla(string rutaDB)
	{
		Dictionary<string, string> dict = new Dictionary<string, string>();
		using (SQLiteConnection c = new SQLiteConnection("Data Source=" + rutaDB + ";Version=3;"))
		{
			c.Open();
			using (SQLiteCommand cmd = new SQLiteCommand("SELECT clave, valor FROM configuracion ORDER BY clave", c))
			{
				using (SQLiteDataReader rd = cmd.ExecuteReader())
				{
					while (rd.Read())
					{
						dict.Add(rd.GetString(0), rd.GetString(1));
					}
				}
			}
		}
		return dict;
	}

	private static bool ValidarConfigInicial(Dictionary<string, string> config, out string[] errores)
	{
		List<string> lista = new List<string>();
		if (config.Count != 3)
		{
			lista.Add("Se esperaban 3 claves de configuracion, hay " + config.Count);
		}
		VerificarClave(config, "impresora", "", lista);
		VerificarClave(config, "imprimir_al_guardar", "false", lista);
		VerificarClave(config, "fuente_ticket", "Courier New, 9", lista);
		errores = lista.ToArray();
		return lista.Count == 0;
	}

	private static void VerificarClave(Dictionary<string, string> config, string clave, string esperado, List<string> errores)
	{
		string valor;
		if (!config.TryGetValue(clave, out valor))
		{
			errores.Add("Falta la clave " + clave);
		}
		else if (!string.Equals(valor, esperado, StringComparison.Ordinal))
		{
			errores.Add("Clave " + clave + " esperada '" + esperado + "', obtenida '" + valor + "'");
		}
	}

	private static bool ConjuntosIguales(string[] a, string[] b)
	{
		Array.Sort(a);
		Array.Sort(b);
		if (a.Length != b.Length)
		{
			return false;
		}
		for (int i = 0; i < a.Length; i++)
		{
			if (!string.Equals(a[i], b[i], StringComparison.Ordinal))
			{
				return false;
			}
		}
		return true;
	}
}
