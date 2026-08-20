# Matriz de Pruebas — ServicioTecnico

## 1. Objetivo del testing

Verificar que el sistema hace correctamente lo que debe hacer desde el punto de
vista funcional: gestión de órdenes de servicio, clientes, configuración,
impresión, condiciones de servicio y reportes. La evidencia funcional tiene
prioridad sobre la apariencia del código o el resultado de una compilación.

Prioridad: 1) funcionalidad crítica, 2) persistencia, 3) impresión,
4) órdenes de servicio, 5) clientes, 6) configuración, 7) reportes,
8) casos límite, 9) unitarios, 10) cobertura.

## 2. Alcance

- Formularios: frmOrdenServicio, frmClientes, frmClienteDetalle,
  frmBuscaOrden, frmConfiguracion, frmCondicionesServicio,
  frmReportesServicios, frmFoto.
- Lógica de negocio y persistencia SQLite (modConexion, Cliente).
- Impresión manual y automática, códigos de barras, reportes.
- No se prueban: *.Designer.cs, código generado, BarcodeLib/DLLs,
  infraestructura heredada sin comportamiento funcional.

## 3. Entorno requerido

- Windows (la app es .NET Framework 4.8.1, x86, WinExe; no compila ni se
  ejecuta en Linux).
- Visual Studio 2022 17.13+ / MSBuild (msbuild ServicioTecnico.slnx
  /restore /p:Configuration=Debug).
- Carpeta de trabajo con: ServicioTecnico.exe, System.Data.SQLite.dll,
  BarcodeLib.dll y una copia controlada de ordenes.db.
- Impresora disponible (requerida por FUN-031 a FUN-034).

## 4. Criterios PASS / FAIL / BLOCKED / NOT_RUN

- PASS: la prueba puede ejecutarse, la entrada es válida, el resultado
  coincide con el esperado, la persistencia fue comprobada cuando
  corresponde, y no hubo error inesperado. No basta la ausencia de excepción.
- FAIL: el resultado no coincide, los datos no persisten, una función
  produce un resultado incorrecto, hay excepción inesperada, una
  configuración es ignorada, o una funcionalidad previamente existente
  dejó de funcionar.
- BLOCKED: la prueba no puede ejecutarse por una dependencia externa
  (ej.: impresora inexistente). No se convierte BLOCKED en PASS.
- NOT_RUN: la prueba aún no fue ejecutada.
- Los fallos se registran con el formato de evidencia de la sección 11.
  No se corrige código de producción automáticamente; la corrección es
  una tarea independiente.

## 5. Matriz funcional

Todas las pruebas comienzan en NOT_RUN.

