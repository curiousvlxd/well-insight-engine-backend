using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WellInsightEngine.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class WellInsightEngineMigrationv1Merged : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "assets",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    parent_id = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_assets", x => x.id);
                    table.ForeignKey(
                        name: "fk_assets_assets_parent_id",
                        column: x => x.parent_id,
                        principalTable: "assets",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "parameters",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    code = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    data_type = table.Column<short>(type: "smallint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_parameters", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "wells",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    external_id = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    asset_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_wells", x => x.id);
                    table.ForeignKey(
                        name: "fk_wells_assets_asset_id",
                        column: x => x.asset_id,
                        principalTable: "assets",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "well_actions",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    well_id = table.Column<Guid>(type: "uuid", nullable: false),
                    timestamp = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    title = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: false),
                    details = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: false),
                    source = table.Column<short>(type: "smallint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_well_actions", x => x.id);
                    table.ForeignKey(
                        name: "fk_well_actions_wells_well_id",
                        column: x => x.well_id,
                        principalTable: "wells",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "well_insights",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    well_id = table.Column<Guid>(type: "uuid", nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    from = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    to = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    interval = table.Column<int>(type: "integer", nullable: false),
                    title = table.Column<string>(type: "text", nullable: false),
                    summary = table.Column<string>(type: "text", nullable: false),
                    payload = table.Column<string>(type: "jsonb", nullable: false),
                    slug = table.Column<string>(type: "text", nullable: false),
                    highlights = table.Column<string>(type: "jsonb", nullable: false),
                    suspicions = table.Column<string>(type: "jsonb", nullable: false),
                    recommended_actions = table.Column<string>(type: "jsonb", nullable: false)
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
                name: "well_parameters",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    well_id = table.Column<Guid>(type: "uuid", nullable: false),
                    parameter_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_well_parameters", x => x.id);
                    table.ForeignKey(
                        name: "fk_well_parameters_parameters_parameter_id",
                        column: x => x.parameter_id,
                        principalTable: "parameters",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_well_parameters_wells_well_id",
                        column: x => x.well_id,
                        principalTable: "wells",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_assets_parent_id_name",
                table: "assets",
                columns: new[] { "parent_id", "name" });

            migrationBuilder.CreateIndex(
                name: "ix_parameters_code",
                table: "parameters",
                column: "code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_well_actions_well_id_timestamp",
                table: "well_actions",
                columns: new[] { "well_id", "timestamp" });

            migrationBuilder.CreateIndex(
                name: "ix_well_insights_well_id_created_at",
                table: "well_insights",
                columns: new[] { "well_id", "created_at" });

            migrationBuilder.CreateIndex(
                name: "ix_well_insights_well_id_from_to",
                table: "well_insights",
                columns: new[] { "well_id", "from", "to" });

            migrationBuilder.CreateIndex(
                name: "ix_well_parameters_parameter_id",
                table: "well_parameters",
                column: "parameter_id");

            migrationBuilder.CreateIndex(
                name: "ix_well_parameters_well_id_parameter_id",
                table: "well_parameters",
                columns: new[] { "well_id", "parameter_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_wells_asset_id",
                table: "wells",
                column: "asset_id");

            migrationBuilder.CreateIndex(
                name: "ix_wells_external_id",
                table: "wells",
                column: "external_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "well_actions");

            migrationBuilder.DropTable(
                name: "well_insights");

            migrationBuilder.DropTable(
                name: "well_parameters");

            migrationBuilder.DropTable(
                name: "parameters");

            migrationBuilder.DropTable(
                name: "wells");

            migrationBuilder.DropTable(
                name: "assets");
        }
    }
}
