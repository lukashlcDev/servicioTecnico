# Roadmap de Implementación de Mejoras — ServicioTécnico .NET Framework 4.8.1

- **Rama:** `release/4.8.x`
- **Target:** .NET Framework 4.8.1
- **Modo:** documento ejecutivo aprobado para construcción.
- **Restricción:** este roadmap no autoriza modificaciones de código. Cada mejora requerirá su propia autorización de implementación.

## 1. Resumen del alcance

| Categoría | Valor |
|---|---|
| Mejoras activas en 4.8.x | 12 |
| Mejora postergada | 1 (MEJ-011) |
| Etapas activas | 8 (Etapa 0 a Etapa 7) |
| Versiones propuestas | 7 (v4.8.2 a v4.8.8) |
| Tags/releases | Solo tras aprobación explícita |

Una mejora puede tener su propio commit aunque comparta una versión estable con otras mejoras coherentes.

## 2. Orden activo definitivo

```text
Etapa 0 — Decisiones e investigaciones previas
Etapa 1 — Layout y recursos visuales
    MEJ-005  Redimensionamiento de frmOrdenServicio
    MEJ-006  Dimensiones de campos de texto
    MEJ-003  Cambio de iconos
    MEJ-007  Tamaño de iconos
Etapa 2 — Presentación monetaria y búsqueda
    MEJ-001  Formato monetario
    MEJ-008  Rediseno de Buscar Orden
Etapa 3 — Proteccion de datos
    MEJ-013  Exportar / importar base de datos
Etapa 4 — Tipos de equipo
    MEJ-002  Tipos de equipo
Etapa 5 — Impresion base
    MEJ-004  Formato de impresion
Etapa 6 — Codigo y fuentes de impresion
    MEJ-009  Codigo de barras / QR
    MEJ-010  Fuentes nativas
Etapa 7 — Reportes
    MEJ-012  Reportes
```

MEJ-011 (Modo oscuro) no forma parte del roadmap activo de `release/4.8.x`.

## 3. Etapas activas

### Etapa 0 — Decisiones e investigaciones previas

**Objetivo:** eliminar bloqueantes antes de modificar código.

**Incluye:**
- Decisiones de MEJ-001, MEJ-002, MEJ-012 y MEJ-013.
- Investigación de MEJ-004, MEJ-009 y MEJ-013.
- Definición de criterios visuales y de prueba.

**Gate de entrada:**
- Fases 1 a 4 aprobadas.
- Baseline técnica disponible.
- Matriz de pruebas sincronizada.

**Gate de salida:**
- Decisiones funcionales documentadas.
- Investigaciones técnicas concluidas.
- Alcance de cada mejora definido.

**Pruebas:** no se implementan cambios; conservar `FUN-041..FUN-043` y `REG-001..REG-016` como baseline.

**Rollback:** no aplica a código.

---

### Etapa 1 — Layout y recursos visuales

**MEJ incluidas, en orden:**
1. MEJ-005 — Redimensionamiento de `frmOrdenServicio`.
2. MEJ-006 — Dimensiones de campos de texto.
3. MEJ-003 — Cambio de iconos.
4. MEJ-007 — Tamaño de iconos.

**Objetivo:** estabilizar la estructura visual del formulario principal antes de agregar funcionalidad.

**Gate de entrada:**
- Estrategia de layout aprobada (`Anchor`, `Dock`, contenedores, etc.).
- Tamaño mínimo y DPI objetivo definidos.
- Recursos visuales disponibles para MEJ-003.

**Gate de salida:**
- Todos los controles accesibles.
- Campos largos utilizables.
- Iconos visibles y no recortados.
- Sin cambios funcionales en órdenes, fotos o botones.

**Pruebas mínimas:**
- `FUN-001`, `FUN-015`, `FUN-016`, `FUN-018`, `FUN-020`, `FUN-026`.
- `REG-001`, `REG-005`, `REG-006`, `REG-013`.
- Pruebas nuevas de maximizado, DPI, textos largos e iconos.

