---
name: pre-commit-audit
description: Auditoría técnica final pre-commit del conjunto completo de cambios de una mejora, corrección o fase. Revisa alcance, git diff, código temporal, regresiones, tests y compilación sin modificar archivos, DB, commits ni remoto.
---

# Pre-Commit Audit

## Propósito

Realizar una auditoría técnica final antes de autorizar un commit.

La auditoría debe determinar si el conjunto completo de cambios acumulados de una mejora, corrección o fase está en condiciones de ser versionado.

Esta skill es de SOLO LECTURA.

Su función es detectar problemas y emitir un veredicto.

NO debe corregir los problemas encontrados.

---

# 1. REGLAS INVIOLABLES

Durante la ejecución de esta skill:

- NO modificar código.
- NO modificar archivos.
- NO modificar la base de datos real.
- NO ejecutar formateadores que escriban archivos.
- NO regenerar Designer.
- NO crear archivos permanentes dentro del repositorio.
- NO eliminar archivos.
- NO hacer `git add`.
- NO hacer commit.
- NO hacer push.
- NO hacer merge.
- NO cambiar de rama.
- NO aplicar fixes automáticamente.

Se permiten:

- lectura de archivos;
- búsquedas globales;
- comandos Git de lectura;
- compilación;
- ejecución de tests;
- creación de archivos/DB temporales fuera del repositorio cuando los tests lo requieran.

Si una prueba requiere modificar la DB real, NO ejecutarla.

Informar la limitación.

---

# 2. CONTEXTO DE LA AUDITORÍA

Antes de comenzar, identificar:

- mejora/corrección/fase auditada;
- rama actual;
- objetivo funcional;
- archivos que deberían formar parte del cambio;
- archivos que explícitamente están fuera de alcance;
- pruebas automatizadas esperadas;
- pruebas manuales ya realizadas;
- restricciones específicas indicadas por el usuario.

Las reglas específicas de la mejora auditada deben venir del contexto, documentación o prompt.

NO inventar requisitos faltantes.

La skill define CÓMO auditar.

El contexto de ejecución define QUÉ comportamiento debe existir.

---

# 3. ESTADO DEL REPOSITORIO

Ejecutar como mínimo:

```bash
git branch --show-current
git status --short
git status
git diff --stat
git diff --name-only
git diff
git diff --check
```

También inspeccionar archivos untracked.

IMPORTANTE:

`git diff` no muestra archivos untracked.

Todo archivo untracked relacionado con el cambio debe inspeccionarse explícitamente.

Determinar:

- archivos modificados;
- archivos nuevos;
- archivos eliminados;
- archivos renombrados;
- cambios fuera de alcance;
- archivos temporales;
- backups accidentales;
- artefactos de compilación incorporados accidentalmente.

Comparar el estado real contra el alcance esperado.

---

# 4. TRAZABILIDAD POR ARCHIVO

Para cada archivo modificado o nuevo, informar:

- ruta;
- función dentro del cambio;
- requisito/PASO/incidencia al que pertenece;
- si es necesario para el cambio;
- si contiene modificaciones no relacionadas.

No asumir que un archivo completo pertenece a la mejora solo porque aparece en `git status`.

Inspeccionar su diff real.

---

# 5. CÓDIGO TEMPORAL E INSTRUMENTACIÓN

Buscar globalmente restos potenciales de diagnóstico o desarrollo:

```text
keylog
LogKey
Console.WriteLine
Debug.WriteLine
Trace.WriteLine
Stopwatch
PRUEBA
TEMP
DEBUG
TODO
FIXME
HACK
test-only
```

También buscar:

- métodos de diagnóstico;
- logging temporal;
- handlers temporales;
- archivos de prueba manual;
- harness temporales;
- backups;
- archivos `.bak`;
- copias de formularios;
- dumps;
- logs generados;
- scripts ad-hoc.

Distinguir obligatoriamente entre:

1. código preexistente;
2. código legítimo permanente;
3. código introducido por el cambio;
4. instrumentación temporal que debería eliminarse.

No marcar automáticamente todo `TODO`, `Console.WriteLine` o similar como error.

