# AGENTS.md

App de escritorio WinForms para gestión de órdenes de servicio de un taller técnico (C# decompilado desde VB.NET, UI e identificadores en español). Target: .NET Framework 4.8.1.

## Build
- Windows-only: `net481`, `WinExe`, `x86`, `Microsoft.NET.Sdk.WindowsDesktop`. **No se puede compilar ni ejecutar en Linux** — se necesita Windows + MSBuild/Visual Studio (`msbuild ServicioTecnico.slnx`). Compilación: `msbuild ServicioTecnico.slnx /restore /p:Configuration=Debug` (se genera en `bin/Debug/net481/`).
- Solución en formato `.slnx` (VS 2022 17.13+).
- DLLs locales en `Lib/` (`System.Data.SQLite.dll`, `BarcodeLib.dll`) vía `HintPath` + `Private=True`. No usar NuGet; deben copiarse junto al exe.

## Base de datos
- SQLite `ordenes.db`, se crea en `Application.StartupPath` (junto al exe) al primer arranque vía `modConexion.VerificarOCrearBD()` (ServicioTecnico/modConexion.cs). Esquema definido ahí: `clientes`, `ordenes`, `condiciones_servicio`, `configuracion`, `estados`.
- La configuración de la app vive en la tabla `configuracion` (claves: `impresora`, `imprimir_al_guardar`, `fuente_ticket`, `tipo_impresion`). `App.config` está vacío y no se usa.

## Arquitectura
- Punto de entrada: `ServicioTecnico.My.MyApplication.Main`; form principal `frmOrdenServicio`. Uso de `My.Forms` en `ServicioTecnico.My/MyProject.cs`.
- Código decompilado de VB.NET a C# estilo ILSpy: `Microsoft.VisualBasic.CompilerServices` (`Operators.CompareString`, `RuntimeHelpers.GetObjectValue`, `Interaction.MsgBox`), atributos `[StandardModule]`, `[DesignerGenerated]`, `[AccessedThroughProperty]`, backing fields `_Xxx` + propiedad. El código nuevo usa namespaces de scope de archivo y C#9. Mantener el estilo del archivo que se edite.
- Impresión manual con `PrintDocument`/`Graphics` y códigos de barras con `BarcodeLib` (frmOrdenServicio.cs). Reportes con gráficas `DataVisualization.Charting` (frmReportesServicios.cs).

## Gotchas
- Si agregas un formulario accesible vía `My.Forms.`, hay que registrarlo a mano en `ServicioTecnico.My/MyProject.cs` (clase `MyForms`); el `.csproj` SDK incluye `.cs` automáticamente.
- La mayoría de `.resx` de formularios están en la raíz del repositorio con formato `ServicioTecnico.frmX.resx` (frmOrdenServicio, frmClientes, frmConfiguracion). Excepción: `frmBuscaOrden.resx` está dentro de `ServicioTecnico/`. Algunos formularios (frmClienteDetalle, frmCondicionesServicio, frmReportesServicios, frmFoto) no tienen `.resx`.
- La app valida al arranque que `BarcodeLib.dll`, `ordenes.db` y `System.Data.SQLite.dll` existan junto al exe y avisa si se ejecuta desde un ZIP.
- Git repo inicializado sin commits; existe `.gitignore` que ignora `bin/`, `obj/`, `.vs/`, `*.suo`, `*.pdb`, `.opencode/node_modules/`, `*.user`, `*.log`, `*.tmp`, `*.temp`.

## Recuperación del Diseñador WinForms

Los formularios provenientes de código VB.NET decompilado mediante ILSpy
pueden no ser compatibles directamente con el Diseñador de Windows Forms
de C#.

Para recuperar el Diseñador de un formulario existente:

- No modificar directamente el código sin realizar primero una auditoría.
- Utilizar la skill `winforms-designer-recovery`.
- Trabajar por fases e hitos.
- Crear backup y registrar hashes antes de transformaciones estructurales.
- Compilar después de cada transformación importante.
- Validar el Diseñador manualmente en Visual Studio.
- Validar funcionalmente el formulario después de recuperar el Diseñador.
- No modificar `.resx`, `.csproj`, otros formularios ni lógica de negocio
  durante el piloto salvo autorización explícita.
- Mantener rollback disponible en cada fase.
- Detener la migración ante errores estructurales no diagnosticados.

### Procedimiento validado

El procedimiento fue validado exitosamente con `frmOrdenServicio`:

1. Auditoría del formulario.
2. Hito 0: creación de `.Designer.cs` y conversión a `partial`.
3. Split entre código principal y código del diseñador.
4. Conversión de propiedades `internal virtual` provenientes de VB.NET
   a campos C# estándar.
5. Migración explícita del cableado de eventos.
6. Eliminación de aliases y variables locales generadas por ILSpy dentro
   de `InitializeComponent()`.
7. Compilación.
8. Validación del Diseñador.
9. Prueba funcional del formulario.
10. Detención antes de modificar otros formularios.

Para futuros formularios, este procedimiento debe utilizarse como referencia,
pero cada formulario debe auditarse individualmente antes de aplicar una
transformación.
