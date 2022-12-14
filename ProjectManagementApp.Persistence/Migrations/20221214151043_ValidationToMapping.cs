using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjectManagementApp.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class ValidationToMapping : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "13518520-ee83-42e8-9733-4ba482a1ed21", "AQAAAAEAACcQAAAAEP3OB9h4Ky7jV2nDdFOnKQ/Kodm9mtYOUUpTmP9TP5ZesSEyeZG6dL2k4JCCeXwhfQ==" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "142a6a94-f5b2-45d2-aa54-620a2e58cc3c", "AQAAAAEAACcQAAAAEJ+BzOTcyJLGs5M/8e9wEPPWctUJIw0JWLdtnp690LdirinhmzG2Dsh5MmrjrFJ2xQ==" });
        }
    }
}
