using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AcaciRRHH.Web.Migrations
{
    /// <inheritdoc />
    public partial class UpdateCapacitacionToPersona : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CapacitacionesEmpleados_Directivos_IdDirectivo",
                table: "CapacitacionesEmpleados");

            migrationBuilder.DropForeignKey(
                name: "FK_CapacitacionesEmpleados_Empleados_IdEmpleado",
                table: "CapacitacionesEmpleados");

            migrationBuilder.DropIndex(
                name: "IX_CapacitacionesEmpleados_IdDirectivo",
                table: "CapacitacionesEmpleados");

            migrationBuilder.DropColumn(
                name: "IdDirectivo",
                table: "CapacitacionesEmpleados");

            migrationBuilder.RenameColumn(
                name: "IdEmpleado",
                table: "CapacitacionesEmpleados",
                newName: "EmpleadoIdEmpleado");

            migrationBuilder.RenameIndex(
                name: "IX_CapacitacionesEmpleados_IdEmpleado",
                table: "CapacitacionesEmpleados",
                newName: "IX_CapacitacionesEmpleados_EmpleadoIdEmpleado");

            migrationBuilder.AddColumn<int>(
                name: "IdPersona",
                table: "CapacitacionesEmpleados",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_CapacitacionesEmpleados_IdPersona",
                table: "CapacitacionesEmpleados",
                column: "IdPersona");

            migrationBuilder.AddForeignKey(
                name: "FK_CapacitacionesEmpleados_Empleados_EmpleadoIdEmpleado",
                table: "CapacitacionesEmpleados",
                column: "EmpleadoIdEmpleado",
                principalTable: "Empleados",
                principalColumn: "IdEmpleado");

            migrationBuilder.AddForeignKey(
                name: "FK_CapacitacionesEmpleados_Personas_IdPersona",
                table: "CapacitacionesEmpleados",
                column: "IdPersona",
                principalTable: "Personas",
                principalColumn: "IdPersona",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CapacitacionesEmpleados_Empleados_EmpleadoIdEmpleado",
                table: "CapacitacionesEmpleados");

            migrationBuilder.DropForeignKey(
                name: "FK_CapacitacionesEmpleados_Personas_IdPersona",
                table: "CapacitacionesEmpleados");

            migrationBuilder.DropIndex(
                name: "IX_CapacitacionesEmpleados_IdPersona",
                table: "CapacitacionesEmpleados");

            migrationBuilder.DropColumn(
                name: "IdPersona",
                table: "CapacitacionesEmpleados");

            migrationBuilder.RenameColumn(
                name: "EmpleadoIdEmpleado",
                table: "CapacitacionesEmpleados",
                newName: "IdEmpleado");

            migrationBuilder.RenameIndex(
                name: "IX_CapacitacionesEmpleados_EmpleadoIdEmpleado",
                table: "CapacitacionesEmpleados",
                newName: "IX_CapacitacionesEmpleados_IdEmpleado");

            migrationBuilder.AddColumn<int>(
                name: "IdDirectivo",
                table: "CapacitacionesEmpleados",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_CapacitacionesEmpleados_IdDirectivo",
                table: "CapacitacionesEmpleados",
                column: "IdDirectivo");

            migrationBuilder.AddForeignKey(
                name: "FK_CapacitacionesEmpleados_Directivos_IdDirectivo",
                table: "CapacitacionesEmpleados",
                column: "IdDirectivo",
                principalTable: "Directivos",
                principalColumn: "IdDirectivo",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CapacitacionesEmpleados_Empleados_IdEmpleado",
                table: "CapacitacionesEmpleados",
                column: "IdEmpleado",
                principalTable: "Empleados",
                principalColumn: "IdEmpleado",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
