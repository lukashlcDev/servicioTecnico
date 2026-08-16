# Fase 0 — Factibilidad y Bloqueantes: Evidencia

Documento de evidencia de la Fase 0 del plan de migración de **ServicioTecnico** desde **.NET Framework 4.0** a **.NET 10**.

Referencia: `docs/migracion_dotnet10.md` (Fase 0 — Factibilidad y bloqueantes).

Estado: **SUPERADA**. Este documento es de registro; no se modificó código de la aplicación.

---

## 1. Objetivo de la Fase 0

Verificar la factibilidad técnica de la migración a .NET 10 antes de establecer la línea base:

- Determinar si `BarcodeLib.dll` es compatible o constituye un bloqueo duro.
- Identificar el proveedor SQLite actual, su naturaleza y compatibilidad con .NET 10.
- Confirmar que la arquitectura de la aplicación (WinForms / WinExe / x86) se puede conservar.
- Listar dependencias que requieren verificación posterior.
- Clasificar cada punto como **COMPATIBLE**, **REQUIERE VERIFICACIÓN** o **BLOQUEANTE**.

El análisis se realizó sobre el estado actual del repositorio sin efectuar modificaciones:

- Lectura de `ServicioTecnico.csproj`, `App.config`, `ServicioTecnico.csproj.user`, `Properties/AssemblyInfo.cs`, `ServicioTecnico.slnx`.
- Inspección de metadatos CLI/PE de las DLLs de `Lib/`.
- Revisión de usos de `BarcodeLib` y `System.Data.SQLite` en el código fuente y los tests.

---

## 2. BarcodeLib.dll

### 2.1 Identificación

| Atributo | Valor |
|---|---|
| Versión | **1.0.0.21** (tabla Assembly de metadatos CLI) |
| Fecha de archivo | 16-abr-2018 |
| Ubicación | `Lib/BarcodeLib.dll`; referencia `HintPath` + `Private=True` en `ServicioTecnico.csproj` (ref. `BarcodeLib`) y en `tests/ServicioTecnico.Tests/ServicioTecnico.Tests.csproj`. Copiada a `bin/Debug/net40/` |
| Framework objetivo | CLR **v2.0.50727** → .NET Framework 2.0 |
| Arquitectura | PE32 / **x86** (bitness preferido del compilador) |
| Tipo de ensamblado | **Administrada (IL-only)**. `COMIMAGE_FLAGS_ILONLY = 0x1`; sin código nativo; sin strong name |
| Referencias (AssemblyRef) | `mscorlib`, `System`, `System.Data`, `System.Drawing`, `System.Xml` — todas versión 2.0.0.0 |

### 2.2 APIs utilizadas

Únicamente en `ServicioTecnico/frmOrdenServicio.cs` (`using BarcodeLib;` en línea 12):

- Generación CODE128: líneas 649-657 (`new Barcode()`, `IncludeLabel`, `AlignmentPositions.CENTER`, `LabelFont`, `Width`, `Height`, `Encode(TYPE.CODE128, codigo)` y `Encode(TYPE.CODE128, codigo, Color.Black, Color.White, ancho, altura)`).
- Generación CODE39: líneas 662-670 (mismo patrón con `TYPE.CODE39`).
- El `Image` resultante se dibuja con `e.Graphics.DrawImage(...)` en la impresión (`PrintDocument`).
- La aplicación valida al arranque que `BarcodeLib.dll` exista junto al exe (`frmOrdenServicio.cs`, línea 110).

### 2.3 Compatibilidad con .NET 10

Alta. El ensamblado es 100% IL, no contiene código nativo y sus dependencias (`mscorlib`, `System`, `System.Data`, `System.Drawing`, `System.Xml`) tienen equivalente en .NET 10 sobre Windows (`System.Drawing` → `System.Drawing.Common`, incluido en el runtime Windows Desktop). No se detectaron APIs eliminadas ni dependencias nativas. La validación final (generación CODE128/CODE39 y salida de impresión) se realizará en las fases de regresión.

### 2.4 Conclusión

**COMPATIBLE** — no bloqueante.

---

## 3. SQLite

### 3.1 Proveedor actual

