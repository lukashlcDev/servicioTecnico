using System;
using System.Data.SQLite;
using System.IO;

namespace ServicioTecnico.Tests;

internal static class TestsTipoEquipo
{
	public static TestResult HELPER_001()
	{
		using Session s = Session.Iniciar();
		ReflectionTipoEquipo.ResolverOCrearTipo("Heladera");
		int antes = s.ContarTipos();
		string r = ReflectionTipoEquipo.ResolverOCrearTipo("heladera");
		int despues = s.ContarTipos();
		if (!string.Equals(r, "Heladera", StringComparison.Ordinal))
		{
			return Fail("retorno = " + r);
		}
		if (antes != despues)
		{
			return Fail("inserto duplicado: " + antes + " -> " + despues);
		}
		return Pass();
	}

	public static TestResult HELPER_002()
	{
		using Session s = Session.Iniciar();
		ReflectionTipoEquipo.ResolverOCrearTipo("Árbol");
		int antes = s.ContarTipos();
		string r = ReflectionTipoEquipo.ResolverOCrearTipo("árbol");
		if (r != "Árbol")
		{
			return Fail("retorno = " + r);
		}
		if (antes != s.ContarTipos())
		{
			return Fail("inserto duplicado");
		}
		return Pass();
	}

	public static TestResult HELPER_003()
	{
		using Session s = Session.Iniciar();
		ReflectionTipoEquipo.ResolverOCrearTipo("Café");
		int antes = s.ContarTipos();
		string r = ReflectionTipoEquipo.ResolverOCrearTipo("Cafe");
		if (r == "Café")
		{
			return Fail("ignoro diacritico");
		}
		if (antes == s.ContarTipos())
		{
			return Fail("no inserto Cafe como nuevo tipo");
		}
		return Pass();
	}

	public static TestResult HELPER_004()
	{
		using Session s = Session.Iniciar();
		if (!ReflectionTipoEquipo.IgualNombre("Otros", "otros"))
		{
			return Fail("Otros != otros");
		}
		if (!ReflectionTipoEquipo.IgualNombre("Otros", "OTROS"))
		{
			return Fail("Otros != OTROS");
		}
		string r = ReflectionTipoEquipo.ResolverOCrearTipo("otros");
		if (r != "Otros")
		{
			return Fail("no reutilizo tipo canonico 'Otros': " + r);
		}
		int count;
		using (SQLiteConnection c = new SQLiteConnection("Data Source=" + modConexion.rutaDB + ";Version=3;"))
		{
			c.Open();
			using SQLiteCommand cmd = new SQLiteCommand("SELECT COUNT(*) FROM tipos_equipo WHERE nombre='Otros'", c);
			count = Convert.ToInt32(cmd.ExecuteScalar());
		}
		if (count != 1)
		{
			return Fail("hay " + count + " tipos 'Otros'");
		}
		return Pass();
	}

	public static TestResult QUICK_001()
	{
		using Session s = Session.Iniciar();
		string r = ReflectionTipoEquipo.ResolverOCrearTipo("Heladera");
		if (r != "Heladera")
		{
			return Fail("retorno = " + r);
		}
		if (s.ObtenerActivo("Heladera") != 1)
		{
			return Fail("no inserto activo");
		}
		return Pass();
	}

	public static TestResult QUICK_006()
	{
		using Session s = Session.Iniciar();
		s.Insertar("Heladera", 0);
		string r = ReflectionTipoEquipo.ResolverOCrearTipo("heladera");
		if (r != "Heladera")
		{
			return Fail("retorno = " + r);
		}
		if (s.ObtenerActivo("Heladera") != 1)
		{
			return Fail("no reactivo");
		}
		if (s.ContarTipos() != 5)
		{
			return Fail("inserto duplicado: " + s.ContarTipos());
		}
		return Pass();
	}

	public static TestResult QUICK_007()
	{
		using Session s = Session.Iniciar();
		string r = ReflectionTipoEquipo.ResolverOCrearTipo("  Aire Acondicionado  ");
		if (r != "Aire Acondicionado")
		{
			return Fail("retorno = [" + r + "]");
		}
		return Pass();
	}

