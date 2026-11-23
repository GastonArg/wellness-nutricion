
namespace WellnessNutricion.Api.Models;

public class Cliente
{
    // Identificador UUID generado desde C#
    public Guid Id { get; set; } = Guid.NewGuid();

    public string Nombre { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string? Telefono { get; set; }

    public DateOnly? FechaNacimiento { get; set; }

    public string Objetivo { get; set; } = string.Empty;

    public string Estado { get; set; } = "Prospecto";

    public string? FenotipoCodigo { get; set; }

    public Guid? PlanId { get; set; }

    public DateOnly FechaInscripcion { get; set; }
        = DateOnly.FromDateTime(DateTime.UtcNow);

    public DateOnly? FechaContacto { get; set; }

    public string? Notas { get; set; }

    public bool FlagPagoRecibido { get; set; }

    public bool FlagEmailEnviado { get; set; }

    public bool FlagEfcaCompletado { get; set; }

    public bool FlagConsentimiento { get; set; }

    public bool FlagContactado { get; set; }

    public bool FlagWhatsapp { get; set; }

    public DateTime CreadoEn { get; set; } = DateTime.UtcNow;

    public DateTime ActualizadoEn { get; set; } = DateTime.UtcNow;
}
