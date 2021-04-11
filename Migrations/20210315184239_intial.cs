using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HUSTLEPLUS.BUYER.ORDER.MICROSERVICE.Migrations
{
    public partial class intial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Order",
                columns: table => new
                {
                    Order_ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Buyer_ID = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Order_DateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Order_Cost = table.Column<float>(type: "real", nullable: false),
                    Order_CustomersName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Order_ShippingAddress = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Order_State = table.Column<int>(type: "int", nullable: false),
                    Order_Quantity = table.Column<int>(type: "int", nullable: false),
                    Order_Status = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Order", x => x.Order_ID);
                });

            migrationBuilder.CreateTable(
                name: "OrdredProducts",
                columns: table => new
                {
                    Order_ID = table.Column<int>(type: "int", nullable: false),
                    Order_SellerID = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Order_TotalCost = table.Column<float>(type: "real", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrdredProducts", x => x.Order_ID);
                    table.ForeignKey(
                        name: "FK_OrdredProducts_Order_Order_ID",
                        column: x => x.Order_ID,
                        principalTable: "Order",
                        principalColumn: "Order_ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OrderedProduct",
                columns: table => new
                {
                    Order_ProductID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Order_ProductCost = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Order_ProductQuantity = table.Column<int>(type: "int", nullable: false),
                    Order_ID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderedProduct", x => x.Order_ProductID);
                    table.ForeignKey(
                        name: "FK_OrderedProduct_OrdredProducts_Order_ID",
                        column: x => x.Order_ID,
                        principalTable: "OrdredProducts",
                        principalColumn: "Order_ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_OrderedProduct_Order_ID",
                table: "OrderedProduct",
                column: "Order_ID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OrderedProduct");

            migrationBuilder.DropTable(
                name: "OrdredProducts");

            migrationBuilder.DropTable(
                name: "Order");
        }
    }
}
