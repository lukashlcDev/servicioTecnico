using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Printing;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.VisualBasic;
using Microsoft.VisualBasic.CompilerServices;

namespace ServicioTecnico;

public partial class frmConfiguracion : Form
{
	private static List<string> _fuentesMonoCache;
	private static Task<List<string>> _tareaFuentes;

	public frmConfiguracion()
	{
		base.Load += frmConfiguracion_Load;
		base.Shown += frmConfiguracion_Shown;
		InitializeComponent();
	}

	private void IniciarCargaFuentes()
	{
		string configured = null;
		decimal configuredSize = 8m;
		string text = modConexion.ObtenerConfiguracion("fuente_ticket");
		if (!string.IsNullOrEmpty(text))
		{
			string[] array = text.Split(',');
			if (array.Length > 0)
			{
				configured = array[0];
			}
			if (array.Length > 1)
			{
				configuredSize = new decimal(Conversion.Val(array[1]));
			}
		}
		else
		{
			configured = "Courier New";
		}

		cmbFuente.Items.Clear();
		cmbFuente.Items.Add(configured);
		cmbFuente.SelectedItem = configured;
		numTamanoFuente.Value = configuredSize;

		List<string> cacheada = _fuentesMonoCache;
		if (cacheada != null)
		{
			AplicarFuentes(cacheada);
			return;
		}

		Task<List<string>> tarea = _tareaFuentes;
		if (tarea == null)
		{
			tarea = Task.Run(new Func<List<string>>(EnumerarFuentesMonoespaciadas));
			_tareaFuentes = tarea;
		}

		tarea.ContinueWith(delegate(Task<List<string>> t)
		{
			List<string> resultado = null;
			if (t.IsFaulted || t.IsCanceled)
			{
				resultado = new List<string> { configured };
			}
			else
			{
				resultado = t.Result;
			}
			_fuentesMonoCache = resultado;
			if (IsDisposed || !IsHandleCreated)
			{
				return;
			}
			try
			{
				BeginInvoke(new Action<List<string>>(AplicarFuentes), resultado);
			}
			catch (Exception)
			{
			}
		}, TaskScheduler.Default);
	}

	internal static List<string> EnumerarFuentesMonoespaciadas()
	{
		List<string> lista = new List<string>();
		HashSet<string> vistos = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
		FontFamily[] families = FontFamily.Families;
		foreach (FontFamily fontFamily in families)
		{
			if (fontFamily.IsStyleAvailable(FontStyle.Regular))
			{
				Font font = new Font(fontFamily, 10f);
				try
				{
					if (TextRenderer.MeasureText("W", font).Width == TextRenderer.MeasureText("I", font).Width)
					{
						string nombre = fontFamily.Name;
						if (!string.IsNullOrEmpty(nombre) && vistos.Add(nombre))
						{
							lista.Add(nombre);
						}
					}
				}
				finally
				{
					font.Dispose();
				}
			}
		}
		return lista;
	}

	private void AplicarFuentes(List<string> fuentes)
	{
		if (fuentes == null)
		{
			return;
		}
		string sel = cmbFuente.SelectedItem?.ToString();
		cmbFuente.Items.Clear();
		foreach (string fuente in fuentes)
		{
			cmbFuente.Items.Add(fuente);
		}
		if (sel != null && !fuentes.Contains(sel))
		{
			cmbFuente.Items.Add(sel);
		}
		cmbFuente.SelectedItem = sel;
	}

	private void frmConfiguracion_Load(object sender, EventArgs e)
	{
		FormBorderStyle = FormBorderStyle.FixedDialog;
		MaximizeBox = false;
		MinimizeBox = false;
		ControlBox = true;
		StartPosition = FormStartPosition.CenterScreen;
		ListImpresoras.View = View.Details;
		ListImpresoras.Columns.Add("Impresoras Disponibles", 300);
		string left = modConexion.ObtenerConfiguracion("tipo_impresion");
		if (Operators.CompareString(left, "carta", TextCompare: false) == 0)
		{
			rbtnCarta.Checked = true;
		}
		else
		{
			rbtnTicket.Checked = true;
		}
		string left2 = modConexion.ObtenerConfiguracion("imprimir_al_guardar");
		chkImprimirAlGuardar.Checked = Operators.CompareString(left2, "true", TextCompare: false) == 0;
		CargarTipos();
		ActiveControl = ListImpresoras;
	}

