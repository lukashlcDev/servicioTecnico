using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace ServicioTecnico;

public partial class frmFoto : Form
{
	private IContainer components;

	internal PictureBox picFoto;

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
		this.picFoto = new System.Windows.Forms.PictureBox();
		((System.ComponentModel.ISupportInitialize)this.picFoto).BeginInit();
		this.SuspendLayout();
		this.picFoto.Location = new System.Drawing.Point(-1, 1);
		this.picFoto.Name = "picFoto";
		this.picFoto.Size = new System.Drawing.Size(559, 398);
		this.picFoto.TabIndex = 8002;
		this.picFoto.TabStop = false;
		this.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		this.ClientSize = new System.Drawing.Size(555, 397);
		this.Controls.Add(this.picFoto);
		this.Name = "frmFoto";
		this.Text = "Foto";
		this.picFoto.Click += new System.EventHandler(this.picFoto_Click);
		((System.ComponentModel.ISupportInitialize)this.picFoto).EndInit();
		this.ResumeLayout(false);
	}
}
