using System;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;

namespace ServicioTecnico.Tests;

internal static class TestsLayoutSizer
{
	private static readonly Size BaseClient = new Size(1030, 561);
	private static readonly Size ClientDoble = new Size(2060, 1122);
	private static readonly Size ClientMitad = new Size(515, 281);
	private static readonly Size ClientMini = new Size(100, 100);
	private static readonly Size ClientEnorme = new Size(6000, 4000);
	private static readonly Size ClientAncho = new Size(2000, 561);
	private static readonly Size ClientMedio = new Size(1545, 842);

	public static TestResult LS_FACTOR_001()
	{
		return Verificar(delegate
		{
			using (Fixture f = Fixture.Crear())
			using (LayoutSizer sizer = LayoutSizer.Capture(f.Contenedor, BaseClient))
			{
				if (sizer.SnapshotCount != 7)
				{
					return "SnapshotCount esperado 7, actual " + sizer.SnapshotCount;
				}
				float factor = sizer.ComputeFactor(BaseClient);
				if (Math.Abs(factor - 1f) > 0.0001f)
				{
					return "factor esperado 1.0, actual " + factor;
				}
				sizer.Apply(BaseClient);
				if (Math.Abs(sizer.CurrentFactor - 1f) > 0.0001f)
				{
					return "CurrentFactor esperado 1.0, actual " + sizer.CurrentFactor;
				}
				return VerificarLayoutBase(f, 1f);
			}
		});
	}

	public static TestResult LS_FACTOR_002()
	{
		return Verificar(delegate
		{
			using (Fixture f = Fixture.Crear())
			using (LayoutSizer sizer = LayoutSizer.Capture(f.Contenedor, BaseClient))
			{
				float factor = sizer.ComputeFactor(ClientDoble);
				if (Math.Abs(factor - 2f) > 0.0001f)
				{
					return "factor esperado 2.0, actual " + factor;
				}
				sizer.Apply(ClientDoble);
				return VerificarLayoutDoble(f);
			}
		});
	}

	public static TestResult LS_FACTOR_003()
	{
		return Verificar(delegate
		{
			using (Fixture f = Fixture.Crear())
			using (LayoutSizer sizer = LayoutSizer.Capture(f.Contenedor, BaseClient))
			{
				float factor = sizer.ComputeFactor(ClientMitad);
				if (Math.Abs(factor - 0.5f) > 0.0001f)
				{
					return "factor esperado 0.5, actual " + factor;
				}
				sizer.Apply(ClientMitad);
				if (f.Contenedor.Location != new Point(5, 4))
				{
					return "contenedor esperado (5,4), actual " + f.Contenedor.Location;
				}
				if (f.Contenedor.Size != new Size(505, 272))
				{
					return "contenedor esperado 505x272, actual " + f.Contenedor.Size;
				}
				if (f.TxtFalla.Location != new Point(62, 161))
				{
					return "txtFalla esperado (62,161), actual " + f.TxtFalla.Location;
				}
				if (f.TxtFalla.Width != 435)
				{
					return "txtFalla.Width esperado 435, actual " + f.TxtFalla.Width;
				}
				return VerificarFuente(f.TxtFalla, 8.25f, 0.5f);
			}
		});
	}

	public static TestResult LS_FACTOR_004()
	{
		return Verificar(delegate
		{
			using (Fixture f = Fixture.Crear())
			using (LayoutSizer sizer = LayoutSizer.Capture(f.Contenedor, BaseClient))
			{
				float factor = sizer.ComputeFactor(ClientMini);
				if (Math.Abs(factor - 0.25f) > 0.0001f)
				{
					return "factor esperado 0.25 (limite inferior), actual " + factor;
				}
				sizer.Apply(ClientMini);
				if (f.Contenedor.Location != new Point(0, 0))
				{
					return "contenedor esperado (0,0), actual " + f.Contenedor.Location;
				}
				if (f.Contenedor.Size != new Size(252, 136))
				{
					return "contenedor esperado 252x136, actual " + f.Contenedor.Size;
				}
				if (f.TxtFalla.Location != new Point(31, 80))
				{
					return "txtFalla esperado (31,80), actual " + f.TxtFalla.Location;
				}
				return null;
			}
		});
	}

