using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AcaciRRHH.Web.Migrations
{
    /// <inheritdoc />
    public partial class SeedTestUsers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "ApplicationRoles",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Administrador" },
                    { 2, "Consulta" }
                });

            var hasher = new Microsoft.AspNetCore.Identity.PasswordHasher<AcaciRRHH.Web.Models.ApplicationUser>();

            migrationBuilder.InsertData(
                table: "ApplicationUsers",
                columns: new[] { "Id", "UserName", "PasswordHash" },
                values: new object[,]
                {
                    { 1, "admin", hasher.HashPassword(null, "admin123") },
                    { 2, "consulta", hasher.HashPassword(null, "consulta123") }
                });

            migrationBuilder.InsertData(
                table: "ApplicationUserRoles",
                columns: new[] { "UserId", "RoleId" },
                values: new object[,]
                {
                    { 1, 1 }, // admin is Administrador
                    { 2, 2 }  // consulta is Consulta
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "ApplicationUserRoles",
                keyColumns: new[] { "UserId", "RoleId" },
                keyValues: new object[,]
                {
                    { 1, 1 },
                    { 2, 2 }
                });

            migrationBuilder.DeleteData(
                table: "ApplicationUsers",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "ApplicationUsers",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "ApplicationRoles",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "ApplicationRoles",
                keyColumn: "Id",
                keyValue: 2);
        }
    }
}