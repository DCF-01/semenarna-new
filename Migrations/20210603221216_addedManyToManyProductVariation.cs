using Microsoft.EntityFrameworkCore.Migrations;

namespace semenarna_id2.Migrations
{
    public partial class addedManyToManyProductVariation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Variations_Products_ProductId",
                table: "Variations");

            migrationBuilder.DropIndex(
                name: "IX_Variations_ProductId",
                table: "Variations");

            migrationBuilder.DropColumn(
                name: "ProductId",
                table: "Variations");

            migrationBuilder.CreateTable(
                name: "ProductVariation",
                columns: table => new
                {
                    ProductsProductId = table.Column<int>(type: "integer", nullable: false),
                    VariationsVariationId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductVariation", x => new { x.ProductsProductId, x.VariationsVariationId });
                    table.ForeignKey(
                        name: "FK_ProductVariation_Products_ProductsProductId",
                        column: x => x.ProductsProductId,
                        principalTable: "Products",
                        principalColumn: "ProductId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProductVariation_Variations_VariationsVariationId",
                        column: x => x.VariationsVariationId,
                        principalTable: "Variations",
                        principalColumn: "VariationId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProductVariation_VariationsVariationId",
                table: "ProductVariation",
                column: "VariationsVariationId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProductVariation");

            migrationBuilder.AddColumn<int>(
                name: "ProductId",
                table: "Variations",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Variations_ProductId",
                table: "Variations",
                column: "ProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_Variations_Products_ProductId",
                table: "Variations",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "ProductId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
