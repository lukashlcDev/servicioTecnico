# Baseline Tecnica - ServicioTecnico .NET Framework 4.8.1

Fecha: 2026-08-20
Version del documento: 1.0.0
Estado: Post-Fase 2 de migracion (net481)
Objetivo: Documentar el estado tecnico real del sistema como referencia para futuras mejoras.

---

## A. Resumen tecnico de la version actual

| Atributo | Valor |
|---|---|
| Target Framework | .NET Framework 4.8.1 (`net481`) |
| Tipo de aplicacion | WinForms, WinExe, x86 |
| SDK | `Microsoft.NET.Sdk.WindowsDesktop` |
| Solucion | `ServicioTecnico.slnx` (VS 2022 17.13+) |
| Punto de entrada | `ServicioTecnico.My.MyApplication.Main` |
| Form principal | `frmOrdenServicio` |
| Base de datos | SQLite `ordenes.db` en `Application.StartupPath` |
| Proveedor SQLite | `System.Data.SQLite` 1.0.119.0 (mixto x86) |
| Codigos de barras | `BarcodeLib` 1.0.0.21 (IL-only) |
| Graficas | `System.Windows.Forms.DataVisualization.Charting` |
| Idioma del codigo | C# decompilado desde VB.NET (estilo ILSpy) |
| Tests | 3 automatizados (FUN-041..043), 56 manuales |
| Configuracion de la app | Almacenada en tabla `configuracion` de SQLite. `App.config` esta vacio y no se usa. |

Arquitectura de datos: Toda la configuracion vive en la tabla `configuracion` de SQLite. `App.config` esta vacio y no se usa. La aplicacion almacena fechas como texto `dd/MM/yyyy`, dinero como `REAL`, y estados como texto libre.

---

## B. Estructura del proyecto

### B.1 Arbol de archivos

```
servicioTecnico/
├── ServicioTecnico.slnx
├── ServicioTecnico.csproj
├── App.config                                  (vacio, no se usa)
├── AGENTS.md
├── .gitignore
├── Properties/
│   └── AssemblyInfo.cs
├── Resources/
│   ├── logo.jpg
│   └── work.ico
├── Lib/
│   ├── System.Data.SQLite.dll                  (1.0.119.0, mixto x86)
│   └── BarcodeLib.dll                          (1.0.0.21, IL-only)
├── ServicioTecnico/
│   ├── modConexion.cs                          (138 lineas)
│   ├── Cliente.cs                              (27 lineas)
│   ├── frmOrdenServicio.cs                     (1365 lineas)
│   ├── frmOrdenServicio.Designer.cs            (752 lineas)
│   ├── frmBuscaOrden.cs                        (137 lineas)
│   ├── frmBuscaOrden.Designer.cs
│   ├── frmBuscaOrden.resx
│   ├── frmClientes.cs                          (208 lineas)
│   ├── frmClientes.Designer.cs
│   ├── frmClienteDetalle.cs                    (122 lineas)
│   ├── frmClienteDetalle.Designer.cs
│   ├── frmCondicionesServicio.cs               (60 lineas)
│   ├── frmCondicionesServicio.Designer.cs
│   ├── frmConfiguracion.cs                     (152 lineas)
│   ├── frmConfiguracion.Designer.cs
│   ├── frmReportesServicios.cs                 (461 lineas)
│   ├── frmReportesServicios.Designer.cs
│   ├── frmFoto.cs                              (34 lineas)
│   └── frmFoto.Designer.cs
├── ServicioTecnico.My/
│   ├── MyApplication.cs                        (45 lineas)
│   └── MyProject.cs                            (179 lineas)
├── ServicioTecnico.frmOrdenServicio.resx
├── ServicioTecnico.frmClientes.resx
├── ServicioTecnico.frmConfiguracion.resx
└── tests/
    └── ServicioTecnico.Tests/
        ├── ServicioTecnico.Tests.csproj
        ├── Program.cs
        ├── TestsModConexion.cs
        └── ReflectionModConexion.cs
```

### B.2 Inventario por tipo de archivo

| Tipo | Cantidad | Archivos |
|---|---|---|
| Formularios (.cs + .Designer.cs) | 8 pares | frmOrdenServicio, frmBuscaOrden, frmClientes, frmClienteDetalle, frmCondicionesServicio, frmConfiguracion, frmReportesServicios, frmFoto |
| Clases modelo | 1 | Cliente.cs |
| Modulos estaticos | 1 | modConexion.cs |
| Infraestructura VB.NET | 2 | MyApplication.cs, MyProject.cs |
| Recursos | 2 | logo.jpg, work.ico |
| Archivos .resx | 4 | 3 en raiz del repositorio, 1 en ServicioTecnico/ |
| Tests | 3 archivos | Program.cs, TestsModConexion.cs, ReflectionModConexion.cs |

### B.3 Ubicacion de archivos .resx

La mayoria de `.resx` de formularios estan en la raiz del repositorio con formato `ServicioTecnico.frmX.resx`:

| Archivo | Ubicacion |
|---|---|
| `ServicioTecnico.frmOrdenServicio.resx` | raiz del repositorio |
| `ServicioTecnico.frmClientes.resx` | raiz del repositorio |
| `ServicioTecnico.frmConfiguracion.resx` | raiz del repositorio |
| `frmBuscaOrden.resx` | dentro de `ServicioTecnico/` |

Los formularios frmClienteDetalle, frmCondicionesServicio, frmReportesServicios y frmFoto no tienen `.resx`.

---

## C. Formularios

### C.1 frmOrdenServicio - Form principal

