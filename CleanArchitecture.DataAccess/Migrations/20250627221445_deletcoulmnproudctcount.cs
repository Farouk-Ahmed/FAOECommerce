using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace CleanArchitecture.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class deletcoulmnproudctcount : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "05761e76-f3e2-4327-acbd-ca7e03faae13");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "e6db490d-be7c-42ab-99c0-c4ee96c439dc");

            migrationBuilder.DropColumn(
                name: "ProductCount",
                table: "Categories");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "3dd79ec6-9e06-474c-9348-d20ceffc37d6", "2", "User", "User" },
                    { "c8d39481-4921-4399-a346-5d8f97ea23d4", "1", "Admin", "Admin" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "3dd79ec6-9e06-474c-9348-d20ceffc37d6");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "c8d39481-4921-4399-a346-5d8f97ea23d4");

            migrationBuilder.AddColumn<int>(
                name: "ProductCount",
                table: "Categories",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "05761e76-f3e2-4327-acbd-ca7e03faae13", "2", "User", "User" },
                    { "e6db490d-be7c-42ab-99c0-c4ee96c439dc", "1", "Admin", "Admin" }
                });
        }
    }
}
