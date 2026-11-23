
namespace WellnessNutricion.Api.Models;

public class CatalogoFenotipo
{
    // Código identificador manual.
    public string Codigo { get; set; } = string.Empty;

    public string Nombre { get; set; } = string.Empty;

    public int UmbralBajo { get; set; }

    public int UmbralMedio { get; set; }

    public int PuntajeMax { get; set; }

    public string? DescBajo { get; set; }

    public string? DescMedio { get; set; }

    public string? DescAlto { get; set; }
}
