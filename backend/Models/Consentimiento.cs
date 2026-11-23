
namespace WellnessNutricion.Api.Models;

public class Consentimiento
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public Guid TestSesionId { get; set; }

    public string VersionTexto { get; set; } = string.Empty;

    public bool Aceptado { get; set; }

    public DateTime? AceptadoEn { get; set; }

    public string? IpOrigen { get; set; }

    public TestSesion TestSesion { get; set; } = null!;
}
