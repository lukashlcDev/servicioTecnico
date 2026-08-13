using System;
using System.ComponentModel;
using System.Data;
using System.Data.SQLite;
using System.Runtime.CompilerServices;
using System.Windows.Forms;
using Microsoft.VisualBasic.CompilerServices;

namespace ServicioTecnico;

public partial class frmClientes : Form
{
	public Cliente ClienteSeleccionado { get; set; }

	public frmClientes()
	{
		base.Load += frmClientes_Load;
		InitializeComponent();
	}

	private void frmClientes_Load(object sender, EventArgs e)
	{
		ConfigurarDataGridView();
		CargarClientes();
		txtBuscar.Focus();
	}

	private void ConfigurarDataGridView()
	{
		DataGridView dataGridView = dgvClientes;
		dataGridView.AutoGenerateColumns = false;
		dataGridView.AllowUserToAddRows = false;
		dataGridView.AllowUserToDeleteRows = false;
		dataGridView.ReadOnly = true;
		dataGridView.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
		dataGridView.RowHeadersVisible = false;
		dataGridView.MultiSelect = false;
		dataGridView.Columns.Clear();
		DataGridViewTextBoxColumn dataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
		dataGridViewTextBoxColumn.Name = "colId";
		dataGridViewTextBoxColumn.DataPropertyName = "id_cliente";
		dataGridViewTextBoxColumn.HeaderText = "ID";
		dataGridViewTextBoxColumn.Visible = false;
		DataGridViewTextBoxColumn dataGridViewTextBoxColumn2 = dataGridViewTextBoxColumn;
		dataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
		dataGridViewTextBoxColumn.Name = "colNombre";
		dataGridViewTextBoxColumn.DataPropertyName = "nombre";
		dataGridViewTextBoxColumn.HeaderText = "Nombre";
		dataGridViewTextBoxColumn.Width = 200;
		DataGridViewTextBoxColumn dataGridViewTextBoxColumn3 = dataGridViewTextBoxColumn;
		dataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
		dataGridViewTextBoxColumn.Name = "colDocumento";
		dataGridViewTextBoxColumn.DataPropertyName = "documento";
		dataGridViewTextBoxColumn.HeaderText = "Documento";
		dataGridViewTextBoxColumn.Width = 100;
		DataGridViewTextBoxColumn dataGridViewTextBoxColumn4 = dataGridViewTextBoxColumn;
		dataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
		dataGridViewTextBoxColumn.Name = "colTelefono";
		dataGridViewTextBoxColumn.DataPropertyName = "telefono";
		dataGridViewTextBoxColumn.HeaderText = "Teléfono";
		dataGridViewTextBoxColumn.Width = 100;
		DataGridViewTextBoxColumn dataGridViewTextBoxColumn5 = dataGridViewTextBoxColumn;
		dataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
		dataGridViewTextBoxColumn.Name = "colDireccion";
		dataGridViewTextBoxColumn.DataPropertyName = "direccion";
		dataGridViewTextBoxColumn.HeaderText = "Dirección";
		dataGridViewTextBoxColumn.Width = 200;
		dataGridViewTextBoxColumn.Visible = false;
		DataGridViewTextBoxColumn dataGridViewTextBoxColumn6 = dataGridViewTextBoxColumn;
		dataGridView.Columns.AddRange(dataGridViewTextBoxColumn2, dataGridViewTextBoxColumn3, dataGridViewTextBoxColumn4, dataGridViewTextBoxColumn5, dataGridViewTextBoxColumn6);
		dataGridView = null;
	}

