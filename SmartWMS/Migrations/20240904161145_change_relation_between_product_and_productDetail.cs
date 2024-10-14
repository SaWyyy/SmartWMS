using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SmartWMS.Migrations
{
    /// <inheritdoc />
    public partial class change_relation_between_product_and_productDetail : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_products_product_details_product_detail_id",
                table: "products");

            migrationBuilder.CreateIndex(
                name: "IX_products_product_details_product_detail_id",
                table: "products",
                column: "product_details_product_detail_id",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_products_product_details_product_detail_id",
                table: "products");

            migrationBuilder.CreateIndex(
                name: "IX_products_product_details_product_detail_id",
                table: "products",
                column: "product_details_product_detail_id");
        }
    }
}
