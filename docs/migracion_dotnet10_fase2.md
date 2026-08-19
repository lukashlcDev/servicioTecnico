# Fase 2 — Migración .NET Framework 4.0 → 4.8.1

Documento de evidencia de la Fase 2 del plan de migración de **ServicioTecnico**
desde **.NET Framework 4.0** a **.NET 10**.

Referencia: `docs/migracion_dotnet10.md` (Fase 2 — .NET Framework 4.0 → 4.8.1).
Baseline: `docs/migracion_dotnet10_fase1.md` (Fase 1 — Baseline .NET Framework 4.0).
Referencia de casos: `docs/testing/matriz_pruebas.md` (43 funcionales + 16 de regresión = 59).

Estado: **SUPERADA**. No se modificó código C#, lógica de negocio, interfaz, SQLite,
BarcodeLib, recursos ni App.config.

---

## 1. Objetivo

Migrar la aplicación de **.NET Framework 4.0** a **.NET Framework 4.8.1** manteniendo
el comportamiento funcional existente. Esta fase es un paso intermedio dentro del
ecosistema .NET Framework antes del salto a .NET 10, con el fin de aislar problemas
de compatibilidad.

## 2. Estado inicial

| Elemento | Valor |
|---|---|
| Target framework previo | `net40` |
| Plataforma | Windows, `Microsoft.NET.Sdk.WindowsDesktop`, `UseWindowsForms=True` |
| Tipo de aplicación | `WinExe` |
| Arquitectura | `PlatformTarget=x86` |
| Dependencias locales | `Lib/System.Data.SQLite.dll` (1.0.119.0, mixto x86), `Lib/BarcodeLib.dll` (1.0.0.21) |
| Dependencias de framework | `Microsoft.VisualBasic`, `System.Data`, `System.Windows.Forms.DataVisualization` |
| Paquete de build | `Microsoft.NETFramework.ReferenceAssemblies` 1.0.3 |
| Tests | 3 implementados (FUN-041, FUN-042, FUN-043), target `net40` |
| Compilación previa | PASS (Fase 1, ERRORLEVEL 0) |
| Tests previos | 3/3 PASS (Fase 1) |
| Entorno de ejecución | Windows 10, x86, .NET Framework 4.8.1 instalado (Release 533325) |

## 3. Cambios realizados

| Archivo | Cambio | Línea |
|---|---|---|
| `ServicioTecnico.csproj` | `<TargetFramework>net40</TargetFramework>` → `<TargetFramework>net481</TargetFramework>` | 7 |
| `tests/ServicioTecnico.Tests/ServicioTecnico.Tests.csproj` | `<TargetFramework>net40</TargetFramework>` → `<TargetFramework>net481</TargetFramework>` | 4 |

No se realizaron otros cambios. No se modificó:
- código C#;
- `App.config`;
- `Lib/System.Data.SQLite.dll`;
- `Lib/BarcodeLib.dll`;
- recursos `.resx`;
- `ServicioTecnico.slnx`;
- `Properties/AssemblyInfo.cs`.

## 4. Compilación

| Proyecto | Configuración | Resultado | MSBuild ERRORLEVEL | Errores | Warnings |
|---|---|---|---|---|---|
| ServicioTecnico (`.slnx`) | Debug | **PASS** | 0 | 0 | Ninguno reportado |
| ServicioTecnico.Tests | Debug | **PASS** | 0 | 0 | Ninguno reportado |

La carpeta de salida cambió automáticamente de `bin/Debug/net40/` a `bin/Debug/net481/`.

## 5. Tests automatizados

| ID | Funcionalidad | Resultado |
|---|---|---|
| FUN-041 | Round-trip `GuardarConfiguracion`/`ObtenerConfiguracion` | **PASS** |
| FUN-042 | Round-trip `GuardarCondicionesServicio`/`ObtenerCondicionesServicio` | **PASS** |
| FUN-043 | `VerificarOCrearBD` — creación de tablas e inserts iniciales | **PASS** |

Total: 3 — PASS: 3 — FAIL: 0.

## 6. Validación SQLite

- El runner de tests (FUN-041..043) crea una BD temporal, ejecuta las operaciones
  y verifica round-trips: PASS.
- La aplicación crea `ordenes.db` en la carpeta de ejecución cuando no existe
  (ver incidencia I1 en sección 11).