| ID | Módulo | Funcionalidad | Precondiciones | Resultado esperado | Tipo | Estado |
|---|---|---|---|---|---|---|
| FUN-001 | Arranque | Inicio normal | exe + DLLs + ordenes.db en carpeta de trabajo | La app abre; ordenes.db se abre/crea; no hay errores | Manual | NOT_RUN |
| FUN-002 | Arranque | Archivos requeridos ausentes | Carpeta sin ordenes.db (o sin DLL) | Mensaje de archivos faltantes + salida de la app | Manual | NOT_RUN |
| FUN-003 | Configuración | Cargar valores existentes | Config previamente guardada | Impresora, fuente, tipo_impresion e imprimir_al_guardar reflejados | Manual | NOT_RUN |
| FUN-004 | Configuración | Guardar impresora + persistencia | Impresora instalada | Valor guardado y presente al reabrir el form | Manual | NOT_RUN |
| FUN-005 | Configuración | Cambiar tipo_impresion a ticket/carta | Config guardada | El despacho de impresión posterior usa la ruta correspondiente | Manual | NOT_RUN |
| FUN-006 | Configuración | Cambiar fuente_ticket | Config guardada | La impresión usa la nueva fuente y tamaño | Manual | NOT_RUN |
| FUN-007 | Cliente | Crear cliente válido | frmClienteDetalle abierto | Cliente guardado; recuperable tras cerrar y reabrir | Manual | NOT_RUN |
| FUN-008 | Cliente | Crear sin nombre | frmClienteDetalle nuevo | Mensaje "nombre obligatorio"; no se guarda | Manual | NOT_RUN |
| FUN-009 | Cliente | Editar cliente | Cliente existente | Los cambios persisten | Manual | NOT_RUN |
| FUN-010 | Cliente | Buscar por nombre/doc/tel | Clientes existentes | Coincidencias mostradas en el grid | Manual | NOT_RUN |
| FUN-011 | Cliente | Búsqueda sin resultados | Criterio inexistente | Lista vacía / sin resultados | Manual | NOT_RUN |
| FUN-012 | Cliente | Eliminar sin órdenes | Cliente sin órdenes | Confirmación + eliminación | Manual | NOT_RUN |
| FUN-013 | Cliente | Eliminar con órdenes asociadas | Cliente con órdenes | Bloqueado con mensaje; no se elimina | Manual | NOT_RUN |
| FUN-014 | Cliente | Seleccionar desde orden | frmClientes desde frmOrdenServicio | Rellena nombre/dirección/documento/teléfono | Manual | NOT_RUN |
| FUN-015 | Orden | Nueva orden | frmOrdenServicio | Campos limpios; fecha de hoy; estado inicial | Manual | NOT_RUN |
| FUN-016 | Orden | Guardar nueva | Nombre y falla ingresados | INSERT; número de orden asignado; mensaje de éxito | Manual | NOT_RUN |
| FUN-017 | Orden | Guardar sin nombre/falla | Campos incompletos | Mensaje; no se guarda | Manual | NOT_RUN |
| FUN-018 | Orden | Persistencia de orden | Orden creada | Visible tras cerrar y reabrir la app (buscar/consultar) | Manual | NOT_RUN |
| FUN-019 | Orden | Cálculo total = presupuesto − abono | Presupuesto y abono válidos | Total actualizado al editar ambos campos | Manual | NOT_RUN |
| FUN-020 | Orden | Cargar orden por número | Orden existente | Campos e imágenes recuperados | Manual | NOT_RUN |
| FUN-021 | Orden | Buscar por frmBuscaOrden | Búsqueda por doc/tel/nombre | Orden seleccionada carga en el formulario | Manual | NOT_RUN |
| FUN-022 | Orden | Buscar inexistente | Criterio sin resultados | Mensaje "no se encontraron/encuentra la orden" | Manual | NOT_RUN |
| FUN-023 | Orden | Editar y guardar | Orden existente | UPDATE aplicado; cambios persisten | Manual | NOT_RUN |
| FUN-024 | Orden | Eliminar con confirmación | Orden existente (BD copia) | DELETE + limpieza del formulario | Manual | NOT_RUN |
| FUN-025 | Orden | Tipos de equipo | RadioButton seleccionado | El tipo (Laptop/Impresora/PC/Otros) se guarda y recupera | Manual | NOT_RUN |
| FUN-026 | Orden | Fotos (3) | Imágenes seleccionadas | Se copian a imagenes/ al guardar y se visualizan | Manual | NOT_RUN |
| FUN-027 | Orden | Cliente por documento (Enter) | Documento existente/inexistente | Autocompleta o mensaje "Cliente no registrado" | Manual | NOT_RUN |
| FUN-028 | Orden | Cliente por teléfono (Enter) | Teléfono existente/inexistente | Autocompleta o mensaje de no registrado | Manual | NOT_RUN |
| FUN-029 | Condiciones | Editar y guardar | frmCondicionesServicio | Condiciones persisten y aparecen al pie de impresión | Manual | NOT_RUN |
| FUN-030 | Condiciones | Borrar con confirmación | Condiciones existentes | Confirmación y limpieza del texto | Manual | NOT_RUN |
| FUN-031 | Impresión | Manual ticket — DEPENDE DE IMPRESORA | Impresora disponible; tipo_impresion=ticket | Usa la ruta ImprimirTicket (si no hay impresora → BLOCKED) | Manual | NOT_RUN |
| FUN-032 | Impresión | Manual carta — DEPENDE DE IMPRESORA | Impresora disponible; tipo_impresion=carta | Usa la ruta ImprimirCarta (si no hay impresora → BLOCKED) | Manual | NOT_RUN |
| FUN-033 | Impresión | Auto ticket — CRÍTICA REGRESIÓN (defecto 5G.1) | imprimir_al_guardar=true; tipo_impresion=ticket | Guardar dispara impresión en formato ticket | Manual | NOT_RUN |
| FUN-034 | Impresión | Auto carta — CRÍTICA REGRESIÓN (defecto 5G.1) | imprimir_al_guardar=true; tipo_impresion=carta | Guardar dispara impresión en formato carta | Manual | NOT_RUN |
| FUN-035 | Impresión | Sin orden cargada | txtOrden vacío | Mensaje "No hay una orden cargada para imprimir" | Manual | NOT_RUN |
| FUN-036 | Impresión | Código de barras | Orden cargada | Imagen de código de barras generada sin excepción (N° en 6 dígitos) | Manual | NOT_RUN |
| FUN-037 | Reportes | Apertura y datos | Órdenes existentes | Grid + 3 gráficas con datos | Manual | NOT_RUN |
| FUN-038 | Reportes | Filtros 7 días/mes/30 días/año | Datos en rango | Datos y gráficas filtrados | Manual | NOT_RUN |
| FUN-039 | Reportes | Filtro personalizado inválido | Desde > Hasta | Mensaje "La fecha 'Desde' no puede ser mayor..." | Manual | NOT_RUN |
| FUN-040 | Reportes | Datos vacíos | BD sin órdenes en rango | Sin excepción; gráficas vacías | Manual | NOT_RUN |
| FUN-041 | BD (unit) | Obtener/GuardarConfiguracion | rutaDB temporal/controlada | Round-trip: guardar y recuperar devuelve el valor | AUTO | PASS |
| FUN-042 | BD (unit) | Obtener/GuardarCondicionesServicio | rutaDB temporal/controlada | Round-trip correcto | AUTO | PASS |
| FUN-043 | BD (unit) | VerificarOCrearBD en carpeta vacía | BD temporal/controlada — NUNCA la BD real | Se crean las 5 tablas y las 3 claves de configuración iniciales | AUTO | PASS |

