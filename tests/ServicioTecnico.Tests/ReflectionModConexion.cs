using System;
using System.Reflection;

namespace ServicioTecnico.Tests;

internal static class ReflectionModConexion
{
	private static readonly Type Tipo = typeof(ServicioTecnico.Cliente).Assembly.GetType("ServicioTecnico.modConexion");

	private static readonly FieldInfo CampoRutaDB = Tipo.GetField("rutaDB", BindingFlags.Public | BindingFlags.Static);

	public static void SetRutaDB(string ruta)
	{
		CampoRutaDB.SetValue(null, ruta);
	}

	public static void VerificarOCrearBD()
	{
		Invocar("VerificarOCrearBD");
	}

	public static void GuardarConfiguracion(string clave, string valor)
	{
		Invocar("GuardarConfiguracion", clave, valor);
	}

	public static string ObtenerConfiguracion(string clave)
	{
		return (string)Invocar("ObtenerConfiguracion", clave);
	}

	public static void GuardarCondicionesServicio(string contenido)
	{
		Invocar("GuardarCondicionesServicio", contenido);
	}

	public static string ObtenerCondicionesServicio()
	{
		return (string)Invocar("ObtenerCondicionesServicio");
	}

	private static object Invocar(string nombre, params object[] args)
	{
		MethodInfo m = Tipo.GetMethod(nombre, BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
		return m.Invoke(null, args);
	}
}