	public static TestResult ORDER_001()
	{
		if (ReflectionTipoEquipo.CompararCanonico("Otros", "Laptop") >= 0)
		{
			return Fail("Otros deberia ir antes de Laptop");
		}
		if (ReflectionTipoEquipo.CompararCanonico("Laptop", "Otros") <= 0)
		{
			return Fail("Laptop deberia ir despues de Otros");
		}
		if (ReflectionTipoEquipo.CompararCanonico("otros", "OTROS") != 0)
		{
			return Fail("otros/OTROS no deberian diferir");
		}
		if (ReflectionTipoEquipo.CompararCanonico("Impresora", "PC") >= 0)
		{
			return Fail("Impresora deberia ir antes de PC (orden alfabetico)");
		}
		return Pass();
	}

	public static TestResult ORDER_002()
	{
		using Session s = Session.Iniciar();
		s.Insertar("Zapato", 1);
		s.Insertar("Arbol", 1);
		s.Insertar("Cama", 1);
		var orden = ReflectionTipoEquipo.ObtenerNombresEnOrden();
		if (orden.Count == 0 || orden[0] != "Otros")
		{
			return Fail("Otros no va primero: [" + string.Join("|", orden) + "]");
		}
		int iOtros = orden.IndexOf("Otros");
		if (iOtros != 0)
		{
			return Fail("Otros en posicion " + iOtros);
		}
		return Pass();
	}

	public static TestResult ORDER_003()
	{
		if (ReflectionTipoEquipo.CompararCanonico("Arbol", "Zapato") >= 0)
		{
			return Fail("Arbol deberia ir antes que Zapato");
		}
		if (ReflectionTipoEquipo.CompararCanonico("Arbol", "Arbol") != 0)
		{
			return Fail("Arbol/Arbol deberian ser iguales");
		}
		if (ReflectionTipoEquipo.CompararCanonico("Aire", "Arbol") >= 0)
		{
			return Fail("Aire deberia ir antes que Arbol");
		}
		return Pass();
	}

	private static TestResult Fail(string detalle)
	{
		TestResult r = new TestResult();
		r.Ok = false;
		r.Detalle = detalle;
		return r;
	}

	private static TestResult Pass()
	{
		TestResult r = new TestResult();
		r.Ok = true;
		r.Detalle = "";
		return r;
	}

	private sealed class Session : IDisposable
	{
		private readonly string _dir;

		private Session(string dir)
		{
			_dir = dir;
			string ruta = Path.Combine(dir, "ordenes.db");
			ReflectionModConexion.SetRutaDB(ruta);
			ReflectionModConexion.VerificarOCrearBD();
		}

		public static Session Iniciar()
		{
			return new Session(TempDatabase.CrearDirectorio());
		}

		public void Insertar(string nombre, int activo)
		{
			using SQLiteConnection c = new SQLiteConnection("Data Source=" + modConexion.rutaDB + ";Version=3;");
			c.Open();
			using SQLiteCommand cmd = new SQLiteCommand("INSERT INTO tipos_equipo (nombre, activo, orden_visual) VALUES (@n, @a, 5)", c);
			cmd.Parameters.AddWithValue("@n", nombre);
			cmd.Parameters.AddWithValue("@a", activo);
			cmd.ExecuteNonQuery();
		}

		public int ContarTipos()
		{
			using SQLiteConnection c = new SQLiteConnection("Data Source=" + modConexion.rutaDB + ";Version=3;");
			c.Open();
			using SQLiteCommand cmd = new SQLiteCommand("SELECT COUNT(*) FROM tipos_equipo", c);
			return Convert.ToInt32(cmd.ExecuteScalar());
		}

		public int ObtenerActivo(string nombre)
		{
			using SQLiteConnection c = new SQLiteConnection("Data Source=" + modConexion.rutaDB + ";Version=3;");
			c.Open();
			using SQLiteCommand cmd = new SQLiteCommand("SELECT activo FROM tipos_equipo WHERE nombre=@n", c);
			cmd.Parameters.AddWithValue("@n", nombre);
			object o = cmd.ExecuteScalar();
			return o == null ? -1 : Convert.ToInt32(o);
		}

		public void Dispose()
		{
			TempDatabase.Eliminar(_dir);
		}
	}
}
