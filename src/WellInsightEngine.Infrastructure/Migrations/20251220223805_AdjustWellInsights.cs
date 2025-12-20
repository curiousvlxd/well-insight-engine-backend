using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WellInsightEngine.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AdjustWellInsights : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "suspicions",
                table: "well_insights",
                type: "jsonb",
                nullable: false,
                oldClrType: typeof(string[]),
                oldType: "text[]");

            migrationBuilder.AlterColumn<string>(
                name: "recommended_actions",
                table: "well_insights",
                type: "jsonb",
                nullable: false,
                oldClrType: typeof(string[]),
                oldType: "text[]");

            migrationBuilder.AlterColumn<string>(
                name: "highlights",
                table: "well_insights",
                type: "jsonb",
                nullable: false,
                oldClrType: typeof(string[]),
                oldType: "text[]");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string[]>(
                name: "suspicions",
                table: "well_insights",
                type: "text[]",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "jsonb");

            migrationBuilder.AlterColumn<string[]>(
                name: "recommended_actions",
                table: "well_insights",
                type: "text[]",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "jsonb");

            migrationBuilder.AlterColumn<string[]>(
                name: "highlights",
                table: "well_insights",
                type: "text[]",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "jsonb");
        }
    }
}
