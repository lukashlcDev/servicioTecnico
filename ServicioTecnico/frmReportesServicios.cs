using System;
using System.Data;
using System.Data.SQLite;
using System.Runtime.CompilerServices;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using Microsoft.VisualBasic.CompilerServices;

namespace ServicioTecnico;

public partial class frmReportesServicios : Form
{
	public frmReportesServicios()
	{
		base.Load += frmReportesServicios_Load;
		InitializeComponent();
	}

	private void frmReportesServicios_Load(object sender, EventArgs e)
	{
		CargarDatosReportes();
		GenerarGraficas();
		ConfigurarFiltros();
	}

	private void ConfigurarFiltros()
	{
		cmbFiltroFecha.Items.AddRange(new object[5] { "Últimos 7 días", "Este mes", "Últimos 30 días", "Este año", "Personalizado" });
		cmbFiltroFecha.SelectedIndex = 1;
		cmbTipoGrafico.Items.AddRange(new object[4] { "Barras", "Líneas", "Torta", "Área" });
		cmbTipoGrafico.SelectedIndex = 0;
	}

	private void AplicarFormatoMonetarioGrid()
	{
		if (dgvReportes.Columns.Contains("Presupuesto"))
		{
			DataGridViewColumn dataGridViewColumn = dgvReportes.Columns["Presupuesto"];
			dataGridViewColumn.DefaultCellStyle.Format = "C2";
			dataGridViewColumn.DefaultCellStyle.FormatProvider = FormatoMoneda.Cultura;
		}
		if (dgvReportes.Columns.Contains("Abono"))
		{
			DataGridViewColumn dataGridViewColumn2 = dgvReportes.Columns["Abono"];
			dataGridViewColumn2.DefaultCellStyle.Format = "C2";
			dataGridViewColumn2.DefaultCellStyle.FormatProvider = FormatoMoneda.Cultura;
		}
		if (dgvReportes.Columns.Contains("Total"))
		{
			DataGridViewColumn dataGridViewColumn3 = dgvReportes.Columns["Total"];
			dataGridViewColumn3.DefaultCellStyle.Format = "C2";
			dataGridViewColumn3.DefaultCellStyle.FormatProvider = FormatoMoneda.Cultura;
		}
	}

