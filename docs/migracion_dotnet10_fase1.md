# Fase 1 — Baseline .NET Framework 4.0

Documento de evidencia de la Fase 1 del plan de migración de **ServicioTecnico**
desde **.NET Framework 4.0** a **.NET 10**.

Referencia: `docs/migracion_dotnet10.md` (Fase 1 — Baseline).
Referencia de casos: `docs/testing/matriz_pruebas.md` (43 funcionales + 16 de regresión = 59).

Estado: fase de observación, ejecución y medición. No se modificó ningún archivo de la
aplicación (código, `.csproj`, `Lib/`, `tests/`, recursos, SQLite, configuración).

---

## 1. Objetivo

Establecer y documentar el estado real de la aplicación sobre .NET Framework 4.0
antes de cualquier migración:

- verificar el estado del proyecto (framework, plataforma, dependencias, BD, tests);
- compilar la versión actual sin cambios;
- ejecutar los tests automatizados existentes;
- registrar el estado de la matriz de 59 casos;
- obtener una baseline de rendimiento aproximada;
- documentar todo sin modificar la aplicación.

## 2. Estado del entorno

| Elemento | Detalle | Estado |
|---|---|---|
| Target framework | `net40` (`ServicioTecnico.csproj`) | Verificado |
| Plataforma | Windows — `Microsoft.NET.Sdk.WindowsDesktop`, `UseWindowsForms=True` | Verificado |
| Tipo de aplicación | `WinExe` | Verificado |
| Arquitectura | `PlatformTarget=x86` | Verificado |
| Compilación | Debug (configuración por defecto de MSBuild) | Verificado |
| Dependencias locales | `Lib/System.Data.SQLite.dll` (1.0.119.0, mixto x86), `Lib/BarcodeLib.dll` (1.0.0.21) | Verificado |
| Dependencias de framework | `Microsoft.VisualBasic`, `System.Data`, `System.Windows.Forms.DataVisualization`; paquete de build `Microsoft.NETFramework.ReferenceAssemblies` 1.0.3 | Verificado |
| Base SQLite | `Application.StartupPath\ordenes.db` (`modConexion.rutaDB`); presente en `bin/Debug/net40/ordenes.db` | Verificado |
| Proyecto de tests | `tests/ServicioTecnico.Tests` (net40, x86, Exe): `Program.cs`, `ReflectionModConexion.cs`, `TestsModConexion.cs`, `.csproj` | Verificado |
| Entorno de ejecución (baseline) | Windows con .NET Framework 4.0+ y MSBuild/VS 2022 (`.slnx`); se ejecutó manualmente | Ejecutado |
| Impresora | No reportada en esta baseline | Dato no disponible |

### Comandos de compilación y ejecución

- App: `msbuild ServicioTecnico.slnx /restore /p:Configuration=Debug`
- Tests: `msbuild tests\ServicioTecnico.Tests\ServicioTecnico.Tests.csproj /restore /p:Configuration=Debug`
- Ejecutar tests: `tests\ServicioTecnico.Tests\bin\Debug\net40\ServicioTecnico.Tests.exe`
- Ejecutar app: `bin\Debug\net40\ServicioTecnico.exe`

## 3. Compilación baseline

| Proyecto | Configuración | Resultado | MSBuild ERRORLEVEL | Errores | Warnings |
|---|---|---|---|---|---|
| ServicioTecnico (`.slnx`) | Debug | **PASS** | 0 | 0 | No reportados (dato no disponible) |
| ServicioTecnico.Tests | Debug | **PASS** | 0 | 0 | No reportados (dato no disponible) |

No se registraron errores ni problemas preexistentes de compilación.

## 4. Tests automatizados existentes

Actualmente solo existen **3 tests implementados** en `tests/ServicioTecnico.Tests`, todos
sobre `modConexion` vía reflexión y base temporal:

| ID | Funcionalidad | Descripción |
|---|---|---|
| FUN-041 | BD (unit) | Round-trip `GuardarConfiguracion`/`ObtenerConfiguracion` sobre BD temporal |
| FUN-042 | BD (unit) | Round-trip `GuardarCondicionesServicio`/`ObtenerCondicionesServicio` sobre BD temporal |
| FUN-043 | BD (unit) | `VerificarOCrearBD` en carpeta vacía: crea `ordenes.db`, las 5 tablas y las 3 claves de configuración iniciales; verifica idempotencia |

El resto de la matriz (56 casos) **no está automatizado**.

## 5. Matriz de 59 casos

Referencia: `docs/testing/matriz_pruebas.md`. Todos comenzaron en `NOT_RUN`.

