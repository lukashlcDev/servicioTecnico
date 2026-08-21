# Catalogo formal de mejoras - ServicioTecnico .NET Framework 4.8.1

Fecha: 2026-08-21  
Version: 1.0.0  
Estado: Catalogo documental aprobado para futuras fases de implementacion  
Referencia principal: `docs/baseline_tecnica_net481.md`

Este documento formaliza mejoras candidatas para la rama .NET Framework 4.8.1. Describe hechos comprobados, impacto preliminar y decisiones pendientes. No establece una solucion definitiva ni un orden obligatorio de implementacion.

## Fichas de mejoras

### MEJ-001 - Formato monetario

1. **ID:** MEJ-001
2. **Nombre:** Formato monetario
3. **Problema actual:** `presupuesto`, `abono` y `total` se muestran sin un formato monetario consistente. La interfaz y la impresion usan el texto crudo de los TextBox, mientras que la grafica de ingresos aplica formato `C2`.
4. **Objetivo:** Evaluar y definir una presentacion monetaria consistente en edicion, calculo, reportes e impresion.
5. **Evidencia en codigo:**
   - `modConexion.cs:33` declara `abono REAL`, `presupuesto REAL` y `total REAL`.
   - `frmOrdenServicio.cs:185-187` guarda los valores mediante `Conversion.Val(...)`.
   - `frmOrdenServicio.cs:1098-1100` carga los valores con `.ToString()` sin formato.
   - `frmOrdenServicio.Designer.cs:598-616` define TextBox simples alineados a la derecha.
   - La impresion concatena `"$"` con el texto de los controles.
   - `frmReportesServicios.cs:162,176` usa `LabelFormat = "C2"` para una grafica, pero el grid no aplica el mismo formato.
   - El uso de SQLite `REAL` es una consideracion tecnica por posibles imprecisiones de punto flotante. Esta mejora no propone migrar el esquema.
6. **Formularios afectados:** `frmOrdenServicio`, `frmReportesServicios`.
7. **Archivos afectados:** `ServicioTecnico/frmOrdenServicio.cs`, `ServicioTecnico/frmOrdenServicio.Designer.cs`, `ServicioTecnico/frmReportesServicios.cs`.
8. **Impacto sobre base de datos:** **NO** para el formato de presentacion. Los campos existentes como `REAL` se documentan como consideracion tecnica, sin cambio de esquema en esta mejora.
9. **Dependencias afectadas:** Ninguna.
10. **Alcance:** **MEDIO**.
11. **Riesgo:** **BAJO**.
12. **Esfuerzo preliminar:** **BAJO**.
13. **Riesgo de regresion:** **BAJO**.
14. **Impacto sobre futura migracion a .NET 10:** **NEUTRO**. El formato de presentacion no depende directamente del framework.
15. **Dependencias con otras mejoras:** Puede combinarse con MEJ-012 y con el formato monetario de MEJ-004.
16. **Pruebas necesarias:** Repetir FUN-010, FUN-012, FUN-022 y FUN-023. Agregar pruebas para entrada, calculo, carga, grid e impresion con valores enteros, decimales y separadores.
17. **Decisiones pendientes:** Formato regional exacto; comportamiento de los TextBox durante la edicion; reglas de redondeo y validacion de importes.

### MEJ-002 - Tipos de equipo

1. **ID:** MEJ-002
2. **Nombre:** Tipos de equipo
3. **Problema actual:** Los tipos estan representados por cuatro RadioButton y se guardan como texto. Cambiar el catalogo impactaria guardado, carga, impresion, reportes y datos existentes.
4. **Objetivo:** Evaluar el cambio de tipos de equipo sin perder ni reinterpretar incorrectamente ordenes existentes.
5. **Evidencia en codigo:**
   - `frmOrdenServicio.Designer.cs:690-709` define `Laptop`, `Impresora`, `PC` y `Otro`.
   - `frmOrdenServicio.cs:166` guarda exactamente `Laptop`, `Impresora`, `PC` o `Otros: ` seguido del texto de `txtOtros`.
   - `CargarOrden`, en `frmOrdenServicio.cs:1066-1085`, primero verifica `text.StartsWith("Otros:")`; si coincide, selecciona `rbtnOtros` y asigna a `txtOtros` el texto posterior a la posicion 6, aplicando `Trim()`.
   - Si el texto es exactamente `Laptop`, `Impresora` o `PC`, selecciona el RadioButton correspondiente mediante `switch`.
   - Si el valor no coincide con esos casos ni comienza con `Otros:`, no se selecciona ningun RadioButton. No existe una rama `default`.
   - `frmReportesServicios.cs` agrupa valores que comienzan con `Otros:` como `Otros`.
   - `frmBuscaOrden.cs` muestra `tipo_equipo`, pero no ofrece filtro por tipo.
