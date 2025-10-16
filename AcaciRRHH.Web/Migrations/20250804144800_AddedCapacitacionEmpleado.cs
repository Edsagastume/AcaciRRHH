using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AcaciRRHH.Web.Migrations
{
    /// <inheritdoc />
    public partial class AddedCapacitacionEmpleado : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CapacitacionesEmpleados_Empleados_EmpleadoIdEmpleado",
                table: "CapacitacionesEmpleados");

            migrationBuilder.DropIndex(
                name: "IX_CapacitacionesEmpleados_EmpleadoIdEmpleado",
                table: "CapacitacionesEmpleados");

            migrationBuilder.DropColumn(
                name: "EmpleadoIdEmpleado",
                table: "CapacitacionesEmpleados");

            migrationBuilder.CreateIndex(
                name: "IX_CapacitacionesEmpleados_IdEmpleado",
                table: "CapacitacionesEmpleados",
                column: "IdEmpleado");

            migrationBuilder.AddForeignKey(
                name: "FK_CapacitacionesEmpleados_Empleados_IdEmpleado",
                table: "CapacitacionesEmpleados",
                column: "IdEmpleado",
                principalTable: "Empleados",
                principalColumn: "IdEmpleado",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CapacitacionesEmpleados_Empleados_IdEmpleado",
                table: "CapacitacionesEmpleados");

            migrationBuilder.DropIndex(
                name: "IX_CapacitacionesEmpleados_IdEmpleado",
                table: "CapacitacionesEmpleados");

            migrationBuilder.AddColumn<int>(
                name: "EmpleadoIdEmpleado",
                table: "CapacitacionesEmpleados",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_CapacitacionesEmpleados_EmpleadoIdEmpleado",
                table: "CapacitacionesEmpleados",
                column: "EmpleadoIdEmpleado");

            migrationBuilder.AddForeignKey(
                name: "FK_CapacitacionesEmpleados_Empleados_EmpleadoIdEmpleado",
                table: "CapacitacionesEmpleados",
                column: "EmpleadoIdEmpleado",
                principalTable: "Empleados",
                principalColumn: "IdEmpleado",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
