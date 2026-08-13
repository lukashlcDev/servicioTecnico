using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace ServicioTecnico;

public partial class frmClienteDetalle : Form
{
	private IContainer components;

	internal TextBox txtNombre;
	internal TextBox txtDireccion;
	internal TextBox txtDocumento;
	internal TextBox txtTelefono;
	internal Button btnGuardar;
	internal Button btnCancelar;
	internal Label Label1;
	internal Label Label2;
	internal Label Label3;
	internal Label Label4;

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
		this.txtNombre = new System.Windows.Forms.TextBox();
		this.txtDireccion = new System.Windows.Forms.TextBox();
		this.txtDocumento = new System.Windows.Forms.TextBox();
		this.txtTelefono = new System.Windows.Forms.TextBox();
		this.btnGuardar = new System.Windows.Forms.Button();
		this.btnCancelar = new System.Windows.Forms.Button();
		this.Label1 = new System.Windows.Forms.Label();
		this.Label2 = new System.Windows.Forms.Label();
		this.Label3 = new System.Windows.Forms.Label();
		this.Label4 = new System.Windows.Forms.Label();
		this.SuspendLayout();
		this.txtNombre.Location = new System.Drawing.Point(12, 24);
		this.txtNombre.Name = "txtNombre";
		this.txtNombre.Size = new System.Drawing.Size(260, 20);
		this.txtNombre.TabIndex = 0;
		this.txtDireccion.Location = new System.Drawing.Point(12, 63);
		this.txtDireccion.Name = "txtDireccion";
		this.txtDireccion.Size = new System.Drawing.Size(260, 20);
		this.txtDireccion.TabIndex = 1;
		this.txtDocumento.Location = new System.Drawing.Point(12, 102);
		this.txtDocumento.Name = "txtDocumento";
		this.txtDocumento.Size = new System.Drawing.Size(134, 20);
		this.txtDocumento.TabIndex = 2;
		this.txtTelefono.Location = new System.Drawing.Point(12, 143);
		this.txtTelefono.Name = "txtTelefono";
		this.txtTelefono.Size = new System.Drawing.Size(134, 20);
		this.txtTelefono.TabIndex = 3;
		this.btnGuardar.BackColor = System.Drawing.Color.LightBlue;
		this.btnGuardar.Location = new System.Drawing.Point(167, 102);
		this.btnGuardar.Name = "btnGuardar";
		this.btnGuardar.Size = new System.Drawing.Size(105, 100);
		this.btnGuardar.TabIndex = 4;
		this.btnGuardar.Text = "GUARDAR";
		this.btnGuardar.UseVisualStyleBackColor = false;
		this.btnCancelar.BackColor = System.Drawing.Color.LightBlue;
		this.btnCancelar.Location = new System.Drawing.Point(12, 179);
		this.btnCancelar.Name = "btnCancelar";
		this.btnCancelar.Size = new System.Drawing.Size(134, 23);
		this.btnCancelar.TabIndex = 5;
		this.btnCancelar.Text = "CANCELAR";
		this.btnCancelar.UseVisualStyleBackColor = false;
		this.Label1.AutoSize = true;
		this.Label1.Location = new System.Drawing.Point(12, 9);
		this.Label1.Name = "Label1";
		this.Label1.Size = new System.Drawing.Size(202, 13);
		this.Label1.TabIndex = 6;
		this.Label1.Text = "NOMBRES Y APELLIDOS (FULL NAME)";
		this.Label2.AutoSize = true;
		this.Label2.Location = new System.Drawing.Point(12, 47);
		this.Label2.Name = "Label2";
		this.Label2.Size = new System.Drawing.Size(66, 13);
		this.Label2.TabIndex = 7;
		this.Label2.Text = "DIRECCION";
		this.Label3.AutoSize = true;
		this.Label3.Location = new System.Drawing.Point(9, 86);
		this.Label3.Name = "Label3";
		this.Label3.Size = new System.Drawing.Size(134, 13);
		this.Label3.TabIndex = 8;
		this.Label3.Text = "CEDULA O DOCUMENTO";
		this.Label4.AutoSize = true;
		this.Label4.Location = new System.Drawing.Point(9, 127);
		this.Label4.Name = "Label4";
		this.Label4.Size = new System.Drawing.Size(137, 13);
		this.Label4.TabIndex = 9;
		this.Label4.Text = "TELEFONO O CORREO E.";
		this.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		this.ClientSize = new System.Drawing.Size(287, 215);
		this.ControlBox = false;
		this.Controls.Add(this.Label4);
		this.Controls.Add(this.Label3);
		this.Controls.Add(this.Label2);
		this.Controls.Add(this.Label1);
		this.Controls.Add(this.btnCancelar);
		this.Controls.Add(this.btnGuardar);
		this.Controls.Add(this.txtTelefono);
		this.Controls.Add(this.txtDocumento);
		this.Controls.Add(this.txtDireccion);
		this.Controls.Add(this.txtNombre);
		this.Name = "frmClienteDetalle";
		this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
		this.Text = "Form1";
		this.btnGuardar.Click += new System.EventHandler(this.btnGuardar_Click);
		this.btnCancelar.Click += new System.EventHandler(this.btnCancelar_Click);
		this.ResumeLayout(false);
		this.PerformLayout();
	}
}