| Atributo | Valor |
|---|---|
| Archivo | `ServicioTecnico/frmOrdenServicio.cs` (1365 lineas) + `.Designer.cs` (752 lineas) |
| Funcion | Gestion central de ordenes de servicio: crear, editar, buscar, eliminar, imprimir, fotos |
| ClientSize | 1030 x 561 |
| Controles | ~65 controles (TextBoxes, Buttons, PictureBoxes, RadioButtons, ComboBox, Panel, Timers) |

**Controles principales:**

| Categoria | Controles |
|---|---|
| Datos cliente | txtNombre, txtDireccion, txtDocumento, txtTelefono |
| Datos equipo | txtMarca, txtModelo, txtIMEI, txtClave, txtAccesorios, rbtnLaptop/rbtnImpresora/rbtnPC/rbtnOtros, txtOtros |
| Datos falla | txtFalla, txtObservaciones, txtReparacion |
| Fechas | txtFecha, txtReparado (DateTimePicker), txtEntregado (DateTimePicker) |
| Financiero | txtAbono, txtPresupuesto, txtTotal |
| Estado | cmbEstadoEntrega (5 items: POR REVISAR, REVISADO, DIAGNOSTICO, REPARADO, ENTREGADO) |
| Fotos | imgFoto1/2/3 (PictureBox 166x125), VER1/2/3 (botones para adjuntar), picLogo (152x133), sinImagen (oculto) |
| Acciones | btnNuevaOrden, btnGuardar (234x79), btnImprimir, btnEliminarOrden, btnBuscarOrden, btnBuscarCliente, Button1 (buscar por numero) |
| Navegacion | btnConfiguracion, btnCondiciones, btnReporte |
| Ocultos | EmpresaCorreo, EmpresaDireccion, queda (contador licencia) - todos Visible=false |

**Eventos principales (27 handlers):**

| Handler | Trigger | Funcion |
|---|---|---|
| frmOrdenServicio_Resize | Resize | Centra Panel1 |
| frmOrdenServicio_Load | Load | Inicializa combo estados, carga empresa, CheckRequiredFiles, Timer1 |
| frmOrdenServicio_FormClosing | FormClosing | Pregunta confirmacion |
| GuardarOrden | btnGuardar.Click | Validacion, upsert cliente, INSERT/UPDATE orden, fotos, auto-impresion |
| CargarOrden | txtOrden.TextChanged | JOIN ordenes+clientes, carga todos los campos |
| Imprimir | btnImprimir.Click | Lee tipo_impresion, despacha a ImprimirTicket/ImprimirCarta |
| ImprimirTicket | (interno) | Layout termico 24 chars/linea, PrintDocument |
| ImprimirCarta | (interno) | Layout carta 90 chars/linea, PrintDocument |
| BuscarClientePorDocumento | txtDocumento.KeyDown | SELECT por documento exacto |
| BuscarClientePorTelefono | txtTelefono.KeyDown | SELECT por telefono (parcial) |
| calcula | txtPresupuesto/Abono.TextChanged | total = presupuesto - abono |
| licencia_Tick | Timer 1s | Nag screen cada 30 ticks (deshabilitado) |

**Dependencias con otros formularios:**

| Formulario | Como se abre | Proposito |
|---|---|---|
| frmBuscaOrden | ShowDialog | Buscar y seleccionar orden |
| frmClientes | ShowDialog | Buscar y seleccionar cliente |
| frmConfiguracion | ShowDialog | Configurar impresora, fuente, tipo |
| frmCondicionesServicio | ShowDialog | Editar condiciones de servicio |
| frmReportesServicios | Show() (no modal) | Ver reportes y graficas |
| frmFoto | ShowDialog | Ver foto en pantalla completa |

**Base de datos:** Consultas directas contra `ordenes`, `clientes`, `configuracion`. Patron: cada operacion crea su propia `SQLiteConnection` con `"Data Source=" + modConexion.rutaDB`. Excepcion: `ObtenerValorConfiguracion()` usa `modConexion.conexion` (conexion estatica compartida). Ver seccion D.3.

**Impresion:** Dos rutas completas (ticket/carta) con `PrintDocument`/`Graphics`. Codigo de barras CODE128. Condiciones de servicio al pie.

**Imagenes:** Hasta 3 fotos por orden. Se copian a subcarpeta `imagenes/` al guardar. Rutas relativas en BD.

**Comportamientos particulares:**
- `ObtenerValorConfiguracion` es un metodo **local privado** duplicado, distinto de `modConexion.ObtenerConfiguracion`. Ver seccion D.3.
- `CheckRequiredFiles` busca 3 archivos por nombre relativo (no por ruta completa).
- `GenerarNuevoNumeroOrden` usa `MAX(id_orden)+1` (posible race condition).
- `GuardarOrden` busca cliente por **nombre exacto** (posible fusion de duplicados).
- Timer `licencia` esta deshabilitado en Designer y nunca se habilita en codigo.

---

### C.2 frmBuscaOrden - Busqueda de ordenes

| Atributo | Valor |
|---|---|
| Archivo | `ServicioTecnico/frmBuscaOrden.cs` (137 lineas) |
| Funcion | Buscar ordenes por documento, telefono o nombre del cliente |
| ClientSize | 853 x 376 |

**Controles:** txtBuscar, rbDocumento/rbTelefono/rbNombre, dgvOrdenes (DataGridView), btnBuscar, btnSeleccionar, btnCancelar, TextBox1 (header).

**Consulta principal:**
```sql
SELECT o.id_orden, o.fecha, c.nombre, c.documento, c.telefono, o.tipo_equipo,
       o.estado_entrega, o.marca, o.modelo
FROM ordenes o INNER JOIN clientes c ON o.id_cliente = c.id_cliente
WHERE <c.documento|c.telefono|c.nombre> LIKE @criterio
ORDER BY o.fecha DESC
```
El WHERE cambia dinamicamente segun el RadioButton seleccionado. `@criterio = "%texto%"`.

