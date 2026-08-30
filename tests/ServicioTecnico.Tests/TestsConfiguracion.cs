using System;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;

namespace ServicioTecnico.Tests;

internal static class ReflectionConfiguracion
{
	private static readonly Type Tipo = typeof(ServicioTecnico.Cliente).Assembly.GetType("ServicioTecnico.frmConfiguracion");

	public static bool ValidarEstructuraTab()
	{
		bool ok = false;
		Exception error = null;
		Thread hilo = new Thread(() =>
		{
			try
			{
				object form = Activator.CreateInstance(Tipo);
				FieldInfo fTab = Tipo.GetField("tabControl", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
				FieldInfo fGeneral = Tipo.GetField("tabGeneral", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
				FieldInfo fTipos = Tipo.GetField("tabTipos", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
				FieldInfo fPanel = Tipo.GetField("Panel1", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
				FieldInfo fGrp = Tipo.GetField("grpTipoEquipo", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);

				TabControl tc = (TabControl)fTab.GetValue(form);
				TabPage gen = (TabPage)fGeneral.GetValue(form);
				TabPage tip = (TabPage)fTipos.GetValue(form);
				Panel p1 = (Panel)fPanel.GetValue(form);
				GroupBox grp = (GroupBox)fGrp.GetValue(form);

				ok = tc.TabCount == 2
					&& tc.SelectedIndex == 0
					&& gen.Contains(p1)
					&& tip.Contains(grp)
					&& tc.Controls.Contains(gen)
					&& tc.Controls.Contains(tip);
			}
			catch (Exception ex)
			{
				error = ex;
			}
		});
		hilo.SetApartmentState(ApartmentState.STA);
		hilo.Start();
		hilo.Join();
		if (error != null)
		{
			throw error;
		}
		return ok;
	}

	public static bool ValidarFocoTitulo()
	{
		bool ok = false;
		Exception error = null;
		Thread hilo = new Thread(() =>
		{
			try
			{
				object form = Activator.CreateInstance(Tipo);
				FieldInfo fTextBox = Tipo.GetField("TextBox1", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
				TextBox titulo = (TextBox)fTextBox.GetValue(form);
				ok = !titulo.TabStop;
			}
			catch (Exception ex)
			{
				error = ex;
			}
		});
		hilo.SetApartmentState(ApartmentState.STA);
		hilo.Start();
		hilo.Join();
		if (error != null)
		{
			throw error;
		}
		return ok;
	}

	public static bool ValidarLayoutTipos()
	{
		bool ok = false;
		Exception error = null;
		Thread hilo = new Thread(() =>
		{
			try
			{
				object form = Activator.CreateInstance(Tipo);
				FieldInfo fDgv = Tipo.GetField("dgvTiposEquipo", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
				FieldInfo fColNombre = Tipo.GetField("colNombre", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
				FieldInfo fColEstado = Tipo.GetField("colEstado", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
				FieldInfo fTxt = Tipo.GetField("txtNuevoNombreTipo", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);

				DataGridView dgv = (DataGridView)fDgv.GetValue(form);
				DataGridViewTextBoxColumn colNombre = (DataGridViewTextBoxColumn)fColNombre.GetValue(form);
				DataGridViewTextBoxColumn colEstado = (DataGridViewTextBoxColumn)fColEstado.GetValue(form);
				TextBox txt = (TextBox)fTxt.GetValue(form);

				bool dgvFill = dgv.AutoSizeColumnsMode == DataGridViewAutoSizeColumnsMode.Fill;
				bool anchorRight = (dgv.Anchor & AnchorStyles.Right) == AnchorStyles.Right;
				bool colsOk = colNombre.FillWeight > colEstado.FillWeight;
				ok = dgvFill && anchorRight && colsOk && dgv.Width >= 350;
			}
			catch (Exception ex)
			{
				error = ex;
			}
		});
		hilo.SetApartmentState(ApartmentState.STA);
		hilo.Start();
		hilo.Join();
		if (error != null)
		{
			throw error;
		}
		return ok;
	}

	public static bool ValidarEnumeracionFuentes()
	{
		MethodInfo metodo = Tipo.GetMethod("EnumerarFuentesMonoespaciadas", BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public);
		if (metodo == null)
		{
			throw new InvalidOperationException("Metodo EnumerarFuentesMonoespaciadas no encontrado");
		}
		object resultado = metodo.Invoke(null, null);
		if (resultado == null || !(resultado is System.Collections.Generic.List<string>))
		{
			throw new InvalidOperationException("resultado null o tipo incorrecto");
		}
		System.Collections.Generic.List<string> lista = (System.Collections.Generic.List<string>)resultado;
		foreach (string nombre in lista)
		{
			if (string.IsNullOrEmpty(nombre))
			{
				throw new InvalidOperationException("nombre vacio o null");
			}
		}
		var vistos = new System.Collections.Generic.HashSet<string>(StringComparer.OrdinalIgnoreCase);
		bool duplicados = false;
		foreach (string nombre in lista)
		{
			if (!vistos.Add(nombre))
			{
				duplicados = true;
				break;
			}
		}
		if (duplicados)
		{
			throw new InvalidOperationException("duplicados logicos detectados");
		}
		return true;
	}
}

internal static class TestsConfiguracion
{
	public static TestResult CFG_INIT_001()
	{
		try
		{
			if (!ReflectionConfiguracion.ValidarEstructuraTab())
			{
				return Fail("estructura de TabControl incorrecta");
			}
			return Pass();
		}
		catch (Exception ex)
		{
			return Fail("excepcion: " + ex.Message);
		}
	}

	public static TestResult CFG_FOCUS_001()
	{
		try
		{
			if (!ReflectionConfiguracion.ValidarFocoTitulo())
			{
				return Fail("TextBox1.TabStop distinto de false");
			}
			return Pass();
		}
		catch (Exception ex)
		{
			return Fail("excepcion: " + ex.Message);
		}
	}

	public static TestResult CFG_LAYOUT_001()
	{
		try
		{
			if (!ReflectionConfiguracion.ValidarLayoutTipos())
			{
				return Fail("layout pestaña Tipos incorrecto");
			}
			return Pass();
		}
		catch (Exception ex)
		{
			return Fail("excepcion: " + ex.Message);
		}
	}

	public static TestResult CFG_FONT_001()
	{
		try
		{
			ReflectionConfiguracion.ValidarEnumeracionFuentes();
			return Pass();
		}
		catch (Exception ex)
		{
			return Fail("excepcion: " + ex.Message);
		}
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
}
