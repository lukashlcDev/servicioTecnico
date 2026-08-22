# Auditoria tecnica de mejoras - ServicioTecnico .NET Framework 4.8.1

Fecha: 2026-08-21  
Version: 1.0.0  
Estado: Fase 4 cerrada - auditoria documental  
Referencia: `docs/baseline_tecnica_net481.md`, `docs/catalogo_mejoras_net481.md`

Esta auditoria verifica las mejoras MEJ-001 a MEJ-013 contra el codigo y la documentacion existente. No implementa soluciones, no define releases y no establece un orden definitivo.

## Criterios y evidencia general

- Plataforma actual: .NET Framework 4.8.1, WinForms, WinExe, x86.
- Persistencia: SQLite `ordenes.db` junto al ejecutable.
- Pruebas oficiales: `FUN-001` a `FUN-043` y `REG-001` a `REG-016`.
- La matriz de pruebas fue sincronizada con la evidencia de Fase 2: `REG-001..REG-016` figuran como PASS.
- La Fase 0 clasifica BarcodeLib como COMPATIBLE/no bloqueante para el estado evaluado; el binario mixto actual de System.Data.SQLite requiere resolucion para .NET 10.

## Auditorias individuales

### MEJ-001 - Formato monetario

1. **Confirmacion del problema:** **CONFIRMADO**. `txtAbono`, `txtPresupuesto` y `txtTotal` muestran valores sin formato consistente; la impresion concatena `$` con texto crudo. La grafica aplica `C2`, pero el grid no.
2. **Componentes afectados:** `frmOrdenServicio`; `frmReportesServicios`; `modConexion`; TextBox monetarios; calculo en `txtAbono_TextChanged` y `txtPresupuesto_TextChanged`; guardado, carga e impresion.
3. **Archivos obligatorios:** `ServicioTecnico/frmOrdenServicio.cs`, `ServicioTecnico/frmOrdenServicio.Designer.cs`, `ServicioTecnico/frmReportesServicios.cs`.
4. **Archivos posibles:** Ninguno adicional previsto; un helper nuevo solo seria una decision posterior.
5. **Impacto funcional:** MEDIO.
6. **Impacto visual:** MEDIO.
7. **Impacto sobre datos:** BAJO. `abono`, `presupuesto` y `total` son `REAL`. La precision de punto flotante es una consideracion tecnica; esta mejora no propone migrar el esquema.
8. **Riesgo tecnico:** MEDIO, por cultura regional, parseo, redondeo y diferencia entre valor ingresado y valor mostrado.
9. **Riesgo de regresion:** MEDIO. Podrian afectarse calculo, guardado, carga y salidas impresas.
10. **Demanda tecnica:** MEDIA.
11. **Deuda relacionada:** Dinero almacenado como `REAL`, `Conversion.Val`, acoplamiento UI/datos y formatos distintos entre grid, grafica e impresion.
12. **Dependencias:** Independiente para presentacion; conviene coordinar con MEJ-004 y MEJ-012.
13. **Conflictos:** No combinar inicialmente con una migracion de esquema ni con el rediseño total de impresion.
14. **Impacto .NET 10:** **NEUTRO**. El formato de presentacion no depende directamente del framework.
15. **Requisitos previos:** Definir formato regional, redondeo, entrada de separadores y comportamiento de edicion.
16. **Pruebas existentes:** `FUN-019`, `FUN-031..FUN-034`, `FUN-037`, `REG-008..REG-011`, `REG-014`.
17. **Pruebas nuevas:** Importes enteros y decimales, separadores, redondeo, carga, grid, grafica, ticket y carta.
18. **Criterio preliminar de aceptacion:** Presentacion consistente en edicion, calculo, carga, reportes e impresion, sin cambiar el esquema ni perder precision definida.

### MEJ-002 - Tipos de equipo

