using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SQLite;
using System.Drawing;
using System.Drawing.Printing;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows.Forms;
using BarcodeLib;
using Microsoft.VisualBasic;
using Microsoft.VisualBasic.CompilerServices;

namespace ServicioTecnico;

public partial class frmOrdenServicio : Form
{

	private Font fuenteTicket;
	private bool cargandoTiposEquipo;
	private LayoutSizer _layoutSizer;
	private Size _ultimoClientAplicado;

	public frmOrdenServicio()
	{
		base.Resize += frmOrdenServicio_Resize;
		base.Load += frmOrdenServicio_Load;
		base.FormClosing += frmOrdenServicio_FormClosing;
		InitializeComponent();
		// Snapshot inmutable del layout original (posición (12,12), 1009x543 y
		// fuentes del Designer): base de todo escalado posterior.
		_layoutSizer = LayoutSizer.Capture(Panel1, ClientSize);
		_ultimoClientAplicado = ClientSize;
		CargarIconosFormulario();
		campoPresupuesto = new CampoMonetario(txtPresupuesto);
		campoAbono = new CampoMonetario(txtAbono);
	}

	private void CargarIconosFormulario()
	{
		ComponentResourceManager rm = new ComponentResourceManager(typeof(frmOrdenServicio));
		btnNuevaOrden.Image = (Image)rm.GetObject("btnNuevaOrden.Image");
		btnGuardar.Image = (Image)rm.GetObject("btnGuardar.Image");
		btnImprimir.Image = (Image)rm.GetObject("btnImprimir.Image");
		btnEliminarOrden.Image = (Image)rm.GetObject("btnEliminarOrden.Image");
		btnBuscarOrden.Image = (Image)rm.GetObject("btnBuscarOrden.Image");
		btnCondiciones.Image = (Image)rm.GetObject("btnCondiciones.Image");
		btnConfiguracion.Image = (Image)rm.GetObject("btnConfiguracion.Image");
		btnReporte.Image = (Image)rm.GetObject("btnReporte.Image");
		btnBuscarCliente.Image = (Image)rm.GetObject("btnBuscarCliente.Image");
		VER1.Image = (Image)rm.GetObject("VER1.Image");
		VER2.Image = (Image)rm.GetObject("VER2.Image");
		VER3.Image = (Image)rm.GetObject("VER3.Image");
		Button1.BackgroundImage = (Image)rm.GetObject("Button1.BackgroundImage");
		sinImagen.BackgroundImage = (Image)rm.GetObject("sinImagen.BackgroundImage");
		picLogo.Image = (Image)rm.GetObject("picLogo.Image");
	}

	private void frmOrdenServicio_Resize(object sender, EventArgs e)
	{
		// El evento Resize cubre resize manual, maximizar, restaurar y cambios de
		// tamaño programáticos (ResizeEnd no se dispara de forma confiable en todos
		// esos casos). La guarda evita trabajo redundante: si el ClientSize no
		// cambió no se re-aplica nada; LayoutSizer deriva siempre del snapshot
		// inmutable, por lo que múltiples aplicaciones no producen drift.
		if (_layoutSizer == null || ClientSize.Width <= 0 || ClientSize.Height <= 0)
		{
			return;
		}
		if (ClientSize == _ultimoClientAplicado)
		{
			return;
		}
		_ultimoClientAplicado = ClientSize;
		_layoutSizer.Apply(ClientSize);
	}

	private void frmOrdenServicio_Load(object sender, EventArgs e)
	{
		if (IsRunningFromCompressedFolder())
		{
			ShowCompressedFolderWarning();
		}
		CheckRequiredFiles();
		modConexion.VerificarOCrearBD();
		CargarTiposEquipo();
		fuenteTicket = new Font("Courier New", 9f);
		cmbEstadoEntrega.Items.AddRange(new object[5] { "POR REVISAR", "REVISADO", "DIAGNOSTICO", "REPARADO", "ENTREGADO" });
		NuevaOrden();
		Timer1.Enabled = true;
	}

	private bool IsRunningFromCompressedFolder()
	{
		bool result;
		try
		{
			string location = Assembly.GetExecutingAssembly().Location;
			if (File.GetAttributes(location).HasFlag(FileAttributes.Compressed))
			{
				result = true;
			}
			else
			{
				string text = location.ToLower();
				if (text.Contains("zip") || text.Contains("rar"))
				{
					result = true;
				}
				else
				{
					string value = Path.GetTempPath().ToLower();
					result = (text.Contains(value) ? true : false);
				}
			}
		}
		catch (Exception ex)
		{
			ProjectData.SetProjectError(ex);
			Exception ex2 = ex;
			result = false;
			ProjectData.ClearProjectError();
		}
		return result;
	}

	private void ShowCompressedFolderWarning()
	{
		string text = "¡ADVERTENCIA!" + Environment.NewLine + Environment.NewLine + "Está ejecutando el programa directamente desde el archivo comprimido." + Environment.NewLine + "Esto causará errores y no se guardará la información correctamente." + Environment.NewLine + Environment.NewLine + "Por favor:" + Environment.NewLine + "1. Extraiga TODOS los archivos a una carpeta" + Environment.NewLine + "2. Ejecute el programa desde la carpeta extraída" + Environment.NewLine + Environment.NewLine + "¿Desea continuar de todos modos (NO RECOMENDADO)?";
		DialogResult dialogResult = MessageBox.Show(text, "Ejecución incorrecta", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation);
		if (dialogResult == DialogResult.No)
		{
			Application.Exit();
		}
	}

	private void CheckRequiredFiles()
	{
		string[] array = new string[3] { "BarcodeLib.dll", "ordenes.db", "System.Data.SQLite.dll" };
		List<string> list = new List<string>();
		string[] array2 = array;
		foreach (string text in array2)
		{
			if (!File.Exists(text))
			{
				list.Add(text);
			}
		}
		if (list.Count > 0)
		{
			string text2 = "Faltan archivos esenciales:" + Environment.NewLine + string.Join(Environment.NewLine, list) + Environment.NewLine + Environment.NewLine + "Extraiga TODOS los archivos del ZIP antes de ejecutar.";
			MessageBox.Show(text2, "Archivos faltantes", MessageBoxButtons.OK, MessageBoxIcon.Hand);
			Application.Exit();
		}
	}

