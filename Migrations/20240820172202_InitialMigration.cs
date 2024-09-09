using System;
using Microsoft.EntityFrameworkCore.Migrations;
using SmartWMS.Entities.Enums;

#nullable disable

namespace SmartWMS.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("Npgsql:Enum:action_type", "given,taken")
                .Annotation("Npgsql:Enum:alert_type", "delivery_delay,delivery_canceled,delivery_returner,product_shortage")
                .Annotation("Npgsql:Enum:level_type", "p0,p1,p2,p3,p4")
                .Annotation("Npgsql:Enum:order_name", "planned,shipped,realized,cancelled")
                .Annotation("Npgsql:Enum:order_type", "delivery,shipment")
                .Annotation("Npgsql:Enum:report_period", "day,week,month,quarter,year")
                .Annotation("Npgsql:Enum:report_type", "warehouse_state,deliveries,shipments,returns,tasks_submitted");

            migrationBuilder.CreateTable(
                name: "categories",
                columns: table => new
                {
                    category_id = table.Column<int>(type: "integer", nullable: false),
                    category_name = table.Column<string>(type: "character varying(45)", maxLength: 45, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("categories_id_unique", x => x.category_id);
                });

            migrationBuilder.CreateTable(
                name: "countries",
                columns: table => new
                {
                    country_id = table.Column<int>(type: "integer", nullable: false),
                    country_name = table.Column<string>(type: "character varying(45)", maxLength: 45, nullable: false),
                    country_code = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("country_id_unique", x => x.country_id);
                });

            migrationBuilder.CreateTable(
                name: "product_details",
                columns: table => new
                {
                    product_detail_id = table.Column<int>(type: "integer", nullable: false),
                    quantity = table.Column<int>(type: "integer", nullable: false),
                    barcode = table.Column<string>(type: "character varying(8)", maxLength: 8, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("warehouse_state_id_unique", x => x.product_detail_id);
                });

            migrationBuilder.CreateTable(
                name: "warehouses",
                columns: table => new
                {
                    warehouse_id = table.Column<int>(type: "integer", nullable: false),
                    address = table.Column<string>(type: "character varying(45)", maxLength: 45, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("warehouses_pkey", x => x.warehouse_id);
                });

            migrationBuilder.CreateTable(
                name: "subcategories",
                columns: table => new
                {
                    subcategory_id = table.Column<int>(type: "integer", nullable: false),
                    subcategory_name = table.Column<string>(type: "character varying(45)", maxLength: 45, nullable: false),
                    categories_category_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("subcategories_id_unique", x => x.subcategory_id);
                    table.ForeignKey(
                        name: "fk_subcategories_categories1",
                        column: x => x.categories_category_id,
                        principalTable: "categories",
                        principalColumn: "category_id");
                });

            migrationBuilder.CreateTable(
                name: "waybills",
                columns: table => new
                {
                    waybill_id = table.Column<int>(type: "integer", nullable: false),
                    shipping_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    loading_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    countries_country_id = table.Column<int>(type: "integer", nullable: false),
                    postal_code = table.Column<string>(type: "character varying(45)", maxLength: 45, nullable: false),
                    supplier_name = table.Column<string>(type: "character varying(45)", maxLength: 45, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("waybill_id_unique", x => x.waybill_id);
                    table.ForeignKey(
                        name: "fk_waybills_countries1",
                        column: x => x.countries_country_id,
                        principalTable: "countries",
                        principalColumn: "country_id");
                });

            migrationBuilder.CreateTable(
                name: "alerts",
                columns: table => new
                {
                    alert_id = table.Column<int>(type: "integer", nullable: false),
                    seen = table.Column<bool>(type: "boolean", nullable: false),
                    alert_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    warehouses_warehouse_id = table.Column<int>(type: "integer", nullable: false),
                    alert_type = table.Column<AlertType>(type: "alert_type", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("alerts_id_unique", x => x.alert_id);
                    table.ForeignKey(
                        name: "fk_alerts_warehouses1",
                        column: x => x.warehouses_warehouse_id,
                        principalTable: "warehouses",
                        principalColumn: "warehouse_id");
                });

            migrationBuilder.CreateTable(
                name: "reports",
                columns: table => new
                {
                    report_id = table.Column<int>(type: "integer", nullable: false),
                    report_type = table.Column<ReportType>(type: "report_type", nullable: false),
                    report_period = table.Column<ReportPeriod>(type: "report_period", nullable: false),
                    report_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    warehouses_warehouse_id = table.Column<int>(type: "integer", nullable: false),
                    report_file = table.Column<byte[]>(type: "bytea", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("report_id_unique", x => x.report_id);
                    table.ForeignKey(
                        name: "fk_reports_warehouses1",
                        column: x => x.warehouses_warehouse_id,
                        principalTable: "warehouses",
                        principalColumn: "warehouse_id");
                });

            migrationBuilder.CreateTable(
                name: "users",
                columns: table => new
                {
                    user_id = table.Column<int>(type: "integer", nullable: false),
                    login = table.Column<string>(type: "character varying(45)", maxLength: 45, nullable: false),
                    password = table.Column<string>(type: "character varying(45)", maxLength: 45, nullable: false),
                    email = table.Column<string>(type: "character varying(45)", maxLength: 45, nullable: false),
                    manager_id = table.Column<int>(type: "integer", nullable: true),
                    warehouses_warehouse_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("users_id_unique", x => x.user_id);
                    table.ForeignKey(
                        name: "fk_users_users1",
                        column: x => x.manager_id,
                        principalTable: "users",
                        principalColumn: "user_id");
                    table.ForeignKey(
                        name: "fk_users_warehouses1",
                        column: x => x.warehouses_warehouse_id,
                        principalTable: "warehouses",
                        principalColumn: "warehouse_id");
                });

            migrationBuilder.CreateTable(
                name: "products",
                columns: table => new
                {
                    product_id = table.Column<int>(type: "integer", nullable: false),
                    product_name = table.Column<string>(type: "character varying(45)", maxLength: 45, nullable: false),
                    product_description = table.Column<string>(type: "character varying(45)", maxLength: 45, nullable: true),
                    price = table.Column<string>(type: "character varying(45)", maxLength: 45, nullable: false),
                    warehouses_warehouse_id = table.Column<int>(type: "integer", nullable: false),
                    product_details_product_detail_id = table.Column<int>(type: "integer", nullable: false),
                    subcategories_subcategory_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("idprodukty_unique", x => x.product_id);
                    table.ForeignKey(
                        name: "fk_products_product_details1",
                        column: x => x.product_details_product_detail_id,
                        principalTable: "product_details",
                        principalColumn: "product_detail_id");
                    table.ForeignKey(
                        name: "fk_products_subcategories1",
                        column: x => x.subcategories_subcategory_id,
                        principalTable: "subcategories",
                        principalColumn: "subcategory_id");
                    table.ForeignKey(
                        name: "fk_products_warehouses1",
                        column: x => x.warehouses_warehouse_id,
                        principalTable: "warehouses",
                        principalColumn: "warehouse_id");
                });

            migrationBuilder.CreateTable(
                name: "order_headers",
                columns: table => new
                {
                    orders_header_id = table.Column<int>(type: "integer", nullable: false),
                    order_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    delivery_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    destination_address = table.Column<string>(type: "character varying(45)", maxLength: 45, nullable: false),
                    waybills_waybill_id = table.Column<int>(type: "integer", nullable: false),
                    type_name = table.Column<OrderType>(type: "order_type", nullable: false),
                    status_name = table.Column<OrderName>(type: "order_name", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("orders_id_unique", x => x.orders_header_id);
                    table.ForeignKey(
                        name: "fk_order_headers_waybills1",
                        column: x => x.waybills_waybill_id,
                        principalTable: "waybills",
                        principalColumn: "waybill_id");
                });

            migrationBuilder.CreateTable(
                name: "shelf",
                columns: table => new
                {
                    warehouse_localization_id = table.Column<int>(type: "integer", nullable: false),
                    lane = table.Column<string>(type: "character varying(2)", maxLength: 2, nullable: false),
                    rack = table.Column<int>(type: "integer", nullable: false),
                    level = table.Column<LevelType>(type: "level_type", nullable: false),
                    max_quant = table.Column<int>(type: "integer", nullable: false),
                    current_quant = table.Column<int>(type: "integer", nullable: false),
                    products_product_id = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("warehouse_localization_id_unique", x => x.warehouse_localization_id);
                    table.ForeignKey(
                        name: "fk_shelf_products1",
                        column: x => x.products_product_id,
                        principalTable: "products",
                        principalColumn: "product_id");
                });

            migrationBuilder.CreateTable(
                name: "order_details",
                columns: table => new
                {
                    order_detail_id = table.Column<int>(type: "integer", nullable: false),
                    quantity = table.Column<int>(type: "integer", nullable: false),
                    products_product_id = table.Column<int>(type: "integer", nullable: false),
                    order_headers_orders_header_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("order_detail_id_unique", x => x.order_detail_id);
                    table.ForeignKey(
                        name: "fk_order_details_order_headers1",
                        column: x => x.order_headers_orders_header_id,
                        principalTable: "order_headers",
                        principalColumn: "orders_header_id");
                    table.ForeignKey(
                        name: "fk_order_details_products1",
                        column: x => x.products_product_id,
                        principalTable: "products",
                        principalColumn: "product_id");
                });

            migrationBuilder.CreateTable(
                name: "tasks",
                columns: table => new
                {
                    task_id = table.Column<int>(type: "integer", nullable: false),
                    start_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    finish_date = table.Column<string>(type: "character varying(45)", maxLength: 45, nullable: true),
                    priority = table.Column<int>(type: "integer", nullable: false),
                    order_headers_orders_header_id = table.Column<int>(type: "integer", nullable: false),
                    seen = table.Column<bool>(type: "boolean", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("task_id_unique", x => x.task_id);
                    table.ForeignKey(
                        name: "fk_tasks_order_headers1",
                        column: x => x.order_headers_orders_header_id,
                        principalTable: "order_headers",
                        principalColumn: "orders_header_id");
                });

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

            migrationBuilder.CreateTable(
                name: "users_has_tasks",
                columns: table => new
                {
                    users_user_id = table.Column<int>(type: "integer", nullable: false),
                    tasks_task_id = table.Column<int>(type: "integer", nullable: false),
                    action = table.Column<ActionType>(type: "action_type", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("users_has_tasks_pkey", x => new { x.users_user_id, x.tasks_task_id });
                    table.ForeignKey(
                        name: "fk_users_has_tasks_tasks1",
                        column: x => x.tasks_task_id,
                        principalTable: "tasks",
                        principalColumn: "task_id");
                    table.ForeignKey(
                        name: "fk_users_has_tasks_users1",
                        column: x => x.users_user_id,
                        principalTable: "users",
                        principalColumn: "user_id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_alerts_warehouses_warehouse_id",
                table: "alerts",
                column: "warehouses_warehouse_id");

            migrationBuilder.CreateIndex(
                name: "countrycode_unique",
                table: "countries",
                column: "country_code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "countryname_unique",
                table: "countries",
                column: "country_name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_order_details_order_headers_orders_header_id",
                table: "order_details",
                column: "order_headers_orders_header_id");

            migrationBuilder.CreateIndex(
                name: "IX_order_details_products_product_id",
                table: "order_details",
                column: "products_product_id");

            migrationBuilder.CreateIndex(
                name: "IX_order_headers_waybills_waybill_id",
                table: "order_headers",
                column: "waybills_waybill_id");

            migrationBuilder.CreateIndex(
                name: "IX_products_product_details_product_detail_id",
                table: "products",
                column: "product_details_product_detail_id");

            migrationBuilder.CreateIndex(
                name: "IX_products_subcategories_subcategory_id",
                table: "products",
                column: "subcategories_subcategory_id");

            migrationBuilder.CreateIndex(
                name: "IX_products_warehouses_warehouse_id",
                table: "products",
                column: "warehouses_warehouse_id");

            migrationBuilder.CreateIndex(
                name: "IX_products_has_tasks_tasks_task_id",
                table: "products_has_tasks",
                column: "tasks_task_id");

            migrationBuilder.CreateIndex(
                name: "IX_reports_warehouses_warehouse_id",
                table: "reports",
                column: "warehouses_warehouse_id");

            migrationBuilder.CreateIndex(
                name: "IX_shelf_products_product_id",
                table: "shelf",
                column: "products_product_id");

            migrationBuilder.CreateIndex(
                name: "lane_unique",
                table: "shelf",
                column: "lane",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_subcategories_categories_category_id",
                table: "subcategories",
                column: "categories_category_id");

            migrationBuilder.CreateIndex(
                name: "IX_tasks_order_headers_orders_header_id",
                table: "tasks",
                column: "order_headers_orders_header_id");

            migrationBuilder.CreateIndex(
                name: "email_unique",
                table: "users",
                column: "email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_users_manager_id",
                table: "users",
                column: "manager_id");

            migrationBuilder.CreateIndex(
                name: "IX_users_warehouses_warehouse_id",
                table: "users",
                column: "warehouses_warehouse_id");

            migrationBuilder.CreateIndex(
                name: "login_unique",
                table: "users",
                column: "login",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_users_has_tasks_tasks_task_id",
                table: "users_has_tasks",
                column: "tasks_task_id");

            migrationBuilder.CreateIndex(
                name: "IX_waybills_countries_country_id",
                table: "waybills",
                column: "countries_country_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "alerts");

            migrationBuilder.DropTable(
                name: "order_details");

            migrationBuilder.DropTable(
                name: "products_has_tasks");

            migrationBuilder.DropTable(
                name: "reports");

            migrationBuilder.DropTable(
                name: "shelf");

            migrationBuilder.DropTable(
                name: "users_has_tasks");

            migrationBuilder.DropTable(
                name: "products");

            migrationBuilder.DropTable(
                name: "tasks");

            migrationBuilder.DropTable(
                name: "users");

            migrationBuilder.DropTable(
                name: "product_details");

            migrationBuilder.DropTable(
                name: "subcategories");

            migrationBuilder.DropTable(
                name: "order_headers");

            migrationBuilder.DropTable(
                name: "warehouses");

            migrationBuilder.DropTable(
                name: "categories");

            migrationBuilder.DropTable(
                name: "waybills");

            migrationBuilder.DropTable(
                name: "countries");
        }
    }
}
