using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace semenarna_id2.Migrations
{
    public partial class cartChange : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Variations_CartProducts_CartProductId",
                table: "Variations");

            migrationBuilder.DropIndex(
                name: "IX_Variations_CartProductId",
                table: "Variations");

            migrationBuilder.DropColumn(
                name: "CartProductId",
                table: "Variations");

            migrationBuilder.AddColumn<string[]>(
                name: "Variations",
                table: "CartProducts",
                type: "text[]",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Variations",
                table: "CartProducts");

            migrationBuilder.AddColumn<int>(
                name: "CartProductId",
                table: "Variations",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Variations_CartProductId",
                table: "Variations",
                column: "CartProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_Variations_CartProducts_CartProductId",
                table: "Variations",
                column: "CartProductId",
                principalTable: "CartProducts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