## 6. Matriz de regresión

Suite mínima a ejecutar tras cualquier modificación estructural importante.
Todas comienzan en NOT_RUN.

| ID | Funcionalidad | Precondiciones | Resultado esperado | Nota | Estado |
|---|---|---|---|---|---|
| REG-001 | Inicio de aplicación | Carpeta de trabajo completa | La app abre sin errores | | NOT_RUN |
| REG-002 | Abrir Clientes | App iniciada | frmClientes abre y lista clientes | | NOT_RUN |
| REG-003 | Crear cliente | frmClienteDetalle | Cliente guardado y recuperable | | NOT_RUN |
| REG-004 | Buscar cliente | Clientes existentes | Coincidencias mostradas | | NOT_RUN |
| REG-005 | Crear orden | Cliente y datos válidos | Orden creada con número asignado | Usar BD copia | NOT_RUN |
| REG-006 | Consultar orden | Orden existente | Orden cargada correctamente | | NOT_RUN |
| REG-007 | Guardar configuración | frmConfiguracion | Claves persistidas | | NOT_RUN |
| REG-008 | Impresión manual ticket | Impresora; tipo=ticket | Ruta ImprimirTicket | | NOT_RUN |
| REG-009 | Impresión manual carta | Impresora; tipo=carta | Ruta ImprimirCarta | | NOT_RUN |
| REG-010 | Impresión automática ticket — OBLIGATORIA | imprimir_al_guardar=true; tipo=ticket | Guardar dispara ticket | Regresión del defecto 5G.1 | NOT_RUN |
| REG-011 | Impresión automática carta — OBLIGATORIA | imprimir_al_guardar=true; tipo=carta | Guardar dispara carta | Regresión del defecto 5G.1 | NOT_RUN |
| REG-012 | Generación de reporte | Órdenes existentes | Grid + gráficas generadas | | NOT_RUN |
| REG-013 | Persistencia SQLite | Orden creada | Visible tras cerrar y reabrir la app | | NOT_RUN |
| REG-014 | Cálculo total = presupuesto − abono | Valores válidos | Total correcto | | NOT_RUN |
| REG-015 | Eliminación de cliente con órdenes | Cliente con órdenes | Bloqueada con mensaje | | NOT_RUN |
| REG-016 | Condiciones de servicio en impresión | Condiciones guardadas | Aparecen al pie de la impresión | | NOT_RUN |

