# Spec 02 — Test EFCA, consentimiento y captura de datos del cliente

**Versión:** 1.2 · **Prototipo base:** `test_efca.html` · **Modelo:** `modelo_datos_V5.0.md` (`Cliente`, `TestEfca`, `Consentimiento`, `TextoConsentimiento`, `ResultadoFenotipo`, `Pregunta`, `Fenotipo`, `Objetivo`)

## Necesidad que se quiere resolver

Identificar el fenotipo de comportamiento alimentario de cada persona y capturar, en el mismo paso, sus datos de contacto y su consentimiento informado, de modo que el equipo pueda acompañarla. Hoy el prototipo calcula el resultado pero solo lo escribe en la consola del navegador: **nada se guarda**.

## Contexto de uso

- La persona completa el test sola, en 3 a 5 minutos, normalmente desde el celular.
- No tiene cuenta. Es gratuito y no obliga a comprar.
- El resultado se muestra en pantalla de inmediato; el equipo lo ve después en el Portal.

## Objetivo

Que cada persona que complete el test obtenga su fenotipo y nivel al instante, y que el sistema **guarde** su cliente, resultado y consentimiento aceptado para que el equipo pueda continuar el proceso.

## Alcance

### Incluye
- Pantalla de introducción y barra de progreso.
- **16 preguntas** con escala 1 a 5 (Nunca, Rara vez, A veces, Casi siempre, Siempre), todas obligatorias.
- Formulario de datos: nombre y apellido (obligatorio), correo (obligatorio, validado), fecha de nacimiento (opcional, mayor de 18 años), WhatsApp/teléfono (opcional), objetivo principal (opcional, 5 opciones).
- **Consentimiento informado** versionado: lectura desplegable y casilla de aceptación obligatoria.
- **Cálculo** de puntaje por fenotipo, nivel y fenotipo predominante.
- **Pantalla de resultado**: fenotipo predominante y nivel, descripción, barras de los 5 fenotipos, correo registrado, próximos pasos y botón hacia la elección de plan (spec 01). El texto aclara que el programa se envía después de confirmado el pago.
- **Persistencia**: alta de `Cliente` (o reutilización), `TestEfca`, `Consentimiento`, `ResultadoFenotipo`, y entrada en `RegistroActividad`. **Solo se guarda el resultado: las respuestas individuales no se almacenan.**
- Validaciones y mensajes de error.

### No incluye
- Envío automático del resultado por correo (el prototipo lo anuncia; en la Etapa 1 lo envía el equipo; ver nota 3).
- Guardado de las respuestas individuales y guardado parcial para retomar luego: solo se registra el test al completarse.
- Test final (`Momento = final`) y comparación inicial vs. final.
- Diagnóstico clínico o recomendaciones médicas.
- Versión para menores de 18 años.

## Comportamiento esperado

### Flujo principal

1. La persona entra al test y ve la introducción (16 preguntas, 3 a 5 minutos, datos confidenciales).
2. Responde cada pregunta con una de las 5 opciones. La barra de progreso refleja las respondidas (n / 16).
3. Completa nombre, correo y, si quiere, fecha de nacimiento, teléfono y objetivo.
4. Lee (opcional) y **acepta** el consentimiento. Hasta aceptarlo, el botón **Ver mi fenotipo** está deshabilitado.
5. Hace clic en **Ver mi fenotipo**. El sistema valida (todas las preguntas, nombre, correo con formato válido).
6. El sistema calcula (reglas abajo) y **guarda** todo en una sola operación:
   - crea o reutiliza el `Cliente` (`Origen = test_efca`, `Estado = Inscripto`, `PagoRecibido = false`);
   - crea `TestEfca` (`Momento = inicial`, `RealizadoEn`, fenotipo y nivel predominantes);
   - crea `Consentimiento` con la versión de `TextoConsentimiento` vigente, fecha y hora (y IP si se registra);
   - crea 5 `ResultadoFenotipo` (puntaje y nivel por fenotipo), **sin guardar las 16 respuestas**;
   - actualiza `Cliente.FenotipoId`;
   - agrega una entrada en `RegistroActividad`: "Persona agregada desde el test EFCA".
