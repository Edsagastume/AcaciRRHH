using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AcaciRRHH.Web.Migrations
{
    /// <inheritdoc />
    public partial class UpdatedGeneroToEnum : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"ALTER TABLE ""Personas"" ALTER COLUMN ""Genero"" TYPE integer USING
                CASE
                    WHEN ""Genero"" = 'Masculino' THEN 0
                    WHEN ""Genero"" = 'Femenino' THEN 1
                    ELSE 0
                END;");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Genero",
                table: "Personas",
                type: "text",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");
        }
    }
}
