
namespace WellnessNutricion.Api.Models;

public class Plan
{
    // UUID generado desde C#.
    public Guid Id { get; set; } = Guid.NewGuid();

    public string Nombre { get; set; } = string.Empty;

    public string Tier { get; set; } = string.Empty;

    public decimal Precio { get; set; }

    public string Moneda { get; set; } = "ARS";

    public bool Activo { get; set; } = true;
}
