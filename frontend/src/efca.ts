
export type Fenotipo =
  | 'desorganizado'
  | 'hedonico'
  | 'compulsivo'
  | 'emocional'
  | 'hiperfagico'

export type Nivel = 'bajo' | 'medio' | 'alto'

export type Respuestas = Record<number, number>

export type Puntajes = Record<Fenotipo, number>

export interface ResultadoEFCA {
  predominante: Fenotipo
  nivel: Nivel
  puntajes: Puntajes
  niveles: Record<Fenotipo, Nivel>
  porcentajes: Record<Fenotipo, number>
}

const PREGUNTAS_POR_FENOTIPO: Record<number, Fenotipo> = {
  1: 'hiperfagico',
  2: 'emocional',
  3: 'hiperfagico',
  4: 'emocional',
  5: 'hedonico',
  6: 'hiperfagico',
  7: 'emocional',
  8: 'hedonico',
  9: 'desorganizado',
  10: 'emocional',
  11: 'desorganizado',
  12: 'hedonico',
  13: 'compulsivo',
  14: 'hedonico',
  15: 'compulsivo',
  16: 'desorganizado',
}

const UMBRALES: Record<
  Fenotipo,
  { bajo: number; medio: number; max: number }
> = {
  desorganizado: { bajo: 4, medio: 7, max: 15 },
  hedonico: { bajo: 11, medio: 15, max: 20 },
  compulsivo: { bajo: 3, medio: 7, max: 10 },
  emocional: { bajo: 8, medio: 13, max: 20 },
  hiperfagico: { bajo: 5, medio: 9, max: 15 },
}

export const FENOTIPO_INFO: Record<
  Fenotipo,
  { nombre: string; icono: string; descripcion: Record<Nivel, string> }
> = {
  desorganizado: {
    nombre: 'Comedor Desorganizado',
    icono: '⏳',
    descripcion: {
      bajo: 'No tendés a saltear comidas habitualmente. Mantener este orden es una gran fortaleza.',
      medio: 'Salteas comidas en ocasiones de urgencia aunque no es de tu agrado. Hay oportunidad de mejorar la estructura.',
      alto: 'Salteas al menos 1 comida principal al día y pasás períodos de ayuno de más de 5 horas. Estructurar tus comidas es tu objetivo central.',
    },
  },
  hedonico: {
    nombre: 'Comedor Hedónico',
    icono: '👁️',
    descripcion: {
      bajo: 'Te regís principalmente por el hambre real para comer y pensás poco en la comida entre momentos de ingesta.',
      medio: 'Comés cuando sentís olores o ves comida sin sentir hambre. Los estímulos sensoriales tienen influencia sobre tus elecciones.',
      alto: 'Pensás en tu próxima comida apenas terminás de comer y no podés evitar comer cuando sentís olores o ves comida que te gusta.',
    },
  },
  compulsivo: {
    nombre: 'Comedor Compulsivo',
    icono: '⚡',
    descripcion: {
      bajo: 'Tendés a comer en tiempos prudentes que facilitan la digestión.',
      medio: 'Cuando tenés hambre, no lográs controlar del todo comer rápido y en mayor cantidad.',
      alto: 'Comés excesivamente rápido en todas las comidas y en cantidades abusivas sin darte cuenta.',
    },
  },
  emocional: {
    nombre: 'Emocional / Picoteador',
    icono: '🎭',
    descripcion: {
      bajo: 'Tenés períodos en que reconocés necesitar colaciones por ansiedad, pero que desaparecen con el tiempo.',
      medio: 'Reconocés que en ocasiones comés por ansiedad y realizás pequeñas ingestas más frecuentes durante el día.',
      alto: 'Comés para compensar angustia, aburrimiento, miedo o cansancio. Siempre encontrás una excusa para comer por emociones, incluyendo colaciones de noche.',
    },
  },
  hiperfagico: {
    nombre: 'Hiperfágico',
    icono: '🍔',
    descripcion: {
      bajo: 'Solés mantener las porciones de alimentos de preferencia en gran parte de tus comidas.',
      medio: 'En ocasiones especiales o festivas solés aumentar la porción y repetir.',
      alto: 'Generalmente consumís doble ración y cantidades excesivas de alimentos en las comidas.',
    },
  },
}

function obtenerNivel(fenotipo: Fenotipo, puntaje: number): Nivel {
  const umbral = UMBRALES[fenotipo]

  if (puntaje <= umbral.bajo) return 'bajo'
  if (puntaje <= umbral.medio) return 'medio'

  return 'alto'
}

export function calcularEFCA(respuestas: Respuestas): ResultadoEFCA {
  for (let id = 1; id <= 16; id++) {
    const respuesta = respuestas[id]

    if (
      !Number.isInteger(respuesta) ||
      respuesta < 1 ||
      respuesta > 5
    ) {
      throw new Error(`La pregunta ${id} no tiene una respuesta válida.`)
    }
  }

  const puntajes: Puntajes = {
    desorganizado: 0,
    hedonico: 0,
    compulsivo: 0,
    emocional: 0,
    hiperfagico: 0,
  }

  for (let id = 1; id <= 16; id++) {
    let valor = respuestas[id]

    if (id === 9) {
      valor = 6 - valor
    }

    puntajes[PREGUNTAS_POR_FENOTIPO[id]] += valor
  }

  const niveles = {} as Record<Fenotipo, Nivel>
  const porcentajes = {} as Record<Fenotipo, number>

  let predominante: Fenotipo = 'emocional'
  let maximoRelativo = 0

  for (const fenotipo of Object.keys(puntajes) as Fenotipo[]) {
    const puntaje = puntajes[fenotipo]
    const umbral = UMBRALES[fenotipo]
    const relativo = puntaje / umbral.max

    niveles[fenotipo] = obtenerNivel(fenotipo, puntaje)
    porcentajes[fenotipo] = Math.round(relativo * 100)

    if (relativo > maximoRelativo) {
      maximoRelativo = relativo
      predominante = fenotipo
    }
  }

  return {
    predominante,
    nivel: niveles[predominante],
    puntajes,
    niveles,
    porcentajes,
  }
}