| Atributo | Valor |
|---|---|
| Proveedor | `System.Data.SQLite` (proveedor ADO.NET oficial de SQLite), distribución **"bundle"** de un único archivo |
| Versión | **1.0.119.0** (último release; evidencia embebida: timestamp `2024-09-15 19:03:31 UTC` y referencia a `SQLite.Designer, Version=1.0.119.0`) |
| Ubicación | `Lib/System.Data.SQLite.dll`; referencia `HintPath` + `Private=True` en `ServicioTecnico.csproj` (ref. `System.Data.SQLite`) y en `tests/ServicioTecnico.Tests/ServicioTecnico.Tests.csproj`. Copiada a `bin/Debug/net40/` |
| Arquitectura | **x86** (PE32). Solo carga en proceso x86 |
| Framework objetivo | **.NET Framework 4.0 Client Profile** (CLR v4.0.30319) |
| Naturaleza del ensamblado | **Mixto**. `COMIMAGE_FLAGS = 0x18` (`NATIVE_ENTRYPOINT` + `STRONGNAMESIGNED`): contiene código nativo (núcleo SQLite embebido, manifest interno `SQLite.Interop.2010`) además del wrapper administrado |

### 3.2 Archivos que la utilizan

- `ServicioTecnico/modConexion.cs` (módulo central: conexión, creación de tablas, configuración, condiciones de servicio).
- `ServicioTecnico/frmOrdenServicio.cs`
- `ServicioTecnico/frmClientes.cs`
- `ServicioTecnico/frmClienteDetalle.cs`
- `ServicioTecnico/frmBuscaOrden.cs`
- `ServicioTecnico/frmReportesServicios.cs`
- `tests/ServicioTecnico.Tests/TestsModConexion.cs`

### 3.3 APIs utilizadas

- `SQLiteConnection` con cadena de conexión `"Data Source=...;Version=3;"`; `Open()` / `Close()`.
- `SQLiteCommand` (`CommandText`, `Connection`, `Parameters.AddWithValue`, `ExecuteNonQuery`, `ExecuteScalar`, `ExecuteReader`).
- `SQLiteDataReader` (lectura por ordinal y por nombre de columna, `IsDBNull`, `GetOrdinal`).
- `SQLiteDataAdapter.Fill(DataTable)` (reportes y búsquedas).
- Conexión estática global `modConexion.conexion` + conexiones locales por operación.

### 3.4 Operaciones relevantes

- `CREATE TABLE IF NOT EXISTS` (tablas `condiciones_servicio`, `clientes`, `ordenes`, `configuracion`, `estados`).
- `INSERT`, `INSERT OR IGNORE`, `INSERT OR REPLACE`, `UPDATE`, `DELETE`.
- `SELECT` con parámetros `@clave`, `last_insert_rowid()`, `COUNT(*)`, `MAX(id_orden)`, `LIKE`, `LEFT JOIN`, `ORDER BY ... LIMIT`.
- No se utilizan transacciones explícitas en el código actual.

### 3.5 Incompatibilidad del binario actual con .NET 10

El binario `Lib/System.Data.SQLite.dll` es un ensamblado **mixto (C++/CLI) de .NET Framework 4.0 Client Profile**. Los ensamblados mixtos de .NET Framework no son cargables por el runtime .NET Core/.NET 5+/.NET 10, por lo que **este binario no puede utilizarse en .NET 10**.

### 3.6 Alternativas identificadas

1. **System.Data.SQLite (distribución Core)** — misma familia de proveedor y mismo namespace/API ADO.NET, pero en dos componentes: `System.Data.SQLite.dll` (administrado, compatible con .NET Standard 2.0) + `SQLite.Interop.dll` (nativo, x86/x64). No es un cambio de proveedor, sino de distribución/despliegue.
2. **Microsoft.Data.Sqlite** — proveedor activo de Microsoft (versión actual para la línea 10.x), con soporte pleno en .NET 10; requiere cambio de namespace/tipos (`SQLite*` → `Sqlite*`) en el código.

### 3.7 Riesgos que deberán validarse (Fase 3 + regresión)

Diferencias de comportamiento entre el proveedor actual y la alternativa elegida, específicamente en:

- `NULL` y manejo de valores nulos.
- Tipos numéricos.
- Fechas (formato de almacenamiento y lectura).
- Texto y conversiones.
- Inserciones, actualizaciones, eliminaciones.
- Consultas y parámetros.
- Manejo de conexiones.
- Transacciones (si se incorporan en la alternativa elegida).
- Compatibilidad con la base de datos `ordenes.db` existente.

### 3.8 Conclusión

**BLOQUEANTE** respecto del **binario actual** (`Lib/System.Data.SQLite.dll`, mixto x86, .NET Framework 4.0 Client Profile), pero **no constituye un bloqueo técnico de la migración**: el proveedor `System.Data.SQLite` es mantenible en su distribución Core y existe `Microsoft.Data.Sqlite` como alternativa. La decisión de proveedor queda pendiente para la Fase 3 y no se modifica en esta fase.

