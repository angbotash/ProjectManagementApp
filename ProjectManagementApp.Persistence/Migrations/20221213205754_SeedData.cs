using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ProjectManagementApp.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class SeedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { 1, "e10dcc3f-a309-441f-9752-a6814f6162cf", "Supervisor", "SUPERVISOR" },
                    { 2, "fd21c7cd-8658-45cf-bd8c-d4ffac3c0ae5", "Manager", "MANAGER" },
                    { 3, "fde66fdb-68cf-444e-9b69-f12141c880f8", "Employee", "EMPLOYEE" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "FirstName", "LastName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "Patronymic", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { 1, 0, "d39d71ac-1b00-404f-98a9-dfda3307cef8", "supervisor@email.com", false, "Super", "Visor", false, null, "SUPERVISOR@EMAIL.COM", "SUPERVISOR@EMAIL.COM", "AQAAAAEAACcQAAAAEC2+9TV9xIze8N9n2cQa7/cfSvjioZI6/rYUhcwxWMNUd4zGSmAt5ahUKNkwFV0s0Q==", null, null, false, null, false, "supervisor@email.com" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1);
        }
    }
}
