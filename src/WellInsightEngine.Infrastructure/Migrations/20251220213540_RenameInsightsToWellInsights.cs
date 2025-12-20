using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WellInsightEngine.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RenameInsightsToWellInsights : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "insight_actions");

            migrationBuilder.DropTable(
                name: "insights");

            migrationBuilder.CreateTable(
                name: "well_insights",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    well_id = table.Column<Guid>(type: "uuid", nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    from = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    to = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    title = table.Column<string>(type: "text", nullable: false),
                    summary = table.Column<string>(type: "text", nullable: false),
                    payload = table.Column<string>(type: "jsonb", nullable: false),
                    highlights = table.Column<string[]>(type: "text[]", nullable: false),
                    suspicions = table.Column<string[]>(type: "text[]", nullable: false),
                    recommended_actions = table.Column<string[]>(type: "text[]", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_well_insights", x => x.id);
                    table.ForeignKey(
                        name: "fk_well_insights_wells_well_id",
                        column: x => x.well_id,
                        principalTable: "wells",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

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

            migrationBuilder.CreateIndex(
                name: "ix_well_insights_well_id_created_at",
                table: "well_insights",
                columns: new[] { "well_id", "created_at" });

            migrationBuilder.CreateIndex(
                name: "ix_well_insights_well_id_from_to",
                table: "well_insights",
                columns: new[] { "well_id", "from", "to" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "well_insight_actions");

            migrationBuilder.DropTable(
                name: "well_insights");

            migrationBuilder.CreateTable(
                name: "insights",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    from = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    payload = table.Column<string>(type: "jsonb", nullable: false),
                    summary = table.Column<string>(type: "text", nullable: false),
                    title = table.Column<string>(type: "text", nullable: false),
                    to = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    well_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_insights", x => x.id);
                    table.ForeignKey(
                        name: "fk_insights_wells_well_id",
                        column: x => x.well_id,
                        principalTable: "wells",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "insight_actions",
                columns: table => new
                {
                    insight_id = table.Column<Guid>(type: "uuid", nullable: false),
                    well_action_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_insight_actions", x => new { x.insight_id, x.well_action_id });
                    table.ForeignKey(
                        name: "fk_insight_actions_insights_insight_id",
                        column: x => x.insight_id,
                        principalTable: "insights",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_insight_actions_well_actions_well_action_id",
                        column: x => x.well_action_id,
                        principalTable: "well_actions",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_insight_actions_well_action_id",
                table: "insight_actions",
                column: "well_action_id");

            migrationBuilder.CreateIndex(
                name: "ix_insights_well_id_created_at",
                table: "insights",
                columns: new[] { "well_id", "created_at" });

            migrationBuilder.CreateIndex(
                name: "ix_insights_well_id_from_to",
                table: "insights",
                columns: new[] { "well_id", "from", "to" });
        }
    }
}
