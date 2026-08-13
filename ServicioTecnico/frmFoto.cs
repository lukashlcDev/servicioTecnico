using System;
using System.Drawing;
using System.Windows.Forms;
using Microsoft.VisualBasic.CompilerServices;

namespace ServicioTecnico;

public partial class frmFoto : Form
{
	public frmFoto()
		: this(null, string.Empty)
	{
	}

	public frmFoto(Image imagen, string titulo)
	{
		base.Load += frmFoto_Load;
		InitializeComponent();
		Text = titulo;
		picFoto.Image = imagen;
		picFoto.SizeMode = PictureBoxSizeMode.Zoom;
	}

	private void frmFoto_Load(object sender, EventArgs e)
	{
		WindowState = FormWindowState.Maximized;
		picFoto.Height = Height;
		picFoto.Width = Width;
	}

	private void picFoto_Click(object sender, EventArgs e)
	{
	}
}