**Comportamientos:**
- `OrdenSeleccionada` se inicializa en `-1` (sentinela de "sin seleccion").
- Doble clic en fila = seleccionar.
- Fechas formateadas como `dd/MM/yyyy` en el grid.
- No tiene AutoScaleMode configurado.

---

### C.3 frmClientes - Gestion de clientes

| Atributo | Valor |
|---|---|
| Archivo | `ServicioTecnico/frmClientes.cs` (208 lineas) |
| Funcion | Listar, buscar, crear, editar, eliminar y seleccionar clientes |
| ClientSize | 651 x 402 |

**Controles:** txtBuscar, dgvClientes (5 columnas: id oculta, nombre, documento, telefono, direccion oculta), btnNuevo, btnEditar, btnEliminar, btnSeleccionar, TextBox1 (header).

**Consultas:**
- Carga: `SELECT ... FROM clientes [WHERE nombre/documento/telefono LIKE @filtro] ORDER BY nombre`
- Verificacion antes de eliminar: `SELECT COUNT(*) FROM ordenes WHERE id_cliente = @id`
- Eliminacion: `DELETE FROM clientes WHERE id_cliente = @id`

**Comportamientos:**
- Busqueda en vivo (cada tecla recarga el grid con filtro).
- Eliminacion bloqueada si el cliente tiene ordenes asociadas.
- Devuelve `ClienteSeleccionado` (objeto `Cliente`) al caller via `DialogResult.OK`.
- Columna `colDireccion` oculta en UI pero disponible para `SeleccionarCliente()`.

---

### C.4 frmClienteDetalle - Alta/edicion de cliente

| Atributo | Valor |
|---|---|
| Archivo | `ServicioTecnico/frmClienteDetalle.cs` (122 lineas) |
| Funcion | Formulario dual: crear nuevo cliente o editar existente |
| ClientSize | 287 x 215 |

**Controles:** txtNombre, txtDireccion, txtDocumento, txtTelefono, btnGuardar, btnCancelar.

**Validacion:** Solo `txtNombre` es obligatorio. Los demas campos son opcionales.

**Consultas:**
- Carga: `SELECT nombre, direccion, documento, telefono FROM clientes WHERE id_cliente = @id`
- INSERT: `INSERT INTO clientes (nombre, direccion, documento, telefono) VALUES (...)`
- UPDATE: `UPDATE clientes SET ... WHERE id_cliente=@id`

**Comportamientos:**
- Constructor acepta `idCliente = 0` (nuevo) o un ID existente (edicion).
- Todos los valores se `.Trim()` antes de guardar.
- El handler de Load se llama `frmClienteDetalle_Load_1` (sufijo `_1` - artefacto de decompilacion).

---

### C.5 frmCondicionesServicio - Editor de condiciones

| Atributo | Valor |
|---|---|
| Archivo | `ServicioTecnico/frmCondicionesServicio.cs` (60 lineas) |
| Funcion | Editar el texto de condiciones que aparece al pie de la impresion |
| ClientSize | 556 x 375 |

**Controles:** txtCondiciones (multiline, Consolas 10pt), btnGuardar, btnCancelar, btnBorrar, TextBox1 (header).

**Base de datos:** Delega completamente a `modConexion.ObtenerCondicionesServicio()` y `modConexion.GuardarCondicionesServicio()`.

**Comportamientos:**
- `btnBorrar` solo limpia el TextBox, NO guarda. El usuario debe pulsar Guardar despues.
- `GuardarCondicionesServicio` hace INSERT (append-only). Las versiones anteriores quedan en BD pero solo se lee la mas reciente.
- Sin limite de caracteres en la UI ni en el codigo.

---

### C.6 frmConfiguracion - Configuracion de impresion

| Atributo | Valor |
|---|---|
| Archivo | `ServicioTecnico/frmConfiguracion.cs` (152 lineas) |
| Funcion | Configurar impresora, tipo de impresion, fuente, auto-impresion |
| ClientSize | 448 x 306 |

**Controles:** ListImpresoras (ListView), seleccionada (Label), cmbFuente (ComboBox), numTamanoFuente (NumericUpDown), rbtnTicket/rbtnCarta (RadioButton), chkImprimirAlGuardar (CheckBox), btnSeleccionarImpresora (boton "GUARDAR"), TextBox1 (header).

**Configuracion guardada (4 claves):**

| Clave | Control | Formato | Default |
|---|---|---|---|
| `impresora` | ListImpresoras (seleccion) | Nombre de impresora | `""` |
| `tipo_impresion` | rbtnTicket/rbtnCarta | `"ticket"` o `"carta"` | No insertado en inicializacion |
| `fuente_ticket` | cmbFuente + numTamanoFuente | `"Familia,Tamanio"` | `"Courier New, 9"` |
| `imprimir_al_guardar` | chkImprimirAlGuardar | `"true"` o `"false"` | `"false"` |

**Comportamientos:**
- Deteccion de fuentes monoespaciadas: compara ancho de "W" vs "I" (`TextRenderer.MeasureText`).
- `ControlBox = false` en Designer pero se sobrescribe a `true` en runtime (Load).
- `seleccionada_Click` es un handler vacio (dead code).
- Uses `Conversion.Val()` de VB.NET para parsear tamanio de fuente.

---

### C.7 frmReportesServicios - Reportes y graficas

| Atributo | Valor |
|---|---|
| Archivo | `ServicioTecnico/frmReportesServicios.cs` (461 lineas - el mas grande del proyecto) |
| Funcion | Visualizar reportes de ordenes con grid y 3 graficas |
| ClientSize | 1133 x 626 |

