using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AcaciRRHH.Web.Migrations
{
    /// <inheritdoc />
    public partial class AddedHistorialDisciplinario : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HistorialDisciplinario_Empleados_EmpleadoIdEmpleado",
                table: "HistorialDisciplinario");

            migrationBuilder.DropIndex(
                name: "IX_HistorialDisciplinario_EmpleadoIdEmpleado",
                table: "HistorialDisciplinario");

            migrationBuilder.DropColumn(
                name: "EmpleadoIdEmpleado",
                table: "HistorialDisciplinario");

            migrationBuilder.AlterColumn<string>(
                name: "TipoIncidente",
                table: "HistorialDisciplinario",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "RegistradoPor",
                table: "HistorialDisciplinario",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_HistorialDisciplinario_IdEmpleado",
                table: "HistorialDisciplinario",
                column: "IdEmpleado");

            migrationBuilder.AddForeignKey(
                name: "FK_HistorialDisciplinario_Empleados_IdEmpleado",
                table: "HistorialDisciplinario",
                column: "IdEmpleado",
                principalTable: "Empleados",
                principalColumn: "IdEmpleado",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HistorialDisciplinario_Empleados_IdEmpleado",
                table: "HistorialDisciplinario");

            migrationBuilder.DropIndex(
                name: "IX_HistorialDisciplinario_IdEmpleado",
                table: "HistorialDisciplinario");

            migrationBuilder.AlterColumn<string>(
                name: "TipoIncidente",
                table: "HistorialDisciplinario",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "RegistradoPor",
                table: "HistorialDisciplinario",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "EmpleadoIdEmpleado",
                table: "HistorialDisciplinario",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_HistorialDisciplinario_EmpleadoIdEmpleado",
                table: "HistorialDisciplinario",
                column: "EmpleadoIdEmpleado");

            migrationBuilder.AddForeignKey(
                name: "FK_HistorialDisciplinario_Empleados_EmpleadoIdEmpleado",
                table: "HistorialDisciplinario",
                column: "EmpleadoIdEmpleado",
                principalTable: "Empleados",
                principalColumn: "IdEmpleado",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
