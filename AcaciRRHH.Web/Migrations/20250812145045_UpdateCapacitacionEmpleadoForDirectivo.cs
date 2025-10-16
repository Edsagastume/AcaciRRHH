using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AcaciRRHH.Web.Migrations
{
    /// <inheritdoc />
    public partial class UpdateCapacitacionEmpleadoForDirectivo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "FechaInicio",
                table: "NivelesEducativos",
                type: "timestamp with time zone",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");

            migrationBuilder.AlterColumn<int>(
                name: "IdEmpleado",
                table: "CapacitacionesEmpleados",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

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
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CapacitacionesEmpleados_Directivos_IdDirectivo",
                table: "CapacitacionesEmpleados");

            migrationBuilder.DropIndex(
                name: "IX_CapacitacionesEmpleados_IdDirectivo",
                table: "CapacitacionesEmpleados");

            migrationBuilder.DropColumn(
                name: "IdDirectivo",
                table: "CapacitacionesEmpleados");

            migrationBuilder.AlterColumn<DateTime>(
                name: "FechaInicio",
                table: "NivelesEducativos",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "IdEmpleado",
                table: "CapacitacionesEmpleados",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);
        }
    }
}