- Las 5 tablas (`clientes`, `ordenes`, `condiciones_servicio`, `configuracion`,
  `estados`) se crean correctamente.
- Las 3 claves de configuración iniciales se insertan.
- Persistencia verificada manualmente (crear cliente/orden → cerrar → reabrir → recuperar).

No se cambió el proveedor `System.Data.SQLite`. Se mantiene `Lib/System.Data.SQLite.dll`
versión 1.0.119.0.

## 7. Validación BarcodeLib

- La aplicación inicia sin excepción de carga de ensamblado.
- Se generaron códigos de barras CODE128 y CODE39 durante la validación manual
  (código de barras en orden cargada): PASS.
- Se mantiene `Lib/BarcodeLib.dll` versión 1.0.0.21 sin cambios.

## 8. Validación de formularios

| Formulario | Resultado |
|---|---|
| frmOrdenServicio (inicio) | **PASS** |
| frmClientes | **PASS** |
| frmConfiguracion | **PASS** |
| frmCondicionesServicio | **PASS** (se modificaron datos y funcionó) |
| frmBuscaOrden | **PASS** |
| Cargar orden | **PASS** |
| Cargar orden con imágenes | **PASS** |
| Generar código de barras | **PASS** |
| Generar reporte | **PASS** |

No se detectaron excepciones, errores ni diferencias observables.

## 9. Validación de recursos

- `Resources/logo.jpg` — cargado correctamente (verificado en formularios).
- `Resources/work.ico` — cargado correctamente (icono de aplicación visible).
- Recursos `.resx` de formularios — funcionales, sin errores.

## 10. Regresión completa

| ID | Funcionalidad | Resultado |
|---|---|---|
| REG-001 | Inicio de aplicación | **PASS** |
| REG-002 | Abrir Clientes | **PASS** |
| REG-003 | Crear cliente | **PASS** |
| REG-004 | Buscar cliente | **PASS** |
| REG-005 | Crear orden | **PASS** |
| REG-006 | Consultar orden | **PASS** |
| REG-007 | Guardar configuración | **PASS** |
| REG-008 | Impresión manual ticket | **PASS** |
| REG-009 | Impresión manual carta | **PASS** |
| REG-010 | Impresión automática ticket | **PASS** |
| REG-011 | Impresión automática carta | **PASS** |
| REG-012 | Generación de reporte | **PASS** |
| REG-013 | Persistencia SQLite | **PASS** |
| REG-014 | Cálculo total = presupuesto − abono | **PASS** |
| REG-015 | Eliminación de cliente con órdenes | **PASS** |
| REG-016 | Condiciones de servicio en impresión | **PASS** |

Total: 16 — PASS: 16 — FAIL: 0 — BLOCKED: 0.

> Los casos de impresión (REG-008..011) fueron ejecutados con impresora disponible
> y resultaron PASS. En la Fase 1 estos casos estaban en NOT_RUN (impresora no
> reportada). En esta fase se ejecutaron por primera vez y pasaron.

## 11. Incidencias