**Controles:** dgvReportes (DataGridView), chartTiposReparacion (274x248), chartEstadoReparaciones (274x248), chartIngresosMensuales (549x248), chartReparacionesPorMes (OCULTO), cmbFiltroFecha (OCULTO), cmbTipoGrafico (OCULTO), dtpDesde, dtpHasta, btnFiltrar, btnActualizar (OCULTO), TextBox1 (header).

**3 graficas activas:**

| Grafica | Tipo | Datos |
|---|---|---|
| chartEstadoReparaciones | Pie | Distribucion por estado_entrega |
| chartTiposReparacion | Doughnut | Distribucion por tipo_equipo (Otros consolidados) |
| chartIngresosMensuales | Column | SUM(presupuesto) por mes/anio |

**Filtros disponibles ( cmbFiltroFecha, OCULTO):**

| Indice | Filtro | Implementacion |
|---|---|---|
| 0 | Ultimos 7 dias | `DateTime.Now.AddDays(-7)` |
| 1 | Este mes (default) | Primer dia del mes actual |
| 2 | Ultimos 30 dias | `DateTime.Now.AddDays(-30)` |
| 3 | Este anio | 1 de enero del anio actual |
| 4 | Personalizado | Muestra dtpDesde/dtpHasta + btnFiltrar |

**Limite de registros:** `LIMIT 100` hardcodeado en todas las consultas de grid.

**Consultas:** 12 en total (4 sin filtro + 4 con filtro numerico + 4 con filtro string). Las 4 "Personalizado" son codigo muerto (nunca invocadas desde handlers de eventos).

**Controles no funcionales:**
- `cmbTipoGrafico` (OCULTO): tiene 4 opciones ("Barras", "Lineas", "Torta", "Area") pero el handler solo regenera las graficas con tipos hardcodeados. El selector no tiene efecto.
- `cmbFiltroFecha` (OCULTO): existe pero el usuario no lo ve.
- `btnActualizar` (OCULTO): existe pero no es visible.
- `chartReparacionesPorMes` (OCULTO): control declarado pero nunca referenciado en codigo.
- `dtpDesde`/`dtpHasta`: visibles pero solo funcionan con btnFiltrar.

**Codigo muerto:**
- `CargarDatosReportesPersonalizado()` (linea 343)
- `ObtenerDatosReportesPersonalizado()` (linea 359)
- `GenerarGraficasPersonalizado()` (linea 373)
- `GenerarGraficaEstadosReparacionPersonalizado()` (linea 380)
- `GenerarGraficaTiposEquipoPersonalizado()` (linea 408)
- `GenerarGraficaIngresosMensualesPersonalizado()` (linea 435)

---

### C.8 frmFoto - Visor de imagen

| Atributo | Valor |
|---|---|
| Archivo | `ServicioTecnico/frmFoto.cs` (34 lineas) |
| Funcion | Mostrar foto en pantalla completa (maximizado) |
| ClientSize | 555 x 397 (sobrescrito a maximizado en Load) |

**Controles:** picFoto (PictureBox).

**Comportamientos:**
- Se abre con `new Bitmap(picBox.Image)` para evitar problemas de dispose.
- `picFoto_Click` es un handler vacio (dead code).
- Sin tecla Escape para cerrar. Solo boton estandar de ventana.
- El PictureBox se dimensiona a `Height/Width` del form (incluye bordes), no del `ClientSize`.

---

### C.9 Cliente.cs - Modelo de dominio

| Atributo | Valor |
|---|---|
| Archivo | `ServicioTecnico/Cliente.cs` (27 lineas) |
| Funcion | POCO/DTO para transferir datos de cliente |

**Propiedades:** Id (int), Nombre (string), Direccion (string), Documento (string), Telefono (string).

**Constructores:** Parameterless + full (5 params).

**Comportamientos:**
- Sin `ToString()` override.
- Sin validacion, sin null checks.
- Solo se instancia en `frmClientes.SeleccionarCliente()`.

---

## D. Base de datos

### D.1 Archivo y ubicacion

| Atributo | Valor |
|---|---|
| Archivo | `ordenes.db` |
| Ubicacion | `Application.StartupPath` (junto al ejecutable) |
| Proveedor | `System.Data.SQLite` 1.0.119.0 |
| Cadena de conexion | `"Data Source=" + modConexion.rutaDB + ";Version=3;"` |
| Creacion automatica | Si, al primer arranque via `modConexion.VerificarOCrearBD()` |

### D.2 Esquema de tablas

**clientes:**
```sql
CREATE TABLE IF NOT EXISTS clientes (
  id_cliente INTEGER PRIMARY KEY AUTOINCREMENT,
  nombre TEXT NOT NULL,
  direccion TEXT,
  documento TEXT,
  telefono TEXT
)
```

**ordenes:**
```sql
CREATE TABLE IF NOT EXISTS ordenes (
  id_orden INTEGER PRIMARY KEY AUTOINCREMENT,
  fecha TEXT,
  id_cliente INTEGER,
  tipo_equipo TEXT,
  marca TEXT,
  modelo TEXT,
  imei TEXT,
  clave TEXT,
  accesorios TEXT,
  falla TEXT,
  observaciones TEXT,
  reparacion TEXT,
  abono REAL,
  reparado TEXT,
  entregado TEXT,
  presupuesto REAL,
  total REAL,
  estado_entrega TEXT,
  imagen1 TEXT,
  imagen2 TEXT,
  imagen3 TEXT,
  FOREIGN KEY(id_cliente) REFERENCES clientes(id_cliente)
)
```

