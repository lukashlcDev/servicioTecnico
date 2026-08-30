using System;
using System.Reflection;

namespace ServicioTecnico.Tests;

internal static class ReflectionTipoEquipo
{
	private static readonly Type Tipo = typeof(ServicioTecnico.Cliente).Assembly.GetType("ServicioTecnico.TipoEquipoCatalog");

	public static string NormalizarNombre(string s)
	{
		return (string)Invocar("NormalizarNombre", s);
	}

	public static bool IgualNombre(string a, string b)
	{
		return (bool)Invocar("IgualNombre", a, b);
	}

	public static string ResolverOCrearTipo(string s)
	{
		return (string)Invocar("ResolverOCrearTipo", s);
	}

	public static int ObtenerActivoPorNombre(string nombre)
	{
		object lista = Invocar("ObtenerTipos");
		foreach (object item in (System.Collections.IEnumerable)lista)
		{
			string n = (string)item.GetType().GetProperty("Nombre").GetValue(item, null);
			if (string.Equals(n, nombre, StringComparison.Ordinal))
			{
				return (int)item.GetType().GetProperty("Activo").GetValue(item, null);
			}
		}
		return -1;
	}

	public static int CompararCanonico(string nombreA, string nombreB)
	{
		Type tipoEquipo = typeof(ServicioTecnico.Cliente).Assembly.GetType("ServicioTecnico.TipoEquipo");
		object oa = Activator.CreateInstance(tipoEquipo, 1, nombreA, 1, 0);
		object ob = Activator.CreateInstance(tipoEquipo, 2, nombreB, 1, 0);
		return (int)Invocar("CompararCanonico", oa, ob);
	}

	public static System.Collections.Generic.List<string> ObtenerNombresEnOrden()
	{
		var resultado = new System.Collections.Generic.List<string>();
		object lista = Invocar("ObtenerTipos");
		foreach (object item in (System.Collections.IEnumerable)lista)
		{
			resultado.Add((string)item.GetType().GetProperty("Nombre").GetValue(item, null));
		}
		return resultado;
	}

	private static object Invocar(string nombre, params object[] args)
	{
		MethodInfo m = Tipo.GetMethod(nombre, BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
		return m.Invoke(null, args);
	}
}
