
namespace WellnessNutricion.Api.Models;

public class Pago
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public Guid ClienteId { get; set; }

    public Guid PlanId { get; set; }

    public Guid? TestSesionId { get; set; }

    public string MetodoRegistro { get; set; } = "manual";

    public string Estado { get; set; } = "pendiente";

    public decimal Monto { get; set; }

    public string Moneda { get; set; } = "ARS";

    public string? ComprobanteUrl { get; set; }

    public string? Notas { get; set; }

    public Guid? RegistradoPor { get; set; }

    public DateTime CreadoEn { get; set; } = DateTime.UtcNow;

    public DateTime? ConfirmadoEn { get; set; }

    public Cliente Cliente { get; set; } = null!;

    public Plan Plan { get; set; } = null!;

    public TestSesion? TestSesion { get; set; }

    public Administrador? Administrador { get; set; }
}