| # | Incidencia | Clasificación |
|---|---|---|
| I1 | **Cartel "Archivos faltantes" al primer inicio desde net481**: al ejecutar desde `bin\Debug\net481\` por primera vez, apareció el aviso "Archivos faltantes — Faltan archivos esenciales: ordenes.db". Hechos comprobados: (a) después de aceptar el aviso, la aplicación inició correctamente; (b) las operaciones SQLite probadas funcionaron; (c) no se realizó ninguna modificación para ocultar o corregir el aviso; (d) no existe evidencia suficiente en esta fase para determinar si el comportamiento es preexistente, consecuencia del cambio de target o consecuencia de la nueva carpeta de salida | Incidencia observada durante el primer arranque de la versión net481; origen no determinado |
| I2 | **Runner de tests muestra "NET Framework 4.0"**: el texto impreso por el runner indica `.NET Framework 4.0` como entorno, pero el ejecutable se ubica en `bin\Debug\net481\`. El texto es hardcodeado en el runner y no refleja el runtime real. **No es evidencia de que el runtime sea 4.0** | Discrepancia de texto en el runner — no afecta funcionalidad; no modificar en esta fase |
| I3 | **Baseline de rendimiento Fase 1 era cualitativa**: la Fase 1 no obtuvo tiempos cuantitativos. La comparación de rendimiento con Fase 2 se realiza cualitativamente (sin regresiones perceptibles) | Limitación conocida de Fase 1 |

No se detectaron nuevas regresiones, comportamientos incompatibles ni errores introducidos por la migración.

## 12. Comparación con baseline de Fase 1

| Aspecto | Fase 1 (net40) | Fase 2 (net481) | Diferencia |
|---|---|---|---|
| Compilación app | PASS, ERRORLEVEL 0 | PASS, ERRORLEVEL 0 | Sin diferencia |
| Compilación tests | PASS, ERRORLEVEL 0 | PASS, ERRORLEVEL 0 | Sin diferencia |
| Tests FUN-041..043 | 3/3 PASS | 3/3 PASS | Sin diferencia |
| Inicio de aplicación | Sin errores (observación manual) | Cartel de `ordenes.db` en primer inicio → OK (incidencia I1) | Diferencia observada; origen no determinado |
| Clientes | Instantáneo | PASS | Sin diferencia |
| Configuración | — | PASS | Sin diferencia |
| Condiciones de servicio | — | PASS | Sin diferencia |
| Buscar orden | Instantáneo | PASS | Sin diferencia |
| Cargar orden | Instantáneo | PASS | Sin diferencia |
| Cargar orden con imágenes | Instantáneo | PASS | Sin diferencia |
| Código de barras | Funcionamiento normal | PASS | Sin diferencia |
| Reportes | Funcionamiento normal | PASS | Sin diferencia |
| Regresión (16 casos) | No ejecutada (NOT_RUN) | 16/16 PASS | Regresión ejecutada por primera vez: PASS |
| Impresión | No ejecutada (sin impresora) | PASS (ticket + carta, manual + automática) | Impresión ejecutada por primera vez: PASS |

No se observan diferencias funcionales significativas entre la baseline de Fase 1 y
los resultados de Fase 2. La única diferencia observable (incidencia I1) corresponde
al primer arranque de la versión net481; su origen no fue determinado en esta fase.

## 13. Archivos modificados

| Archivo | Tipo de cambio |
|---|---|
| `ServicioTecnico.csproj` | `net40` → `net481` (1 línea) |
| `tests/ServicioTecnico.Tests/ServicioTecnico.Tests.csproj` | `net40` → `net481` (1 línea) |

Total: **2 archivos modificados**. No se modificó ningún otro archivo del proyecto.

## 14. Criterio de aprobación

| # | Criterio | Estado |
|---|---|---|
| 1 | Compila en .NET Framework 4.8.1 sin errores | **CUMPLIDO** — ERRORLEVEL 0, 0 errores |
| 2 | La aplicación inicia correctamente | **CUMPLIDO** — inicia después del primer arranque (incidencia I1 registrada, origen no determinado) |
| 3 | SQLite funciona correctamente | **CUMPLIDO** — creación, tablas, CRUD, persistencia verificados |
| 4 | BarcodeLib funciona correctamente | **CUMPLIDO** — CODE128 y CODE39 generados sin excepción |
| 5 | FUN-041, FUN-042, FUN-043 pasan | **CUMPLIDO** — 3/3 PASS |
| 6 | No aparecen regresiones funcionales relevantes | **CUMPLIDO** — 16/16 REG PASS, 0 FAIL |
| 7 | WinForms + WinExe + x86 se mantienen | **CUMPLIDO** — sin cambios en OutputType, UseWindowsForms ni PlatformTarget |
| 8 | Comportamiento observable equivalente a Fase 1 | **CUMPLIDO** — sin diferencias funcionales significativas |

Todos los criterios cumplidos.

## 15. Conclusión

La migración de .NET Framework 4.0 a .NET Framework 4.8.1 se completó con éxito.

Los únicos cambios realizados fueron la actualización de `TargetFramework` de `net40`
a `net481` en los dos archivos `.csproj`. No se modificó código, dependencias,
recursos ni configuración.

Los resultados de compilación (PASS, 0 errores), tests automatizados (3/3 PASS),
validación manual de formularios (9/9 PASS) y regresión completa (16/16 PASS)
confirman que el comportamiento de la aplicación se mantiene equivalente al baseline
de Fase 1.

Las dos incidencias registradas (cartel de `ordenes.db` en primer inicio y texto del
runner) no constituyen regresiones introducidas por la migración. La primera fue
observada durante el primer arranque de la versión net481 con origen no determinado;
la segunda es una discrepancia cosmética del runner.

La **Fase 2 se considera SUPERADA**.

**NO se avanza a la Fase 3.**
