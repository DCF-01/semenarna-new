using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace semenarna_id2.Migrations
{
    public partial class addedskushortdescription : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SKUS",
                table: "Products");

            migrationBuilder.AddColumn<string>(
                name: "SKU",
                table: "Products",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ShortDescription",
                table: "Products",
                type: "text",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SKU",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "ShortDescription",
                table: "Products");

            migrationBuilder.AddColumn<string[]>(
                name: "SKUS",
                table: "Products",
                type: "text[]",
                nullable: true);
        }
    }
}
