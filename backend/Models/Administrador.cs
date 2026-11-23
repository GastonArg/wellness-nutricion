
namespace WellnessNutricion.Api.Models;

public class Administrador
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public string Nombre { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public bool Activo { get; set; } = true;
}
