using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AcaciRRHH.Web.Migrations
{
    /// <inheritdoc />
    public partial class AddAuditableProperties : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EvaluacionesDesempeno_Empleados_EmpleadoIdEmpleado",
                table: "EvaluacionesDesempeno");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "TrayectoriaEmpleado",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "TrayectoriaEmpleado",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastModifiedAt",
                table: "TrayectoriaEmpleado",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "LastModifiedBy",
                table: "TrayectoriaEmpleado",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Personas",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "Personas",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastModifiedAt",
                table: "Personas",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "LastModifiedBy",
                table: "Personas",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "NivelesEducativos",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "NivelesEducativos",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastModifiedAt",
                table: "NivelesEducativos",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "LastModifiedBy",
                table: "NivelesEducativos",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "HistorialPuestos",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "HistorialPuestos",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastModifiedAt",
                table: "HistorialPuestos",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "LastModifiedBy",
                table: "HistorialPuestos",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "HistorialDisciplinario",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "HistorialDisciplinario",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastModifiedAt",
                table: "HistorialDisciplinario",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "LastModifiedBy",
                table: "HistorialDisciplinario",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "HistorialDirectivo",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "HistorialDirectivo",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastModifiedAt",
                table: "HistorialDirectivo",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "LastModifiedBy",
                table: "HistorialDirectivo",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "EmpleadoIdEmpleado",
                table: "EvaluacionesDesempeno",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "EvaluacionesDesempeno",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "EvaluacionesDesempeno",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastModifiedAt",
                table: "EvaluacionesDesempeno",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "LastModifiedBy",
                table: "EvaluacionesDesempeno",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Empleados",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "Empleados",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastModifiedAt",
                table: "Empleados",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "LastModifiedBy",
                table: "Empleados",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Directivos",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "Directivos",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastModifiedAt",
                table: "Directivos",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "LastModifiedBy",
                table: "Directivos",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "CapacitacionesEmpleados",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "CapacitacionesEmpleados",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastModifiedAt",
                table: "CapacitacionesEmpleados",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "LastModifiedBy",
                table: "CapacitacionesEmpleados",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Atestados",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "Atestados",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastModifiedAt",
                table: "Atestados",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "LastModifiedBy",
                table: "Atestados",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_EvaluacionesDesempeno_Empleados_EmpleadoIdEmpleado",
                table: "EvaluacionesDesempeno",
                column: "EmpleadoIdEmpleado",
                principalTable: "Empleados",
                principalColumn: "IdEmpleado");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EvaluacionesDesempeno_Empleados_EmpleadoIdEmpleado",
                table: "EvaluacionesDesempeno");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "TrayectoriaEmpleado");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "TrayectoriaEmpleado");

            migrationBuilder.DropColumn(
                name: "LastModifiedAt",
                table: "TrayectoriaEmpleado");

            migrationBuilder.DropColumn(
                name: "LastModifiedBy",
                table: "TrayectoriaEmpleado");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Personas");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "Personas");

            migrationBuilder.DropColumn(
                name: "LastModifiedAt",
                table: "Personas");

            migrationBuilder.DropColumn(
                name: "LastModifiedBy",
                table: "Personas");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "NivelesEducativos");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "NivelesEducativos");

            migrationBuilder.DropColumn(
                name: "LastModifiedAt",
                table: "NivelesEducativos");

            migrationBuilder.DropColumn(
                name: "LastModifiedBy",
                table: "NivelesEducativos");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "HistorialPuestos");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "HistorialPuestos");

            migrationBuilder.DropColumn(
                name: "LastModifiedAt",
                table: "HistorialPuestos");

            migrationBuilder.DropColumn(
                name: "LastModifiedBy",
                table: "HistorialPuestos");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "HistorialDisciplinario");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "HistorialDisciplinario");

            migrationBuilder.DropColumn(
                name: "LastModifiedAt",
                table: "HistorialDisciplinario");

            migrationBuilder.DropColumn(
                name: "LastModifiedBy",
                table: "HistorialDisciplinario");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "HistorialDirectivo");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "HistorialDirectivo");

            migrationBuilder.DropColumn(
                name: "LastModifiedAt",
                table: "HistorialDirectivo");

            migrationBuilder.DropColumn(
                name: "LastModifiedBy",
                table: "HistorialDirectivo");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "EvaluacionesDesempeno");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "EvaluacionesDesempeno");

            migrationBuilder.DropColumn(
                name: "LastModifiedAt",
                table: "EvaluacionesDesempeno");

            migrationBuilder.DropColumn(
                name: "LastModifiedBy",
                table: "EvaluacionesDesempeno");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Empleados");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "Empleados");

            migrationBuilder.DropColumn(
                name: "LastModifiedAt",
                table: "Empleados");

            migrationBuilder.DropColumn(
                name: "LastModifiedBy",
                table: "Empleados");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Directivos");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "Directivos");

            migrationBuilder.DropColumn(
                name: "LastModifiedAt",
                table: "Directivos");

            migrationBuilder.DropColumn(
                name: "LastModifiedBy",
                table: "Directivos");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "CapacitacionesEmpleados");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "CapacitacionesEmpleados");

            migrationBuilder.DropColumn(
                name: "LastModifiedAt",
                table: "CapacitacionesEmpleados");

            migrationBuilder.DropColumn(
                name: "LastModifiedBy",
                table: "CapacitacionesEmpleados");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Atestados");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "Atestados");

            migrationBuilder.DropColumn(
                name: "LastModifiedAt",
                table: "Atestados");

            migrationBuilder.DropColumn(
                name: "LastModifiedBy",
                table: "Atestados");

            migrationBuilder.AlterColumn<int>(
                name: "EmpleadoIdEmpleado",
                table: "EvaluacionesDesempeno",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_EvaluacionesDesempeno_Empleados_EmpleadoIdEmpleado",
                table: "EvaluacionesDesempeno",
                column: "EmpleadoIdEmpleado",
                principalTable: "Empleados",
                principalColumn: "IdEmpleado",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