1. **Confirmacion del problema:** **CONFIRMADO**. Los tipos estan hardcodeados como RadioButton y se persisten como texto.
2. **Componentes afectados:** `frmOrdenServicio`, `frmReportesServicios`, `frmBuscaOrden`; `GuardarOrden`, `CargarOrden`, impresion y consultas de reportes.
3. **Archivos obligatorios:** `frmOrdenServicio.cs`, `frmOrdenServicio.Designer.cs`, `frmReportesServicios.cs`.
4. **Archivos posibles:** `frmBuscaOrden.cs`; una tabla/catalogo o migracion de datos, si se decide normalizar.
5. **Impacto funcional:** ALTO.
6. **Impacto visual:** MEDIO.
7. **Impacto sobre datos:** MEDIO. El campo `tipo_equipo` es texto y existen valores con prefijo `Otros:`.
8. **Riesgo tecnico:** ALTO, por datos existentes y valores no reconocidos.
9. **Riesgo de regresion:** ALTO. Puede afectar guardado, carga, impresion, busqueda y reportes.
10. **Demanda tecnica:** ALTA.
11. **Deuda relacionada:** Valores hardcodeados, texto libre, acoplamiento UI/datos y tabla `estados` no utilizada.
12. **Dependencias:** Afecta MEJ-004, MEJ-008 y MEJ-012.
13. **Conflictos:** No combinar inicialmente con cambios de reportes ni con una migracion de datos.
14. **Impacto .NET 10:** **NEUTRO**. El riesgo principal es de datos y reglas de UI.
15. **Requisitos previos:** Catalogo final, control UI y tratamiento de datos existentes.
16. **Pruebas existentes:** `FUN-016`, `FUN-020`, `FUN-021`, `FUN-023`, `FUN-025`, `REG-005`, `REG-006`.
17. **Pruebas nuevas:** Los cuatro tipos, `Otros: xxx`, `Otros: `, valor vacio y valores desconocidos.
18. **Criterio preliminar de aceptacion:** Cada tipo definido se guarda, carga, busca, reporta e imprime correctamente; los valores existentes no se pierden.

**Comportamiento verificado:** `GuardarOrden` guarda `Laptop`, `Impresora`, `PC` o `Otros: ` mas `txtOtros.Text.Trim()`. Si no hay RadioButton seleccionado y `txtOtros` esta vacio, persiste `Otros: `. `CargarOrden` selecciona cada tipo exacto; reconoce `Otros:` y extrae el texto posterior; para cualquier otro valor no selecciona ningun RadioButton y no tiene rama `default`.

### MEJ-003 - Cambio de iconos

1. **Confirmacion del problema:** **CONFIRMADO** como necesidad visual.
2. **Componentes afectados:** `Resources/work.ico`, `Resources/logo.jpg`, imagenes de botones, recursos `.resx` y Designers.
3. **Archivos obligatorios:** Recursos y `.resx` que contienen imagenes utilizadas.
4. **Archivos posibles:** `frmOrdenServicio.Designer.cs` y otros Designers con imagenes.
5. **Impacto funcional:** BAJO.
6. **Impacto visual:** MEDIO.
7. **Impacto sobre datos:** NULO.
8. **Riesgo tecnico:** BAJO.
9. **Riesgo de regresion:** BAJO, salvo recursos faltantes o mal escalados.
10. **Demanda tecnica:** MEDIA.
11. **Deuda relacionada:** Recursos incrustados y ausencia de una estrategia documentada para DPI.
12. **Dependencias:** MEJ-007.
13. **Conflictos:** Separar cambio de recurso y cambio de tamaño.
14. **Impacto .NET 10:** **NEUTRO**; Fase 0 deja recursos `.resx` sujetos a verificacion futura.
15. **Requisitos previos:** Fuente, licencia, formatos, tamaños y alcance.
16. **Pruebas existentes:** `FUN-001`, `FUN-031..FUN-034`, `REG-001`, `REG-008..REG-011`.
17. **Pruebas nuevas:** Carga de cada recurso, DPI, ausencia de recurso y visualizacion en formularios.
18. **Criterio preliminar de aceptacion:** Todos los iconos cargan y son legibles sin alterar acciones ni impresion.

### MEJ-004 - Formato de impresion

