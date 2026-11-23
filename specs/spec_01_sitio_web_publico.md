# Spec 01 — Sitio web público: landing, planes y elección de plan

**Versión:** 1.0 · **Prototipo base:** `wellness_nutricion_landing_v3 enfoque reeducacion.html` · **Modelo:** `modelo_datos_V4.0.md` (`Plan`, `DetallePlan`, `PlanDetallePlan`, `Cliente`)

## Necesidad que se quiere resolver

Las personas interesadas necesitan entender qué es el programa de reeducación alimentaria por fenotipo, confiar en las profesionales, ver qué incluye cada plan y saber cómo avanzar (hacer el test, elegir plan y pagar). Hoy esa información no está centralizada ni conduce a un registro.

## Contexto de uso

- Quien visita es una persona adulta, llega desde redes o recomendación, en su mayoría desde el celular.
- No tiene cuenta ni la necesita. La navegación es de una sola página con anclas, más las pantallas del test y de elección de plan.
- El contenido (planes, prestaciones, precios) lo mantiene el equipo; no debe requerir cambiar código para actualizarlo.

## Objetivo

Que la persona comprenda la propuesta en pocos minutos y llegue al test; y que, después del test, pueda elegir un plan y saber exactamente cómo pagar, quedando esa elección registrada en su ficha.

## Alcance

### Incluye
- **Landing** con secciones: Hero, Para quién es, Test EFCA (pasos 1 a 4), Fenotipos, Material incluido, Equipo, Planes y precios, FAQ, CTA final y pie.
- **Navegación** fija con anclas y botón "Hacer el test EFCA".
- **Planes y precios** leídos desde el catálogo (`Plan`, `PlanDetallePlan`, `DetallePlan`): nombre, subtítulo, precio, leyenda de período, lista de prestaciones y plan destacado.
- **FAQ** desplegable.
- **Pantalla de elección de plan** (sucesora del `checkout.html` referenciado en el prototipo): muestra los 3 planes, permite elegir uno, registra la elección y muestra las **instrucciones de pago manual**.
- Diseño responsive y accesible.

### No incluye
- Pago en línea ni pasarela.
- Cuenta de usuario, login de clientes ni área privada.
- Blog, chat o formularios de contacto adicionales.
- Panel para editar el contenido de la landing (se edita a través del catálogo de planes en el Portal o por despliegue).

## Comportamiento esperado

### Flujo principal

1. La persona entra a la landing y recorre las secciones o salta por el menú.
2. Hace clic en **Hacer el test** (nav, hero, sección Test, Fenotipos o CTA final) y se la lleva a la spec 02.
3. Tras completar el test y ver su resultado, hace clic en **Ver planes y confirmar pago** y llega a la pantalla de elección de plan, identificada por el cliente recién creado.
4. Ve los tres planes con su detalle. El plan Completo muestra la insignia "Más elegido". Elige uno.
5. El sistema guarda `Cliente.PlanId` y registra la acción en `RegistroActividad` ("Plan: Sin definir → Completo").
6. La pantalla muestra la confirmación de elección, el **monto del plan** y las **instrucciones de pago manual** (datos bancarios o alias, referencia a indicar, a quién enviar el comprobante y por qué medio) y el plazo: *"En 24–48 hs hábiles de confirmado el pago recibís el programa."*
7. La persona paga fuera del sistema. La confirmación la hace el equipo desde el Portal (spec 03).

### Flujos alternativos / edge cases

- **Entra a elegir plan sin haber hecho el test** (botones "Elegir Esencial/Completo/Premium" de la landing). Se pide nombre, correo y teléfono, se crea el cliente con `Origen = web_plan` (ver nota 1) en estado Inscripto, se registra el plan y se invita a hacer el test sin costo, como paso recomendado, no obligatorio.
- **Cambia de plan antes de pagar.** Puede volver a elegir; el plan anterior se reemplaza y el cambio queda en `RegistroActividad`.
- **Cambia de plan después de pagar o de ser activado.** No se puede desde la web; se gestiona con el equipo, que lo edita en el Portal.
- **El plan está inactivo** (`EstaActivo = false`). No se muestra ni se puede elegir.
- **Precio no definido.** Mientras `Plan.Precio` sea nulo, la tarjeta muestra "Consultar" y la pantalla de pago indica que el equipo informará el monto.
- **Enlace de elección inválido o vencido** (cliente inexistente). Se muestra un mensaje y se ofrece volver al test.
- **Sin JavaScript o conexión lenta.** El contenido informativo es legible; las animaciones de aparición no ocultan texto de forma permanente.

