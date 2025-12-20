using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WellInsightEngine.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AdjustParameterEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_well_parameters_parameters_parameter_id",
                table: "well_parameters");

            migrationBuilder.DropIndex(
                name: "ix_well_parameters_well_id_name",
                table: "well_parameters");

            migrationBuilder.DropColumn(
                name: "data_type",
                table: "well_parameters");

            migrationBuilder.DropColumn(
                name: "name",
                table: "well_parameters");

            migrationBuilder.AlterColumn<Guid>(
                name: "parameter_id",
                table: "well_parameters",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "ix_well_parameters_well_id_parameter_id",
                table: "well_parameters",
                columns: new[] { "well_id", "parameter_id" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "fk_well_parameters_parameters_parameter_id",
                table: "well_parameters",
                column: "parameter_id",
                principalTable: "parameters",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_well_parameters_parameters_parameter_id",
                table: "well_parameters");

            migrationBuilder.DropIndex(
                name: "ix_well_parameters_well_id_parameter_id",
                table: "well_parameters");

            migrationBuilder.AlterColumn<Guid>(
                name: "parameter_id",
                table: "well_parameters",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AddColumn<short>(
                name: "data_type",
                table: "well_parameters",
                type: "smallint",
                nullable: false,
                defaultValue: (short)0);

            migrationBuilder.AddColumn<string>(
                name: "name",
                table: "well_parameters",
                type: "character varying(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "ix_well_parameters_well_id_name",
                table: "well_parameters",
                columns: new[] { "well_id", "name" });

            migrationBuilder.AddForeignKey(
                name: "fk_well_parameters_parameters_parameter_id",
                table: "well_parameters",
                column: "parameter_id",
                principalTable: "parameters",
                principalColumn: "id");
        }
    }
}
