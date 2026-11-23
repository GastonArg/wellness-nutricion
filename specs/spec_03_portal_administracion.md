# Spec 03 — Portal de Administración: gestión de clientes, pago y seguimiento manual

**Versión:** 1.2 · **Prototipo base:** `Administracion_WN (6).html` · **Modelo:** `modelo_datos_V5.0.md` · **Reemplaza a:** `Product_Spec_Administracion_WN` y a las versiones anteriores de la spec del Portal

## Necesidad que se quiere resolver

Las administradoras necesitan un lugar único donde ver y gestionar a todos los clientes (los que llegaron por el test web y los que cargan a mano), registrar el pago, controlar el estado del programa y llevar el seguimiento de acciones (mails, consultas, encuentros, informe, certificado) sin planillas ni memoria.

## Contexto de uso

- Usuarias: **Sofía** (Lic. Alurralde, MP 1440/2) y **Marilina** (Lic. Borras, MP 1512/2), desde notebook y a veces celular.
- Volumen inicial bajo (decenas de clientes); uso diario.
- Todo es **manual** en la Etapa 1: no hay pasarela de pago ni envío automático de mails. El Portal **registra** lo que el equipo hace.

## Objetivo

Que las administradoras vean, en una sola grilla y en menos de dos clics, el estado, el plan, el pago, la última y la próxima acción de cada cliente; y que puedan dar de alta, editar, registrar pagos, cambiar estados y registrar acciones con trazabilidad completa.

## Alcance

### Incluye
- **Acceso** restringido al equipo y registro de quién actúa.
- **Panel de clientes**: indicadores (Total registrados, Programa activo, Inscriptos sin iniciar, Pago pendiente), búsqueda por nombre, filtros por estado y por pago pendiente, y grilla.
- **Alta manual** de un cliente en cualquier estado inicial.
- **Clientes que vienen del test web**: aparecen automáticamente como Inscripto, con fenotipo, plan elegido (si lo eligió) y origen; se destacan como "Nuevos desde la web" hasta que alguien los revise.
- **Detalle** (solo lectura) y **edición** en un modal con dos solapas: **Principal** e **Historial**.
- **Registro manual de pago**: marca de pago recibido y fecha de pago (sin monto ni comprobante; ver nota 5).
- **Gestión de estados** con reglas y diagrama del flujo.
- **Seguimiento de acciones**: última acción, próxima acción con fecha programada, "Marcar como realizada" e historial completo.
- **Auditoría**: lista de actividad reciente con autor, qué cambió y cuándo.
- **Eliminación** de un cliente (con confirmación).
- Catálogos administrables en etapa 1 solo por carga inicial (planes, acciones, estados).

### No incluye
- Registro de pagos múltiples o por cuotas, conciliación bancaria, facturación.
- Envío de mails desde el sistema ni plantillas de mail.
- Recordatorios o notificaciones automáticas por acciones vencidas.
- Roles y permisos diferenciados (las dos administradoras tienen los mismos).
- Exportación, reportes e importación masiva.
- Edición del catálogo de preguntas del test o del contenido de la landing.
- Área para el cliente.

## Comportamiento esperado

### Flujo principal

**A. Ingreso**
1. La administradora inicia sesión. Ve el panel con los indicadores y la grilla de clientes ordenada por fecha de inscripción (más recientes primero).

**B. Grilla**
2. Columnas: **Cliente** (nombre y fenotipo), **Email**, **Tel**, **Plan**, **Estado visible**, **Última acción** (tipo y fecha), **Próxima acción**, **Fecha Programada** y **menú de acciones** (⋮: Ver detalle, Editar). Las columnas se muestran siempre, aun sin registros.
3. **Estado visible** (derivado): *Pago pendiente*, *Pago realizado*, *Programa en proceso*, *Programa finalizado*, según pago y estado (ver reglas).
4. Puede buscar por nombre y filtrar por estado (Inscripto, Activo, Pausado, Finalizado) o por "Pago pendiente".

**C. Alta manual**
5. Hace clic en **+ Agregar persona**. Completa nombre, correo y teléfono (obligatorios), fenotipo, plan, estado inicial (libre), fecha de inscripción (hoy por defecto) y notas. Guarda.
6. El sistema crea el cliente con `Origen = manual` y registra "Persona agregada (estado inicial: X)".

**D. Cliente que llegó por la web**
7. Ve al cliente nuevo en la grilla con la etiqueta "Nuevo desde la web". Abre el detalle, revisa sus datos y su fenotipo, completa lo que falte y, si corresponde, lo contacta. La etiqueta se quita al abrirlo y guardar o marcarlo como revisado.

**E. Registro de pago y activación**
8. La persona avisa que pagó (comprobante por el medio que use el equipo). La administradora abre **Editar**, activa **Pago recibido** e indica la **fecha de pago** (por defecto hoy).
9. Cambia el estado de **Inscripto** a **Activo**. El sistema permite el cambio porque hay pago y registra: "Cambió estado: Inscripto → Activo (Pago confirmado)".
10. La grilla pasa a mostrar "Programa en proceso".