	private void CargarDatosReportes()
	{
		try
		{
			DataTable dataSource = ObtenerDatosReportes();
			dgvReportes.DataSource = dataSource;
			dgvReportes.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
			dgvReportes.ReadOnly = true;
			AplicarFormatoMonetarioGrid();
		}
		catch (Exception ex)
		{
			ProjectData.SetProjectError(ex);
			Exception ex2 = ex;
			MessageBox.Show("Error al cargar reportes: " + ex2.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
			ProjectData.ClearProjectError();
		}
	}

	private DataTable ObtenerDatosReportes()
	{
		DataTable dataTable = new DataTable();
		string value = ObtenerFechaDesde(cmbFiltroFecha.SelectedIndex);
		string commandText = "SELECT o.id_orden as [N° Orden], o.fecha as Fecha, c.nombre as Cliente, o.tipo_equipo as [Tipo Equipo], o.marca as Marca, o.modelo as Modelo, o.falla as Falla, o.estado_entrega as Estado, o.presupuesto as Presupuesto, o.abono as Abono, o.total as Total, o.reparado as [Fecha Reparación], o.entregado as [Fecha Entrega] FROM ordenes o LEFT JOIN clientes c ON o.id_cliente = c.id_cliente WHERE o.fecha >= @fechaDesde ORDER BY o.fecha DESC, o.id_orden DESC LIMIT 100";
		using SQLiteConnection sQLiteConnection = new SQLiteConnection("Data Source=" + modConexion.rutaDB);
		sQLiteConnection.Open();
		using SQLiteCommand sQLiteCommand = new SQLiteCommand(commandText, sQLiteConnection);
		sQLiteCommand.Parameters.AddWithValue("@fechaDesde", value);
		using SQLiteDataAdapter sQLiteDataAdapter = new SQLiteDataAdapter(sQLiteCommand);
		sQLiteDataAdapter.Fill(dataTable);
		return dataTable;
	}

	private string ObtenerFechaDesde(int indiceFiltro)
	{
		DateTime dateTime;
		switch (indiceFiltro)
		{
		case 0:
			return DateTime.Now.AddDays(-7.0).ToString("dd/MM/yyyy");
		case 1:
		{
			dateTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
			DateTime dateTime3 = dateTime;
			return dateTime3.ToString("dd/MM/yyyy");
		}
		case 2:
			return DateTime.Now.AddDays(-30.0).ToString("dd/MM/yyyy");
		case 3:
		{
			dateTime = new DateTime(DateTime.Now.Year, 1, 1);
			DateTime dateTime2 = dateTime;
			return dateTime2.ToString("dd/MM/yyyy");
		}
		default:
		{
			dateTime = new DateTime(DateTime.Now.Year, 1, 1);
			DateTime dateTime2 = dateTime;
			return dateTime2.ToString("dd/MM/yyyy");
		}
		}
	}

	private void GenerarGraficas()
	{
		GenerarGraficaEstadosReparacion();
		GenerarGraficaTiposEquipo();
		GenerarGraficaIngresosMensuales();
	}

	private void GenerarGraficaEstadosReparacion()
	{
		chartEstadoReparaciones.Series.Clear();
		chartEstadoReparaciones.Titles.Clear();
		chartEstadoReparaciones.Titles.Add("Estado de Reparaciones");
		Series series = new Series("Estados");
		series.ChartType = SeriesChartType.Pie;
		series.IsValueShownAsLabel = true;
		series.Label = "#PERCENT{P0}";
		series.LegendText = "#VALX (#VALY)";
		Series series2 = series;
		string commandText = "SELECT estado_entrega, COUNT(*) as Total FROM ordenes WHERE estado_entrega IS NOT NULL AND estado_entrega <> '' GROUP BY estado_entrega ORDER BY Total DESC";
		using (SQLiteConnection sQLiteConnection = new SQLiteConnection("Data Source=" + modConexion.rutaDB))
		{
			sQLiteConnection.Open();
			using SQLiteCommand sQLiteCommand = new SQLiteCommand(commandText, sQLiteConnection);
			using SQLiteDataReader sQLiteDataReader = sQLiteCommand.ExecuteReader();
			while (sQLiteDataReader.Read())
			{
				string xValue = ((Operators.CompareString(sQLiteDataReader["estado_entrega"].ToString(), "", TextCompare: false) == 0) ? "SIN ESTADO" : sQLiteDataReader["estado_entrega"].ToString());
				series2.Points.AddXY(xValue, RuntimeHelpers.GetObjectValue(sQLiteDataReader["Total"]));
			}
		}
		chartEstadoReparaciones.Series.Add(series2);
	}

	private void GenerarGraficaTiposEquipo()
	{
		chartTiposReparacion.Series.Clear();
		chartTiposReparacion.Titles.Clear();
		chartTiposReparacion.Titles.Add("Tipos de Equipo");
		Series series = new Series("Tipos");
		series.ChartType = SeriesChartType.Doughnut;
		series.IsValueShownAsLabel = true;
		series.Label = "#PERCENT{P0}";
		series.LegendText = "#VALX (#VALY)";
		Series series2 = series;
		string commandText = "SELECT CASE WHEN tipo_equipo LIKE 'Otros:%' THEN 'Otros' ELSE tipo_equipo END as Tipo, COUNT(*) as Total FROM ordenes GROUP BY CASE WHEN tipo_equipo LIKE 'Otros:%' THEN 'Otros' ELSE tipo_equipo END ORDER BY Total DESC";
		using (SQLiteConnection sQLiteConnection = new SQLiteConnection("Data Source=" + modConexion.rutaDB))
		{
			sQLiteConnection.Open();
			using SQLiteCommand sQLiteCommand = new SQLiteCommand(commandText, sQLiteConnection);
			using SQLiteDataReader sQLiteDataReader = sQLiteCommand.ExecuteReader();
			while (sQLiteDataReader.Read())
			{
				series2.Points.AddXY(RuntimeHelpers.GetObjectValue(sQLiteDataReader["Tipo"]), RuntimeHelpers.GetObjectValue(sQLiteDataReader["Total"]));
			}
		}
		chartTiposReparacion.Series.Add(series2);
	}

	private void GenerarGraficaIngresosMensuales()
	{
		chartIngresosMensuales.Series.Clear();
		chartIngresosMensuales.Titles.Clear();
		chartIngresosMensuales.Titles.Add("Ingresos Mensuales");
		Series series = new Series("Ingresos");
		series.ChartType = SeriesChartType.Column;
		series.IsValueShownAsLabel = true;
		series.LabelFormat = "";
		Series series2 = series;
		string commandText = "SELECT substr(fecha, 4, 2) || '/' || substr(fecha, 7, 4) as Mes, SUM(presupuesto) as Total FROM ordenes WHERE presupuesto > 0 GROUP BY substr(fecha, 4, 2) || '/' || substr(fecha, 7, 4) ORDER BY substr(fecha, 7, 4), substr(fecha, 4, 2)";
		using (SQLiteConnection sQLiteConnection = new SQLiteConnection("Data Source=" + modConexion.rutaDB))
		{
			sQLiteConnection.Open();
			using SQLiteCommand sQLiteCommand = new SQLiteCommand(commandText, sQLiteConnection);
			using SQLiteDataReader sQLiteDataReader = sQLiteCommand.ExecuteReader();
			while (sQLiteDataReader.Read())
			{
				double yValue = Convert.ToDouble(RuntimeHelpers.GetObjectValue(sQLiteDataReader["Total"]));
				int index = series2.Points.AddXY(RuntimeHelpers.GetObjectValue(sQLiteDataReader["Mes"]), yValue);
				series2.Points[index].Label = FormatoMoneda.Formatear(Convert.ToDecimal(yValue));
			}
		}
		chartIngresosMensuales.Series.Add(series2);
		AplicarEtiquetasEjeYMonetario();
	}

	private void btnActualizar_Click(object sender, EventArgs e)
	{
		CargarDatosReportes();
		GenerarGraficas();
	}

	private void cmbTipoGrafico_SelectedIndexChanged(object sender, EventArgs e)
	{
		GenerarGraficas();
	}

	private void btnFiltrar_Click(object sender, EventArgs e)
	{
		if (DateTime.Compare(dtpDesde.Value, dtpHasta.Value) > 0)
		{
			MessageBox.Show("La fecha 'Desde' no puede ser mayor que la fecha 'Hasta'", "Error en fechas", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			return;
		}
		string fechaDesde = dtpDesde.Value.ToString("dd/MM/yyyy");
		string fechaHasta = dtpHasta.Value.ToString("dd/MM/yyyy");
		try
		{
			CargarDatosConFiltro(fechaDesde, fechaHasta);
			GenerarGraficasConFiltro(fechaDesde, fechaHasta);
		}
		catch (Exception ex)
		{
			ProjectData.SetProjectError(ex);
			Exception ex2 = ex;
			MessageBox.Show("Error al filtrar datos: " + ex2.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
			ProjectData.ClearProjectError();
		}
	}

	private void CargarDatosConFiltro(string fechaDesde, string fechaHasta)
	{
		DataTable dataTable = new DataTable();
		string commandText = "SELECT o.id_orden as [N° Orden], o.fecha as Fecha, c.nombre as Cliente, o.tipo_equipo as [Tipo Equipo], o.marca as Marca, o.modelo as Modelo, o.falla as Falla, o.estado_entrega as Estado, o.presupuesto as Presupuesto, o.abono as Abono, o.total as Total, o.reparado as [Fecha Reparación], o.entregado as [Fecha Entrega] FROM ordenes o LEFT JOIN clientes c ON o.id_cliente = c.id_cliente WHERE substr(fecha, 7, 4) || substr(fecha, 4, 2) || substr(fecha, 1, 2) BETWEEN @fechaDesdeNum AND @fechaHastaNum ORDER BY substr(fecha, 7, 4) || substr(fecha, 4, 2) || substr(fecha, 1, 2) DESC LIMIT 100";
		using (SQLiteConnection sQLiteConnection = new SQLiteConnection("Data Source=" + modConexion.rutaDB))
		{
			sQLiteConnection.Open();
			using SQLiteCommand sQLiteCommand = new SQLiteCommand(commandText, sQLiteConnection);
			sQLiteCommand.Parameters.AddWithValue("@fechaDesdeNum", ConvertirFechaANumero(fechaDesde));
			sQLiteCommand.Parameters.AddWithValue("@fechaHastaNum", ConvertirFechaANumero(fechaHasta));
			using SQLiteDataAdapter sQLiteDataAdapter = new SQLiteDataAdapter(sQLiteCommand);
			sQLiteDataAdapter.Fill(dataTable);
		}
		dgvReportes.DataSource = dataTable;
		AplicarFormatoMonetarioGrid();
	}

	private void GenerarGraficasConFiltro(string fechaDesde, string fechaHasta)
	{
		GenerarGraficaEstadosConFiltro(fechaDesde, fechaHasta);
		GenerarGraficaTiposEquipoConFiltro(fechaDesde, fechaHasta);
		GenerarGraficaIngresosConFiltro(fechaDesde, fechaHasta);
	}

	private void GenerarGraficaEstadosConFiltro(string fechaDesde, string fechaHasta)
	{
		chartEstadoReparaciones.Series.Clear();
		chartEstadoReparaciones.Titles.Clear();
		chartEstadoReparaciones.Titles.Add("Estados de Reparación (" + fechaDesde + " a " + fechaHasta + ")");
		Series series = new Series("Estados");
		series.ChartType = SeriesChartType.Pie;
		series.IsValueShownAsLabel = true;
		series.Label = "#PERCENT{P0}";
		series.LegendText = "#VALX (#VALY)";
		Series series2 = series;
		string commandText = "SELECT estado_entrega, COUNT(*) as Total FROM ordenes WHERE substr(fecha, 7, 4) || substr(fecha, 4, 2) || substr(fecha, 1, 2) BETWEEN @fechaDesdeNum AND @fechaHastaNum AND estado_entrega IS NOT NULL AND estado_entrega <> '' GROUP BY estado_entrega ORDER BY Total DESC";
		using (SQLiteConnection sQLiteConnection = new SQLiteConnection("Data Source=" + modConexion.rutaDB))
		{
			sQLiteConnection.Open();
			using SQLiteCommand sQLiteCommand = new SQLiteCommand(commandText, sQLiteConnection);
			sQLiteCommand.Parameters.AddWithValue("@fechaDesdeNum", ConvertirFechaANumero(fechaDesde));
			sQLiteCommand.Parameters.AddWithValue("@fechaHastaNum", ConvertirFechaANumero(fechaHasta));
			using SQLiteDataReader sQLiteDataReader = sQLiteCommand.ExecuteReader();
			while (sQLiteDataReader.Read())
			{
				string xValue = (string.IsNullOrEmpty(sQLiteDataReader["estado_entrega"].ToString()) ? "SIN ESTADO" : sQLiteDataReader["estado_entrega"].ToString());
				series2.Points.AddXY(xValue, RuntimeHelpers.GetObjectValue(sQLiteDataReader["Total"]));
			}
		}
		chartEstadoReparaciones.Series.Add(series2);
	}

	private void cmbFiltroFecha_SelectedIndexChanged(object sender, EventArgs e)
	{
		if (cmbFiltroFecha.SelectedIndex == 4)
		{
			dtpDesde.Visible = true;
			dtpHasta.Visible = true;
			btnFiltrar.Visible = true;
			dtpDesde.Value = DateTime.Now.AddDays(-7.0);
			dtpHasta.Value = DateTime.Now;
		}
		else
		{
			string fechaDesde = ObtenerFechaDesde(cmbFiltroFecha.SelectedIndex);
			string fechaHasta = DateTime.Now.ToString("dd/MM/yyyy");
			CargarDatosConFiltro(fechaDesde, fechaHasta);
			GenerarGraficasConFiltro(fechaDesde, fechaHasta);
		}
	}

	private void GenerarGraficaTiposEquipoConFiltro(string fechaDesde, string fechaHasta)
	{
		chartTiposReparacion.Series.Clear();
		chartTiposReparacion.Titles.Clear();
		chartTiposReparacion.Titles.Add("Tipos de Equipo (" + fechaDesde + " a " + fechaHasta + ")");
		Series series = new Series("Tipos");
		series.ChartType = SeriesChartType.Doughnut;
		series.IsValueShownAsLabel = true;
		series.Label = "#PERCENT{P0}";
		series.LegendText = "#VALX (#VALY)";
		Series series2 = series;
		string commandText = "SELECT CASE WHEN tipo_equipo LIKE 'Otros:%' THEN 'Otros' ELSE tipo_equipo END as Tipo, COUNT(*) as Total FROM ordenes WHERE substr(fecha, 7, 4) || substr(fecha, 4, 2) || substr(fecha, 1, 2) BETWEEN @fechaDesdeNum AND @fechaHastaNum GROUP BY CASE WHEN tipo_equipo LIKE 'Otros:%' THEN 'Otros' ELSE tipo_equipo END ORDER BY Total DESC";
		using (SQLiteConnection sQLiteConnection = new SQLiteConnection("Data Source=" + modConexion.rutaDB))
		{
			sQLiteConnection.Open();
			using SQLiteCommand sQLiteCommand = new SQLiteCommand(commandText, sQLiteConnection);
			sQLiteCommand.Parameters.AddWithValue("@fechaDesdeNum", ConvertirFechaANumero(fechaDesde));
			sQLiteCommand.Parameters.AddWithValue("@fechaHastaNum", ConvertirFechaANumero(fechaHasta));
			using SQLiteDataReader sQLiteDataReader = sQLiteCommand.ExecuteReader();
			while (sQLiteDataReader.Read())
			{
				series2.Points.AddXY(RuntimeHelpers.GetObjectValue(sQLiteDataReader["Tipo"]), RuntimeHelpers.GetObjectValue(sQLiteDataReader["Total"]));
			}
		}
		chartTiposReparacion.Series.Add(series2);
	}

	private void GenerarGraficaIngresosConFiltro(string fechaDesde, string fechaHasta)
	{
		chartIngresosMensuales.Series.Clear();
		chartIngresosMensuales.Titles.Clear();
		chartIngresosMensuales.Titles.Add("Ingresos Mensuales (" + fechaDesde + " a " + fechaHasta + ")");
		Series series = new Series("Ingresos");
		series.ChartType = SeriesChartType.Column;
		series.IsValueShownAsLabel = true;
		series.LabelFormat = "";
		Series series2 = series;
		string commandText = "SELECT substr(fecha, 4, 2) || '/' || substr(fecha, 7, 4) as Mes, SUM(presupuesto) as Total FROM ordenes WHERE presupuesto > 0 AND substr(fecha, 7, 4) || substr(fecha, 4, 2) || substr(fecha, 1, 2) BETWEEN @fechaDesdeNum AND @fechaHastaNum GROUP BY substr(fecha, 4, 2) || '/' || substr(fecha, 7, 4) ORDER BY substr(fecha, 7, 4), substr(fecha, 4, 2)";
		using (SQLiteConnection sQLiteConnection = new SQLiteConnection("Data Source=" + modConexion.rutaDB))
		{
			sQLiteConnection.Open();
			using SQLiteCommand sQLiteCommand = new SQLiteCommand(commandText, sQLiteConnection);
			sQLiteCommand.Parameters.AddWithValue("@fechaDesdeNum", ConvertirFechaANumero(fechaDesde));
			sQLiteCommand.Parameters.AddWithValue("@fechaHastaNum", ConvertirFechaANumero(fechaHasta));
			using SQLiteDataReader sQLiteDataReader = sQLiteCommand.ExecuteReader();
			while (sQLiteDataReader.Read())
			{
				double yValue = Convert.ToDouble(RuntimeHelpers.GetObjectValue(sQLiteDataReader["Total"]));
				int index = series2.Points.AddXY(RuntimeHelpers.GetObjectValue(sQLiteDataReader["Mes"]), yValue);
				series2.Points[index].Label = FormatoMoneda.Formatear(Convert.ToDecimal(yValue));
			}
		}
		chartIngresosMensuales.Series.Add(series2);
		AplicarEtiquetasEjeYMonetario();
	}

	private string ConvertirFechaANumero(string fechaTexto)
	{
		string[] array = fechaTexto.Split('/');
		return array[2] + array[1].PadLeft(2, '0') + array[0].PadLeft(2, '0');
	}

	private void CargarDatosReportesPersonalizado()
	{
		try
		{
			DataTable dataSource = ObtenerDatosReportesPersonalizado();
			dgvReportes.DataSource = dataSource;
			AplicarFormatoMonetarioGrid();
		}
		catch (Exception ex)
		{
			ProjectData.SetProjectError(ex);
			Exception ex2 = ex;
			MessageBox.Show("Error al cargar reportes: " + ex2.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
			ProjectData.ClearProjectError();
		}
	}

	private DataTable ObtenerDatosReportesPersonalizado()
	{
		DataTable dataTable = new DataTable();
		string commandText = "SELECT o.id_orden as [N° Orden], o.fecha as Fecha, c.nombre as Cliente, o.tipo_equipo as [Tipo Equipo], o.marca as Marca, o.modelo as Modelo, o.falla as Falla, o.estado_entrega as Estado, o.presupuesto as Presupuesto, o.abono as Abono, o.total as Total, o.reparado as [Fecha Reparación], o.entregado as [Fecha Entrega] FROM ordenes o LEFT JOIN clientes c ON o.id_cliente = c.id_cliente WHERE o.fecha BETWEEN @fechaDesde AND @fechaHasta ORDER BY o.fecha DESC, o.id_orden DESC LIMIT 100";
		using SQLiteConnection sQLiteConnection = new SQLiteConnection("Data Source=" + modConexion.rutaDB);
		sQLiteConnection.Open();
		using SQLiteCommand sQLiteCommand = new SQLiteCommand(commandText, sQLiteConnection);
		sQLiteCommand.Parameters.AddWithValue("@fechaDesde", dtpDesde.Value.ToString("dd/MM/yyyy"));
		sQLiteCommand.Parameters.AddWithValue("@fechaHasta", dtpHasta.Value.ToString("dd/MM/yyyy"));
		using SQLiteDataAdapter sQLiteDataAdapter = new SQLiteDataAdapter(sQLiteCommand);
		sQLiteDataAdapter.Fill(dataTable);
		return dataTable;
	}

	private void GenerarGraficasPersonalizado()
	{
		GenerarGraficaEstadosReparacionPersonalizado();
		GenerarGraficaTiposEquipoPersonalizado();
		GenerarGraficaIngresosMensualesPersonalizado();
	}

	private void GenerarGraficaEstadosReparacionPersonalizado()
	{
		chartEstadoReparaciones.Series.Clear();
		chartEstadoReparaciones.Titles.Clear();
		chartEstadoReparaciones.Titles.Add("Estado de Reparaciones (" + dtpDesde.Value.ToString("dd/MM/yyyy") + " - " + dtpHasta.Value.ToString("dd/MM/yyyy") + ")");
		Series series = new Series("Estados");
		series.ChartType = SeriesChartType.Pie;
		series.IsValueShownAsLabel = true;
		series.Label = "#PERCENT{P0}";
		series.LegendText = "#VALX (#VALY)";
		Series series2 = series;
		string commandText = "SELECT estado_entrega, COUNT(*) as Total FROM ordenes WHERE estado_entrega IS NOT NULL AND estado_entrega <> '' AND fecha BETWEEN @fechaDesde AND @fechaHasta GROUP BY estado_entrega ORDER BY Total DESC";
		using (SQLiteConnection sQLiteConnection = new SQLiteConnection("Data Source=" + modConexion.rutaDB))
		{
			sQLiteConnection.Open();
			using SQLiteCommand sQLiteCommand = new SQLiteCommand(commandText, sQLiteConnection);
			sQLiteCommand.Parameters.AddWithValue("@fechaDesde", dtpDesde.Value.ToString("dd/MM/yyyy"));
			sQLiteCommand.Parameters.AddWithValue("@fechaHasta", dtpHasta.Value.ToString("dd/MM/yyyy"));
			using SQLiteDataReader sQLiteDataReader = sQLiteCommand.ExecuteReader();
			while (sQLiteDataReader.Read())
			{
				string xValue = ((Operators.CompareString(sQLiteDataReader["estado_entrega"].ToString(), "", TextCompare: false) == 0) ? "SIN ESTADO" : sQLiteDataReader["estado_entrega"].ToString());
				series2.Points.AddXY(xValue, RuntimeHelpers.GetObjectValue(sQLiteDataReader["Total"]));
			}
		}
		chartEstadoReparaciones.Series.Add(series2);
	}

	private void GenerarGraficaTiposEquipoPersonalizado()
	{
		chartTiposReparacion.Series.Clear();
		chartTiposReparacion.Titles.Clear();
		chartTiposReparacion.Titles.Add("Tipos de Equipo (" + dtpDesde.Value.ToString("dd/MM/yyyy") + " - " + dtpHasta.Value.ToString("dd/MM/yyyy") + ")");
		Series series = new Series("Tipos");
		series.ChartType = SeriesChartType.Doughnut;
		series.IsValueShownAsLabel = true;
		series.Label = "#PERCENT{P0}";
		series.LegendText = "#VALX (#VALY)";
		Series series2 = series;
		string commandText = "SELECT CASE WHEN tipo_equipo LIKE 'Otros:%' THEN 'Otros' ELSE tipo_equipo END as Tipo, COUNT(*) as Total FROM ordenes WHERE fecha BETWEEN @fechaDesde AND @fechaHasta GROUP BY CASE WHEN tipo_equipo LIKE 'Otros:%' THEN 'Otros' ELSE tipo_equipo END ORDER BY Total DESC";
		using (SQLiteConnection sQLiteConnection = new SQLiteConnection("Data Source=" + modConexion.rutaDB))
		{
			sQLiteConnection.Open();
			using SQLiteCommand sQLiteCommand = new SQLiteCommand(commandText, sQLiteConnection);
			sQLiteCommand.Parameters.AddWithValue("@fechaDesde", dtpDesde.Value.ToString("dd/MM/yyyy"));
			sQLiteCommand.Parameters.AddWithValue("@fechaHasta", dtpHasta.Value.ToString("dd/MM/yyyy"));
			using SQLiteDataReader sQLiteDataReader = sQLiteCommand.ExecuteReader();
			while (sQLiteDataReader.Read())
			{
				series2.Points.AddXY(RuntimeHelpers.GetObjectValue(sQLiteDataReader["Tipo"]), RuntimeHelpers.GetObjectValue(sQLiteDataReader["Total"]));
			}
		}
		chartTiposReparacion.Series.Add(series2);
	}

	private void GenerarGraficaIngresosMensualesPersonalizado()
	{
		chartIngresosMensuales.Series.Clear();
		chartIngresosMensuales.Titles.Clear();
		chartIngresosMensuales.Titles.Add("Ingresos Mensuales (" + dtpDesde.Value.ToString("dd/MM/yyyy") + " - " + dtpHasta.Value.ToString("dd/MM/yyyy") + ")");
		Series series = new Series("Ingresos");
		series.ChartType = SeriesChartType.Column;
		series.IsValueShownAsLabel = true;
		series.LabelFormat = "";
		Series series2 = series;
		string commandText = "SELECT substr(fecha, 4, 2) || '/' || substr(fecha, 7, 4) as Mes, SUM(presupuesto) as Total FROM ordenes WHERE presupuesto > 0 AND fecha BETWEEN @fechaDesde AND @fechaHasta GROUP BY substr(fecha, 4, 2) || '/' || substr(fecha, 7, 4) ORDER BY substr(fecha, 7, 4), substr(fecha, 4, 2)";
		using (SQLiteConnection sQLiteConnection = new SQLiteConnection("Data Source=" + modConexion.rutaDB))
		{
			sQLiteConnection.Open();
			using SQLiteCommand sQLiteCommand = new SQLiteCommand(commandText, sQLiteConnection);
			sQLiteCommand.Parameters.AddWithValue("@fechaDesde", dtpDesde.Value.ToString("dd/MM/yyyy"));
			sQLiteCommand.Parameters.AddWithValue("@fechaHasta", dtpHasta.Value.ToString("dd/MM/yyyy"));
			using SQLiteDataReader sQLiteDataReader = sQLiteCommand.ExecuteReader();
			while (sQLiteDataReader.Read())
			{
				double yValue = Convert.ToDouble(RuntimeHelpers.GetObjectValue(sQLiteDataReader["Total"]));
				int index = series2.Points.AddXY(RuntimeHelpers.GetObjectValue(sQLiteDataReader["Mes"]), yValue);
				series2.Points[index].Label = FormatoMoneda.Formatear(Convert.ToDecimal(yValue));
			}
		}
		chartIngresosMensuales.Series.Add(series2);
		AplicarEtiquetasEjeYMonetario();
	}

	private double CalcularIntervaloNice(double rango)
	{
		if (rango <= 0.0)
		{
			return 1.0;
		}

		double division = rango / 8.0;
		double magnitud = Math.Pow(10.0, Math.Floor(Math.Log10(division)));
		double residual = division / magnitud;

		if (residual <= 1.0)
		{
			return magnitud;
		}

		if (residual <= 2.0)
		{
			return magnitud * 2.0;
		}

		if (residual <= 5.0)
		{
			return magnitud * 5.0;
		}

		return magnitud * 10.0;
	}

	private static void RestaurarEscalaAutomatica(Axis axisY)
	{
		axisY.Minimum = double.NaN;
		axisY.Maximum = double.NaN;
		axisY.Interval = 0.0;
		axisY.LabelStyle.Enabled = true;
	}

	private void AplicarEtiquetasEjeYMonetario()
	{
		Axis axisY = chartIngresosMensuales.ChartAreas[0].AxisY;
		axisY.CustomLabels.Clear();
		RestaurarEscalaAutomatica(axisY);

		Series series = chartIngresosMensuales.Series["Ingresos"];
		if (series == null || series.Points.Count == 0)
		{
			return;
		}

		double min = double.MaxValue;
		double max = double.MinValue;
		bool hayValores = false;

		foreach (DataPoint point in series.Points)
		{
			if (point.YValues.Length == 0)
			{
				continue;
			}

			double y = point.YValues[0];
			if (double.IsNaN(y))
			{
				continue;
			}

			if (!hayValores || y < min)
			{
				min = y;
			}

			if (!hayValores || y > max)
			{
				max = y;
			}

			hayValores = true;
		}

		if (!hayValores)
		{
			return;
		}

		double rango = max - min;
		double intervalo;
		double minEtiqueta;
		double maxEtiqueta;

		if (rango == 0.0)
		{
			double baseMagnitud = (max != 0.0) ? Math.Abs(max) : 1.0;
			intervalo = CalcularIntervaloNice(baseMagnitud);
			if (intervalo < 1.0)
			{
				intervalo = 1.0;
			}

			minEtiqueta = Math.Min(0.0, Math.Floor(max / intervalo) * intervalo);
			maxEtiqueta = Math.Ceiling(max / intervalo) * intervalo;
		}
		else
		{
			minEtiqueta = Math.Min(0.0, min);
			intervalo = CalcularIntervaloNice(max - minEtiqueta);
			if (intervalo < 1.0)
			{
				intervalo = 1.0;
			}

			minEtiqueta = Math.Floor(minEtiqueta / intervalo) * intervalo;
			maxEtiqueta = Math.Ceiling(max / intervalo) * intervalo;
		}

		if (intervalo <= 0.0)
		{
			return;
		}

		if (maxEtiqueta - minEtiqueta < intervalo)
		{
			maxEtiqueta = minEtiqueta + intervalo;
		}

		axisY.Minimum = minEtiqueta;
		axisY.Maximum = maxEtiqueta;
		axisY.Interval = intervalo;

		int maxLabels = 20;
		int count = 0;
		double epsilon = intervalo / 1000.0;
		for (double y = minEtiqueta; y <= maxEtiqueta + epsilon && count < maxLabels; y += intervalo)
		{
			string texto = FormatoMoneda.Formatear(Convert.ToDecimal(y));
			axisY.CustomLabels.Add(y - intervalo / 2.0, y + intervalo / 2.0, texto);
			count++;
		}
	}
}
