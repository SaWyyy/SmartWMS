using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SmartWMS.Migrations
{
    /// <inheritdoc />
    public partial class change_realtion_between_task_and_orderdetail : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_tasks_orderDetails_orderDetail_id",
                table: "tasks");

            migrationBuilder.CreateIndex(
                name: "IX_tasks_orderDetails_orderDetail_id",
                table: "tasks",
                column: "orderDetails_orderDetail_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_tasks_orderDetails_orderDetail_id",
                table: "tasks");

            migrationBuilder.CreateIndex(
                name: "IX_tasks_orderDetails_orderDetail_id",
                table: "tasks",
                column: "orderDetails_orderDetail_id",
                unique: true);
        }
    }
}
