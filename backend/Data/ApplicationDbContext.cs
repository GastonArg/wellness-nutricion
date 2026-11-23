
using Microsoft.EntityFrameworkCore;
using WellnessNutricion.Api.Models;

namespace WellnessNutricion.Api.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(
        DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Cliente> Clientes { get; set; }

    public DbSet<TestSesion> TestSesiones { get; set; }

    public DbSet<TestRespuesta> TestRespuestas { get; set; }

    public DbSet<CatalogoPregunta> CatalogoPreguntas { get; set; }

    public DbSet<CatalogoFenotipo> CatalogoFenotipos { get; set; }

    public DbSet<TestResultadosFenotipo> TestResultadosFenotipos { get; set; }

    public DbSet<Plan> Planes { get; set; }

    public DbSet<DetallePlan> DetallesPlan { get; set; }

    public DbSet<Administrador> Administradores { get; set; }
    
    public DbSet<Consentimiento> Consentimientos { get; set; }

    public DbSet<Pago> Pagos { get; set; }
    public DbSet<HistorialActividad> HistorialesActividad { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // IDs UUID generados desde C#.
        modelBuilder.Entity<Cliente>()
            .Property(c => c.Id).ValueGeneratedNever();

        modelBuilder.Entity<TestSesion>()
            .Property(s => s.Id).ValueGeneratedNever();

        modelBuilder.Entity<TestRespuesta>()
            .Property(r => r.Id).ValueGeneratedNever();

        modelBuilder.Entity<TestResultadosFenotipo>()
            .Property(r => r.Id).ValueGeneratedNever();

        // Cliente -> TestSesion
        modelBuilder.Entity<TestSesion>()
            .HasOne(s => s.Cliente)
            .WithMany()
            .HasForeignKey(s => s.ClienteId)
            .OnDelete(DeleteBehavior.Restrict);

        // TestSesion -> TestRespuesta
        modelBuilder.Entity<TestRespuesta>()
            .HasOne(r => r.TestSesion)
            .WithMany(s => s.Respuestas)
            .HasForeignKey(r => r.TestSesionId)
            .OnDelete(DeleteBehavior.Cascade);

        // CatalogoPregunta -> TestRespuesta
        modelBuilder.Entity<TestRespuesta>()
            .HasOne(r => r.Pregunta)
            .WithMany(p => p.Respuestas)
            .HasForeignKey(r => r.PreguntaId)
            .OnDelete(DeleteBehavior.Restrict);

        // Una pregunta por sesión.
        modelBuilder.Entity<TestRespuesta>()
            .HasIndex(r => new { r.TestSesionId, r.PreguntaId })
            .IsUnique();

        // Restricción: respuesta entre 1 y 5.
        modelBuilder.Entity<TestRespuesta>()
            .ToTable(t => t.HasCheckConstraint(
                "CK_TestRespuestas_Valor",
                "\"Valor\" BETWEEN 1 AND 5"
            ));

        // TestSesion -> TestResultadosFenotipo
        modelBuilder.Entity<TestResultadosFenotipo>()
            .HasOne(r => r.TestSesion)
            .WithMany()
            .HasForeignKey(r => r.TestSesionId)
            .OnDelete(DeleteBehavior.Cascade);

        // CatalogoFenotipo -> TestResultadosFenotipo
        modelBuilder.Entity<TestResultadosFenotipo>()
            .HasOne(r => r.Fenotipo)
            .WithMany()
            .HasForeignKey(r => r.FenotipoCodigo)
            .OnDelete(DeleteBehavior.Restrict);

        // Un resultado por fenotipo en cada sesión.
        modelBuilder.Entity<TestResultadosFenotipo>()
            .HasIndex(r => new { r.TestSesionId, r.FenotipoCodigo })
            .IsUnique();

        // IDs UUID generados desde C#.
        modelBuilder.Entity<Plan>()
            .Property(p => p.Id)
            .ValueGeneratedNever();

        modelBuilder.Entity<DetallePlan>()
            .Property(d => d.Id)
            .ValueGeneratedNever();

        // Administrador
        modelBuilder.Entity<Administrador>()
            .Property(a => a.Id)
            .ValueGeneratedNever();

        modelBuilder.Entity<Administrador>()
            .HasIndex(a => a.Email)
            .IsUnique();

        // Consentimiento
        modelBuilder.Entity<Consentimiento>()
            .Property(c => c.Id)
            .ValueGeneratedNever();

        modelBuilder.Entity<Consentimiento>()
            .HasOne(c => c.TestSesion)
            .WithOne(s => s.Consentimiento)
            .HasForeignKey<Consentimiento>(c => c.TestSesionId)
            .OnDelete(DeleteBehavior.Restrict);
        
        // Pago
        modelBuilder.Entity<Pago>()
            .Property(p => p.Id)
            .ValueGeneratedNever();

        modelBuilder.Entity<Pago>()
            .HasOne(p => p.Cliente)
            .WithMany()
            .HasForeignKey(p => p.ClienteId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Pago>()
            .HasOne(p => p.Plan)
            .WithMany()
            .HasForeignKey(p => p.PlanId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Pago>()
            .HasOne(p => p.TestSesion)
            .WithMany()
            .HasForeignKey(p => p.TestSesionId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Pago>()
            .HasOne(p => p.Administrador)
            .WithMany()
            .HasForeignKey(p => p.RegistradoPor)
            .OnDelete(DeleteBehavior.Restrict);


        // HistorialActividad
        modelBuilder.Entity<HistorialActividad>()
            .Property(h => h.Id)
            .ValueGeneratedNever();

        modelBuilder.Entity<HistorialActividad>()
            .HasOne(h => h.Cliente)
            .WithMany()
            .HasForeignKey(h => h.ClienteId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<HistorialActividad>()
            .HasOne(h => h.Administrador)
            .WithMany()
            .HasForeignKey(h => h.AdministradorId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
