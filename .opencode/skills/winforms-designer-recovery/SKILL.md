---
name: winforms-designer-recovery
description: Recuperar el Diseñador de Windows Forms en formularios .NET Framework decompilados desde VB.NET o procesados mediante ILSpy, preservando la lógica, eventos, controles, recursos y comportamiento funcional. Aplicar una recuperación estructural controlada, reversible y verificable, sin convertirla en una refactorización general.
---

# WinForms Designer Recovery

## 1. Propósito

Esta skill define un procedimiento controlado para recuperar el Diseñador de
Windows Forms de Visual Studio cuando un formulario C# fue obtenido mediante
decompilación, especialmente desde VB.NET con ILSpy u otra herramienta.

El problema objetivo no es "mejorar" el código. El objetivo es reconstruir la
estructura que el Diseñador de Windows Forms espera.

La recuperación debe preservar, salvo que exista evidencia explícita de lo
contrario:

- lógica de negocio;
- flujo de ejecución;
- handlers de eventos;
- nombres de controles;
- tipos de controles;
- propiedades visuales;
- relaciones padre/hijo;
- recursos;
- inicialización;
- orden funcional de inicialización;
- comportamiento del formulario.

La skill debe preferir cambios estructurales mínimos y verificables.

---

# 2. Alcance

Aplicar cuando:

- el proyecto utiliza Windows Forms;
- el proyecto utiliza .NET Framework o una estructura equivalente compatible;
- el formulario compila pero el Diseñador no abre;
- el formulario fue decompilado;
- el formulario procede de VB.NET;
- ILSpy generó patrones incompatibles con el Diseñador de C#;
- existen artefactos como `[AccessedThroughProperty]`;
- existen campos `_Control` y propiedades `Control`;
- existen propiedades `internal virtual`, `protected virtual` o equivalentes
  utilizadas como acceso indirecto a controles;
- `InitializeComponent()` contiene variables locales que funcionan como
  aliases de campos;
- existen declaraciones duplicadas de miembros;
- existen nombres de miembros que interfieren con resolución de controles;
- el `.Designer.cs` no existe, está vacío o no representa correctamente la
  estructura del formulario;
- el Diseñador muestra errores de resolución de campos, propiedades o
  controles.

No asumir que un patrón de decompilación concreto aparece en todos los
formularios.

Cada formulario debe analizarse individualmente.

---

# 3. Principios obligatorios

## 3.1 No modificar antes de diagnosticar

La primera acción debe ser una auditoría.

No comenzar moviendo código, renombrando controles ni eliminando propiedades.

Primero determinar:

1. cuál es la clase del formulario;
2. cuál es su archivo principal;
3. cuál es su `.Designer.cs`, si existe;
4. cuál es su `.resx`, si existe;
5. cuál es el proyecto que lo contiene;
6. qué clase base utiliza;
7. cómo están declarados los controles;
8. cómo se inicializan;
9. cómo se conectan los eventos;
10. qué error presenta el Diseñador;
11. cuál es la causa estructural probable.

Si la causa no puede determinarse razonablemente:

**DETENER.**

No realizar cambios por ensayo y error.

---

## 3.2 Cambios mínimos

La recuperación del Diseñador no debe convertirse en una refactorización.

No realizar como parte automática de esta skill:

- cambios de arquitectura;
- renombrado de controles;
- cambio de nombres públicos;
- cambio de tipos;
- modificación de lógica de negocio;
- optimización;
- limpieza general;
- actualización de .NET;
- migración de paquetes;
- cambios de base de datos;
- modificación de otros formularios;
- cambios de comportamiento no relacionados con el Diseñador.

Si se detecta una mejora adicional, registrarla como tarea separada.

---

## 3.3 Preservar evidencia

Antes de modificar:

- conservar el código original;
- crear backup;
- calcular SHA-256 de los archivos relevantes;
- registrar qué archivos serán modificados;
- registrar el motivo de cada modificación.

Nunca destruir el único original disponible.

---

## 3.4 No confiar únicamente en que compila

La compilación y el Diseñador son validaciones diferentes.

Un formulario puede:

- compilar correctamente;
- ejecutarse correctamente;
- y aun así no ser editable mediante el Diseñador.

Por eso la validación debe incluir tanto compilación como apertura del
Diseñador.

---

# 4. Fase 0 — Inventario y baseline

Antes de modificar, registrar:

- ruta del proyecto;
- `.csproj`;
- `.cs`;
- `.Designer.cs`;
- `.resx`;
- namespace;
- nombre de la clase;
- clase base;
- modificador de la clase;
- presencia de `partial`;
- campos de controles;
- propiedades de acceso a controles;
- constructor;
- `InitializeComponent()`;
- `Dispose(bool)`;
- handlers;
- `+=` de eventos;
- recursos;
- componentes no visuales;
- imágenes, iconos y `ComponentResourceManager`;
- controles creados dinámicamente;
- referencias externas relevantes.

