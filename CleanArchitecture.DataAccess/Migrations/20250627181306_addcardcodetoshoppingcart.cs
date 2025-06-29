using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace CleanArchitecture.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class addcardcodetoshoppingcart : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "72d29a1b-cdfa-487b-978b-ae5e289ffa35");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "f8fe3397-dcd4-4ab6-8dce-e3f461b1b42b");

            migrationBuilder.AddColumn<string>(
                name: "CartCode",
                table: "ShoppingCartItems",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "CartCode",
                table: "OrderItems",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "4c3e7b78-9e24-4a11-80b2-341676d7a804", "1", "Admin", "Admin" },
                    { "6cbf4104-cb70-4365-a18d-8d57379aba8a", "2", "User", "User" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "4c3e7b78-9e24-4a11-80b2-341676d7a804");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "6cbf4104-cb70-4365-a18d-8d57379aba8a");

            migrationBuilder.DropColumn(
                name: "CartCode",
                table: "ShoppingCartItems");

            migrationBuilder.DropColumn(
                name: "CartCode",
                table: "OrderItems");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "72d29a1b-cdfa-487b-978b-ae5e289ffa35", "1", "Admin", "Admin" },
                    { "f8fe3397-dcd4-4ab6-8dce-e3f461b1b42b", "2", "User", "User" }
                });
        }
    }
}
