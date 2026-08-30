using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Runtime.CompilerServices;
using System.Windows.Forms;
using Microsoft.VisualBasic.CompilerServices;

namespace ServicioTecnico;

[StandardModule]
internal sealed class modConexion
{
	public static string rutaDB = Path.Combine(Application.StartupPath, "ordenes.db");

	public static SQLiteConnection conexion;

	public static void VerificarOCrearBD()
	{
		bool dbNueva = !File.Exists(rutaDB);
		conexion = new SQLiteConnection("Data Source=" + rutaDB + ";Version=3;");
		bool existiaTiposEquipo = ExisteTabla("tipos_equipo");
		CrearTablas();
		if (dbNueva)
		{
			InsertarDatosIniciales();
		}
		if (!existiaTiposEquipo)
		{
			InsertarTiposEquipoIniciales();
		}
	}

	private static bool ExisteTabla(string nombre)
	{
		if (!File.Exists(rutaDB))
		{
			return false;
		}
		using SQLiteConnection sQLiteConnection = new SQLiteConnection("Data Source=" + rutaDB + ";Version=3;");
		sQLiteConnection.Open();
		bool result;
		using (SQLiteCommand sQLiteCommand = new SQLiteCommand("SELECT COUNT(*) FROM sqlite_master WHERE type='table' AND name=@nombre", sQLiteConnection))
		{
			sQLiteCommand.Parameters.AddWithValue("@nombre", nombre);
			result = Conversions.ToLong(sQLiteCommand.ExecuteScalar()) > 0L;
		}
		sQLiteConnection.Close();
		return result;
	}

	public static void CrearTablas()
	{
		List<string> list = new List<string>();
		list.Add("CREATE TABLE IF NOT EXISTS condiciones_servicio (id INTEGER PRIMARY KEY AUTOINCREMENT, contenido TEXT NOT NULL, fecha_actualizacion TEXT DEFAULT CURRENT_TIMESTAMP)");
		list.Add("CREATE TABLE IF NOT EXISTS clientes (id_cliente INTEGER PRIMARY KEY AUTOINCREMENT, nombre TEXT NOT NULL, direccion TEXT, documento TEXT, telefono TEXT)");
		list.Add("CREATE TABLE IF NOT EXISTS ordenes (id_orden INTEGER PRIMARY KEY AUTOINCREMENT, fecha TEXT, id_cliente INTEGER, tipo_equipo TEXT, marca TEXT, modelo TEXT, imei TEXT, clave TEXT, accesorios TEXT, falla TEXT, observaciones TEXT, reparacion TEXT, abono REAL, reparado TEXT, entregado TEXT, presupuesto REAL, total REAL, estado_entrega TEXT, imagen1 TEXT, imagen2 TEXT, imagen3 TEXT, FOREIGN KEY(id_cliente) REFERENCES clientes(id_cliente))");
		list.Add("CREATE TABLE IF NOT EXISTS configuracion (clave TEXT PRIMARY KEY, valor TEXT)");
		list.Add("CREATE TABLE IF NOT EXISTS estados (id_estado INTEGER PRIMARY KEY AUTOINCREMENT, descripcion TEXT)");
		list.Add("CREATE TABLE IF NOT EXISTS tipos_equipo (id INTEGER PRIMARY KEY AUTOINCREMENT, nombre TEXT NOT NULL UNIQUE, activo INTEGER NOT NULL DEFAULT 1, orden_visual INTEGER NOT NULL DEFAULT 0)");
		using SQLiteConnection sQLiteConnection = new SQLiteConnection("Data Source=" + rutaDB + ";Version=3;");
		sQLiteConnection.Open();
		using (SQLiteCommand sQLiteCommand = new SQLiteCommand())
		{
			sQLiteCommand.Connection = sQLiteConnection;
			foreach (string item in list)
			{
				sQLiteCommand.CommandText = item;
				sQLiteCommand.ExecuteNonQuery();
			}
		}
		sQLiteConnection.Close();
	}

	private static void InsertarDatosIniciales()
	{
		using SQLiteConnection sQLiteConnection = new SQLiteConnection("Data Source=" + rutaDB + ";Version=3;");
		sQLiteConnection.Open();
		Dictionary<string, string> dictionary = new Dictionary<string, string>();
		dictionary.Add("impresora", "");
		dictionary.Add("imprimir_al_guardar", "false");
		dictionary.Add("fuente_ticket", "Courier New, 9");
		using (SQLiteCommand sQLiteCommand = new SQLiteCommand("INSERT OR IGNORE INTO configuracion (clave, valor) VALUES (@clave, @valor)", sQLiteConnection))
		{
			foreach (KeyValuePair<string, string> item in dictionary)
			{
				sQLiteCommand.Parameters.Clear();
				sQLiteCommand.Parameters.AddWithValue("@clave", item.Key);
				sQLiteCommand.Parameters.AddWithValue("@valor", item.Value);
				sQLiteCommand.ExecuteNonQuery();
			}
		}
		sQLiteConnection.Close();
	}

