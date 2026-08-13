# SKILL — Testing Funcional y Regresión de ServicioTecnico

## 1. Propósito

Esta skill define el procedimiento para auditar, diseñar y ejecutar pruebas sobre el proyecto ServicioTecnico.

El objetivo principal es verificar que el sistema funcione correctamente desde el punto de vista funcional.

La prioridad es:

1. pruebas funcionales;
2. pruebas de regresión;
3. pruebas de integración;
4. pruebas unitarias cuando aporten valor.

No se debe priorizar la cantidad de tests sino la cobertura de comportamientos relevantes del sistema.

---

## 2. Alcance

La skill puede analizar y probar:

- formularios WinForms;
- lógica de negocio;
- acceso a SQLite;
- persistencia;
- clientes;
- activos;
- órdenes de servicio;
- configuración;
- impresión;
- reportes;
- navegación;
- validaciones;
- estados de órdenes;
- generación de códigos de barras;
- interacción entre módulos;
- regresiones producidas por modificaciones estructurales.

El proyecto es Windows-only y utiliza:

- C#;
- WinForms;
- .NET Framework 4.0;
- SQLite;
- BarcodeLib;
- Visual Studio/MSBuild.

La ejecución de pruebas que requiera interfaz gráfica o Windows debe realizarse en Windows.

---

## 3. Principio fundamental

El objetivo del testing es comprobar el comportamiento real del sistema.

No se debe considerar suficiente:

- que el proyecto compile;
- que no existan errores sintácticos;
- que una clase pueda instanciarse;
- que un método tenga cobertura;
- que un test unitario pase.

Un test exitoso debe aportar evidencia sobre un comportamiento esperado.

Ejemplo:

NO suficiente:

    Test: Crear instancia de frmClientes
    Resultado: OK

Preferido:

    Test:
    Crear cliente → guardar → cerrar → volver a buscar cliente

    Resultado esperado:
    El cliente aparece con los datos almacenados.

---

## 4. Regla de no modificación de producción

La skill NO debe modificar código de producción para conseguir que un test pase.

Si una prueba falla:

1. registrar el fallo;
2. identificar el componente afectado;
3. registrar pasos para reproducirlo;
4. registrar resultado esperado;
5. registrar resultado obtenido;
6. determinar si es:
   - defecto funcional;
   - problema de entorno;
   - problema de datos;
   - problema de test;
   - comportamiento esperado;
7. detener la corrección.

Nunca cambiar código de producción automáticamente para adaptar el sistema al test.

Las correcciones de código requieren autorización independiente.

---

## 5. Modos de ejecución

La skill debe trabajar en fases.

### Fase 0 — Auditoría

Solo lectura.

Objetivo:

- conocer estructura;
- identificar funcionalidades;
- identificar formularios;
- identificar dependencias;
- identificar base de datos;
- identificar puntos críticos;
- determinar qué puede probarse automáticamente;
- determinar qué requiere prueba manual.

No modificar archivos.

### Fase 1 — Plan de pruebas

Crear una matriz de pruebas.

Cada prueba debe tener:

- ID;
- módulo;
- funcionalidad;
- precondiciones;
- datos de entrada;
- pasos;
- resultado esperado;
- resultado obtenido;
- estado;
- evidencia.

Estados permitidos:

- PASS
- FAIL
- BLOCKED
- NOT_RUN
- NOT_APPLICABLE

No inventar resultados.

### Fase 2 — Tests unitarios

Crear tests unitarios únicamente cuando exista lógica aislable.

Priorizar:

- validaciones;
- conversiones;
- cálculos;
- generación de números;
- lógica de estados;
- funciones de acceso a datos;
- funciones auxiliares.

No crear tests unitarios artificiales para:

- getters triviales;
- código generado;
- Designer;
- métodos de infraestructura sin comportamiento propio.

---

## 6. Pruebas funcionales

Las pruebas funcionales son prioritarias.

### 6.1 Clientes

Comprobar como mínimo:

#### Crear cliente