	private void GuardarOrden()
	{
		if ((Operators.CompareString(txtNombre.Text.Trim(), "", TextCompare: false) == 0) | (Operators.CompareString(txtFalla.Text.Trim(), "", TextCompare: false) == 0))
		{
			Interaction.MsgBox("Por favor complete al menos el nombre y la falla.", MsgBoxStyle.Exclamation);
			return;
		}
		bool flag = string.IsNullOrEmpty(txtOrden.Text.Trim());
		if (flag && cmbTipoEquipo.SelectedIndex < 0)
		{
			Interaction.MsgBox("Debe seleccionar un tipo de equipo.", MsgBoxStyle.Exclamation);
			return;
		}
		if (flag)
		{
			txtOrden.Text = GenerarNuevoNumeroOrden();
		}
		string text = Path.Combine(Application.StartupPath, "imagenes");
		if (!Directory.Exists(text))
		{
			Directory.CreateDirectory(text);
		}
		using SQLiteConnection sQLiteConnection = new SQLiteConnection("Data Source=" + modConexion.rutaDB);
		sQLiteConnection.Open();
		int num = -1;
		using (SQLiteCommand sQLiteCommand = new SQLiteCommand("SELECT id_cliente FROM clientes WHERE nombre=@nombre", sQLiteConnection))
		{
			sQLiteCommand.Parameters.AddWithValue("@nombre", txtNombre.Text.Trim());
			object objectValue = RuntimeHelpers.GetObjectValue(sQLiteCommand.ExecuteScalar());
			if (objectValue == null)
			{
				using SQLiteCommand sQLiteCommand2 = new SQLiteCommand("INSERT INTO clientes (nombre, direccion, documento, telefono) VALUES (@n, @d, @doc, @t); SELECT last_insert_rowid()", sQLiteConnection);
				sQLiteCommand2.Parameters.AddWithValue("@n", txtNombre.Text.Trim());
				sQLiteCommand2.Parameters.AddWithValue("@d", txtDireccion.Text.Trim());
				sQLiteCommand2.Parameters.AddWithValue("@doc", txtDocumento.Text.Trim());
				sQLiteCommand2.Parameters.AddWithValue("@t", txtTelefono.Text.Trim());
				num = Convert.ToInt32(RuntimeHelpers.GetObjectValue(sQLiteCommand2.ExecuteScalar()));
			}
			else
			{
				num = Convert.ToInt32(RuntimeHelpers.GetObjectValue(objectValue));
			}
		}
		string value = cmbTipoEquipo.Text;
		if (Operators.CompareString(cmbTipoEquipo.SelectedItem?.ToString(), "Otros", TextCompare: false) == 0)
		{
			string text5 = txtNuevoTipoEquipo.Text.Trim();
			if (Operators.CompareString(text5, "", TextCompare: false) == 0)
			{
				Interaction.MsgBox("Debe ingresar el tipo de equipo.", MsgBoxStyle.Exclamation);
				txtNuevoTipoEquipo.Focus();
				return;
			}
			string text6 = TipoEquipoCatalog.ResolverOCrearTipo(text5);
			CargarTiposEquipo();
			int num2 = cmbTipoEquipo.Items.IndexOf(text6);
			if (num2 < 0)
			{
				num2 = cmbTipoEquipo.Items.Add(text6);
			}
			cmbTipoEquipo.SelectedIndex = num2;
			value = text6;
			txtNuevoTipoEquipo.Text = "";
			lblEspecificar.Visible = false;
			txtNuevoTipoEquipo.Visible = false;
		}
		string text2 = ManejarImagen(imgFoto1, text);
		string text3 = ManejarImagen(imgFoto2, text);
		string text4 = ManejarImagen(imgFoto3, text);
		if (!FormatoMoneda.TryParseMonetarioPositivo(txtPresupuesto.Text, out decimal presupuesto))
		{
			Interaction.MsgBox("El presupuesto ingresado no es válido. Corrija el valor antes de guardar.", MsgBoxStyle.Exclamation, "Valor inválido");
			txtPresupuesto.Focus();
			return;
		}
		if (!FormatoMoneda.TryParseMonetarioPositivo(txtAbono.Text, out decimal abono))
		{
			Interaction.MsgBox("El abono ingresado no es válido. Corrija el valor antes de guardar.", MsgBoxStyle.Exclamation, "Valor inválido");
			txtAbono.Focus();
			return;
		}
		decimal total = presupuesto - abono;
		string commandText = (flag ? "INSERT INTO ordenes (fecha, id_cliente, tipo_equipo, marca, modelo, imei, clave, accesorios, falla, observaciones, reparacion, estado_entrega, abono, presupuesto, total, imagen1, imagen2, imagen3, reparado, entregado) VALUES (@fecha, @cliente, @tipo, @marca, @modelo, @imei, @clave, @accesorios, @falla, @obs, @reparacion, @estado, @abono, @presupuesto, @total, @foto1, @foto2, @foto3, @reparado, @entregado)" : "UPDATE ordenes SET fecha=@fecha, id_cliente=@cliente, tipo_equipo=@tipo, marca=@marca, modelo=@modelo, imei=@imei, clave=@clave, accesorios=@accesorios, falla=@falla, observaciones=@obs, reparacion=@reparacion, estado_entrega=@estado, abono=@abono, presupuesto=@presupuesto, total=@total, imagen1=@foto1, imagen2=@foto2, imagen3=@foto3, reparado=@reparado, entregado=@entregado WHERE id_orden=@id");
		using (SQLiteCommand sQLiteCommand3 = new SQLiteCommand(commandText, sQLiteConnection))
		{
			sQLiteCommand3.Parameters.AddWithValue("@fecha", txtFecha.Text);
			sQLiteCommand3.Parameters.AddWithValue("@cliente", num);
			sQLiteCommand3.Parameters.AddWithValue("@tipo", value);
			sQLiteCommand3.Parameters.AddWithValue("@marca", txtMarca.Text);
			sQLiteCommand3.Parameters.AddWithValue("@modelo", txtModelo.Text);
			sQLiteCommand3.Parameters.AddWithValue("@imei", txtIMEI.Text);
			sQLiteCommand3.Parameters.AddWithValue("@clave", txtClave.Text);
			sQLiteCommand3.Parameters.AddWithValue("@accesorios", txtAccesorios.Text);
			sQLiteCommand3.Parameters.AddWithValue("@falla", txtFalla.Text);
			sQLiteCommand3.Parameters.AddWithValue("@obs", txtObservaciones.Text);
			sQLiteCommand3.Parameters.AddWithValue("@reparacion", txtReparacion.Text);
			sQLiteCommand3.Parameters.AddWithValue("@estado", cmbEstadoEntrega.Text);
			sQLiteCommand3.Parameters.AddWithValue("@abono", abono);
			sQLiteCommand3.Parameters.AddWithValue("@presupuesto", presupuesto);
			sQLiteCommand3.Parameters.AddWithValue("@total", total);
			sQLiteCommand3.Parameters.AddWithValue("@reparado", txtReparado.Text);
			sQLiteCommand3.Parameters.AddWithValue("@entregado", txtEntregado.Text);
			sQLiteCommand3.Parameters.AddWithValue("@foto1", RuntimeHelpers.GetObjectValue(string.IsNullOrEmpty(text2) ? ((IConvertible)DBNull.Value) : ((IConvertible)text2)));
			sQLiteCommand3.Parameters.AddWithValue("@foto2", RuntimeHelpers.GetObjectValue(string.IsNullOrEmpty(text3) ? ((IConvertible)DBNull.Value) : ((IConvertible)text3)));
			sQLiteCommand3.Parameters.AddWithValue("@foto3", RuntimeHelpers.GetObjectValue(string.IsNullOrEmpty(text4) ? ((IConvertible)DBNull.Value) : ((IConvertible)text4)));
			if (!flag)
			{
				sQLiteCommand3.Parameters.AddWithValue("@id", txtOrden.Text);
			}
			sQLiteCommand3.ExecuteNonQuery();
		}
		if (flag)
		{
			using SQLiteCommand sQLiteCommand4 = new SQLiteCommand("SELECT last_insert_rowid()", sQLiteConnection);
			txtOrden.Text = sQLiteCommand4.ExecuteScalar().ToString();
		}
		Interaction.MsgBox("Orden guardada correctamente.", MsgBoxStyle.Information);
		if (Operators.CompareString(modConexion.ObtenerConfiguracion("imprimir_al_guardar"), "true", TextCompare: false) == 0)
		{
			Imprimir();
		}
		btnNuevaOrden_Click(null, EventArgs.Empty);
	}

	private string ManejarImagen(PictureBox picBox, string carpetaImagenes)
	{
		if (picBox.Tag != null && File.Exists(picBox.Tag.ToString()))
		{
			string text = picBox.Tag.ToString();
			string fileName = Path.GetFileName(text);
			string text2 = Path.Combine(carpetaImagenes, fileName);
			int num = 1;
			string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(fileName);
			string extension = Path.GetExtension(fileName);
			while (File.Exists(text2))
			{
				text2 = Path.Combine(carpetaImagenes, $"{fileNameWithoutExtension}_{num}{extension}");
				num = checked(num + 1);
			}
			File.Copy(text, text2);
			return "imagenes\\" + Path.GetFileName(text2);
		}
		return "";
	}

	public void buscaNumero()
	{
		string text = "";
		bool flag = false;
		do
		{
			text = Interaction.InputBox("Ingresa el Número de Orden", "Buscar Orden");
			if (string.IsNullOrEmpty(text))
			{
				return;
			}
			string s = text;
			int result = 0;
			flag = int.TryParse(s, out result);
			if (!flag)
			{
				Interaction.MsgBox("Por favor ingrese solo números.", MsgBoxStyle.Exclamation, "Entrada no válida");
			}
		}
		while (!flag && !string.IsNullOrEmpty(text));
		txtOrden.Text = text;
	}

	private void DibujarTextoConFondo(PrintPageEventArgs e, string texto, Font fuente, int y, int padding = 10)
	{
		SizeF sizeF = e.Graphics.MeasureString(texto, fuente);
		int num;
		int num2;
		checked
		{
			num = (int)Math.Round(sizeF.Width) + padding * 2;
			num2 = (int)Math.Round(sizeF.Height) + unchecked(padding / 2);
		}
		int num3 = checked(e.PageBounds.Width - num) / 2;
		e.Graphics.FillRectangle(Brushes.Black, num3, y, num, num2);
		checked
		{
			e.Graphics.DrawString(texto, fuente, Brushes.White, num3 + padding, y + unchecked(padding / 4));
		}
	}

