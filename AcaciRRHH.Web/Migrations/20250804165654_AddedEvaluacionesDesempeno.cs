using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AcaciRRHH.Web.Migrations
{
    /// <inheritdoc />
    public partial class AddedEvaluacionesDesempeno : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EvaluacionesDesempeno_Empleados_EmpleadoIdEmpleado",
                table: "EvaluacionesDesempeno");

            migrationBuilder.DropIndex(
                name: "IX_EvaluacionesDesempeno_EmpleadoIdEmpleado",
                table: "EvaluacionesDesempeno");

            migrationBuilder.DropColumn(
                name: "EmpleadoIdEmpleado",
                table: "EvaluacionesDesempeno");

            migrationBuilder.CreateIndex(
                name: "IX_EvaluacionesDesempeno_IdEmpleado",
                table: "EvaluacionesDesempeno",
                column: "IdEmpleado");

            migrationBuilder.AddForeignKey(
                name: "FK_EvaluacionesDesempeno_Empleados_IdEmpleado",
                table: "EvaluacionesDesempeno",
                column: "IdEmpleado",
                principalTable: "Empleados",
                principalColumn: "IdEmpleado",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EvaluacionesDesempeno_Empleados_IdEmpleado",
                table: "EvaluacionesDesempeno");

            migrationBuilder.DropIndex(
                name: "IX_EvaluacionesDesempeno_IdEmpleado",
                table: "EvaluacionesDesempeno");

            migrationBuilder.AddColumn<int>(
                name: "EmpleadoIdEmpleado",
                table: "EvaluacionesDesempeno",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_EvaluacionesDesempeno_EmpleadoIdEmpleado",
                table: "EvaluacionesDesempeno",
                column: "EmpleadoIdEmpleado");

            migrationBuilder.AddForeignKey(
                name: "FK_EvaluacionesDesempeno_Empleados_EmpleadoIdEmpleado",
                table: "EvaluacionesDesempeno",
                column: "EmpleadoIdEmpleado",
                principalTable: "Empleados",
                principalColumn: "IdEmpleado");
        }
    }
}
