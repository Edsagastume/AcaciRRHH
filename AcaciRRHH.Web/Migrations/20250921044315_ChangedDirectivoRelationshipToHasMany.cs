using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AcaciRRHH.Web.Migrations
{
    /// <inheritdoc />
    public partial class ChangedDirectivoRelationshipToHasMany : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Directivos_IdPersona",
                table: "Directivos");

            migrationBuilder.CreateIndex(
                name: "IX_Directivos_IdPersona",
                table: "Directivos",
                column: "IdPersona");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Directivos_IdPersona",
                table: "Directivos");

            migrationBuilder.CreateIndex(
                name: "IX_Directivos_IdPersona",
                table: "Directivos",
                column: "IdPersona",
                unique: true);
        }
    }
}