	public static TestResult LS_FACTOR_005()
	{
		return Verificar(delegate
		{
			using (Fixture f = Fixture.Crear())
			using (LayoutSizer sizer = LayoutSizer.Capture(f.Contenedor, BaseClient))
			{
				float factor = sizer.ComputeFactor(ClientEnorme);
				if (Math.Abs(factor - 4f) > 0.0001f)
				{
					return "factor esperado 4.0 (limite superior), actual " + factor;
				}
				sizer.Apply(ClientEnorme);
				if (f.Contenedor.Location != new Point(982, 914))
				{
					return "contenedor esperado (982,914), actual " + f.Contenedor.Location;
				}
				if (f.Contenedor.Size != new Size(4036, 2172))
				{
					return "contenedor esperado 4036x2172, actual " + f.Contenedor.Size;
				}
				if (f.TxtFalla.Location != new Point(492, 1284))
				{
					return "txtFalla esperado (492,1284), actual " + f.TxtFalla.Location;
				}
				return null;
			}
		});
	}

	public static TestResult LS_RATIO_001()
	{
		return Verificar(delegate
		{
			using (Fixture f = Fixture.Crear())
			using (LayoutSizer sizer = LayoutSizer.Capture(f.Contenedor, BaseClient))
			{
				float factor = sizer.ComputeFactor(ClientAncho);
				if (Math.Abs(factor - 1f) > 0.0001f)
				{
					return "factor esperado 1.0 (Min uniforme), actual " + factor;
				}
				sizer.Apply(ClientAncho);
				if (f.Contenedor.Size != new Size(1009, 543))
				{
					return "contenedor no debe estirarse: esperado 1009x543, actual " + f.Contenedor.Size;
				}
				if (f.TxtFalla.Width != 869)
				{
					return "txtFalla.Width no debe estirarse: esperado 869, actual " + f.TxtFalla.Width;
				}
				if (f.ImgFoto1.Size != new Size(166, 125))
				{
					return "imgFoto1 no debe estirarse: esperado 166x125, actual " + f.ImgFoto1.Size;
				}
				if (f.Contenedor.Location != new Point(12, 12))
				{
					return "con factor 1.0 debe prevalecer la identidad exacta (12,12), actual " + f.Contenedor.Location;
				}
				return null;
			}
		});
	}

	public static TestResult LS_SNAP_001()
	{
		return Verificar(delegate
		{
			using (Fixture f = Fixture.Crear())
			using (LayoutSizer sizer = LayoutSizer.Capture(f.Contenedor, BaseClient))
			{
				sizer.Apply(ClientDoble);
				sizer.Apply(ClientDoble);
				sizer.Apply(ClientDoble);
				if (f.TxtFalla.Location != new Point(246, 642))
				{
					return "idempotencia violada: esperado (246,642), actual " + f.TxtFalla.Location + " (no debe componer desde el estado actual)";
				}
				if (f.TxtFalla.Width != 1738)
				{
					return "idempotencia violada en ancho: esperado 1738, actual " + f.TxtFalla.Width;
				}
				return null;
			}
		});
	}

	public static TestResult LS_RESTORE_001()
	{
		return Verificar(delegate
		{
			using (Fixture f = Fixture.Crear())
			using (LayoutSizer sizer = LayoutSizer.Capture(f.Contenedor, BaseClient))
			{
				sizer.Apply(ClientDoble);
				sizer.Apply(BaseClient);
				return VerificarLayoutBase(f, 1f);
			}
		});
	}

	public static TestResult LS_CYCLE_001()
	{
		return Verificar(delegate
		{
			using (Fixture f = Fixture.Crear())
			using (LayoutSizer sizer = LayoutSizer.Capture(f.Contenedor, BaseClient))
			{
				for (int i = 0; i < 10; i++)
				{
					sizer.Apply(ClientDoble);
					sizer.Apply(BaseClient);
				}
				return VerificarLayoutBase(f, 1f);
			}
		});
	}

	public static TestResult LS_CYCLE_002()
	{
		return Verificar(delegate
		{
			using (Fixture f = Fixture.Crear())
			using (LayoutSizer sizer = LayoutSizer.Capture(f.Contenedor, BaseClient))
			{
				for (int i = 0; i < 100; i++)
				{
					sizer.Apply(ClientDoble);
					sizer.Apply(ClientMedio);
					sizer.Apply(BaseClient);
				}
				return VerificarLayoutBase(f, 1f);
			}
		});
	}

