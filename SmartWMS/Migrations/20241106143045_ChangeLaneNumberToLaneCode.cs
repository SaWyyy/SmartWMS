using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SmartWMS.Migrations
{
    /// <inheritdoc />
    public partial class ChangeLaneNumberToLaneCode : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_lane_lane_number",
                table: "lane");

            migrationBuilder.DropColumn(
                name: "lane_number",
                table: "lane");

            migrationBuilder.AddColumn<string>(
                name: "lane_code",
                table: "lane",
                type: "character varying(3)",
                maxLength: 3,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_lane_lane_code",
                table: "lane",
                column: "lane_code",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_lane_lane_code",
                table: "lane");

            migrationBuilder.DropColumn(
                name: "lane_code",
                table: "lane");

            migrationBuilder.AddColumn<int>(
                name: "lane_number",
                table: "lane",
                type: "integer",
                maxLength: 3,
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_lane_lane_number",
                table: "lane",
                column: "lane_number",
                unique: true);
        }
    }
}
