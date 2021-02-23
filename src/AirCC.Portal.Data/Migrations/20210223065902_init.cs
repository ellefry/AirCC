using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace AirCC.Portal.EntityFramework.Migrations
{
    public partial class init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Applications",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    CreationTime = table.Column<DateTime>(nullable: true),
                    Creator = table.Column<string>(nullable: true),
                    ModificationTime = table.Column<DateTime>(nullable: true),
                    Modifier = table.Column<string>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    ClientSecret = table.Column<string>(nullable: true),
                    Active = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Applications", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ApplicationConfigurations",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    CreationTime = table.Column<DateTime>(nullable: true),
                    Creator = table.Column<string>(nullable: true),
                    ModificationTime = table.Column<DateTime>(nullable: true),
                    Modifier = table.Column<string>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    CfgKey = table.Column<string>(nullable: true),
                    CfgValue = table.Column<string>(nullable: true),
                    Status = table.Column<int>(nullable: false),
                    ApplicationId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplicationConfigurations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ApplicationConfigurations_Applications_ApplicationId",
                        column: x => x.ApplicationId,
                        principalTable: "Applications",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ApplicationConfigurationHistories",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    CreationTime = table.Column<DateTime>(nullable: true),
                    Creator = table.Column<string>(nullable: true),
                    ModificationTime = table.Column<DateTime>(nullable: true),
                    Modifier = table.Column<string>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    CfgKey = table.Column<string>(nullable: true),
                    CfgValue = table.Column<string>(nullable: true),
                    ApplicationConfigurationId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplicationConfigurationHistories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ApplicationConfigurationHistories_ApplicationConfigurations_ApplicationConfigurationId",
                        column: x => x.ApplicationConfigurationId,
                        principalTable: "ApplicationConfigurations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationConfigurationHistories_ApplicationConfigurationId",
                table: "ApplicationConfigurationHistories",
                column: "ApplicationConfigurationId");

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationConfigurations_ApplicationId",
                table: "ApplicationConfigurations",
                column: "ApplicationId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ApplicationConfigurationHistories");

            migrationBuilder.DropTable(
                name: "ApplicationConfigurations");

            migrationBuilder.DropTable(
                name: "Applications");
        }
    }
}
