using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace SmartWMS.Migrations
{
    /// <inheritdoc />
    public partial class merge_product_detail_to_product : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_products_product_details1",
                table: "products");

            migrationBuilder.DropTable(
                name: "product_details");

            migrationBuilder.DropIndex(
                name: "IX_products_product_details_product_detail_id",
                table: "products");

            migrationBuilder.RenameColumn(
                name: "product_details_product_detail_id",
                table: "products",
                newName: "quantity");

            migrationBuilder.AddColumn<string>(
                name: "barcode",
                table: "products",
                type: "character varying(8)",
                maxLength: 8,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "barcode",
                table: "products");

            migrationBuilder.RenameColumn(
                name: "quantity",
                table: "products",
                newName: "product_details_product_detail_id");

            migrationBuilder.CreateTable(
                name: "product_details",
                columns: table => new
                {
                    product_detail_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    barcode = table.Column<string>(type: "character varying(8)", maxLength: 8, nullable: false),
                    quantity = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("warehouse_state_id_unique", x => x.product_detail_id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_products_product_details_product_detail_id",
                table: "products",
                column: "product_details_product_detail_id",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "fk_products_product_details1",
                table: "products",
                column: "product_details_product_detail_id",
                principalTable: "product_details",
                principalColumn: "product_detail_id");
        }
    }
}
