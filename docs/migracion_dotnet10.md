# Plan de Migración de ServicioTecnico a .NET 10

## Objetivo

Migrar el proyecto **ServicioTecnico** desde **.NET Framework 4.0** a **.NET 10**, manteniendo el comportamiento funcional existente y realizando una validación completa mediante pruebas de regresión y un chequeo comparativo de rendimiento.

La migración debe realizarse de forma controlada, evitando combinarla con refactorizaciones o cambios funcionales no necesarios.

---

## Principios de la migración

1. Mantener el comportamiento observable de la aplicación.
2. Separar los riesgos de compatibilidad de .NET Framework de los riesgos del salto a .NET moderno.
3. Resolver los bloqueantes técnicos antes de comenzar la migración.
4. No introducir refactorizaciones innecesarias durante la migración.
5. Utilizar la matriz de pruebas existente como baseline funcional.
6. Validar no solo funcionalidad, sino también datos y rendimiento.

---

# Fase 0 — Factibilidad y bloqueantes

Esta fase es un **gate previo obligatorio**.

No se debe comenzar la Fase 1 hasta haber resuelto los posibles bloqueantes de la migración.

## 0.1 BarcodeLib.dll — prioridad máxima

`BarcodeLib.dll` debe verificarse primero porque puede constituir un bloqueo duro para la migración.

### Verificaciones

- Identificar versión exacta de `BarcodeLib.dll`.
- Determinar si es una biblioteca administrada, nativa o mixta.
- Determinar arquitectura soportada:
  - x86
  - x64
  - AnyCPU
- Determinar sobre qué versión de .NET/.NET Framework fue compilada.
- Verificar compatibilidad real con .NET 10.
- Verificar compatibilidad con WinForms sobre .NET 10.

### Si BarcodeLib es compatible

Mantenerla y continuar con el plan.

### Si BarcodeLib no es compatible

Evaluar:

1. Existencia de una versión compatible.
2. Existencia de un reemplazo equivalente.
3. Impacto sobre el código existente.
4. Cambios necesarios para generación/lectura de códigos de barras.
5. Nuevos casos de prueba necesarios.
6. Impacto estimado sobre tiempo y alcance de la migración.

Si no existe un reemplazo directo, la sustitución de BarcodeLib debe tratarse como **subproyecto independiente** antes de continuar con la migración.

> **Regla:** BarcodeLib es un gate de factibilidad. Si no puede resolverse de forma aceptable, la migración puede quedar pospuesta hasta resolver esta dependencia.

---

## 0.2 SQLite — decisión explícita

Identificar exactamente:

- proveedor SQLite utilizado actualmente;
- versión;
- referencias existentes;
- APIs utilizadas;
- consultas y operaciones realizadas;
- tipos de datos utilizados.

Determinar si se:

- mantiene el proveedor actual con una versión compatible, o
- migra a `Microsoft.Data.Sqlite`.

La migración del proveedor no debe asumirse automáticamente.

### Riesgos a evaluar

Especial atención a posibles diferencias en:

- `NULL`;
- tipos numéricos;
- fechas;
- texto;
- conversiones;
- inserciones;
- actualizaciones;
- eliminaciones;
- consultas;
- parámetros;
- manejo de conexiones;
- transacciones.

Las operaciones afectadas deberán quedar cubiertas por pruebas específicas.

---

## 0.3 Arquitectura y tipo de aplicación

La aplicación debe conservar:

- **WinForms**
- **WinExe**
- **x86**

En el proyecto SDK-style se debe declarar explícitamente:

```xml
<OutputType>WinExe</OutputType>
<PlatformTarget>x86</PlatformTarget>
```

No se debe dejar la arquitectura implícita si alguna dependencia requiere x86.

---

# Fase 1 — Baseline

Una vez despejados los bloqueantes de la Fase 0, se establece la línea base de la aplicación actual.

## Versión de referencia

**.NET Framework 4.0**

## Objetivos

Registrar el comportamiento actual antes de realizar modificaciones.

Se ejecutará la matriz existente:

- **43 pruebas funcionales**
- **16 pruebas de regresión**
- **59 casos en total**

### Registrar

- resultado de cada prueba;
- comportamiento esperado;
- comportamiento observado;
- datos utilizados;
- incidencias existentes;
- tiempos aproximados de operaciones relevantes.

Esta versión constituye la **baseline funcional y de rendimiento**.

---

# Fase 2 — Migración .NET Framework 4.0 → 4.8.1

Se realizará primero el paso intermedio a **.NET Framework 4.8.1**.

El objetivo es aislar los problemas de compatibilidad dentro del ecosistema .NET Framework antes de realizar el salto a .NET moderno.

## Objetivo

Confirmar:

> .NET Framework 4.0 ≡ .NET Framework 4.8.1 en comportamiento funcional.

## Validaciones

Repetir la matriz completa de 59 pruebas.

Verificar especialmente:

- inicio y cierre de la aplicación;
- formularios;
- clientes;
- activos;
- órdenes de trabajo;
- SQLite;
- imágenes;
- configuración;
- códigos de barras;
- impresión;
- reportes.

