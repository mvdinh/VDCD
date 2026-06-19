using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VDCD.DL.Migrations
{
    /// <inheritdoc />
    public partial class AddUnitTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "unit",
                columns: table => new
                {
                    unitid = table.Column<Guid>(type: "uuid", nullable: false),
                    unitname = table.Column<string>(type: "text", nullable: false),
                    description = table.Column<string>(type: "text", nullable: true),
                    createdby = table.Column<Guid>(type: "uuid", nullable: true),
                    createddate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    modifiedby = table.Column<Guid>(type: "uuid", nullable: true),
                    modifieddate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_unit", x => x.unitid);
                });

            migrationBuilder.DropColumn(
                name: "unit",
                table: "product");

            migrationBuilder.AddColumn<Guid>(
                name: "unitid",
                table: "product",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.Sql("INSERT INTO unit (unitid, unitname, createddate) VALUES ('00000000-0000-0000-0000-000000000000', 'Cái', NOW());");

            migrationBuilder.CreateIndex(
                name: "IX_product_unitid",
                table: "product",
                column: "unitid");

            migrationBuilder.AddForeignKey(
                name: "FK_product_unit_unitid",
                table: "product",
                column: "unitid",
                principalTable: "unit",
                principalColumn: "unitid",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_product_unit_unitid",
                table: "product");

            migrationBuilder.DropIndex(
                name: "IX_product_unitid",
                table: "product");

            migrationBuilder.DropColumn(
                name: "unitid",
                table: "product");

            migrationBuilder.AddColumn<string>(
                name: "unit",
                table: "product",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.DropTable(
                name: "unit");
        }
    }
}