	public static TestResult LS_CENTER_001()
	{
		return Verificar(delegate
		{
			using (Fixture f = Fixture.Crear())
			using (LayoutSizer sizer = LayoutSizer.Capture(f.Contenedor, BaseClient))
			{
				// Con factor != 1.0 el centrado proporcional aplica: cliente con
				// excedente vertical (fx=2.0 limita, fy sobra) → panel centrado.
				sizer.Apply(new Size(2060, 1400));
				if (f.Contenedor.Location != new Point(21, 157))
				{
					return "contenedor esperado (21,157) centrado con excedente vertical, actual " + f.Contenedor.Location;
				}
				if (f.Contenedor.Size != new Size(2018, 1086))
				{
					return "contenedor esperado 2018x1086, actual " + f.Contenedor.Size;
				}
				if (f.TxtFalla.Location != new Point(246, 642))
				{
					return "txtFalla esperado (246,642) con factor 2.0, actual " + f.TxtFalla.Location;
				}
				return null;
			}
		});
	}

	public static TestResult LS_CENTER_002()
	{
		return Verificar(delegate
		{
			using (Fixture f = Fixture.Crear())
			using (LayoutSizer sizer = LayoutSizer.Capture(f.Contenedor, BaseClient))
			{
				sizer.Apply(BaseClient);
				Point primera = f.Contenedor.Location;
				sizer.Apply(BaseClient);
				Point segunda = f.Contenedor.Location;
				if (primera != new Point(12, 12) || segunda != new Point(12, 12))
				{
					return "identidad no deterministica: primera " + primera + ", segunda " + segunda;
				}
				if (!ReferenceEquals(f.TxtFalla.Font, f.FuenteDefault))
				{
					return "la referencia de fuente original debe permanecer en identidad";
				}
				return null;
			}
		});
	}

	public static TestResult LS_FONT_001()
	{
		return Verificar(delegate
		{
			using (Fixture f = Fixture.Crear())
			using (LayoutSizer sizer = LayoutSizer.Capture(f.Contenedor, BaseClient))
			{
				sizer.Apply(BaseClient);
				sizer.Apply(ClientDoble);
				string errorFuente = VerificarFuentesLegibles(f);
				if (errorFuente != null)
				{
					return errorFuente;
				}
				if (!ReferenceEquals(f.TxtFalla.Font, f.BtnGuardar.Font))
				{
					return "fuentes del mismo spec deben compartir instancia (pool)";
				}
				if (ReferenceEquals(f.TxtFalla.Font, f.CmbEstado.Font))
				{
					return "fuentes de distinto spec no deben compartir instancia";
				}
				sizer.Apply(ClientMedio);
				errorFuente = VerificarFuentesLegibles(f);
				if (errorFuente != null)
				{
					return errorFuente;
				}
				sizer.Apply(BaseClient);
				errorFuente = VerificarFuentesLegibles(f);
				if (errorFuente != null)
				{
					return errorFuente;
				}
				if (f.CmbEstado.Font.Bold != true)
				{
					return "cmbEstado debe conservar FontStyle.Bold";
				}
				if (f.TxtFalla.Font.Bold != false)
				{
					return "txtFalla debe conservar FontStyle.Regular";
				}
				return VerificarFuente(f.TxtFalla, 8.25f, 1f);
			}
		});
	}

	public static TestResult LS_RECUR_001()
	{
		return Verificar(delegate
		{
			using (Fixture f = Fixture.Crear())
			using (LayoutSizer sizer = LayoutSizer.Capture(f.Contenedor, BaseClient))
			{
				sizer.Apply(ClientDoble);
				if (f.GrpTipo.Location != new Point(32, 380))
				{
					return "grpTipo esperado (32,380), actual " + f.GrpTipo.Location;
				}
				if (f.GrpTipo.Size != new Size(1326, 108))
				{
					return "grpTipo esperado 1326x108, actual " + f.GrpTipo.Size;
				}
				if (f.CmbTipo.Location != new Point(60, 36))
				{
					return "cmbTipo (hijo de GroupBox) esperado (60,36), actual " + f.CmbTipo.Location;
				}
				if (f.CmbTipo.Width != 860)
				{
					return "cmbTipo.Width esperado 860, actual " + f.CmbTipo.Width;
				}
				return VerificarFuente(f.CmbTipo, 8.25f, 2f);
			}
		});
	}