1. **Confirmacion del problema:** **CONFIRMADO**. Existen dos layouts manuales con posiciones y anchos calculados en codigo.
2. **Componentes afectados:** `frmOrdenServicio`, `PrintDocument`, `Graphics`, helpers de texto, logo, condiciones, importes y codigos.
3. **Archivos obligatorios:** `frmOrdenServicio.cs`.
4. **Archivos posibles:** Nuevos componentes de impresion, recursos o referencias, segun decision posterior.
5. **Impacto funcional:** ALTO.
6. **Impacto visual:** ALTO.
7. **Impacto sobre datos:** NULO para rediseño visual.
8. **Riesgo tecnico:** ALTO.
9. **Riesgo de regresion:** ALTO.
10. **Demanda tecnica:** ALTA.
11. **Deuda relacionada:** Logica concentrada en el formulario, layout fijo, datos de empresa en controles ocultos y configuracion duplicada.
12. **Dependencias:** MEJ-001, MEJ-002, MEJ-009 y MEJ-010.
13. **Conflictos:** No combinar inicialmente con QR, fuentes o modo oscuro.
14. **Impacto .NET 10:** **DEPENDE** de la solucion. `PrintDocument/Graphics` y cualquier alternativa requieren evaluacion especifica; no hay motores externos validados.
15. **Requisitos previos:** Definir estrategia, formatos ticket/carta, paginacion y manejo de textos largos.
16. **Pruebas existentes:** `FUN-031..FUN-036`, `REG-008..REG-011`, `REG-016`.
17. **Pruebas nuevas:** Papel, impresoras, textos largos, fuentes, logo, condiciones, importes y varias paginas.
18. **Criterio preliminar de aceptacion:** Ticket y carta imprimen contenido completo, legible y coherente con la configuracion, sin regresiones de autoimpresion.

Las alternativas de motores de reportes quedan como **investigacion tecnica posterior**, no como decisiones.

### MEJ-005 - Redimensionamiento de frmOrdenServicio

1. **Confirmacion del problema:** **CONFIRMADO**.
2. **Componentes afectados:** `Panel1`, `frmOrdenServicio_Resize`, `CentrarPanel` y controles internos.
3. **Archivos obligatorios:** `frmOrdenServicio.cs`, `frmOrdenServicio.Designer.cs`.
4. **Archivos posibles:** Ninguno adicional previsto.
5. **Impacto funcional:** MEDIO.
6. **Impacto visual:** ALTO.
7. **Impacto sobre datos:** NULO.
8. **Riesgo tecnico:** MEDIO.
9. **Riesgo de regresion:** MEDIO.
10. **Demanda tecnica:** MEDIA.
11. **Deuda relacionada:** Posiciones absolutas, panel fijo y falta de estrategia sistematica `Anchor`/`Dock`.
12. **Dependencias:** MEJ-006 y MEJ-007.
13. **Conflictos:** No combinar inicialmente con cambios de campos o botones.
14. **Impacto .NET 10:** **NEUTRO**.
15. **Requisitos previos:** Estrategia de layout, tamaño minimo y controles expandibles.
16. **Pruebas existentes:** `FUN-001`, `FUN-015`, `FUN-016`, `FUN-018`, `FUN-020`, `REG-001`, `REG-005`, `REG-006`, `REG-013`.
17. **Pruebas nuevas:** Maximizar/restaurar, resoluciones, DPI, acceso a controles y uso con fotos.
18. **Criterio preliminar de aceptacion:** Todos los controles permanecen visibles, accesibles y funcionales en tamaños soportados.

### MEJ-006 - Dimensiones de campos de texto

1. **Confirmacion del problema:** **CONFIRMADO**. Los tres TextBox son de una linea, aunque la impresion admite texto multilinea.
2. **Componentes afectados:** `txtFalla`, `txtObservaciones`, `txtReparacion`, `GuardarOrden`, `CargarOrden` y helpers de impresion.
3. **Archivos obligatorios:** `frmOrdenServicio.Designer.cs`.
4. **Archivos posibles:** `frmOrdenServicio.cs`.
5. **Impacto funcional:** MEDIO.
6. **Impacto visual:** MEDIO.
7. **Impacto sobre datos:** NULO.
8. **Riesgo tecnico:** BAJO.
9. **Riesgo de regresion:** BAJO.
10. **Demanda tecnica:** BAJA.
11. **Deuda relacionada:** Tamaño fijo, ausencia de multilinea y limites funcionales.
12. **Dependencias:** MEJ-005 y MEJ-004.
13. **Conflictos:** Cambiar simultaneamente layout y campos dificulta aislar regresiones.
14. **Impacto .NET 10:** **NEUTRO**.
15. **Requisitos previos:** Elegir `Multiline` o `RichTextBox`, scroll y alturas.
16. **Pruebas existentes:** `FUN-016`, `FUN-018`, `FUN-020`, `FUN-023`, `FUN-031`, `FUN-032`, `REG-005`, `REG-006`, `REG-013`.
17. **Pruebas nuevas:** Saltos de linea, texto largo, scroll, recarga exacta, caracteres especiales e impresion.
18. **Criterio preliminar de aceptacion:** El texto se edita, guarda, recarga e imprime completo sin perdida.

