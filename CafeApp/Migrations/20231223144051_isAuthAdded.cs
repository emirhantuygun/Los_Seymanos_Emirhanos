using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CafeApp.Migrations
{
    /// <inheritdoc />
    public partial class isAuthAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsAuthenticated",
                table: "Baristas",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsAuthenticated",
                table: "Baristas");
        }
    }
}