**Rollback:** simple por commit individual.

---

### Etapa 2 — Presentación monetaria y búsqueda

**MEJ incluidas, en orden:**
1. MEJ-001 — Formato monetario.
2. MEJ-008 — Rediseño de Buscar Orden.

**Objetivo:** mejorar presentación y experiencia de consulta sin cambiar el esquema de datos.

**Gate de entrada:**
- Formato monetario definido para MEJ-001.
- Contrato `OrdenSeleccionada` preservado para MEJ-008.
- Layout de Etapa 1 estable.

**Gate de salida:**
- Valores consistentes en UI, grid e impresión.
- Búsqueda actual preservada.
- Nuevas funciones de búsqueda aisladas y verificadas.

**Pruebas mínimas:**
- MEJ-001: `FUN-019`, `FUN-031..FUN-034`, `FUN-037`, `REG-008..REG-011`, `REG-014`.
- MEJ-008: `FUN-021`, `FUN-022`, `REG-006`, `REG-013`.

**Rollback:** simple por commit.

---

### Etapa 3 — Protección de datos

**MEJ incluida:**
- MEJ-013 — Exportar / importar base de datos.

**Objetivo:** disponer de un mecanismo de backup/restore probado antes de cualquier cambio con impacto potencial sobre datos.

**Motivo:** MEJ-013 tiene riesgo técnico CRÍTICO y actúa como protección para MEJ-002 y otras mejoras de datos.

**Gate de entrada:**
- Estrategia de backup aprobada.
- Definición sobre inclusión de la carpeta `imagenes/`.
- Estrategia para SQLite cerrado y SQLite en uso.

**Gate de salida obligatorio:**
- Backup probado.
- Restore probado.
- Integridad comprobada.
- Imágenes consideradas según decisión aprobada.
- BD original preservada.
- Restauración de prueba exitosa.
- Pruebas específicas PASS.
- Regresión de arranque, persistencia y lectura PASS.

**Pruebas mínimas:**
- `FUN-001`, `FUN-018`, `FUN-020`, `FUN-026`, `FUN-041..FUN-043`.
- `REG-001`, `REG-006`, `REG-013`.
- Pruebas nuevas de BD cerrada, BD en uso, integridad e imágenes.

**Rollback:** con riesgo de datos. Nunca sobrescribir la BD original sin preservarla.

**Regla:** esta etapa no se mezcla con otra mejora funcional.

---

### Etapa 4 — Tipos de equipo

**MEJ incluida:**
- MEJ-002 — Tipos de equipo.

**Objetivo:** modificar el catálogo de equipos preservando valores existentes.

**Gate de entrada:**
- Gate de salida de MEJ-013 cumplido.
- Catálogo final aprobado.
- Tratamiento de `Laptop`, `Impresora`, `PC`, `Otros: xxx`, `Otros: ` y valores no reconocidos definido.

**Gate de salida:**
- Valores nuevos guardados y cargados correctamente.
- Datos existentes preservados.
- Reportes, búsqueda e impresión coherentes.
- `FUN-025`, `REG-005` y `REG-006` PASS.

**Pruebas mínimas:**
- `FUN-016`, `FUN-020`, `FUN-021`, `FUN-023`, `FUN-025`.
- `REG-005`, `REG-006`.
- Pruebas nuevas para valores legacy.

**Rollback:** con riesgo de datos. Requiere restauración de BD si hubo conversión.

**Regla:** etapa independiente; no se combina con MEJ-013 ni MEJ-012.

---

### Etapa 5 — Impresión base

**MEJ incluida:**
- MEJ-004 — Formato de impresión.

**Objetivo:** estabilizar el formato impreso antes de cambiar códigos o fuentes.

**Gate de entrada:**
- Decisión sobre mantener `PrintDocument/Graphics` o investigar otra arquitectura.
- Layout ticket/carta definido.
- Requisitos de texto largo y paginación definidos.

