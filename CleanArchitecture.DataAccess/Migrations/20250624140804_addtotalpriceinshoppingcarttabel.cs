using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace CleanArchitecture.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class addtotalpriceinshoppingcarttabel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "31d95809-12c9-4d13-8115-aab165e2d9cb");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "9c6ea579-e142-4f82-8555-b4dcd3abfeb3");

            migrationBuilder.AddColumn<string>(
                name: "ProductName",
                table: "ShoppingCartItems",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<decimal>(
                name: "TotalPrice",
                table: "ShoppingCartItems",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "416c2bda-3d0b-4175-9c50-d33ec0e9fc38", "1", "Admin", "Admin" },
                    { "50d9c4f4-ed2a-441e-9c0a-776ee41f0928", "2", "User", "User" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "416c2bda-3d0b-4175-9c50-d33ec0e9fc38");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "50d9c4f4-ed2a-441e-9c0a-776ee41f0928");

            migrationBuilder.DropColumn(
                name: "ProductName",
                table: "ShoppingCartItems");

            migrationBuilder.DropColumn(
                name: "TotalPrice",
                table: "ShoppingCartItems");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "31d95809-12c9-4d13-8115-aab165e2d9cb", "2", "User", "User" },
                    { "9c6ea579-e142-4f82-8555-b4dcd3abfeb3", "1", "Admin", "Admin" }
                });
        }
    }
}