	public void ImprimirCarta()
	{
		try
		{
			PrintDocument printDocument = new PrintDocument();
			int num = 90;
			bool flag = true;
			string left = "CODE128";
			string text = modConexion.ObtenerConfiguracion("impresora");
			if (!string.IsNullOrEmpty(text))
			{
				printDocument.PrinterSettings.PrinterName = text;
			}
			string text2 = modConexion.ObtenerConfiguracion("fuente_ticket");
			if (!string.IsNullOrEmpty(text2))
			{
				try
				{
					string[] array = text2.Split(',');
					string familyName = array[0].Trim();
					float emSize = ((array.Length > 1) ? float.Parse(array[1].Trim()) : 10f);
					fuenteTicket = new Font(familyName, emSize, FontStyle.Bold);
				}
				catch (Exception ex)
				{
					ProjectData.SetProjectError(ex);
					Exception ex2 = ex;
					fuenteTicket = new Font("Courier New", 10f, FontStyle.Bold);
					ProjectData.ClearProjectError();
				}
			}
			else
			{
				fuenteTicket = new Font("Courier New", 10f, FontStyle.Bold);
			}
			Font font = new Font(fuenteTicket.FontFamily, fuenteTicket.Size + 1f, FontStyle.Bold);
			Font font2 = new Font(fuenteTicket.FontFamily, fuenteTicket.Size + 2f, FontStyle.Bold);
			Font font3 = new Font(fuenteTicket.FontFamily, fuenteTicket.Size + 2f, FontStyle.Bold);
			printDocument.PrintPage += checked([SpecialName] (object sender, PrintPageEventArgs e) =>
			{
				int num2 = 40;
				int num3 = 120;
				int num4 = 50;
				int num5 = fuenteTicket.Height + 4;
				int num6 = e.PageBounds.Width - num2 - num3;
				int num7 = e.PageBounds.Width - num2 * 2;
				int num8 = 100;
				int num9 = 80;
				if (picLogo.Image != null)
				{
					e.Graphics.DrawImage(picLogo.Image, num2, num4, num8, num9);
				}
				int num10 = num2 + num8 + 20;
				e.Graphics.DrawString(Text.ToUpper(), font2, Brushes.Black, num10, num4);
				num4 += font2.Height + 5;
				e.Graphics.DrawString(EmpresaCorreo.Text, font3, Brushes.Black, num10, num4);
				num4 += font3.Height + 2;
				e.Graphics.DrawString(EmpresaDireccion.Text, font3, Brushes.Black, num10, num4);
				string s = "ORDEN N°: " + txtOrden.Text.PadLeft(6, '0');
				int num11 = (int)Math.Round(e.Graphics.MeasureString(s, font).Width);
				e.Graphics.DrawString(s, font, Brushes.Black, num2 + (num6 - num11), num4);
				num4 += num5;
				num4 = num2 + num9 + 2;
				DibujarTextoConFondo(e, "ORDEN DE SERVICIO", font, num4);
				num4 += (int)Math.Round(e.Graphics.MeasureString("ORDEN DE SERVICIO", font).Height) + 2;
				DibujarLinea(e, new string('_', num), num2, num4);
				num4 += num5;
				DibujarLineaDosColumnas(e, "Fecha:", txtFecha.Text, "Cliente:", txtNombre.Text, num - (int)Math.Round((double)num2 / 3.0), num2, num4);
				num4 += num5;
				DibujarLineaDosColumnas(e, "Documento:", txtDocumento.Text, "Teléfono:", txtTelefono.Text, num - (int)Math.Round((double)num2 / 3.0), num2, num4);
				num4 += num5;
				if (!string.IsNullOrEmpty(txtDireccion.Text))
				{
					DibujarTextoMultilinea(e, "Dir/Correo: " + txtDireccion.Text, num2, num4, num6);
					num4 += ObtenerAlturaTexto(e, "Dir/Correo: " + txtDireccion.Text, num6, fuenteTicket);
				}
				DibujarLinea(e, new string('_', num), num2, num4);
				num4 += num5;
				string valor = cmbTipoEquipo.Text;
				DibujarLineaDosColumnas(e, "Equipo:", valor, "Marca:", txtMarca.Text, num - (int)Math.Round((double)num2 / 3.0), num2, num4);
				num4 += num5;
				DibujarLineaDosColumnas(e, "Modelo:", txtModelo.Text, "IMEI/Serie:", txtIMEI.Text, num - (int)Math.Round((double)num2 / 3.0), num2, num4);
				num4 += num5;
				if (!string.IsNullOrEmpty(txtAccesorios.Text))
				{
					DibujarTextoMultilinea(e, "Accesorios: " + txtAccesorios.Text, num2, num4, num6);
					num4 += ObtenerAlturaTexto(e, "Accesorios: " + txtAccesorios.Text, num6, fuenteTicket);
				}
				num4 += num5;
				DibujarLinea(e, "FALLA REPORTADA:", num2, num4);
				num4 += num5;
				DibujarTextoMultilinea(e, txtFalla.Text, num2, num4, num6);
				num4 += ObtenerAlturaTexto(e, txtFalla.Text, num6, fuenteTicket) + num5;
				if (!string.IsNullOrEmpty(txtObservaciones.Text))
				{
					DibujarLinea(e, "OBSERVACIONES:", num2, num4);
					num4 += num5;
					DibujarTextoMultilinea(e, txtObservaciones.Text, num2, num4, num6);
					num4 += ObtenerAlturaTexto(e, txtObservaciones.Text, num6, fuenteTicket) + num5;
				}
				if (!string.IsNullOrEmpty(txtReparacion.Text))
				{
					DibujarLinea(e, "REPARACIÓN:", num2, num4);
					num4 += num5;
					DibujarTextoMultilinea(e, txtReparacion.Text, num2, num4, num6);
					num4 += ObtenerAlturaTexto(e, txtReparacion.Text, num6, fuenteTicket);
				}
				DibujarLinea(e, new string('_', num), num2, num4);
				num4 += num5;
				DibujarTextoMultilinea(e, "Estado: " + cmbEstadoEntrega.Text, num2, num4, num6);
				num4 += ObtenerAlturaTexto(e, "Estado: " + cmbEstadoEntrega.Text, num6, fuenteTicket);
				if (FormatoMoneda.TryParseMonetario(txtPresupuesto.Text, out decimal presupuesto2) && presupuesto2 > 0m)
				{
					DibujarLineaAlineadaDerecha(e, "Presupuesto: " + FormatoMoneda.Formatear(presupuesto2), num6, num2, num4);
					num4 += num5;
				}
				if (FormatoMoneda.TryParseMonetario(txtAbono.Text, out decimal abono2) && abono2 > 0m)
				{
					DibujarLineaAlineadaDerecha(e, "Abono: " + FormatoMoneda.Formatear(abono2), num6, num2, num4);
					num4 += num5;
				}
				if (FormatoMoneda.TryParseMonetario(txtTotal.Text, out decimal total2) && total2 > 0m)
				{
					DibujarLineaAlineadaDerecha(e, "RESTA: " + FormatoMoneda.Formatear(total2), num6, num2, num4);
					num4 += num5;
				}
				if (flag)
				{
					string codigo = txtOrden.Text.PadLeft(6, '0');
					int altura = 50;
					int ancho = (int)Math.Round((double)num6 * 0.2);
					int num12 = num4 - num5 * 3;
					if (Operators.CompareString(left, "CODE128", TextCompare: false) == 0)
					{
						DibujarCodigoBarras128(e, codigo, num2, num12, ancho, altura);
					}
					else
					{
						DibujarCodigoBarras39(e, codigo, num2, num12, ancho, altura);
					}
					num4 += num5;
				}
				DibujarLinea(e, ObtenerCondicionesTexto(), num2, num4);
				num4 += num5;
				e.HasMorePages = false;
			});
			printDocument.Print();
		}
		catch (Exception ex3)
		{
			ProjectData.SetProjectError(ex3);
			Exception ex4 = ex3;
			Interaction.MsgBox("Error al imprimir: " + ex4.Message, MsgBoxStyle.Critical);
			ProjectData.ClearProjectError();
		}
	}

	private void CentrarTexto2(PrintPageEventArgs e, string texto, Font fuente, int y, int anchoMaximo, int margenIzq)
	{
		checked
		{
			int num = (int)Math.Round(e.Graphics.MeasureString(texto, fuente).Width);
			int num2 = margenIzq + unchecked(checked(anchoMaximo - num) / 2);
			e.Graphics.DrawString(texto, fuente, Brushes.Black, num2, y);
		}
	}

	public void ImprimirTicket()
	{
		checked
		{
			try
			{
				PrintDocument printDocument = new PrintDocument();
				int num = 24;
				bool flag = true;
				string left = "CODE128";
				string text = modConexion.ObtenerConfiguracion("impresora");
				if (!string.IsNullOrEmpty(text))
				{
					printDocument.PrinterSettings.PrinterName = text;
				}
				string text2 = modConexion.ObtenerConfiguracion("fuente_ticket");
				if (!string.IsNullOrEmpty(text2))
				{
					try
					{
						string[] array = text2.Split(',');
						string familyName = array[0].Trim();
						float emSize = ((array.Length > 1) ? float.Parse(array[1].Trim()) : 7f);
						fuenteTicket = new Font(familyName, emSize, FontStyle.Bold);
					}
					catch (Exception ex)
					{
						ProjectData.SetProjectError(ex);
						Exception ex2 = ex;
						fuenteTicket = new Font("Courier New", 7f, FontStyle.Bold);
						ProjectData.ClearProjectError();
					}
				}
				else
				{
					fuenteTicket = new Font("Courier New", 7f, FontStyle.Bold);
				}
				Font fuente = new Font(fuenteTicket.FontFamily, 8f, FontStyle.Bold);
				Font font = new Font(fuenteTicket.FontFamily, 9f, FontStyle.Bold);
				int num2 = 1;
				printDocument.PrintPage += [SpecialName] (object sender, PrintPageEventArgs e) =>
				{
					int num3 = 0;
					int num4 = fuenteTicket.Height + 1;
					int num5 = (int)Math.Round((double)e.PageBounds.Width / 2.5);
					int num6 = (int)Math.Round(e.Graphics.MeasureString(new string('W', num), fuenteTicket).Width);
					if (picLogo.Image != null)
					{
						double num7 = (double)picLogo.Image.Width / (double)picLogo.Image.Height;
						int num8 = (int)Math.Round((double)num6 * 0.5);
						int num9 = (int)Math.Round((double)num8 * 1.4);
						int num10 = (int)Math.Round((double)num9 / num7);
						int num11 = num5 - unchecked(num9 / 2);
						e.Graphics.DrawImage(picLogo.Image, num11, num3, num9, num10);
						num3 += num10 + num4;
					}
					CentrarTexto(e, Text.ToUpper(), fuente, num3);
					num3 += num4 * 1;
					CentrarTexto(e, EmpresaCorreo.Text, fuente, num3); // Correo centrado
                    num3 += num4 * 1;
					CentrarTexto(e, EmpresaDireccion.Text, fuente, num3); // Dirección centrada
                    num3 += num4 * 1;
					CentrarTexto(e, "ORDEN DE SERVICIO", fuente, num3); // Título centrado
                    num3 += num4 * 1;
					DibujarLinea(e, new string('-', num), num2, num3);
					num3 += num4 * 1;
					DibujarLineaDosColumnas(e, "Orden:", txtOrden.Text.PadLeft(6, '0'), " ", " ", num, num2, num3);
					num3 += num4;
					DibujarLineaDosColumnas(e, "Fecha:", txtFecha.Text, " ", " ", num, num2, num3);
					num3 += num4;
					DibujarLineaDosColumnas(e, "Documento:", txtDocumento.Text, " ", " ", num, num2, num3);
					num3 += num4;
					DibujarLineaDosColumnas(e, "Teléfono:", txtTelefono.Text, " ", " ", num, num2, num3);
					num3 += num4;
					DibujarTextoMultilinea(e, "Dir/Correo:" + txtDireccion.Text, num2, num3, num6);
					num3 += ObtenerAlturaTexto(e, "Dir/Correo:" + txtDireccion.Text, num6, fuenteTicket);
					DibujarTextoMultilinea(e, "Cliente: " + txtNombre.Text, num2, num3, num6);
					num3 += ObtenerAlturaTexto(e, "Cliente: " + txtNombre.Text, num6, fuenteTicket);
					DibujarLinea(e, new string('-', num), num2, num3);
					num3 += num4;
					string text3 = cmbTipoEquipo.Text;
					DibujarTextoMultilinea(e, "Equipo: " + text3, num2, num3, num6);
					num3 += ObtenerAlturaTexto(e, "Equipo: " + text3, num6, fuenteTicket);
					if (!string.IsNullOrEmpty(txtMarca.Text))
					{
						DibujarTextoMultilinea(e, "Marca: " + txtMarca.Text, num2, num3, num6);
						num3 += ObtenerAlturaTexto(e, "Marca: " + txtMarca.Text, num6, fuenteTicket);
						if (!string.IsNullOrEmpty(txtModelo.Text))
						{
							DibujarTextoMultilinea(e, "Modelo: " + txtModelo.Text, num2, num3, num6);
							num3 += ObtenerAlturaTexto(e, "Modelo: " + txtModelo.Text, num6, fuenteTicket);
						}
					}
					if (!string.IsNullOrEmpty(txtIMEI.Text))
					{
						DibujarTextoMultilinea(e, "IMEI/Serie: " + txtIMEI.Text, num2, num3, num6);
						num3 += ObtenerAlturaTexto(e, "IMEI/Serie: " + txtIMEI.Text, num6, fuenteTicket);
					}
					if (!string.IsNullOrEmpty(txtAccesorios.Text))
					{
						DibujarTextoMultilinea(e, "Accesorios: " + txtAccesorios.Text, num2, num3, num6);
						num3 += ObtenerAlturaTexto(e, "Accesorios: " + txtAccesorios.Text, num6, fuenteTicket);
					}
					DibujarLinea(e, new string('-', num), num2, num3);
					num3 += num4;
					DibujarLinea(e, "FALLA REPORTADA:", num2, num3);
					num3 += num4;
					DibujarTextoMultilinea(e, txtFalla.Text, num2, num3, num6);
					num3 += ObtenerAlturaTexto(e, txtFalla.Text, num6, fuenteTicket) + num4;
					if (!string.IsNullOrEmpty(txtObservaciones.Text))
					{
						DibujarLinea(e, "OBSERVACIONES:", num2, num3);
						num3 += num4;
						DibujarTextoMultilinea(e, txtObservaciones.Text, num2, num3, num6);
						num3 += ObtenerAlturaTexto(e, txtObservaciones.Text, num6, fuenteTicket) + num4;
					}
					if (!string.IsNullOrEmpty(txtReparacion.Text))
					{
						DibujarLinea(e, "REPARACIÓN:", num2, num3);
						num3 += num4;
						DibujarTextoMultilinea(e, txtReparacion.Text, num2, num3, num6);
						num3 += ObtenerAlturaTexto(e, txtReparacion.Text, num6, fuenteTicket) + num4;
					}
					DibujarLinea(e, new string('-', num), num2, num3);
					num3 += num4;
					DibujarTextoMultilinea(e, "Estado: " + cmbEstadoEntrega.Text, num2, num3, num6);
					num3 += ObtenerAlturaTexto(e, "Estado: " + cmbEstadoEntrega.Text, num6, fuenteTicket);
					if (FormatoMoneda.TryParseMonetario(txtAbono.Text, out decimal abono3) && abono3 > 0m)
					{
						DibujarLineaAlineadaDerecha(e, "Abono: " + FormatoMoneda.Formatear(abono3), num6, num2, num3);
						num3 += num4;
					}
					if (FormatoMoneda.TryParseMonetario(txtPresupuesto.Text, out decimal presupuesto3) && presupuesto3 > 0m)
					{
						DibujarLineaAlineadaDerecha(e, "Presupuesto: " + FormatoMoneda.Formatear(presupuesto3), num6, num2, num3);
						num3 += num4;
					}
					if (FormatoMoneda.TryParseMonetario(txtTotal.Text, out decimal total3) && total3 > 0m)
					{
						DibujarLineaAlineadaDerecha(e, "TOTAL: " + FormatoMoneda.Formatear(total3), num6, num2, num3);
						num3 += num4;
					}
					DibujarLinea(e, new string('-', num), num2, num3);
					num3 += num4;
					if (!string.IsNullOrEmpty(txtReparado.Text) && !string.IsNullOrEmpty(txtEntregado.Text) && Operators.CompareString(txtReparado.Text, txtEntregado.Text, TextCompare: false) != 0)
					{
						DibujarLinea(e, "Reparado: " + txtReparado.Text, num2, num3);
						num3 += num4;
						DibujarLinea(e, "Entregado: " + txtEntregado.Text, num2, num3);
						num3 += num4;
					}
					DibujarLinea(e, new string('-', num), num2, num3);
					num3 += num4;
					if (flag)
					{
						string codigo = txtOrden.Text.PadLeft(6, '0');
						int num12 = 40;
						if (Operators.CompareString(left, "CODE128", TextCompare: false) == 0)
						{
							DibujarCodigoBarras128(e, codigo, 1, num3, num6 - 20, num12);
						}
						else
						{
							DibujarCodigoBarras39(e, codigo, 1, num3, num6 - 20, num12);
						}
						num3 += num12 + num4;
					}
					DibujarLinea(e, ObtenerCondicionesTexto(), num2, num3);
					num3 += num4;
					e.HasMorePages = false;
				};
				printDocument.Print();
			}
			catch (Exception ex3)
			{
				ProjectData.SetProjectError(ex3);
				Exception ex4 = ex3;
				Interaction.MsgBox("Error al imprimir: " + ex4.Message, MsgBoxStyle.Critical);
				ProjectData.ClearProjectError();
			}
		}
	}

