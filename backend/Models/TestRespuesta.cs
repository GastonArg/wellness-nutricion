
namespace WellnessNutricion.Api.Models;

public class TestRespuesta
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public Guid TestSesionId { get; set; }

    public int PreguntaId { get; set; }

    public int Valor { get; set; }

    // Relación con la sesión.
    public TestSesion TestSesion { get; set; } = null!;

    // Relación con la pregunta del catálogo.
    public CatalogoPregunta Pregunta { get; set; } = null!;
}
