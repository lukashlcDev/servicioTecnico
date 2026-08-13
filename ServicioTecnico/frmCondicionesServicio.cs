using System;
using System.Drawing;
using System.Windows.Forms;
using Microsoft.VisualBasic.CompilerServices;

namespace ServicioTecnico;

public partial class frmCondicionesServicio : Form
{
	public frmCondicionesServicio()
	{
		base.Load += frmCondicionesServicio_Load;
		InitializeComponent();
	}

	private void frmCondicionesServicio_Load(object sender, EventArgs e)
	{
		txtCondiciones.Multiline = true;
		txtCondiciones.ScrollBars = ScrollBars.Vertical;
		txtCondiciones.Font = new Font("Consolas", 10f);
		txtCondiciones.Text = modConexion.ObtenerCondicionesServicio();
		txtCondiciones.Select();
	}

	private void btnGuardar_Click(object sender, EventArgs e)
	{
		try
		{
			if (string.IsNullOrWhiteSpace(txtCondiciones.Text))
			{
				MessageBox.Show("Por favor ingrese las condiciones del servicio.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				return;
			}
			modConexion.GuardarCondicionesServicio(txtCondiciones.Text);
			DialogResult = DialogResult.OK;
			Close();
		}
		catch (Exception ex)
		{
			ProjectData.SetProjectError(ex);
			Exception ex2 = ex;
			MessageBox.Show("Error al guardar las condiciones: " + ex2.Message + " ", "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
			ProjectData.ClearProjectError();
		}
	}

	private void btnCancelar_Click(object sender, EventArgs e)
	{
		DialogResult = DialogResult.Cancel;
		Close();
	}

	private void btnBorrar_Click(object sender, EventArgs e)
	{
		if (MessageBox.Show("¿Está seguro que desea borrar todas las condiciones?", "Confirmar", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
		{
			txtCondiciones.Clear();
		}
	}
}