---

## 4. Arquitectura

| Atributo | Valor |
|---|---|
| Target framework | **`net40`** (`ServicioTecnico.csproj`) |
| Plataforma | Windows — SDK `Microsoft.NET.Sdk.WindowsDesktop`, `UseWindowsForms=True` |
| Tipo de aplicación | **`WinExe`** |
| Arquitectura | **`PlatformTarget=x86`** |
| Entrada | `ServicioTecnico.My.MyApplication.Main` → `MyProject.Application.Run` (VB `WindowsFormsApplicationBase`, instancia única, visual styles, `ShutdownMode.AfterMainFormCloses`) |
| Otros | `LangVersion=latest`, `AllowUnsafeBlocks=True`, `CheckForOverflowUnderflow=False`, `RootNamespace` vacío, `GenerateAssemblyInfo=False`, icono `Resources/work.ico` |
| Paquete de build | `Microsoft.NETFramework.ReferenceAssemblies` 1.0.3 (solo compilación, no runtime) |

WinForms, `WinExe` y `x86` son plenamente soportados en .NET 10 (`net10.0-windows` + `PlatformTarget=x86`).

**Conclusión: COMPATIBLE.**

---

## 5. Dependencias que requieren verificación

| Dependencia | Estado | Observación |
|---|---|---|
| `System.Windows.Forms.DataVisualization` (charting) | **REQUIERE VERIFICACIÓN** | Utilizada en `ServicioTecnico/frmReportesServicios.cs` (`System.Windows.Forms.DataVisualization.Charting`, 4 controles `Chart`). No viene incluida en WinForms de .NET 10; su resolución queda pendiente para la Fase 3 |
| `Microsoft.VisualBasic` | **REQUIERE VERIFICACIÓN** | APIs utilizadas: `Interaction.MsgBox` / `Interaction.InputBox`, `MsgBoxStyle`, `RuntimeHelpers.GetObjectValue`, `Information.IsDBNull`, `WindowsFormsApplicationBase`, `ProjectData.SetProjectError` / `ClearProjectError`, `Operators`. Todas con equivalente en .NET 10; verificación puntual en Fase 3 |
| Recursos `.resx` | **REQUIERE VERIFICACIÓN** | Formularios con `.resx` en la raíz del proyecto; sin referencias a ensamblados externos detectadas. Verificación en Fase 3 |
| Recursos embebidos | **REQUIERE VERIFICACIÓN** | `Resources/logo.jpg`, `Resources/work.ico` (icono de aplicación) |
| `System.Data` (DataTable/DataAdapter) | COMPATIBLE | Disponible como `System.Data.Common` en .NET 10 |
| `Microsoft.NETFramework.ReferenceAssemblies` | COMPATIBLE | Solo build; se elimina al migrar |
| `tests/ServicioTecnico.Tests` (net40/x86) | — | Host de pruebas actual con 3 casos implementados (FUN-041..043); la matriz de 59 casos (43 funcionales + 16 de regresión) es meta del plan |

---

## 6. Decisiones y estado del gate

| Punto | Clasificación | Estado |
|---|---|---|
| **BarcodeLib** | COMPATIBLE | **Resuelto como compatible** — no bloqueante |
| **Arquitectura** (WinForms / WinExe / x86) | COMPATIBLE | **Resuelta como compatible** |
| **SQLite** | BLOQUEANTE (binario actual) | **Decisión pendiente para Fase 3** — no modificar en esta fase |
| **DataVisualization** | REQUIERE VERIFICACIÓN | **Pendiente de resolución en Fase 3** |

---

## 7. Veredicto

**Fase 0 SUPERADA** para avanzar a la **Fase 1 — Baseline**.

El único punto clasificado como bloqueante (SQLite) es un bloqueo del **binario actual**, no de la migración: existe mantenibilidad del proveedor (`System.Data.SQLite` Core) y alternativa activa (`Microsoft.Data.Sqlite`). Su decisión se difiere a la Fase 3 sin afectar la baseline.

---

## 8. Restricciones para Fase 1

- **No modificar el código de producción.**
- **No cambiar el proveedor SQLite.**
- **No cambiar BarcodeLib.**
- **No migrar a .NET 10 todavía.**
- **Establecer únicamente la baseline de .NET Framework 4.0** (registro de los 59 casos previstos y tiempos de referencia).

---

## 9. Archivos modificados o creados en esta fase

- Creado: `docs/migracion_dotnet10_fase0.md` (este documento).
- No se modificó ningún archivo de la aplicación (`ServicioTecnico.csproj`, código, `Lib/`, `tests/` ni recursos).