Cualquier diferencia debe registrarse y clasificarse como:

- regresión;
- cambio de comportamiento;
- problema preexistente;
- incompatibilidad.

---

# Fase 3 — Migración .NET Framework 4.8.1 → .NET 10

Este constituye el salto tecnológico principal.

## Objetivo

Migrar la aplicación a .NET 10 manteniendo el comportamiento existente.

## Elementos a revisar

- Proyecto `.csproj` SDK-style.
- Target Framework.
- WinForms.
- Referencias de ensamblados.
- APIs obsoletas o eliminadas.
- `Microsoft.VisualBasic`.
- SQLite.
- `BarcodeLib.dll`.
- recursos `.resx`.
- imágenes.
- rutas de archivos.
- configuración.
- impresión.
- reportes.
- código de barras.
- arquitectura x86.
- inicialización y cierre de la aplicación.

## Regla

Durante esta fase:

> **No realizar refactorizaciones que no sean necesarias para la migración.**

La prioridad es obtener una versión funcional equivalente.

---

# Fase 4 — Regresión completa y rendimiento

Una vez que la aplicación compile y pueda ejecutarse sobre .NET 10, se realizará la validación completa.

## Regresión funcional

Ejecutar nuevamente los **59 casos**:

### Pruebas funcionales

- Clientes.
- Activos.
- Órdenes.
- Estados.
- Búsquedas.
- Configuración.
- Imágenes.
- Códigos de barras.
- Impresión.
- Reportes.

### Pruebas de regresión

Repetir específicamente los escenarios que puedan verse afectados por:

- SQLite;
- manejo de archivos;
- imágenes;
- dependencias externas;
- arquitectura x86;
- WinForms;
- impresión;
- generación/lectura de códigos de barras.

---

## Chequeo de rendimiento

Los 59 casos validan principalmente resultados funcionales, por lo que se realizará adicionalmente un chequeo comparativo de tiempos.

Comparar aproximadamente contra la baseline:

- tiempo de inicio de la aplicación;
- apertura de formularios;
- consultas SQLite relevantes;
- carga de órdenes;
- carga de imágenes;
- búsquedas;
- generación de códigos de barras;
- generación de reportes;
- impresión;
- operaciones frecuentes.

El objetivo inicial no es construir un benchmark científico, sino detectar:

- regresiones perceptibles;
- operaciones significativamente más lentas;
- cuellos de botella introducidos por la migración.

Si aparece una diferencia significativa, se analizará antes de aprobar la migración.

---

# Criterio de finalización

La migración se considera aprobada cuando se cumplen simultáneamente estas condiciones:

1. **Compila correctamente en .NET 10.**
2. **La aplicación ejecuta correctamente.**
3. **Los datos existentes se conservan correctamente.**
4. **El comportamiento funcional se mantiene.**
5. **Los 59 casos de prueba pasan.**
6. **No existen regresiones funcionales relevantes.**
7. **No existen regresiones de rendimiento relevantes.**
8. **Las dependencias críticas son compatibles y están resueltas.**
9. **La arquitectura x86 se mantiene correctamente.**

En forma resumida:

> **Compila + ejecuta + conserva datos + conserva comportamiento + pasa regresión + no presenta regresiones de rendimiento relevantes.**

---

# Orden definitivo del proceso

```text
FASE 0
Factibilidad y bloqueantes
        │
        ├── BarcodeLib.dll
        │       └── Resolver antes de continuar
        │
        ├── SQLite
        │       └── Decisión explícita
        │
        └── Arquitectura x86
                │
                ▼
FASE 1
Baseline .NET Framework 4.0
        │
        └── 59 pruebas + tiempos de referencia
                │
                ▼
FASE 2
.NET Framework 4.0 → 4.8.1
        │
        └── Regresión completa
                │
                ▼
FASE 3
.NET Framework 4.8.1 → .NET 10
        │
        └── Migración técnica
                │
                ▼
FASE 4
Regresión + rendimiento
        │
        ├── 59 pruebas
        ├── Comparación funcional
        └── Chequeo de tiempos
                │
                ▼
        APROBACIÓN DE MIGRACIÓN
```

---

# Regla de control de cambios

Durante la migración se debe evitar modificar simultáneamente:

- arquitectura;
- lógica de negocio;
- interfaz;
- base de datos;
- dependencias;
- comportamiento funcional.

Si una modificación adicional resulta necesaria, debe documentarse para poder determinar posteriormente si una eventual diferencia proviene de la migración o del cambio adicional.

---

# Resultado esperado

Al finalizar se debe disponer de una versión de **ServicioTecnico ejecutándose sobre .NET 10**, manteniendo:

- funcionalidad existente;
- datos existentes;
- WinForms;
- arquitectura x86;
- SQLite;
- códigos de barras;
- imágenes;
- impresión;
- reportes;

y con evidencia de que la migración no introdujo regresiones funcionales ni degradaciones de rendimiento relevantes.