1. abrir módulo Clientes;
2. ingresar datos válidos;
3. guardar;
4. verificar mensaje/resultado;
5. comprobar persistencia;
6. volver a buscar el cliente.

Resultado esperado:

El cliente queda almacenado y puede recuperarse posteriormente.

#### Modificar cliente

1. localizar cliente;
2. modificar datos;
3. guardar;
4. volver a consultar.

Resultado esperado:

Los datos modificados persisten.

#### Buscar cliente

Probar:

- búsqueda existente;
- búsqueda inexistente;
- campos vacíos;
- coincidencias relevantes.

#### Eliminar cliente

Solo ejecutar si la funcionalidad existe.

Comprobar:

- confirmación;
- eliminación;
- comportamiento posterior.

---

## 7. Activos

Si el módulo existe, probar:

- creación;
- asociación con cliente;
- modificación;
- búsqueda;
- persistencia;
- comportamiento ante datos inválidos.

---

## 8. Órdenes de servicio

Este es uno de los módulos críticos.

### Crear orden

Probar:

1. seleccionar cliente;
2. seleccionar activo;
3. ingresar problema;
4. guardar;
5. verificar número de orden;
6. comprobar persistencia.

Resultado esperado:

La orden queda almacenada correctamente y asociada al cliente/activo correspondiente.

### Estados

Verificar los estados definidos por el sistema.

Cuando existan:

- INGRESADO
- DIAGNOSTICO
- PRESUPUESTADO
- APROBADO
- REPARACION
- ESPERA_REPUESTO
- REPARADO
- ENTREGADO
- CANCELADO

Comprobar que:

- puedan establecerse correctamente;
- se almacenen;
- se recuperen;
- se muestren correctamente.

No asumir que todos los cambios de estado son válidos sin analizar primero la lógica existente.

---

## 9. Persistencia SQLite

La persistencia es una parte crítica.

Comprobar:

1. crear registro;
2. guardar;
3. cerrar formulario;
4. volver a abrir;
5. recuperar registro.

No considerar PASS una prueba que solamente verifica el contenido inmediatamente después de guardar.

Debe comprobarse la persistencia real.

---

## 10. Configuración

Probar las claves actualmente utilizadas por el sistema:

- impresora;
- imprimir_al_guardar;
- fuente_ticket;
- tipo_impresion.

Verificar:

- lectura;
- modificación;
- persistencia;
- recuperación posterior.

No introducir nuevas claves durante testing.

---

## 11. Impresión

La impresión es una funcionalidad crítica y debe recibir pruebas específicas.

### Impresión manual

Probar:

#### Ticket

Configurar:

    tipo_impresion = ticket

Ejecutar impresión manual.

Resultado esperado:

La orden utiliza la ruta de impresión de ticket.

#### Carta / Media Carta

Configurar:

    tipo_impresion = carta

Ejecutar impresión manual.

Resultado esperado:

La orden utiliza la ruta correspondiente.

---

## 12. Impresión automática

Probar específicamente:

    imprimir_al_guardar = true

con:

    tipo_impresion = ticket

Resultado esperado:

Guardar la orden provoca impresión en formato ticket.

Luego:

    imprimir_al_guardar = true

con:

    tipo_impresion = carta

Resultado esperado:

Guardar la orden provoca impresión en formato carta.

IMPORTANTE:

La impresión automática debe respetar la misma configuración de tipo de impresión que la impresión manual.

Este caso debe permanecer dentro de la regresión debido a que anteriormente se detectó un defecto donde GuardarOrden() llamaba directamente a ImprimirTicket().

---

## 13. Reportes

Probar:

- apertura del módulo;
- consulta;
- generación;
- filtros si existen;
- datos vacíos;
- datos existentes;
- impresión/exportación si existe.

No considerar PASS únicamente porque el formulario abre.

---

## 14. Código de barras

Si la funcionalidad está presente:

1. generar código;
2. comprobar que no produzca excepción;
3. verificar que el resultado corresponda al dato enviado;
4. probar valores válidos;
5. probar valores límite si corresponde.

