using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace CleanArchitecture.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddUserNameToOtp : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "9a3d19b9-704c-42a3-a70f-e8c81c952d48");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "fedc35d5-ad27-4400-adc5-eb02e9030623");

            migrationBuilder.AddColumn<string>(
                name: "UserName",
                table: "Otps",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "212a59af-1a9f-4d1a-9587-cd3c6bf20169", "1", "Admin", "Admin" },
                    { "5f1ae03d-d0d2-4e9c-8622-f0f75aac2a2f", "2", "User", "User" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "212a59af-1a9f-4d1a-9587-cd3c6bf20169");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "5f1ae03d-d0d2-4e9c-8622-f0f75aac2a2f");

            migrationBuilder.DropColumn(
                name: "UserName",
                table: "Otps");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "9a3d19b9-704c-42a3-a70f-e8c81c952d48", "1", "Admin", "Admin" },
                    { "fedc35d5-ad27-4400-adc5-eb02e9030623", "2", "User", "User" }
                });
        }
    }
}