	private void DibujarLineaDosColumnas(PrintPageEventArgs e, string texto1, string valor1, string texto2, string valor2, int caracteresPorLinea, int x, int y)
	{
		string s = texto1 + " " + valor1;
		string text = texto2 + " " + valor2;
		checked
		{
			int num = (int)Math.Round(e.Graphics.MeasureString(s, fuenteTicket).Width);
			int num2 = (int)Math.Round((float)caracteresPorLinea * fuenteTicket.Size - (float)num - 25f);
			if (e.Graphics.MeasureString(text, fuenteTicket).Width > (float)num2)
			{
				text = text.Substring(0, Math.Max(0, text.Length - 3)) + "..";
			}
			e.Graphics.DrawString(s, fuenteTicket, Brushes.Black, x, y);
			e.Graphics.DrawString(text, fuenteTicket, Brushes.Black, (float)x + (float)caracteresPorLinea * fuenteTicket.Size - e.Graphics.MeasureString(text, fuenteTicket).Width, y);
		}
	}

	private void DibujarLineaAlineadaDerecha(PrintPageEventArgs e, string texto, int anchoMaximo, int x, int y)
	{
		int num = checked(x + anchoMaximo - (int)Math.Round(e.Graphics.MeasureString(texto, fuenteTicket).Width));
		e.Graphics.DrawString(texto, fuenteTicket, Brushes.Black, num, y);
	}

	private void DibujarCodigoBarras128(PrintPageEventArgs e, string codigo, int x, int y, int ancho, int altura)
	{
		Barcode barcode = new Barcode();
		barcode.IncludeLabel = true;
		barcode.Alignment = AlignmentPositions.CENTER;
		barcode.LabelFont = fuenteTicket;
		barcode.Width = ancho;
		barcode.Height = altura;
		barcode.Encode(TYPE.CODE128, codigo);
		Image image = barcode.Encode(TYPE.CODE128, codigo, Color.Black, Color.White, ancho, altura);
		e.Graphics.DrawImage(image, x, y);
	}

	private void DibujarCodigoBarras39(PrintPageEventArgs e, string codigo, int x, int y, int ancho, int altura)
	{
		Barcode barcode = new Barcode();
		barcode.IncludeLabel = true;
		barcode.Alignment = AlignmentPositions.CENTER;
		barcode.LabelFont = fuenteTicket;
		barcode.Width = ancho;
		barcode.Height = altura;
		barcode.Encode(TYPE.CODE39, codigo);
		Image image = barcode.Encode(TYPE.CODE39, codigo, Color.Black, Color.White, ancho, altura);
		e.Graphics.DrawImage(image, x, y);
	}

	private void DibujarLinea(PrintPageEventArgs e, string texto, int x, int y)
	{
		e.Graphics.DrawString(texto, fuenteTicket, Brushes.Black, x, y);
	}

	private void CentrarTexto(PrintPageEventArgs e, string texto, Font fuente, int y)
	{
		int num = checked((int)Math.Round(e.Graphics.MeasureString(texto, fuente).Width));
		int num2 = checked(e.PageBounds.Width - num) / 2;
		e.Graphics.DrawString(texto, fuente, Brushes.Black, checked(num2 - 20), y);
	}

	private void DibujarTextoMultilinea(PrintPageEventArgs e, string texto, int x, int y, int anchoMaximo)
	{
		RectangleF layoutRectangle = new RectangleF(x, y, anchoMaximo, checked(e.PageBounds.Height - y));
		e.Graphics.DrawString(texto, fuenteTicket, Brushes.Black, layoutRectangle);
	}

	private int ObtenerAlturaTexto(PrintPageEventArgs e, string texto, int anchoMaximo, Font fuente)
	{
		return checked((int)Math.Round(e.Graphics.MeasureString(texto, fuente, anchoMaximo).Height));
	}

