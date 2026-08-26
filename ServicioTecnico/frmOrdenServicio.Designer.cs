using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Printing;
using System.Windows.Forms;

namespace ServicioTecnico;

public partial class frmOrdenServicio : Form
{
	private IContainer components;

	internal Panel Panel1;
	internal Timer Timer1;
	internal TextBox txtNombre;
	internal Label Label3;
	internal TextBox txtTelefono;
	internal TextBox txtDocumento;
	internal Label Label16;
	internal Label Label17;
	internal TextBox txtDireccion;
	internal Label Label4;
	internal PictureBox imgFoto3;
	internal PictureBox imgFoto2;
	internal Button btnSeleccionarImagen;
	internal Button btnBuscarCliente;
	internal Label Label19;
	internal Label Label18;
	internal DateTimePicker txtEntregado;
	internal DateTimePicker txtReparado;
	internal TextBox txtModelo;
	internal Label Label15;
	internal Label Label14;
	internal Label Label13;
	internal Label Label12;
	internal Label Label11;
	internal Label Label10;
	internal Label Label9;
	internal Label Label8;
	internal Label Label7;
	internal Label Label6;
	internal Label Label5;
	internal Button btnEliminarOrden;
	internal TextBox TextBox3;
	internal TextBox TextBox2;
	internal TextBox TextBox1;
	internal PictureBox picLogo;
	internal Label Label2;
	internal Label Label1;
	internal Button btnImprimir;
	internal TextBox txtOrden;
	internal TextBox txtFecha;
	internal TextBox txtMarca;
	internal TextBox txtIMEI;
	internal TextBox txtClave;
	internal TextBox txtAccesorios;
	internal TextBox txtFalla;
	internal TextBox txtObservaciones;
	internal TextBox txtReparacion;
	internal ComboBox cmbEstadoEntrega;
	internal TextBox txtAbono;
	internal TextBox txtPresupuesto;
	internal TextBox txtTotal;
	internal PictureBox imgFoto1;
	internal Button btnNuevaOrden;
	internal Button btnGuardar;
	internal Button btnBuscarOrden;
	internal Button btnCondiciones;
	internal Button btnConfiguracion;
	internal Button btnReporte;
	internal GroupBox grpTipoEquipo;
	internal TextBox txtOtros;
	internal RadioButton rbtnOtros;
	internal RadioButton rbtnPC;
	internal RadioButton rbtnImpresora;
	internal RadioButton rbtnLaptop;
	internal Button Button1;
	internal PictureBox sinImagen;
	internal Button VER1;
	internal Button VER3;
	internal Button VER2;
	internal Timer licencia;
	internal TextBox queda;
	internal TextBox EmpresaDireccion;
	internal TextBox EmpresaCorreo;
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
            this.Panel1 = new System.Windows.Forms.Panel();
            this.EmpresaCorreo = new System.Windows.Forms.TextBox();
            this.EmpresaDireccion = new System.Windows.Forms.TextBox();
            this.queda = new System.Windows.Forms.TextBox();
            this.VER3 = new System.Windows.Forms.Button();
            this.VER2 = new System.Windows.Forms.Button();
            this.VER1 = new System.Windows.Forms.Button();
            this.sinImagen = new System.Windows.Forms.PictureBox();
            this.Button1 = new System.Windows.Forms.Button();
            this.txtNombre = new System.Windows.Forms.TextBox();
            this.Label3 = new System.Windows.Forms.Label();
            this.txtTelefono = new System.Windows.Forms.TextBox();
            this.txtDocumento = new System.Windows.Forms.TextBox();
            this.Label16 = new System.Windows.Forms.Label();
            this.Label17 = new System.Windows.Forms.Label();
            this.txtDireccion = new System.Windows.Forms.TextBox();
            this.Label4 = new System.Windows.Forms.Label();
            this.imgFoto3 = new System.Windows.Forms.PictureBox();
            this.imgFoto2 = new System.Windows.Forms.PictureBox();
            this.btnSeleccionarImagen = new System.Windows.Forms.Button();
            this.btnBuscarCliente = new System.Windows.Forms.Button();
            this.Label19 = new System.Windows.Forms.Label();
            this.Label18 = new System.Windows.Forms.Label();
            this.txtEntregado = new System.Windows.Forms.DateTimePicker();
            this.txtReparado = new System.Windows.Forms.DateTimePicker();
            this.txtModelo = new System.Windows.Forms.TextBox();
            this.Label15 = new System.Windows.Forms.Label();
            this.Label14 = new System.Windows.Forms.Label();
            this.Label13 = new System.Windows.Forms.Label();
            this.Label12 = new System.Windows.Forms.Label();
            this.Label11 = new System.Windows.Forms.Label();
            this.Label10 = new System.Windows.Forms.Label();
            this.Label9 = new System.Windows.Forms.Label();
            this.Label8 = new System.Windows.Forms.Label();
            this.Label7 = new System.Windows.Forms.Label();
            this.Label6 = new System.Windows.Forms.Label();
            this.Label5 = new System.Windows.Forms.Label();
            this.btnEliminarOrden = new System.Windows.Forms.Button();
            this.TextBox3 = new System.Windows.Forms.TextBox();
            this.TextBox2 = new System.Windows.Forms.TextBox();
            this.TextBox1 = new System.Windows.Forms.TextBox();
            this.picLogo = new System.Windows.Forms.PictureBox();
            this.Label2 = new System.Windows.Forms.Label();
            this.Label1 = new System.Windows.Forms.Label();
            this.btnImprimir = new System.Windows.Forms.Button();
            this.txtOrden = new System.Windows.Forms.TextBox();
            this.txtFecha = new System.Windows.Forms.TextBox();
            this.txtMarca = new System.Windows.Forms.TextBox();
            this.txtIMEI = new System.Windows.Forms.TextBox();
            this.txtClave = new System.Windows.Forms.TextBox();
            this.txtAccesorios = new System.Windows.Forms.TextBox();
            this.txtFalla = new System.Windows.Forms.TextBox();
            this.txtObservaciones = new System.Windows.Forms.TextBox();
            this.txtReparacion = new System.Windows.Forms.TextBox();
            this.cmbEstadoEntrega = new System.Windows.Forms.ComboBox();
            this.txtAbono = new System.Windows.Forms.TextBox();
            this.txtPresupuesto = new System.Windows.Forms.TextBox();
            this.txtTotal = new System.Windows.Forms.TextBox();
            this.imgFoto1 = new System.Windows.Forms.PictureBox();
            this.btnNuevaOrden = new System.Windows.Forms.Button();
            this.btnGuardar = new System.Windows.Forms.Button();
            this.btnBuscarOrden = new System.Windows.Forms.Button();
            this.btnCondiciones = new System.Windows.Forms.Button();
            this.btnConfiguracion = new System.Windows.Forms.Button();
            this.btnReporte = new System.Windows.Forms.Button();
            this.grpTipoEquipo = new System.Windows.Forms.GroupBox();
            this.txtOtros = new System.Windows.Forms.TextBox();
            this.rbtnOtros = new System.Windows.Forms.RadioButton();
            this.rbtnPC = new System.Windows.Forms.RadioButton();
            this.rbtnImpresora = new System.Windows.Forms.RadioButton();
            this.rbtnLaptop = new System.Windows.Forms.RadioButton();
            this.Timer1 = new System.Windows.Forms.Timer(this.components);
            this.licencia = new System.Windows.Forms.Timer(this.components);
            this.Panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.sinImagen)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.imgFoto3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.imgFoto2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picLogo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.imgFoto1)).BeginInit();
            this.grpTipoEquipo.SuspendLayout();
            this.SuspendLayout();
            // 
            // Panel1
            // 
            this.Panel1.BackColor = System.Drawing.SystemColors.Control;
            this.Panel1.Controls.Add(this.EmpresaCorreo);
            this.Panel1.Controls.Add(this.EmpresaDireccion);
            this.Panel1.Controls.Add(this.queda);
            this.Panel1.Controls.Add(this.VER3);
            this.Panel1.Controls.Add(this.VER2);
            this.Panel1.Controls.Add(this.VER1);
            this.Panel1.Controls.Add(this.sinImagen);
            this.Panel1.Controls.Add(this.Button1);
            this.Panel1.Controls.Add(this.txtNombre);
            this.Panel1.Controls.Add(this.Label3);
            this.Panel1.Controls.Add(this.txtTelefono);
            this.Panel1.Controls.Add(this.txtDocumento);
            this.Panel1.Controls.Add(this.Label16);
            this.Panel1.Controls.Add(this.Label17);
            this.Panel1.Controls.Add(this.txtDireccion);
            this.Panel1.Controls.Add(this.Label4);
            this.Panel1.Controls.Add(this.imgFoto3);
            this.Panel1.Controls.Add(this.imgFoto2);
            this.Panel1.Controls.Add(this.btnSeleccionarImagen);
            this.Panel1.Controls.Add(this.btnBuscarCliente);
            this.Panel1.Controls.Add(this.Label19);
            this.Panel1.Controls.Add(this.Label18);
            this.Panel1.Controls.Add(this.txtEntregado);
            this.Panel1.Controls.Add(this.txtReparado);
            this.Panel1.Controls.Add(this.txtModelo);
            this.Panel1.Controls.Add(this.Label15);
            this.Panel1.Controls.Add(this.Label14);
            this.Panel1.Controls.Add(this.Label13);
            this.Panel1.Controls.Add(this.Label12);
            this.Panel1.Controls.Add(this.Label11);
            this.Panel1.Controls.Add(this.Label10);
            this.Panel1.Controls.Add(this.Label9);
            this.Panel1.Controls.Add(this.Label8);
            this.Panel1.Controls.Add(this.Label7);
            this.Panel1.Controls.Add(this.Label6);
            this.Panel1.Controls.Add(this.Label5);
            this.Panel1.Controls.Add(this.btnEliminarOrden);
            this.Panel1.Controls.Add(this.TextBox3);
            this.Panel1.Controls.Add(this.TextBox2);
            this.Panel1.Controls.Add(this.TextBox1);
            this.Panel1.Controls.Add(this.picLogo);
            this.Panel1.Controls.Add(this.Label2);
            this.Panel1.Controls.Add(this.Label1);
            this.Panel1.Controls.Add(this.btnImprimir);
            this.Panel1.Controls.Add(this.txtOrden);
            this.Panel1.Controls.Add(this.txtFecha);
            this.Panel1.Controls.Add(this.txtMarca);
            this.Panel1.Controls.Add(this.txtIMEI);
            this.Panel1.Controls.Add(this.txtClave);
            this.Panel1.Controls.Add(this.txtAccesorios);
            this.Panel1.Controls.Add(this.txtFalla);
            this.Panel1.Controls.Add(this.txtObservaciones);
            this.Panel1.Controls.Add(this.txtReparacion);
            this.Panel1.Controls.Add(this.cmbEstadoEntrega);
            this.Panel1.Controls.Add(this.txtAbono);
            this.Panel1.Controls.Add(this.txtPresupuesto);
            this.Panel1.Controls.Add(this.txtTotal);
            this.Panel1.Controls.Add(this.imgFoto1);
            this.Panel1.Controls.Add(this.btnNuevaOrden);
            this.Panel1.Controls.Add(this.btnGuardar);
            this.Panel1.Controls.Add(this.btnBuscarOrden);
            this.Panel1.Controls.Add(this.btnCondiciones);
            this.Panel1.Controls.Add(this.btnConfiguracion);
            this.Panel1.Controls.Add(this.btnReporte);
            this.Panel1.Controls.Add(this.grpTipoEquipo);
            this.Panel1.Location = new System.Drawing.Point(12, 12);
            this.Panel1.Name = "Panel1";
            this.Panel1.Size = new System.Drawing.Size(1009, 543);
            this.Panel1.TabIndex = 59;
            this.Panel1.Paint += new System.Windows.Forms.PaintEventHandler(this.Panel1_Paint);
            // 
            // EmpresaCorreo
            // 
            this.EmpresaCorreo.Location = new System.Drawing.Point(19, 161);
            this.EmpresaCorreo.Name = "EmpresaCorreo";
            this.EmpresaCorreo.Size = new System.Drawing.Size(209, 20);
            this.EmpresaCorreo.TabIndex = 8007;
            this.EmpresaCorreo.Text = "Servicio Técnico";
            this.EmpresaCorreo.Visible = false;
            // 
            // EmpresaDireccion
            // 
            this.EmpresaDireccion.Location = new System.Drawing.Point(470, 161);
            this.EmpresaDireccion.Name = "EmpresaDireccion";
            this.EmpresaDireccion.Size = new System.Drawing.Size(200, 20);
            this.EmpresaDireccion.TabIndex = 8006;
            this.EmpresaDireccion.Text = "Sarmiento 1234 - Centro ";
            this.EmpresaDireccion.Visible = false;
            // 
            // queda
            // 
            this.queda.Font = new System.Drawing.Font("Microsoft Sans Serif", 27.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.queda.Location = new System.Drawing.Point(174, 51);
            this.queda.Multiline = true;
            this.queda.Name = "queda";
            this.queda.ReadOnly = true;
            this.queda.Size = new System.Drawing.Size(54, 46);
            this.queda.TabIndex = 8005;
            this.queda.Text = "30";
            this.queda.Visible = false;
            // 
            // VER3
            // 
            this.VER3.Location = new System.Drawing.Point(362, 402);
            this.VER3.Name = "VER3";
            this.VER3.Size = new System.Drawing.Size(25, 34);
            this.VER3.TabIndex = 8004;
            this.VER3.UseVisualStyleBackColor = true;
            this.VER3.Click += new System.EventHandler(this.VER3_Click);
            // 
            // VER2
            // 
            this.VER2.Location = new System.Drawing.Point(190, 402);
            this.VER2.Name = "VER2";
            this.VER2.Size = new System.Drawing.Size(25, 34);
            this.VER2.TabIndex = 8003;
            this.VER2.UseVisualStyleBackColor = true;
            this.VER2.Click += new System.EventHandler(this.VER2_Click);
            // 
            // VER1
            // 
            this.VER1.Location = new System.Drawing.Point(18, 402);
            this.VER1.Name = "VER1";
            this.VER1.Size = new System.Drawing.Size(25, 34);
            this.VER1.TabIndex = 8002;
            this.VER1.UseVisualStyleBackColor = true;
            this.VER1.Click += new System.EventHandler(this.VER1_Click);
            // 
            // sinImagen
            // 
            this.sinImagen.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.sinImagen.Location = new System.Drawing.Point(75, 438);
            this.sinImagen.Name = "sinImagen";
            this.sinImagen.Size = new System.Drawing.Size(46, 36);
            this.sinImagen.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.sinImagen.TabIndex = 8001;
            this.sinImagen.TabStop = false;
            this.sinImagen.Visible = false;
            // 
            // Button1
            // 
            this.Button1.Location = new System.Drawing.Point(447, 49);
            this.Button1.Name = "Button1";
            this.Button1.Size = new System.Drawing.Size(32, 23);
            this.Button1.TabIndex = 8000;
            this.Button1.UseVisualStyleBackColor = true;
            this.Button1.Click += new System.EventHandler(this.Button1_Click);
            // 
            // txtNombre
            // 
            this.txtNombre.Location = new System.Drawing.Point(379, 103);
            this.txtNombre.Name = "txtNombre";
            this.txtNombre.Size = new System.Drawing.Size(300, 20);
            this.txtNombre.TabIndex = 660;
            // 
            // Label3
            // 
            this.Label3.AutoSize = true;
            this.Label3.BackColor = System.Drawing.SystemColors.Control;
            this.Label3.Location = new System.Drawing.Point(253, 106);
            this.Label3.Name = "Label3";
            this.Label3.Size = new System.Drawing.Size(122, 13);
            this.Label3.TabIndex = 662;
            this.Label3.Text = "NOMBRE Y APELLIDO:";
            // 
            // txtTelefono
            // 
            this.txtTelefono.Location = new System.Drawing.Point(558, 77);
            this.txtTelefono.Name = "txtTelefono";
            this.txtTelefono.Size = new System.Drawing.Size(121, 20);
            this.txtTelefono.TabIndex = 659;
            this.txtTelefono.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtTelefono_KeyDown);
            // 
            // txtDocumento
            // 
            this.txtDocumento.Location = new System.Drawing.Point(379, 77);
            this.txtDocumento.Name = "txtDocumento";
            this.txtDocumento.Size = new System.Drawing.Size(100, 20);
            this.txtDocumento.TabIndex = 658;
            this.txtDocumento.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtDocumento_KeyDown);
            // 
            // Label16
            // 
            this.Label16.AutoSize = true;
            this.Label16.BackColor = System.Drawing.SystemColors.Control;
            this.Label16.Location = new System.Drawing.Point(293, 80);
            this.Label16.Name = "Label16";
            this.Label16.Size = new System.Drawing.Size(80, 13);
            this.Label16.TabIndex = 664;
            this.Label16.Text = "DOCUMENTO:";
            // 
            // Label17
            // 
            this.Label17.AutoSize = true;
            this.Label17.BackColor = System.Drawing.SystemColors.Control;
            this.Label17.Location = new System.Drawing.Point(485, 80);
            this.Label17.Name = "Label17";
            this.Label17.Size = new System.Drawing.Size(67, 13);
            this.Label17.TabIndex = 665;
            this.Label17.Text = "TELEFONO:";
            // 
            // txtDireccion
            // 
            this.txtDireccion.Location = new System.Drawing.Point(379, 129);
            this.txtDireccion.Name = "txtDireccion";
            this.txtDireccion.Size = new System.Drawing.Size(300, 20);
            this.txtDireccion.TabIndex = 661;
            // 
            // Label4
            // 
            this.Label4.AutoSize = true;
            this.Label4.BackColor = System.Drawing.SystemColors.Control;
            this.Label4.Location = new System.Drawing.Point(236, 132);
            this.Label4.Name = "Label4";
            this.Label4.Size = new System.Drawing.Size(139, 13);
            this.Label4.TabIndex = 663;
            this.Label4.Text = "DIRECCION O CORREO E:";
            // 
            // imgFoto3
            // 
            this.imgFoto3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.imgFoto3.Location = new System.Drawing.Point(360, 399);
            this.imgFoto3.Name = "imgFoto3";
            this.imgFoto3.Size = new System.Drawing.Size(166, 125);
            this.imgFoto3.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.imgFoto3.TabIndex = 654;
            this.imgFoto3.TabStop = false;
            this.imgFoto3.Click += new System.EventHandler(this.imgFoto3_Click);
            // 
            // imgFoto2
            // 
            this.imgFoto2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.imgFoto2.Location = new System.Drawing.Point(188, 399);
            this.imgFoto2.Name = "imgFoto2";
            this.imgFoto2.Size = new System.Drawing.Size(166, 125);
            this.imgFoto2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.imgFoto2.TabIndex = 653;
            this.imgFoto2.TabStop = false;
            this.imgFoto2.Click += new System.EventHandler(this.imgFoto2_Click);
            // 
            // btnSeleccionarImagen
            // 
            this.btnSeleccionarImagen.Location = new System.Drawing.Point(26, 480);
            this.btnSeleccionarImagen.Name = "btnSeleccionarImagen";
            this.btnSeleccionarImagen.Size = new System.Drawing.Size(142, 28);
            this.btnSeleccionarImagen.TabIndex = 624;
            this.btnSeleccionarImagen.Text = "Adjuntar Foto";
            this.btnSeleccionarImagen.Visible = false;
            // 
            // btnBuscarCliente
            // 
            this.btnBuscarCliente.ForeColor = System.Drawing.Color.Teal;
            this.btnBuscarCliente.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnBuscarCliente.Location = new System.Drawing.Point(847, 99);
            this.btnBuscarCliente.Name = "btnBuscarCliente";
            this.btnBuscarCliente.Size = new System.Drawing.Size(146, 39);
            this.btnBuscarCliente.TabIndex = 652;
            this.btnBuscarCliente.Text = "Buscar Cliente";
            this.btnBuscarCliente.Click += new System.EventHandler(this.btnBuscarCliente_Click);
            // 
            // Label19
            // 
            this.Label19.AutoSize = true;
            this.Label19.BackColor = System.Drawing.SystemColors.Control;
            this.Label19.Location = new System.Drawing.Point(532, 441);
            this.Label19.Name = "Label19";
            this.Label19.Size = new System.Drawing.Size(113, 13);
            this.Label19.TabIndex = 651;
            this.Label19.Text = "FECHA ENTREGADO";
            // 
            // Label18
            // 
            this.Label18.AutoSize = true;
            this.Label18.BackColor = System.Drawing.SystemColors.Control;
            this.Label18.Location = new System.Drawing.Point(532, 400);
            this.Label18.Name = "Label18";
            this.Label18.Size = new System.Drawing.Size(108, 13);
            this.Label18.TabIndex = 650;
            this.Label18.Text = "FECHA REPARADO:";
            // 
            // txtEntregado
            // 
            this.txtEntregado.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.txtEntregado.Location = new System.Drawing.Point(532, 457);
            this.txtEntregado.Name = "txtEntregado";
            this.txtEntregado.Size = new System.Drawing.Size(108, 20);
            this.txtEntregado.TabIndex = 649;
            // 
            // txtReparado
            // 
            this.txtReparado.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.txtReparado.Location = new System.Drawing.Point(532, 415);
            this.txtReparado.Name = "txtReparado";
            this.txtReparado.Size = new System.Drawing.Size(108, 20);
            this.txtReparado.TabIndex = 648;
            // 
            // txtModelo
            // 
            this.txtModelo.Location = new System.Drawing.Point(305, 250);
            this.txtModelo.Name = "txtModelo";
            this.txtModelo.Size = new System.Drawing.Size(150, 20);
            this.txtModelo.TabIndex = 611;
            // 
            // Label15
            // 
            this.Label15.AutoSize = true;
            this.Label15.BackColor = System.Drawing.SystemColors.Control;
            this.Label15.Location = new System.Drawing.Point(883, 486);
            this.Label15.Name = "Label15";
            this.Label15.Size = new System.Drawing.Size(96, 13);
            this.Label15.TabIndex = 647;
            this.Label15.Text = "RESTA A PAGAR:";
            // 
            // Label14
            // 
            this.Label14.AutoSize = true;
            this.Label14.BackColor = System.Drawing.SystemColors.Control;
            this.Label14.Location = new System.Drawing.Point(883, 401);
            this.Label14.Name = "Label14";
            this.Label14.Size = new System.Drawing.Size(91, 13);
            this.Label14.TabIndex = 646;
            this.Label14.Text = "PRESUPUESTO:";
            // 
            // Label13
            // 
            this.Label13.AutoSize = true;
            this.Label13.BackColor = System.Drawing.SystemColors.Control;
            this.Label13.Location = new System.Drawing.Point(884, 441);
            this.Label13.Name = "Label13";
            this.Label13.Size = new System.Drawing.Size(48, 13);
            this.Label13.TabIndex = 645;
            this.Label13.Text = "ABONO:";
            // 
            // Label12
            // 
            this.Label12.AutoSize = true;
            this.Label12.BackColor = System.Drawing.SystemColors.Control;
            this.Label12.Location = new System.Drawing.Point(37, 375);
            this.Label12.Name = "Label12";
            this.Label12.Size = new System.Drawing.Size(80, 13);
            this.Label12.TabIndex = 644;
            this.Label12.Text = "REPARACION:";
            // 
            // Label11
            // 
            this.Label11.AutoSize = true;
            this.Label11.BackColor = System.Drawing.SystemColors.Control;
            this.Label11.Location = new System.Drawing.Point(16, 348);
            this.Label11.Name = "Label11";
            this.Label11.Size = new System.Drawing.Size(101, 13);
            this.Label11.TabIndex = 643;
            this.Label11.Text = "OBSERVACIONES:";
            // 
            // Label10
            // 
            this.Label10.AutoSize = true;
            this.Label10.BackColor = System.Drawing.SystemColors.Control;
            this.Label10.Location = new System.Drawing.Point(72, 321);
            this.Label10.Name = "Label10";
            this.Label10.Size = new System.Drawing.Size(42, 13);
            this.Label10.TabIndex = 642;
            this.Label10.Text = "FALLA:";
            // 
            // Label9
            // 
            this.Label9.AutoSize = true;
            this.Label9.BackColor = System.Drawing.SystemColors.Control;
            this.Label9.Location = new System.Drawing.Point(694, 232);
            this.Label9.Name = "Label9";
            this.Label9.Size = new System.Drawing.Size(103, 13);
            this.Label9.TabIndex = 641;
            this.Label9.Text = "CLAVE O PATRON:";
            // 
            // Label8
            // 
            this.Label8.AutoSize = true;
            this.Label8.BackColor = System.Drawing.SystemColors.Control;
            this.Label8.Location = new System.Drawing.Point(694, 190);
            this.Label8.Name = "Label8";
            this.Label8.Size = new System.Drawing.Size(79, 13);
            this.Label8.TabIndex = 640;
            this.Label8.Text = "ACCESORIOS:";
            // 
            // Label7
            // 
            this.Label7.AutoSize = true;
            this.Label7.BackColor = System.Drawing.SystemColors.Control;
            this.Label7.Location = new System.Drawing.Point(467, 253);
            this.Label7.Name = "Label7";
            this.Label7.Size = new System.Drawing.Size(48, 13);
            this.Label7.TabIndex = 639;
            this.Label7.Text = "SERIAL:";
            // 
            // Label6
            // 
            this.Label6.AutoSize = true;
            this.Label6.BackColor = System.Drawing.SystemColors.Control;
            this.Label6.Location = new System.Drawing.Point(243, 253);
            this.Label6.Name = "Label6";
            this.Label6.Size = new System.Drawing.Size(56, 13);
            this.Label6.TabIndex = 638;
            this.Label6.Text = "MODELO:";
            // 
            // Label5
            // 
            this.Label5.AutoSize = true;
            this.Label5.BackColor = System.Drawing.SystemColors.Control;
            this.Label5.Location = new System.Drawing.Point(23, 253);
            this.Label5.Name = "Label5";
            this.Label5.Size = new System.Drawing.Size(48, 13);
            this.Label5.TabIndex = 637;
            this.Label5.Text = "MARCA:";
            // 
            // btnEliminarOrden
            // 
            this.btnEliminarOrden.ForeColor = System.Drawing.Color.Teal;
            this.btnEliminarOrden.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnEliminarOrden.Location = new System.Drawing.Point(697, 59);
            this.btnEliminarOrden.Name = "btnEliminarOrden";
            this.btnEliminarOrden.Size = new System.Drawing.Size(146, 36);
            this.btnEliminarOrden.TabIndex = 636;
            this.btnEliminarOrden.Text = "Eliminar Orden";
            this.btnEliminarOrden.Click += new System.EventHandler(this.btnEliminarOrden_Click);
            // 
            // TextBox3
            // 
            this.TextBox3.BackColor = System.Drawing.Color.LightBlue;
            this.TextBox3.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TextBox3.Location = new System.Drawing.Point(16, 286);
            this.TextBox3.Name = "TextBox3";
            this.TextBox3.ReadOnly = true;
            this.TextBox3.Size = new System.Drawing.Size(977, 29);
            this.TextBox3.TabIndex = 635;
            this.TextBox3.Text = "DATOS DE LA FALLA";
            this.TextBox3.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // TextBox2
            // 
            this.TextBox2.BackColor = System.Drawing.Color.LightBlue;
            this.TextBox2.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TextBox2.Location = new System.Drawing.Point(15, 155);
            this.TextBox2.Name = "TextBox2";
            this.TextBox2.ReadOnly = true;
            this.TextBox2.Size = new System.Drawing.Size(664, 29);
            this.TextBox2.TabIndex = 634;
            this.TextBox2.Text = "DATOS DEL EQUIPO";
            this.TextBox2.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // TextBox1
            // 
            this.TextBox1.BackColor = System.Drawing.Color.LightBlue;
            this.TextBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TextBox1.Location = new System.Drawing.Point(174, 16);
            this.TextBox1.Name = "TextBox1";
            this.TextBox1.ReadOnly = true;
            this.TextBox1.Size = new System.Drawing.Size(505, 29);
            this.TextBox1.TabIndex = 633;
            this.TextBox1.Text = "DATOS DEL CLIENTE";
            this.TextBox1.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // picLogo
            // 
            this.picLogo.Location = new System.Drawing.Point(16, 16);
            this.picLogo.Name = "picLogo";
            this.picLogo.Size = new System.Drawing.Size(152, 133);
            this.picLogo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.picLogo.TabIndex = 632;
            this.picLogo.TabStop = false;
            // 
            // Label2
            // 
            this.Label2.AutoSize = true;
            this.Label2.BackColor = System.Drawing.SystemColors.Control;
            this.Label2.Location = new System.Drawing.Point(253, 54);
            this.Label2.Name = "Label2";
            this.Label2.Size = new System.Drawing.Size(120, 13);
            this.Label2.TabIndex = 631;
            this.Label2.Text = "ORDEN DE SERVICIO:";
            // 
            // Label1
            // 
            this.Label1.AutoSize = true;
            this.Label1.BackColor = System.Drawing.SystemColors.Control;
            this.Label1.Location = new System.Drawing.Point(507, 54);
            this.Label1.Name = "Label1";
            this.Label1.Size = new System.Drawing.Size(45, 13);
            this.Label1.TabIndex = 630;
            this.Label1.Text = "FECHA:";
            // 
            // btnImprimir
            // 
            this.btnImprimir.ForeColor = System.Drawing.Color.Teal;
            this.btnImprimir.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnImprimir.Location = new System.Drawing.Point(697, 99);
            this.btnImprimir.Name = "btnImprimir";
            this.btnImprimir.Size = new System.Drawing.Size(146, 39);
            this.btnImprimir.TabIndex = 629;
            this.btnImprimir.Text = "ReImprimir";
            this.btnImprimir.UseVisualStyleBackColor = true;
            this.btnImprimir.Click += new System.EventHandler(this.btnImprimir_Click);
            // 
            // txtOrden
            // 
            this.txtOrden.Location = new System.Drawing.Point(379, 51);
            this.txtOrden.Name = "txtOrden";
            this.txtOrden.ReadOnly = true;
            this.txtOrden.Size = new System.Drawing.Size(62, 20);
            this.txtOrden.TabIndex = 655;
            this.txtOrden.TextChanged += new System.EventHandler(this.txtOrden_TextChanged);
            // 
            // txtFecha
            // 
            this.txtFecha.Location = new System.Drawing.Point(558, 51);
            this.txtFecha.Name = "txtFecha";
            this.txtFecha.Size = new System.Drawing.Size(121, 20);
            this.txtFecha.TabIndex = 656;
            // 
            // txtMarca
            // 
            this.txtMarca.Location = new System.Drawing.Point(81, 250);
            this.txtMarca.Name = "txtMarca";
            this.txtMarca.Size = new System.Drawing.Size(150, 20);
            this.txtMarca.TabIndex = 610;
            // 
            // txtIMEI
            // 
            this.txtIMEI.Location = new System.Drawing.Point(518, 251);
            this.txtIMEI.Name = "txtIMEI";
            this.txtIMEI.Size = new System.Drawing.Size(150, 20);
            this.txtIMEI.TabIndex = 612;
            // 
            // txtClave
            // 
            this.txtClave.Location = new System.Drawing.Point(697, 250);
            this.txtClave.Name = "txtClave";
            this.txtClave.Size = new System.Drawing.Size(295, 20);
            this.txtClave.TabIndex = 613;
            // 
            // txtAccesorios
            // 
            this.txtAccesorios.Location = new System.Drawing.Point(697, 209);
            this.txtAccesorios.Name = "txtAccesorios";
            this.txtAccesorios.Size = new System.Drawing.Size(295, 20);
            this.txtAccesorios.TabIndex = 609;
            // 
            // txtFalla
            // 
            this.txtFalla.Location = new System.Drawing.Point(123, 321);
            this.txtFalla.Name = "txtFalla";
            this.txtFalla.Size = new System.Drawing.Size(869, 20);
            this.txtFalla.TabIndex = 614;
            // 
            // txtObservaciones
            // 
            this.txtObservaciones.Location = new System.Drawing.Point(123, 347);
            this.txtObservaciones.Name = "txtObservaciones";
            this.txtObservaciones.Size = new System.Drawing.Size(870, 20);
            this.txtObservaciones.TabIndex = 615;
            // 
            // txtReparacion
            // 
            this.txtReparacion.Location = new System.Drawing.Point(123, 373);
            this.txtReparacion.Name = "txtReparacion";
            this.txtReparacion.Size = new System.Drawing.Size(869, 20);
            this.txtReparacion.TabIndex = 616;
            // 
            // cmbEstadoEntrega
            // 
            this.cmbEstadoEntrega.Font = new System.Drawing.Font("Microsoft Sans Serif", 21.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbEstadoEntrega.Location = new System.Drawing.Point(529, 483);
            this.cmbEstadoEntrega.Name = "cmbEstadoEntrega";
            this.cmbEstadoEntrega.Size = new System.Drawing.Size(351, 41);
            this.cmbEstadoEntrega.TabIndex = 1000;
            // 
            // txtAbono
            // 
            this.txtAbono.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtAbono.Location = new System.Drawing.Point(887, 456);
            this.txtAbono.Name = "txtAbono";
            this.txtAbono.Size = new System.Drawing.Size(106, 24);
            this.txtAbono.TabIndex = 620;
            this.txtAbono.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtAbono.TextChanged += new System.EventHandler(this.txtAbono_TextChanged);
            this.txtAbono.Enter += new System.EventHandler(this.txtAbono_Enter);
            this.txtAbono.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtAbono_KeyDown);
            this.txtAbono.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtAbono_KeyPress);
            this.txtAbono.Validating += new System.ComponentModel.CancelEventHandler(this.txtAbono_Validating);
            this.txtAbono.Validated += new System.EventHandler(this.txtAbono_Validated);
            // 
            // txtPresupuesto
            // 
            this.txtPresupuesto.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtPresupuesto.Location = new System.Drawing.Point(886, 416);
            this.txtPresupuesto.Name = "txtPresupuesto";
            this.txtPresupuesto.Size = new System.Drawing.Size(106, 24);
            this.txtPresupuesto.TabIndex = 619;
            this.txtPresupuesto.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtPresupuesto.TextChanged += new System.EventHandler(this.txtPresupuesto_TextChanged);
            this.txtPresupuesto.Enter += new System.EventHandler(this.txtPresupuesto_Enter);
            this.txtPresupuesto.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtPresupuesto_KeyDown);
            this.txtPresupuesto.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtPresupuesto_KeyPress);
            this.txtPresupuesto.Validating += new System.ComponentModel.CancelEventHandler(this.txtPresupuesto_Validating);
            this.txtPresupuesto.Validated += new System.EventHandler(this.txtPresupuesto_Validated);
            // 
            // txtTotal
            // 
            this.txtTotal.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtTotal.ForeColor = System.Drawing.Color.Maroon;
            this.txtTotal.Location = new System.Drawing.Point(886, 500);
            this.txtTotal.Name = "txtTotal";
            this.txtTotal.ReadOnly = true;
            this.txtTotal.Size = new System.Drawing.Size(106, 24);
            this.txtTotal.TabIndex = 621;
            this.txtTotal.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // imgFoto1
            // 
            this.imgFoto1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.imgFoto1.Location = new System.Drawing.Point(16, 399);
            this.imgFoto1.Name = "imgFoto1";
            this.imgFoto1.Size = new System.Drawing.Size(166, 125);
            this.imgFoto1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.imgFoto1.TabIndex = 621;
            this.imgFoto1.TabStop = false;
            this.imgFoto1.Click += new System.EventHandler(this.imgFoto1_Click);
            // 
            // btnNuevaOrden
            // 
            this.btnNuevaOrden.ForeColor = System.Drawing.Color.Teal;
            this.btnNuevaOrden.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnNuevaOrden.Location = new System.Drawing.Point(697, 17);
            this.btnNuevaOrden.Name = "btnNuevaOrden";
            this.btnNuevaOrden.Size = new System.Drawing.Size(146, 38);
            this.btnNuevaOrden.TabIndex = 623;
            this.btnNuevaOrden.Text = "Nueva Orden";
            this.btnNuevaOrden.Click += new System.EventHandler(this.btnNuevaOrden_Click);
            // 
            // btnGuardar
            // 
            this.btnGuardar.BackColor = System.Drawing.Color.LightBlue;
            this.btnGuardar.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnGuardar.ForeColor = System.Drawing.Color.Black;
            this.btnGuardar.Location = new System.Drawing.Point(646, 400);
            this.btnGuardar.Name = "btnGuardar";
            this.btnGuardar.Size = new System.Drawing.Size(234, 79);
            this.btnGuardar.TabIndex = 622;
            this.btnGuardar.Text = "GUARDAR";
            this.btnGuardar.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnGuardar.UseVisualStyleBackColor = false;
            this.btnGuardar.Click += new System.EventHandler(this.btnGuardar_Click);
            // 
            // btnBuscarOrden
            // 
            this.btnBuscarOrden.ForeColor = System.Drawing.Color.Teal;
            this.btnBuscarOrden.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnBuscarOrden.Location = new System.Drawing.Point(846, 59);
            this.btnBuscarOrden.Name = "btnBuscarOrden";
            this.btnBuscarOrden.Size = new System.Drawing.Size(146, 36);
            this.btnBuscarOrden.TabIndex = 625;
            this.btnBuscarOrden.Text = "Buscar Orden";
            this.btnBuscarOrden.Click += new System.EventHandler(this.btnBuscarOrden_Click);
            // 
            // btnCondiciones
            // 
            this.btnCondiciones.ForeColor = System.Drawing.Color.Teal;
            this.btnCondiciones.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnCondiciones.Location = new System.Drawing.Point(846, 17);
            this.btnCondiciones.Name = "btnCondiciones";
            this.btnCondiciones.Size = new System.Drawing.Size(146, 38);
            this.btnCondiciones.TabIndex = 626;
            this.btnCondiciones.Text = "Editar Condiciones";
            this.btnCondiciones.Click += new System.EventHandler(this.btnCondiciones_Click);
            // 
            // btnConfiguracion
            // 
            this.btnConfiguracion.ForeColor = System.Drawing.Color.Teal;
            this.btnConfiguracion.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnConfiguracion.Location = new System.Drawing.Point(847, 142);
            this.btnConfiguracion.Name = "btnConfiguracion";
            this.btnConfiguracion.Size = new System.Drawing.Size(146, 39);
            this.btnConfiguracion.TabIndex = 627;
            this.btnConfiguracion.Text = "Configuración";
            this.btnConfiguracion.Click += new System.EventHandler(this.btnConfiguracion_Click);
            // 
            // btnReporte
            // 
            this.btnReporte.ForeColor = System.Drawing.Color.Teal;
            this.btnReporte.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnReporte.Location = new System.Drawing.Point(697, 142);
            this.btnReporte.Name = "btnReporte";
            this.btnReporte.Size = new System.Drawing.Size(146, 39);
            this.btnReporte.TabIndex = 628;
            this.btnReporte.Text = "Reporte Servicios";
            this.btnReporte.Click += new System.EventHandler(this.btnReporte_Click);
            // 
            // grpTipoEquipo
            // 
            this.grpTipoEquipo.Controls.Add(this.txtOtros);
            this.grpTipoEquipo.Controls.Add(this.rbtnOtros);
            this.grpTipoEquipo.Controls.Add(this.rbtnPC);
            this.grpTipoEquipo.Controls.Add(this.rbtnImpresora);
            this.grpTipoEquipo.Controls.Add(this.rbtnLaptop);
            this.grpTipoEquipo.Location = new System.Drawing.Point(16, 190);
            this.grpTipoEquipo.Name = "grpTipoEquipo";
            this.grpTipoEquipo.Size = new System.Drawing.Size(663, 54);
            this.grpTipoEquipo.TabIndex = 657;
            this.grpTipoEquipo.TabStop = false;
            this.grpTipoEquipo.Text = "Tipo de equipo";
            // 
            // txtOtros
            // 
            this.txtOtros.Location = new System.Drawing.Point(363, 18);
            this.txtOtros.Name = "txtOtros";
            this.txtOtros.Size = new System.Drawing.Size(289, 20);
            this.txtOtros.TabIndex = 8;
            // 
            // rbtnOtros
            // 
            this.rbtnOtros.Location = new System.Drawing.Point(301, 16);
            this.rbtnOtros.Name = "rbtnOtros";
            this.rbtnOtros.Size = new System.Drawing.Size(54, 24);
            this.rbtnOtros.TabIndex = 7;
            this.rbtnOtros.Text = "Otro";
            // 
            // rbtnPC
            // 
            this.rbtnPC.Location = new System.Drawing.Point(223, 16);
            this.rbtnPC.Name = "rbtnPC";
            this.rbtnPC.Size = new System.Drawing.Size(59, 24);
            this.rbtnPC.TabIndex = 6;
            this.rbtnPC.Text = "PC";
            // 
            // rbtnImpresora
            // 
            this.rbtnImpresora.Location = new System.Drawing.Point(126, 16);
            this.rbtnImpresora.Name = "rbtnImpresora";
            this.rbtnImpresora.Size = new System.Drawing.Size(86, 24);
            this.rbtnImpresora.TabIndex = 5;
            this.rbtnImpresora.Text = "Impresora";
            // 
            // rbtnLaptop
            // 
            this.rbtnLaptop.Location = new System.Drawing.Point(30, 17);
            this.rbtnLaptop.Name = "rbtnLaptop";
            this.rbtnLaptop.Size = new System.Drawing.Size(81, 24);
            this.rbtnLaptop.TabIndex = 4;
            this.rbtnLaptop.Text = "Laptop";
            // 
            // Timer1
            // 
            this.Timer1.Tick += new System.EventHandler(this.Timer1_Tick);
            // 
            // licencia
            // 
            this.licencia.Interval = 1000;
            this.licencia.Tick += new System.EventHandler(this.licencia_Tick);
            // 
            // frmOrdenServicio
            // 
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.ClientSize = new System.Drawing.Size(1030, 561);
            this.Controls.Add(this.Panel1);
            this.Name = "frmOrdenServicio";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Hlc Informática";
            this.Panel1.ResumeLayout(false);
            this.Panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.sinImagen)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.imgFoto3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.imgFoto2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picLogo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.imgFoto1)).EndInit();
            this.grpTipoEquipo.ResumeLayout(false);
            this.grpTipoEquipo.PerformLayout();
            this.ResumeLayout(false);

	}
}