| Conjunto | Total | Automatizados | Manuales | Estado en esta baseline |
|---|---|---|---|---|
| FUN-001..FUN-040 | 40 | 0 | 40 | `NOT_RUN` / pendiente (requieren Windows + UI; impresión requiere impresora) |
| FUN-041..FUN-043 | 3 | 3 | 0 | **PASS** (ejecutados) |
| REG-001..REG-016 | 16 | 0 | 16 | `NOT_RUN` / pendiente (manuales; impresión requiere impresora) |
| **Total** | **59** | **3** | **56** | 3 PASS / 56 NOT_RUN |

## 6. Resultados de las pruebas ejecutadas

| ID | Estado | Evidencia |
|---|---|---|
| FUN-041 | **PASS** | Salida del runner: PASS |
| FUN-042 | **PASS** | Salida del runner: PASS |
| FUN-043 | **PASS** | Salida del runner: PASS |

Total: 3 — PASS: 3 — FAIL: 0.

## 7. Casos manuales / no automatizados

- 40 casos funcionales manuales (FUN-001..040) y 16 de regresión (REG-001..016):
  **NOT_RUN / pendientes**. No se afirma su resultado.
- Requieren Windows + interacción UI. La impresión física (FUN-031..034, REG-008..011)
  además requiere impresora instalada; sin impresora el estado es BLOCKED (no PASS).
- Durante la baseline solo se ejecutaron los tests automatizados y se realizaron
  observaciones informales del arranque (ver sección 9); esas observaciones **no
  sustituyen** la ejecución formal de los casos manuales.

## 8. Incidencias o comportamientos preexistentes

| # | Incidencia | Clasificación |
|---|---|---|
| I1 | La medición automática de arranque con `WaitForInputIdle()` dio `MAIN_FORM_MS=161`, pero durante esa ejecución apareció una ventana de error. Por eso **la medición se descarta como NO VÁLIDA** y no debe usarse como baseline | Medición descartada — no es un fallo funcional confirmado |
| I2 | Arranque manual desde el Explorador de archivos: la aplicación abrió sin ventana de error | Verificado (observación) |
| I3 | Riesgos R1–R8 documentados en `docs/testing/matriz_pruebas.md` (autoimpresión 5G.1, `CheckRequiredFiles`, fechas en texto, reportes limitados a 100 filas, etc.) | Información existente en documentación — no verificados en esta fase |

## 9. Baseline de rendimiento

### Cuantitativa
No fue posible obtener tiempos en milisegundos de forma reproducible y válida en esta
baseline. Por lo tanto **no se registran valores numéricos** (no inventar).

### Cualitativa (observaciones manuales — no mediciones)
| Operación | Observación |
|---|---|
| Abrir Clientes | Instantáneo, sin demora perceptible |
| Buscar orden | Instantáneo, sin demora perceptible |
| Cargar orden | Instantáneo, sin demora perceptible |
| Cargar orden con imágenes | Instantáneo, sin demora perceptible |
| Generación de código de barras | Funcionamiento normal |
| Generación de reportes | Funcionamiento normal |

> "Instantáneo" es una observación cualitativa, no una medición de tiempo.
> Queda pendiente una baseline de rendimiento cuantitativa reproducible para
> comparar con .NET 10 en fases posteriores.

## 10. Conclusión de la Fase 1

- **Compilación**: verificada — PASS (app y tests, ERRORLEVEL 0).
- **Cobertura real de tests automatizados**: 3 casos (FUN-041..043), todos PASS.
- **Matriz de 59 casos**: estado conocido y registrado — 3 ejecutados (PASS), 56 pendientes (NOT_RUN).
- **Baseline de rendimiento**: solo cualitativa; cuantitativa no disponible en esta fase.
- **Incidencias**: medición de arranque descartada (no válida); sin fallos funcionales confirmados.
- **Sin modificaciones**: no se modificó ningún archivo de la aplicación.

La Fase 1 se considera **SUPERADA** en cuanto al estado documentado (compilación y tests
verificados, matriz registrada), con la salvedad de que la baseline de rendimiento es
cualitativa y queda pendiente la medición cuantitativa.

## 11. Archivos modificados o creados

- **Creado**: `docs/migracion_dotnet10_fase1.md` (este documento).
- No se modificó ningún archivo de la aplicación (código, `.csproj`, `.slnx`, `Lib/`,
  `tests/`, `App.config`, recursos, SQLite, BarcodeLib ni configuración de compilación).

**NO se avanza a la Fase 2.**