6. **Formularios afectados:** `frmOrdenServicio`, `frmReportesServicios`, `frmBuscaOrden`.
7. **Archivos afectados:** `ServicioTecnico/frmOrdenServicio.cs`, `ServicioTecnico/frmOrdenServicio.Designer.cs`, `ServicioTecnico/frmReportesServicios.cs`, `ServicioTecnico/frmBuscaOrden.cs`.
8. **Impacto sobre base de datos:** **POSIBLE**. El campo actual es texto; una tabla de catalogo o una normalizacion posterior requeriria decision y migracion de datos.
9. **Dependencias afectadas:** Ninguna externa.
10. **Alcance:** **MEDIO**.
11. **Riesgo:** **MEDIO**.
12. **Esfuerzo preliminar:** **MEDIO**.
13. **Riesgo de regresion:** **MEDIO**, por los valores ya almacenados.
14. **Impacto sobre futura migracion a .NET 10:** **NEUTRO**. El riesgo principal es de datos y reglas de UI, no de framework.
15. **Dependencias con otras mejoras:** MEJ-004 y MEJ-008 utilizan o muestran el tipo de equipo. Puede combinarse con MEJ-012.
16. **Pruebas necesarias:** Repetir pruebas existentes de alta, edicion, carga, busqueda y reimpresion de ordenes, incluyendo FUN-022, FUN-023 y FUN-041..FUN-043. Agregar casos para los cuatro valores, `Otros: xxx`, valores legacy no reconocidos y ordenes sin tipo.
17. **Decisiones pendientes:** Catalogo final; control UI; tratamiento de valores existentes; posibilidad de agregar o eliminar tipos; necesidad de tabla de catalogo.

### MEJ-003 - Cambio de iconos

1. **ID:** MEJ-003
2. **Nombre:** Cambio de iconos
3. **Problema actual:** Los iconos de la aplicacion, botones y recursos visuales estan incrustados en recursos de formularios y requieren una actualizacion visual.
4. **Objetivo:** Evaluar el reemplazo de los iconos sin alterar las acciones asociadas.
5. **Evidencia en codigo:**
   - `Resources/work.ico` es el icono de la aplicacion.
   - `Resources/logo.jpg` se usa como logo de `frmOrdenServicio` y en impresion.
   - `frmOrdenServicio.Designer.cs` carga imagenes mediante `resources.GetObject(...)` en botones como guardar, imprimir, buscar, configuracion, reportes y condiciones.
   - `VER1`, `VER2`, `VER3`, `Button1` y `sinImagen` tambien usan recursos visuales.
6. **Formularios afectados:** Principalmente `frmOrdenServicio`; deben auditarse tambien los formularios con `.resx`.
7. **Archivos afectados:** Recursos `.resx`, `Resources/work.ico`, y eventualmente `Resources/logo.jpg` si se decide reemplazarlo.
8. **Impacto sobre base de datos:** **NO**.
9. **Dependencias afectadas:** Ninguna.
10. **Alcance:** **LOCAL**.
11. **Riesgo:** **BAJO**.
12. **Esfuerzo preliminar:** **MEDIO**.
13. **Riesgo de regresion:** **BAJO**.
14. **Impacto sobre futura migracion a .NET 10:** **NEUTRO**. Los recursos deberan conservarse y validarse durante la migracion.
15. **Dependencias con otras mejoras:** MEJ-007 puede depender del nuevo juego de iconos.
16. **Pruebas necesarias:** Repetir pruebas de interfaz y las pruebas de impresion que incluyan logo. Agregar verificacion de carga de todos los recursos en escalas DPI relevantes.
17. **Decisiones pendientes:** Fuente y licencia de los iconos; alcance del reemplazo; si tambien se reemplaza el logo; formatos y tamanos objetivo.

### MEJ-004 - Formato de impresion

1. **ID:** MEJ-004
2. **Nombre:** Formato de impresion
3. **Problema actual:** La aplicacion utiliza dos layouts manuales con posiciones y anchos calculados en codigo. El formato es rigido y la logica de impresion esta concentrada en `frmOrdenServicio`.
4. **Objetivo:** Evaluar el rediseño del documento impreso sin seleccionar todavia una arquitectura definitiva.
5. **Evidencia en codigo:**
   - `ImprimirTicket()` usa `PrintDocument`, `Graphics` y un ancho de 24 caracteres.
   - `ImprimirCarta()` usa `PrintDocument`, `Graphics` y un ancho de 90 caracteres.
   - Existen helpers de dibujo para lineas, columnas, texto multilinea, alineacion y fondos.
   - La impresion incluye datos de cliente, equipo, falla, observaciones, reparacion, estado, importes, condiciones y codigo.
   - Las rutas de impresion leen impresora y fuente desde `configuracion`.
