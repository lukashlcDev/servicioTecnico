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
	internal GroupBox grpTipoEquipo;
	internal DataGridView dgvTiposEquipo;
	internal Label lblNuevoNombreTipo;
	internal TextBox txtNuevoNombreTipo;
	internal Button btnAgregarTipo;
	internal Button btnRenombrarTipo;
	internal Button btnActivarTipo;
	internal Button btnDesactivarTipo;
	internal DataGridViewTextBoxColumn colNombre;
	internal DataGridViewTextBoxColumn colEstado;
	internal TabControl tabControl;
	internal TabPage tabGeneral;
	internal TabPage tabTipos;

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
		this.grpTipoEquipo = new System.Windows.Forms.GroupBox();
		this.dgvTiposEquipo = new System.Windows.Forms.DataGridView();
		this.lblNuevoNombreTipo = new System.Windows.Forms.Label();
		this.txtNuevoNombreTipo = new System.Windows.Forms.TextBox();
		this.btnAgregarTipo = new System.Windows.Forms.Button();
		this.btnRenombrarTipo = new System.Windows.Forms.Button();
		this.btnActivarTipo = new System.Windows.Forms.Button();
		this.btnDesactivarTipo = new System.Windows.Forms.Button();
		this.colNombre = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.colEstado = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.tabControl = new System.Windows.Forms.TabControl();
		this.tabGeneral = new System.Windows.Forms.TabPage();
		this.tabTipos = new System.Windows.Forms.TabPage();
		this.Panel1.SuspendLayout();
		this.tabControl.SuspendLayout();
		this.tabGeneral.SuspendLayout();
		this.tabTipos.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.numTamanoFuente).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.dgvTiposEquipo).BeginInit();
		this.SuspendLayout();
		// 
		// TextBox1
		// 
		this.TextBox1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.TextBox1.BackColor = System.Drawing.Color.LightBlue;
		this.TextBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.TextBox1.Location = new System.Drawing.Point(0, -1);
		this.TextBox1.Name = "TextBox1";
		this.TextBox1.ReadOnly = true;
		this.TextBox1.Size = new System.Drawing.Size(478, 29);
		this.TextBox1.TabIndex = 36;
		this.TextBox1.TabStop = false;
		this.TextBox1.Text = "CONFIGURACION";
		this.TextBox1.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		// 
		// Panel1
		// 
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
		this.Panel1.Location = new System.Drawing.Point(10, 8);
		this.Panel1.Name = "Panel1";
		this.Panel1.Size = new System.Drawing.Size(422, 260);
		this.Panel1.TabIndex = 0;
		// 
		// seleccionada
		// 
		this.seleccionada.AutoSize = true;
		this.seleccionada.Font = new System.Drawing.Font("Microsoft Sans Serif", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.seleccionada.Location = new System.Drawing.Point(2, 7);
		this.seleccionada.Name = "seleccionada";
		this.seleccionada.Size = new System.Drawing.Size(90, 20);
		this.seleccionada.TabIndex = 5;
		this.seleccionada.Text = "Impresora";
		// 
		// btnSeleccionarImpresora
		// 
		this.btnSeleccionarImpresora.BackColor = System.Drawing.Color.LightBlue;
		this.btnSeleccionarImpresora.Image = (System.Drawing.Image)resources.GetObject("btnSeleccionarImpresora.Image");
		this.btnSeleccionarImpresora.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.btnSeleccionarImpresora.Location = new System.Drawing.Point(220, 212);
		this.btnSeleccionarImpresora.Name = "btnSeleccionarImpresora";
		this.btnSeleccionarImpresora.Size = new System.Drawing.Size(195, 32);
		this.btnSeleccionarImpresora.TabIndex = 4;
		this.btnSeleccionarImpresora.Text = "GUARDAR";
		this.btnSeleccionarImpresora.UseVisualStyleBackColor = false;
		// 
		// ListImpresoras
		// 
		this.ListImpresoras.Location = new System.Drawing.Point(3, 28);
		this.ListImpresoras.Name = "ListImpresoras";
		this.ListImpresoras.Size = new System.Drawing.Size(207, 216);
		this.ListImpresoras.TabIndex = 0;
		this.ListImpresoras.UseCompatibleStateImageBehavior = false;
		// 
		// cmbFuente
		// 
		this.cmbFuente.FormattingEnabled = true;
		this.cmbFuente.Location = new System.Drawing.Point(216, 28);
		this.cmbFuente.Name = "cmbFuente";
		this.cmbFuente.Size = new System.Drawing.Size(171, 21);
		this.cmbFuente.TabIndex = 6;
		// 
		// numTamanoFuente
		// 
		this.numTamanoFuente.Location = new System.Drawing.Point(217, 55);
		this.numTamanoFuente.Name = "numTamanoFuente";
		this.numTamanoFuente.Size = new System.Drawing.Size(48, 20);
		this.numTamanoFuente.TabIndex = 7;
		// 
		// rbtnCarta
		// 
		this.rbtnCarta.AutoSize = true;
		this.rbtnCarta.Location = new System.Drawing.Point(217, 94);
		this.rbtnCarta.Name = "rbtnCarta";
		this.rbtnCarta.Size = new System.Drawing.Size(135, 17);
		this.rbtnCarta.TabIndex = 8;
		this.rbtnCarta.TabStop = true;
		this.rbtnCarta.Text = "Imprimir en Media Carta";
		this.rbtnCarta.UseVisualStyleBackColor = true;
		// 
		// rbtnTicket
		// 
		this.rbtnTicket.AutoSize = true;
		this.rbtnTicket.Location = new System.Drawing.Point(217, 117);
		this.rbtnTicket.Name = "rbtnTicket";
		this.rbtnTicket.Size = new System.Drawing.Size(108, 17);
		this.rbtnTicket.TabIndex = 9;
		this.rbtnTicket.TabStop = true;
		this.rbtnTicket.Text = "Imprimir en Ticket";
		this.rbtnTicket.UseVisualStyleBackColor = true;
		// 
		// Label1
		// 
		this.Label1.AutoSize = true;
		this.Label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.Label1.Location = new System.Drawing.Point(216, 7);
		this.Label1.Name = "Label1";
		this.Label1.Size = new System.Drawing.Size(113, 20);
		this.Label1.TabIndex = 10;
		this.Label1.Text = "Fuente Letra";
		// 
		// Label2
		// 
		this.Label2.AutoSize = true;
		this.Label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.Label2.Location = new System.Drawing.Point(271, 55);
		this.Label2.Name = "Label2";
		this.Label2.Size = new System.Drawing.Size(120, 20);
		this.Label2.TabIndex = 11;
		this.Label2.Text = "Tamaño Letra";
		// 
		// chkImprimirAlGuardar
		// 
		this.chkImprimirAlGuardar.AutoSize = true;
		this.chkImprimirAlGuardar.Location = new System.Drawing.Point(220, 155);
		this.chkImprimirAlGuardar.Name = "chkImprimirAlGuardar";
		this.chkImprimirAlGuardar.Size = new System.Drawing.Size(195, 17);
		this.chkImprimirAlGuardar.TabIndex = 38;
		this.chkImprimirAlGuardar.Text = "Imprimir automáticamente al guardar";
		this.chkImprimirAlGuardar.UseVisualStyleBackColor = true;
		// 
		// grpTipoEquipo
		// 
		this.grpTipoEquipo.Controls.Add(this.btnDesactivarTipo);
		this.grpTipoEquipo.Controls.Add(this.btnActivarTipo);
		this.grpTipoEquipo.Controls.Add(this.btnRenombrarTipo);
		this.grpTipoEquipo.Controls.Add(this.btnAgregarTipo);
		this.grpTipoEquipo.Controls.Add(this.txtNuevoNombreTipo);
		this.grpTipoEquipo.Controls.Add(this.lblNuevoNombreTipo);
		this.grpTipoEquipo.Controls.Add(this.dgvTiposEquipo);
		this.grpTipoEquipo.Location = new System.Drawing.Point(6, 4);
		this.grpTipoEquipo.Name = "grpTipoEquipo";
		this.grpTipoEquipo.Size = new System.Drawing.Size(434, 272);
		this.grpTipoEquipo.TabIndex = 0;
		this.grpTipoEquipo.TabStop = false;
		this.grpTipoEquipo.Text = "";
		// 
		// dgvTiposEquipo
		// 
		this.dgvTiposEquipo.AllowUserToAddRows = false;
		this.dgvTiposEquipo.AllowUserToDeleteRows = false;
		this.dgvTiposEquipo.AutoGenerateColumns = false;
		this.dgvTiposEquipo.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
		this.dgvTiposEquipo.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colNombre,
            this.colEstado});
		this.dgvTiposEquipo.Location = new System.Drawing.Point(9, 19);
		this.dgvTiposEquipo.Name = "dgvTiposEquipo";
		this.dgvTiposEquipo.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
		this.dgvTiposEquipo.ReadOnly = true;
		this.dgvTiposEquipo.RowHeadersVisible = false;
		this.dgvTiposEquipo.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
		this.dgvTiposEquipo.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
		this.dgvTiposEquipo.Size = new System.Drawing.Size(416, 168);
		this.dgvTiposEquipo.TabIndex = 0;
		// 
		// lblNuevoNombreTipo
		// 
		this.lblNuevoNombreTipo.AutoSize = true;
		this.lblNuevoNombreTipo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
		this.lblNuevoNombreTipo.Location = new System.Drawing.Point(9, 196);
		this.lblNuevoNombreTipo.Name = "lblNuevoNombreTipo";
		this.lblNuevoNombreTipo.Size = new System.Drawing.Size(68, 13);
		this.lblNuevoNombreTipo.TabIndex = 1;
		this.lblNuevoNombreTipo.Text = "Nuevo nombre:";
		// 
		// txtNuevoNombreTipo
		// 
		this.txtNuevoNombreTipo.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
		this.txtNuevoNombreTipo.Location = new System.Drawing.Point(9, 212);
		this.txtNuevoNombreTipo.Name = "txtNuevoNombreTipo";
		this.txtNuevoNombreTipo.Size = new System.Drawing.Size(416, 20);
		this.txtNuevoNombreTipo.TabIndex = 2;
		// 
		// btnAgregarTipo
		// 
		this.btnAgregarTipo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
		this.btnAgregarTipo.Location = new System.Drawing.Point(9, 240);
		this.btnAgregarTipo.Name = "btnAgregarTipo";
		this.btnAgregarTipo.Size = new System.Drawing.Size(66, 27);
		this.btnAgregarTipo.TabIndex = 3;
		this.btnAgregarTipo.Text = "Agregar";
		this.btnAgregarTipo.UseVisualStyleBackColor = true;
		// 
		// btnRenombrarTipo
		// 
		this.btnRenombrarTipo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
		this.btnRenombrarTipo.Location = new System.Drawing.Point(81, 240);
		this.btnRenombrarTipo.Name = "btnRenombrarTipo";
		this.btnRenombrarTipo.Size = new System.Drawing.Size(78, 27);
		this.btnRenombrarTipo.TabIndex = 4;
		this.btnRenombrarTipo.Text = "Renombrar";
		this.btnRenombrarTipo.UseVisualStyleBackColor = true;
		// 
		// btnActivarTipo
		// 
		this.btnActivarTipo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
		this.btnActivarTipo.Location = new System.Drawing.Point(165, 240);
		this.btnActivarTipo.Name = "btnActivarTipo";
		this.btnActivarTipo.Size = new System.Drawing.Size(66, 27);
		this.btnActivarTipo.TabIndex = 5;
		this.btnActivarTipo.Text = "Activar";
		this.btnActivarTipo.UseVisualStyleBackColor = true;
		// 
		// btnDesactivarTipo
		// 
		this.btnDesactivarTipo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
		this.btnDesactivarTipo.Location = new System.Drawing.Point(237, 240);
		this.btnDesactivarTipo.Name = "btnDesactivarTipo";
		this.btnDesactivarTipo.Size = new System.Drawing.Size(80, 27);
		this.btnDesactivarTipo.TabIndex = 6;
		this.btnDesactivarTipo.Text = "Desactivar";
		this.btnDesactivarTipo.UseVisualStyleBackColor = true;
		// 
		// colNombre
		// 
		this.colNombre.HeaderText = "Nombre";
		this.colNombre.Name = "colNombre";
		this.colNombre.ReadOnly = true;
		this.colNombre.MinimumWidth = 120;
		this.colNombre.FillWeight = 70f;
		// 
		// colEstado
		// 
		this.colEstado.HeaderText = "Estado";
		this.colEstado.Name = "colEstado";
		this.colEstado.ReadOnly = true;
		this.colEstado.MinimumWidth = 70;
		this.colEstado.FillWeight = 30f;
		// 
		// tabGeneral
		// 
		this.tabGeneral.Controls.Add(this.Panel1);
		this.tabGeneral.Location = new System.Drawing.Point(4, 22);
		this.tabGeneral.Name = "tabGeneral";
		this.tabGeneral.Padding = new System.Windows.Forms.Padding(3);
		this.tabGeneral.Size = new System.Drawing.Size(446, 284);
		this.tabGeneral.TabIndex = 0;
		this.tabGeneral.Text = "General";
		this.tabGeneral.UseVisualStyleBackColor = true;
		// 
		// tabTipos
		// 
		this.tabTipos.Controls.Add(this.grpTipoEquipo);
		this.tabTipos.Location = new System.Drawing.Point(4, 22);
		this.tabTipos.Name = "tabTipos";
		this.tabTipos.Padding = new System.Windows.Forms.Padding(3);
		this.tabTipos.Size = new System.Drawing.Size(446, 284);
		this.tabTipos.TabIndex = 1;
		this.tabTipos.Text = "Tipos de Equipo";
		this.tabTipos.UseVisualStyleBackColor = true;
		// 
		// tabControl
		// 
		this.tabControl.Controls.Add(this.tabGeneral);
		this.tabControl.Controls.Add(this.tabTipos);
		this.tabControl.Location = new System.Drawing.Point(12, 36);
		this.tabControl.Name = "tabControl";
		this.tabControl.SelectedIndex = 0;
		this.tabControl.Size = new System.Drawing.Size(454, 310);
		this.tabControl.TabIndex = 37;
		this.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		this.ClientSize = new System.Drawing.Size(478, 358);
		this.ControlBox = false;
		this.Controls.Add(this.tabControl);
		this.Controls.Add(this.TextBox1);
		this.Name = "frmConfiguracion";
		this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
		this.Text = "Configuración";
		this.ListImpresoras.SelectedIndexChanged += new System.EventHandler(this.ListImpresoras_SelectedIndexChanged);
		this.seleccionada.Click += new System.EventHandler(this.seleccionada_Click);
		this.btnSeleccionarImpresora.Click += new System.EventHandler(this.btnSeleccionarImpresora_Click);
		this.btnAgregarTipo.Click += new System.EventHandler(this.btnAgregarTipo_Click);
		this.btnRenombrarTipo.Click += new System.EventHandler(this.btnRenombrarTipo_Click);
		this.btnActivarTipo.Click += new System.EventHandler(this.btnActivarTipo_Click);
		this.btnDesactivarTipo.Click += new System.EventHandler(this.btnDesactivarTipo_Click);
		this.tabGeneral.ResumeLayout(false);
		this.tabTipos.ResumeLayout(false);
		this.tabControl.ResumeLayout(false);
		this.Panel1.ResumeLayout(false);
		this.Panel1.PerformLayout();
		this.grpTipoEquipo.ResumeLayout(false);
		this.grpTipoEquipo.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.numTamanoFuente).EndInit();
		((System.ComponentModel.ISupportInitialize)this.dgvTiposEquipo).EndInit();
		this.ResumeLayout(false);
		this.PerformLayout();
	}
}
