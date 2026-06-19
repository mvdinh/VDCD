using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VDCD.DL.Migrations
{
    /// <inheritdoc />
    public partial class RemoveProductSku : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "sku",
                table: "product");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "sku",
                table: "product",
                type: "text",
                nullable: true);
        }
    }
}