6. **Formularios afectados:** `frmOrdenServicio`.
7. **Archivos afectados:** `ServicioTecnico/frmOrdenServicio.cs`; eventualmente nuevos archivos de impresion, si se decide separar responsabilidades.
8. **Impacto sobre base de datos:** **NO** para el rediseño visual. Podria ser POSIBLE si se definieran nuevos datos imprimibles, decision que no forma parte de esta ficha.
9. **Dependencias afectadas:** `System.Drawing.Printing`, `System.Drawing` y BarcodeLib si cambia el contenido del codigo.
10. **Alcance:** **TRANSVERSAL**.
11. **Riesgo:** **ALTO**.
12. **Esfuerzo preliminar:** **ALTO**.
13. **Riesgo de regresion:** **ALTO**.
14. **Impacto sobre futura migracion a .NET 10:** **DEPENDE DE LA SOLUCION**. Mantener `PrintDocument/Graphics` y evaluar otra alternativa tienen impactos distintos; ninguna alternativa ha sido validada en esta fase.
15. **Dependencias con otras mejoras:** MEJ-001, MEJ-002, MEJ-009 y MEJ-010.
16. **Pruebas necesarias:** Repetir FUN-022, FUN-023, FUN-036 y las pruebas de regresion relacionadas con impresion. Agregar casos para papel ticket y carta, textos largos, fuentes, importes, logo, condiciones y codigo.
17. **Decisiones pendientes:** Mantener `PrintDocument/Graphics` o evaluar posteriormente un motor de reportes; estructura final ticket/carta; paginacion; manejo de textos largos. Las alternativas tecnologicas requieren investigacion tecnica posterior y no se presentan como soluciones validadas.

### MEJ-005 - Redimensionamiento de frmOrdenServicio

1. **ID:** MEJ-005
2. **Nombre:** Redimensionamiento de `frmOrdenServicio`
3. **Problema actual:** Al maximizar la ventana, el contenido no se expande correctamente.
4. **Objetivo:** Evaluar una estructura de layout que conserve la usabilidad en diferentes tamanos de ventana.
5. **Evidencia en codigo:**
   - `Panel1` tiene tamano fijo `1009x543` y posicion fija inicial.
   - `CentrarPanel()` solo recalcula la posicion del panel durante `Resize`.
   - Los controles internos tienen posiciones y tamanos absolutos; no se observa configuracion sistematica de `Anchor` o `Dock`.
   - El formulario declara `ClientSize = 1030x561`.
6. **Formularios afectados:** `frmOrdenServicio`.
7. **Archivos afectados:** `ServicioTecnico/frmOrdenServicio.cs`, `ServicioTecnico/frmOrdenServicio.Designer.cs`.
8. **Impacto sobre base de datos:** **NO**.
9. **Dependencias afectadas:** Ninguna.
10. **Alcance:** **MEDIO**.
11. **Riesgo:** **MEDIO**.
12. **Esfuerzo preliminar:** **MEDIO**.
13. **Riesgo de regresion:** **MEDIO**.
14. **Impacto sobre futura migracion a .NET 10:** **NEUTRO**.
15. **Dependencias con otras mejoras:** MEJ-006 y MEJ-007 afectan el espacio y la distribucion visual.
16. **Pruebas necesarias:** Repetir pruebas de interfaz y ordenes. Agregar pruebas en tamano inicial, maximizado, resoluciones distintas y escalado DPI.
17. **Decisiones pendientes:** Mantener o eliminar el panel centrado; usar `Anchor`, `Dock` u otro layout; definir controles expandibles y controles con tamano fijo.

### MEJ-006 - Dimensiones de campos de texto

1. **ID:** MEJ-006
2. **Nombre:** Dimensiones de campos de texto
3. **Problema actual:** `txtFalla`, `txtObservaciones` y `txtReparacion` son TextBox de una sola linea, pese a que la impresion los trata como texto multilinea.
4. **Objetivo:** Evaluar tamano, multilinea, scroll y distribucion de los campos descriptivos.
5. **Evidencia en codigo:**
   - `frmOrdenServicio.Designer.cs:581-592` define los tres controles con aproximadamente `869/870x20`.
   - No se configura `Multiline` para esos controles.
   - La impresion usa `DibujarTextoMultilinea` para esos mismos valores.
   - Los campos se guardan en columnas TEXT sin limite de longitud definido en el esquema.