**F. Seguimiento de acciones**
11. En **Principal → Seguimiento de acciones** programa la **próxima acción** (de 12 tipos) y su **fecha programada**. La grilla muestra ambas.
12. Cuando la realiza, abre **Editar**, usa **✓ Marcar como realizada**, indica la fecha real (por defecto hoy) y guarda. La acción pasa a **última acción** y se libera la próxima para programar otra.
13. En la solapa **Historial** ve la lista completa de acciones realizadas (tipo, fecha realizada, fecha programada, quién la registró), la acción programada pendiente y, debajo, la **actividad reciente** (auditoría).

**G. Ciclo de vida**
14. Cambios de estado: Activo ⇄ Pausado (solicita pausa / retoma), Activo → Finalizado (completa las 4 semanas), Pausado → Finalizado (decide no continuar o no retoma en el plazo). Cada cambio guarda su evento en la auditoría.
15. Puede abrir **Ver flujo de estados** para consultar las transiciones y sus eventos.

### Reglas de negocio

| Regla | Detalle |
|---|---|
| **Activación** | No se puede pasar a **Activo** si `PagoRecibido` es falso. Es un bloqueo con mensaje, no una excepción |
| **Transiciones estándar** | Inscripto→Activo (pago confirmado); Activo→Pausado (solicita pausa); Pausado→Activo (retoma); Activo→Finalizado (completa las 4 semanas); Pausado→Finalizado (no continúa) |
| **Excepciones** | Inscripto→Finalizado (baja sin haber pagado) y cualquier otro cambio fuera del flujo (por ejemplo reabrir un Finalizado) piden **confirmación explícita** y quedan auditados |
| **Estado visible** | Sin pago ⇒ Pago pendiente; con pago + Inscripto ⇒ Pago realizado; con pago + Activo o Pausado ⇒ Programa en proceso; con pago + Finalizado ⇒ Programa finalizado |
| **Una acción pendiente** | Cada cliente tiene como máximo una acción programada sin realizar |
| **Última acción** | La de `FechaRealizada` más reciente |
| **Fecha de pago** | Se exige si hay pago recibido (hoy por defecto); se borra si se desmarca el pago |
| **Plan** | Un solo plan por cliente; el cambio queda auditado |
| **Auditoría** | Se registran: alta, pago marcado/desmarcado, cambio de estado, cambio de plan y cada acción realizada |
| **Alta manual** | Permite elegir libremente el estado inicial |

### Catálogo de acciones

Mail de Bienvenida · Mail programa completo · Mail con Menu · Consulta Recibida · Mail de Respuesta a Consulta · Mail Semana 1 · Mail Semana 2 · Mail Semana 3 · Mail Semana 4 · Informe Personalizado · Encuentro Agendado · Mail Certificado Participación.

### Flujos alternativos / edge cases

- **Pasar a Activo sin pago.** Se bloquea con "Para pasar a Activo primero marcá Pago recibido".
- **Cambio fuera del flujo estándar.** El sistema pide confirmación; si la administradora cancela, no se guarda nada.
- **Baja sin pago (Inscripto → Finalizado).** Pide confirmación y queda auditada.
- **Marcar como realizada sin acción programada.** El botón no aparece o avisa "No hay una acción programada para marcar".
- **Marcar como realizada con fecha anterior a la programada o futura.** Se permite; se guardan ambas fechas.
- **Intentar programar una segunda acción pendiente.** Solo se puede reemplazar la actual.
- **Dos administradoras editan al mismo tiempo.** Gana el último guardado, pero se avisa si el registro cambió desde que se abrió (ver nota 4).
- **Desmarcar el pago de un cliente Activo.** Se avisa que el cliente queda con "Pago pendiente"; se registra en la auditoría.
- **Cliente sin plan o sin fenotipo.** La grilla muestra "Sin definir" / "Sin evaluar".
- **Cliente duplicado por correo.** Si se da de alta manualmente un correo existente, se avisa y se propone abrir el cliente existente.
- **Eliminar cliente.** Pide confirmación. El borrado puede ser lógico (ver nota 6).
- **Grilla vacía o sin resultados de búsqueda.** Se mantienen las columnas y se muestra "No hay clientes para mostrar".
- **Sin conexión o error al guardar.** Mensaje claro, el modal permanece abierto con los datos cargados.

## Criterios de aceptación

