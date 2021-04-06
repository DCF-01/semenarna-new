using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace semenarna_id2.Data.Migrations
{
    public partial class added_img_field : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte[]>(
                name: "Img",
                table: "TestProduct",
                type: "varbinary(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Img",
                table: "TestProduct");
        }
    }
}
