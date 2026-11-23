using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace WellnessNutricion.Api.Migrations
{
    /// <inheritdoc />
    public partial class CrearEstructuraTestEFCA : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TestSesiones",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ClienteId = table.Column<int>(type: "integer", nullable: false),
                    FechaInicio = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    FechaFinalizacion = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Estado = table.Column<string>(type: "text", nullable: false),
                    FenotipoPredominante = table.Column<string>(type: "text", nullable: true),
                    NivelPredominante = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TestSesiones", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TestSesiones_Clientes_ClienteId",
                        column: x => x.ClienteId,
                        principalTable: "Clientes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TestRespuestas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TestSesionId = table.Column<int>(type: "integer", nullable: false),
                    NumeroPregunta = table.Column<int>(type: "integer", nullable: false),
                    ValorRespuesta = table.Column<int>(type: "integer", nullable: false),
                    CreadoEn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TestRespuestas", x => x.Id);
                    table.CheckConstraint("CK_TestRespuestas_NumeroPregunta", "\"NumeroPregunta\" BETWEEN 1 AND 16");
                    table.CheckConstraint("CK_TestRespuestas_ValorRespuesta", "\"ValorRespuesta\" BETWEEN 1 AND 5");
                    table.ForeignKey(
                        name: "FK_TestRespuestas_TestSesiones_TestSesionId",
                        column: x => x.TestSesionId,
                        principalTable: "TestSesiones",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TestRespuestas_TestSesionId_NumeroPregunta",
                table: "TestRespuestas",
                columns: new[] { "TestSesionId", "NumeroPregunta" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TestSesiones_ClienteId",
                table: "TestSesiones",
                column: "ClienteId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TestRespuestas");

            migrationBuilder.DropTable(
                name: "TestSesiones");
        }
    }
}