	public string ObtenerCondicionesTexto()
	{
		string result;
		try
		{
			string text = modConexion.ObtenerCondicionesServicio();
			result = ((!string.IsNullOrEmpty(text)) ? text : "1. No nos hacemos responsables por pérdida de datos.\r\n2. Garantía de 30 días en reparaciones.\r\n3. El equipo será entregado únicamente con comprobante.");
		}
		catch (Exception ex)
		{
			ProjectData.SetProjectError(ex);
			Exception ex2 = ex;
			MessageBox.Show("Error al obtener condiciones: " + ex2.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
			result = "1. No nos hacemos responsables por pérdida de datos.\r\n2. Garantía de 30 días en reparaciones.\r\n3. El equipo será entregado únicamente con comprobante.";
			ProjectData.ClearProjectError();
		}
		return result;
	}

	private void btnNuevaOrden_Click(object sender, EventArgs e)
	{
		NuevaOrden();
	}

	public void NuevaOrden()
	{
		limpiarCampos();
		imgFoto1.BackgroundImage = sinImagen.BackgroundImage;
		imgFoto2.BackgroundImage = sinImagen.BackgroundImage;
		imgFoto3.BackgroundImage = sinImagen.BackgroundImage;
		txtDocumento.Focus();
		cmbEstadoEntrega.SelectedIndex = 0;
		CargarTiposEquipo();
		cmbTipoEquipo.SelectedIndex = -1;
	}

	private void cmbTipoEquipo_SelectedIndexChanged(object sender, EventArgs e)
	{
		if (cargandoTiposEquipo)
		{
			return;
		}
		bool esOtros = cmbTipoEquipo.SelectedItem != null && TipoEquipoCatalog.IgualNombre(cmbTipoEquipo.SelectedItem.ToString(), "Otros");
		lblEspecificar.Visible = esOtros;
		txtNuevoTipoEquipo.Visible = esOtros;
		if (esOtros)
		{
			txtNuevoTipoEquipo.Focus();
		}
		else
		{
			txtNuevoTipoEquipo.Text = "";
		}
	}

	private void btnGuardar_Click(object sender, EventArgs e)
	{
		GuardarOrden();
	}

	private void btnImprimir_Click(object sender, EventArgs e)
	{
		if (string.IsNullOrEmpty(txtOrden.Text))
		{
			Interaction.MsgBox("No hay una orden cargada para imprimir.", MsgBoxStyle.Exclamation);
		}
		else
		{
			Imprimir();
		}
	}

	public void Imprimir()
	{
		string text = modConexion.ObtenerConfiguracion("tipo_impresion");
		string left = text.ToLower();
		if (Operators.CompareString(left, "ticket", TextCompare: false) != 0 && Operators.CompareString(left, "carta", TextCompare: false) != 0)
		{
			Interaction.MsgBox("El tipo de impresión configurado no es válido.", MsgBoxStyle.Exclamation);
			return;
		}
		OrdenImpresionData datos = CrearOrdenImpresionData();
		ConfiguracionImpresion config = CrearConfiguracionImpresion();
		try
		{
			ImpresorOrden impresor = new ImpresorOrden();
			impresor.Imprimir(datos, config);
		}
		catch (Exception ex)
		{
			ProjectData.SetProjectError(ex);
			Interaction.MsgBox("Error al imprimir: " + ex.Message, MsgBoxStyle.Critical);
			ProjectData.ClearProjectError();
		}
	}

	public void Previsualizar()
	{
		string text = modConexion.ObtenerConfiguracion("tipo_impresion");
		string left = text.ToLower();
		if (Operators.CompareString(left, "ticket", TextCompare: false) != 0 && Operators.CompareString(left, "carta", TextCompare: false) != 0)
		{
			Interaction.MsgBox("El tipo de impresión configurado no es válido.", MsgBoxStyle.Exclamation);
			return;
		}
		OrdenImpresionData datos = CrearOrdenImpresionData();
		ConfiguracionImpresion config = CrearConfiguracionImpresion();
		try
		{
			ImpresorOrden impresor = new ImpresorOrden();
			impresor.Previsualizar(datos, config);
		}
		catch (Exception ex)
		{
			ProjectData.SetProjectError(ex);
			Interaction.MsgBox("Error al previsualizar: " + ex.Message, MsgBoxStyle.Critical);
			ProjectData.ClearProjectError();
		}
	}

	// Construye el modelo de datos de impresion a partir de los controles actuales.
	// Aun NO se utiliza: se introdujo en MEJ-004 Paso 1 sin cambiar la impresion.
	private OrdenImpresionData CrearOrdenImpresionData()
	{
		decimal? presupuesto = null;
		if (FormatoMoneda.TryParseMonetario(txtPresupuesto.Text, out decimal presupuestoParseado))
		{
			presupuesto = presupuestoParseado;
		}
		decimal? abono = null;
		if (FormatoMoneda.TryParseMonetario(txtAbono.Text, out decimal abonoParseado))
		{
			abono = abonoParseado;
		}
		return new OrdenImpresionData
		{
			NumeroOrden = txtOrden.Text,
			Fecha = txtFecha.Text,
			Cliente = txtNombre.Text,
			Documento = txtDocumento.Text,
			Telefono = txtTelefono.Text,
			Direccion = txtDireccion.Text,
			TipoEquipo = cmbTipoEquipo.Text,
			Marca = txtMarca.Text,
			Modelo = txtModelo.Text,
			IMEI = txtIMEI.Text,
			Accesorios = txtAccesorios.Text,
			Falla = txtFalla.Text,
			Observaciones = txtObservaciones.Text,
			Reparacion = txtReparacion.Text,
			Estado = cmbEstadoEntrega.Text,
			FechaReparado = txtReparado.Text,
			FechaEntregado = txtEntregado.Text,
			Condiciones = ObtenerCondicionesTexto(),
			EmpresaNombre = Text,
			EmpresaCorreo = EmpresaCorreo.Text,
			EmpresaDireccion = EmpresaDireccion.Text,
			Presupuesto = presupuesto,
			Abono = abono,
			Logo = picLogo.Image
		};
	}

	// Construye la configuracion de impresion desde la tabla 'configuracion'.
	// Aun NO se utiliza: se introdujo en MEJ-004 Paso 1 sin cambiar la impresion.
	private ConfiguracionImpresion CrearConfiguracionImpresion()
	{
		ConfiguracionImpresion config = new ConfiguracionImpresion
		{
			Impresora = modConexion.ObtenerConfiguracion("impresora"),
			TipoImpresion = modConexion.ObtenerConfiguracion("tipo_impresion")
		};
		string textoFuente = modConexion.ObtenerConfiguracion("fuente_ticket");
		ConfiguracionImpresion.ParseFuente(textoFuente, 10f, out string nombre, out float tamano);
		config.FuenteNombre = nombre;
		config.TamanoFuente = tamano;
		return config;
	}

	private string ObtenerValorConfiguracion(string clave)
	{
		string result = "";
		try
		{
			if (modConexion.conexion.State == ConnectionState.Closed)
			{
				modConexion.conexion.Open();
			}
			string commandText = "SELECT valor FROM configuracion WHERE clave = ?";
			using SQLiteCommand sQLiteCommand = new SQLiteCommand(commandText, modConexion.conexion);
			sQLiteCommand.Parameters.AddWithValue("?", clave);
			object objectValue = RuntimeHelpers.GetObjectValue(sQLiteCommand.ExecuteScalar());
			if (objectValue != null)
			{
				result = objectValue.ToString();
			}
		}
		catch (Exception ex)
		{
			ProjectData.SetProjectError(ex);
			Exception ex2 = ex;
			Interaction.MsgBox("Error al leer configuración: " + ex2.Message);
			ProjectData.ClearProjectError();
		}
		finally
		{
			if (modConexion.conexion.State == ConnectionState.Open)
			{
				modConexion.conexion.Close();
			}
		}
		return result;
	}

	private void btnBuscarOrden_Click(object sender, EventArgs e)
	{
		using frmBuscaOrden frmBuscaOrden2 = new frmBuscaOrden();
		frmBuscaOrden2.FormBorderStyle = FormBorderStyle.FixedDialog;
		frmBuscaOrden2.MaximizeBox = false;
		frmBuscaOrden2.MinimizeBox = false;
		frmBuscaOrden2.StartPosition = FormStartPosition.CenterParent;
		if (frmBuscaOrden2.ShowDialog() == DialogResult.OK)
		{
			txtOrden.Text = frmBuscaOrden2.OrdenSeleccionada.ToString();
		}
	}

	private void btnBuscarCliente_Click(object sender, EventArgs e)
	{
		using frmClientes frmClientes2 = new frmClientes();
		frmClientes2.StartPosition = FormStartPosition.CenterParent;
		frmClientes2.FormBorderStyle = FormBorderStyle.FixedDialog;
		frmClientes2.MaximizeBox = false;
		frmClientes2.MinimizeBox = false;
		if (frmClientes2.ShowDialog() == DialogResult.OK)
		{
			Cliente clienteSeleccionado = frmClientes2.ClienteSeleccionado;
			txtNombre.Text = clienteSeleccionado.Nombre;
			txtDireccion.Text = clienteSeleccionado.Direccion;
			txtDocumento.Text = clienteSeleccionado.Documento;
			txtTelefono.Text = clienteSeleccionado.Telefono;
		}
	}

	private void btnEliminarOrden_Click(object sender, EventArgs e)
	{
		if (string.IsNullOrEmpty(txtOrden.Text) || !Versioned.IsNumeric(txtOrden.Text))
		{
			Interaction.MsgBox("No hay una orden seleccionada para eliminar.", MsgBoxStyle.Exclamation);
			return;
		}
		DialogResult dialogResult = (DialogResult)Interaction.MsgBox("¿Está seguro que desea eliminar la orden N° " + txtOrden.Text + "?\r\nEsta acción no se puede deshacer.", MsgBoxStyle.YesNo | MsgBoxStyle.Question, "Confirmar eliminación");
		if (dialogResult != DialogResult.Yes)
		{
			return;
		}
		try
		{
			using SQLiteConnection sQLiteConnection = new SQLiteConnection("Data Source=" + modConexion.rutaDB);
			sQLiteConnection.Open();
			using SQLiteCommand sQLiteCommand = new SQLiteCommand("DELETE FROM ordenes WHERE id_orden = @id", sQLiteConnection);
			sQLiteCommand.Parameters.AddWithValue("@id", txtOrden.Text);
			int num = sQLiteCommand.ExecuteNonQuery();
			if (num > 0)
			{
				Interaction.MsgBox("Orden eliminada correctamente.", MsgBoxStyle.Information);
				btnNuevaOrden_Click(null, EventArgs.Empty);
			}
			else
			{
				Interaction.MsgBox("No se encontró la orden especificada.", MsgBoxStyle.Exclamation);
				limpiarCampos();
			}
		}
		catch (Exception ex)
		{
			ProjectData.SetProjectError(ex);
			Exception ex2 = ex;
			Interaction.MsgBox("Error al eliminar la orden: " + ex2.Message, MsgBoxStyle.Critical);
			ProjectData.ClearProjectError();
		}
	}

	private void txtDocumento_KeyDown(object sender, KeyEventArgs e)
	{
		if (e.KeyCode == Keys.Return)
		{
			BuscarClientePorDocumento();
			e.SuppressKeyPress = true;
		}
	}

	private void txtTelefono_KeyDown(object sender, KeyEventArgs e)
	{
		if (e.KeyCode == Keys.Return)
		{
			BuscarClientePorTelefono();
			e.SuppressKeyPress = true;
		}
	}

	private void txtOrden_TextChanged(object sender, EventArgs e)
	{
		if (!string.IsNullOrEmpty(txtOrden.Text) && Versioned.IsNumeric(txtOrden.Text) && Operators.CompareString(txtOrden.Text, "0", TextCompare: false) != 0)
		{
			CargarOrden(txtOrden.Text);
		}
	}

	private void imgFoto1_Click(object sender, EventArgs e)
	{
		MostrarImagenEnFormulario(imgFoto1, "Foto 1 - Orden #" + txtOrden.Text);
	}

	private void imgFoto2_Click(object sender, EventArgs e)
	{
		MostrarImagenEnFormulario(imgFoto2, "Foto 2 - Orden #" + txtOrden.Text);
	}

	private void imgFoto3_Click(object sender, EventArgs e)
	{
		MostrarImagenEnFormulario(imgFoto3, "Foto 3 - Orden #" + txtOrden.Text);
	}

	public void limpiarCampos()
	{
		LimpiarControlesRecursivo(this);
		txtFecha.Text = DateTime.Now.ToString("dd/MM/yyyy");
		txtReparado.Text = "";
		txtEntregado.Text = "";
		cmbEstadoEntrega.SelectedIndex = -1;
		cmbTipoEquipo.SelectedIndex = -1;
		lblEspecificar.Visible = false;
		txtNuevoTipoEquipo.Visible = false;
		txtNuevoTipoEquipo.Text = "";
		LimpiarImagen(imgFoto1);
		LimpiarImagen(imgFoto2);
		LimpiarImagen(imgFoto3);
		txtOrden.Text = "";
	}

	private void CargarTiposEquipo()
	{
		cargandoTiposEquipo = true;
		try
		{
			cmbTipoEquipo.Items.Clear();
			foreach (TipoEquipo tipo in TipoEquipoCatalog.ObtenerTipos())
			{
				if (tipo.Activo == 1)
				{
					cmbTipoEquipo.Items.Add(tipo.Nombre);
				}
			}
		}
		finally
		{
			cargandoTiposEquipo = false;
		}
	}

	private void LimpiarControlesRecursivo(Control container)
	{
		foreach (Control control in container.Controls)
		{
			bool flag = true;
			if (flag == control is TextBox)
			{
				if (!((Operators.CompareString(control.Name, "queda", TextCompare: false) == 0) | (Operators.CompareString(control.Name, "TextBox1", TextCompare: false) == 0) | (Operators.CompareString(control.Name, "TextBox2", TextCompare: false) == 0) | (Operators.CompareString(control.Name, "TextBox3", TextCompare: false) == 0) | (Operators.CompareString(control.Name, "EmpresaDireccion", TextCompare: false) == 0) | (Operators.CompareString(control.Name, "EmpresaCorreo", TextCompare: false) == 0)))
				{
					TextBox textBox = (TextBox)control;
					textBox.Clear();
				}
			}
			else if (flag == control is ComboBox)
			{
				ComboBox comboBox = (ComboBox)control;
				comboBox.SelectedIndex = -1;
			}
			else if (flag == control is RadioButton)
			{
				RadioButton radioButton = (RadioButton)control;
				radioButton.Checked = false;
			}
			else if (flag == control is DateTimePicker)
			{
				DateTimePicker dateTimePicker = (DateTimePicker)control;
				dateTimePicker.Value = DateTime.Now;
			}
			if (control.HasChildren)
			{
				LimpiarControlesRecursivo(control);
			}
		}
	}

	private void LimpiarImagen(PictureBox picBox)
	{
		if (picBox.Image != null)
		{
			picBox.Image.Dispose();
			picBox.Image = null;
		}
		picBox.Tag = null;
	}

	private void BuscarClientePorDocumento()
	{
		string value = txtDocumento.Text.Trim();
		if (string.IsNullOrEmpty(value))
		{
			return;
		}
		try
		{
			using SQLiteConnection sQLiteConnection = new SQLiteConnection("Data Source=" + modConexion.rutaDB);
			sQLiteConnection.Open();
			using SQLiteCommand sQLiteCommand = new SQLiteCommand("SELECT nombre, direccion, telefono FROM clientes WHERE documento = @doc", sQLiteConnection);
			sQLiteCommand.Parameters.AddWithValue("@doc", value);
			using SQLiteDataReader sQLiteDataReader = sQLiteCommand.ExecuteReader();
			if (sQLiteDataReader.Read())
			{
				txtNombre.Text = sQLiteDataReader["nombre"].ToString();
				txtDireccion.Text = sQLiteDataReader["direccion"].ToString();
				txtTelefono.Text = sQLiteDataReader["telefono"].ToString();
			}
			else
			{
				txtNombre.Clear();
				txtDireccion.Clear();
				txtTelefono.Clear();
				Interaction.MsgBox("Cliente no registrado.", MsgBoxStyle.Information);
			}
		}
		catch (Exception ex)
		{
			ProjectData.SetProjectError(ex);
			Exception ex2 = ex;
			Interaction.MsgBox("Error al buscar cliente: " + ex2.Message, MsgBoxStyle.Critical);
			ProjectData.ClearProjectError();
		}
	}

	private void BuscarClientePorTelefono()
	{
		string value = txtTelefono.Text.Trim();
		if (string.IsNullOrEmpty(value))
		{
			return;
		}
		try
		{
			using SQLiteConnection sQLiteConnection = new SQLiteConnection("Data Source=" + modConexion.rutaDB);
			sQLiteConnection.Open();
			using SQLiteCommand sQLiteCommand = new SQLiteCommand("SELECT nombre, direccion, documento FROM clientes WHERE telefono = @tel OR telefono LIKE '%' || @tel || '%'", sQLiteConnection);
			sQLiteCommand.Parameters.AddWithValue("@tel", value);
			using SQLiteDataReader sQLiteDataReader = sQLiteCommand.ExecuteReader();
			if (sQLiteDataReader.Read())
			{
				txtNombre.Text = sQLiteDataReader["nombre"].ToString();
				txtDireccion.Text = sQLiteDataReader["direccion"].ToString();
				txtDocumento.Text = sQLiteDataReader["documento"].ToString();
			}
			else
			{
				txtNombre.Clear();
				txtDireccion.Clear();
				txtDocumento.Clear();
				Interaction.MsgBox("Cliente no registrado con este teléfono.", MsgBoxStyle.Information);
			}
		}
		catch (Exception ex)
		{
			ProjectData.SetProjectError(ex);
			Exception ex2 = ex;
			Interaction.MsgBox("Error al buscar cliente: " + ex2.Message, MsgBoxStyle.Critical);
			ProjectData.ClearProjectError();
		}
	}

	private void CargarOrden(string numeroOrden)
	{
		try
		{
			LimpiarImagen(imgFoto1);
			LimpiarImagen(imgFoto2);
			LimpiarImagen(imgFoto3);
			using SQLiteConnection sQLiteConnection = new SQLiteConnection("Data Source=" + modConexion.rutaDB);
			sQLiteConnection.Open();
			string commandText = "SELECT o.*, c.nombre, c.direccion, c.documento, c.telefono FROM ordenes o INNER JOIN clientes c ON o.id_cliente = c.id_cliente WHERE o.id_orden = @id_orden";
			using SQLiteCommand sQLiteCommand = new SQLiteCommand(commandText, sQLiteConnection);
			sQLiteCommand.Parameters.AddWithValue("@id_orden", numeroOrden);
			using SQLiteDataReader sQLiteDataReader = sQLiteCommand.ExecuteReader();
			if (sQLiteDataReader.Read())
			{
				txtNombre.Text = sQLiteDataReader["nombre"].ToString();
				txtDireccion.Text = sQLiteDataReader["direccion"].ToString();
				txtDocumento.Text = sQLiteDataReader["documento"].ToString();
				txtTelefono.Text = sQLiteDataReader["telefono"].ToString();
				string text = sQLiteDataReader["tipo_equipo"].ToString();
				int num = cmbTipoEquipo.Items.IndexOf(text);
				if (num >= 0)
				{
					cmbTipoEquipo.SelectedIndex = num;
				}
				else if (!string.IsNullOrEmpty(text))
				{
					cmbTipoEquipo.Items.Add(text);
					cmbTipoEquipo.SelectedIndex = cmbTipoEquipo.Items.Count - 1;
				}
				else
				{
					cmbTipoEquipo.SelectedIndex = -1;
				}
				txtMarca.Text = sQLiteDataReader["marca"].ToString();
				txtModelo.Text = sQLiteDataReader["modelo"].ToString();
				txtIMEI.Text = sQLiteDataReader["imei"].ToString();
				txtClave.Text = sQLiteDataReader["clave"].ToString();
				txtAccesorios.Text = sQLiteDataReader["accesorios"].ToString();
				txtFalla.Text = sQLiteDataReader["falla"].ToString();
				txtObservaciones.Text = sQLiteDataReader["observaciones"].ToString();
				txtReparacion.Text = sQLiteDataReader["reparacion"].ToString();
				txtReparado.Text = sQLiteDataReader["reparado"].ToString();
				txtEntregado.Text = sQLiteDataReader["entregado"].ToString();
				cmbEstadoEntrega.Text = sQLiteDataReader["estado_entrega"].ToString();
				txtAbono.Text = FormatoMoneda.FormatearDesdeObjeto(sQLiteDataReader["abono"]);
				txtPresupuesto.Text = FormatoMoneda.FormatearDesdeObjeto(sQLiteDataReader["presupuesto"]);
				txtTotal.Text = FormatoMoneda.FormatearDesdeObjeto(sQLiteDataReader["total"]);
				txtFecha.Text = Convert.ToDateTime(RuntimeHelpers.GetObjectValue(sQLiteDataReader["fecha"])).ToString("dd/MM/yyyy");
				CargarImagen(sQLiteDataReader, "imagen1", imgFoto1);
				CargarImagen(sQLiteDataReader, "imagen2", imgFoto2);
				CargarImagen(sQLiteDataReader, "imagen3", imgFoto3);
			}
			else
			{
				Interaction.MsgBox("No se encontró la orden especificada.", MsgBoxStyle.Exclamation);
				limpiarCampos();
			}
		}
		catch (Exception ex)
		{
			ProjectData.SetProjectError(ex);
			Exception ex2 = ex;
			Interaction.MsgBox("Error al cargar la orden: " + ex2.Message, MsgBoxStyle.Critical);
			ProjectData.ClearProjectError();
		}
	}

	private void CargarImagen(SQLiteDataReader reader, string campoImagen, PictureBox picBox)
	{
		if (reader.IsDBNull(reader.GetOrdinal(campoImagen)))
		{
			return;
		}
		string text = reader[campoImagen].ToString();
		if (!string.IsNullOrEmpty(text))
		{
			string text2 = Path.Combine(Application.StartupPath, text);
			if (File.Exists(text2))
			{
				picBox.Image = Image.FromFile(text2);
				picBox.Tag = text2;
			}
		}
	}

	private string GenerarNuevoNumeroOrden()
	{
		int num = 1;
		try
		{
			using SQLiteConnection sQLiteConnection = new SQLiteConnection("Data Source=" + modConexion.rutaDB);
			sQLiteConnection.Open();
			using SQLiteCommand sQLiteCommand = new SQLiteCommand("SELECT MAX(id_orden) FROM ordenes", sQLiteConnection);
			object objectValue = RuntimeHelpers.GetObjectValue(sQLiteCommand.ExecuteScalar());
			if (objectValue != null && !Information.IsDBNull(RuntimeHelpers.GetObjectValue(objectValue)))
			{
				num = checked(Convert.ToInt32(RuntimeHelpers.GetObjectValue(objectValue)) + 1);
			}
		}
		catch (Exception ex)
		{
			ProjectData.SetProjectError(ex);
			Exception ex2 = ex;
			MessageBox.Show("Error al generar nuevo número de orden: " + ex2.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
			ProjectData.ClearProjectError();
		}
		txtOrden.TextChanged -= txtOrden_TextChanged;
		txtOrden.Text = num.ToString();
		txtOrden.TextChanged += txtOrden_TextChanged;
		return num.ToString();
	}

	private void frmOrdenServicio_FormClosing(object sender, FormClosingEventArgs e)
	{
		if (fuenteTicket != null)
		{
			fuenteTicket.Dispose();
		}
		if (_layoutSizer != null)
		{
			_layoutSizer.Dispose();
			_layoutSizer = null;
		}
	}

	private void Timer1_Tick(object sender, EventArgs e)
	{
		txtDocumento.Focus();
		Timer1.Enabled = false;
	}

	private void btnConfiguracion_Click(object sender, EventArgs e)
	{
		string tipoAnterior = cmbTipoEquipo.SelectedItem != null ? cmbTipoEquipo.SelectedItem.ToString() : cmbTipoEquipo.Text;
		string nuevoTipoAnterior = txtNuevoTipoEquipo.Text;
		bool estabaEnAltaRapida = cmbTipoEquipo.SelectedItem != null && TipoEquipoCatalog.IgualNombre(cmbTipoEquipo.SelectedItem.ToString(), "Otros");

		using frmConfiguracion frmConfiguracion2 = new frmConfiguracion();
		frmConfiguracion2.FormBorderStyle = FormBorderStyle.FixedDialog;
		frmConfiguracion2.MaximizeBox = false;
		frmConfiguracion2.MinimizeBox = false;
		frmConfiguracion2.ControlBox = true;
		frmConfiguracion2.ShowDialog();

		CargarTiposEquipo();

		if (string.IsNullOrEmpty(tipoAnterior))
		{
			cmbTipoEquipo.SelectedIndex = -1;
		}
		else
		{
			int num = cmbTipoEquipo.Items.IndexOf(tipoAnterior);
			if (num >= 0)
			{
				cmbTipoEquipo.SelectedIndex = num;
			}
			else
			{
				cmbTipoEquipo.Items.Add(tipoAnterior);
				cmbTipoEquipo.SelectedIndex = cmbTipoEquipo.Items.Count - 1;
			}
		}

		if (estabaEnAltaRapida)
		{
			lblEspecificar.Visible = true;
			txtNuevoTipoEquipo.Visible = true;
			txtNuevoTipoEquipo.Text = nuevoTipoAnterior;
		}
	}

	private void Panel1_Paint(object sender, PaintEventArgs e)
	{
	}

	private void Button1_Click(object sender, EventArgs e)
	{
		buscaNumero();
	}

	private void txtPresupuesto_TextChanged(object sender, EventArgs e)
	{
		campoPresupuesto.AlTextChanged();
		calcula();
	}

	private void txtAbono_TextChanged(object sender, EventArgs e)
	{
		campoAbono.AlTextChanged();
		calcula();
	}

	public void calcula()
	{
		if (FormatoMoneda.TryParseMonetario(txtPresupuesto.Text, out decimal presupuesto) && FormatoMoneda.TryParseMonetario(txtAbono.Text, out decimal abono))
		{
			decimal num = presupuesto - abono;
			txtTotal.Text = FormatoMoneda.Formatear(num);
		}
		else
		{
			txtTotal.Text = FormatoMoneda.Formatear(0m);
		}
	}

	private void btnCondiciones_Click(object sender, EventArgs e)
	{
		frmCondicionesServicio frmCondicionesServicio2 = new frmCondicionesServicio();
		frmCondicionesServicio2.FormBorderStyle = FormBorderStyle.FixedDialog;
		frmCondicionesServicio2.MaximizeBox = false;
		frmCondicionesServicio2.MinimizeBox = false;
		frmCondicionesServicio2.ControlBox = true;
		frmCondicionesServicio2.StartPosition = FormStartPosition.CenterParent;
		frmCondicionesServicio2.Text = "Condiciones del Servicio";
		frmCondicionesServicio2.ShowDialog();
		if (frmCondicionesServicio2.DialogResult == DialogResult.OK)
		{
			MessageBox.Show("Las condiciones se actualizaron correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
		}
		frmCondicionesServicio2.Dispose();
	}

	private void btnReporte_Click(object sender, EventArgs e)
	{
		frmReportesServicios frmReportesServicios2 = new frmReportesServicios();
		frmReportesServicios2.StartPosition = FormStartPosition.CenterScreen;
		frmReportesServicios2.ShowDialog();
		frmReportesServicios2.Dispose();
	}

	private void VER1_Click(object sender, EventArgs e)
	{
		using OpenFileDialog openFileDialog = new OpenFileDialog();
		openFileDialog.Filter = "Imágenes|*.jpg;*.jpeg;*.png;*.bmp";
		openFileDialog.Title = "Seleccionar imagen del equipo";
		if (openFileDialog.ShowDialog() == DialogResult.OK)
		{
			try
			{
				imgFoto1.Image = Image.FromFile(openFileDialog.FileName);
				imgFoto1.Tag = openFileDialog.FileName;
				return;
			}
			catch (Exception ex)
			{
				ProjectData.SetProjectError(ex);
				Exception ex2 = ex;
				Interaction.MsgBox("Error al cargar la imagen: " + ex2.Message, MsgBoxStyle.Exclamation);
				ProjectData.ClearProjectError();
				return;
			}
		}
	}

	private void VER2_Click(object sender, EventArgs e)
	{
		using OpenFileDialog openFileDialog = new OpenFileDialog();
		openFileDialog.Filter = "Imágenes|*.jpg;*.jpeg;*.png;*.bmp";
		openFileDialog.Title = "Seleccionar imagen del equipo";
		if (openFileDialog.ShowDialog() == DialogResult.OK)
		{
			try
			{
				imgFoto2.Image = Image.FromFile(openFileDialog.FileName);
				imgFoto2.Tag = openFileDialog.FileName;
				return;
			}
			catch (Exception ex)
			{
				ProjectData.SetProjectError(ex);
				Exception ex2 = ex;
				Interaction.MsgBox("Error al cargar la imagen: " + ex2.Message, MsgBoxStyle.Exclamation);
				ProjectData.ClearProjectError();
				return;
			}
		}
	}

	private void VER3_Click(object sender, EventArgs e)
	{
		using OpenFileDialog openFileDialog = new OpenFileDialog();
		openFileDialog.Filter = "Imágenes|*.jpg;*.jpeg;*.png;*.bmp";
		openFileDialog.Title = "Seleccionar imagen del equipo";
		if (openFileDialog.ShowDialog() == DialogResult.OK)
		{
			try
			{
				imgFoto3.Image = Image.FromFile(openFileDialog.FileName);
				imgFoto3.Tag = openFileDialog.FileName;
				return;
			}
			catch (Exception ex)
			{
				ProjectData.SetProjectError(ex);
				Exception ex2 = ex;
				Interaction.MsgBox("Error al cargar la imagen: " + ex2.Message, MsgBoxStyle.Exclamation);
				ProjectData.ClearProjectError();
				return;
			}
		}
	}

	private void MostrarImagenEnFormulario(PictureBox picBox, string titulo)
	{
		if (picBox.Image == null)
		{
			MessageBox.Show("No hay imagen para mostrar.", "Información", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
			return;
		}
		try
		{
			Image image = new Bitmap(picBox.Image);
			using (frmFoto frmFoto2 = new frmFoto(image, titulo))
			{
				frmFoto2.StartPosition = FormStartPosition.CenterScreen;
				frmFoto2.ShowDialog();
			}
			image.Dispose();
		}
		catch (Exception ex)
		{
			ProjectData.SetProjectError(ex);
			Exception ex2 = ex;
			MessageBox.Show("Error al mostrar la imagen: " + ex2.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
			ProjectData.ClearProjectError();
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization)]
	private void licencia_Tick(object sender, EventArgs e)
	{
		try
		{
			queda.Text = Conversions.ToString(Conversion.Val(queda.Text) - 1.0);
			if (Conversions.ToDouble(queda.Text) == 0.0)
			{
				Interaction.MsgBox("Gracias por usar este Software, Esta es la Versión Gratuita!, **En la paga quita este cartel y le colocamos logo y nombre de taller**");
				queda.Text = "30";
			}
		}
		catch (Exception ex)
		{
			ProjectData.SetProjectError(ex);
			Exception ex2 = ex;
			Interaction.MsgBox("Gracias por usar este Software, Esta es la Versión Gratuita!, **En la paga quita este cartel y le colocamos logo y nombre de taller**");
			ProjectData.EndApp();
			ProjectData.ClearProjectError();
		}
	}

	private readonly CampoMonetario campoPresupuesto;

	private readonly CampoMonetario campoAbono;

	private void txtPresupuesto_Enter(object sender, EventArgs e)
	{
		campoPresupuesto.AlEntrar();
	}

	private void txtAbono_Enter(object sender, EventArgs e)
	{
		campoAbono.AlEntrar();
	}

	private void txtPresupuesto_KeyDown(object sender, KeyEventArgs e)
	{
		campoPresupuesto.AlKeyDown(e);
	}

	private void txtAbono_KeyDown(object sender, KeyEventArgs e)
	{
		campoAbono.AlKeyDown(e);
	}

	private void txtPresupuesto_KeyPress(object sender, KeyPressEventArgs e)
	{
		campoPresupuesto.AlKeyPress(e);
	}

	private void txtAbono_KeyPress(object sender, KeyPressEventArgs e)
	{
		campoAbono.AlKeyPress(e);
	}

	private void txtPresupuesto_Validating(object sender, CancelEventArgs e)
	{
		if (!FormatoMoneda.TryParseMonetarioPositivo(txtPresupuesto.Text, out _))
		{
			Interaction.MsgBox("El presupuesto ingresado no es válido.", MsgBoxStyle.Exclamation, "Valor inválido");
			e.Cancel = true;
		}
	}

	private void txtAbono_Validating(object sender, CancelEventArgs e)
	{
		if (!FormatoMoneda.TryParseMonetarioPositivo(txtAbono.Text, out _))
		{
			Interaction.MsgBox("El abono ingresado no es válido.", MsgBoxStyle.Exclamation, "Valor inválido");
			e.Cancel = true;
		}
	}

	private void txtPresupuesto_Validated(object sender, EventArgs e)
	{
		if (FormatoMoneda.TryParseMonetarioPositivo(txtPresupuesto.Text, out decimal valor))
		{
			txtPresupuesto.Text = FormatoMoneda.Formatear(valor);
		}
	}

	private void txtAbono_Validated(object sender, EventArgs e)
	{
		if (FormatoMoneda.TryParseMonetarioPositivo(txtAbono.Text, out decimal valor))
		{
			txtAbono.Text = FormatoMoneda.Formatear(valor);
		}
	}

	private sealed class CampoMonetario
	{
		private readonly TextBox caja;

		private readonly MotorOrigenEdicion motor;

		private bool normalizando;

		private string ultimoValido = "";

		private int ultimoCaret;

		public CampoMonetario(TextBox caja)
		{
			this.caja = caja;
			motor = new MotorOrigenEdicion();
		}

		public void AlEntrar()
		{
			motor.Reiniciar();
			if (FormatoMoneda.TryParseMonetario(caja.Text, out decimal valor))
			{
				Asignar(FormatoMoneda.FormatearParaEdicion(valor));
			}
			ultimoValido = caja.Text;
			ultimoCaret = caja.SelectionStart;
			caja.BeginInvoke(new Action(() => caja.SelectAll()));
		}

		public void AlKeyDown(KeyEventArgs e)
		{
			bool mutacionConfirmada = false;

			if (e.Control && !e.Alt && e.KeyCode == Keys.X)
			{
				if (caja.SelectionLength > 0)
				{
					if (FormatoMoneda.EsEliminacionInseguraDeComa(caja.Text, caja.SelectionStart, caja.SelectionLength))
					{
						e.Handled = true;
					}
					else
					{
						mutacionConfirmada = true;
					}
				}
			}
			else if (!e.Control && !e.Alt && (e.KeyCode == Keys.Back || e.KeyCode == Keys.Delete))
			{
				int inicio;
				int longitud;
				if (caja.SelectionLength > 0)
				{
					inicio = caja.SelectionStart;
					longitud = caja.SelectionLength;
				}
				else if (e.KeyCode == Keys.Back)
				{
					inicio = caja.SelectionStart - 1;
					longitud = 1;
				}
				else
				{
					inicio = caja.SelectionStart;
					longitud = 1;
				}

				if (inicio >= 0 && longitud > 0 && inicio + longitud <= caja.Text.Length)
				{
					if (FormatoMoneda.EsEliminacionInseguraDeComa(caja.Text, inicio, longitud))
					{
						e.Handled = true;
					}
					else
					{
						mutacionConfirmada = true;
					}
				}
			}

			else if (!e.Control && !e.Alt && e.KeyCode == Keys.Decimal)
			{
				InsertarComaDecimal(e);
				return;
			}

			motor.ProcesarKeyDown(mutacionConfirmada);
		}

		private void InsertarComaDecimal(KeyEventArgs e)
		{
			if (!FormatoMoneda.EsInsercionValida(caja.Text, caja.SelectionStart, caja.SelectionLength, ','))
			{
				e.Handled = true;
				return;
			}

			int inicio = caja.SelectionStart;
			int longitud = caja.SelectionLength;
			string texto = caja.Text;

			string crudo = texto.Remove(inicio, longitud).Insert(inicio, ",");
			string agrupado = FormatoMoneda.AgruparEdicion(crudo);

			int caret = 0;
			int comaIdx = crudo.IndexOf(',');
			if (comaIdx >= 0 && inicio + 1 >= comaIdx)
			{
				int decimalesAntes = 0;
				for (int i = comaIdx + 1; i < inicio + 1 && i < crudo.Length; i++)
				{
					if (crudo[i] >= '0' && crudo[i] <= '9')
						decimalesAntes++;
				}

				int comaAgrupada = agrupado.IndexOf(',');
				if (comaAgrupada < 0)
					caret = agrupado.Length;
				else
				{
					caret = comaAgrupada + 1;
					int vistos = 0;
					while (caret < agrupado.Length && vistos < decimalesAntes)
					{
						caret++;
						vistos++;
					}
				}
			}
			else
			{
				int enterosAntes = 0;
				for (int i = 0; i < inicio; i++)
				{
					if (comaIdx >= 0 && i == comaIdx)
						break;
					if (texto[i] >= '0' && texto[i] <= '9')
						enterosAntes++;
				}

				int comaAgrupada = agrupado.IndexOf(',');
				int pos = 0;
				int vistos = 0;
				while (pos < agrupado.Length)
				{
					if (comaAgrupada >= 0 && pos == comaAgrupada)
						break;
					char c = agrupado[pos];
					if (c >= '0' && c <= '9')
					{
						if (vistos == enterosAntes)
							break;
						vistos++;
					}
					pos++;
				}
				caret = pos;
			}

			motor.ProcesarKeyDown(true);
			Asignar(agrupado);
			caja.SelectionStart = caret;
			caja.SelectionLength = 0;

			e.Handled = true;
			e.SuppressKeyPress = true;
		}

		public void AlKeyPress(KeyPressEventArgs e)
		{
			char c = e.KeyChar;
			if (c < ' ')
				return;

			bool aceptado = FormatoMoneda.EsInsercionValida(caja.Text, caja.SelectionStart, caja.SelectionLength, c);
			motor.ProcesarKeyPress(c, aceptado);
			if (!aceptado)
			{
				e.Handled = true;
				return;
			}

			if (c == '.')
				e.KeyChar = ',';
		}

		public void AlTextChanged()
		{
			if (normalizando || !caja.Focused)
				return;

			if (motor.ConsumirTextChanged() == OrigenEdicion.Teclado)
			{
				string crudo = caja.Text;
				string nuevo = FormatoMoneda.AgruparEdicion(crudo);
				if (nuevo != crudo)
				{
					int caret = CalcularCaret(crudo, caja.SelectionStart, nuevo);
					Asignar(nuevo);
					caja.SelectionStart = caret;
					caja.SelectionLength = 0;
				}
			}
			else if (FormatoMoneda.TryNormalizarEdicion(caja.Text, out string edicion))
			{
				Asignar(edicion);
				caja.SelectionStart = edicion.Length;
				caja.SelectionLength = 0;
			}
			else
			{
				Asignar(ultimoValido);
				caja.SelectionStart = Math.Min(ultimoCaret, ultimoValido.Length);
				caja.SelectionLength = 0;
			}

			ultimoValido = caja.Text;
			ultimoCaret = caja.SelectionStart;
		}

		private void Asignar(string texto)
		{
			normalizando = true;
			try
			{
				caja.Text = texto;
			}
			finally
			{
				normalizando = false;
			}
		}

		private static int CalcularCaret(string crudo, int caret, string nuevo)
		{
			if (caret < 0)
				caret = 0;
			if (caret > crudo.Length)
				caret = crudo.Length;

			int comaCrudo = crudo.IndexOf(',');

			if (comaCrudo >= 0 && caret > comaCrudo)
			{
				int decimalesAntes = 0;
				for (int i = comaCrudo + 1; i < caret && i < crudo.Length; i++)
				{
					if (crudo[i] >= '0' && crudo[i] <= '9')
						decimalesAntes++;
				}

				int comaNuevo = nuevo.IndexOf(',');
				if (comaNuevo < 0)
					return nuevo.Length;

				int posicion = comaNuevo + 1;
				int vistos = 0;
				while (posicion < nuevo.Length && vistos < decimalesAntes)
				{
					posicion++;
					vistos++;
				}
				return posicion;
			}

			int enterosAntes = 0;
			for (int i = 0; i < caret; i++)
			{
				if (comaCrudo >= 0 && i == comaCrudo)
					break;
				if (crudo[i] >= '0' && crudo[i] <= '9')
					enterosAntes++;
			}

			int comaNuevoEntero = nuevo.IndexOf(',');
			int posicionEntera = 0;
			int vistosEnteros = 0;
			while (posicionEntera < nuevo.Length)
			{
				if (comaNuevoEntero >= 0 && posicionEntera == comaNuevoEntero)
					break;
				char c = nuevo[posicionEntera];
				if (c >= '0' && c <= '9')
				{
					if (vistosEnteros == enterosAntes)
						break;
					vistosEnteros++;
				}
				posicionEntera++;
			}
			return posicionEntera;
		}
	}
}
