using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WellnessNutricion.Api.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Clientes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Nombre = table.Column<string>(type: "text", nullable: false),
                    Email = table.Column<string>(type: "text", nullable: false),
                    Telefono = table.Column<string>(type: "text", nullable: true),
                    Edad = table.Column<int>(type: "integer", nullable: true),
                    Objetivo = table.Column<string>(type: "text", nullable: true),
                    Estado = table.Column<string>(type: "text", nullable: false),
                    FenotipoCodigo = table.Column<string>(type: "text", nullable: true),
                    PlanId = table.Column<Guid>(type: "uuid", nullable: true),
                    FechaInscripcion = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    FechaContacto = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Notas = table.Column<string>(type: "text", nullable: true),
                    FlagPagoRecibido = table.Column<bool>(type: "boolean", nullable: false),
                    FlagEmailEnviado = table.Column<bool>(type: "boolean", nullable: false),
                    FlagEfcaCompletado = table.Column<bool>(type: "boolean", nullable: false),
                    FlagConsentimiento = table.Column<bool>(type: "boolean", nullable: false),
                    FlagContactado = table.Column<bool>(type: "boolean", nullable: false),
                    FlagWhatsapp = table.Column<bool>(type: "boolean", nullable: false),
                    CreadoEn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ActualizadoEn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Clientes", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Clientes");
        }
    }
}
