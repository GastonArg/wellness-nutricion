# Intent — Wellness Nutrición · "ReConectá con Vos"

**Versión:** 1.0 · **Etapa:** 1 (operación manual) · **Fecha:** 23/11/2026
**Modelo de datos de referencia:** `modelo_datos_V5.0.md`

---

## 1. Propósito

Dar a Wellness Nutrición (Lic. Sofía Alurralde y Lic. Marilina Borras) una plataforma web para **captar personas interesadas en reeducación alimentaria por fenotipo**, **evaluarlas con el test EFCA** y **gestionar su recorrido como clientes** (inscripción, pago, programa de 4 semanas, seguimiento y cierre), sin depender de planillas, mails sueltos ni memoria del equipo.

## 2. Problema que resuelve

- Las personas interesadas hoy no tienen un camino claro entre "conocer el programa", "hacer el test" y "contratar un plan".
- Los datos del test, el consentimiento, el plan elegido y el pago viven en lugares distintos (o no se guardan).
- Las administradoras no tienen una vista única de cada cliente: en qué estado está, si pagó, qué se le envió, qué sigue y cuándo.
- Sin registro, es fácil olvidar un mail semanal, una respuesta a una consulta o un encuentro agendado.

## 3. Para quién

| Rol | Quién es | Qué necesita |
|---|---|---|
| **Visitante / potencial cliente** | Persona adulta que quiere mejorar su relación con la comida | Entender el programa, hacer el test gratis, ver su fenotipo, elegir un plan y saber cómo pagar |
| **Administradora** | Sofía y Marilina (profesionales nutricionistas) | Ver y gestionar clientes, registrar pagos y acciones, saber qué hacer y cuándo |
| **Equipo técnico** | Quien desarrolle e implemente | Specs claras, modelo de datos y reglas de negocio sin ambigüedades |

## 4. Qué se quiere lograr (objetivos)

1. Que cada persona que hace el test **quede registrada** con sus datos, resultado del test y consentimiento aceptado.
2. Que el equipo vea **todos los clientes en un solo lugar**, sin importar si llegaron por la web o fueron cargados a mano.
3. Que cada cliente tenga **un estado claro** (Inscripto, Activo, Pausado, Finalizado) con reglas de cambio explícitas y auditables.
4. Que el **pago** (manual en esta etapa) quede registrado y habilite el inicio del programa.
5. Que las **acciones de seguimiento** (mails, consultas, encuentro, informe, certificado) se programen, se marquen como realizadas y queden en un historial.
6. Que toda la operación sea **trazable**: quién cambió qué y cuándo.

## 5. Alcance de la Etapa 1

**Incluye**
- Sitio web público: landing informativa, planes, FAQ, test EFCA con consentimiento informado y captura de datos de contacto, resultado en pantalla, elección de plan e instrucciones de pago manual.
- Portal de Administración: acceso del equipo, grilla de clientes, alta manual, incorporación automática de quienes completan el test, edición, detalle con historial, registro manual de pago, cambio de estado con reglas, programación y registro de acciones de seguimiento, auditoría.
- Un único plan por cliente.

**No incluye (Etapa 1)**
- Pasarela de pago ni conciliación automática.
- Envío automático de mails: el equipo envía por sus propios medios y **registra** en el sistema que lo hizo.
- Área privada para el cliente (login de clientes, descarga de material).
- Historial clínico, diagnóstico o indicaciones médicas.
- Test final con comparación de resultados (el modelo lo prevé, no se construye ahora).
- Múltiples planes a lo largo del tiempo para una misma persona.
- Notificaciones o recordatorios automáticos a las administradoras.

## 6. Principios de diseño

- **Manual primero, automatizable después.** Cada paso manual de hoy es un evento claro que mañana se puede automatizar (el modelo y los estados ya lo contemplan).
- **Un solo cliente, un solo registro.** La misma entidad `Cliente` sirve para el sitio web y para el Portal.
- **El estado lo gobiernan reglas, no la buena voluntad.** No se activa a nadie sin pago confirmado; lo excepcional pide confirmación y deja rastro.
- **Datos de salud y de contacto son sensibles.** Consentimiento explícito y versionado, acceso restringido, Ley 25.326.
- **Lenguaje cercano.** Español rioplatense, tono cálido y sin promesas de resultados clínicos ("no es una dieta").

