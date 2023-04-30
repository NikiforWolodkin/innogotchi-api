using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace innogotchi_api.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Email = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Email);
                });

            migrationBuilder.CreateTable(
                name: "Farms",
                columns: table => new
                {
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    UserEmail = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Farms", x => x.Name);
                    table.ForeignKey(
                        name: "FK_Farms_Users_UserEmail",
                        column: x => x.UserEmail,
                        principalTable: "Users",
                        principalColumn: "Email");
                });

            migrationBuilder.CreateTable(
                name: "Collaborations",
                columns: table => new
                {
                    FarmName = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    UserEmail = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Collaborations", x => new { x.FarmName, x.UserEmail });
                    table.ForeignKey(
                        name: "FK_Collaborations_Farms_FarmName",
                        column: x => x.FarmName,
                        principalTable: "Farms",
                        principalColumn: "Name",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Collaborations_Users_UserEmail",
                        column: x => x.UserEmail,
                        principalTable: "Users",
                        principalColumn: "Email",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Innogotchis",
                columns: table => new
                {
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeathDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    FarmName = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Innogotchis", x => x.Name);
                    table.ForeignKey(
                        name: "FK_Innogotchis_Farms_FarmName",
                        column: x => x.FarmName,
                        principalTable: "Farms",
                        principalColumn: "Name");
                });

            migrationBuilder.CreateTable(
                name: "FeedingAndQuenchings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FeedingTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    QuenchingTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UnhappyDays = table.Column<int>(type: "int", nullable: true),
                    FeedingPeriod = table.Column<int>(type: "int", nullable: true),
                    QuenchingPeriod = table.Column<int>(type: "int", nullable: true),
                    InnogotchiName = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FeedingAndQuenchings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FeedingAndQuenchings_Innogotchis_InnogotchiName",
                        column: x => x.InnogotchiName,
                        principalTable: "Innogotchis",
                        principalColumn: "Name");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Collaborations_UserEmail",
                table: "Collaborations",
                column: "UserEmail");

            migrationBuilder.CreateIndex(
                name: "IX_Farms_UserEmail",
                table: "Farms",
                column: "UserEmail");

            migrationBuilder.CreateIndex(
                name: "IX_FeedingAndQuenchings_InnogotchiName",
                table: "FeedingAndQuenchings",
                column: "InnogotchiName");

            migrationBuilder.CreateIndex(
                name: "IX_Innogotchis_FarmName",
                table: "Innogotchis",
                column: "FarmName");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Collaborations");

            migrationBuilder.DropTable(
                name: "FeedingAndQuenchings");

            migrationBuilder.DropTable(
                name: "Innogotchis");

            migrationBuilder.DropTable(
                name: "Farms");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