7. Se muestra el resultado y el botón **Ver planes y confirmar pago**, que lleva a la spec 01 con el cliente identificado.

### Reglas de cálculo

- **Puntaje de un fenotipo** = suma de los valores de sus preguntas. En la pregunta 9 (invertida) se usa `6 − valor`.
- **Preguntas por fenotipo y máximos:**

| Fenotipo | Preguntas | Máx. | Bajo ≤ | Medio ≤ |
|---|---|---|---|---|
| Desorganizado | 9 (inv.), 11, 16 | 15 | 4 | 7 |
| Hedónico | 5, 8, 12, 14 | 20 | 11 | 15 |
| Compulsivo | 13, 15 | 10 | 3 | 7 |
| Emocional / Picoteador | 2, 4, 7, 10 | 20 | 8 | 13 |
| Hiperfágico | 1, 3, 6 | 15 | 5 | 9 |

- **Nivel**: ≤ umbral bajo ⇒ bajo; ≤ umbral medio ⇒ medio; si no, alto.
- **Predominante**: mayor `puntaje / máximo`. Desempate: el primero en el orden Desorganizado, Hedónico, Compulsivo, Emocional, Hiperfágico (comportamiento actual del prototipo).
- Los textos de descripción por fenotipo y nivel y los umbrales salen del catálogo `Fenotipo`.

### Flujos alternativos / edge cases

- **Faltan respuestas.** Mensaje con la cantidad y qué preguntas faltan; el scroll lleva a la primera sin responder; no se guarda nada.
- **Nombre vacío o correo inválido.** Mensaje claro y foco en el campo; no se guarda nada.
- **No aceptó el consentimiento.** El botón de envío permanece deshabilitado con la leyenda "Aceptá el consentimiento informado para continuar".
- **Fecha de nacimiento inválida** (futura, o de una persona menor de 18 años). Se rechaza el valor y se avisa que el programa es para personas adultas.
- **El correo ya existe como cliente.** Se reutiliza el cliente existente (no se duplica), se agrega un nuevo `TestEfca` y se actualiza `FenotipoId`; no se pisan datos que el equipo ya editó (nombre, teléfono, observaciones) sin avisar (ver nota 2).
- **Falla el guardado** (red o servidor). Se conserva el formulario completo, se muestra un error con opción de reintentar y **no** se muestra un resultado que no quedó guardado.
- **Doble clic en "Ver mi fenotipo".** El envío se bloquea mientras se procesa; no se crean registros duplicados.
- **Recarga del navegador a mitad del test.** Las respuestas ya cargadas se pierden (el guardado parcial queda fuera de alcance).
- **Persona fuera de perfil** (con una enfermedad o trastorno). El consentimiento lo advierte; el sistema no la bloquea (ver nota 5).

## Criterios de aceptación

- **CA-01** — **Dado** que abro el test, **cuando** respondo la pregunta 1, **entonces** el progreso muestra "1 / 16" y la barra avanza en 6 %.
- **CA-02** — **Dado** que no marqué el consentimiento, **cuando** miro el botón "Ver mi fenotipo", **entonces** está deshabilitado.
- **CA-03** — **Dado** que respondí 15 de 16 preguntas, **cuando** hago clic en "Ver mi fenotipo", **entonces** veo qué pregunta falta y no se crea ningún registro.
- **CA-04** — **Dado** un correo sin formato válido, **cuando** envío, **entonces** veo el error y no se guarda.
- **CA-05** — **Dado** que respondí todo "Siempre" (5), **cuando** se calcula, **entonces** el puntaje de Desorganizado es 5 + 5 + 1 = 11 (la pregunta 9 se invierte).
- **CA-06** — **Dado** el puntaje de Hedónico 16 sobre 20, **cuando** se asigna nivel, **entonces** es **alto** (supera el umbral medio de 15).
- **CA-07** — **Dado** dos fenotipos con la misma proporción máxima, **cuando** se elige el predominante, **entonces** gana el que viene primero en el orden definido.
- **CA-08** — **Dado** que el envío se completó, **cuando** reviso la base, **entonces** existen 1 `TestEfca`, 5 `ResultadoFenotipo`, ninguna respuesta individual almacenada, (uno con `EsPredominante`), 1 `Consentimiento` con la versión vigente y el cliente en estado Inscripto.
- **CA-09** — **Dado** que completé el test, **cuando** una administradora abre el Portal, **entonces** me ve en la grilla como cliente Inscripto con mi fenotipo y `Origen = test_efca`.
- **CA-10** — **Dado** un correo que ya existe, **cuando** completo el test, **entonces** no se duplica el cliente y se asocia el nuevo test al existente.
- **CA-11** — **Dado** una falla al guardar, **cuando** se muestra el error, **entonces** conservo mis respuestas y puedo reintentar sin volver a empezar.
- **CA-12** — **Dado** que ya envié, **cuando** hago doble clic o recargo la pantalla de resultado, **entonces** no se crean registros duplicados.