	public static TestResult LS_DPI_001()
	{
		return Verificar(delegate
		{
			using (Fixture f = Fixture.Crear())
			using (LayoutSizer sizer = LayoutSizer.Capture(f.Contenedor, BaseClient))
			{
				// Documentación ejecutable de la limitación DPI 150% (P2.1):
				// una app DPI-Unaware reporta el mismo ClientSize lógico (1030x561)
				// a 100/125/150% en tamaño normal, por lo que el factor es 1.0 y
				// LayoutSizer no aplica escalado alguno. El overflow físico a 150%
				// (bitmap scaling del OS, p.ej. 1030x561 -> 1545x842 sobre 1366x768)
				// NO queda corregido por este cálculo; corregirlo exige
				// DPI-awareness o acotar el tamaño inicial, fuera del alcance de P2.
				float factor = sizer.ComputeFactor(BaseClient);
				if (Math.Abs(factor - 1f) > 0.0001f)
				{
					return "en tamaño normal el factor debe ser exactamente 1.0 (raíz de la limitación DPI documentada), actual " + factor;
				}
				sizer.Apply(BaseClient);
				if (f.Contenedor.Size != new Size(1009, 543))
				{
					return "el sizer no debe modificar dimensiones en tamaño normal, actual " + f.Contenedor.Size;
				}
				if (f.Contenedor.Location != new Point(12, 12))
				{
					return "el sizer no debe modificar posición en tamaño normal, actual " + f.Contenedor.Location;
				}
				return null;
			}
		});
	}

	private static string VerificarLayoutBase(Fixture f, float factor)
	{
		if (f.Contenedor.Location != new Point(12, 12))
		{
			return "contenedor esperado (12,12) identidad exacta con el diseño, actual " + f.Contenedor.Location;
		}
		if (f.Contenedor.Size != new Size(1009, 543))
		{
			return "contenedor esperado 1009x543, actual " + f.Contenedor.Size;
		}
		if (f.TxtFalla.Location != new Point(123, 321))
		{
			return "txtFalla esperado (123,321), actual " + f.TxtFalla.Location;
		}
		if (f.TxtFalla.Width != 869)
		{
			return "txtFalla.Width esperado 869, actual " + f.TxtFalla.Width;
		}
		if (f.LblNombre.Location != new Point(253, 106))
		{
			return "lblNombre esperado (253,106), actual " + f.LblNombre.Location;
		}
		if (f.CmbEstado.Location != new Point(529, 483))
		{
			return "cmbEstado esperado (529,483), actual " + f.CmbEstado.Location;
		}
		if (f.CmbEstado.Width != 351)
		{
			return "cmbEstado.Width esperado 351, actual " + f.CmbEstado.Width;
		}
		if (f.GrpTipo.Location != new Point(16, 190))
		{
			return "grpTipo esperado (16,190), actual " + f.GrpTipo.Location;
		}
		if (f.GrpTipo.Size != new Size(663, 54))
		{
			return "grpTipo esperado 663x54, actual " + f.GrpTipo.Size;
		}
		if (f.CmbTipo.Location != new Point(30, 18))
		{
			return "cmbTipo esperado (30,18), actual " + f.CmbTipo.Location;
		}
		if (f.CmbTipo.Width != 430)
		{
			return "cmbTipo.Width esperado 430, actual " + f.CmbTipo.Width;
		}
		if (f.BtnGuardar.Location != new Point(646, 400))
		{
			return "btnGuardar esperado (646,400), actual " + f.BtnGuardar.Location;
		}
		if (f.BtnGuardar.Size != new Size(234, 79))
		{
			return "btnGuardar esperado 234x79, actual " + f.BtnGuardar.Size;
		}
		if (f.ImgFoto1.Location != new Point(16, 399))
		{
			return "imgFoto1 esperado (16,399), actual " + f.ImgFoto1.Location;
		}
		if (f.ImgFoto1.Size != new Size(166, 125))
		{
			return "imgFoto1 esperado 166x125, actual " + f.ImgFoto1.Size;
		}
		string errorFuente = VerificarFuente(f.TxtFalla, 8.25f, factor);
		if (errorFuente != null)
		{
			return errorFuente;
		}
		errorFuente = VerificarFuente(f.CmbEstado, 21.75f, factor);
		if (errorFuente != null)
		{
			return errorFuente;
		}
		if (!ReferenceEquals(f.TxtFalla.Font, f.FuenteDefault))
		{
			return "txtFalla debe restaurar la referencia de fuente original (identidad byte a byte)";
		}
		if (!ReferenceEquals(f.CmbEstado.Font, f.FuenteGrande))
		{
			return "cmbEstado debe restaurar la referencia de fuente original (identidad byte a byte)";
		}
		return null;
	}