### MEJ-007 - Tamaño de iconos

1. **Confirmacion del problema:** **CONFIRMADO**. Los botones tienen tamaños heterogeneos y posiciones absolutas.
2. **Componentes afectados:** Botones de accion, guardar, VER1/2/3, Button1, imagenes y alineacion.
3. **Archivos obligatorios:** `frmOrdenServicio.Designer.cs`.
4. **Archivos posibles:** `frmOrdenServicio.cs`, `.resx`.
5. **Impacto funcional:** BAJO.
6. **Impacto visual:** MEDIO.
7. **Impacto sobre datos:** NULO.
8. **Riesgo tecnico:** BAJO.
9. **Riesgo de regresion:** BAJO.
10. **Demanda tecnica:** BAJA.
11. **Deuda relacionada:** Layout absoluto y DPI.
12. **Dependencias:** MEJ-003 y MEJ-005.
13. **Conflictos:** No combinar cambios de recursos, tamaños y tema sin especificacion visual.
14. **Impacto .NET 10:** **NEUTRO**.
15. **Requisitos previos:** Tamaños objetivo, escalado y area clickeable.
16. **Pruebas existentes:** `FUN-001`, `FUN-015`, `FUN-016`, `FUN-021`, `FUN-026`, `REG-001`, `REG-005`.
17. **Pruebas nuevas:** DPI, iconos recortados, foco, teclado, tamaños pequeños y area clickeable.
18. **Criterio preliminar de aceptacion:** Botones visibles, legibles, clickeables y funcionales en los tamaños definidos.

### MEJ-008 - Rediseno de Buscar Orden

1. **Confirmacion del problema:** **PARCIALMENTE CONFIRMADO**. La interfaz es basica, pero ya tiene busqueda parametrizada, Enter, doble clic, seleccion y grid configurable.
2. **Componentes afectados:** `frmBuscaOrden`, criterios, DataGridView, `OrdenSeleccionada` y contrato con `frmOrdenServicio`.
3. **Archivos obligatorios:** `frmBuscaOrden.cs`, `frmBuscaOrden.Designer.cs`.
4. **Archivos posibles:** `frmOrdenServicio.cs`, `.resx` y nuevos componentes para preview/paginacion.
5. **Impacto funcional:** MEDIO.
6. **Impacto visual:** MEDIO.
7. **Impacto sobre datos:** BAJO; MEDIO si se agregan filtros o consultas.
8. **Riesgo tecnico:** MEDIO.
9. **Riesgo de regresion:** MEDIO.
10. **Demanda tecnica:** MEDIA.
11. **Deuda relacionada:** Sin preview, contador ni paginacion; ClientSize fijo. `frmBuscaOrden.Designer.cs` configura `AutoScaleMode = Font`.
12. **Dependencias:** MEJ-002; interaccion posible con MEJ-005 y MEJ-011.
13. **Conflictos:** Preview, filtros y rediseño visual pueden mezclar alcance.
14. **Impacto .NET 10:** **NEUTRO** para UI y consultas existentes; SQLite debera resolverse en migracion futura.
15. **Requisitos previos:** Mantener Enter, doble clic, retorno de ID, SQL parametrizado y mensajes actuales.
16. **Pruebas existentes:** `FUN-021`, `FUN-022`, `REG-006`, `REG-013`; `FUN-010` corresponde a clientes, no a ordenes.
17. **Pruebas nuevas:** Cero/uno/muchos resultados, filtros, preview, paginacion, DPI y valores legacy.
18. **Criterio preliminar de aceptacion:** Las busquedas actuales mantienen resultados y contrato; las funciones nuevas no alteran la carga de la orden seleccionada.

### MEJ-009 - Codigo de barras / QR