También registrar:

- resultado de la compilación antes del cambio;
- error exacto del Diseñador;
- si el formulario se puede ejecutar;
- si otros formularios del proyecto funcionan.

## Criterio de salida

Debe existir una representación suficiente del estado original para comparar
antes y después.

---
# 5. Modo obligatorio de planificación y aprobación

## Regla fundamental

Cuando esta skill sea ejecutada por un agente de código, el proceso debe
comenzar obligatoriamente en modo de planificación.

Durante la fase de planificación:

- NO modificar archivos.
- NO crear archivos.
- NO eliminar archivos.
- NO ejecutar transformaciones sobre el código.
- NO realizar refactorizaciones.
- NO modificar el `.csproj`.
- NO modificar el `.resx`.

El agente debe limitarse a inspeccionar y analizar el repositorio.

---

## Fase PLAN

Antes de modificar cualquier archivo, el agente debe:

1. Identificar el formulario objetivo.
2. Identificar sus archivos asociados.
3. Analizar el `.cs`.
4. Analizar el `.Designer.cs`, si existe.
5. Analizar el `.resx`, si existe.
6. Analizar el `.csproj`.
7. Analizar `InitializeComponent()`.
8. Identificar controles.
9. Identificar wrappers generados por decompilación.
10. Identificar eventos.
11. Identificar recursos.
12. Identificar dependencias.
13. Comparar con formularios ya recuperados cuando exista un ejemplo
    funcional dentro del mismo proyecto.
14. Determinar la causa raíz.
15. Diseñar la transformación necesaria.

---

## Informe obligatorio del PLAN

Antes de realizar cualquier modificación, el agente debe presentar:

### Formulario objetivo

```text
<nombre>

---

# 6. Backup y rollback

Antes de cualquier modificación estructural:

1. Crear una copia del formulario.
2. Crear una copia del `.Designer.cs`, si existe.
3. Crear una copia del `.resx`, si será tocado.
4. Crear una copia del `.csproj`, si será tocado.
5. Registrar SHA-256.
6. Registrar fecha y estado inicial.

Ejemplo de información que debe quedar registrada:

```text
Formulario: frmEjemplo
Archivo principal: frmEjemplo.cs
Designer: frmEjemplo.Designer.cs
Recursos: frmEjemplo.resx
Estado inicial: compila / no compila
Diseñador: error <mensaje>
Backup: creado
SHA-256: <hash>
```

Si existe Git, preferir un commit de punto de restauración antes de modificar.

---

# 7. Diagnóstico de patrones decompilados

## 7.1 `[AccessedThroughProperty]`

Patrón típico:

```csharp
[AccessedThroughProperty("Panel1")]
private Panel _Panel1;

internal virtual Panel Panel1
{
    get
    {
        return _Panel1;
    }
    [MethodImpl(MethodImplOptions.Synchronized)]
    set
    {
        _Panel1 = value;
    }
}
```

Este patrón puede representar una propiedad generada por la compilación VB.NET
y reconstruida por el decompilador.

No debe conservarse automáticamente como arquitectura del formulario.

Si el objetivo es recuperar el Diseñador, determinar si la propiedad solamente
sirve como envoltorio de un control.

Si no existe lógica adicional en getter/setter, puede ser candidata a
eliminación en favor del campo de control estándar.

---

## 7.2 Campos `_Control` + propiedades `Control`

Patrón:

```csharp
private Button _btnAceptar;

