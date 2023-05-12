using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ClientLayer.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Avatars",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Image = table.Column<byte[]>(type: "varbinary(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Avatars", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Farms",
                columns: table => new
                {
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Farms", x => x.Name);
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
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AvatarId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    FarmName = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Users_Avatars_AvatarId",
                        column: x => x.AvatarId,
                        principalTable: "Avatars",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Users_Farms_FarmName",
                        column: x => x.FarmName,
                        principalTable: "Farms",
                        principalColumn: "Name");
                });

            migrationBuilder.CreateTable(
                name: "FeedingsAndQuenchings",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FeedingTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    QuenchingTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UnhappyDays = table.Column<int>(type: "int", nullable: true),
                    FeedingPeriod = table.Column<int>(type: "int", nullable: true),
                    QuenchingPeriod = table.Column<int>(type: "int", nullable: true),
                    InnogotchiName = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FeedingsAndQuenchings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FeedingsAndQuenchings_Innogotchis_InnogotchiName",
                        column: x => x.InnogotchiName,
                        principalTable: "Innogotchis",
                        principalColumn: "Name");
                });

            migrationBuilder.CreateTable(
                name: "Collaborations",
                columns: table => new
                {
                    FarmName = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Collaborations", x => new { x.FarmName, x.UserId });
                    table.ForeignKey(
                        name: "FK_Collaborations_Farms_FarmName",
                        column: x => x.FarmName,
                        principalTable: "Farms",
                        principalColumn: "Name",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Collaborations_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Collaborations_UserId",
                table: "Collaborations",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_FeedingsAndQuenchings_InnogotchiName",
                table: "FeedingsAndQuenchings",
                column: "InnogotchiName");

            migrationBuilder.CreateIndex(
                name: "IX_Innogotchis_FarmName",
                table: "Innogotchis",
                column: "FarmName");

            migrationBuilder.CreateIndex(
                name: "IX_Users_AvatarId",
                table: "Users",
                column: "AvatarId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Email",
                table: "Users",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_FarmName",
                table: "Users",
                column: "FarmName");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Collaborations");

            migrationBuilder.DropTable(
                name: "FeedingsAndQuenchings");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Innogotchis");

            migrationBuilder.DropTable(
                name: "Avatars");

            migrationBuilder.DropTable(
                name: "Farms");
        }
    }
}