**condiciones_servicio:**
```sql
CREATE TABLE IF NOT EXISTS condiciones_servicio (
  id INTEGER PRIMARY KEY AUTOINCREMENT,
  contenido TEXT NOT NULL,
  fecha_actualizacion TEXT DEFAULT CURRENT_TIMESTAMP
)
```

**configuracion:**
```sql
CREATE TABLE IF NOT EXISTS configuracion (
  clave TEXT PRIMARY KEY,
  valor TEXT
)
```

**estados:**
```sql
CREATE TABLE IF NOT EXISTS estados (
  id_estado INTEGER PRIMARY KEY AUTOINCREMENT,
  descripcion TEXT
)
```

### D.3 Patron de acceso a BD

**Patron principal (conexion local):** La mayoria de los metodos crean su propia `SQLiteConnection` local, la abren, usan y cierran en un bloque `using`. Este es el patron correcto y consistente.

Metodos que usan este patron:
- `modConexion.ObtenerConfiguracion()` (linea 71)
- `modConexion.GuardarConfiguracion()` (linea 93)
- `modConexion.ObtenerCondicionesServicio()` (linea 106)
- `modConexion.GuardarCondicionesServicio()` (linea 127)
- `modConexion.CrearTablas()` (linea 28)
- `modConexion.InsertarDatosIniciales()` (linea 50)
- Todos los metodos de frmOrdenServicio excepto `ObtenerValorConfiguracion`
- Todos los metodos de frmBuscaOrden, frmClientes, frmClienteDetalle, frmReportesServicios

**Patron secundario (conexion estatica compartida):**

El campo `modConexion.conexion` (`public static SQLiteConnection`) es utilizado por un unico metodo fuera de modConexion: `frmOrdenServicio.ObtenerValorConfiguracion()`.

Flujo completo:
1. `modConexion.conexion` se declara en `modConexion.cs:15` como `public static SQLiteConnection conexion;` (sin inicializar, null por defecto).
2. Se inicializa en `modConexion.VerificarOCrearBD()` (linea 20): `conexion = new SQLiteConnection("Data Source=" + rutaDB + ";Version=3;");`. La conexion se crea pero NO se abre en este punto.
3. Se usa exclusivamente en `frmOrdenServicio.ObtenerValorConfiguracion()` (lineas 765-798):
   - Verifica `conexion.State == ConnectionState.Closed` y la abre si es necesario.
   - Ejecuta `SELECT valor FROM configuracion WHERE clave = ?`.
   - Cierra la conexion en el bloque `finally`.
4. La cadena de invocacion que depende de este metodo es:
   ```
   btnImprimir_Click (linea 735)
     -> Imprimir() (linea 747)
       -> ObtenerValorConfiguracion("tipo_impresion") (linea 749)
         -> modConexion.conexion (lineas 770-794)

   GuardarOrden (linea 167)
     -> modConexion.ObtenerConfiguracion("imprimir_al_guardar") (linea 205)
     -> Imprimir() (linea 207) [si imprimir_al_guardar == "true"]
       -> ObtenerValorConfiguracion("tipo_impresion") (linea 749)
         -> modConexion.conexion (lineas 770-794)
   ```

**Inconsistencia de diseno:** `frmOrdenServicio.ObtenerValorConfiguracion()` es un metodo privado que duplica la funcionalidad de `modConexion.ObtenerConfiguracion()`. La diferencia es que el metodo privado usa la conexion estatica compartida (`modConexion.conexion`) mientras que el metodo publico crea una conexion local nueva. Ambos hacen exactamente lo mismo: leer un valor de la tabla `configuracion`.

La mayoria del formulario usa correctamente `modConexion.ObtenerConfiguracion()` (lineas 205, 282, 287, 451, 456), pero la ruta de impresion usa el duplicado local.

**Nota:** `modConexion.conexion` NO es dead code. Se usa en cada impresion de orden. Sin embargo, representa una inconsistencia de patron que deberia resolverse en una futura refactorizacion.

### D.4 Datos iniciales

`modConexion.InsertarDatosIniciales()` inserta 3 registros al crear la BD por primera vez:

| Clave | Valor |
|---|---|
| `impresora` | `""` |
| `imprimir_al_guardar` | `"false"` |
| `fuente_ticket` | `"Courier New, 9"` |

**Nota:** `tipo_impresion` NO se inserta en la inicializacion. Se crea recien cuando el usuario guarda la configuracion por primera vez via `frmConfiguracion`.

---

## E. Dependencias

### E.1 Dependencias de la aplicacion

| Dependencia | Version | Tipo | Ubicacion | Archivos que la usan | Importancia |
|---|---|---|---|---|---|
| System.Data.SQLite | 1.0.119.0 | DLL local (mixto x86) | `Lib/` | modConexion, frmOrdenServicio, frmBuscaOrden, frmClientes, frmClienteDetalle, frmReportesServicios, tests | Critica - toda persistencia |
| BarcodeLib | 1.0.0.21 | DLL local (IL-only) | `Lib/` | frmOrdenServicio | Alta - codigos de barras en impresion |
| DataVisualization.Charting | (GAC) | Referencia framework | GAC | frmReportesServicios | Media - 3 graficas de reportes |
| Microsoft.VisualBasic | (GAC) | Referencia framework | GAC | 6 de 8 formularios, MyApplication, MyProject | Alta - patrones decompilados |
| System.Drawing | (GAC) | Referencia framework | GAC | frmOrdenServicio, frmCondicionesServicio, frmConfiguracion, frmFoto | Media - impresion, imagenes |
| System.Drawing.Printing | (GAC) | Referencia framework | GAC | frmOrdenServicio, frmConfiguracion | Alta - impresion |
| System.Data | (GAC) | Referencia framework | GAC | frmBuscaOrden, frmClientes, frmReportesServicios | Media - DataTable/DataAdapter |