6. **Formularios afectados:** `frmOrdenServicio`.
7. **Archivos afectados:** `ServicioTecnico/frmOrdenServicio.Designer.cs`, eventualmente `ServicioTecnico/frmOrdenServicio.cs`.
8. **Impacto sobre base de datos:** **NO**.
9. **Dependencias afectadas:** Ninguna.
10. **Alcance:** **LOCAL**.
11. **Riesgo:** **BAJO**.
12. **Esfuerzo preliminar:** **BAJO**.
13. **Riesgo de regresion:** **BAJO**.
14. **Impacto sobre futura migracion a .NET 10:** **NEUTRO**.
15. **Dependencias con otras mejoras:** MEJ-005 condiciona el espacio disponible.
16. **Pruebas necesarias:** Repetir pruebas de alta, edicion y carga. Agregar casos con varias lineas, texto largo, scroll, guardado, recarga e impresion.
17. **Decisiones pendientes:** `Multiline` o `RichTextBox`; scroll vertical; tamanos minimos; limites funcionales de texto.

### MEJ-007 - Tamano de iconos

1. **ID:** MEJ-007
2. **Nombre:** Tamano de iconos
3. **Problema actual:** Los botones tienen tamanos y proporciones diferentes: acciones de `146x39`, guardar de `234x79`, botones VER de `25x34` y buscar por numero de `32x23`.
4. **Objetivo:** Evaluar consistencia visual, area clickeable y escalado de iconos.
5. **Evidencia en codigo:** Los tamanos estan fijados en `frmOrdenServicio.Designer.cs` para `btnGuardar`, botones de accion, `VER1/2/3`, `Button1` y `btnSeleccionarImagen`.
6. **Formularios afectados:** Principalmente `frmOrdenServicio`.
7. **Archivos afectados:** `ServicioTecnico/frmOrdenServicio.Designer.cs` y recursos si cambia la escala de imagen.
8. **Impacto sobre base de datos:** **NO**.
9. **Dependencias afectadas:** Ninguna.
10. **Alcance:** **LOCAL**.
11. **Riesgo:** **BAJO**.
12. **Esfuerzo preliminar:** **BAJO**.
13. **Riesgo de regresion:** **BAJO**.
14. **Impacto sobre futura migracion a .NET 10:** **NEUTRO**.
15. **Dependencias con otras mejoras:** MEJ-003 y MEJ-005.
16. **Pruebas necesarias:** Repetir prueba de interfaz. Agregar verificaciones de visibilidad, area clickeable, iconos recortados y comportamiento en diferentes DPI.
17. **Decisiones pendientes:** Tamano objetivo de cada familia de botones; mantener imagen mas texto; estrategia para iconos de alta densidad.

### MEJ-008 - Rediseno de Buscar Orden

1. **ID:** MEJ-008
2. **Nombre:** Rediseno de Buscar Orden
3. **Problema actual:** `frmBuscaOrden` ofrece busqueda por documento, telefono o nombre, pero tiene una interfaz basica y no incorpora preview, contador de resultados ni filtros adicionales.
4. **Objetivo:** Evaluar mejoras visuales y funcionales de distribucion, grid, criterios y experiencia de uso.
5. **Evidencia en codigo:**
   - `frmBuscaOrden.cs` ejecuta una consulta parametrizada con `LIKE` y tres criterios seleccionables.
   - La busqueda se dispara con boton o Enter; un criterio vacio muestra advertencia.
   - El grid usa `AutoSizeColumnsMode.Fill`, seleccion de fila completa, solo lectura y nueve columnas.
   - No se implementa preview de la orden, contador de resultados, paginacion ni filtro por tipo/estado.
   - `ClientSize` es `853x376` y el formulario no tiene `AutoScaleMode` configurado explicitamente.
6. **Formularios afectados:** `frmBuscaOrden`; potencialmente `frmOrdenServicio` por el contrato de seleccion.
7. **Archivos afectados:** `ServicioTecnico/frmBuscaOrden.cs`, `ServicioTecnico/frmBuscaOrden.Designer.cs`.
8. **Impacto sobre base de datos:** **NO** para cambios visuales. **POSIBLE** si se agregan filtros o consultas nuevas.
9. **Dependencias afectadas:** Ninguna externa.
10. **Alcance:** **MEDIO**.
11. **Riesgo:** **BAJO**.
12. **Esfuerzo preliminar:** **MEDIO**.
13. **Riesgo de regresion:** **BAJO**.
14. **Impacto sobre futura migracion a .NET 10:** **NEUTRO**.
15. **Dependencias con otras mejoras:** MEJ-002 si cambia la representacion de tipos de equipo.
16. **Pruebas necesarias:** Repetir las pruebas existentes de busqueda por documento, telefono y nombre. Agregar casos de resultados vacios, muchos resultados, seleccion, doble clic, filtros nuevos y escalado.
17. **Decisiones pendientes:** Busqueda en vivo o boton/Enter; preview; columnas adicionales; filtros por estado o tipo; paginacion.

