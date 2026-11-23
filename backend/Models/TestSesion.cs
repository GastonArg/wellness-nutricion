
namespace WellnessNutricion.Api.Models;

public class TestSesion
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public Guid ClienteId { get; set; }

    public string EstadoSesion { get; set; } = "en_progreso";

    public string? FenotipoPredominante { get; set; }

    public string? NivelPredominante { get; set; }

    public DateTime IniciadoEn { get; set; } = DateTime.UtcNow;

    public DateTime? CompletadoEn { get; set; }

    // Relación con el cliente
    public Cliente Cliente { get; set; } = null!;

    // Respuestas pertenecientes a esta sesión
    public ICollection<TestRespuesta> Respuestas { get; set; }
        = new List<TestRespuesta>();

    public Consentimiento? Consentimiento { get; set; }
}
