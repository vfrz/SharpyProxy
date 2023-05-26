using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SharpyProxy.Database.Migrations
{
    /// <inheritdoc />
    public partial class AddLetsEncryptChallenges : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Pem",
                table: "LetsEncryptAccounts");

            migrationBuilder.AddColumn<byte[]>(
                name: "RSABytes",
                table: "LetsEncryptAccounts",
                type: "bytea",
                nullable: false,
                defaultValue: new byte[0]);

            migrationBuilder.CreateTable(
                name: "LetsEncryptChallenges",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Domain = table.Column<string>(type: "text", nullable: false),
                    Token = table.Column<string>(type: "text", nullable: false),
                    LetsEncryptAccountId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedDateUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedDateUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LetsEncryptChallenges", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LetsEncryptChallenges_LetsEncryptAccounts_LetsEncryptAccoun~",
                        column: x => x.LetsEncryptAccountId,
                        principalTable: "LetsEncryptAccounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_LetsEncryptChallenges_LetsEncryptAccountId",
                table: "LetsEncryptChallenges",
                column: "LetsEncryptAccountId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LetsEncryptChallenges");

            migrationBuilder.DropColumn(
                name: "RSABytes",
                table: "LetsEncryptAccounts");

            migrationBuilder.AddColumn<string>(
                name: "Pem",
                table: "LetsEncryptAccounts",
                type: "text",
                nullable: false,
                defaultValue: "");
        }
    }
}