internal virtual Button btnAceptar
{
    get { return _btnAceptar; }
    set { _btnAceptar = value; }
}
```

Si la propiedad solamente devuelve y asigna el campo, el patrón debe tratarse
como un artefacto de decompilación.

La estructura esperada normalmente es:

```csharp
private Button btnAceptar;
```

El cambio solo es válido después de comprobar todas las referencias.

### Obligación

Buscar todas las referencias a:

- `_btnAceptar`;
- `btnAceptar`;
- getter;
- setter;
- asignaciones;
- eventos;
- métodos;
- expresiones indirectas.

No eliminar una propiedad sin comprobar su uso.

---

## 7.3 Propiedades con lógica adicional

No eliminar automáticamente una propiedad si contiene:

- validaciones;
- efectos secundarios;
- suscripción o desuscripción de eventos;
- creación de objetos;
- sincronización de estado;
- llamadas a otros métodos;
- modificaciones de otros controles;
- lógica que no sea simple acceso al campo.

En ese caso, primero documentar la dependencia y determinar una estrategia
específica.

---

# 8. Diagnóstico de `InitializeComponent()`

`InitializeComponent()` es el núcleo del diseño serializado.

Debe revisarse completo.

Buscar:

- declaraciones locales de controles;
- aliases;
- instancias;
- asignaciones de campos;
- propiedades duplicadas;
- `Controls.Add`;
- `SuspendLayout`;
- `ResumeLayout`;
- `PerformLayout`;
- eventos;
- recursos;
- `ComponentResourceManager`;
- `resources.GetObject`;
- `resources.GetString`;
- `BeginInit`;
- `EndInit`;
- `Dispose`;
- componentes no visuales.

---

## 8.1 Variables locales que actúan como aliases

Patrón problemático:

```csharp
Panel panel = this.Panel1;
panel.Location = new Point(...);
panel.Size = new Size(...);
```

Si `panel` solamente es un alias de `this.Panel1`, no debe conservarse
innecesariamente.

La transformación típica es:

```csharp
this.Panel1.Location = new Point(...);
this.Panel1.Size = new Size(...);
```

Pero no eliminar la variable si representa un objeto distinto o si su
identidad tiene importancia.

---

## 8.2 Instanciación de controles

Debe existir una relación clara entre:

1. campo;
2. instancia;
3. configuración;
4. inserción en el contenedor.

Estructura esperada:

```csharp
this.btnAceptar = new Button();
this.btnAceptar.Location = new Point(...);
this.btnAceptar.Name = "btnAceptar";
this.btnAceptar.Size = new Size(...);
this.Controls.Add(this.btnAceptar);
```

Evitar estructuras donde el Diseñador no pueda resolver el campo.

---

# 9. Hito 0 — Estructura `partial`

Antes de realizar un split grande:

1. determinar el namespace exacto;
2. determinar la clase base;
3. convertir la clase en `partial`;
4. crear el `.Designer.cs` si no existe;
5. verificar que ambas partes utilizan el mismo:
   - namespace;
   - nombre de clase;
   - modificador de acceso compatible;
   - clase `partial`.

Ejemplo:

```csharp
namespace ServicioTecnico;

public partial class frmEjemplo : Form
{
}
```

Y:

```csharp
namespace ServicioTecnico;

partial class frmEjemplo
{
}
```

No mover todavía todo el contenido.

## Criterio de salida

El proyecto debe reconocer ambas partes como una única clase.

---

# 10. Estructura objetivo del formulario

Cuando la recuperación requiera separación, utilizar esta organización:

```text
frmEjemplo.cs
frmEjemplo.Designer.cs
frmEjemplo.resx
```

## `frmEjemplo.cs`

Debe contener principalmente:

- lógica de negocio;
- constructor;
- handlers;
- métodos propios;
- validaciones;
- operaciones funcionales.

## `frmEjemplo.Designer.cs`

Debe contener principalmente:

- campos de controles;
- componentes;
- `InitializeComponent()`;
- `Dispose(bool)` cuando corresponda;
- código generado relacionado con el diseño.

No mover lógica de negocio al `.Designer.cs`.

---

# 11. Migración de campos de controles

Los campos que representan controles visuales deben quedar disponibles para
el Diseñador.

Ejemplo:

```csharp
private System.Windows.Forms.Button btnAceptar;
private System.Windows.Forms.Panel panelPrincipal;
private System.Windows.Forms.TextBox txtNombre;
```

Mantener los nombres originales.

No cambiar:

```text
txtNombre
```

por:

```text
textBox1
```

solo para "normalizar".

---

## 11.1 Controles con modificador especial

No cambiar indiscriminadamente:

```csharp
private
protected
internal
public
```

La visibilidad forma parte del comportamiento potencial del código.

Si el Diseñador requiere una estructura diferente, justificar el cambio y
comprobar todas las referencias.

---

# 12. Eliminación controlada de wrappers

Cuando exista:

```csharp
private Button _button1;

internal virtual Button Button1
{
    get { return _button1; }
    set { _button1 = value; }
}
```

seguir:

1. buscar todas las referencias a `Button1`;
2. buscar todas las referencias a `_button1`;
3. determinar si la propiedad se utiliza fuera del formulario;
4. comprobar si getter/setter tienen lógica;
5. reemplazar referencias solo si es seguro;
6. convertir el campo en el miembro utilizado por el Diseñador;
7. eliminar el wrapper únicamente después de validar.

Nunca realizar una sustitución global sin analizar contexto.

---

# 13. Eventos

Los eventos son funcionalidad, no decoración.

Preservar:

```csharp
this.btnAceptar.Click += this.btnAceptar_Click;
```

y handlers como:

```csharp
private void btnAceptar_Click(object sender, EventArgs e)
{
    ...
}
```

No eliminar eventos solo porque estén en `InitializeComponent()`.

---

## 13.1 Eventos duplicados

Buscar:

- `+=` duplicados;
- `-=` innecesarios;
- suscripciones en constructor;
- suscripciones en `InitializeComponent()`;
- suscripciones dentro de propiedades decompiladas.

Un mismo evento no debe quedar conectado dos veces como consecuencia de la
recuperación.

---

## 13.2 Eventos generados por el Diseñador

Cuando el patrón sea el estándar de WinForms, mantenerlo en
`InitializeComponent()`.

No trasladar automáticamente eventos al constructor.

---

# 14. `Dispose(bool)`

Revisar el método de disposición.

Patrón habitual:

```csharp
protected override void Dispose(bool disposing)
{
    if (disposing && (components != null))
    {
        components.Dispose();
    }

    base.Dispose(disposing);
}
```

No reemplazarlo ciegamente.

Comprobar:

- componentes no visuales;
- objetos IDisposable;
- recursos;
- código adicional generado por el decompilador.

Si existe lógica adicional, determinar si es funcional antes de eliminarla.

---

# 15. Recursos `.resx`

Los recursos forman parte del formulario.

No eliminar ni regenerar el `.resx` sin comprobar:

- imágenes;
- iconos;
- textos;
- cadenas;
- localización;
- `ImageList`;
- `Icon`;
- `BackgroundImage`;
- propiedades obtenidas mediante `resources.GetObject`;
- propiedades obtenidas mediante `resources.GetString`.

Si `InitializeComponent()` utiliza:

```csharp
ComponentResourceManager resources =
    new ComponentResourceManager(typeof(frmEjemplo));
