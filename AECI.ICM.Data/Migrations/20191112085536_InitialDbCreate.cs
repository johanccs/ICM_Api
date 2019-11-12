using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace AECI.ICM.Data.Migrations
{
    public partial class InitialDbCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SectionDetail",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Section = table.Column<string>(nullable: true),
                    SectionName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SectionDetail", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ICM",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ControlStatement = table.Column<string>(nullable: true),
                    BranchManager = table.Column<bool>(nullable: false),
                    RegionalAccountant = table.Column<bool>(nullable: false),
                    FinanceFunctionCheck = table.Column<bool>(nullable: false),
                    SectionDetailId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ICM", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ICM_SectionDetail_SectionDetailId",
                        column: x => x.SectionDetailId,
                        principalTable: "SectionDetail",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ICM_SectionDetailId",
                table: "ICM",
                column: "SectionDetailId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ICM");

            migrationBuilder.DropTable(
                name: "SectionDetail");
        }
    }
}