	private static void InsertarTiposEquipoIniciales()
	{
		using SQLiteConnection sQLiteConnection = new SQLiteConnection("Data Source=" + rutaDB + ";Version=3;");
		sQLiteConnection.Open();
		Dictionary<string, int> dictionary = new Dictionary<string, int>();
		dictionary.Add("Laptop", 1);
		dictionary.Add("Impresora", 2);
		dictionary.Add("PC", 3);
		dictionary.Add("Otros", 4);
		using (SQLiteCommand sQLiteCommand = new SQLiteCommand("INSERT OR IGNORE INTO tipos_equipo (nombre, activo, orden_visual) VALUES (@nombre, 1, @orden_visual)", sQLiteConnection))
		{
			foreach (KeyValuePair<string, int> item in dictionary)
			{
				sQLiteCommand.Parameters.Clear();
				sQLiteCommand.Parameters.AddWithValue("@nombre", item.Key);
				sQLiteCommand.Parameters.AddWithValue("@orden_visual", item.Value);
				sQLiteCommand.ExecuteNonQuery();
			}
		}
		sQLiteConnection.Close();
	}

	public static string ObtenerConfiguracion(string clave)
	{
		string result = "";
		if (!File.Exists(rutaDB))
		{
			return result;
		}
		using SQLiteConnection sQLiteConnection = new SQLiteConnection("Data Source=" + rutaDB + ";Version=3;");
		sQLiteConnection.Open();
		using (SQLiteCommand sQLiteCommand = new SQLiteCommand("SELECT valor FROM configuracion WHERE clave=@clave", sQLiteConnection))
		{
			sQLiteCommand.Parameters.AddWithValue("@clave", clave);
			object objectValue = RuntimeHelpers.GetObjectValue(sQLiteCommand.ExecuteScalar());
			if (objectValue != null)
			{
				result = objectValue.ToString();
			}
		}
		sQLiteConnection.Close();
		return result;
	}

	public static void GuardarConfiguracion(string clave, string valor)
	{
		using SQLiteConnection sQLiteConnection = new SQLiteConnection("Data Source=" + rutaDB + ";Version=3;");
		sQLiteConnection.Open();
		using (SQLiteCommand sQLiteCommand = new SQLiteCommand("INSERT OR REPLACE INTO configuracion (clave, valor) VALUES (@clave, @valor)", sQLiteConnection))
		{
			sQLiteCommand.Parameters.AddWithValue("@clave", clave);
			sQLiteCommand.Parameters.AddWithValue("@valor", valor);
			sQLiteCommand.ExecuteNonQuery();
		}
		sQLiteConnection.Close();
	}

	public static string ObtenerCondicionesServicio()
	{
		string result = "";
		if (!File.Exists(rutaDB))
		{
			return result;
		}
		using SQLiteConnection sQLiteConnection = new SQLiteConnection("Data Source=" + rutaDB + ";Version=3;");
		sQLiteConnection.Open();
		using (SQLiteCommand sQLiteCommand = new SQLiteCommand("SELECT contenido FROM condiciones_servicio ORDER BY fecha_actualizacion DESC LIMIT 1", sQLiteConnection))
		{
			object objectValue = RuntimeHelpers.GetObjectValue(sQLiteCommand.ExecuteScalar());
			if (objectValue != null)
			{
				result = objectValue.ToString();
			}
		}
		sQLiteConnection.Close();
		return result;
	}

	public static void GuardarCondicionesServicio(string contenido)
	{
		using SQLiteConnection sQLiteConnection = new SQLiteConnection("Data Source=" + rutaDB + ";Version=3;");
		sQLiteConnection.Open();
		using (SQLiteCommand sQLiteCommand = new SQLiteCommand("INSERT INTO condiciones_servicio (contenido) VALUES (@contenido)", sQLiteConnection))
		{
			sQLiteCommand.Parameters.AddWithValue("@contenido", contenido);
			sQLiteCommand.ExecuteNonQuery();
		}
		sQLiteConnection.Close();
	}
}