	private static string VerificarLayoutDoble(Fixture f)
	{
		if (f.Contenedor.Location != new Point(21, 18))
		{
			return "contenedor esperado (21,18), actual " + f.Contenedor.Location;
		}
		if (f.Contenedor.Size != new Size(2018, 1086))
		{
			return "contenedor esperado 2018x1086, actual " + f.Contenedor.Size;
		}
		if (f.TxtFalla.Location != new Point(246, 642))
		{
			return "txtFalla esperado (246,642), actual " + f.TxtFalla.Location;
		}
		if (f.TxtFalla.Width != 1738)
		{
			return "txtFalla.Width esperado 1738, actual " + f.TxtFalla.Width;
		}
		if (f.LblNombre.Location != new Point(506, 212))
		{
			return "lblNombre esperado (506,212), actual " + f.LblNombre.Location;
		}
		if (f.CmbEstado.Location != new Point(1058, 966))
		{
			return "cmbEstado esperado (1058,966), actual " + f.CmbEstado.Location;
		}
		if (f.CmbEstado.Width != 702)
		{
			return "cmbEstado.Width esperado 702, actual " + f.CmbEstado.Width;
		}
		if (f.BtnGuardar.Location != new Point(1292, 800))
		{
			return "btnGuardar esperado (1292,800), actual " + f.BtnGuardar.Location;
		}
		if (f.BtnGuardar.Size != new Size(468, 158))
		{
			return "btnGuardar esperado 468x158, actual " + f.BtnGuardar.Size;
		}
		if (f.ImgFoto1.Location != new Point(32, 798))
		{
			return "imgFoto1 esperado (32,798), actual " + f.ImgFoto1.Location;
		}
		if (f.ImgFoto1.Size != new Size(332, 250))
		{
			return "imgFoto1 esperado 332x250, actual " + f.ImgFoto1.Size;
		}
		string errorFuente = VerificarFuente(f.TxtFalla, 8.25f, 2f);
		if (errorFuente != null)
		{
			return errorFuente;
		}
		return VerificarFuente(f.CmbEstado, 21.75f, 2f);
	}

	private static string VerificarFuente(Control control, float tamanoBase, float factor)
	{
		float esperado = (float)Math.Round((double)(tamanoBase * factor), 2);
		if (Math.Abs(control.Font.Size - esperado) > 0.001f)
		{
			return control.Name + ": fuente esperada " + esperado + ", actual " + control.Font.Size;
		}
		return null;
	}

	private static string VerificarFuentesLegibles(Fixture f)
	{
		Control[] controles = new Control[]
		{
			f.TxtFalla, f.LblNombre, f.CmbEstado, f.GrpTipo, f.CmbTipo, f.BtnGuardar, f.ImgFoto1
		};
		Control[] array = controles;
		foreach (Control control in array)
		{
			try
			{
				string nombre = control.Font.Name;
				float tamano = control.Font.Size;
				FontStyle estilo = control.Font.Style;
				if (string.IsNullOrEmpty(nombre) || tamano <= 0f)
				{
					return control.Name + ": fuente ilegible tras Apply";
				}
				if (estilo != FontStyle.Regular && estilo != FontStyle.Bold)
				{
					return control.Name + ": estilo de fuente inesperado " + estilo;
				}
			}
			catch (Exception ex)
			{
				return control.Name + ": fuente dispuesta mientras estaba referenciada: " + ex.GetType().Name;
			}
		}
		return null;
	}

	private static TestResult Verificar(Func<string> cuerpo)
	{
		string detalle = null;
		Exception error = null;
		Thread hilo = new Thread(() =>
		{
			try
			{
				detalle = cuerpo();
			}
			catch (Exception ex)
			{
				error = ex;
			}
		});
		hilo.SetApartmentState(ApartmentState.STA);
		hilo.Start();
		hilo.Join();
		if (error != null)
		{
			return Fail("excepcion: " + error.Message);
		}
		if (detalle != null)
		{
			return Fail(detalle);
		}
		return Pass();
	}