1. **Confirmacion del problema:** **CONFIRMADO**. Actualmente se generan CODE128 y CODE39; no existe QR.
2. **Componentes afectados:** BarcodeLib, generacion Bitmap, impresion ticket/carta y validacion de DLL.
3. **Archivos obligatorios:** `frmOrdenServicio.cs`, `Lib/BarcodeLib.dll`.
4. **Archivos posibles:** `.csproj`, `.resx`, nueva DLL o wrapper.
5. **Impacto funcional:** ALTO.
6. **Impacto visual:** MEDIO.
7. **Impacto sobre datos:** NULO si se mantiene `id_orden` como contenido.
8. **Riesgo tecnico:** ALTO.
9. **Riesgo de regresion:** ALTO.
10. **Demanda tecnica:** MEDIA.
11. **Deuda relacionada:** Dependencia local y validacion por archivo.
12. **Dependencias:** MEJ-004.
13. **Conflictos:** No mezclar reemplazo de dependencia con migracion .NET 10 sin separar pruebas.
14. **Impacto .NET 10:** **DEPENDE**. Fase 0 clasifica BarcodeLib como COMPATIBLE/no bloqueante para el estado evaluado; la validacion definitiva en .NET 10 queda para la migracion y regresion futura.
15. **Requisitos previos:** Simbologia, contenido, tamaño, contraste, convivencia, proveedor y licencia.
16. **Pruebas existentes:** `FUN-031..FUN-036`, `REG-008..REG-011`.
17. **Pruebas nuevas:** Escaneo real, tamaños, contraste, impresoras, datos invalidos y ausencia de DLL.
18. **Criterio preliminar de aceptacion:** El codigo se genera, imprime y escanea correctamente en los formatos definidos sin regresiones.

QRCoder, ZXing.Net u otras bibliotecas quedan como alternativas a investigar, no como decisiones.

### MEJ-010 - Fuentes nativas

1. **Confirmacion del problema:** **CONFIRMADO**. `frmConfiguracion` solo lista fuentes monoespaciadas.
2. **Componentes afectados:** `FontFamily.Families`, `frmConfiguracion`, `fuente_ticket` y construccion de `Font` en impresion.
3. **Archivos obligatorios:** `frmConfiguracion.cs`, `frmOrdenServicio.cs`.
4. **Archivos posibles:** `.resx` y Designer si se agrega preview.
5. **Impacto funcional:** MEDIO sobre impresion.
6. **Impacto visual:** MEDIO.
7. **Impacto sobre datos:** BAJO; usa clave existente.
8. **Riesgo tecnico:** MEDIO.
9. **Riesgo de regresion:** MEDIO.
10. **Demanda tecnica:** MEDIA.
11. **Deuda relacionada:** Cadena `Familia,Tamanio`, `Conversion.Val`, fallback y layouts por ancho.
12. **Dependencias:** MEJ-004 y MEJ-001.
13. **Conflictos:** Las fuentes proporcionales pueden invalidar supuestos de ancho fijo.
14. **Impacto .NET 10:** **NEUTRO**; la persistencia y la enumeracion deberan validarse en la migracion.
15. **Requisitos previos:** Fuentes permitidas, validacion, fallback y preview.
16. **Pruebas existentes:** `FUN-003`, `FUN-005`, `FUN-006`, `REG-007..REG-009`, `REG-016`.
17. **Pruebas nuevas:** Fuente desinstalada, tamaño invalido, proporcional, valor corrupto y textos largos.
18. **Criterio preliminar de aceptacion:** La fuente elegida persiste, se aplica a ambas rutas y no pierde contenido.

### MEJ-011 - Modo oscuro

1. **Confirmacion del problema:** **CONFIRMADO**. No existe estrategia global de tema.
2. **Componentes afectados:** Los 8 formularios, controles, DataGridView, Charting, recursos e imagenes.
3. **Archivos obligatorios:** Formularios y Designers afectados.
4. **Archivos posibles:** Gestor de tema y configuracion.
5. **Impacto funcional:** MEDIO por alcance transversal.
6. **Impacto visual:** ALTO.
7. **Impacto sobre datos:** BAJO; posible preferencia en `configuracion`.
8. **Riesgo tecnico:** ALTO.
9. **Riesgo de regresion:** ALTO.
10. **Demanda tecnica:** ALTA.
11. **Deuda relacionada:** Colores hardcodeados, ausencia de tema central y graficos sin tema.
12. **Dependencias:** MEJ-003, MEJ-005 y MEJ-007.
13. **Conflictos:** No combinar con otros cambios visuales amplios.
14. **Impacto .NET 10:** **NEUTRO**. Los recursos y controles requeriran validacion futura.
15. **Requisitos previos:** Paleta, contraste, foco, seleccion, graficos, imagenes y persistencia.
16. **Pruebas existentes:** Evaluar los 59 casos: `FUN-001..FUN-043` y `REG-001..REG-016`, con prioridad en `REG-001`, `REG-002`, `REG-006`, `REG-007`, `REG-012` y `REG-016`.
17. **Pruebas nuevas:** Todos los formularios, grid, graficos, foco, DPI, recursos e impresion sin cambios.
18. **Criterio preliminar de aceptacion:** Tema coherente y legible, sin alterar datos, funcionalidad ni salida impresa.