	private void frmConfiguracion_Shown(object sender, EventArgs e)
	{
		CargarImpresoras();
		SeleccionarImpresoraActual();
		IniciarCargaFuentes();
	}

	private void CargarImpresoras()
	{
		ListImpresoras.Items.Clear();
		foreach (object installedPrinter in PrinterSettings.InstalledPrinters)
		{
			string text = Conversions.ToString(installedPrinter);
			ListViewItem value = new ListViewItem(text);
			ListImpresoras.Items.Add(value);
		}
	}

	private void SeleccionarImpresoraActual()
	{
		string text = modConexion.ObtenerConfiguracion("impresora");
		seleccionada.Text = (string.IsNullOrEmpty(text) ? "Ninguna impresora seleccionada" : text);
		if (string.IsNullOrEmpty(text))
		{
			return;
		}
		foreach (ListViewItem item in ListImpresoras.Items)
		{
			if (Operators.CompareString(item.Text, text, TextCompare: false) == 0)
			{
				item.Selected = true;
				break;
			}
		}
	}

	private void ListImpresoras_SelectedIndexChanged(object sender, EventArgs e)
	{
		if (ListImpresoras.SelectedItems.Count > 0)
		{
			seleccionada.Text = ListImpresoras.SelectedItems[0].Text;
		}
	}

