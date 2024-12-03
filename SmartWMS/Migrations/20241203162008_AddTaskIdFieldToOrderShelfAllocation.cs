using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SmartWMS.Migrations
{
    /// <inheritdoc />
    public partial class AddTaskIdFieldToOrderShelfAllocation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "task_id",
                table: "orderShelvesAllocation",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "task_id",
                table: "orderShelvesAllocation");
        }
    }
}
