using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AcaciRRHH.Web.Migrations
{
    /// <inheritdoc />
    public partial class UpdatedTipoAFPToEnum : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"ALTER TABLE ""Personas"" ALTER COLUMN ""TipoAFP"" TYPE integer USING
                CASE
                    WHEN ""TipoAFP"" = 'AFP Crecer' THEN 0
                    WHEN ""TipoAFP"" = 'AFP Confia' THEN 1
                    ELSE NULL
                END;");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "TipoAFP",
                table: "Personas",
                type: "character varying(50)",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);
        }
    }
}
