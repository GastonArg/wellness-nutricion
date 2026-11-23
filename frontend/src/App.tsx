
import { useState } from 'react'
import './App.css'

import {
  calcularEFCA,
  FENOTIPO_INFO,
  type Respuestas,
  type Fenotipo,
} from './efca'

const options = [
  { value: 1, label: 'Nunca' },
  { value: 2, label: 'Rara vez' },
  { value: 3, label: 'A veces' },
  { value: 4, label: 'Casi siempre' },
  { value: 5, label: 'Siempre' },
]

const questions = [
  'Como hasta sentirme muy lleno/a.',
  'Calmo mis emociones con comida.',
  'Pido más comida cuando termino mi plato.',
  'Tengo la costumbre de picotear (realizar pequeñas ingestas entre las comidas principales sin medir la cantidad).',
  'Cuando empiezo a comer algo que me gusta mucho, me cuesta detenerme.',
  'Suelo comer más de un plato en las comidas principales.',
  'Picoteo entre comidas por ansiedad, aburrimiento, soledad, miedo, enojo, tristeza y/o cansancio.',
  'Me siento tentado/a de comer cuando veo o huelo comida que me gusta, o cuando paso frente a un kiosco, panadería o fast food.',
  'Desayuno todos los días.',
  'Como en los momentos en que estoy aburrido/a, ansioso/a, nervioso/a, triste, cansado/a, enojado/a y/o solo/a.',
  'Salteo alguna o al menos una de las comidas principales (desayuno, almuerzo, merienda o cena).',
  'Cuando estoy frente a comida que me gusta mucho, aunque no tenga hambre, termino comiéndola.',
  'Como mucha comida en poco tiempo.',
  'Cuando como algo que me gusta, finalizo toda la porción.',
  'Cuando como algo que me gusta, lo como muy rápido.',
  'Paso más de 5 horas en el día sin comer.',
]

function App() {
  const [started, setStarted] = useState(false)
  const [currentIndex, setCurrentIndex] = useState(0)
  const [answers, setAnswers] = useState<Record<number, number>>({})
  const [finished, setFinished] = useState(false)

  const currentQuestion = questions[currentIndex]
  const currentAnswer = answers[currentIndex]

  const progress =
    (Object.keys(answers).length / questions.length) * 100

  // Convertimos los índices 0-15 en números de pregunta 1-16.
  const respuestasEFCA: Respuestas = Object.fromEntries(
    Object.entries(answers).map(([index, value]) => [
      Number(index) + 1,
      value,
    ])
  )

  const resultado = finished
    ? calcularEFCA(respuestasEFCA)
    : null

  function selectAnswer(value: number) {
    setAnswers((previous) => ({
      ...previous,
      [currentIndex]: value,
    }))
  }

  function goBack() {
    if (currentIndex > 0) {
      setCurrentIndex((previous) => previous - 1)
    } else {
      setStarted(false)
    }
  }

  function goNext() {
    if (currentIndex < questions.length - 1) {
      setCurrentIndex((previous) => previous + 1)
    } else {
      setFinished(true)
    }
  }

  function restartTest() {
    setStarted(false)
    setCurrentIndex(0)
    setAnswers({})
    setFinished(false)
  }

  return (
    <main className="app">
      <header className="header">
        <span className="brand">Wellness Nutrición</span>
      </header>

      {!started ? (
        <section className="welcome">
          <p className="eyebrow">
            CONOCÉ TU FENOTIPO ALIMENTARIO
          </p>

          <h1>
            Tu bienestar comienza
            <br />
            con conocerte.
          </h1>

          <p className="description">
            Respondé nuestro test EFCA y descubrí qué patrones
            alimentarios pueden ayudarte a comprender mejor tu
            relación con la comida.
          </p>

          <button
            className="start-button"
            onClick={() => setStarted(true)}
          >
            Comenzar test
          </button>

          <p className="note">16 preguntas · 3–5 minutos</p>
        </section>
      ) : finished && resultado ? (
        <section className="test-container result-container">
          <p className="eyebrow">TU RESULTADO EFCA</p>

          <div className="result-icon">
            {FENOTIPO_INFO[resultado.predominante].icono}
          </div>

          <h1 className="result-title">
            {FENOTIPO_INFO[resultado.predominante].nombre}
          </h1>

          <p className="result-level">
            Nivel:{' '}
            <strong>{resultado.nivel.toUpperCase()}</strong>
          </p>

          <p className="description">
            {FENOTIPO_INFO[resultado.predominante]
              .descripcion[resultado.nivel]}
          </p>

          <div className="scores-section">
            <h2>Resumen de tus resultados</h2>

            {(Object.keys(resultado.puntajes) as Fenotipo[]).map(
              (fenotipo) => (
                <div className="score-item" key={fenotipo}>
                  <div className="score-header">
                    <span>
                      {FENOTIPO_INFO[fenotipo].nombre}
                    </span>

                    <strong>
                      {resultado.puntajes[fenotipo]} puntos
                    </strong>
                  </div>

                  <div className="score-track">
                    <div
                      className="score-fill"
                      style={{
                        width: `${resultado.porcentajes[fenotipo]}%`,
                      }}
                    />
                  </div>

                  <span className="score-level">
                    Nivel {resultado.niveles[fenotipo]}
                  </span>
                </div>
              )
            )}
          </div>

          <button
            className="start-button"
            onClick={restartTest}
          >
            Volver al inicio
          </button>
        </section>
      ) : (
        <section className="test-container">
          <p className="eyebrow">
            CONOCÉ TU FENOTIPO ALIMENTARIO
          </p>

          <div className="progress-info">
            <span>
              Pregunta {currentIndex + 1} de {questions.length}
            </span>

            <span>
              {Math.round(progress)}% respondido
            </span>
          </div>

          <div
            className="progress-track"
            role="progressbar"
            aria-label="Progreso del test"
            aria-valuenow={Math.round(progress)}
            aria-valuemin={0}
            aria-valuemax={100}
          >
            <div
              className="progress-fill"
              style={{ width: `${progress}%` }}
            />
          </div>

          <h2 className="question-title">
            {currentQuestion}
          </h2>

          <p className="question-hint">
            Elegí la opción que mejor refleje tu experiencia.
          </p>

          <div className="options-list">
            {options.map((option) => (
              <button
                key={option.value}
                type="button"
                className={`option-button ${
                  currentAnswer === option.value ? 'selected' : ''
                }`}
                aria-pressed={currentAnswer === option.value}
                onClick={() => selectAnswer(option.value)}
              >
                <span className="option-number">
                  {option.value}
                </span>

                <span>{option.label}</span>
              </button>
            ))}
          </div>

          <div className="navigation-buttons">
            <button
              className="back-button"
              type="button"
              onClick={goBack}
            >
              {currentIndex === 0
                ? 'Volver al inicio'
                : 'Anterior'}
            </button>

            <button
              className="start-button"
              type="button"
              onClick={goNext}
              disabled={currentAnswer === undefined}
            >
              {currentIndex === questions.length - 1
                ? 'Finalizar test'
                : 'Siguiente'}
            </button>
          </div>
        </section>
      )}
    </main>
  )
}

export default App
