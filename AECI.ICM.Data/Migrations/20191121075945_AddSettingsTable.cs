using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace AECI.ICM.Data.Migrations
{
    public partial class AddSettingsTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Settings",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    SignatureLocation = table.Column<string>(nullable: true),
                    EnableWarning = table.Column<bool>(nullable: false),
                    WarningCuttOffDate = table.Column<DateTime>(nullable: false),
                    WarningEmail = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Settings", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SettingEmails",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    BranchManagerName = table.Column<string>(nullable: true),
                    BranchManagerEmail = table.Column<string>(nullable: true),
                    RegionalAccountantName = table.Column<string>(nullable: true),
                    RegionalAccountantEmail = table.Column<string>(nullable: true),
                    SettingId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SettingEmails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SettingEmails_Settings_SettingId",
                        column: x => x.SettingId,
                        principalTable: "Settings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SettingEmails_SettingId",
                table: "SettingEmails",
                column: "SettingId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SettingEmails");

            migrationBuilder.DropTable(
                name: "Settings");
        }
    }
}
