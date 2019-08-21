using Microsoft.EntityFrameworkCore.Migrations;

namespace Login.Migrations
{
    public partial class currentmigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "Current_balance",
                table: "Users",
                nullable: false,
                defaultValue: 0.0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Current_balance",
                table: "Users");
        }
    }
}
