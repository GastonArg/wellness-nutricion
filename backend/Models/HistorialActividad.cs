
namespace WellnessNutricion.Api.Models;

public class HistorialActividad
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public Guid ClienteId { get; set; }

    public Guid? AdministradorId { get; set; }

    public string Accion { get; set; } = string.Empty;

    public DateTime CreadoEn { get; set; } = DateTime.UtcNow;

    public Cliente Cliente { get; set; } = null!;

    public Administrador? Administrador { get; set; }
}