## Restricciones

### Técnicas
- El cálculo se hace en el **servidor** (o se revalida en él) usando `Pregunta`, `Fenotipo` y los umbrales del catálogo; no se confía en el puntaje enviado por el navegador.
- El guardado es **transaccional**: o se guarda todo o nada.
- Preguntas, textos y umbrales son datos del catálogo, no constantes en el código.
- Cada `Consentimiento` referencia la versión exacta de `TextoConsentimiento` mostrada.

### De negocio
- El test es **gratuito** y no exige compra.
- Dirigido a **personas adultas sanas** (18 o más); el consentimiento declara mayoría de edad y buena salud.
- El texto del consentimiento tiene 5 bloques (objetivo, modalidad, alcances y limitaciones, confidencialidad y protección de datos, riesgos y beneficios) y las firmas de ambas profesionales.
- No se promete tratamiento médico ni diagnóstico.

### De seguridad
- Datos sensibles: transporte HTTPS, cifrado en reposo, acceso solo al equipo.
- Cumplimiento de la **Ley 25.326** de Protección de Datos Personales; los datos se usan solo para el programa y no se comparten con terceros.
- Protección contra envíos automatizados (limitación de intentos o captcha invisible) y validación de entrada en el servidor.
- No se exponen resultados de una persona a otra: la pantalla de resultado se vincula a un token no adivinable.

## Notas

1. **Hoy el prototipo no guarda nada**: el resultado va a `console.log`. Esta spec define la persistencia.
2. **Duplicados por correo.** Hay que decidir: ¿se reutiliza siempre el cliente? ¿se avisa a la administradora? ¿qué datos pisa el test nuevo?
3. **Mail del resultado.** El prototipo dice "Tu programa está en camino" y "Resultado por mail". En la Etapa 1 no hay envío automático: ajustar el texto para no prometer lo que no ocurre, o definir que el equipo lo envía.
4. **Momento de envío del programa (decidido).** El programa se envía **después de confirmado el pago** (24–48 hs hábiles). La tarjeta "¡Tu programa está en camino!" de la pantalla de resultado debe corregirse para no sugerir que llega tras el test; por ejemplo: "Para recibir tu programa de 4 semanas, elegí tu plan y confirmá el pago. Lo recibís en 24–48 hs hábiles de confirmado el pago."
5. **Personas fuera de perfil.** Definir si se filtra con una pregunta de salud o solo con el consentimiento.
6. **Texto "VIP".** La pantalla de próximos pasos aún dice "VIP con consulta 1:1"; debe decir Premium.
7. **Nombres de fenotipo.** El test, la landing y el Portal los nombran distinto; unificar.
8. **Fecha de nacimiento (decidido).** El test pide fecha de nacimiento en lugar de edad (el prototipo hoy muestra un campo numérico "Edad" que hay que reemplazar). El modelo guarda `FechaNacimiento` y calcula la edad.
9. **Sin guardado parcial ni respuestas.** Al no guardarse las respuestas, no hay test "en progreso" ni recálculo posterior si cambian preguntas o umbrales (ver nota 22 del modelo V5.0).
10. **Test final.** `Momento = final` existe en el modelo; ninguna pantalla lo usa todavía.