## 7. Mapa de especificaciones

| # | Archivo | Qué cubre |
|---|---|---|
| 1 | `spec_01_sitio_web_publico.md` | Landing, planes, FAQ, elección de plan e instrucciones de pago |
| 2 | `spec_02_test_efca_captura_datos.md` | Test EFCA, consentimiento, captura de datos, cálculo y resultado, creación del cliente |
| 3 | `spec_03_portal_administracion.md` | Portal del equipo: clientes, alta manual, pago manual, estados, acciones, historial |

Dependencia: la spec 02 crea los clientes que la spec 03 gestiona; la spec 01 deriva a la spec 02 y recibe de vuelta la elección de plan.

## 8. Recorrido de punta a punta (Etapa 1)

1. La persona entra a la landing, lee el programa y hace clic en **Hacer el test**.
2. Responde 16 preguntas, completa sus datos y acepta el consentimiento (spec 02).
3. El sistema calcula el fenotipo, muestra el resultado y **crea el cliente en estado Inscripto**, sin pago.
4. La persona elige un plan y ve cómo pagar (spec 01). El plan queda registrado en su ficha.
5. La persona paga fuera del sistema (transferencia) y avisa/envía comprobante.
6. Una administradora ve al cliente en el Portal, **registra el pago** y lo pasa a **Activo** (spec 03).
7. Se programan y realizan las acciones: mail de bienvenida, programa completo, mails semanales, consultas, encuentro, informe, certificado. Cada una queda en el historial.
8. Al completar las 4 semanas, el cliente pasa a **Finalizado**. Si pausa, pasa a **Pausado** y luego retoma o finaliza.

Los clientes también pueden cargarse a mano en el Portal en cualquier punto del recorrido.

## 9. Criterios de éxito de la Etapa 1

- El 100 % de las personas que completan el test aparecen en el Portal con sus datos, resultado y consentimiento.
- Ninguna activación de cliente sin pago registrado.
- Para cada cliente, las administradoras ven en menos de dos clics: estado, plan, pago, última acción y próxima acción.
- Cada cambio relevante (estado, plan, pago, acción) deja una entrada de auditoría con autor y fecha.

## 10. Glosario

| Término | Significado |
|---|---|
| **EFCA** | Escala de Fenotipo de Comportamiento Alimentario: 16 preguntas, 5 subescalas |
| **Fenotipo** | Perfil de conducta alimentaria: Desorganizado, Hedónico, Compulsivo, Emocional/Picoteador, Hiperfágico |
| **Nivel** | Bajo / medio / alto, según puntaje de cada fenotipo |
| **Plan** | Esencial (Solo Materiales), Completo (Con Acompañamiento), Premium (Experiencia Grupal) |
| **Estado** | Etapa del cliente: Inscripto, Activo, Pausado, Finalizado |
| **Acción** | Tarea de seguimiento del equipo (mail, consulta, encuentro, informe, certificado) |
| **Portal** | Sitio de Administración usado por el equipo |

## 11. Decisiones tomadas y puntos abiertos

**Tomadas para esta etapa**
- Quien completa el test se crea **automáticamente** como cliente Inscripto (no requiere aprobación previa).
- El pago es manual y lo registra una administradora.
- Un cliente tiene un solo plan.
- El programa se envía **después de confirmado el pago**, nunca solo por haber hecho el test.

**Abiertas (se detallan en cada spec)**
- Precios definitivos de los planes y datos bancarios para el pago.
- Mecanismo real de login del equipo (hoy el prototipo solo elige perfil).
- Qué pasa si una misma persona repite el test o contrata sin hacerlo.
- Corrección de textos inconsistentes entre prototipos (por ejemplo "VIP" vs "Premium").
- Baja física o lógica de clientes.