### MEJ-009 - Codigo de barras / QR

1. **ID:** MEJ-009
2. **Nombre:** Codigo de barras / QR
3. **Problema actual:** La aplicacion utiliza `BarcodeLib` 1.0.0.21 para CODE128 y CODE39. Se evalua si conservarlo, agregar un codigo 2D o reemplazarlo.
4. **Objetivo:** Resolver el futuro del identificador impreso y, potencialmente, anticipar el punto de dependencia de la migracion a .NET 10.
5. **Evidencia en codigo y documentacion:**
   - `Lib/BarcodeLib.dll` version 1.0.0.21 se referencia desde `frmOrdenServicio.cs`.
   - `CheckRequiredFiles()` verifica la existencia de la DLL junto al ejecutable.
   - La impresion utiliza funciones para CODE128 y CODE39 en ticket y carta.
   - La Fase 0 clasifico BarcodeLib como **COMPATIBLE / no bloqueante** para el estado evaluado.
   - La Fase 2 valido la generacion de CODE128 y CODE39 sin excepcion en .NET Framework 4.8.1.
   - No existe generacion de QR actualmente.
   - La validacion definitiva para .NET 10 corresponde a la migracion y regresion futura; BarcodeLib representa una dependencia que debera resolverse en ese contexto.
6. **Formularios afectados:** `frmOrdenServicio`.
7. **Archivos afectados:** `ServicioTecnico/frmOrdenServicio.cs`, recursos de impresion y, si correspondiera, `Lib/` y referencias del proyecto en una fase futura.
8. **Impacto sobre base de datos:** **NO** para cambiar la representacion impresa del numero de orden.
9. **Dependencias afectadas:** `BarcodeLib`; cualquier reemplazo requeriria investigacion tecnica y de licencia.
10. **Alcance:** **MEDIO**.
11. **Riesgo:** **MEDIO**.
12. **Esfuerzo preliminar:** **MEDIO**.
13. **Riesgo de regresion:** **MEDIO**, por el impacto en impresion y escaneo.
14. **Impacto sobre futura migracion a .NET 10:** **DEPENDE DE LA SOLUCION**. MEJ-009 podria resolver anticipadamente una dependencia, pero la compatibilidad debe validarse durante una evaluacion especifica.
15. **Dependencias con otras mejoras:** MEJ-004.
16. **Pruebas necesarias:** Repetir FUN-022, FUN-023 y FUN-036. Agregar casos de lectura/escaneo, tamano, contraste, impresoras termicas y datos invalidos. Evaluar regresion de la dependencia durante la migracion futura.
17. **Decisiones pendientes:** Mantener CODE128/CODE39, agregar QR o reemplazar; contenido del QR; estrategia de convivencia; biblioteca a investigar. QRCoder, ZXing.Net u otras son alternativas no seleccionadas.

### MEJ-010 - Fuentes nativas

1. **ID:** MEJ-010
2. **Nombre:** Fuentes nativas
3. **Problema actual:** `frmConfiguracion` solo lista fuentes monoespaciadas y persiste una familia y tamanio para la impresion.
4. **Objetivo:** Evaluar ampliar o mejorar la seleccion de fuentes manteniendo un resultado imprimible.
5. **Evidencia en codigo:**
   - `CargarFuentesMonoespaciadas()` recorre `FontFamily.Families`, comprueba estilo regular y compara el ancho de `W` e `I`.
   - La configuracion se persiste como `Familia,Tamanio` en `fuente_ticket`.
   - `frmOrdenServicio` lee esa cadena y construye una `Font` en las rutas de impresion.
   - La fuente seleccionada afecta la impresion, no la UI general.
6. **Formularios afectados:** `frmConfiguracion`, `frmOrdenServicio`.
7. **Archivos afectados:** `ServicioTecnico/frmConfiguracion.cs`, `ServicioTecnico/frmOrdenServicio.cs`.
8. **Impacto sobre base de datos:** **NO**. La clave `fuente_ticket` ya existe.
9. **Dependencias afectadas:** Ninguna externa.
10. **Alcance:** **LOCAL**.
11. **Riesgo:** **BAJO**.
12. **Esfuerzo preliminar:** **BAJO**.
13. **Riesgo de regresion:** **BAJO**.
14. **Impacto sobre futura migracion a .NET 10:** **NEUTRO**. Debe conservarse la validacion de fuentes y el formato persistido.
15. **Dependencias con otras mejoras:** MEJ-004.
16. **Pruebas necesarias:** Repetir configuracion e impresion. Agregar pruebas con familias instaladas, familias no disponibles, tamanos invalidos, fuentes proporcionales y layout de ticket/carta.
17. **Decisiones pendientes:** Todas las fuentes o solo monoespaciadas; preview; advertencias sobre cambios de layout; validacion de estilos.

