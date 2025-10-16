using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AcaciRRHH.Web.Migrations
{
    /// <inheritdoc />
    public partial class ConfigureForeignKeyForPersonaInEvaluacion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_EvaluacionesDesempeno_IdPersona",
                table: "EvaluacionesDesempeno",
                column: "IdPersona");

            migrationBuilder.AddForeignKey(
                name: "FK_EvaluacionesDesempeno_Personas_IdPersona",
                table: "EvaluacionesDesempeno",
                column: "IdPersona",
                principalTable: "Personas",
                principalColumn: "IdPersona",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EvaluacionesDesempeno_Personas_IdPersona",
                table: "EvaluacionesDesempeno");

            migrationBuilder.DropIndex(
                name: "IX_EvaluacionesDesempeno_IdPersona",
                table: "EvaluacionesDesempeno");
        }
    }
}