	private void btnSeleccionarImpresora_Click(object sender, EventArgs e)
	{
		if (ListImpresoras.SelectedItems.Count == 0)
		{
			MessageBox.Show("Por favor seleccione una impresora de la lista", "Selección requerida", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			return;
		}
		string text = ListImpresoras.SelectedItems[0].Text;
		try
		{
			if (cmbFuente.SelectedItem != null)
			{
				string valor = cmbFuente.SelectedItem.ToString() + "," + numTamanoFuente.Value;
				modConexion.GuardarConfiguracion("fuente_ticket", valor);
			}
			string valor2 = (rbtnCarta.Checked ? "carta" : "ticket");
			modConexion.GuardarConfiguracion("tipo_impresion", valor2);
			modConexion.GuardarConfiguracion("impresora", text);
			string valor3 = (chkImprimirAlGuardar.Checked ? "true" : "false");
			modConexion.GuardarConfiguracion("imprimir_al_guardar", valor3);
			seleccionada.Text = text;
			MessageBox.Show("Impresora '" + text + "' configurada como predeterminada", "Configuración guardada", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
			DialogResult = DialogResult.OK;
			Close();
		}
		catch (Exception ex)
		{
			ProjectData.SetProjectError(ex);
			Exception ex2 = ex;
			MessageBox.Show("Error al guardar la configuración: " + ex2.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
			ProjectData.ClearProjectError();
		}
	}

	private void CargarTipos()
	{
		dgvTiposEquipo.Rows.Clear();
		foreach (TipoEquipo tipo in TipoEquipoCatalog.ObtenerTipos())
		{
			int indice = dgvTiposEquipo.Rows.Add(tipo.Nombre, tipo.Activo == 1 ? "Activo" : "Inactivo");
			dgvTiposEquipo.Rows[indice].Tag = tipo.Id;
			if (TipoEquipoCatalog.IgualNombre(tipo.Nombre, "Otros"))
			{
				dgvTiposEquipo.Rows[indice].DefaultCellStyle.Font = new Font(dgvTiposEquipo.Font, FontStyle.Bold);
			}
		}
	}

	private TipoEquipo ObtenerTipoSeleccionado()
	{
		if (dgvTiposEquipo.SelectedRows.Count == 0)
		{
			return null;
		}
		DataGridViewRow fila = dgvTiposEquipo.SelectedRows[0];
		if (fila.Tag == null)
		{
			return null;
		}
		int id = Convert.ToInt32(fila.Tag);
		foreach (TipoEquipo tipo in TipoEquipoCatalog.ObtenerTipos())
		{
			if (tipo.Id == id)
			{
				return tipo;
			}
		}
		return null;
	}

	private void btnAgregarTipo_Click(object sender, EventArgs e)
	{
		string nombreTrim = TipoEquipoCatalog.NormalizarNombre(txtNuevoNombreTipo.Text);
		if (Operators.CompareString(nombreTrim, "", TextCompare: false) == 0)
		{
			MessageBox.Show("Debe ingresar el nombre del tipo de equipo.", "Tipo de equipo", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			txtNuevoNombreTipo.Focus();
			return;
		}
		TipoEquipo existente = TipoEquipoCatalog.BuscarTipo(nombreTrim);
		if (existente == null)
		{
			TipoEquipoCatalog.Agregar(nombreTrim);
			CargarTipos();
			return;
		}
		if (existente.Activo == 1)
		{
			MessageBox.Show("El tipo '" + existente.Nombre + "' ya existe.", "Tipo de equipo", MessageBoxButtons.OK, MessageBoxIcon.Information);
			return;
		}
		DialogResult r = MessageBox.Show("El tipo '" + existente.Nombre + "' está inactivo. ¿Desea reactivarlo?", "Tipo inactivo", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);
		if (r == DialogResult.Yes)
		{
			TipoEquipoCatalog.Activar(existente.Id);
			CargarTipos();
		}
	}

	private void btnRenombrarTipo_Click(object sender, EventArgs e)
	{
		TipoEquipo tipo = ObtenerTipoSeleccionado();
		if (tipo == null)
		{
			MessageBox.Show("Seleccione un tipo de equipo.", "Renombrar", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			return;
		}
		if (TipoEquipoCatalog.IgualNombre(tipo.Nombre, "Otros"))
		{
			MessageBox.Show("El tipo 'Otros' no puede renombrarse.", "Renombrar", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			return;
		}
		if (TipoEquipoCatalog.ContarOrdenesPorTipo(tipo.Nombre) > 0)
		{
			MessageBox.Show("Este tipo está siendo utilizado por órdenes existentes.\r\nDesactívelo y cree un nuevo tipo si necesita reemplazarlo.", "Renombrar", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			return;
		}
		string nuevoTrim = TipoEquipoCatalog.NormalizarNombre(txtNuevoNombreTipo.Text);
		if (Operators.CompareString(nuevoTrim, "", TextCompare: false) == 0)
		{
			MessageBox.Show("Debe ingresar el nuevo nombre.", "Renombrar", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			txtNuevoNombreTipo.Focus();
			return;
		}
		if (TipoEquipoCatalog.IgualNombre(nuevoTrim, "Otros"))
		{
			MessageBox.Show("No puede renombrarse un tipo a 'Otros'.", "Renombrar", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			return;
		}
		TipoEquipo duplicado = TipoEquipoCatalog.BuscarTipo(nuevoTrim);
		if (duplicado != null && duplicado.Id != tipo.Id)
		{
			MessageBox.Show("Ya existe un tipo denominado '" + duplicado.Nombre + "'.", "Renombrar", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			return;
		}
		TipoEquipoCatalog.Renombrar(tipo.Id, nuevoTrim);
		CargarTipos();
	}

	private void btnActivarTipo_Click(object sender, EventArgs e)
	{
		TipoEquipo tipo = ObtenerTipoSeleccionado();
		if (tipo == null)
		{
			MessageBox.Show("Seleccione un tipo de equipo.", "Activar", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			return;
		}
		TipoEquipoCatalog.Activar(tipo.Id);
		CargarTipos();
	}

	private void btnDesactivarTipo_Click(object sender, EventArgs e)
	{
		TipoEquipo tipo = ObtenerTipoSeleccionado();
		if (tipo == null)
		{
			MessageBox.Show("Seleccione un tipo de equipo.", "Desactivar", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			return;
		}
		if (TipoEquipoCatalog.IgualNombre(tipo.Nombre, "Otros"))
		{
			MessageBox.Show("El tipo 'Otros' no puede desactivarse.", "Desactivar", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			return;
		}
		TipoEquipoCatalog.Desactivar(tipo.Id);
		CargarTipos();
	}

	private void seleccionada_Click(object sender, EventArgs e)
	{
	}
}
