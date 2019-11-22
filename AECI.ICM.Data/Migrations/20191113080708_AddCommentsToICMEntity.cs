using Microsoft.EntityFrameworkCore.Migrations;

namespace AECI.ICM.Data.Migrations
{
    public partial class AddCommentsToICMEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Comments",
                table: "ICM",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Comments",
                table: "ICM");
        }
    }
}