**Gate de salida:**
- Ticket y carta validados.
- Autoimpresión conserva el formato configurado.
- Logo, condiciones, importes y datos de orden correctos.

**Pruebas mínimas:**
- `FUN-031..FUN-036`.
- `REG-008..REG-011`, `REG-016`.
- Pruebas de papel, textos largos, impresoras y varias páginas.

**Rollback:** simple si se mantiene la arquitectura actual; complejo si se introduce un renderer nuevo.

---

### Etapa 6 — Código y fuentes de impresión

**MEJ incluidas, como commits separados:**
1. MEJ-009 — Código de barras / QR.
2. MEJ-010 — Fuentes nativas.

**Objetivo:** modificar simbología y fuentes sobre un subsistema de impresión estable.

**Gate de entrada:**
- MEJ-004 cerrada.
- Biblioteca y licencia investigadas para MEJ-009.
- Fuentes y tamaños permitidos definidos para MEJ-010.

**Gate de salida:**
- Código generado, impreso y escaneado.
- Fuentes persistidas y aplicadas.
- Ticket y carta siguen siendo legibles.

**Pruebas mínimas:**
- `FUN-006`, `FUN-031..FUN-036`.
- `REG-007..REG-011`, `REG-016`.
- Pruebas de escaneo, DPI, fuentes proporcionales y textos largos.

**Rollback:** individual por commit; puede requerir restaurar DLLs o recursos en MEJ-009.

---

### Etapa 7 — Reportes

**MEJ incluida:**
- MEJ-012 — Reportes.

**Objetivo:** ampliar reportes usando datos y formato ya estabilizados.

**Gate de entrada:**
- Regla de “ingresos adeudados” aprobada.
- MEJ-001 cerrada.
- Estrategia de impresión definida.
- Correspondencia entre filtros, grid y gráficos especificada.

**Gate de salida:**
- Indicador calculado según regla aprobada.
- Reportes respetan filtros.
- Impresión coincide con el reporte mostrado.
- `LIMIT 100` tratado según decisión aprobada.

**Pruebas mínimas:**
- `FUN-037..FUN-040`.
- `REG-012`, `REG-014`.
- Casos de abono cero, parcial, excedente, estados y más de 100 filas.

**Rollback:** simple si no se persisten datos nuevos.

## 4. Estrategia de versionado

Una versión puede contener varias mejoras, pero cada mejora debe mantener su propio commit.

| Versión propuesta | Contenido |
|---|---|
| v4.8.2 | MEJ-005, MEJ-006, MEJ-003, MEJ-007 (commits separados) |
| v4.8.3 | MEJ-001, MEJ-008 (commits separados) |
| v4.8.4 | MEJ-013 exclusivamente |
| v4.8.5 | MEJ-002 exclusivamente |
| v4.8.6 | MEJ-004 exclusivamente |
| v4.8.7 | MEJ-009 y MEJ-010 (commits separados) |
| v4.8.8 | MEJ-012 exclusivamente |

No se reserva versión para MEJ-011.

### Diferencia entre commit y versión

**Commit:**
- Una mejora concreta.
- Pruebas específicas PASS.
- Regresión relacionada PASS.
- Rollback individual.
- Documentación actualizada.

**Versión/tag:**
- Conjunto coherente y estable de commits.
- Compilación PASS.
- Pruebas correspondientes PASS.
- Regresión PASS.
- Revisión funcional aprobada.
- Working tree limpio.
- Tag solo después de aprobación explícita.

No se crearán tags ni releases durante esta fase.

## 5. MEJ-011 — Modo oscuro (postergada)

**Estado:** POSTERGADA / FUERA DEL ALCANCE DE LA LÍNEA 4.8.x.

**Motivo:**
- Alcance transversal sobre todos los formularios.
- Riesgo técnico alto.
- Riesgo de regresión alto.
- Demanda alta.
- Bajo beneficio funcional frente al esfuerzo requerido.

**Reevaluación:** después de completar la migración a .NET 10.

