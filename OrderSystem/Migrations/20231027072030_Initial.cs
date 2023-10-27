using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace OrderSystem.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Carts",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Carts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Countries",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ConversionRate = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Countries", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DestinationCountry = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MailingCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TotalPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AvailableQuantity = table.Column<int>(type: "int", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "InCartProducts",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProductId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    CartId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InCartProducts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InCartProducts_Carts_CartId",
                        column: x => x.CartId,
                        principalTable: "Carts",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_InCartProducts_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "InOrderProducts",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProductId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    OrderId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InOrderProducts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InOrderProducts_Orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_InOrderProducts_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Countries",
                columns: new[] { "Id", "ConversionRate", "Name" },
                values: new object[,]
                {
                    { new Guid("40bd6510-aea1-4af0-8366-55a5939804dc"), 1.0m, "Canada" },
                    { new Guid("8d1d5eaf-3e84-4db6-9b75-b1657c64b79d"), 13.29m, "Mexico" },
                    { new Guid("d58ab7b8-53ff-446d-8fc1-9a809ec6ac7a"), 0.73m, "USA" }
                });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "AvailableQuantity", "Description", "Name", "Price" },
                values: new object[,]
                {
                    { new Guid("10d9b8b8-3bab-46ca-bf6a-7f5a54ab7ac1"), 5, "NuoMi's favorite", "Cat licking toy", 19.99m },
                    { new Guid("13208e45-595f-45c2-920b-ca10a95e7c8a"), 1, "XiaoGou's favorite", "Cat toy", 10.99m },
                    { new Guid("75c0a25e-29bd-4613-90e6-8a9fc9384538"), 0, "Best cat food", "Cat food", 4.99m },
                    { new Guid("94ccd15a-48ae-4de3-991c-6cad55a52caf"), 2, "ZhaZha's favorite", "Cat tree", 9.99m },
                    { new Guid("e4bd5a9e-eaf9-44ce-9067-283711181bae"), 3, "Best Cat scratch post", "Cat scratch post", 12.99m }
                });

            migrationBuilder.CreateIndex(
                name: "IX_InCartProducts_CartId",
                table: "InCartProducts",
                column: "CartId");

            migrationBuilder.CreateIndex(
                name: "IX_InCartProducts_ProductId",
                table: "InCartProducts",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_InOrderProducts_OrderId",
                table: "InOrderProducts",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_InOrderProducts_ProductId",
                table: "InOrderProducts",
                column: "ProductId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Countries");

            migrationBuilder.DropTable(
                name: "InCartProducts");

            migrationBuilder.DropTable(
                name: "InOrderProducts");

            migrationBuilder.DropTable(
                name: "Carts");

            migrationBuilder.DropTable(
                name: "Orders");

            migrationBuilder.DropTable(
                name: "Products");
        }
    }
}
