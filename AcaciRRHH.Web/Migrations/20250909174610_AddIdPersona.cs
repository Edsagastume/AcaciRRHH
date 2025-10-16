using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AcaciRRHH.Web.Migrations
{
    /// <inheritdoc />
    public partial class AddIdPersona : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "IdPersona",
                table: "HistorialDisciplinario",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "PersonaIdPersona",
                table: "HistorialDisciplinario",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_HistorialDisciplinario_PersonaIdPersona",
                table: "HistorialDisciplinario",
                column: "PersonaIdPersona");

            migrationBuilder.AddForeignKey(
                name: "FK_HistorialDisciplinario_Personas_PersonaIdPersona",
                table: "HistorialDisciplinario",
                column: "PersonaIdPersona",
                principalTable: "Personas",
                principalColumn: "IdPersona");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HistorialDisciplinario_Personas_PersonaIdPersona",
                table: "HistorialDisciplinario");

            migrationBuilder.DropIndex(
                name: "IX_HistorialDisciplinario_PersonaIdPersona",
                table: "HistorialDisciplinario");

            migrationBuilder.DropColumn(
                name: "IdPersona",
                table: "HistorialDisciplinario");

            migrationBuilder.DropColumn(
                name: "PersonaIdPersona",
                table: "HistorialDisciplinario");
        }
    }
}
