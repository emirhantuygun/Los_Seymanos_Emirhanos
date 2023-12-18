using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CafeApp.Migrations
{
    /// <inheritdoc />
    public partial class AddProductSizes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Price",
                table: "Products",
                newName: "PriceVenti");

            migrationBuilder.AddColumn<decimal>(
                name: "PriceDessert",
                table: "Products",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "PriceGrande",
                table: "Products",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "PriceTall",
                table: "Products",
                type: "TEXT",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PriceDessert",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "PriceGrande",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "PriceTall",
                table: "Products");

            migrationBuilder.RenameColumn(
                name: "PriceVenti",
                table: "Products",
                newName: "Price");
        }
    }
}
