using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SharpyProxy.Database.Migrations
{
    /// <inheritdoc />
    public partial class UpdateTablesForManagedCertificates : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Domain",
                table: "Certificates",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "ExpirationDateUtc",
                table: "Certificates",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<Guid>(
                name: "LetsEncryptAccountId",
                table: "Certificates",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Type",
                table: "Certificates",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "LetsEncryptAccounts",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Email = table.Column<string>(type: "text", nullable: false),
                    Pem = table.Column<string>(type: "text", nullable: false),
                    CreatedDateUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedDateUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LetsEncryptAccounts", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Certificates_LetsEncryptAccountId",
                table: "Certificates",
                column: "LetsEncryptAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_LetsEncryptAccounts_Email",
                table: "LetsEncryptAccounts",
                column: "Email",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Certificates_LetsEncryptAccounts_LetsEncryptAccountId",
                table: "Certificates",
                column: "LetsEncryptAccountId",
                principalTable: "LetsEncryptAccounts",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Certificates_LetsEncryptAccounts_LetsEncryptAccountId",
                table: "Certificates");

            migrationBuilder.DropTable(
                name: "LetsEncryptAccounts");

            migrationBuilder.DropIndex(
                name: "IX_Certificates_LetsEncryptAccountId",
                table: "Certificates");

            migrationBuilder.DropColumn(
                name: "Domain",
                table: "Certificates");

            migrationBuilder.DropColumn(
                name: "ExpirationDateUtc",
                table: "Certificates");

            migrationBuilder.DropColumn(
                name: "LetsEncryptAccountId",
                table: "Certificates");

            migrationBuilder.DropColumn(
                name: "Type",
                table: "Certificates");
        }
    }
}
