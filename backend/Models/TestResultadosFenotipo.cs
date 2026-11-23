
namespace WellnessNutricion.Api.Models;

public class TestResultadosFenotipo
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public Guid TestSesionId { get; set; }

    public string FenotipoCodigo { get; set; } = string.Empty;

    public int Puntaje { get; set; }

    public string Nivel { get; set; } = string.Empty;

    public bool EsPredominante { get; set; }

    // Relación con la sesión.
    public TestSesion TestSesion { get; set; } = null!;

    // Relación con el catálogo de fenotipos.
    public CatalogoFenotipo Fenotipo { get; set; } = null!;
}