### MEJ-012 - Reportes

1. **Confirmacion del problema:** **CONFIRMADO**. No existe indicador de “ingresos adeudados” ni impresion de reportes; si existen filtros.
2. **Componentes afectados:** `frmReportesServicios`, grid, graficos, filtros, importes y consultas.
3. **Archivos obligatorios:** `frmReportesServicios.cs`, `frmReportesServicios.Designer.cs`.
4. **Archivos posibles:** `frmOrdenServicio.cs` o renderer nuevo si se reutiliza impresion.
5. **Impacto funcional:** ALTO por la regla financiera pendiente.
6. **Impacto visual:** MEDIO.
7. **Impacto sobre datos:** MEDIO si se persiste informacion; BAJO si se calcula con datos existentes.
8. **Riesgo tecnico:** ALTO mientras no exista definicion de negocio.
9. **Riesgo de regresion:** MEDIO.
10. **Demanda tecnica:** MEDIA.
11. **Deuda relacionada:** Fechas `TEXT`, dinero `REAL`, `LIMIT 100`, filtros ocultos y consultas duplicadas.
12. **Dependencias:** MEJ-001 y MEJ-004.
13. **Conflictos:** No implementar calculo antes de definir la regla.
14. **Impacto .NET 10:** **DEPENDE**. Charting requiere verificacion futura y SQLite actual requiere resolucion para .NET 10.
15. **Requisitos previos:** Definir “adeudado”, filtros, limite, columnas y formato de impresion.
16. **Pruebas existentes:** `FUN-037..FUN-040`, `REG-012`, `REG-014`.
17. **Pruebas nuevas:** Abono cero/parcial/excedente, estados, filtros, mas de 100 filas, grid/graficos e impresion.
18. **Criterio preliminar de aceptacion:** El indicador aplica una regla aprobada, respeta filtros y coincide con la impresion.

No se asume que `ENTREGADO` determine por si solo la existencia de deuda.

### MEJ-013 - Exportar/importar base de datos

1. **Confirmacion del problema:** **CONFIRMADO**. No existe backup, exportacion ni restauracion.
2. **Componentes afectados:** `ordenes.db`, `imagenes/`, rutas relativas, `modConexion` y restauracion.
3. **Archivos obligatorios:** `modConexion.cs` y `frmConfiguracion.cs` o formulario nuevo.
4. **Archivos posibles:** `frmOrdenServicio.cs`, `MyProject.cs` si se registra un formulario nuevo.
5. **Archivos nuevos probables:** Servicio de backup/restore y dialogos.
6. **Impacto funcional:** ALTO.
7. **Impacto visual:** BAJO.
8. **Impacto sobre datos:** ALTO.
9. **Riesgo tecnico:** CRITICO sin estrategia de consistencia.
10. **Riesgo de regresion:** ALTO.
11. **Demanda tecnica:** ALTA.
12. **Deuda relacionada:** BD local, ausencia de transacciones explicitas, conexiones mixtas y rutas relativas de imagenes.
13. **Dependencias:** SQLite, sistema de archivos y permisos.
14. **Conflictos:** No copiar directamente `ordenes.db` durante operaciones activas; no mezclar con cambio de proveedor SQLite.
15. **Impacto .NET 10:** **POSITIVO**. Facilita preservar y trasladar datos, pero no resuelve el proveedor futuro.
16. **Requisitos previos:** Backup consistente, inclusion de `imagenes/`, integridad, version, destino, confirmacion y restauracion reversible.
17. **Pruebas existentes:** `FUN-001`, `FUN-018`, `FUN-020`, `FUN-026`, `FUN-041..FUN-043`, `REG-001`, `REG-006`, `REG-013`.
18. **Pruebas nuevas:** App cerrada, app en uso, integridad, restauracion, imagenes faltantes, version incompatible, cancelacion, espacio insuficiente y sobrescritura.
19. **Criterio preliminar de aceptacion:** Backup y restauracion reproducen BD, configuracion, ordenes e imagenes sin perder la fuente original.