```

debe mantenerse la coherencia entre:

```text
namespace
clase
resx
csproj
```

---

# 16. `InitializeComponent()` y recursos

Preservar el orden lógico de operaciones cuando sea relevante.

Especialmente:

```csharp
this.SuspendLayout();
...
this.Controls.Add(...);
...
this.ResumeLayout(false);
this.PerformLayout();
```

No mover arbitrariamente estas instrucciones.

Con controles que implementan `ISupportInitialize`, preservar:

```csharp
((ISupportInitialize)this.control).BeginInit();
...
((ISupportInitialize)this.control).EndInit();
```

si forman parte de la inicialización original.

---

# 17. Controles anidados

Auditar la jerarquía:

```text
Form
 ├── Panel
 │    ├── Label
 │    └── TextBox
 └── Button
```

La recuperación debe conservar:

- contenedor;
- orden;
- relaciones padre/hijo;
- `Controls.Add`;
- `Controls.SetChildIndex`, si existe;
- docking;
- anchoring.

Un formulario que abre en el Diseñador pero cambia la jerarquía visual no se
considera correctamente recuperado.

---

# 18. Propiedades visuales

Preservar, cuando existan:

- `Location`;
- `Size`;
- `Name`;
- `Text`;
- `TabIndex`;
- `TabStop`;
- `Anchor`;
- `Dock`;
- `Font`;
- `ForeColor`;
- `BackColor`;
- `AutoSize`;
- `Enabled`;
- `Visible`;
- `RightToLeft`;
- `UseVisualStyleBackColor`;
- `FlatStyle`;
- `Image`;
- `ImageIndex`;
- `BackgroundImage`;
- `BackgroundImageLayout`;
- `MinimumSize`;
- `MaximumSize`;
- `Margin`;
- `Padding`;
- `Cursor`;
- `AccessibleName`;
- `AccessibleDescription`;
- propiedades específicas del control.

No "limpiar" propiedades porque parezcan redundantes.

---

# 19. Nombres y resolución de miembros

El Diseñador debe poder resolver cada control como miembro de la clase.

Ante errores como:

```text
'Form' no contiene una definición para 'Panel1'
```

comprobar:

1. que existe el campo;
2. que pertenece a la clase correcta;
3. que está en una parte `partial` de la misma clase;
4. que el namespace coincide;
5. que el tipo es correcto;
6. que no existe una propiedad conflictiva;
7. que no existe otro miembro con el mismo nombre;
8. que el `.Designer.cs` está incluido en el proyecto.

---

# 20. Declaraciones duplicadas

Buscar duplicaciones de:

- campos;
- propiedades;
- métodos;
- clases parciales;
- nombres de controles;
- variables locales;
- miembros heredados ocultados accidentalmente.

Ejemplo problemático:

```csharp
private Point location;
private Point location;
```

o:

```csharp
private Button button1;
internal Button button1 { get; set; }
```

No resolver automáticamente renombrando.

Primero determinar qué declaración corresponde al modelo original.

---

# 21. Inclusión de archivos en el proyecto

En proyectos antiguos de .NET Framework, revisar el `.csproj`.

Comprobar que:

```text
Form.cs
Form.Designer.cs
Form.resx
```

están correctamente asociados.

En proyectos con estructura tradicional pueden existir relaciones como:

```xml
<Compile Include="frmEjemplo.cs">
  <SubType>Form</SubType>
</Compile>

<Compile Include="frmEjemplo.Designer.cs">
  <DependentUpon>frmEjemplo.cs</DependentUpon>
</Compile>

<EmbeddedResource Include="frmEjemplo.resx">
  <DependentUpon>frmEjemplo.cs</DependentUpon>
