using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace MovieService.Migrations
{
    /// <inheritdoc />
    public partial class FixedInitTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("a2033ac8-7782-42ad-b4a1-3b6b450677b3"));

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("e391c4be-d8f0-460a-a5eb-ab66880f700c"));

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("e4357d6d-baf7-459a-8138-f2dc1a79806b"));

            migrationBuilder.DeleteData(
                table: "ContentTypes",
                keyColumn: "Id",
                keyValue: new Guid("427b030e-e5d6-4bd2-bf4f-53d224f58d54"));

            migrationBuilder.DeleteData(
                table: "ContentTypes",
                keyColumn: "Id",
                keyValue: new Guid("cf875881-d348-4d85-aeee-aee25d262332"));

            migrationBuilder.DeleteData(
                table: "ContentTypes",
                keyColumn: "Id",
                keyValue: new Guid("cfd227f1-8017-4af5-bb51-2774c7417506"));

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { new Guid("02acc9d1-6563-439a-9ba8-a005a1738904"), "Action" },
                    { new Guid("42ab8f16-fbc4-4feb-8fb8-922138363b9f"), "Drama" },
                    { new Guid("4c192c13-2b23-48e9-85c9-e17117f51742"), "Comedy" }
                });

            migrationBuilder.InsertData(
                table: "ContentTypes",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { new Guid("77d144bd-c293-4f3a-b636-0d33c617cb8f"), "Anime" },
                    { new Guid("c71e0e7a-6b24-4092-85ea-a45a391a1072"), "Series" },
                    { new Guid("edefcb4e-5ade-4928-9f91-ed7f5eeb6dd2"), "Movie" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("02acc9d1-6563-439a-9ba8-a005a1738904"));

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("42ab8f16-fbc4-4feb-8fb8-922138363b9f"));

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("4c192c13-2b23-48e9-85c9-e17117f51742"));

            migrationBuilder.DeleteData(
                table: "ContentTypes",
                keyColumn: "Id",
                keyValue: new Guid("77d144bd-c293-4f3a-b636-0d33c617cb8f"));

            migrationBuilder.DeleteData(
                table: "ContentTypes",
                keyColumn: "Id",
                keyValue: new Guid("c71e0e7a-6b24-4092-85ea-a45a391a1072"));

            migrationBuilder.DeleteData(
                table: "ContentTypes",
                keyColumn: "Id",
                keyValue: new Guid("edefcb4e-5ade-4928-9f91-ed7f5eeb6dd2"));

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { new Guid("a2033ac8-7782-42ad-b4a1-3b6b450677b3"), "Movie" },
                    { new Guid("e391c4be-d8f0-460a-a5eb-ab66880f700c"), "Series" },
                    { new Guid("e4357d6d-baf7-459a-8138-f2dc1a79806b"), "Anime" }
                });

            migrationBuilder.InsertData(
                table: "ContentTypes",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { new Guid("427b030e-e5d6-4bd2-bf4f-53d224f58d54"), "Action" },
                    { new Guid("cf875881-d348-4d85-aeee-aee25d262332"), "Comedy" },
                    { new Guid("cfd227f1-8017-4af5-bb51-2774c7417506"), "Drama" }
                });
        }
    }
}
