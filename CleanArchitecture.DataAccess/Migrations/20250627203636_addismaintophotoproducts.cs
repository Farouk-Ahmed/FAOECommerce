using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace CleanArchitecture.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class addismaintophotoproducts : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "7cb86501-acea-4abd-b7e6-e9dc32036fc8");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "d247bbba-35dc-448b-83c3-8f09e20c2da6");

            migrationBuilder.AddColumn<bool>(
                name: "IsMain",
                table: "ProductPhotos",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "5f431e21-1c62-422f-8311-c23873fb50fc", "1", "Admin", "Admin" },
                    { "c709d607-77ce-4c25-914c-403ade3f4567", "2", "User", "User" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "5f431e21-1c62-422f-8311-c23873fb50fc");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "c709d607-77ce-4c25-914c-403ade3f4567");

            migrationBuilder.DropColumn(
                name: "IsMain",
                table: "ProductPhotos");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "7cb86501-acea-4abd-b7e6-e9dc32036fc8", "2", "User", "User" },
                    { "d247bbba-35dc-448b-83c3-8f09e20c2da6", "1", "Admin", "Admin" }
                });
        }
    }
}