- **CA-01** — **Dado** que inicio sesión, **cuando** se carga el panel, **entonces** veo los 4 indicadores y la grilla con sus columnas, aun sin clientes.
- **CA-02** — **Dado** que un cliente completó el test web, **cuando** abro la grilla, **entonces** aparece como Inscripto, con su fenotipo, origen "test" y la etiqueta "Nuevo desde la web".
- **CA-03** — **Dado** que busco "walter", **cuando** escribo, **entonces** la grilla filtra por nombre sin distinguir mayúsculas.
- **CA-04** — **Dado** que filtro por "Pago pendiente", **cuando** se aplica, **entonces** solo veo clientes con pago no recibido que no están Finalizados.
- **CA-05** — **Dado** un alta manual con nombre, correo o teléfono vacío (o un correo con formato inválido), **cuando** guardo, **entonces** veo el error del campo correspondiente y no se crea el cliente.
- **CA-06** — **Dado** un alta manual válida, **cuando** guardo, **entonces** el cliente aparece en la grilla y la auditoría muestra "Persona agregada (estado inicial: …)" con mi nombre.
- **CA-07** — **Dado** un cliente Inscripto sin pago, **cuando** intento pasarlo a Activo, **entonces** se bloquea con el mensaje de pago requerido.
- **CA-08** — **Dado** un cliente Inscripto, **cuando** marco "Pago recibido" con fecha y lo paso a Activo, **entonces** se guarda, la grilla muestra "Programa en proceso" y la auditoría registra el pago y el cambio con el evento "Pago confirmado".
- **CA-09** — **Dado** un cliente Finalizado, **cuando** intento volver a Activo, **entonces** el sistema pide confirmación como excepción y, si confirmo, lo registra.
- **CA-10** — **Dado** un cliente con acción programada "Mail Semana 1" para el 20/09, **cuando** uso "Marcar como realizada" con fecha 21/09 y guardo, **entonces** esa acción pasa a Última acción (21/09), la próxima queda vacía y el historial muestra "programada para 20/09".
- **CA-11** — **Dado** un cliente con historial, **cuando** abro Historial, **entonces** veo todas las acciones realizadas ordenadas por fecha descendente, la pendiente (si hay) y la actividad reciente.
- **CA-12** — **Dado** que cambio el plan de Sin definir a Premium, **cuando** guardo, **entonces** la auditoría registra "Plan: Sin definir → Premium".
- **CA-13** — **Dado** que abro un cliente con "Ver detalle", **cuando** se muestra, **entonces** todos los campos están en solo lectura y existe un botón para pasar a Editar en la misma solapa.
- **CA-14** — **Dado** que elimino un cliente, **cuando** confirmo, **entonces** desaparece de la grilla.
- **CA-15** — **Dado** que no inicié sesión, **cuando** intento abrir el Portal o sus datos, **entonces** el acceso es rechazado.

## Restricciones

### Técnicas
- Una única entidad `Cliente` compartida con el sitio web (spec 02); un solo origen de verdad para el Portal y el sitio.
- Los datos viven en base de datos compartida (el prototipo usa almacenamiento local por navegador y no sirve para el equipo real).
- Acciones, planes, estados y fenotipos salen de catálogos.
- Historial de acciones y auditoría en tablas propias (`HistorialAccion`, `RegistroActividad`); la auditoría es solo inserción (no se edita ni se borra) y no se recorta a 15 entradas en producción.
- Una acción pendiente por cliente: restricción única parcial en `HistorialAccion`.
- Las reglas de estado se validan en el servidor, no solo en la pantalla.
- Responsive; diseño del prototipo (paleta salvia, tipografías del sitio).

### De negocio
- Etapa 1 manual: el sistema no envía mails ni cobra; solo registra.
- Un solo plan por cliente.
- Estados: Inscripto, Activo, Pausado, Finalizado; el antiguo "Prospecto" se llama Inscripto.
- Pago confirmado manualmente por una administradora antes de activar.
- El programa dura 4 semanas.

### De seguridad
- Acceso solo con identificación de la administradora (usuario y contraseña, ver nota 1) y sesión con vencimiento.
- Cada acción se atribuye a la administradora que la hizo (`AdministradorId`).
- Datos personales y de salud: HTTPS, cifrado en reposo, copias de respaldo, acceso mínimo necesario, Ley 25.326.
- Eliminación y cambios excepcionales con confirmación y rastro.
- No exponer el Portal a buscadores ni a personas sin sesión.

## Notas

1. **Autenticación.** El prototipo solo elige perfil (Sofia / Marilina) sin contraseña. Definir mecanismo real: usuario/contraseña, enlace por correo o inicio con Google.
2. **Alta desde la web.** Se asume que el cliente nace automáticamente como Inscripto, sin aprobación. Confirmar si se prefiere una bandeja de revisión previa.
3. **Fecha de inicio del programa.** Los mails de Semana 1 a 4 y el paso a Finalizado dependen de una fecha de inicio; se propone `Cliente.FechaInicioPrograma` (al activar), que hoy no existe.
4. **Concurrencia.** Definir si se bloquea la edición simultánea o solo se avisa.
5. **Pago (decidido).** El pago se registra solo con "Pago recibido" y "Fecha de pago" (`Cliente.PagoRecibido` y `Cliente.FechaPago`). El modelo ya no tiene tabla `Pago`: no se guardan monto ni comprobante. La auditoría registra quién marcó o desmarcó el pago y cuándo.