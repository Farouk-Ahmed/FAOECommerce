using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace CleanArchitecture.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddUserIDandISUSedToOts : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "212a59af-1a9f-4d1a-9587-cd3c6bf20169");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "5f1ae03d-d0d2-4e9c-8622-f0f75aac2a2f");

            migrationBuilder.AddColumn<bool>(
                name: "IsUsed",
                table: "Otps",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "Otps",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "31d95809-12c9-4d13-8115-aab165e2d9cb", "2", "User", "User" },
                    { "9c6ea579-e142-4f82-8555-b4dcd3abfeb3", "1", "Admin", "Admin" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "31d95809-12c9-4d13-8115-aab165e2d9cb");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "9c6ea579-e142-4f82-8555-b4dcd3abfeb3");

            migrationBuilder.DropColumn(
                name: "IsUsed",
                table: "Otps");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Otps");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "212a59af-1a9f-4d1a-9587-cd3c6bf20169", "1", "Admin", "Admin" },
                    { "5f1ae03d-d0d2-4e9c-8622-f0f75aac2a2f", "2", "User", "User" }
                });
        }
    }
}
