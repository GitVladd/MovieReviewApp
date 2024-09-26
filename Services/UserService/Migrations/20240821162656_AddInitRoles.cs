using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace UserService.Migrations
{
    /// <inheritdoc />
    public partial class AddInitRoles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { new Guid("1bc69609-c82d-4851-a2fb-b5a421c7172d"), null, "Admin", "ADMIN" },
                    { new Guid("21bd6e0a-1ec0-48a8-ba70-10985b72a598"), null, "User", "USER" },
                    { new Guid("ece34c76-9309-44ae-8bbc-ab80eaab8541"), null, "Manager", "MANAGER" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("1bc69609-c82d-4851-a2fb-b5a421c7172d"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("21bd6e0a-1ec0-48a8-ba70-10985b72a598"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("ece34c76-9309-44ae-8bbc-ab80eaab8541"));
        }
    }
}
