using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace ServicioTecnico;

public partial class frmClientes : Form
{
	private IContainer components;

	internal Button btnNuevo;
	internal Button btnEditar;
	internal Button btnEliminar;
	internal Button btnSeleccionar;
	internal TextBox txtBuscar;
	internal DataGridView dgvClientes;
	internal TextBox TextBox1;

	[DebuggerNonUserCode]
	protected override void Dispose(bool disposing)
	{
		try
		{
			if (disposing && components != null)
			{
				components.Dispose();
			}
		}
		finally
		{
			base.Dispose(disposing);
		}
	}

	[System.Diagnostics.DebuggerStepThrough]
	private void InitializeComponent()
	{
		this.components = new System.ComponentModel.Container();
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ServicioTecnico.frmClientes));
		this.btnNuevo = new System.Windows.Forms.Button();
		this.btnEditar = new System.Windows.Forms.Button();
		this.btnEliminar = new System.Windows.Forms.Button();
		this.btnSeleccionar = new System.Windows.Forms.Button();
		this.txtBuscar = new System.Windows.Forms.TextBox();
		this.dgvClientes = new System.Windows.Forms.DataGridView();
		this.TextBox1 = new System.Windows.Forms.TextBox();
		((System.ComponentModel.ISupportInitialize)this.dgvClientes).BeginInit();
		this.SuspendLayout();
		this.btnNuevo.BackColor = System.Drawing.Color.LightBlue;
		this.btnNuevo.Image = (System.Drawing.Image)resources.GetObject("btnNuevo.Image");
		this.btnNuevo.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.btnNuevo.Location = new System.Drawing.Point(12, 86);
		this.btnNuevo.Name = "btnNuevo";
		this.btnNuevo.Size = new System.Drawing.Size(203, 31);
		this.btnNuevo.TabIndex = 0;
		this.btnNuevo.Text = "NUEVO";
		this.btnNuevo.UseVisualStyleBackColor = false;
		this.btnEditar.BackColor = System.Drawing.Color.LightBlue;
		this.btnEditar.Image = (System.Drawing.Image)resources.GetObject("btnEditar.Image");
		this.btnEditar.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.btnEditar.Location = new System.Drawing.Point(12, 123);
		this.btnEditar.Name = "btnEditar";
		this.btnEditar.Size = new System.Drawing.Size(203, 31);
		this.btnEditar.TabIndex = 1;
		this.btnEditar.Text = "EDITAR";
		this.btnEditar.UseVisualStyleBackColor = false;
		this.btnEliminar.BackColor = System.Drawing.Color.LightBlue;
		this.btnEliminar.Image = (System.Drawing.Image)resources.GetObject("btnEliminar.Image");
		this.btnEliminar.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.btnEliminar.Location = new System.Drawing.Point(12, 160);
		this.btnEliminar.Name = "btnEliminar";
		this.btnEliminar.Size = new System.Drawing.Size(203, 31);
		this.btnEliminar.TabIndex = 2;
		this.btnEliminar.Text = "ELIMINAR";
		this.btnEliminar.UseVisualStyleBackColor = false;
		this.btnSeleccionar.BackColor = System.Drawing.Color.LightBlue;
		this.btnSeleccionar.Image = (System.Drawing.Image)resources.GetObject("btnSeleccionar.Image");
		this.btnSeleccionar.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.btnSeleccionar.Location = new System.Drawing.Point(12, 357);
		this.btnSeleccionar.Name = "btnSeleccionar";
		this.btnSeleccionar.Size = new System.Drawing.Size(203, 31);
		this.btnSeleccionar.TabIndex = 3;
		this.btnSeleccionar.Text = "SELECCIONAR";
		this.btnSeleccionar.UseVisualStyleBackColor = false;
		this.txtBuscar.Font = new System.Drawing.Font("Microsoft Sans Serif", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtBuscar.Location = new System.Drawing.Point(12, 54);
		this.txtBuscar.Name = "txtBuscar";
		this.txtBuscar.Size = new System.Drawing.Size(203, 26);
		this.txtBuscar.TabIndex = 6;
		this.dgvClientes.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
		this.dgvClientes.Location = new System.Drawing.Point(221, 54);
		this.dgvClientes.Name = "dgvClientes";
		this.dgvClientes.Size = new System.Drawing.Size(415, 334);
		this.dgvClientes.TabIndex = 7;
		this.TextBox1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.TextBox1.BackColor = System.Drawing.Color.LightBlue;
		this.TextBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.TextBox1.Location = new System.Drawing.Point(-3, -2);
		this.TextBox1.Name = "TextBox1";
		this.TextBox1.ReadOnly = true;
		this.TextBox1.Size = new System.Drawing.Size(655, 29);
		this.TextBox1.TabIndex = 37;
		this.TextBox1.Text = "GESTION DE CLIENTES";
		this.TextBox1.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		this.ClientSize = new System.Drawing.Size(651, 402);
		this.Controls.Add(this.TextBox1);
		this.Controls.Add(this.dgvClientes);
		this.Controls.Add(this.txtBuscar);
		this.Controls.Add(this.btnSeleccionar);
		this.Controls.Add(this.btnEliminar);
		this.Controls.Add(this.btnEditar);
		this.Controls.Add(this.btnNuevo);
		this.Name = "frmClientes";
		this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
		this.Text = "Clientes";
		((System.ComponentModel.ISupportInitialize)this.dgvClientes).EndInit();
		this.btnNuevo.Click += new System.EventHandler(this.btnNuevo_Click);
		this.btnEditar.Click += new System.EventHandler(this.btnEditar_Click);
		this.btnEliminar.Click += new System.EventHandler(this.btnEliminar_Click);
		this.btnSeleccionar.Click += new System.EventHandler(this.btnSeleccionar_Click);
		this.txtBuscar.TextChanged += new System.EventHandler(this.txtBuscar_TextChanged);
		this.txtBuscar.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtBuscar_KeyDown);
		this.dgvClientes.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvClientes_CellDoubleClick);
		this.ResumeLayout(false);
		this.PerformLayout();
	}
}
