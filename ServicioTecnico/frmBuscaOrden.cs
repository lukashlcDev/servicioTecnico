using System;
using System.ComponentModel;
using System.Data;
using System.Data.SQLite;
using System.Runtime.CompilerServices;
using System.Windows.Forms;
using Microsoft.VisualBasic.CompilerServices;

namespace ServicioTecnico;

public partial class frmBuscaOrden : Form
{
	public int OrdenSeleccionada { get; set; }

	public frmBuscaOrden()
	{
		base.Load += frmBuscaOrden_Load;
		OrdenSeleccionada = -1;
		InitializeComponent();
	}

	private void frmBuscaOrden_Load(object sender, EventArgs e)
	{
		txtBuscar.Focus();
		rbDocumento.Checked = true;
	}

	private void btnBuscar_Click(object sender, EventArgs e)
	{
		BuscarOrdenes();
	}

	private void txtBuscar_KeyDown(object sender, KeyEventArgs e)
	{
		if (e.KeyCode == Keys.Return)
		{
			BuscarOrdenes();
		}
	}

	private void BuscarOrdenes()
	{
		string text = txtBuscar.Text.Trim();
		if (string.IsNullOrEmpty(text))
		{
			MessageBox.Show("Ingrese un criterio de búsqueda", "Búsqueda", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
			return;
		}
		try
		{
			using SQLiteConnection sQLiteConnection = new SQLiteConnection("Data Source=" + modConexion.rutaDB);
			sQLiteConnection.Open();
			string text2 = "SELECT o.id_orden, o.fecha, c.nombre, c.documento, c.telefono, o.tipo_equipo, o.estado_entrega, o.marca, o.modelo FROM ordenes o INNER JOIN clientes c ON o.id_cliente = c.id_cliente WHERE ";
			text2 = (rbDocumento.Checked ? (text2 + "c.documento LIKE @criterio") : ((!rbTelefono.Checked) ? (text2 + "c.nombre LIKE @criterio") : (text2 + "c.telefono LIKE @criterio")));
			text2 += " ORDER BY o.fecha DESC";
			using SQLiteCommand sQLiteCommand = new SQLiteCommand(text2, sQLiteConnection);
			sQLiteCommand.Parameters.AddWithValue("@criterio", "%" + text + "%");
			using SQLiteDataAdapter sQLiteDataAdapter = new SQLiteDataAdapter(sQLiteCommand);
			DataTable dataTable = new DataTable();
			sQLiteDataAdapter.Fill(dataTable);
			if (dataTable.Rows.Count > 0)
			{
				dgvOrdenes.DataSource = dataTable;
				ConfigurarGrid();
			}
			else
			{
				dgvOrdenes.DataSource = null;
				MessageBox.Show("No se encontraron órdenes con el criterio especificado", "Búsqueda", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
			}
		}
		catch (Exception ex)
		{
			ProjectData.SetProjectError(ex);
			Exception ex2 = ex;
			MessageBox.Show("Error al buscar órdenes: " + ex2.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
			ProjectData.ClearProjectError();
		}
	}

	private void ConfigurarGrid()
	{
		DataGridView dataGridView = dgvOrdenes;
		dataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
		dataGridView.AllowUserToAddRows = false;
		dataGridView.AllowUserToDeleteRows = false;
		dataGridView.ReadOnly = true;
		dataGridView.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
		dataGridView.RowHeadersVisible = false;
		if (dataGridView.Columns.Count > 0)
		{
			dataGridView.Columns["id_orden"].HeaderText = "N° Orden";
			dataGridView.Columns["fecha"].HeaderText = "Fecha";
			dataGridView.Columns["nombre"].HeaderText = "Cliente";
			dataGridView.Columns["documento"].HeaderText = "Documento";
			dataGridView.Columns["telefono"].HeaderText = "Teléfono";
			dataGridView.Columns["tipo_equipo"].HeaderText = "Tipo";
			dataGridView.Columns["marca"].HeaderText = "Marca";
			dataGridView.Columns["modelo"].HeaderText = "Modelo";
			dataGridView.Columns["estado_entrega"].HeaderText = "Estado";
			dataGridView.Columns["fecha"].DefaultCellStyle.Format = "dd/MM/yyyy";
			dataGridView.Columns["id_orden"].FillWeight = 70f;
			dataGridView.Columns["fecha"].FillWeight = 80f;
			dataGridView.Columns["tipo_equipo"].FillWeight = 80f;
			dataGridView.Columns["estado_entrega"].FillWeight = 100f;
		}
		dataGridView = null;
	}

	private void btnSeleccionar_Click(object sender, EventArgs e)
	{
		if (dgvOrdenes.SelectedRows.Count > 0)
		{
			OrdenSeleccionada = Convert.ToInt32(RuntimeHelpers.GetObjectValue(dgvOrdenes.SelectedRows[0].Cells["id_orden"].Value));
			DialogResult = DialogResult.OK;
			Close();
		}
		else
		{
			MessageBox.Show("Seleccione una orden de la lista", "Selección", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
		}
	}

	private void dgvOrdenes_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
	{
		if (e.RowIndex >= 0)
		{
			btnSeleccionar.PerformClick();
		}
	}

	private void btnCancelar_Click(object sender, EventArgs e)
	{
		DialogResult = DialogResult.Cancel;
		Close();
	}
}
