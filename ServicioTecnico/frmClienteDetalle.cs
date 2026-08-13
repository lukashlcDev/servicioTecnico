using System;
using System.Data.SQLite;
using System.Windows.Forms;
using Microsoft.VisualBasic.CompilerServices;

namespace ServicioTecnico;

public partial class frmClienteDetalle : Form
{
	private int _idCliente;

	private bool _esNuevo;

	public frmClienteDetalle(int idCliente = 0)
	{
		base.Load += frmClienteDetalle_Load_1;
		_esNuevo = true;
		InitializeComponent();
		_idCliente = idCliente;
		_esNuevo = idCliente == 0;
	}

	private void CargarDatosCliente()
	{
		try
		{
			using SQLiteConnection sQLiteConnection = new SQLiteConnection("Data Source=" + modConexion.rutaDB);
			sQLiteConnection.Open();
			using SQLiteCommand sQLiteCommand = new SQLiteCommand("SELECT nombre, direccion, documento, telefono FROM clientes WHERE id_cliente = @id", sQLiteConnection);
			sQLiteCommand.Parameters.AddWithValue("@id", _idCliente);
			using SQLiteDataReader sQLiteDataReader = sQLiteCommand.ExecuteReader();
			if (sQLiteDataReader.Read())
			{
				txtNombre.Text = sQLiteDataReader["nombre"].ToString();
				txtDireccion.Text = sQLiteDataReader["direccion"].ToString();
				txtDocumento.Text = sQLiteDataReader["documento"].ToString();
				txtTelefono.Text = sQLiteDataReader["telefono"].ToString();
			}
		}
		catch (Exception ex)
		{
			ProjectData.SetProjectError(ex);
			Exception ex2 = ex;
			MessageBox.Show("Error al cargar datos del cliente: " + ex2.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
			ProjectData.ClearProjectError();
		}
	}

	private void btnGuardar_Click(object sender, EventArgs e)
	{
		if (!ValidarCampos())
		{
			return;
		}
		try
		{
			using SQLiteConnection sQLiteConnection = new SQLiteConnection("Data Source=" + modConexion.rutaDB);
			sQLiteConnection.Open();
			if (_esNuevo)
			{
				using SQLiteCommand sQLiteCommand = new SQLiteCommand("INSERT INTO clientes (nombre, direccion, documento, telefono) VALUES (@nombre, @direccion, @documento, @telefono)", sQLiteConnection);
				AsignarParametros(sQLiteCommand);
				sQLiteCommand.ExecuteNonQuery();
			}
			else
			{
				using SQLiteCommand sQLiteCommand2 = new SQLiteCommand("UPDATE clientes SET nombre=@nombre, direccion=@direccion, documento=@documento, telefono=@telefono WHERE id_cliente=@id", sQLiteConnection);
				sQLiteCommand2.Parameters.AddWithValue("@id", _idCliente);
				AsignarParametros(sQLiteCommand2);
				sQLiteCommand2.ExecuteNonQuery();
			}
			MessageBox.Show("Cliente guardado correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
			DialogResult = DialogResult.OK;
			Close();
		}
		catch (Exception ex)
		{
			ProjectData.SetProjectError(ex);
			Exception ex2 = ex;
			MessageBox.Show("Error al guardar cliente: " + ex2.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
			ProjectData.ClearProjectError();
		}
	}

	private void AsignarParametros(SQLiteCommand cmd)
	{
		cmd.Parameters.AddWithValue("@nombre", txtNombre.Text.Trim());
		cmd.Parameters.AddWithValue("@direccion", txtDireccion.Text.Trim());
		cmd.Parameters.AddWithValue("@documento", txtDocumento.Text.Trim());
		cmd.Parameters.AddWithValue("@telefono", txtTelefono.Text.Trim());
	}

	private bool ValidarCampos()
	{
		if (string.IsNullOrWhiteSpace(txtNombre.Text))
		{
			MessageBox.Show("El nombre del cliente es obligatorio.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			txtNombre.Focus();
			return false;
		}
		return true;
	}

	private void btnCancelar_Click(object sender, EventArgs e)
	{
		DialogResult = DialogResult.Cancel;
		Close();
	}

	private void frmClienteDetalle_Load_1(object sender, EventArgs e)
	{
		if (!_esNuevo)
		{
			Text = "Editar Cliente";
			CargarDatosCliente();
		}
		else
		{
			Text = "Nuevo Cliente";
		}
	}
}
