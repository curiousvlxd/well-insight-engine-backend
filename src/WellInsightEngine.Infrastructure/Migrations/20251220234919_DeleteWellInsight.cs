using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WellInsightEngine.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class DeleteWellInsight : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "well_insight_actions");

            migrationBuilder.AddColumn<int>(
                name: "interval",
                table: "well_insights",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "interval",
                table: "well_insights");

            migrationBuilder.CreateTable(
                name: "well_insight_actions",
                columns: table => new
                {
                    insight_id = table.Column<Guid>(type: "uuid", nullable: false),
                    well_action_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_well_insight_actions", x => new { x.insight_id, x.well_action_id });
                    table.ForeignKey(
                        name: "fk_well_insight_actions_well_actions_well_action_id",
                        column: x => x.well_action_id,
                        principalTable: "well_actions",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_well_insight_actions_well_insights_insight_id",
                        column: x => x.insight_id,
                        principalTable: "well_insights",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_well_insight_actions_well_action_id",
                table: "well_insight_actions",
                column: "well_action_id");
        }
    }
}
