using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace ServicioTecnico;

public partial class frmConfiguracion : Form
{
	private IContainer components;

	internal TextBox TextBox1;
	internal Panel Panel1;
	internal ListView ListImpresoras;
	internal Label seleccionada;
	internal Button btnSeleccionarImpresora;
	internal RadioButton rbtnTicket;
	internal RadioButton rbtnCarta;
	internal NumericUpDown numTamanoFuente;
	internal ComboBox cmbFuente;
	internal Label Label2;
	internal Label Label1;
	internal CheckBox chkImprimirAlGuardar;

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
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ServicioTecnico.frmConfiguracion));
		this.TextBox1 = new System.Windows.Forms.TextBox();
		this.Panel1 = new System.Windows.Forms.Panel();
		this.seleccionada = new System.Windows.Forms.Label();
		this.btnSeleccionarImpresora = new System.Windows.Forms.Button();
		this.ListImpresoras = new System.Windows.Forms.ListView();
		this.cmbFuente = new System.Windows.Forms.ComboBox();
		this.numTamanoFuente = new System.Windows.Forms.NumericUpDown();
		this.rbtnCarta = new System.Windows.Forms.RadioButton();
		this.rbtnTicket = new System.Windows.Forms.RadioButton();
		this.Label1 = new System.Windows.Forms.Label();
		this.Label2 = new System.Windows.Forms.Label();
		this.chkImprimirAlGuardar = new System.Windows.Forms.CheckBox();
		this.Panel1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.numTamanoFuente).BeginInit();
		this.SuspendLayout();
		this.TextBox1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.TextBox1.BackColor = System.Drawing.Color.LightBlue;
		this.TextBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.TextBox1.Location = new System.Drawing.Point(0, -1);
		this.TextBox1.Name = "TextBox1";
		this.TextBox1.ReadOnly = true;
		this.TextBox1.Size = new System.Drawing.Size(449, 29);
		this.TextBox1.TabIndex = 36;
		this.TextBox1.Text = "CONFIGURACION";
		this.TextBox1.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.Panel1.BackColor = System.Drawing.Color.FromArgb(224, 224, 224);
		this.Panel1.Controls.Add(this.chkImprimirAlGuardar);
		this.Panel1.Controls.Add(this.Label2);
		this.Panel1.Controls.Add(this.Label1);
		this.Panel1.Controls.Add(this.rbtnTicket);
		this.Panel1.Controls.Add(this.rbtnCarta);
		this.Panel1.Controls.Add(this.numTamanoFuente);
		this.Panel1.Controls.Add(this.cmbFuente);
		this.Panel1.Controls.Add(this.seleccionada);
		this.Panel1.Controls.Add(this.btnSeleccionarImpresora);
		this.Panel1.Controls.Add(this.ListImpresoras);
		this.Panel1.Location = new System.Drawing.Point(12, 34);
		this.Panel1.Name = "Panel1";
		this.Panel1.Size = new System.Drawing.Size(422, 260);
		this.Panel1.TabIndex = 37;
		this.seleccionada.AutoSize = true;
		this.seleccionada.Font = new System.Drawing.Font("Microsoft Sans Serif", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.seleccionada.Location = new System.Drawing.Point(2, 7);
		this.seleccionada.Name = "seleccionada";
		this.seleccionada.Size = new System.Drawing.Size(90, 20);
		this.seleccionada.TabIndex = 5;
		this.seleccionada.Text = "Impresora";
		this.btnSeleccionarImpresora.BackColor = System.Drawing.Color.LightBlue;
		this.btnSeleccionarImpresora.Image = (System.Drawing.Image)resources.GetObject("btnSeleccionarImpresora.Image");
		this.btnSeleccionarImpresora.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.btnSeleccionarImpresora.Location = new System.Drawing.Point(220, 212);
		this.btnSeleccionarImpresora.Name = "btnSeleccionarImpresora";
		this.btnSeleccionarImpresora.Size = new System.Drawing.Size(195, 32);
		this.btnSeleccionarImpresora.TabIndex = 4;
		this.btnSeleccionarImpresora.Text = "GUARDAR";
		this.btnSeleccionarImpresora.UseVisualStyleBackColor = false;
		this.ListImpresoras.Location = new System.Drawing.Point(3, 28);
		this.ListImpresoras.Name = "ListImpresoras";
		this.ListImpresoras.Size = new System.Drawing.Size(207, 216);
		this.ListImpresoras.TabIndex = 0;
		this.ListImpresoras.UseCompatibleStateImageBehavior = false;
		this.cmbFuente.FormattingEnabled = true;
		this.cmbFuente.Location = new System.Drawing.Point(216, 28);
		this.cmbFuente.Name = "cmbFuente";
		this.cmbFuente.Size = new System.Drawing.Size(171, 21);
		this.cmbFuente.TabIndex = 6;
		this.numTamanoFuente.Location = new System.Drawing.Point(217, 55);
		this.numTamanoFuente.Name = "numTamanoFuente";
		this.numTamanoFuente.Size = new System.Drawing.Size(48, 20);
		this.numTamanoFuente.TabIndex = 7;
		this.rbtnCarta.AutoSize = true;
		this.rbtnCarta.Location = new System.Drawing.Point(217, 94);
		this.rbtnCarta.Name = "rbtnCarta";
		this.rbtnCarta.Size = new System.Drawing.Size(135, 17);
		this.rbtnCarta.TabIndex = 8;
		this.rbtnCarta.TabStop = true;
		this.rbtnCarta.Text = "Imprimir en Media Carta";
		this.rbtnCarta.UseVisualStyleBackColor = true;
		this.rbtnTicket.AutoSize = true;
		this.rbtnTicket.Location = new System.Drawing.Point(217, 117);
		this.rbtnTicket.Name = "rbtnTicket";
		this.rbtnTicket.Size = new System.Drawing.Size(108, 17);
		this.rbtnTicket.TabIndex = 9;
		this.rbtnTicket.TabStop = true;
		this.rbtnTicket.Text = "Imprimir en Ticket";
		this.rbtnTicket.UseVisualStyleBackColor = true;
		this.Label1.AutoSize = true;
		this.Label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.Label1.Location = new System.Drawing.Point(216, 7);
		this.Label1.Name = "Label1";
		this.Label1.Size = new System.Drawing.Size(113, 20);
		this.Label1.TabIndex = 10;
		this.Label1.Text = "Fuente Letra";
		this.Label2.AutoSize = true;
		this.Label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.Label2.Location = new System.Drawing.Point(271, 55);
		this.Label2.Name = "Label2";
		this.Label2.Size = new System.Drawing.Size(120, 20);
		this.Label2.TabIndex = 11;
		this.Label2.Text = "Tamaño Letra";
		this.chkImprimirAlGuardar.AutoSize = true;
		this.chkImprimirAlGuardar.Location = new System.Drawing.Point(220, 155);
		this.chkImprimirAlGuardar.Name = "chkImprimirAlGuardar";
		this.chkImprimirAlGuardar.Size = new System.Drawing.Size(195, 17);
		this.chkImprimirAlGuardar.TabIndex = 38;
		this.chkImprimirAlGuardar.Text = "Imprimir automáticamente al guardar";
		this.chkImprimirAlGuardar.UseVisualStyleBackColor = true;
		this.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		this.ClientSize = new System.Drawing.Size(448, 306);
		this.ControlBox = false;
		this.Controls.Add(this.Panel1);
		this.Controls.Add(this.TextBox1);
		this.Name = "frmConfiguracion";
		this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
		this.Text = "Configuración";
		this.ListImpresoras.SelectedIndexChanged += new System.EventHandler(this.ListImpresoras_SelectedIndexChanged);
		this.seleccionada.Click += new System.EventHandler(this.seleccionada_Click);
		this.btnSeleccionarImpresora.Click += new System.EventHandler(this.btnSeleccionarImpresora_Click);
		this.Panel1.ResumeLayout(false);
		this.Panel1.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.numTamanoFuente).EndInit();
		this.ResumeLayout(false);
		this.PerformLayout();
	}
}