## 7. Dependencias y precondiciones

- Sistema: Windows con .NET Framework 4.8.1; MSBuild/VS para compilación.
- Archivos junto al exe: System.Data.SQLite.dll, BarcodeLib.dll, ordenes.db.
- BD: copia controlada para pruebas (FUN-016/018/023/024, REG-005/006/013/015).
- Impresora instalada para FUN-031 a FUN-034 y REG-008 a REG-011.
- Configuración inicial (tabla configuracion): impresora="",
  imprimir_al_guardar="false", fuente_ticket="Courier New, 9";
  tipo_impresion no se inserta en la inicialización; si está vacía o ausente,
  Imprimir() muestra error.
- El arranque exige la presencia de ordenes.db (CheckRequiredFiles).
- Función 5G.1: GuardarOrden() debe despachar por Imprimir() según
  tipo_impresion, no llamar directamente a ImprimirTicket().

## 8. Casos que requieren Windows

FUN-001..FUN-040 (todos los manuales), REG-001..REG-016.
La impresión física (FUN-031..034, REG-008..011) además requiere impresora.

## 9. Casos que pueden automatizarse

Lógica aislable sin UI:
- FUN-041: Obtener/GuardarConfiguracion (round-trip) — modConexion.
- FUN-042: Obtener/GuardarCondicionesServicio (round-trip) — modConexion.
- FUN-043: VerificarOCrearBD/CrearTablas en carpeta vacía.
- Complementario: consultas SQL de CRUD/agregación de reportes sobre BD
  temporal (no son pruebas del código, sino validación de consultas).

El resto requiere UI Windows y se ejecuta manualmente.

## 10. Riesgos identificados en la Fase 0

| # | Riesgo | Ubicación |
|---|---|---|
| R1 | Autoimpresión que ignore tipo_impresion (defecto 5G.1 ya corregido; regresión obligatoria) | frmOrdenServicio.cs:205 |
| R2 | CheckRequiredFiles exige ordenes.db antes de crearla: arranque en carpeta limpia = mensaje + salida | frmOrdenServicio.cs:108-126 |
| R3 | GuardarOrden reutiliza cliente por nombre exacto: posible fusión de duplicados | frmOrdenServicio.cs:148-164 |
| R4 | Fechas en texto dd/MM/yyyy: Convert.ToDateTime sensible a locale; reportes parsean con substr | frmOrdenServicio.cs:1101; frmReportesServicios.cs:216 |
| R5 | Impresión sin impresora/printer inválido: excepción capturada con MsgBox → BLOCKED | frmOrdenServicio.cs:422/614 |
| R6 | Reportes limitados a 100 filas | frmReportesServicios.cs:56 |
| R7 | Tabla `estados` creada pero no usada; estados en texto estado_entrega | modConexion.cs:35 |
| R8 | Licencia: Timer licencia deshabilitado en el Diseñador; funcionalidad inactiva, sin pruebas activas | frmOrdenServicio.Designer.cs:710 |

> Nota: los números de línea referenciados corresponden al estado del código en la Fase 0. Pueden haber cambiado en versiones posteriores.

## 11. Evidencia requerida para cada prueba

- Cada prueba registra: ID, módulo, funcionalidad, precondiciones, pasos,
  resultado esperado, resultado obtenido, estado y evidencia.
- Manual (UI): captura de pantalla o descripción textual del resultado
  observado, más pasos reproducidos. Impresión: ruta tomada (ticket/carta)
  y salida o error de la impresora.
- AUTO (unit): salida del runner, afirmaciones (asserts) y trazabilidad
  de la consulta SQL; round-trips verificados contra la BD temporal.
- FAIL adicional: error, archivo/línea involucrado si puede determinarse,
  severidad (CRITICAL/HIGH/MEDIUM/LOW), reproducible, evidencia.
- Prohibido: afirmar PASS, FAIL o cobertura sin evidencia de ejecución.
  Si no fue ejecutada, se registra NOT_RUN.
