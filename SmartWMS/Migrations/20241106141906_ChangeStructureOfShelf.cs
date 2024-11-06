using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace SmartWMS.Migrations
{
    /// <inheritdoc />
    public partial class ChangeStructureOfShelf : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "lane",
                table: "shelf");

            migrationBuilder.RenameColumn(
                name: "rack",
                table: "shelf",
                newName: "RacksRackId");

            migrationBuilder.CreateTable(
                name: "lane",
                columns: table => new
                {
                    lane_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    lane_number = table.Column<int>(type: "integer", maxLength: 3, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("lane_id_unique", x => x.lane_id);
                });

            migrationBuilder.CreateTable(
                name: "rack",
                columns: table => new
                {
                    rack_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    rack_number = table.Column<int>(type: "integer", nullable: false),
                    lanes_lane_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("rack_id_unique", x => x.rack_id);
                    table.ForeignKey(
                        name: "fk_rack_lanes1",
                        column: x => x.lanes_lane_id,
                        principalTable: "lane",
                        principalColumn: "lane_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_shelf_RacksRackId",
                table: "shelf",
                column: "RacksRackId");

            migrationBuilder.CreateIndex(
                name: "IX_lane_lane_number",
                table: "lane",
                column: "lane_number",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_rack_lanes_lane_id",
                table: "rack",
                column: "lanes_lane_id");

            migrationBuilder.AddForeignKey(
                name: "fk_shelf_racks1",
                table: "shelf",
                column: "RacksRackId",
                principalTable: "rack",
                principalColumn: "rack_id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_shelf_racks1",
                table: "shelf");

            migrationBuilder.DropTable(
                name: "rack");

            migrationBuilder.DropTable(
                name: "lane");

            migrationBuilder.DropIndex(
                name: "IX_shelf_RacksRackId",
                table: "shelf");

            migrationBuilder.RenameColumn(
                name: "RacksRackId",
                table: "shelf",
                newName: "rack");

            migrationBuilder.AddColumn<string>(
                name: "lane",
                table: "shelf",
                type: "character varying(3)",
                maxLength: 3,
                nullable: false,
                defaultValue: "");
        }
    }
}
