using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SmartWMS.Migrations
{
    /// <inheritdoc />
    public partial class change_relation_between_orderHeader_and_waybill : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_order_headers_waybills1",
                table: "order_headers");

            migrationBuilder.DropIndex(
                name: "IX_order_headers_waybills_waybill_id",
                table: "order_headers");

            migrationBuilder.DropColumn(
                name: "waybills_waybill_id",
                table: "order_headers");

            migrationBuilder.AddColumn<int>(
                name: "OrderHeadersOrderHeaderId",
                table: "waybills",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_waybills_OrderHeadersOrderHeaderId",
                table: "waybills",
                column: "OrderHeadersOrderHeaderId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "fk_order_headers_waybills1",
                table: "waybills",
                column: "OrderHeadersOrderHeaderId",
                principalTable: "order_headers",
                principalColumn: "orders_header_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_order_headers_waybills1",
                table: "waybills");

            migrationBuilder.DropIndex(
                name: "IX_waybills_OrderHeadersOrderHeaderId",
                table: "waybills");

            migrationBuilder.DropColumn(
                name: "OrderHeadersOrderHeaderId",
                table: "waybills");

            migrationBuilder.AddColumn<int>(
                name: "waybills_waybill_id",
                table: "order_headers",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_order_headers_waybills_waybill_id",
                table: "order_headers",
                column: "waybills_waybill_id");

            migrationBuilder.AddForeignKey(
                name: "fk_order_headers_waybills1",
                table: "order_headers",
                column: "waybills_waybill_id",
                principalTable: "waybills",
                principalColumn: "waybill_id");
        }
    }
}
