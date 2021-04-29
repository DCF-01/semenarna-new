using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace semenarna_id2.Migrations
{
    public partial class changeSpec : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Fourth",
                table: "Specs");

            migrationBuilder.DropColumn(
                name: "Second",
                table: "Specs");

            migrationBuilder.RenameColumn(
                name: "Third",
                table: "Specs",
                newName: "Rest");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Rest",
                table: "Specs",
                newName: "Third");

            migrationBuilder.AddColumn<string[]>(
                name: "Fourth",
                table: "Specs",
                type: "text[]",
                nullable: true);

            migrationBuilder.AddColumn<string[]>(
                name: "Second",
                table: "Specs",
                type: "text[]",
                nullable: true);
        }
    }
}