### MEJ-011 - Modo oscuro

1. **ID:** MEJ-011
2. **Nombre:** Modo oscuro
3. **Problema actual:** La interfaz utiliza colores claros, colores del sistema y colores fijados individualmente; no existe un tema oscuro.
4. **Objetivo:** Evaluar la viabilidad de un tema oscuro coherente en formularios y controles sin alterar la impresion.
5. **Evidencia en codigo:**
   - `frmOrdenServicio.Designer.cs` usa `Color.FromArgb(224,224,224)`, `LightBlue`, `Teal`, `Maroon` y colores del sistema.
   - Los formularios y DataGridView no tienen una estrategia global de tema.
   - Los graficos usan la configuracion visual por defecto de Charting.
   - Las imagenes y el contenido impreso son rutas independientes que deben evaluarse por separado.
6. **Formularios afectados:** Los 8 formularios WinForms.
7. **Archivos afectados:** Formularios y sus `.Designer.cs`; eventualmente configuracion.
8. **Impacto sobre base de datos:** **POSIBLE** si la preferencia se persiste en la tabla `configuracion`; no es necesario para evaluar la viabilidad.
9. **Dependencias afectadas:** Ninguna externa.
10. **Alcance:** **TRANSVERSAL**.
11. **Riesgo:** **ALTO**.
12. **Esfuerzo preliminar:** **ALTO**.
13. **Riesgo de regresion:** **ALTO**.
14. **Impacto sobre futura migracion a .NET 10:** **NEUTRO** en concepto. La implementacion debera reevaluarse en WinForms .NET 10 sin asumir compatibilidad adicional.
15. **Dependencias con otras mejoras:** Interactua con todas las mejoras visuales, especialmente MEJ-003, MEJ-005 y MEJ-007.
16. **Pruebas necesarias:** La matriz oficial contiene 43 casos FUN + 16 casos REG = 59 casos. Por ser una modificacion transversal, debe evaluarse que parte de la matriz completa de 59 casos se repite. Como minimo: interfaz, ordenes, busqueda, configuracion, reportes, impresion y regresion de recursos. Agregar pruebas de legibilidad, DataGridView, graficos y confirmacion de que la impresion no cambia.
17. **Decisiones pendientes:** Tema global o por formulario; persistencia; activacion manual o automatica; tratamiento de graficos, imagenes y colores del sistema.

### MEJ-012 - Reportes

1. **ID:** MEJ-012
2. **Nombre:** Reportes
3. **Problema actual:** El reporte muestra importes disponibles, pero no tiene un indicador denominado "ingresos adeudados" ni permite imprimir el reporte filtrado.
4. **Objetivo:** Evaluar la incorporacion de un indicador financiero y la impresion del reporte respetando los filtros aplicados.
5. **Evidencia en codigo:**
   - `frmReportesServicios.cs:56` muestra `presupuesto`, `abono`, `total` y `estado_entrega` en el grid.
   - La tabla `ordenes` dispone actualmente de `presupuesto`, `abono`, `total` y `estado_entrega`.
   - La grafica de ingresos utiliza `SUM(presupuesto)`.
   - No existe una columna ni un calculo implementado para "ingresos adeudados".
   - No existe boton de impresion del reporte.
   - Las consultas del grid usan `LIMIT 100`.
6. **Formularios afectados:** `frmReportesServicios`.
7. **Archivos afectados:** `ServicioTecnico/frmReportesServicios.cs`, `ServicioTecnico/frmReportesServicios.Designer.cs`.
8. **Impacto sobre base de datos:** **NO** para usar los datos ya disponibles. Podria ser **POSIBLE** si se decidiera persistir un nuevo dato, decision fuera del alcance actual.
9. **Dependencias afectadas:** Ninguna externa; la impresion podria relacionarse con MEJ-004.
10. **Alcance:** **MEDIO**.
11. **Riesgo:** **BAJO**.
12. **Esfuerzo preliminar:** **MEDIO**.
13. **Riesgo de regresion:** **BAJO**.
14. **Impacto sobre futura migracion a .NET 10:** **NEUTRO**.
15. **Dependencias con otras mejoras:** MEJ-001 para formato monetario y MEJ-004 si se reutiliza o cambia la estrategia de impresion.
16. **Pruebas necesarias:** Repetir las pruebas de reportes existentes. Agregar pruebas de filtros, datos vacios, valores de `presupuesto`, `abono`, `total`, `estado_entrega`, impresion y limite de resultados.
17. **Decisiones pendientes:** Definicion exacta de "ingresos adeudados". Los datos disponibles no determinan por si solos la regla de negocio; no se asume que `ENTREGADO` determine deuda. Tambien queda pendiente el formato de impresion, el boton de impresion y el tratamiento de `LIMIT 100`.

