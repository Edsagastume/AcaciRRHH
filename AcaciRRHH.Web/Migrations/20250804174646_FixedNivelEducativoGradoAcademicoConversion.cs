using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AcaciRRHH.Web.Migrations
{
    /// <inheritdoc />
    public partial class FixedNivelEducativoGradoAcademicoConversion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "GradoAcademico",
                table: "NivelesEducativos",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "GradoAcademico",
                table: "NivelesEducativos",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");
        }
    }
}