## Criterios de aceptación

- **CA-01** — **Dado** que visito la landing, **cuando** se carga, **entonces** veo las secciones Hero, Para quién, Test, Fenotipos, Material, Equipo, Planes, FAQ y CTA, y el menú lleva a cada una.
- **CA-02** — **Dado** que hay planes activos en el catálogo, **cuando** veo "Planes y precios", **entonces** aparecen solo los activos, ordenados por `Orden`, con sus prestaciones activas y el plan `EsDestacado` con la insignia "Más elegido".
- **CA-03** — **Dado** que hago clic en cualquier botón "Hacer el test", **cuando** se procesa, **entonces** llego a la pantalla inicial del test (spec 02).
- **CA-04** — **Dado** que terminé el test y estoy identificado, **cuando** elijo el plan Completo, **entonces** `Cliente.PlanId` queda en Completo y aparece una entrada en `RegistroActividad`.
- **CA-05** — **Dado** que elegí un plan, **cuando** veo la confirmación, **entonces** aparecen el monto (o "Consultar"), las instrucciones de pago y el plazo de 24–48 hs hábiles desde la confirmación del pago.
- **CA-06** — **Dado** que elijo un plan sin haber hecho el test, **cuando** completo nombre y correo válidos, **entonces** se crea un cliente Inscripto con ese plan y se me invita a hacer el test.
- **CA-07** — **Dado** que elijo un plan sin datos de contacto válidos, **cuando** intento continuar, **entonces** veo un mensaje de error y no se crea el cliente.
- **CA-08** — **Dado** que abro la FAQ, **cuando** hago clic en una pregunta, **entonces** se despliega su respuesta y se pliega la anterior.
- **CA-09** — **Dado** un celular de 360 px de ancho, **cuando** recorro la página, **entonces** no hay scroll horizontal ni contenido cortado.

## Restricciones

### Técnicas
- Contenido de planes y prestaciones servido desde el catálogo (`Plan`, `PlanDetallePlan`, `DetallePlan`); no hardcodeado.
- HTML semántico, contraste suficiente, navegación por teclado, `alt` en imágenes, fuentes con fallback.
- Respeta el diseño del prototipo (paleta salvia/terracota/dorado, Cormorant Garamond + DM Sans).
- Rendimiento: carga inicial liviana; animaciones no bloqueantes.

### De negocio
- Un solo plan por cliente.
- El texto debe mantener el enfoque de **reeducación alimentaria, no dieta**, y el aviso de que no reemplaza la consulta nutricional ni médica; dirigido a adultos sanos.
- Los nombres de plan son Esencial, Completo y Premium (el término "VIP" ya no se usa).
- El programa se entrega **después** de confirmado el pago (24–48 hs hábiles).

### De seguridad
- HTTPS obligatorio.
- La pantalla de elección de plan solo muestra o modifica datos del cliente identificado por un token no adivinable (no por un id secuencial).
- No se muestran datos personales de otras personas.
- Los datos bancarios de pago se leen de configuración, no del código.

## Notas

1. **Origen del cliente.** El modelo V4.0 define `Origen` como `test_efca` o `manual`; para el caso "eligió plan sin test" se propone agregar `web_plan`.
2. **Precios.** La landing tiene `XX.XXX` como placeholder; deben definirse antes de publicar.
3. **Instrucciones de pago.** Falta definir medio (transferencia, alias, link de cobro manual) y forma de enviar el comprobante (mail, WhatsApp).
4. **Vigencia del material.** Esencial dice "acceso de por vida al material"; el resto, 4 semanas. Aclarar o unificar.
5. **Grupo de WhatsApp.** Se muestra como no incluido en Esencial, pero no figura en ningún otro plan.
6. **Consultas en Completo y Premium.** Las prestaciones "semana a semana" y "personalizadas" se solapan; revisar el texto.
7. **Mismas personas que repiten.** Si el correo ya existe, definir si se reutiliza el cliente (ver spec 02, nota 2).
8. **`checkout.html`** no estaba en los prototipos recibidos; esta pantalla es una propuesta a validar.