	private void CargarClientes(string filtro = "")
	{
		try
		{
			string text = "SELECT id_cliente, nombre, direccion, documento, telefono FROM clientes";
			if (!string.IsNullOrEmpty(filtro))
			{
				text += " WHERE nombre LIKE @filtro OR documento LIKE @filtro OR telefono LIKE @filtro";
			}
			text += " ORDER BY nombre";
			using SQLiteConnection sQLiteConnection = new SQLiteConnection("Data Source=" + modConexion.rutaDB);
			using SQLiteCommand sQLiteCommand = new SQLiteCommand(text, sQLiteConnection);
			if (!string.IsNullOrEmpty(filtro))
			{
				sQLiteCommand.Parameters.AddWithValue("@filtro", "%" + filtro + "%");
			}
			sQLiteConnection.Open();
			DataTable dataTable = new DataTable();
			dataTable.Load(sQLiteCommand.ExecuteReader());
			dgvClientes.DataSource = dataTable;
		}
		catch (Exception ex)
		{
			ProjectData.SetProjectError(ex);
			Exception ex2 = ex;
			MessageBox.Show("Error al cargar clientes: " + ex2.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
			ProjectData.ClearProjectError();
		}
	}

	private void btnNuevo_Click(object sender, EventArgs e)
	{
		using frmClienteDetalle frmClienteDetalle2 = new frmClienteDetalle();
		if (frmClienteDetalle2.ShowDialog() == DialogResult.OK)
		{
			CargarClientes(txtBuscar.Text);
		}
	}

	private void btnEditar_Click(object sender, EventArgs e)
	{
		if (dgvClientes.SelectedRows.Count == 0)
		{
			MessageBox.Show("Seleccione un cliente para editar.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			return;
		}
		int idCliente = Convert.ToInt32(RuntimeHelpers.GetObjectValue(dgvClientes.SelectedRows[0].Cells["colId"].Value));
		using frmClienteDetalle frmClienteDetalle2 = new frmClienteDetalle(idCliente);
		if (frmClienteDetalle2.ShowDialog() == DialogResult.OK)
		{
			CargarClientes(txtBuscar.Text);
		}
	}

	private void btnEliminar_Click(object sender, EventArgs e)
	{
		if (dgvClientes.SelectedRows.Count == 0)
		{
			MessageBox.Show("Seleccione un cliente para eliminar.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			return;
		}
		int num = Convert.ToInt32(RuntimeHelpers.GetObjectValue(dgvClientes.SelectedRows[0].Cells["colId"].Value));
		string text = dgvClientes.SelectedRows[0].Cells["colNombre"].Value.ToString();
		if (MessageBox.Show("¿Está seguro que desea eliminar al cliente: " + text + "?", "Confirmar eliminación", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
		{
			return;
		}
		try
		{
			using SQLiteConnection sQLiteConnection = new SQLiteConnection("Data Source=" + modConexion.rutaDB);
			sQLiteConnection.Open();
			using (SQLiteCommand sQLiteCommand = new SQLiteCommand("SELECT COUNT(*) FROM ordenes WHERE id_cliente = @id", sQLiteConnection))
			{
				sQLiteCommand.Parameters.AddWithValue("@id", num);
				if (Convert.ToInt32(RuntimeHelpers.GetObjectValue(sQLiteCommand.ExecuteScalar())) > 0)
				{
					MessageBox.Show("No se puede eliminar el cliente porque tiene órdenes asociadas.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
					return;
				}
			}
			using SQLiteCommand sQLiteCommand2 = new SQLiteCommand("DELETE FROM clientes WHERE id_cliente = @id", sQLiteConnection);
			sQLiteCommand2.Parameters.AddWithValue("@id", num);
			sQLiteCommand2.ExecuteNonQuery();
			MessageBox.Show("Cliente eliminado correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
			CargarClientes(txtBuscar.Text);
		}
		catch (Exception ex)
		{
			ProjectData.SetProjectError(ex);
			Exception ex2 = ex;
			MessageBox.Show("Error al eliminar cliente: " + ex2.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
			ProjectData.ClearProjectError();
		}
	}

	private void btnSeleccionar_Click(object sender, EventArgs e)
	{
		SeleccionarCliente();
	}

	private void dgvClientes_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
	{
		if (e.RowIndex >= 0)
		{
			SeleccionarCliente();
		}
	}

	private void SeleccionarCliente()
	{
		if (dgvClientes.SelectedRows.Count == 0)
		{
			MessageBox.Show("Seleccione un cliente para continuar.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			return;
		}
		DataGridViewRow dataGridViewRow = dgvClientes.SelectedRows[0];
		ClienteSeleccionado = new Cliente(Convert.ToInt32(RuntimeHelpers.GetObjectValue(dataGridViewRow.Cells["colId"].Value)), dataGridViewRow.Cells["colNombre"].Value.ToString(), dataGridViewRow.Cells["colDireccion"].Value.ToString(), dataGridViewRow.Cells["colDocumento"].Value.ToString(), dataGridViewRow.Cells["colTelefono"].Value.ToString());
		DialogResult = DialogResult.OK;
		Close();
	}

	private void txtBuscar_TextChanged(object sender, EventArgs e)
	{
		CargarClientes(txtBuscar.Text);
	}

	private void txtBuscar_KeyDown(object sender, KeyEventArgs e)
	{
		if (e.KeyCode == Keys.Return && dgvClientes.Rows.Count > 0)
		{
			dgvClientes.Focus();
			dgvClientes.Rows[0].Selected = true;
		}
	}
}
