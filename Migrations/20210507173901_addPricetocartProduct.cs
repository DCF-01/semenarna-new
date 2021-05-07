using Microsoft.EntityFrameworkCore.Migrations;

namespace semenarna_id2.Migrations
{
    public partial class addPricetocartProduct : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Price",
                table: "CartProducts",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Price",
                table: "CartProducts");
        }
    }
}
