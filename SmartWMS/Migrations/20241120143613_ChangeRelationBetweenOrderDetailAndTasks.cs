using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SmartWMS.Migrations
{
    /// <inheritdoc />
    public partial class ChangeRelationBetweenOrderDetailAndTasks : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_tasks_orderDetails_orderDetail_id",
                table: "tasks");

            migrationBuilder.AddColumn<string>(
                name: "Barcode",
                table: "waybills",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_tasks_orderDetails_orderDetail_id",
                table: "tasks",
                column: "orderDetails_orderDetail_id",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_tasks_orderDetails_orderDetail_id",
                table: "tasks");

            migrationBuilder.DropColumn(
                name: "Barcode",
                table: "waybills");

            migrationBuilder.CreateIndex(
                name: "IX_tasks_orderDetails_orderDetail_id",
                table: "tasks",
                column: "orderDetails_orderDetail_id");
        }
    }
}