### MEJ-013 - Exportar / importar base de datos

1. **ID:** MEJ-013
2. **Nombre:** Exportar / importar base de datos
3. **Problema actual:** No existe una funcion de backup, exportacion o importacion; `ordenes.db` y `imagenes/` se administran como archivos locales.
4. **Objetivo:** Evaluar una operacion segura de backup/restore y, si corresponde, exportacion/importacion.
5. **Evidencia en codigo:**
   - `modConexion.cs:13` ubica `ordenes.db` en `Application.StartupPath`.
   - `frmOrdenServicio.cs:140-143` crea la carpeta `imagenes/` junto al ejecutable.
   - `imagen1`, `imagen2` e `imagen3` guardan rutas relativas a esos archivos.
   - No existe actualmente una funcion de exportacion, importacion o backup.
   - Una copia del archivo con la aplicacion cerrada o sin operaciones activas puede ser una estrategia simple, pero no se debe asumir que una copia durante operaciones SQLite es siempre segura.
   - Un backup con SQLite en uso debe garantizar consistencia mediante una estrategia especifica antes de implementarse.
6. **Formularios afectados:** `frmConfiguracion` o un formulario nuevo; `modConexion` podria centralizar validaciones.
7. **Archivos afectados:** Potencialmente nuevos archivos y `ServicioTecnico/modConexion.cs`.
8. **Impacto sobre base de datos:** **SI**.
9. **Dependencias afectadas:** `System.Data.SQLite` ya existente; no se selecciona una dependencia nueva.
10. **Alcance:** **MEDIO**.
11. **Riesgo:** **MEDIO**.
12. **Esfuerzo preliminar:** **MEDIO**.
13. **Riesgo de regresion:** **MEDIO**, especialmente por sobrescritura, restauracion parcial o perdida de archivos asociados.
14. **Impacto sobre futura migracion a .NET 10:** **POSITIVO**. Un backup verificable facilita preservar y trasladar datos, sin resolver por si solo la compatibilidad futura.
15. **Dependencias con otras mejoras:** Ninguna obligatoria.
16. **Pruebas necesarias:** Agregar pruebas de backup con aplicacion cerrada, backup con operaciones activas, restauracion, integridad, versiones, errores, sobrescritura y recuperacion de `imagenes/`. Repetir pruebas funcionales de arranque y lectura de ordenes despues de restaurar.
17. **Decisiones pendientes:** Copia de `.db` o formato de exportacion; estrategia para SQLite en uso; incluir o no `imagenes/`; validacion de versiones; destino; proteccion contra sobrescritura; restauracion parcial o atomica.

## A. Matriz comparativa

| ID | Nombre | Alcance | Riesgo | Esfuerzo | Riesgo de regresion | Impacto .NET 10 |
|---|---|---|---|---|---|---|
| MEJ-001 | Formato monetario | MEDIO | BAJO | BAJO | BAJO | NEUTRO |
| MEJ-002 | Tipos de equipo | MEDIO | MEDIO | MEDIO | MEDIO | NEUTRO |
| MEJ-003 | Cambio de iconos | LOCAL | BAJO | MEDIO | BAJO | NEUTRO |
| MEJ-004 | Formato de impresion | TRANSVERSAL | ALTO | ALTO | ALTO | DEPENDE DE LA SOLUCION |
| MEJ-005 | Redimensionamiento de frmOrdenServicio | MEDIO | MEDIO | MEDIO | MEDIO | NEUTRO |
| MEJ-006 | Dimensiones de campos de texto | LOCAL | BAJO | BAJO | BAJO | NEUTRO |
| MEJ-007 | Tamano de iconos | LOCAL | BAJO | BAJO | BAJO | NEUTRO |
| MEJ-008 | Rediseno de Buscar Orden | MEDIO | BAJO | MEDIO | BAJO | NEUTRO |
| MEJ-009 | Codigo de barras / QR | MEDIO | MEDIO | MEDIO | MEDIO | DEPENDE DE LA SOLUCION |
| MEJ-010 | Fuentes nativas | LOCAL | BAJO | BAJO | BAJO | NEUTRO |
| MEJ-011 | Modo oscuro | TRANSVERSAL | ALTO | ALTO | ALTO | NEUTRO |
| MEJ-012 | Reportes | MEDIO | BAJO | MEDIO | BAJO | NEUTRO |
| MEJ-013 | Exportar / importar base de datos | MEDIO | MEDIO | MEDIO | MEDIO | POSITIVO |

