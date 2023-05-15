using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SharpyProxy.Database.Migrations
{
    /// <inheritdoc />
    public partial class SetClusterDestinationEntityTrackedAndNameIndexes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDateUtc",
                table: "ClusterDestinations",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedDateUtc",
                table: "ClusterDestinations",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.CreateIndex(
                name: "IX_Routes_Name",
                table: "Routes",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Clusters_Name",
                table: "Clusters",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ClusterDestinations_Name",
                table: "ClusterDestinations",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Certificates_Name",
                table: "Certificates",
                column: "Name",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Routes_Name",
                table: "Routes");

            migrationBuilder.DropIndex(
                name: "IX_Clusters_Name",
                table: "Clusters");

            migrationBuilder.DropIndex(
                name: "IX_ClusterDestinations_Name",
                table: "ClusterDestinations");

            migrationBuilder.DropIndex(
                name: "IX_Certificates_Name",
                table: "Certificates");

            migrationBuilder.DropColumn(
                name: "CreatedDateUtc",
                table: "ClusterDestinations");

            migrationBuilder.DropColumn(
                name: "UpdatedDateUtc",
                table: "ClusterDestinations");
        }
    }
}