Evaluar contexto y procedencia.

---

# 6. CÓDIGO MUERTO Y DUPLICACIÓN

Buscar:

- métodos sin referencias;
- handlers sin eventos;
- eventos sin handlers;
- controles Designer huérfanos;
- campos eliminados que aún tengan referencias;
- helpers duplicados;
- lógica funcional duplicada;
- código legacy que debía desaparecer;
- ramas imposibles;
- métodos reemplazados que siguen coexistiendo con la implementación nueva.

Cuando se haya centralizado una lógica en un helper/servicio, verificar que no continúe existiendo una segunda implementación divergente.

No refactorizar.

Solo informar.

---

# 7. REVISIÓN FUNCIONAL DEL CAMBIO

Reconstruir el flujo funcional completo utilizando:

- requisitos proporcionados;
- documentación del proyecto;
- código anterior cuando sea relevante;
- diff actual;
- tests;
- resultados de validación manual disponibles.

Verificar:

- flujo nominal;
- estados iniciales;
- persistencia;
- segunda ejecución;
- reapertura;
- datos históricos;
- valores vacíos/null;
- casos legacy;
- estados desactivados;
- duplicados;
- errores;
- límites relevantes;
- interacción entre formularios/componentes afectados.

No limitar la auditoría a comprobar que el código compila.

---

# 8. COMPATIBILIDAD Y REGRESIONES

Determinar qué comportamiento anterior podía verse afectado.

Revisar especialmente:

- lectura de datos existentes;
- escritura de datos;
- impresión;
- reportes;
- configuración;
- inicialización;
- apertura de formularios;
- eventos WinForms;
- limpieza de controles;
- navegación;
- persistencia SQLite;
- compatibilidad con datos legacy.

Si se modificó un comportamiento existente, comprobar que sea deliberado y esté respaldado por el requisito.

---

# 9. SQLITE Y PERSISTENCIA

Cuando el cambio afecte SQLite, revisar según corresponda:

- `CREATE TABLE`;
- `ALTER`;
- índices;
- UNIQUE;
- FK;
- seeds;
- migración de DB existente;
- DB nueva;
- segunda ejecución;
- idempotencia;
- parámetros SQL;
- manejo de NULL;
- conexiones;
- readers;
- commands;
- transactions cuando sean necesarias.

Buscar SQL construido mediante concatenación de entrada del usuario.

Preferir parámetros SQL.

Verificar explícitamente que un seed no vuelva a insertar datos administrados posteriormente por el usuario, salvo que ese comportamiento sea requisito.

Nunca modificar la DB real durante la auditoría.

Para inspección, usar modo read-only cuando sea posible.

---

# 10. WINFORMS / DESIGNER

Cuando existan cambios WinForms, revisar:

- jerarquía de Controls;
- declaraciones;
- instanciación;
- `Controls.Add`;
- eventos;
- `TabIndex`;
- `TabStop`;
- `Anchor`;
- `Dock`;
- tamaños;
- límites;
- visibilidad;
- controles eliminados;
- recursos;
- Designer/code-behind consistentes.

Buscar especialmente:

- handlers desconectados;
- handlers inexistentes;
- controles declarados pero no agregados;
- controles agregados pero no declarados;
- modificaciones masivas accidentales del Designer;
- problemas de foco;
- OwnerDraw;
- recursos GDI/GDI+ incorrectamente liberados.

No regenerar el Designer durante la auditoría.

---

# 11. RECURSOS Y DISPOSAL

Revisar correctamente el ciclo de vida de:

- SQLiteConnection;
- SQLiteCommand;
- SQLiteDataReader;
- Font;
- Image;
- Graphics;
- Stream;
- Form;
- Task/callbacks relacionados con formularios.

Prestar especial atención a objetos que NO son propiedad del método actual.

Nunca asumir que un objeto recibido desde un EventArgs puede ser dispuesto por el handler.

Buscar:

- double dispose;
- dispose de recursos compartidos;
- recursos sin liberar;
- callbacks sobre formularios cerrados.

---

# 12. THREADING / ASYNC

Si el cambio introduce Task, async, threads o callbacks:

Verificar:

- ningún worker thread toca controles WinForms directamente;
- actualización de UI mediante Invoke/BeginInvoke cuando corresponda;
- manejo de formulario cerrado/disposed;
- excepciones de tareas;
- tareas duplicadas;
- cache compartida;
- condiciones de carrera relevantes;
- deadlocks;
- bloqueo del hilo UI;
- `.Result` / `.Wait()` peligrosos en UI.

No considerar correcta una optimización solamente porque mueve trabajo desde `Load` a `Shown`.

Comprobar si el trabajo sigue bloqueando el hilo UI.

---

# 13. TESTS

Inspeccionar los tests agregados/modificados.

Determinar si prueban realmente el código productivo.

Buscar:

- copia de la lógica productiva dentro del test;
- tests que solo validan su propia implementación;
- asserts demasiado débiles;
- falsos positivos;
- dependencia innecesaria del entorno;
- dependencia del orden de ejecución;
- DB real;
- archivos reales del usuario;
- temporales sin cleanup;
- estado global no restaurado.

Cuando utilicen DB temporal:

- confirmar que sea realmente temporal;
- confirmar cleanup;
- confirmar que no se toque la DB real;
- confirmar restauración de variables/rutas globales cuando corresponda.

Los tests por reflexión son aceptables cuando permiten ejecutar la implementación productiva real y no duplican su lógica.

---

# 14. COMPILACIÓN

Ejecutar la compilación correspondiente al proyecto.

Para ServicioTecnico, utilizar los comandos definidos por el proyecto/AGENTS/documentación vigente.

Registrar:

- errores;
- warnings;
- warnings nuevos;
- warnings preexistentes;
- MSB3577;
- resultado final.

No corregir warnings durante la auditoría.

---

# 15. SUITE AUTOMATIZADA

Compilar y ejecutar el proyecto de tests correspondiente.

Registrar:

```text
Total:
PASS:
FAIL:
```

Listar cualquier test fallido.

Si el proyecto de tests no forma parte de la solución principal, compilarlo explícitamente.

No interpretar una compilación de la solución como evidencia de que los tests fueron ejecutados.

---

# 16. VALIDACIÓN MANUAL

Distinguir claramente:

```text
VALIDADO AUTOMÁTICAMENTE
VALIDADO POR INSPECCIÓN
VALIDADO MANUALMENTE POR EL USUARIO
PENDIENTE DE VALIDACIÓN
```

Nunca convertir una inspección de código en un `PASS manual`.

Nunca afirmar que una UI se ve correctamente si no existe evidencia visual/manual suficiente.

Si una prueba requiere interacción humana, marcarla como pendiente.

---

# 17. GIT DIFF --CHECK

Ejecutar:

```bash
git diff --check
```

Clasificar los resultados.

Trailing whitespace generado normalmente por Visual Studio Designer puede clasificarse como MENOR si:

- no afecta compilación;
- no afecta comportamiento;
- corregirlo produciría ruido innecesario;
- Visual Studio probablemente lo regeneraría.

No ocultar el hallazgo.

---

# 18. CLASIFICACIÓN DE HALLAZGOS

Todo hallazgo debe clasificarse en una de estas categorías.

## BLOQUEANTE

Ejemplos:

- pérdida/corrupción de datos;
- regresión funcional;
- crash;
- comportamiento contrario al requisito;
- migración destructiva;
- seed incorrecto;
- archivo necesario ausente;
- archivo necesario untracked que se omitiría del commit;
- test que aparenta validar algo pero no ejecuta la lógica real;
- cambio accidental relevante;
- compilación fallida;
- tests relevantes fallidos.

Un BLOQUEANTE implica:

```text
NO APTO PARA COMMIT
```

## IMPORTANTE

Problema introducido por el cambio que conviene resolver antes del commit aunque no produzca pérdida inmediata de datos.

Ejemplos:

- duplicación significativa;
- manejo incorrecto de recursos;
- deuda técnica directamente introducida que vuelve frágil la funcionalidad;
- test insuficiente sobre un comportamiento crítico;
- riesgo concreto de regresión.

Normalmente implica:

```text
NO APTO PARA COMMIT
```

