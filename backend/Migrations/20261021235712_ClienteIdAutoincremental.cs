
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace WellnessNutricion.Api.Migrations
{
    public partial class ClienteIdAutoincremental : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Clientes");

            migrationBuilder.CreateTable(
                name: "Clientes",
                columns: table => new
                {
                    Id = table.Column<int>(
                        type: "integer",
                        nullable: false)
                        .Annotation(
                            "Npgsql:ValueGenerationStrategy",
                            NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),

                    Nombre = table.Column<string>(
                        type: "character varying(150)",
                        maxLength: 150,
                        nullable: false),

                    Email = table.Column<string>(
                        type: "character varying(150)",
                        maxLength: 150,
                        nullable: false),

                    Telefono = table.Column<string>(
                        type: "character varying(30)",
                        maxLength: 30,
                        nullable: true),

                    Edad = table.Column<int>(
                        type: "integer",
                        nullable: true),

                    Objetivo = table.Column<string>(
                        type: "character varying(50)",
                        maxLength: 50,
                        nullable: true),

                    Estado = table.Column<string>(
                        type: "character varying(20)",
                        maxLength: 20,
                        nullable: false),

                    FenotipoCodigo = table.Column<string>(
                        type: "character varying(30)",
                        maxLength: 30,
                        nullable: true),

                    PlanId = table.Column<Guid>(
                        type: "uuid",
                        nullable: true),

                    FechaInscripcion = table.Column<DateTime>(
                        type: "timestamp with time zone",
                        nullable: false),

                    FechaContacto = table.Column<DateTime>(
                        type: "timestamp with time zone",
                        nullable: true),

                    Notas = table.Column<string>(
                        type: "text",
                        nullable: true),

                    FlagPagoRecibido = table.Column<bool>(
                        type: "boolean",
                        nullable: false),

                    FlagEmailEnviado = table.Column<bool>(
                        type: "boolean",
                        nullable: false),

                    FlagEfcaCompletado = table.Column<bool>(
                        type: "boolean",
                        nullable: false),

                    FlagConsentimiento = table.Column<bool>(
                        type: "boolean",
                        nullable: false),

                    FlagContactado = table.Column<bool>(
                        type: "boolean",
                        nullable: false),

                    FlagWhatsapp = table.Column<bool>(
                        type: "boolean",
                        nullable: false),

                    CreadoEn = table.Column<DateTime>(
                        type: "timestamp with time zone",
                        nullable: false),

                    ActualizadoEn = table.Column<DateTime>(
                        type: "timestamp with time zone",
                        nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Clientes", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Clientes");
        }
    }
}