	private static TestResult Fail(string detalle)
	{
		TestResult r = new TestResult();
		r.Ok = false;
		r.Detalle = detalle;
		return r;
	}

	private static TestResult Pass()
	{
		TestResult r = new TestResult();
		r.Ok = true;
		r.Detalle = "";
		return r;
	}

	private sealed class Fixture : IDisposable
	{
		public Panel Padre;
		public Panel Contenedor;
		public TextBox TxtFalla;
		public Label LblNombre;
		public ComboBox CmbEstado;
		public GroupBox GrpTipo;
		public ComboBox CmbTipo;
		public Button BtnGuardar;
		public PictureBox ImgFoto1;
		public Font FuenteDefault;
		public Font FuenteGrande;

		public static Fixture Crear()
		{
			Font fuenteDefault = new Font("Microsoft Sans Serif", 8.25f);
			Font fuenteGrande = new Font("Microsoft Sans Serif", 21.75f, FontStyle.Bold);

			TextBox txtFalla = new TextBox();
			txtFalla.Name = "txtFalla";
			txtFalla.Location = new Point(123, 321);
			txtFalla.Size = new Size(869, 20);
			txtFalla.Font = fuenteDefault;

			Label lblNombre = new Label();
			lblNombre.Name = "lblNombre";
			lblNombre.AutoSize = true;
			lblNombre.Location = new Point(253, 106);
			lblNombre.Text = "NOMBRE:";
			lblNombre.Font = fuenteDefault;

			ComboBox cmbEstado = new ComboBox();
			cmbEstado.Name = "cmbEstado";
			cmbEstado.Location = new Point(529, 483);
			cmbEstado.Size = new Size(351, 41);
			cmbEstado.Font = fuenteGrande;

			GroupBox grpTipo = new GroupBox();
			grpTipo.Name = "grpTipo";
			grpTipo.Location = new Point(16, 190);
			grpTipo.Size = new Size(663, 54);
			grpTipo.Text = "Tipo de equipo";
			grpTipo.Font = fuenteDefault;

			ComboBox cmbTipo = new ComboBox();
			cmbTipo.Name = "cmbTipo";
			cmbTipo.DropDownStyle = ComboBoxStyle.DropDownList;
			cmbTipo.Location = new Point(30, 18);
			cmbTipo.Size = new Size(430, 21);
			cmbTipo.Font = fuenteDefault;

			Button btnGuardar = new Button();
			btnGuardar.Name = "btnGuardar";
			btnGuardar.Location = new Point(646, 400);
			btnGuardar.Size = new Size(234, 79);
			btnGuardar.Font = fuenteDefault;

			PictureBox imgFoto1 = new PictureBox();
			imgFoto1.Name = "imgFoto1";
			imgFoto1.Location = new Point(16, 399);
			imgFoto1.Size = new Size(166, 125);
			imgFoto1.Font = fuenteDefault;

			grpTipo.Controls.Add(cmbTipo);

			Panel contenedor = new Panel();
			contenedor.Name = "Contenedor";
			contenedor.Location = new Point(12, 12);
			contenedor.Size = new Size(1009, 543);
			contenedor.Controls.Add(txtFalla);
			contenedor.Controls.Add(lblNombre);
			contenedor.Controls.Add(cmbEstado);
			contenedor.Controls.Add(grpTipo);
			contenedor.Controls.Add(btnGuardar);
			contenedor.Controls.Add(imgFoto1);

			Panel padre = new Panel();
			padre.Name = "Padre";
			padre.Size = new Size(1030, 561);
			padre.Controls.Add(contenedor);

			return new Fixture
			{
				Padre = padre,
				Contenedor = contenedor,
				TxtFalla = txtFalla,
				LblNombre = lblNombre,
				CmbEstado = cmbEstado,
				GrpTipo = grpTipo,
				CmbTipo = cmbTipo,
				BtnGuardar = btnGuardar,
				ImgFoto1 = imgFoto1,
				FuenteDefault = fuenteDefault,
				FuenteGrande = fuenteGrande
			};
		}

		public void Dispose()
		{
			try
			{
				Padre.Dispose();
			}
			catch (Exception)
			{
			}
			try
			{
				FuenteDefault.Dispose();
				FuenteGrande.Dispose();
			}
			catch (Exception)
			{
			}
		}
	}
}