## B. Dependencias entre mejoras

```text
MEJ-003 (iconos) --------------------> MEJ-007 (tamano de iconos)
MEJ-005 (redimensionamiento) --------> MEJ-006 (campos de texto)
MEJ-001 (formato monetario) ---------> MEJ-012 (reportes)
MEJ-004 (formato de impresion) ------> MEJ-009 (barras / QR)
MEJ-004 (formato de impresion) ------> MEJ-010 (fuentes)
MEJ-002 (tipos de equipo) -----------> MEJ-008 (Buscar Orden)
```

Agrupamientos naturales, sin imponer orden definitivo:

- **Visual:** MEJ-003, MEJ-005, MEJ-006, MEJ-007 y MEJ-011.
- **Impresion:** MEJ-001, MEJ-004, MEJ-009 y MEJ-010.
- **Funcional:** MEJ-002, MEJ-008 y MEJ-012.
- **Datos/operacion:** MEJ-013.

## C. Clasificacion por tipo

**Visuales:** MEJ-003, MEJ-005, MEJ-006, MEJ-007, MEJ-011.

**Funcionales:** MEJ-002, MEJ-008, MEJ-012.

**Datos:** MEJ-001 y MEJ-013. MEJ-001 es principalmente de presentacion, pero documenta una consideracion sobre datos REAL sin proponer cambio de esquema.

**Infraestructura/dependencias:** MEJ-009 y MEJ-010.

**Mixtas:** MEJ-004.

## D. Posibles conflictos

| Mejoras | Motivo para separarlas |
|---|---|
| MEJ-004 + MEJ-009 | Ambas modifican el contenido y la validacion de impresion; separar facilita aislar problemas de escaneo y layout. |
| MEJ-004 + MEJ-010 | Motor/layout y fuentes pueden afectar simultaneamente posiciones y cortes de texto. |
| MEJ-004 + MEJ-011 | La impresion debe mantenerse independiente del tema visual de la UI. |
| MEJ-002 + MEJ-012 | Ambas modifican la interpretacion o presentacion de datos de ordenes en reportes. |
| MEJ-003 + MEJ-007 | Cambiar recursos y tamanos a la vez dificulta determinar el origen de problemas visuales. |
| MEJ-005 + MEJ-006 | Cambiar layout y dimensiones de campos simultaneamente dificulta validar escalado y texto. |

Estas relaciones son advertencias de aislamiento de regresiones, no un orden de implementacion aprobado.

## E. Preguntas abiertas

1. MEJ-001: Cual es el formato monetario y la regla de redondeo requeridos?
2. MEJ-002: Cual es el catalogo final y como se tratan los valores existentes, incluidos `Otros: xxx` y valores no reconocidos?
3. MEJ-003: Cual es la fuente y licencia de los nuevos iconos? Se reemplaza tambien el logo?
4. MEJ-004: Se mantiene `PrintDocument/Graphics` o se investiga posteriormente un motor de reportes? Ninguna alternativa esta validada en esta fase.
5. MEJ-005: Se usara `Anchor`, `Dock`, un layout contenedor u otra estrategia?
6. MEJ-006: Se usara `Multiline`, `RichTextBox`, scroll y que tamanos?
7. MEJ-009: Se mantiene CODE128/CODE39, se agrega QR o se reemplaza? Que contenido tendra?
8. MEJ-011: El modo oscuro sera manual o automatico? Se persistira la preferencia?
9. MEJ-012: Que regla de negocio define exactamente "ingresos adeudados"? Los campos disponibles no determinan por si solos esa regla.
10. MEJ-013: Como se garantiza consistencia con SQLite en uso y se incluye la carpeta `imagenes/`?

## Estado del catalogo

- Fichas documentadas: 13, desde MEJ-001 hasta MEJ-013.
- Secciones complementarias: 5, desde A hasta E.
- Decisiones funcionales y tecnicas: permanecen abiertas donde corresponde.
- Orden de implementacion: no establecido.
- Tecnologias alternativas: no seleccionadas ni presentadas como compatibles sin evaluacion.
- Alcance: exclusivamente documental; no implica cambios de codigo, datos, dependencias ni configuracion.