**DLLs locales:** Ambas DLLs en `Lib/` se copian junto al exe via `Private=True` en el `.csproj`. Deben distribuirse junto al ejecutable.

### E.2 Dependencias en tests

| Dependencia | Version | Nota |
|---|---|---|
| Proyecto principal | ProjectReference | `ServicioTecnico.csproj` |
| System.Data.SQLite | 1.0.119.0 | Referencia directa via HintPath |
| BarcodeLib | 1.0.0.21 | Referencia directa via HintPath (no usado en tests) |

---

## F. Impresion

### F.1 Arquitectura de impresion

La impresion se implementa manualmente con `System.Drawing.Printing.PrintDocument` y `System.Drawing.Graphics`. No se usan reportes RDLC ni Crystal Reports.

### F.2 Rutas de impresion

La funcion `Imprimir()` en `frmOrdenServicio.cs:747` despacha segun la clave `tipo_impresion`:

| Tipo | Metodo | Descripcion |
|---|---|---|
| `"ticket"` | `ImprimirTicket()` | Layout termico, 24 caracteres por linea |
| `"carta"` | `ImprimirCarta()` | Layout carta, 90 caracteres por linea |
| Otro | Mensaje de error | "El tipo de impresion configurado no es valido" |

### F.3 Impresion de ticket

- Layout termico con 24 caracteres por linea.
- Fuentes: fuente configurada por el usuario (default `Courier New, 9`).
- Logo de empresa (`picLogo.Image`) si esta disponible.
- Codigo de barras CODE128 con el numero de orden.
- Datos del cliente, equipo, falla, observaciones, reparacion.
- Condiciones de servicio al pie (via `modConexion.ObtenerCondicionesServicio()`).
- Datos de la empresa hardcodeados en TextBoxes ocultos (EmpresaCorreo, EmpresaDireccion).

### F.4 Impresion de carta

- Layout de carta con 90 caracteres por linea.
- Mismos datos que ticket pero en formato horizontal.
- Logo de empresa.
- Codigo de barras.
- Condiciones de servicio al pie.

### F.5 Impresion automatica

`GuardarOrden` verifica `modConexion.ObtenerConfiguracion("imprimir_al_guardar")` y si es `"true"`, ejecuta `Imprimir()` automaticamente despues de guardar la orden.

---

## G. Reportes

### G.1 Descripcion general

`frmReportesServicios.cs` (461 lineas) es el archivo mas grande del proyecto. Contiene un grid de datos y 3 graficas activas para visualizar ordenes de servicio.

### G.2 Graficas activas

| Grafica | Tipo de Chart | Tipo de datos | Datos |
|---|---|---|---|
| chartEstadoReparaciones | Pie | Distribucion por estado | COUNT(*) GROUP BY estado_entrega |
| chartTiposReparacion | Doughnut | Distribucion por tipo de equipo | COUNT(*) GROUP BY tipo_equipo (Otros consolidados) |
| chartIngresosMensuales | Column | Ingresos por mes | SUM(presupuesto) GROUP BY mes/anio |

### G.3 Controles no funcionales

| Control | Estado | Problema |
|---|---|---|
| `cmbTipoGrafico` | OCULTO | Tiene 4 opciones ("Barras", "Lineas", "Torta", "Area") pero el handler ignora la seleccion y regenera graficas con tipos hardcodeados |
| `cmbFiltroFecha` | OCULTO | Existe pero el usuario no lo ve |
| `btnActualizar` | OCULTO | Existe pero no es visible |
| `chartReparacionesPorMes` | OCULTO | Control declarado pero nunca referenciado en codigo |

### G.4 Limite de registros

Todas las consultas de grid tienen `LIMIT 100` hardcodeado. No hay paginacion ni indicador de que se muestran solo 100 registros.

### G.5 Filtros por fecha

Los filtros por fecha (via `cmbFiltroFecha`, actualmente oculto) son:

| Indice | Filtro | Implementacion |
|---|---|---|
| 0 | Ultimos 7 dias | `DateTime.Now.AddDays(-7)` |
| 1 | Este mes (default) | Primer dia del mes actual |
| 2 | Ultimos 30 dias | `DateTime.Now.AddDays(-30)` |
| 3 | Este anio | 1 de enero del anio actual |
| 4 | Personalizado | Muestra dtpDesde/dtpHasta + btnFiltrar |

### G.6 Codigo muerto

6 metodos completos sin invocar (lineas 343-461):
- `CargarDatosReportesPersonalizado()`
- `ObtenerDatosReportesPersonalizado()`
- `GenerarGraficasPersonalizado()`
- `GenerarGraficaEstadosReparacionPersonalizado()`
- `GenerarGraficaTiposEquipoPersonalizado()`
- `GenerarGraficaIngresosMensualesPersonalizado()`

---

## H. Tipos de equipo

### H.1 Equipos soportados

La aplicacion soporta 4 tipos de equipo, definidos como RadioButton en `frmOrdenServicio`:

| Valor en BD | RadioButton | Campo adicional |
|---|---|---|
| `Laptop` | rbtnLaptop | - |
| `Impresora` | rbtnImpresora | - |
| `PC` | rbtnPC | - |
| `Otros` | rbtnOtros | txtOtros (descripcion libre) |

### H.2 Campos de equipo

Todos los campos son opcionales excepto el tipo:

| Campo | TextBox | Max chars | Nota |
|---|---|---|---|
| tipo_equipo | RadioButtons | - | Obligatorio (uno de 4) |
| marca | txtMarca | - | Texto libre |
| modelo | txtModelo | - | Texto libre |
| imei | txtIMEI | - | Texto libre |
| clave | txtClave | - | Texto libre (password/acceso) |
| accesorios | txtAccesorios | - | Texto libre |

### H.3 Consolidacion en reportes

En `frmReportesServicios`, los 4 tipos se consolidan en 3 para la grafica `chartTiposReparacion`:
- `Laptop`
- `Impresora`
- `PC` + `Otros` se agrupan como `PC/Otros`

---

## I. Codigos de barras

### I.1 Implementacion

- Libreria: `BarcodeLib` 1.0.0.21 (IL-only, distribuida en `Lib/`).
- Tipo: CODE128.
- Uso: Generacion de codigo de barras con el numero de orden para inclusion en impresion (ticket y carta).
- Ubicacion en impresion: Despues del encabezado, antes de los datos del cliente.

### I.2 Validacion al arranque

`frmOrdenServicio.CheckRequiredFiles()` verifica que `BarcodeLib.dll` exista junto al ejecutable. Si no existe, muestra un MessageBox de advertencia.

---

## J. Configuracion

### J.1 Claves de configuracion

La configuracion se almacena en la tabla `configuracion` de SQLite. No se usa `App.config`.

| Clave | Tipo | Default | Descripcion | Donde se lee | Donde se escribe |
|---|---|---|---|---|---|
| `impresora` | string | `""` | Nombre de impresora | frmConfiguracion, frmOrdenServicio (PrintTicket/Carta) | frmConfiguracion |
| `tipo_impresion` | string | No existe hasta que el usuario guarda | `"ticket"` o `"carta"` | frmOrdenServicio (Imprimir) | frmConfiguracion |
| `fuente_ticket` | string | `"Courier New, 9"` | Fuente y tamanio para ticket | frmConfiguracion, frmOrdenServicio (PrintTicket) | frmConfiguracion |
| `imprimir_al_guardar` | string | `"false"` | `"true"` o `"false"` | frmConfiguracion, frmOrdenServicio (GuardarOrden) | frmConfiguracion |

### J.2 Inicializacion

Solo 3 de las 4 claves se insertan en `InsertarDatosIniciales()`. `tipo_impresion` no se inserta. Si el usuario nunca abre `frmConfiguracion`, la clave `tipo_impresion` no existira y `Imprimir()` mostrara un error.

### J.3 Validacion de impresora

La configuracion no valida que la impresora seleccionada exista o este disponible. Si la impresora no esta conectada, la impresion fallara en tiempo de ejecucion.

---

## K. Imagenes e iconos

### K.1 Imagenes de la aplicacion

| Imagen | Ubicacion | Uso |
|---|---|---|
| `Resources/logo.jpg` | Carpeta Resources | Logo de empresa, se muestra en `picLogo` del form principal y se incluye en impresiones |
| `Resources/work.ico` | Carpeta Resources | Icono de la aplicacion |

### K.2 Fotos de ordenes

- Hasta 3 fotos por orden (`imagen1`, `imagen2`, `imagen3` en tabla `ordenes`).
- Se guardan como archivos en subcarpeta `imagenes/` junto al ejecutable.
- Las rutas se almacenan como texto relativo en la BD.
- Se muestran en PictureBoxes de 166x125 pixeles.
- `sinImagen` (PictureBox oculto) contiene la imagen por defecto cuando no hay foto.
- Al seleccionar una foto, se crea un `new Bitmap(picBox.Image)` para evitar problemas de dispose.

---

## L. Deuda tecnica relevante

**Nota:** Esta seccion documenta el estado actual del codigo sin proponer soluciones. Las mejoras se disearan en fases futuras.

### L.1 Duplicacion de acceso a configuracion

**Comportamiento actual:** Existen dos metodos que leen valores de la tabla `configuracion`:

| Metodo | Ubicacion | Patron de conexion | Alcance |
|---|---|---|---|
| `modConexion.ObtenerConfiguracion(clave)` | modConexion.cs:71 | Conexion local nueva (using) | Publico, usado por 10+ llamadas en 3 formularios |
| `frmOrdenServicio.ObtenerValorConfiguracion(clave)` | frmOrdenServicio.cs:765 | `modConexion.conexion` (estatica compartida) | Privado, usado solo en `Imprimir()` |

Ambos metodos ejecutan exactamente la misma consulta: `SELECT valor FROM configuracion WHERE clave=@clave`. La diferencia es exclusivamente el patron de conexion.

La mayoria del formulario usa `modConexion.ObtenerConfiguracion()`. Solo la ruta de impresion (lectura de `tipo_impresion`) usa el duplicado local.

### L.2 Codigo muerto

| Ubicacion | Descripcion | Lineas |
|---|---|---|
| frmReportesServicios.cs | 6 metodos "Personalizado" sin invocar | 343-461 (~118 lineas) |
| frmReportesServicios.Designer.cs | `chartReparacionesPorMes` declarado sin uso | ~8 lineas |
| frmReportesServicios.Designer.cs | `cmbFiltroFecha` oculto sin handler visible | ~15 lineas |
| frmReportesServicios.Designer.cs | `cmbTipoGrafico` oculto sin efecto funcional | ~15 lineas |
| frmReportesServicios.Designer.cs | `btnActualizar` oculto sin handler | ~10 lineas |
| frmOrdenServicio.cs | Timer `licencia` deshabilitado, handler vacio | ~10 lineas |
| frmConfiguracion.cs | `seleccionada_Click` handler vacio | 1 linea |
| frmFoto.cs | `picFoto_Click` handler vacio | 1 linea |
| modConexion.cs | Campo `conexion` (ver L.1) | 1 linea |