No modificar BarcodeLib ni sus DLL.

---

## 15. Validaciones

Probar entradas inválidas:

- campos obligatorios vacíos;
- fechas inválidas;
- valores numéricos inválidos;
- registros inexistentes;
- combinaciones incompatibles.

Registrar exactamente el comportamiento observado.

No asumir que toda entrada inválida debe producir una excepción.

El comportamiento esperado debe derivarse del código o especificación existente.

---

## 16. Navegación WinForms

Comprobar:

- apertura;
- cierre;
- regreso;
- formularios modales;
- botones;
- diálogos;
- selección de registros;
- retorno de información entre formularios.

Especial atención a:

- frmOrdenServicio;
- frmClientes;
- frmBuscaOrden;
- frmConfiguracion;
- frmCondicionesServicio;
- frmReportesServicios.

---

## 17. Pruebas de regresión

Después de cualquier modificación estructural importante ejecutar nuevamente las pruebas críticas.

Suite mínima de regresión:

### REG-001

Inicio de aplicación.

### REG-002

Abrir Clientes.

### REG-003

Crear cliente.

### REG-004

Buscar cliente.

### REG-005

Crear orden.

### REG-006

Consultar orden.

### REG-007

Guardar configuración.

### REG-008

Impresión manual ticket.

### REG-009

Impresión manual carta.

### REG-010

Impresión automática ticket.

### REG-011

Impresión automática carta.

### REG-012

Generación de reporte.

### REG-013

Persistencia SQLite.

---

## 18. Pruebas destructivas

No ejecutar automáticamente:

- eliminación masiva;
- limpieza de base de datos;
- modificación irreversible de datos;
- cambios de esquema;
- eliminación de archivos;
- modificación de configuración de producción.

Para pruebas destructivas se debe utilizar una copia controlada de la base de datos.

---

## 19. Base de datos de pruebas

Cuando sea posible, utilizar una base de datos de prueba.

No destruir la base de datos real del usuario.

Antes de realizar pruebas que modifiquen datos:

1. identificar la base utilizada;
2. determinar si corresponde a producción o prueba;
3. crear copia si es necesario;
4. registrar ubicación de la copia;
5. realizar las pruebas.

No modificar la estructura de la base de datos durante una prueba funcional salvo que la prueba específicamente evalúe una migración.

---

## 20. Evidencia

Cada FAIL debe contener:

    ID:
    Módulo:
    Funcionalidad:
    Precondiciones:
    Pasos:
    Resultado esperado:
    Resultado obtenido:
    Error:
    Archivo/línea involucrado, si puede determinarse:
    Severidad:
    Reproducible:
    Evidencia:

Severidad:

- CRITICAL
- HIGH
- MEDIUM
- LOW

---

## 21. Errores de entorno

No clasificar automáticamente como defecto del programa:

- MSBuild ausente;
- Visual Studio ausente;
- DLL faltante por entorno;
- permisos;
- impresora inexistente;
- base de datos no disponible;
- rutas externas;
- configuración específica de Windows.

Clasificar primero:

    ENVIRONMENT

y documentar la causa.

---

## 22. Compilación

Cuando sea posible en Windows:

    msbuild ServicioTecnico.slnx /restore /p:Configuration=Debug

Resultado esperado:

- 0 errores.

Los warnings no deben ocultarse.

Registrar:

- cantidad de errores;
- cantidad de warnings;
- mensaje relevante;
- proyecto afectado.

Una compilación exitosa NO implica que el testing funcional sea exitoso.

---

## 23. Ejecución del EXE

Después de una compilación exitosa:

1. ejecutar el EXE;
2. comprobar inicio;
3. ejecutar suite funcional;
4. registrar resultados.

No modificar el EXE manualmente.

---

## 24. Tests sobre código generado

No crear ni modificar tests para:

- *.Designer.cs
- archivos generados automáticamente;
- recursos generados;
- infraestructura heredada que no tenga comportamiento funcional.

El comportamiento debe probarse desde el consumidor funcional.

