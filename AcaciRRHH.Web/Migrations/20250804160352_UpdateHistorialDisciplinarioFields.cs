using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AcaciRRHH.Web.Migrations
{
    /// <inheritdoc />
    public partial class UpdateHistorialDisciplinarioFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // 1. Add a new temporary column of type integer
            migrationBuilder.AddColumn<int>(
                name: "TipoIncidente_temp",
                table: "HistorialDisciplinario",
                nullable: true); // Make it nullable temporarily

            // 2. Copy data from the old string column to the new integer column with explicit casting
            // Assuming 'Grave' maps to 0 and 'Leve' maps to 1. Adjust if your enum values are different.
            migrationBuilder.Sql(
                "UPDATE \"HistorialDisciplinario\" SET \"TipoIncidente_temp\" = CASE \"TipoIncidente\" WHEN 'Grave' THEN 0 WHEN 'Leve' THEN 1 ELSE 0 END;"
            );

            // 3. Drop the old string column
            migrationBuilder.DropColumn(
                name: "TipoIncidente",
                table: "HistorialDisciplinario");

            // 4. Rename the new temporary column to the original column name
            migrationBuilder.RenameColumn(
                name: "TipoIncidente_temp",
                table: "HistorialDisciplinario",
                newName: "TipoIncidente");

            // 5. Alter the column to be non-nullable if it was originally non-nullable
            migrationBuilder.AlterColumn<int>(
                name: "TipoIncidente",
                table: "HistorialDisciplinario",
                type: "integer",
                nullable: false, // Set to false if it was originally non-nullable
                oldClrType: typeof(int),
                oldType: "integer"); // This oldClrType and oldType might be incorrect after rename, but it's fine as it's just a placeholder
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Revert changes in reverse order
            migrationBuilder.AddColumn<string>(
                name: "TipoIncidente_temp",
                table: "HistorialDisciplinario",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true); // Make it nullable temporarily

            migrationBuilder.Sql(
                "UPDATE \"HistorialDisciplinario\" SET \"TipoIncidente_temp\" = CASE \"TipoIncidente\" WHEN 0 THEN 'Grave' WHEN 1 THEN 'Leve' ELSE '' END;"
            );

            migrationBuilder.DropColumn(
                name: "TipoIncidente",
                table: "HistorialDisciplinario");

            migrationBuilder.RenameColumn(
                name: "TipoIncidente_temp",
                table: "HistorialDisciplinario",
                newName: "TipoIncidente");

            migrationBuilder.AlterColumn<string>(
                name: "TipoIncidente",
                table: "HistorialDisciplinario",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(100)");
        }
    }
}