
namespace WellnessNutricion.Api.Models;

public class DetallePlan
{
    // UUID generado desde C#.
    public Guid Id { get; set; } = Guid.NewGuid();

    public string Nombre { get; set; } = string.Empty;

    public int Posicion { get; set; }

    public bool Activo { get; set; } = true;
}
