
namespace WellnessNutricion.Api.Models;

public class CatalogoPregunta
{
    // Identificador manual: preguntas del 1 al 16.
    public int Id { get; set; }

    public string Texto { get; set; } = string.Empty;

    public string Fenotipo { get; set; } = string.Empty;

    public bool Invertida { get; set; }

    public bool Activa { get; set; } = true;

    // Respuestas asociadas a esta pregunta.
    public ICollection<TestRespuesta> Respuestas { get; set; }
        = new List<TestRespuesta>();
}
