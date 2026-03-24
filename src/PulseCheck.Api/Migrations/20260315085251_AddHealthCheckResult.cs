using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PulseCheck.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddHealthCheckResult : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "HealthCheckResults",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    MonitoredEndpointId = table.Column<int>(type: "INTEGER", nullable: false),
                    IsUp = table.Column<bool>(type: "INTEGER", nullable: false),
                    StatusCode = table.Column<int>(type: "INTEGER", nullable: false),
                    ResponseTimeMs = table.Column<int>(type: "INTEGER", nullable: false),
                    CheckedAt = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HealthCheckResults", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HealthCheckResults_MonitoredEndpoints_MonitoredEndpointId",
                        column: x => x.MonitoredEndpointId,
                        principalTable: "MonitoredEndpoints",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_HealthCheckResults_MonitoredEndpointId",
                table: "HealthCheckResults",
                column: "MonitoredEndpointId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "HealthCheckResults");
        }
    }
}
