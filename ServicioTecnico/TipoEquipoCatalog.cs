using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Globalization;
using System.Text;

namespace ServicioTecnico;

internal static class TipoEquipoCatalog
{
	private static readonly CultureInfo CulturaArgentina = new CultureInfo("es-AR");

	public static string NormalizarNombre(string nombre)
	{
		return nombre.Trim().Normalize(NormalizationForm.FormC);
	}

	public static bool IgualNombre(string a, string b)
	{
		string x = (a ?? "").Trim().Normalize(NormalizationForm.FormC);
		string y = (b ?? "").Trim().Normalize(NormalizationForm.FormC);
		return CulturaArgentina.CompareInfo.Compare(x, y, CompareOptions.IgnoreCase) == 0;
	}

	public static int CompararCanonico(TipoEquipo a, TipoEquipo b)
	{
		bool aOtros = IgualNombre(a.Nombre, "Otros");
		bool bOtros = IgualNombre(b.Nombre, "Otros");
		if (aOtros != bOtros)
		{
			return aOtros ? -1 : 1;
		}
		return CulturaArgentina.CompareInfo.Compare(a.Nombre.Normalize(NormalizationForm.FormC), b.Nombre.Normalize(NormalizationForm.FormC), CompareOptions.IgnoreCase);
	}

	public static List<TipoEquipo> ObtenerTipos()
	{
		List<TipoEquipo> lista = new List<TipoEquipo>();
		using SQLiteConnection sQLiteConnection = new SQLiteConnection("Data Source=" + modConexion.rutaDB);
		sQLiteConnection.Open();
		using (SQLiteCommand sQLiteCommand = new SQLiteCommand("SELECT id, nombre, activo, orden_visual FROM tipos_equipo", sQLiteConnection))
		{
			using SQLiteDataReader sQLiteDataReader = sQLiteCommand.ExecuteReader();
			while (sQLiteDataReader.Read())
			{
				lista.Add(new TipoEquipo(Convert.ToInt32(sQLiteDataReader["id"]), sQLiteDataReader["nombre"].ToString(), Convert.ToInt32(sQLiteDataReader["activo"]), Convert.ToInt32(sQLiteDataReader["orden_visual"])));
			}
		}
		sQLiteConnection.Close();
		lista.Sort(CompararCanonico);
		return lista;
	}

	public static TipoEquipo BuscarTipo(string nombreNormalizado)
	{
		foreach (TipoEquipo tipo in ObtenerTipos())
		{
			if (IgualNombre(tipo.Nombre, nombreNormalizado))
			{
				return tipo;
			}
		}
		return null;
	}

	public static string ResolverOCrearTipo(string nombreSugerido)
	{
		string nombreTrim = NormalizarNombre(nombreSugerido);
		if (string.IsNullOrEmpty(nombreTrim))
		{
			return nombreTrim;
		}
		using SQLiteConnection sQLiteConnection = new SQLiteConnection("Data Source=" + modConexion.rutaDB);
		sQLiteConnection.Open();
		List<(int id, string nombre, int activo)> list = new List<(int, string, int)>();
		using (SQLiteCommand sQLiteCommand = new SQLiteCommand("SELECT id, nombre, activo FROM tipos_equipo", sQLiteConnection))
		{
			using SQLiteDataReader sQLiteDataReader = sQLiteCommand.ExecuteReader();
			while (sQLiteDataReader.Read())
			{
				list.Add((Convert.ToInt32(sQLiteDataReader["id"]), sQLiteDataReader["nombre"].ToString().Normalize(NormalizationForm.FormC), Convert.ToInt32(sQLiteDataReader["activo"])));
			}
		}
		foreach (var item in list)
		{
			if (CulturaArgentina.CompareInfo.Compare(item.nombre, nombreTrim, CompareOptions.IgnoreCase) == 0)
			{
				if (item.activo == 0)
				{
					using SQLiteCommand sQLiteCommand2 = new SQLiteCommand("UPDATE tipos_equipo SET activo = 1 WHERE id = @id", sQLiteConnection);
					sQLiteCommand2.Parameters.AddWithValue("@id", item.id);
					sQLiteCommand2.ExecuteNonQuery();
				}
				sQLiteConnection.Close();
				return item.nombre;
			}
		}
		int num = 1;
		using (SQLiteCommand sQLiteCommand3 = new SQLiteCommand("SELECT COALESCE(MAX(orden_visual), 0) + 1 FROM tipos_equipo", sQLiteConnection))
		{
			num = Convert.ToInt32(sQLiteCommand3.ExecuteScalar());
		}
		using (SQLiteCommand sQLiteCommand4 = new SQLiteCommand("INSERT INTO tipos_equipo (nombre, activo, orden_visual) VALUES (@nombre, 1, @orden)", sQLiteConnection))
		{
			sQLiteCommand4.Parameters.AddWithValue("@nombre", nombreTrim);
			sQLiteCommand4.Parameters.AddWithValue("@orden", num);
			sQLiteCommand4.ExecuteNonQuery();
		}
		sQLiteConnection.Close();
		return nombreTrim;
	}

