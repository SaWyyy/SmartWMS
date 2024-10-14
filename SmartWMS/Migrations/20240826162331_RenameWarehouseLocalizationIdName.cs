using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SmartWMS.Migrations
{
    /// <inheritdoc />
    public partial class RenameWarehouseLocalizationIdName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "warehouse_localization_id_unique",
                table: "shelf");

            migrationBuilder.RenameColumn(
                name: "warehouse_localization_id",
                table: "shelf",
                newName: "shelf_id");

            migrationBuilder.AddPrimaryKey(
                name: "shelf_id_unique",
                table: "shelf",
                column: "shelf_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "shelf_id_unique",
                table: "shelf");

            migrationBuilder.RenameColumn(
                name: "shelf_id",
                table: "shelf",
                newName: "warehouse_localization_id");

            migrationBuilder.AddPrimaryKey(
                name: "warehouse_localization_id_unique",
                table: "shelf",
                column: "warehouse_localization_id");
        }
    }
}