salvo justificación técnica explícita.

## MENOR

Ejemplos:

- formato;
- naming;
- trailing whitespace;
- comentario;
- limpieza cosmética;
- deuda preexistente fuera del alcance.

Puede resultar en:

```text
APTO PARA COMMIT CON OBSERVACIONES
```

---

# 19. NO AMPLIAR EL ALCANCE

La auditoría NO es una revisión general de toda la aplicación.

No convertirla en una lista de mejoras futuras.

Solo reportar problemas fuera del alcance cuando:

- fueron introducidos por el cambio;
- interactúan directamente con el cambio;
- representan riesgo de regresión;
- son bloqueantes para versionarlo.

La deuda histórica no relacionada debe marcarse como:

```text
PREEXISTENTE / FUERA DE ALCANCE
```

y no debe bloquear el commit salvo riesgo directo.

---

# 20. VEREDICTO

Finalizar obligatoriamente con exactamente uno de estos tres estados:

## A) APTO PARA COMMIT

Usar cuando:

- BLOQUEANTES = 0
- IMPORTANTES = 0
- no existen observaciones relevantes pendientes.

## B) APTO PARA COMMIT CON OBSERVACIONES

Usar cuando:

- BLOQUEANTES = 0
- IMPORTANTES = 0
- existen únicamente hallazgos MENORES o preexistentes aceptables.

## C) NO APTO PARA COMMIT

Usar cuando:

- existe al menos un BLOQUEANTE;
- o existe un IMPORTANTE que debe corregirse antes de versionar.

---

# 21. FORMATO OBLIGATORIO DEL REPORTE

Entregar:

```text
AUDITORÍA FINAL PRE-COMMIT — <MEJORA/FASE>

1. Alcance auditado
2. Rama actual
3. Estado Git
4. Archivos modificados
5. Archivos nuevos/untracked
6. Archivos eliminados/renombrados
7. Cambios fuera de alcance
8. Código temporal/instrumentación
9. Código muerto/duplicado
10. Revisión funcional
11. Persistencia/SQLite
12. WinForms/Designer
13. Recursos/Threading
14. Tests
15. Compilación
16. Suite automatizada
17. Validaciones manuales conocidas
18. git diff --check
19. Hallazgos BLOQUEANTES
20. Hallazgos IMPORTANTES
21. Hallazgos MENORES
22. Elementos PREEXISTENTES/FUERA DE ALCANCE
23. VEREDICTO
```

El veredicto debe explicar brevemente por qué el cambio está o no en condiciones de ser versionado.

---

# 22. SALIDA FINAL OBLIGATORIA

Cerrar siempre indicando:

```text
Archivos modificados por la auditoría: ninguno
DB real modificada: no
Commit realizado: no
Push realizado: no
Modo: PLAN / SOLO LECTURA
```

Si por alguna razón una herramienta produjo una modificación accidental, NO afirmar que no hubo cambios.

Reportarlo inmediatamente como incidencia.

---

# 23. PRINCIPIO DE EVIDENCIA

Toda conclusión importante debe distinguir entre:

- evidencia directa del código;
- resultado de compilación;
- resultado de test automatizado;
- evidencia manual del usuario;
- inferencia técnica.

No presentar una inferencia como un hecho comprobado.

Ejemplo incorrecto:

```text
La impresión funciona porque usa cmbTipoEquipo.Text.
```

Ejemplo correcto:

```text
Por inspección, ImprimirCarta e ImprimirTicket consumen cmbTipoEquipo.Text.
La impresión física requiere validación manual si no existe un test que ejecute ese flujo.
```

La prioridad de la auditoría es detectar falsos PASS antes del commit.

---

# 24. PRINCIPIO DE MÍNIMO CAMBIO

La auditoría puede recomendar una corrección cuando encuentre un problema, pero:

- no debe implementarla;
- debe proponer primero el cambio mínimo;
- no debe aprovechar el hallazgo para refactorizar áreas no relacionadas;
- debe preservar comportamiento legacy salvo requisito contrario.

Después de un veredicto `NO APTO PARA COMMIT`, detenerse y esperar autorización para una corrección separada en modo BUILD.