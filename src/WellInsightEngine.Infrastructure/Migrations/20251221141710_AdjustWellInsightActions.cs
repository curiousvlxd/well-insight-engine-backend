using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WellInsightEngine.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AdjustWellInsightActions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "insight_id1",
                table: "well_insight_actions",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "ix_well_insight_actions_insight_id1",
                table: "well_insight_actions",
                column: "insight_id1");

            migrationBuilder.AddForeignKey(
                name: "fk_well_insight_actions_well_insights_insight_id1",
                table: "well_insight_actions",
                column: "insight_id1",
                principalTable: "well_insights",
                principalColumn: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_well_insight_actions_well_insights_insight_id1",
                table: "well_insight_actions");

            migrationBuilder.DropIndex(
                name: "ix_well_insight_actions_insight_id1",
                table: "well_insight_actions");

            migrationBuilder.DropColumn(
                name: "insight_id1",
                table: "well_insight_actions");
        }
    }
}
