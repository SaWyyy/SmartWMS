using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SmartWMS.Migrations
{
    /// <inheritdoc />
    public partial class ChangeTasksOrderDetailRelation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_tasks_order_headers1",
                table: "tasks");

            migrationBuilder.DropTable(
                name: "products_has_tasks");

            migrationBuilder.DropIndex(
                name: "IX_tasks_order_headers_orders_header_id",
                table: "tasks");

            migrationBuilder.RenameColumn(
                name: "order_headers_orders_header_id",
                table: "tasks",
                newName: "quantity_collected");

            migrationBuilder.AddColumn<int>(
                name: "orderDetails_orderDetail_id",
                table: "tasks",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "quantity_allocated",
                table: "tasks",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_tasks_orderDetails_orderDetail_id",
                table: "tasks",
                column: "orderDetails_orderDetail_id",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "fk_order_details_tasks1",
                table: "tasks",
                column: "orderDetails_orderDetail_id",
                principalTable: "order_details",
                principalColumn: "order_detail_id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_order_details_tasks1",
                table: "tasks");

            migrationBuilder.DropIndex(
                name: "IX_tasks_orderDetails_orderDetail_id",
                table: "tasks");

            migrationBuilder.DropColumn(
                name: "orderDetails_orderDetail_id",
                table: "tasks");

            migrationBuilder.DropColumn(
                name: "quantity_allocated",
                table: "tasks");

            migrationBuilder.RenameColumn(
                name: "quantity_collected",
                table: "tasks",
                newName: "order_headers_orders_header_id");

            migrationBuilder.CreateTable(
                name: "products_has_tasks",
                columns: table => new
                {
                    products_product_id = table.Column<int>(type: "integer", nullable: false),
                    tasks_task_id = table.Column<int>(type: "integer", nullable: false),
                    quantity_allocated = table.Column<int>(type: "integer", nullable: false),
                    quantity_collected = table.Column<int>(type: "integer", nullable: true, defaultValue: 0)
                },
                constraints: table =>
                {
                    table.PrimaryKey("products_has_tasks_pkey", x => new { x.products_product_id, x.tasks_task_id });
                    table.ForeignKey(
                        name: "fk_products_has_tasks_products1",
                        column: x => x.products_product_id,
                        principalTable: "products",
                        principalColumn: "product_id");
                    table.ForeignKey(
                        name: "fk_products_has_tasks_tasks1",
                        column: x => x.tasks_task_id,
                        principalTable: "tasks",
                        principalColumn: "task_id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_tasks_order_headers_orders_header_id",
                table: "tasks",
                column: "order_headers_orders_header_id");

            migrationBuilder.CreateIndex(
                name: "IX_products_has_tasks_tasks_task_id",
                table: "products_has_tasks",
                column: "tasks_task_id");

            migrationBuilder.AddForeignKey(
                name: "fk_tasks_order_headers1",
                table: "tasks",
                column: "order_headers_orders_header_id",
                principalTable: "order_headers",
                principalColumn: "orders_header_id");
        }
    }
}