**Evidencia conservada:** las fichas y auditoría de MEJ-011 permanecen intactas en `docs/catalogo_mejoras_net481.md` y `docs/auditoria_mejoras_net481.md` como sustento de esta decisión. No se eliminará ni modificará información de esos documentos históricos/técnicos.

## 6. MEJ-013 — Protección de datos

- Tiene etapa propia (Etapa 3).
- Su gate de salida debe completarse antes de habilitar MEJ-002.
- Backup y restore deben estar probados.
- La BD original debe preservarse.
- Debe decidirse el tratamiento de la carpeta `imagenes/`.
- Nunca debe mezclarse con MEJ-002 antes de superar su gate.

## 7. Gates globales

### GATE A — Decisiones

Decisiones funcionales resueltas antes de implementar:
- MEJ-001: formato monetario y regla de redondeo.
- MEJ-002: catálogo y tratamiento de valores existentes.
- MEJ-012: regla de “ingresos adeudados”.
- MEJ-013: alcance del backup e inclusión de `imagenes/`.
- MEJ-004: conservar `PrintDocument/Graphics` u otra arquitectura.

### GATE B — Investigación técnica

Investigaciones concluidas antes de implementar:
- MEJ-004: motores alternativos de impresión (si aplica).
- MEJ-009: bibliotecas QR/2D, licencia y compatibilidad.
- MEJ-013: backup de SQLite con operaciones activas.
- MEJ-012: correspondencia entre filtros, grid y gráficos.

### GATE C — Protección de datos

MEJ-013 completamente probada antes de MEJ-002.

### GATE D — Regresión por mejora

Prueba específica y regresión relacionada PASS después de cada commit de mejora.

### GATE E — Candidato de versión

Compilación, pruebas, regresión, documentación y rollback verificados antes de declarar una versión estable.

### GATE F — Aprobación del propietario

Revisión funcional del propietario antes de tag o release.

## 8. Criterio de cierre por mejora

Cada MEJ se cierra cuando cumple:

- Implementación limitada al alcance aprobado.
- Prueba específica PASS.
- Regresión relacionada PASS.
- Rollback disponible.
- Documentación actualizada.
- Sin cambios fuera de alcance.
- Decisiones funcionales resueltas.

## 9. Criterio de cierre por versión

Una versión se considera cerrada cuando cumple:

- Compilación PASS.
- Pruebas específicas PASS.
- Regresión PASS.
- Documentación actualizada.
- Rollback verificado.
- Working tree limpio.
- Commits realizados.
- Revisión funcional aprobada.
- Tag únicamente después de aprobación explícita.

## 10. Verificación del roadmap resultante

| Verificación | Resultado |
|---|---|
| Mejoras activas | 12 |
| Mejora postergada | 1 (MEJ-011) |
| Etapas activas | 8 (Etapa 0 a Etapa 7) |
| Versiones propuestas | 7 (v4.8.2 a v4.8.8) |
| Última etapa activa | Etapa 7 — MEJ-012 Reportes |
| MEJ-013 aislada | Sí, Etapa 3 exclusiva |
| MEJ-002 después de MEJ-013 | Sí, Etapa 4 requiere gate de Etapa 3 |
| MEJ-004 antes de MEJ-009 | Sí, Etapas 5 y 6 respectivamente |
| MEJ-012 última mejora funcional activa | Sí |
| MEJ-011 fuera de implementación 4.8.x | Sí |
| Análisis históricos de MEJ-011 conservados | Sí |

## 11. Archivos relacionados

- `docs/baseline_tecnica_net481.md` — baseline técnico (sin modificar).
- `docs/catalogo_mejoras_net481.md` — catálogo de mejoras (sin modificar).
- `docs/auditoria_mejoras_net481.md` — auditoría técnica (sin modificar).
- `docs/testing/matriz_pruebas.md` — matriz de pruebas (sin modificar).
- Documentación histórica de migración a .NET 10 (sin modificar).
