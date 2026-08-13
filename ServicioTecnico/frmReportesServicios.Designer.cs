using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace ServicioTecnico;

public partial class frmReportesServicios : Form
{
	private IContainer components;

	internal TextBox TextBox1;
	internal DataGridView dgvReportes;
	internal Chart chartReparacionesPorMes;
	internal Chart chartTiposReparacion;
	internal Chart chartEstadoReparaciones;
	internal Chart chartIngresosMensuales;
	internal ComboBox cmbFiltroFecha;
	internal ComboBox cmbTipoGrafico;
	internal DateTimePicker dtpDesde;
	internal DateTimePicker dtpHasta;
	internal Button btnFiltrar;
	internal Button btnActualizar;

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
		System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
		System.Windows.Forms.DataVisualization.Charting.Legend legend = new System.Windows.Forms.DataVisualization.Charting.Legend();
		System.Windows.Forms.DataVisualization.Charting.Series series = new System.Windows.Forms.DataVisualization.Charting.Series();
		System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea2 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
		System.Windows.Forms.DataVisualization.Charting.Legend legend2 = new System.Windows.Forms.DataVisualization.Charting.Legend();
		System.Windows.Forms.DataVisualization.Charting.Series series2 = new System.Windows.Forms.DataVisualization.Charting.Series();
		System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea3 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
		System.Windows.Forms.DataVisualization.Charting.Legend legend3 = new System.Windows.Forms.DataVisualization.Charting.Legend();
		System.Windows.Forms.DataVisualization.Charting.Series series3 = new System.Windows.Forms.DataVisualization.Charting.Series();
		System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea4 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
		System.Windows.Forms.DataVisualization.Charting.Legend legend4 = new System.Windows.Forms.DataVisualization.Charting.Legend();
		System.Windows.Forms.DataVisualization.Charting.Series series4 = new System.Windows.Forms.DataVisualization.Charting.Series();
		this.TextBox1 = new System.Windows.Forms.TextBox();
		this.dgvReportes = new System.Windows.Forms.DataGridView();
		this.chartReparacionesPorMes = new System.Windows.Forms.DataVisualization.Charting.Chart();
		this.chartTiposReparacion = new System.Windows.Forms.DataVisualization.Charting.Chart();
		this.chartEstadoReparaciones = new System.Windows.Forms.DataVisualization.Charting.Chart();
		this.chartIngresosMensuales = new System.Windows.Forms.DataVisualization.Charting.Chart();
		this.cmbFiltroFecha = new System.Windows.Forms.ComboBox();
		this.cmbTipoGrafico = new System.Windows.Forms.ComboBox();
		this.dtpDesde = new System.Windows.Forms.DateTimePicker();
		this.dtpHasta = new System.Windows.Forms.DateTimePicker();
		this.btnFiltrar = new System.Windows.Forms.Button();
		this.btnActualizar = new System.Windows.Forms.Button();
		((System.ComponentModel.ISupportInitialize)this.dgvReportes).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.chartReparacionesPorMes).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.chartTiposReparacion).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.chartEstadoReparaciones).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.chartIngresosMensuales).BeginInit();
		this.SuspendLayout();
		this.TextBox1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.TextBox1.BackColor = System.Drawing.Color.LightBlue;
		this.TextBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.TextBox1.Location = new System.Drawing.Point(0, -1);
		this.TextBox1.Name = "TextBox1";
		this.TextBox1.ReadOnly = true;
		this.TextBox1.Size = new System.Drawing.Size(1135, 29);
		this.TextBox1.TabIndex = 37;
		this.TextBox1.Text = "REPORTE DE SERVICIOS";
		this.TextBox1.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.dgvReportes.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.dgvReportes.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
		this.dgvReportes.Location = new System.Drawing.Point(12, 63);
		this.dgvReportes.Name = "dgvReportes";
		this.dgvReportes.Size = new System.Drawing.Size(1109, 297);
		this.dgvReportes.TabIndex = 38;
		chartArea.Name = "ChartArea1";
		this.chartReparacionesPorMes.ChartAreas.Add(chartArea);
		legend.Name = "Legend1";
		this.chartReparacionesPorMes.Legends.Add(legend);
		this.chartReparacionesPorMes.Location = new System.Drawing.Point(975, 75);
		this.chartReparacionesPorMes.Name = "chartReparacionesPorMes";
		series.ChartArea = "ChartArea1";
		series.Legend = "Legend1";
		series.Name = "Series1";
		this.chartReparacionesPorMes.Series.Add(series);
		this.chartReparacionesPorMes.Size = new System.Drawing.Size(133, 66);
		this.chartReparacionesPorMes.TabIndex = 39;
		this.chartReparacionesPorMes.Text = "Chart1";
		this.chartReparacionesPorMes.Visible = false;
		chartArea2.Name = "ChartArea1";
		this.chartTiposReparacion.ChartAreas.Add(chartArea2);
		legend2.Name = "Legend1";
		this.chartTiposReparacion.Legends.Add(legend2);
		this.chartTiposReparacion.Location = new System.Drawing.Point(12, 366);
		this.chartTiposReparacion.Name = "chartTiposReparacion";
		series2.ChartArea = "ChartArea1";
		series2.Legend = "Legend1";
		series2.Name = "Series1";
		this.chartTiposReparacion.Series.Add(series2);
		this.chartTiposReparacion.Size = new System.Drawing.Size(274, 248);
		this.chartTiposReparacion.TabIndex = 40;
		this.chartTiposReparacion.Text = "Chart1";
		chartArea3.Name = "ChartArea1";
		this.chartEstadoReparaciones.ChartAreas.Add(chartArea3);
		legend3.Name = "Legend1";
		this.chartEstadoReparaciones.Legends.Add(legend3);
		this.chartEstadoReparaciones.Location = new System.Drawing.Point(292, 366);
		this.chartEstadoReparaciones.Name = "chartEstadoReparaciones";
		series3.ChartArea = "ChartArea1";
		series3.Legend = "Legend1";
		series3.Name = "Series1";
		this.chartEstadoReparaciones.Series.Add(series3);
		this.chartEstadoReparaciones.Size = new System.Drawing.Size(274, 248);
		this.chartEstadoReparaciones.TabIndex = 41;
		this.chartEstadoReparaciones.Text = "Chart1";
		this.chartIngresosMensuales.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		chartArea4.Name = "ChartArea1";
		this.chartIngresosMensuales.ChartAreas.Add(chartArea4);
		legend4.Name = "Legend1";
		this.chartIngresosMensuales.Legends.Add(legend4);
		this.chartIngresosMensuales.Location = new System.Drawing.Point(572, 366);
		this.chartIngresosMensuales.Name = "chartIngresosMensuales";
		series4.ChartArea = "ChartArea1";
		series4.Legend = "Legend1";
		series4.Name = "Series1";
		this.chartIngresosMensuales.Series.Add(series4);
		this.chartIngresosMensuales.Size = new System.Drawing.Size(549, 248);
		this.chartIngresosMensuales.TabIndex = 42;
		this.chartIngresosMensuales.Text = "Chart1";
		this.cmbFiltroFecha.FormattingEnabled = true;
		this.cmbFiltroFecha.Location = new System.Drawing.Point(587, 34);
		this.cmbFiltroFecha.Name = "cmbFiltroFecha";
		this.cmbFiltroFecha.Size = new System.Drawing.Size(121, 21);
		this.cmbFiltroFecha.TabIndex = 43;
		this.cmbFiltroFecha.Visible = false;
		this.cmbTipoGrafico.FormattingEnabled = true;
		this.cmbTipoGrafico.Location = new System.Drawing.Point(779, 35);
		this.cmbTipoGrafico.Name = "cmbTipoGrafico";
		this.cmbTipoGrafico.Size = new System.Drawing.Size(121, 21);
		this.cmbTipoGrafico.TabIndex = 44;
		this.cmbTipoGrafico.Visible = false;
		this.dtpDesde.Format = System.Windows.Forms.DateTimePickerFormat.Short;
		this.dtpDesde.Location = new System.Drawing.Point(12, 34);
		this.dtpDesde.Name = "dtpDesde";
		this.dtpDesde.Size = new System.Drawing.Size(121, 20);
		this.dtpDesde.TabIndex = 45;
		this.dtpHasta.Format = System.Windows.Forms.DateTimePickerFormat.Short;
		this.dtpHasta.Location = new System.Drawing.Point(139, 34);
		this.dtpHasta.Name = "dtpHasta";
		this.dtpHasta.Size = new System.Drawing.Size(121, 20);
		this.dtpHasta.TabIndex = 46;
		this.btnFiltrar.Location = new System.Drawing.Point(266, 33);
		this.btnFiltrar.Name = "btnFiltrar";
		this.btnFiltrar.Size = new System.Drawing.Size(75, 23);
		this.btnFiltrar.TabIndex = 47;
		this.btnFiltrar.Text = "FILTRAR";
		this.btnFiltrar.UseVisualStyleBackColor = true;
		this.btnActualizar.Location = new System.Drawing.Point(714, 33);
		this.btnActualizar.Name = "btnActualizar";
		this.btnActualizar.Size = new System.Drawing.Size(75, 23);
		this.btnActualizar.TabIndex = 48;
		this.btnActualizar.Text = "Actualizar";
		this.btnActualizar.UseVisualStyleBackColor = true;
		this.btnActualizar.Visible = false;
		this.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		this.ClientSize = new System.Drawing.Size(1133, 626);
		this.Controls.Add(this.btnActualizar);
		this.Controls.Add(this.btnFiltrar);
		this.Controls.Add(this.dtpHasta);
		this.Controls.Add(this.dtpDesde);
		this.Controls.Add(this.cmbTipoGrafico);
		this.Controls.Add(this.cmbFiltroFecha);
		this.Controls.Add(this.chartIngresosMensuales);
		this.Controls.Add(this.chartEstadoReparaciones);
		this.Controls.Add(this.chartTiposReparacion);
		this.Controls.Add(this.chartReparacionesPorMes);
		this.Controls.Add(this.dgvReportes);
		this.Controls.Add(this.TextBox1);
		this.Name = "frmReportesServicios";
		this.Text = "Reportes";
		((System.ComponentModel.ISupportInitialize)this.dgvReportes).EndInit();
		((System.ComponentModel.ISupportInitialize)this.chartReparacionesPorMes).EndInit();
		((System.ComponentModel.ISupportInitialize)this.chartTiposReparacion).EndInit();
		((System.ComponentModel.ISupportInitialize)this.chartEstadoReparaciones).EndInit();
		((System.ComponentModel.ISupportInitialize)this.chartIngresosMensuales).EndInit();
		this.cmbFiltroFecha.SelectedIndexChanged += new System.EventHandler(this.cmbFiltroFecha_SelectedIndexChanged);
		this.cmbTipoGrafico.SelectedIndexChanged += new System.EventHandler(this.cmbTipoGrafico_SelectedIndexChanged);
		this.btnFiltrar.Click += new System.EventHandler(this.btnFiltrar_Click);
		this.btnActualizar.Click += new System.EventHandler(this.btnActualizar_Click);
		this.ResumeLayout(false);
		this.PerformLayout();
	}
}
