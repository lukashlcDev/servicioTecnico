using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace ServicioTecnico;

public partial class frmCondicionesServicio : Form
{
	private IContainer components;

	internal TextBox txtCondiciones;
	internal Button btnGuardar;
	internal Button btnCancelar;
	internal Button btnBorrar;
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
		this.txtCondiciones = new System.Windows.Forms.TextBox();
		this.btnGuardar = new System.Windows.Forms.Button();
		this.btnCancelar = new System.Windows.Forms.Button();
		this.btnBorrar = new System.Windows.Forms.Button();
		this.TextBox1 = new System.Windows.Forms.TextBox();
		this.SuspendLayout();
		this.txtCondiciones.Location = new System.Drawing.Point(12, 47);
		this.txtCondiciones.Multiline = true;
		this.txtCondiciones.Name = "txtCondiciones";
		this.txtCondiciones.Size = new System.Drawing.Size(532, 287);
		this.txtCondiciones.TabIndex = 0;
		this.btnGuardar.BackColor = System.Drawing.Color.LightBlue;
		this.btnGuardar.Location = new System.Drawing.Point(342, 340);
		this.btnGuardar.Name = "btnGuardar";
		this.btnGuardar.Size = new System.Drawing.Size(202, 23);
		this.btnGuardar.TabIndex = 1;
		this.btnGuardar.Text = "Guardar";
		this.btnGuardar.UseVisualStyleBackColor = false;
		this.btnCancelar.BackColor = System.Drawing.Color.LightBlue;
		this.btnCancelar.Location = new System.Drawing.Point(12, 340);
		this.btnCancelar.Name = "btnCancelar";
		this.btnCancelar.Size = new System.Drawing.Size(75, 23);
		this.btnCancelar.TabIndex = 2;
		this.btnCancelar.Text = "Cancelar";
		this.btnCancelar.UseVisualStyleBackColor = false;
		this.btnBorrar.BackColor = System.Drawing.Color.LightBlue;
		this.btnBorrar.Location = new System.Drawing.Point(93, 340);
		this.btnBorrar.Name = "btnBorrar";
		this.btnBorrar.Size = new System.Drawing.Size(75, 23);
		this.btnBorrar.TabIndex = 3;
		this.btnBorrar.Text = "Borrar";
		this.btnBorrar.UseVisualStyleBackColor = false;
		this.TextBox1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.TextBox1.BackColor = System.Drawing.Color.LightBlue;
		this.TextBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.TextBox1.Location = new System.Drawing.Point(0, 0);
		this.TextBox1.Name = "TextBox1";
		this.TextBox1.ReadOnly = true;
		this.TextBox1.Size = new System.Drawing.Size(557, 29);
		this.TextBox1.TabIndex = 37;
		this.TextBox1.Text = "CONDICIONES DEL SERVICIO";
		this.TextBox1.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		this.ClientSize = new System.Drawing.Size(556, 375);
		this.Controls.Add(this.TextBox1);
		this.Controls.Add(this.btnBorrar);
		this.Controls.Add(this.btnCancelar);
		this.Controls.Add(this.btnGuardar);
		this.Controls.Add(this.txtCondiciones);
		this.Name = "frmCondicionesServicio";
		this.Text = "Form1";
		this.btnGuardar.Click += new System.EventHandler(this.btnGuardar_Click);
		this.btnCancelar.Click += new System.EventHandler(this.btnCancelar_Click);
		this.btnBorrar.Click += new System.EventHandler(this.btnBorrar_Click);
		this.ResumeLayout(false);
		this.PerformLayout();
	}
}
