using System;
using Microsoft.EntityFrameworkCore.Migrations;
using SharpyProxy.Database.Entities;

#nullable disable

namespace SharpyProxy.Database.Migrations
{
    /// <inheritdoc />
    public partial class RemoveClusterDestinationTableReplacedByArrayTypeOnClusterTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ClusterDestinations");

            migrationBuilder.AddColumn<ClusterDestination[]>(
                name: "Destinations",
                table: "Clusters",
                type: "jsonb",
                nullable: false,
                defaultValue: new ClusterDestination[0]);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Destinations",
                table: "Clusters");

            migrationBuilder.CreateTable(
                name: "ClusterDestinations",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ClusterId = table.Column<Guid>(type: "uuid", nullable: false),
                    Address = table.Column<string>(type: "text", nullable: false),
                    CreatedDateUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    UpdatedDateUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClusterDestinations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ClusterDestinations_Clusters_ClusterId",
                        column: x => x.ClusterId,
                        principalTable: "Clusters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ClusterDestinations_ClusterId",
                table: "ClusterDestinations",
                column: "ClusterId");

            migrationBuilder.CreateIndex(
                name: "IX_ClusterDestinations_Name",
                table: "ClusterDestinations",
                column: "Name",
                unique: true);
        }
    }
}
