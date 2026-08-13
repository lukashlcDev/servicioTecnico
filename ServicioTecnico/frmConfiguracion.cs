using System;
using System.Drawing;
using System.Drawing.Printing;
using System.Windows.Forms;
using Microsoft.VisualBasic;
using Microsoft.VisualBasic.CompilerServices;

namespace ServicioTecnico;

public partial class frmConfiguracion : Form
{
	public frmConfiguracion()
	{
		base.Load += frmConfiguracion_Load;
		InitializeComponent();
	}

	private void CargarFuentesMonoespaciadas()
	{
		cmbFuente.Items.Clear();
		FontFamily[] families = FontFamily.Families;
		foreach (FontFamily fontFamily in families)
		{
			if (fontFamily.IsStyleAvailable(FontStyle.Regular))
			{
				Font font = new Font(fontFamily, 10f);
				if (TextRenderer.MeasureText("W", font).Width == TextRenderer.MeasureText("I", font).Width)
				{
					cmbFuente.Items.Add(fontFamily.Name);
				}
			}
		}
		string text = modConexion.ObtenerConfiguracion("fuente_ticket");
		if (!string.IsNullOrEmpty(text))
		{
			string[] array = text.Split(',');
			if (array.Length > 0)
			{
				cmbFuente.SelectedItem = array[0];
			}
			if (array.Length > 1)
			{
				numTamanoFuente.Value = new decimal(Conversion.Val(array[1]));
			}
		}
		else
		{
			cmbFuente.SelectedItem = "Courier New";
			numTamanoFuente.Value = 8m;
		}
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
		CargarImpresoras();
		SeleccionarImpresoraActual();
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
		CargarFuentesMonoespaciadas();
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

	private void seleccionada_Click(object sender, EventArgs e)
	{
	}
}