</EmbeddedResource>
```

No insertar estas entradas si el tipo de proyecto utiliza otra convención.

Primero identificar el formato real del `.csproj`.

---

# 22. Namespace y clase

Las dos partes deben pertenecer exactamente a la misma clase.

Ejemplo:

```csharp
namespace MiAplicacion
{
    public partial class frmEjemplo : Form
    {
    }
}
```

y:

```csharp
namespace MiAplicacion
{
    partial class frmEjemplo
    {
    }
}
```

Errores de namespace provocan síntomas similares a campos inexistentes.

---

# 23. Herencia

Comprobar la clase base original:

```csharp
public partial class frmEjemplo : Form
```

No cambiarla a otra clase únicamente para hacer funcionar el Diseñador.

Si hereda de un formulario base:

```csharp
public partial class frmEjemplo : frmBase
```

debe analizarse también:

- accesibilidad;
- constructor;
- inicialización;
- controles heredados;
- disponibilidad del diseñador del formulario base.

---

# 24. Formularios heredados

Si el formulario depende de otro formulario:

1. identificar la clase base;
2. comprobar que la clase base compila;
3. comprobar que sus controles son accesibles;
4. comprobar que el diseñador puede resolver la herencia;
5. no duplicar controles heredados en el formulario derivado.

No convertir automáticamente un formulario heredado en un formulario
independiente.

---

# 25. Constructor

El constructor debe conservar la inicialización funcional.

Patrón esperado:

```csharp
public frmEjemplo()
{
    InitializeComponent();
}
```

Si el constructor original contiene lógica adicional:

```csharp
public frmEjemplo()
{
    InitializeComponent();
    CargarDatos();
}
```

no eliminarla.

No trasladar lógica de negocio al `.Designer.cs`.

---

# 26. Orden de inicialización

El orden puede ser funcional.

Comparar:

1. constructor;
2. `InitializeComponent()`;
3. suscripciones;
4. carga de datos;
5. inicialización de servicios;
6. eventos de formulario.

No cambiar el orden salvo que exista una razón estructural necesaria y
documentada.

---

# 27. Transformación segura

La transformación debe hacerse en etapas.

## Etapa A — Estructura

- `partial`;
- `.Designer.cs`;
- archivos del proyecto.

## Etapa B — Campos

- identificar controles;
- mover campos;
- eliminar wrappers únicamente cuando sea seguro.

## Etapa C — Inicialización

- reconstruir `InitializeComponent()`;
- corregir aliases;
- conservar propiedades.

## Etapa D — Eventos

- comprobar handlers;
- comprobar suscripciones.

## Etapa E — Recursos

- comprobar `.resx`;
- iconos;
- imágenes;
- recursos localizados.

## Etapa F — Validación

- compilar;
- abrir Diseñador;
- inspeccionar visualmente;
- comprobar ejecución.

No realizar todas las etapas simultáneamente sin puntos de control.

---

# 28. Reglas para sustituciones de código

No usar reemplazos ciegos de texto para cambios estructurales complejos.

Antes de modificar una referencia:

1. conocer el símbolo;
2. conocer su alcance;
3. comprobar referencias;
4. comprobar si existe sombra de nombres;
5. comprobar comentarios y strings;
6. modificar el código;
7. compilar.

Preferir herramientas de análisis sintáctico o IDE cuando estén disponibles.

Si se utiliza un script textual, debe:

- crear backup;
- registrar archivos modificados;
- ser idempotente;
- fallar si encuentra un patrón inesperado;
- no sobrescribir silenciosamente estructuras desconocidas.

---

# 29. Idempotencia

Una transformación ya aplicada no debe volver a alterar el formulario.

Por ejemplo, si ya existe:

```csharp
partial class frmEjemplo
```

no volver a insertar `partial`.

Si ya existe un `.Designer.cs` correcto, no regenerarlo sin motivo.

Si el script encuentra una estructura diferente de la esperada:

**DETENER.**

No intentar adivinar.

---

# 30. Validación estructural

Después de cada etapa revisar:

- existe exactamente una definición lógica de cada control;
- no existen wrappers huérfanos;
- no existen referencias a campos eliminados;
- no existen referencias a propiedades eliminadas;
- `InitializeComponent()` utiliza miembros existentes;
- todos los eventos apuntan a métodos existentes;
- no existen duplicados;
- `.Designer.cs` pertenece a la misma clase.

---

# 31. Validación de compilación

Ejecutar la compilación del proyecto utilizando el mecanismo real del proyecto.

Por ejemplo, según corresponda:

```text
MSBuild
dotnet build
Visual Studio
```

No asumir que `dotnet build` es válido para cualquier proyecto .NET Framework
antiguo.

Registrar:

- comando utilizado;
- resultado;
- errores;
- warnings relevantes.

No considerar recuperado el formulario si existen errores de compilación
relacionados con la transformación.

---

# 32. Validación del Diseñador

La prueba principal es:

1. abrir Visual Studio;
2. abrir el formulario;
3. seleccionar "Diseñador";
4. esperar la carga completa;
5. comprobar que no aparece una excepción;
6. comprobar que todos los controles son visibles;
7. seleccionar controles;
8. comprobar la ventana de propiedades;
9. mover o seleccionar un control;
10. guardar si corresponde;
11. comprobar que Visual Studio no destruye o altera inesperadamente la
   estructura.

Un Diseñador que simplemente abre pero pierde controles o propiedades no pasa
la validación.

---

# 33. Validación funcional

Después de recuperar el Diseñador:

1. compilar;
2. ejecutar;
3. abrir el formulario;
4. probar controles principales;
5. probar eventos;
6. comprobar carga de datos;
7. comprobar navegación;
8. comprobar cierre;
9. comprobar recursos visuales.

La recuperación estructural no debe haber alterado el comportamiento.

---

# 34. Comparación antes/después

Comparar el formulario original con el recuperado.

Verificar especialmente:

- número de controles;
- nombres;
- tipos;
- jerarquía;
- tamaños;
- posiciones;
- textos;
- eventos;
- imágenes;
- iconos;
- recursos;
- lógica del constructor;
- métodos funcionales.

No buscar solamente diferencias textuales.

Una diferencia textual puede ser estructuralmente necesaria.

Una igualdad textual no garantiza comportamiento equivalente.

---

# 35. Criterios de aceptación

La recuperación se considera exitosa únicamente si:

- [ ] el proyecto compila;
- [ ] el formulario abre en el Diseñador;
- [ ] no aparecen errores de resolución de controles;
- [ ] todos los controles esperados están presentes;
- [ ] los controles conservan sus nombres;
- [ ] los controles conservan sus tipos;
- [ ] la jerarquía se conserva;
- [ ] las propiedades visuales principales se conservan;
- [ ] los eventos se conservan;
- [ ] los recursos se conservan;
- [ ] el constructor conserva su comportamiento;
- [ ] el formulario ejecuta correctamente;
- [ ] no quedan referencias a miembros eliminados;
- [ ] no quedan wrappers decompilados innecesarios;
- [ ] no se modificaron otros formularios sin necesidad;
- [ ] existe backup o commit de restauración.

---

# 36. Criterios de rechazo

No considerar terminada la recuperación si:

- el Diseñador sigue sin abrir;
- el Diseñador abre con controles faltantes;
- se perdieron eventos;
- se cambiaron nombres sin necesidad;
- se perdieron recursos;
- el formulario compila pero lanza errores al abrir;
- aparecen referencias a miembros inexistentes;
- existen declaraciones duplicadas;
- el `.resx` dejó de estar asociado;
- el `.Designer.cs` no pertenece a la clase correcta;
- se realizaron cambios de lógica no justificados;
- no existe un punto de rollback.

---

# 37. Errores comunes que deben evitarse

## 37.1 Crear un Designer vacío y considerar terminado el trabajo

Incorrecto.

El `.Designer.cs` debe contener la estructura real del formulario.

---

## 37.2 Copiar todo el `.cs` al `.Designer.cs`

Incorrecto.

La lógica de negocio y los handlers deben permanecer en el archivo principal.

---

## 37.3 Eliminar todas las propiedades virtuales

Incorrecto.

Solo eliminar wrappers que hayan sido identificados como artefactos de
decompilación y cuya eliminación sea segura.

---

## 37.4 Renombrar controles

Incorrecto salvo necesidad demostrada.

Los nombres pueden estar utilizados por lógica, eventos, recursos u otros
formularios.

---

## 37.5 Regenerar el formulario desde cero

No hacerlo como primera estrategia.

La regeneración puede perder:

- propiedades;
- eventos;
- recursos;
- orden;
- configuraciones específicas;
- comportamiento.

Solo considerar una reconstrucción completa si la estructura original es
irrecuperable y existe una especificación suficiente para reproducirla.

---

## 37.6 Modificar lógica de negocio durante la recuperación

Incorrecto.

Separar:

```text
Recuperación del Diseñador
```

de:

```text
Refactorización / mantenimiento
```

---

# 38. Estrategia de reconstrucción cuando el Designer está muy dañado

Si el `.Designer.cs` es inexistente o inutilizable:

1. conservar el formulario original;
2. identificar todos los controles;
3. identificar tipos;
4. identificar nombres;
5. identificar propiedades;
6. identificar jerarquía;
7. identificar eventos;
8. identificar recursos;
9. reconstruir el `.Designer.cs`;
10. comparar contra el comportamiento original;
11. compilar;
12. probar el Diseñador;
13. probar ejecución.

La reconstrucción debe partir del código existente y no de una plantilla
genérica.

---

# 39. Inventario de controles

Antes de reconstruir, generar una tabla lógica:

| Campo | Tipo | Nombre | Contenedor | Eventos | Recursos |
|---|---|---|---|---|---|
| btnAceptar | Button | btnAceptar | Form | Click | No |
| txtNombre | TextBox | txtNombre | panelDatos | TextChanged | No |
| panelDatos | Panel | panelDatos | Form | — | No |

Esta tabla permite detectar pérdidas durante la transformación.

---

# 40. Inventario de eventos

Registrar:

| Control | Evento | Handler |
|---|---|---|
| btnAceptar | Click | btnAceptar_Click |
| txtNombre | TextChanged | txtNombre_TextChanged |
| Form | Load | frmEjemplo_Load |

Después de la recuperación, comparar esta tabla.

---

# 41. Inventario de recursos

Registrar:

| Recurso | Tipo | Uso |
|---|---|---|
| icono | Icon | Form.Icon |
| logo | Image | PictureBox.Image |
| texto | String | Label.Text |

No eliminar un recurso porque aparentemente no se utilice desde código normal.
Puede ser consumido mediante `ComponentResourceManager`.

---

# 42. Control de cambios

Cada modificación estructural debe poder explicarse.

Ejemplo:

```text
Cambio 001
Archivo: frmEjemplo.cs
Motivo: eliminar wrapper decompilado Button1
Causa: propiedad únicamente devuelve _Button1
Riesgo: bajo
Validación: referencias revisadas + compilación
```

No registrar cambios irrelevantes.

---

# 43. Procedimiento operativo completo

Aplicar exactamente esta secuencia:

### Paso 1
Identificar el formulario y sus archivos asociados.

### Paso 2
Crear backup o commit de restauración.

### Paso 3
Registrar baseline de compilación y error del Diseñador.

### Paso 4
Analizar namespace, clase base y estructura `partial`.

### Paso 5
Analizar campos y propiedades decompiladas.

### Paso 6
Analizar `InitializeComponent()` completo.

### Paso 7
Analizar eventos.

### Paso 8
Analizar recursos.

### Paso 9
Determinar la causa raíz.

### Paso 10
Crear o corregir `.Designer.cs`.

### Paso 11
Convertir la clase a `partial` si corresponde.

### Paso 12
Mover o reconstruir únicamente los miembros de diseño.

### Paso 13
Eliminar wrappers únicamente después de comprobar referencias.

### Paso 14
Corregir aliases y referencias de controles.

### Paso 15
Preservar eventos.

### Paso 16
Preservar recursos.

### Paso 17
Revisar `.csproj` si la asociación de archivos lo requiere.

### Paso 18
Compilar.

### Paso 19
Abrir el Diseñador.

### Paso 20
Comparar visualmente y estructuralmente.

### Paso 21
Ejecutar el formulario.

### Paso 22
Probar eventos principales.

### Paso 23
Registrar resultado.

### Paso 24
Si falla una etapa, volver al último punto estable.

---

# 44. Árbol de decisión

```text
¿El Diseñador no abre?
        |
        v
