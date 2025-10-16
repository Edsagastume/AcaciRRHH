using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace AcaciRRHH.Web.Migrations
{
    /// <inheritdoc />
    public partial class InitialFullSchema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Personas",
                columns: table => new
                {
                    IdPersona = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Nombre = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Apellidos = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    FechaNacimiento = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Genero = table.Column<string>(type: "text", nullable: false),
                    DUI = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    NIT = table.Column<string>(type: "character varying(17)", maxLength: 17, nullable: false),
                    Email = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    Telefono = table.Column<string>(type: "character varying(9)", maxLength: 9, nullable: false),
                    Direccion = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: false),
                    NumeroAFP = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    TipoAFP = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Personas", x => x.IdPersona);
                });

            migrationBuilder.CreateTable(
                name: "Atestados",
                columns: table => new
                {
                    IdAtestado = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    IdPersona = table.Column<int>(type: "integer", nullable: false),
                    TipoAtestado = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    RutaArchivo = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    NombreOriginalArchivo = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    ExtensionArchivo = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: true),
                    FechaSubida = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    FechaVencimiento = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    SubidoPor = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Activo = table.Column<bool>(type: "boolean", nullable: false),
                    Comentarios = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Atestados", x => x.IdAtestado);
                    table.ForeignKey(
                        name: "FK_Atestados_Personas_IdPersona",
                        column: x => x.IdPersona,
                        principalTable: "Personas",
                        principalColumn: "IdPersona",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Directivos",
                columns: table => new
                {
                    IdDirectivo = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    IdPersona = table.Column<int>(type: "integer", nullable: false),
                    CargoDirectivo = table.Column<string>(type: "text", nullable: false),
                    FechaInicioMandato = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    FechaFinMandato = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    TipoMiembro = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Directivos", x => x.IdDirectivo);
                    table.ForeignKey(
                        name: "FK_Directivos_Personas_IdPersona",
                        column: x => x.IdPersona,
                        principalTable: "Personas",
                        principalColumn: "IdPersona",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Empleados",
                columns: table => new
                {
                    IdEmpleado = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    IdPersona = table.Column<int>(type: "integer", nullable: false),
                    CodigoEmpleado = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    FechaContratacion = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Puesto = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Salario = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    Departamento = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Estado = table.Column<int>(type: "integer", nullable: false),
                    FechaFinContrato = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    MotivoFinalizacion = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    TipoFinalizacion = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Empleados", x => x.IdEmpleado);
                    table.ForeignKey(
                        name: "FK_Empleados_Personas_IdPersona",
                        column: x => x.IdPersona,
                        principalTable: "Personas",
                        principalColumn: "IdPersona",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "NivelesEducativos",
                columns: table => new
                {
                    IdNivelEducativo = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    IdPersona = table.Column<int>(type: "integer", nullable: false),
                    GradoAcademico = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Institucion = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    TituloObtenido = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    FechaInicio = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    FechaFin = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Comentarios = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NivelesEducativos", x => x.IdNivelEducativo);
                    table.ForeignKey(
                        name: "FK_NivelesEducativos_Personas_IdPersona",
                        column: x => x.IdPersona,
                        principalTable: "Personas",
                        principalColumn: "IdPersona",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "HistorialDirectivo",
                columns: table => new
                {
                    IdHistorialDirectivo = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    IdDirectivo = table.Column<int>(type: "integer", nullable: false),
                    Cargo = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false, defaultValue: ""),
                    FechaInicioMandato = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    FechaFinMandato = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    TipoMandato = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false, defaultValue: ""),
                    Comentarios = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    RegistradoPor = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    FechaRegistro = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HistorialDirectivo", x => x.IdHistorialDirectivo);
                    table.ForeignKey(
                        name: "FK_HistorialDirectivo_Directivos_IdDirectivo",
                        column: x => x.IdDirectivo,
                        principalTable: "Directivos",
                        principalColumn: "IdDirectivo",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CapacitacionesEmpleados",
                columns: table => new
                {
                    IdCapacitacionEmpleado = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    IdEmpleado = table.Column<int>(type: "integer", nullable: false),
                    NombreCurso = table.Column<string>(type: "text", nullable: false),
                    Institucion = table.Column<string>(type: "text", nullable: true),
                    FechaInicio = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    FechaFin = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CertificadoObtenido = table.Column<bool>(type: "boolean", nullable: false),
                    RutaCertificado = table.Column<string>(type: "text", nullable: true),
                    Comentarios = table.Column<string>(type: "text", nullable: true),
                    EmpleadoIdEmpleado = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CapacitacionesEmpleados", x => x.IdCapacitacionEmpleado);
                    table.ForeignKey(
                        name: "FK_CapacitacionesEmpleados_Empleados_EmpleadoIdEmpleado",
                        column: x => x.EmpleadoIdEmpleado,
                        principalTable: "Empleados",
                        principalColumn: "IdEmpleado",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EvaluacionesDesempeno",
                columns: table => new
                {
                    IdEvaluacion = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    IdEmpleado = table.Column<int>(type: "integer", nullable: false),
                    FechaEvaluacion = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Periodo = table.Column<string>(type: "text", nullable: false),
                    Puntuacion = table.Column<decimal>(type: "numeric", nullable: false),
                    Evaluador = table.Column<string>(type: "text", nullable: true),
                    Comentarios = table.Column<string>(type: "text", nullable: true),
                    RutaInforme = table.Column<string>(type: "text", nullable: true),
                    EmpleadoIdEmpleado = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EvaluacionesDesempeno", x => x.IdEvaluacion);
                    table.ForeignKey(
                        name: "FK_EvaluacionesDesempeno_Empleados_EmpleadoIdEmpleado",
                        column: x => x.EmpleadoIdEmpleado,
                        principalTable: "Empleados",
                        principalColumn: "IdEmpleado",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "HistorialDisciplinario",
                columns: table => new
                {
                    IdIncidente = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    IdEmpleado = table.Column<int>(type: "integer", nullable: false),
                    FechaIncidente = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    TipoIncidente = table.Column<string>(type: "text", nullable: false),
                    Descripcion = table.Column<string>(type: "text", nullable: false),
                    AccionTomada = table.Column<string>(type: "text", nullable: false),
                    RutaIncidente = table.Column<string>(type: "text", nullable: true),
                    RegistradoPor = table.Column<string>(type: "text", nullable: true),
                    FechaRegistro = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    EmpleadoIdEmpleado = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HistorialDisciplinario", x => x.IdIncidente);
                    table.ForeignKey(
                        name: "FK_HistorialDisciplinario_Empleados_EmpleadoIdEmpleado",
                        column: x => x.EmpleadoIdEmpleado,
                        principalTable: "Empleados",
                        principalColumn: "IdEmpleado",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "HistorialPuestos",
                columns: table => new
                {
                    IdHistorialPuesto = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    IdEmpleado = table.Column<int>(type: "integer", nullable: false),
                    Puesto = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    FechaInicioPuesto = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    FechaFinPuesto = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HistorialPuestos", x => x.IdHistorialPuesto);
                    table.ForeignKey(
                        name: "FK_HistorialPuestos_Empleados_IdEmpleado",
                        column: x => x.IdEmpleado,
                        principalTable: "Empleados",
                        principalColumn: "IdEmpleado",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TrayectoriaEmpleado",
                columns: table => new
                {
                    IdHistorialLaboral = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    EmpleadoIdEmpleado = table.Column<int>(type: "integer", nullable: false),
                    FechaCambio = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    TipoCambio = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    PuestoAnterior = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    PuestoNuevo = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    SalarioAnterior = table.Column<decimal>(type: "numeric(18,2)", nullable: true),
                    SalarioNuevo = table.Column<decimal>(type: "numeric(18,2)", nullable: true),
                    DepartamentoAnterior = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    DepartamentoNuevo = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    MotivoCambio = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    RegistradoPor = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    FechaRegistro = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrayectoriaEmpleado", x => x.IdHistorialLaboral);
                    table.ForeignKey(
                        name: "FK_TrayectoriaEmpleado_Empleados_EmpleadoIdEmpleado",
                        column: x => x.EmpleadoIdEmpleado,
                        principalTable: "Empleados",
                        principalColumn: "IdEmpleado",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Atestados_IdPersona",
                table: "Atestados",
                column: "IdPersona");

            migrationBuilder.CreateIndex(
                name: "IX_CapacitacionesEmpleados_EmpleadoIdEmpleado",
                table: "CapacitacionesEmpleados",
                column: "EmpleadoIdEmpleado");

            migrationBuilder.CreateIndex(
                name: "IX_Directivos_IdPersona",
                table: "Directivos",
                column: "IdPersona",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Empleados_IdPersona",
                table: "Empleados",
                column: "IdPersona",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_EvaluacionesDesempeno_EmpleadoIdEmpleado",
                table: "EvaluacionesDesempeno",
                column: "EmpleadoIdEmpleado");

            migrationBuilder.CreateIndex(
                name: "IX_HistorialDirectivo_IdDirectivo",
                table: "HistorialDirectivo",
                column: "IdDirectivo");

            migrationBuilder.CreateIndex(
                name: "IX_HistorialDisciplinario_EmpleadoIdEmpleado",
                table: "HistorialDisciplinario",
                column: "EmpleadoIdEmpleado");

            migrationBuilder.CreateIndex(
                name: "IX_HistorialPuestos_IdEmpleado",
                table: "HistorialPuestos",
                column: "IdEmpleado");

            migrationBuilder.CreateIndex(
                name: "IX_NivelesEducativos_IdPersona",
                table: "NivelesEducativos",
                column: "IdPersona");

            migrationBuilder.CreateIndex(
                name: "IX_Personas_DUI",
                table: "Personas",
                column: "DUI",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Personas_NIT",
                table: "Personas",
                column: "NIT",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TrayectoriaEmpleado_EmpleadoIdEmpleado",
                table: "TrayectoriaEmpleado",
                column: "EmpleadoIdEmpleado");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Atestados");

            migrationBuilder.DropTable(
                name: "CapacitacionesEmpleados");

            migrationBuilder.DropTable(
                name: "EvaluacionesDesempeno");

            migrationBuilder.DropTable(
                name: "HistorialDirectivo");

            migrationBuilder.DropTable(
                name: "HistorialDisciplinario");

            migrationBuilder.DropTable(
                name: "HistorialPuestos");

            migrationBuilder.DropTable(
                name: "NivelesEducativos");

            migrationBuilder.DropTable(
                name: "TrayectoriaEmpleado");

            migrationBuilder.DropTable(
                name: "Directivos");

            migrationBuilder.DropTable(
                name: "Empleados");

            migrationBuilder.DropTable(
                name: "Personas");
        }
    }
}