---

## 25. Criterio de PASS

Una funcionalidad es PASS cuando:

1. puede ejecutarse;
2. los datos de entrada son válidos;
3. el resultado coincide con el comportamiento esperado;
4. la persistencia, cuando corresponde, fue comprobada;
5. no se produjo error inesperado.

No marcar PASS por ausencia de excepción únicamente.

---

## 26. Criterio de FAIL

Marcar FAIL cuando:

- el resultado no coincide con lo esperado;
- los datos no persisten;
- una función produce un resultado incorrecto;
- existe una excepción inesperada;
- una configuración es ignorada;
- una funcionalidad previamente existente deja de funcionar.

---

## 27. Criterio de BLOCKED

Usar BLOCKED cuando la prueba no pueda ejecutarse por una dependencia externa.

Ejemplo:

    Prueba: impresión física
    Estado: BLOCKED
    Motivo: no existe impresora disponible en el entorno.

No convertir BLOCKED en PASS.

---

## 28. No inventar resultados

Nunca afirmar:

- PASS;
- FAIL;
- cobertura;
- porcentaje;
- cantidad de pruebas ejecutadas;

si no existe evidencia de ejecución.

Si una prueba no fue ejecutada:

    NOT_RUN

---

## 29. Cobertura

La cobertura de código es secundaria.

No perseguir un porcentaje arbitrario.

Priorizar cobertura funcional:

- operaciones críticas;
- caminos principales;
- casos de error;
- persistencia;
- impresión;
- regresiones.

---

## 30. Orden recomendado de ejecución

Ejecutar en este orden:

1. compilación;
2. inicio;
3. configuración;
4. clientes;
5. activos;
6. órdenes;
7. persistencia;
8. impresión;
9. reportes;
10. regresión.

Si una etapa crítica falla, documentarla y evaluar si tiene sentido continuar.

---

## 31. Informe final

Al finalizar una campaña de testing generar:

    # Informe de Testing

    ## Resumen

    Fecha:
    Versión/build:
    Entorno:
    Resultado general:

    ## Estadísticas

    Total:
    PASS:
    FAIL:
    BLOCKED:
    NOT_RUN:

    ## Pruebas críticas

    ...

    ## Defectos encontrados

    ...

    ## Regresiones

    ...

    ## Problemas de entorno

    ...

    ## Riesgos

    ...

    ## Conclusión

No generar porcentajes si no existe un denominador claramente definido.

---

## 32. Política de cambios

Durante testing:

PERMITIDO:

- crear archivos de tests;
- crear datos de prueba;
- crear copias controladas de bases;
- generar informes;
- generar logs;
- generar resultados de pruebas.

NO PERMITIDO sin autorización:

- modificar código de producción;
- modificar modelos;
- modificar base de datos de producción;
- cambiar configuración de producción;
- eliminar archivos;
- actualizar dependencias;
- cambiar versiones;
- cambiar arquitectura.

---

## 33. Regla para nuevos defectos

Cuando se detecte un defecto:

1. NO corregir automáticamente.
2. Registrar el defecto.
3. Determinar causa probable.
4. Determinar severidad.
5. Proponer corrección mínima.
6. Detenerse.

La corrección del defecto será una tarea independiente.

---

## 34. Prioridad del testing

Prioridad:

1. Funcionalidad crítica del usuario.
2. Persistencia.
3. Impresión.
4. Órdenes de servicio.
5. Clientes.
6. Configuración.
7. Reportes.
8. Casos límite.
9. Unitarios.
10. Cobertura de código.

---

## 35. Principio final

Esta skill existe para responder:

> "¿El sistema hace correctamente lo que debe hacer?"

No simplemente:

> "¿El código compila?"

Un sistema que compila pero no guarda clientes correctamente debe considerarse FAIL.

Un sistema que pasa tests unitarios pero imprime una orden en el formato incorrecto debe considerarse FAIL.

La evidencia funcional tiene prioridad sobre la apariencia del código.

FIN DE SKILL