¿Compila el proyecto?
   |            |
  NO           SÍ
   |            |
Corregir       Analizar estructura
compilación          |
                     v
              ¿Existe Designer.cs?
                |          |
               NO         SÍ
                |          |
          Reconstruir    Auditar
                |          |
                +----+-----+
                     |
                     v
            ¿Hay wrappers VB/decompilador?
                 |          |
                SÍ          NO
                 |          |
          Analizar uso     Continuar
                 |          |
                 +----+-----+
                      |
                      v
             ¿InitializeComponent()
                resuelve todos
                los miembros?
                 |          |
                NO         SÍ
                 |          |
             Corregir    Validar
                 |          |
                 +----+-----+
                      |
                      v
                 Compilar
                      |
                      v
              Abrir Diseñador
                      |
             +--------+--------+
             |                 |
            FALLA            FUNCIONA
             |                 |
       rollback /             Probar
       diagnóstico            ejecución
```

---

# 45. Política de parada

La skill debe detener el proceso cuando:

- aparece una estructura no prevista;
- una propiedad tiene lógica desconocida;
- una transformación puede cambiar comportamiento;
- no se puede determinar el propietario de un control;
- existen dependencias externas no verificadas;
- el proyecto no permite determinar qué archivo es el Designer;
- el `.resx` está corrupto o ambiguo;
- el `.csproj` utiliza una estructura no comprendida;
- la compilación falla por una causa ajena y no puede aislarse el cambio;
- el Diseñador modifica automáticamente el código de manera inesperada.

En estos casos no continuar acumulando cambios.

Registrar:

```text
ESTADO: DETENIDO
CAUSA:
ARCHIVOS AFECTADOS:
ÚLTIMO ESTADO VÁLIDO:
SIGUIENTE ACCIÓN PROPUESTA:
```

---

# 46. Uso con agentes de código

## Regla de ejecución

El agente debe comenzar siempre en modo PLAN.

La planificación debe ser presentada al usuario y aprobada explícitamente
antes de modificar cualquier archivo.

La aprobación del PLAN no autoriza cambios fuera del alcance definido.

Si durante la implementación aparece una situación no contemplada, el agente
debe detenerse y solicitar una nueva aprobación.

Cuando esta skill sea utilizada por un agente como OpenCode:

1. El agente debe leer primero la skill completa.
2. Debe inspeccionar el repositorio antes de modificar.
3. Debe identificar el formulario objetivo.
4. Debe informar el diagnóstico antes de ejecutar una transformación grande.
5. Debe crear un punto de rollback.
6. Debe aplicar cambios pequeños.
7. Debe validar después de cada etapa.
8. Debe mostrar los archivos modificados.
9. Debe informar los errores de compilación.
10. No debe modificar formularios no relacionados.

El agente no debe interpretar:

```text
"recuperar el diseñador"
```

como:

```text
"refactorizar el formulario"
```

Son tareas diferentes.

---

# 47. Formato de informe final

Al finalizar, producir un informe breve:

```text
FORMULARIO:
<nombre>