### L.3 Datos como texto en SQLite

**Comportamiento actual:**
- Fechas se almacenan como texto `dd/MM/yyyy` en campo `fecha TEXT`.
- Dinero (`abono`, `presupuesto`, `total`) se almacena como `REAL`.
- Estados se almacenan como texto libre (`estado_entrega TEXT`).
- `fecha_actualizacion` en `condiciones_servicio` usa `DEFAULT CURRENT_TIMESTAMP` (formato ISO).

**Implicaciones:**
- No hay validacion de formato de fecha a nivel de BD.
- No hay constraints CHECK para estados validos.
- Las ordenes por fecha dependen del formato de texto, no de comparacion de fechas reales.
- El filtro por fecha en reportes convierte strings a DateTime manualmente.

### L.4 Condiciones de servicio append-only

**Comportamiento actual:** `GuardarCondicionesServicio()` hace INSERT (append). Las versiones anteriores quedan en la tabla pero solo se lee la mas reciente (`ORDER BY fecha_actualizacion DESC LIMIT 1`). No hay forma de ver el historial en la UI.

### L.5 Limites de la interfaz

| Limite | Ubicacion | Descripcion |
|---|---|---|
| Sin scroll en grid de ordenes | frmBuscaOrden | El DataGridView no tiene paginacion |
| Sin indicador de limite | frmReportesServicios | `LIMIT 100` sin indicador al usuario |
| Sin tecla Escape | frmFoto | No se puede cerrar con Escape |
| Sin drag-and-drop | frmClienteDetalle | Fotos solo via dialogo de archivo |
| Sin auto-guardado | frmOrdenServicio | Los cambios se pierden si el usuario cierra sin guardar |
| Sin validacion de impresora | frmConfiguracion | No verifica que la impresora exista |

### L.6 Patron de conexion inconsistente

**Comportamiento actual:** La mayoria del sistema usa conexiones locales (crear, abrir, usar, cerrar en `using`). Un solo metodo (`ObtenerValorConfiguracion`) usa la conexion estatica compartida `modConexion.conexion`. Ver seccion D.3 para el analisis detallado.

### L.7 Potential race condition

**Comportamiento actual:** `GenerarNuevoNumeroOrden()` usa `SELECT MAX(id_orden) + 1 FROM ordenes`. En un escenario de multiples usuarios simultaneos (no previsto actualmente), dos usuarios podrian obtener el mismo numero de orden.

---

## M. Mapa preliminar de impacto sobre futuras mejoras

**Nota:** Esta seccion es un mapa de referencia, no un plan de implementacion. Los items se documentan para facilitar la toma de decisiones en fases futuras.

### M.1 Impacto por area de mejora

| Area de mejora | Archivos afectados | Dependencias criticas | Complejidad estimada |
|---|---|---|---|
| Eliminar ObtenerValorConfiguracion duplicado | frmOrdenServicio.cs | Reemplazar 1 llamada por modConexion.ObtenerConfiguracion() | Baja |
| Limpiar codigo muerto en reportes | frmReportesServicios.cs, frmReportesServicios.Designer.cs | Sin dependencias externas | Baja |
| Migrar fechas de texto a REAL/ISO | modConexion.cs, frmOrdenServicio.cs, frmBuscaOrden.cs, frmReportesServicios.cs | Requiere migracion de BD | Alta |
| Agregar estados como catalogo | modConexion.cs, frmOrdenServicio.cs, frmBuscaOrden.cs, frmReportesServicios.cs | Requiere tabla estados (ya existe vacia) | Media |
| Restaurar visibilidad de filtros de reportes | frmReportesServicios.Designer.cs, frmReportesServicios.cs | cmbFiltroFecha, btnActualizar ya tienen codigo | Baja |
| Restaurar cmbTipoGrafico funcional | frmReportesServicios.cs | Requiere refactorizar GenerarGraficas() | Media |
| Eliminar LIMIT 100 o agregar paginacion | frmReportesServicios.cs | Requiere UI de paginacion | Media |
| Agregar tecla Escape a frmFoto | frmFoto.cs | Sin dependencias | Baja |
| Migrar a .NET 10 | Todos los archivos | Requiere evaluacion de compatibilidad VB.NET | Alta |
| Migrar de SQLite a SQL Server | modConexion.cs, todos los formularios | Requiere cambio de proveedor y cadena de conexion | Alta |
| Agregar Entity Framework | modConexion.cs, todos los formularios | Requiere refactorizacion completa de acceso a BD | Alta |
| Agregar unit tests | Proyecto tests | Requiere refactorizar modConexion para inyeccion de dependencias | Media |

### M.2 Dependencias entre mejoras

```
Eliminar duplicado L.1 -----> Sin bloqueos
Limpiar dead code L.2 ------> Sin bloqueos
Restaurar filtros G.3 ------> Sin bloqueos
Restaurar grafico G.3 ------> Sin bloqueos
Tecla Escape K.2 -----------> Sin bloqueos
Migrar fechas L.3 ----------> Requiere esquema nuevo
Agregar estados L.3 --------> Requiere tabla estados
Migrar a .NET 10 -----------> Requiere evaluacion completa
Migrar a SQL Server --------> Requiere cambio de proveedor
Agregar EF ------------------> Requiere migrar fechas + estados
```

### M.3 Limitaciones conocidas de esta baseline

- No incluye analisis de rendimiento ni metricas de uso.
- No incluye evaluacion de compatibilidad con versiones futuras de Windows.
- No incluye analisis de seguridad (no hay autenticacion en la app).
- No incluye estimacion de esfuerzo para ninguna mejora.
- El mapa de impacto es preliminar y debe refinarse antes de cada fase de implementacion.