## A. Matriz de auditoria

| ID | Problema | Funcional | Visual | Datos | Riesgo tecnico | Regresion | Demanda | .NET 10 |
|---|---|---|---|---|---|---|---|---|
| MEJ-001 | CONFIRMADO | MEDIO | MEDIO | BAJO | MEDIO | MEDIO | MEDIA | NEUTRO |
| MEJ-002 | CONFIRMADO | ALTO | MEDIO | MEDIO | ALTO | ALTO | ALTA | NEUTRO |
| MEJ-003 | CONFIRMADO | BAJO | MEDIO | NULO | BAJO | BAJO | MEDIA | NEUTRO |
| MEJ-004 | CONFIRMADO | ALTO | ALTO | NULO | ALTO | ALTO | ALTA | DEPENDE |
| MEJ-005 | CONFIRMADO | MEDIO | ALTO | NULO | MEDIO | MEDIO | MEDIA | NEUTRO |
| MEJ-006 | CONFIRMADO | MEDIO | MEDIO | NULO | BAJO | BAJO | BAJA | NEUTRO |
| MEJ-007 | CONFIRMADO | BAJO | MEDIO | NULO | BAJO | BAJO | BAJA | NEUTRO |
| MEJ-008 | PARCIALMENTE CONFIRMADO | MEDIO | MEDIO | BAJO | MEDIO | MEDIO | MEDIA | NEUTRO |
| MEJ-009 | CONFIRMADO | ALTO | MEDIO | NULO | ALTO | ALTO | MEDIA | DEPENDE |
| MEJ-010 | CONFIRMADO | MEDIO | MEDIO | BAJO | MEDIO | MEDIO | MEDIA | NEUTRO |
| MEJ-011 | CONFIRMADO | MEDIO | ALTO | BAJO | ALTO | ALTO | ALTA | NEUTRO |
| MEJ-012 | CONFIRMADO | ALTO | MEDIO | MEDIO | ALTO | MEDIO | MEDIA | DEPENDE |
| MEJ-013 | CONFIRMADO | ALTO | BAJO | ALTO | CRITICO | ALTO | ALTA | POSITIVO |

## B. Mapa real de dependencias

- **Tecnicas:** MEJ-009 depende de resolver la estrategia de BarcodeLib; MEJ-013 depende de una estrategia consistente para SQLite en uso.
- **Funcionales:** MEJ-002 afecta MEJ-004, MEJ-008 y MEJ-012.
- **Presentacion:** MEJ-001 afecta MEJ-004 y MEJ-012; MEJ-010 afecta MEJ-004.
- **Layout:** MEJ-005 condiciona MEJ-006 y MEJ-007.
- **Conveniencia:** MEJ-003 conviene definirla antes de MEJ-007.
- **Conflictos:** MEJ-004 con MEJ-009/010/011; MEJ-002 con MEJ-012; MEJ-005 con MEJ-006/007.

Las relaciones anteriores no constituyen un orden definitivo.

## C. Archivos con mayor concentracion de cambios

**`ServicioTecnico/frmOrdenServicio.cs`:** MEJ-001, MEJ-002, MEJ-004, MEJ-005, MEJ-006, MEJ-009, MEJ-010 y potencialmente MEJ-013.

**`ServicioTecnico/frmOrdenServicio.Designer.cs`:** MEJ-001, MEJ-002, MEJ-003, MEJ-005, MEJ-006, MEJ-007 y MEJ-011.

**`ServicioTecnico/frmReportesServicios.cs`:** MEJ-001, MEJ-002, MEJ-004 si se reutiliza impresion, MEJ-011 y MEJ-012.

## D. Riesgo acumulado por formulario

