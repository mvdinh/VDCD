using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VDCD.DL.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "category",
                columns: table => new
                {
                    categoryid = table.Column<Guid>(type: "uuid", nullable: false),
                    categoryname = table.Column<string>(type: "text", nullable: false),
                    description = table.Column<string>(type: "text", nullable: true),
                    createdby = table.Column<Guid>(type: "uuid", nullable: true),
                    createddate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    modifiedby = table.Column<Guid>(type: "uuid", nullable: true),
                    modifieddate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_category", x => x.categoryid);
                });

            migrationBuilder.CreateTable(
                name: "customer",
                columns: table => new
                {
                    customerid = table.Column<Guid>(type: "uuid", nullable: false),
                    customername = table.Column<string>(type: "text", nullable: false),
                    phonenumber = table.Column<string>(type: "text", nullable: true),
                    address = table.Column<string>(type: "text", nullable: true),
                    note = table.Column<string>(type: "text", nullable: true),
                    createdby = table.Column<Guid>(type: "uuid", nullable: true),
                    createddate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    modifiedby = table.Column<Guid>(type: "uuid", nullable: true),
                    modifieddate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_customer", x => x.customerid);
                });

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

            migrationBuilder.CreateTable(
                name: "users",
                columns: table => new
                {
                    userid = table.Column<Guid>(type: "uuid", nullable: false),
                    username = table.Column<string>(type: "text", nullable: false),
                    email = table.Column<string>(type: "text", nullable: false),
                    passwordhash = table.Column<string>(type: "text", nullable: false),
                    role = table.Column<string>(type: "text", nullable: false),
                    createdby = table.Column<Guid>(type: "uuid", nullable: true),
                    createddate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    modifiedby = table.Column<Guid>(type: "uuid", nullable: true),
                    modifieddate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_users", x => x.userid);
                });

            migrationBuilder.CreateTable(
                name: "orders",
                columns: table => new
                {
                    orderid = table.Column<Guid>(type: "uuid", nullable: false),
                    customerid = table.Column<Guid>(type: "uuid", nullable: true),
                    orderdate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    totalamount = table.Column<decimal>(type: "numeric", nullable: false),
                    discount = table.Column<decimal>(type: "numeric", nullable: false),
                    finalamount = table.Column<decimal>(type: "numeric", nullable: false),
                    paymentmethod = table.Column<string>(type: "text", nullable: false),
                    note = table.Column<string>(type: "text", nullable: true),
                    createdby = table.Column<Guid>(type: "uuid", nullable: true),
                    createddate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    modifiedby = table.Column<Guid>(type: "uuid", nullable: true),
                    modifieddate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_orders", x => x.orderid);
                    table.ForeignKey(
                        name: "FK_orders_customer_customerid",
                        column: x => x.customerid,
                        principalTable: "customer",
                        principalColumn: "customerid");
                });

            migrationBuilder.CreateTable(
                name: "product",
                columns: table => new
                {
                    productid = table.Column<Guid>(type: "uuid", nullable: false),
                    categoryid = table.Column<Guid>(type: "uuid", nullable: false),
                    productname = table.Column<string>(type: "text", nullable: false),
                    price = table.Column<decimal>(type: "numeric", nullable: false),
                    quantity = table.Column<int>(type: "integer", nullable: false),
                    unitid = table.Column<Guid>(type: "uuid", nullable: false),
                    description = table.Column<string>(type: "text", nullable: true),
                    createdby = table.Column<Guid>(type: "uuid", nullable: true),
                    createddate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    modifiedby = table.Column<Guid>(type: "uuid", nullable: true),
                    modifieddate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_product", x => x.productid);
                    table.ForeignKey(
                        name: "FK_product_category_categoryid",
                        column: x => x.categoryid,
                        principalTable: "category",
                        principalColumn: "categoryid",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_product_unit_unitid",
                        column: x => x.unitid,
                        principalTable: "unit",
                        principalColumn: "unitid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "order_detail",
                columns: table => new
                {
                    orderdetailid = table.Column<Guid>(type: "uuid", nullable: false),
                    orderid = table.Column<Guid>(type: "uuid", nullable: false),
                    productid = table.Column<Guid>(type: "uuid", nullable: false),
                    quantity = table.Column<int>(type: "integer", nullable: false),
                    unitprice = table.Column<decimal>(type: "numeric", nullable: false),
                    subtotal = table.Column<decimal>(type: "numeric", nullable: false),
                    createdby = table.Column<Guid>(type: "uuid", nullable: true),
                    createddate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    modifiedby = table.Column<Guid>(type: "uuid", nullable: true),
                    modifieddate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_order_detail", x => x.orderdetailid);
                    table.ForeignKey(
                        name: "FK_order_detail_orders_orderid",
                        column: x => x.orderid,
                        principalTable: "orders",
                        principalColumn: "orderid",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_order_detail_product_productid",
                        column: x => x.productid,
                        principalTable: "product",
                        principalColumn: "productid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_order_detail_orderid",
                table: "order_detail",
                column: "orderid");

            migrationBuilder.CreateIndex(
                name: "IX_order_detail_productid",
                table: "order_detail",
                column: "productid");

            migrationBuilder.CreateIndex(
                name: "IX_orders_customerid",
                table: "orders",
                column: "customerid");

            migrationBuilder.CreateIndex(
                name: "IX_product_categoryid",
                table: "product",
                column: "categoryid");

            migrationBuilder.CreateIndex(
                name: "IX_product_unitid",
                table: "product",
                column: "unitid");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "order_detail");

            migrationBuilder.DropTable(
                name: "users");

            migrationBuilder.DropTable(
                name: "orders");

            migrationBuilder.DropTable(
                name: "product");

            migrationBuilder.DropTable(
                name: "customer");

            migrationBuilder.DropTable(
                name: "category");

            migrationBuilder.DropTable(
                name: "unit");
        }
    }
}