	public static void Agregar(string nombre)
	{
		string nombreTrim = NormalizarNombre(nombre);
		using SQLiteConnection sQLiteConnection = new SQLiteConnection("Data Source=" + modConexion.rutaDB);
		sQLiteConnection.Open();
		int num = 1;
		using (SQLiteCommand sQLiteCommand = new SQLiteCommand("SELECT COALESCE(MAX(orden_visual), 0) + 1 FROM tipos_equipo", sQLiteConnection))
		{
			num = Convert.ToInt32(sQLiteCommand.ExecuteScalar());
		}
		using (SQLiteCommand sQLiteCommand2 = new SQLiteCommand("INSERT INTO tipos_equipo (nombre, activo, orden_visual) VALUES (@nombre, 1, @orden)", sQLiteConnection))
		{
			sQLiteCommand2.Parameters.AddWithValue("@nombre", nombreTrim);
			sQLiteCommand2.Parameters.AddWithValue("@orden", num);
			sQLiteCommand2.ExecuteNonQuery();
		}
		sQLiteConnection.Close();
	}

	public static void Reactivar(int id)
	{
		Activar(id);
	}

	public static void Activar(int id)
	{
		EjecutarUpdate("UPDATE tipos_equipo SET activo = 1 WHERE id = @id", id);
	}

	public static void Desactivar(int id)
	{
		EjecutarUpdate("UPDATE tipos_equipo SET activo = 0 WHERE id = @id", id);
	}

	public static void Renombrar(int id, string nuevoNombre)
	{
		string nombreTrim = NormalizarNombre(nuevoNombre);
		EjecutarUpdate("UPDATE tipos_equipo SET nombre = @nombre WHERE id = @id", id, nombreTrim);
	}

	public static int ContarOrdenesPorTipo(string nombreTipo)
	{
		using SQLiteConnection sQLiteConnection = new SQLiteConnection("Data Source=" + modConexion.rutaDB);
		sQLiteConnection.Open();
		int resultado = 0;
		using (SQLiteCommand sQLiteCommand = new SQLiteCommand("SELECT COUNT(*) FROM ordenes WHERE tipo_equipo = @nombre", sQLiteConnection))
		{
			sQLiteCommand.Parameters.AddWithValue("@nombre", nombreTipo);
			resultado = Convert.ToInt32(sQLiteCommand.ExecuteScalar());
		}
		sQLiteConnection.Close();
		return resultado;
	}

	private static void EjecutarUpdate(string sql, int id, string nombre = null)
	{
		using SQLiteConnection sQLiteConnection = new SQLiteConnection("Data Source=" + modConexion.rutaDB);
		sQLiteConnection.Open();
		using (SQLiteCommand sQLiteCommand = new SQLiteCommand(sql, sQLiteConnection))
		{
			sQLiteCommand.Parameters.AddWithValue("@id", id);
			if (nombre != null)
			{
				sQLiteCommand.Parameters.AddWithValue("@nombre", nombre);
			}
			sQLiteCommand.ExecuteNonQuery();
		}
		sQLiteConnection.Close();
	}
}

internal sealed class TipoEquipo
{
	public int Id { get; }
	public string Nombre { get; }
	public int Activo { get; }
	public int OrdenVisual { get; }

	public TipoEquipo(int id, string nombre, int activo, int ordenVisual)
	{
		Id = id;
		Nombre = nombre;
		Activo = activo;
		OrdenVisual = ordenVisual;
	}
}