- **Muy alto:** `frmOrdenServicio`, por concentrar datos, impresion, imagenes, tipos, moneda, layout y BarcodeLib.
- **Alto:** `frmReportesServicios`, por consultas, fechas, graficos, importes y filtros.
- **Medio:** `frmConfiguracion`, por fuentes, impresora y posibles operaciones de backup/tema.
- **Medio:** `frmBuscaOrden`, por consultas y contrato de seleccion.
- **Bajo:** `frmClientes`, `frmClienteDetalle`, `frmCondicionesServicio` y `frmFoto`, salvo cambios transversales de tema o layout.

## E. Mejoras por nivel de riesgo tecnico

**BAJO:** MEJ-003, MEJ-006, MEJ-007.

**MEDIO:** MEJ-001, MEJ-005, MEJ-008, MEJ-010.

**ALTO:** MEJ-002, MEJ-004, MEJ-009, MEJ-011, MEJ-012.

**CRITICO:** MEJ-013.

## F. Quick wins

- **MEJ-006**, dimensiones de campos de texto.
- **MEJ-007**, tamaño de iconos.

Ambas combinan riesgo y demanda bajos, con beneficio visible y dependencia minima. Esto no decide el orden de implementacion.

## G. Mejoras que requieren decision del propietario

- MEJ-001: formato regional y redondeo.
- MEJ-002: catalogo y tratamiento de valores existentes.
- MEJ-003: identidad, licencia y alcance de iconos.
- MEJ-004: conservar impresion actual o investigar otra arquitectura.
- MEJ-006: `Multiline` o `RichTextBox`.
- MEJ-007: tamaños y estilo visual.
- MEJ-009: conservar, agregar o reemplazar codigos y definir contenido.
- MEJ-011: modo manual/automatico y persistencia.
- MEJ-012: definicion exacta de ingresos adeudados.
- MEJ-013: alcance del backup, consistencia e inclusion de `imagenes/`.

## H. Mejoras que requieren investigacion tecnica adicional

- MEJ-004: motores alternativos de impresion.
- MEJ-009: bibliotecas QR/2D, licencia y compatibilidad futura.
- MEJ-011: tematizacion de controles y Charting.
- MEJ-013: backup SQLite con operaciones activas.
- MEJ-012: correspondencia entre filtros, grid y graficos.

## I. Orden tecnico preliminar

No es un orden definitivo de releases:

1. Definir decisiones funcionales de MEJ-001, MEJ-002, MEJ-012 y MEJ-013.
2. Ejecutar una mejora aislada de bajo riesgo: MEJ-006 o MEJ-007.
3. MEJ-003, separada de la normalizacion de tamaños.
4. MEJ-001, sin cambiar esquema.
5. MEJ-005 y luego MEJ-006 si el layout lo requiere.
6. MEJ-008, preservando el contrato de seleccion.
7. MEJ-010, con pruebas de impresion.
8. MEJ-012, una vez definida la regla de negocio.
9. MEJ-004 como trabajo aislado de alto riesgo.
10. MEJ-009 coordinada con MEJ-004, con pruebas separadas.
11. MEJ-011 como cambio transversal independiente.
12. MEJ-013 despues de definir una estrategia de consistencia, preferentemente antes de cambios de datos mayores.

## J. Posibles bloqueantes

- **MEJ-002:** falta definir tratamiento de datos existentes.
- **MEJ-004:** falta decidir la arquitectura de impresion.
- **MEJ-009:** falta investigar biblioteca, licencia y compatibilidad futura si se reemplaza.
- **MEJ-012:** falta definir la regla de “ingresos adeudados”.
- **MEJ-013:** falta definir consistencia con SQLite en uso e inclusion de `imagenes/`.
- **MEJ-011:** requiere especificacion visual antes de modificar ocho formularios.

## Veredicto general

- 13 mejoras auditadas.
- 12 problemas confirmados.
- 1 problema parcialmente confirmado: MEJ-008.
- Riesgo tecnico: BAJO 3, MEDIO 4, ALTO 5, CRITICO 1.
- Demanda: BAJA 2, MEDIA 7, ALTA 4.
- Quick wins: MEJ-006 y MEJ-007.
- Bloqueantes principales: MEJ-002, MEJ-004, MEJ-009, MEJ-012 y MEJ-013.
- No se inventaron IDs: FUN-001..FUN-043 y REG-001..REG-016.
- No se seleccionaron tecnologias ni reglas de negocio.
- No se definieron releases.
- Fase 4 es exclusivamente documental.