CAUSA RAÍZ:
<descripción>

ARCHIVOS MODIFICADOS:
- <archivo>
- <archivo>

CAMBIOS:
- <cambio>
- <cambio>

CAMBIOS NO REALIZADOS:
- <detalle>

COMPILACIÓN:
OK / FALLÓ

DISEÑADOR:
OK / FALLÓ

EJECUCIÓN:
OK / FALLÓ

RECURSOS:
OK / REVISAR

EVENTOS:
OK / REVISAR

ROLLBACK:
<commit o backup>

ESTADO FINAL:
RECUPERADO / DETENIDO
```

---

# 48. Checklist final

## Diagnóstico

- [ ] Formulario identificado.
- [ ] `.cs` identificado.
- [ ] `.Designer.cs` identificado o ausencia confirmada.
- [ ] `.resx` identificado o ausencia confirmada.
- [ ] `.csproj` identificado.
- [ ] Error del Diseñador registrado.
- [ ] Causa raíz identificada.

## Seguridad

- [ ] Backup realizado.
- [ ] Punto de rollback disponible.
- [ ] SHA-256 registrado cuando corresponda.

## Estructura

- [ ] Clase correctamente marcada como `partial`.
- [ ] Namespace coincide.
- [ ] Clase base coincide.
- [ ] `.Designer.cs` pertenece a la clase correcta.
- [ ] Archivos están incluidos correctamente en el proyecto.

## Controles

- [ ] Todos los controles identificados.
- [ ] Campos únicos.
- [ ] Tipos correctos.
- [ ] Nombres preservados.
- [ ] Jerarquía preservada.
- [ ] Propiedades principales preservadas.

## Código decompilado

- [ ] `[AccessedThroughProperty]` revisado.
- [ ] Wrappers `_Control` / `Control` revisados.
- [ ] Aliases locales revisados.
- [ ] Duplicados eliminados solo cuando corresponde.
- [ ] No quedan referencias a miembros eliminados.

## Inicialización

- [ ] `InitializeComponent()` válido.
- [ ] `SuspendLayout()` / `ResumeLayout()` preservados cuando corresponda.
- [ ] `BeginInit()` / `EndInit()` preservados cuando corresponda.
- [ ] `Controls.Add()` correcto.
- [ ] Orden de inicialización revisado.

## Eventos

- [ ] Handlers preservados.
- [ ] Suscripciones preservadas.
- [ ] No hay eventos duplicados.
- [ ] Métodos referenciados existen.

## Recursos

- [ ] `.resx` preservado.
- [ ] Iconos revisados.
- [ ] Imágenes revisadas.
- [ ] Recursos localizados revisados.
- [ ] `ComponentResourceManager` coherente.

## Validación

- [ ] Compilación exitosa.
- [ ] Diseñador abre.
- [ ] Controles visibles.
- [ ] Propiedades accesibles.
- [ ] Formulario ejecuta.
- [ ] Eventos principales funcionan.
- [ ] No se alteró lógica no relacionada.
- [ ] Informe final generado.

---

# 49. Regla principal

La recuperación correcta no se mide por cuánto código fue cambiado.

Se mide por si se consiguió reconstruir una estructura de Windows Forms que:

1. Visual Studio puede interpretar;
2. el Diseñador puede abrir y editar;
3. conserva los controles y recursos originales;
4. conserva los eventos;
5. conserva el comportamiento funcional;
6. compila;
7. ejecuta;
8. y puede revertirse si algo falla.

**Primero diagnosticar.  
Después modificar.  
Después compilar.  
Después abrir el Diseñador.  
Después validar comportamiento.**

Nunca invertir ese orden.
