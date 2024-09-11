using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SmartWMS.Migrations
{
    /// <inheritdoc />
    public partial class change_price_type_in_product : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("ALTER TABLE products ALTER COLUMN price TYPE money USING price::numeric;");

        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "price",
                table: "products",
                type: "character varying(45)",
                maxLength: 45,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "money");
        }
    }
}
