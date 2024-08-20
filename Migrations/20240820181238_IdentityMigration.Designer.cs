﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using SmartWMS.Models;
using SmartWMS.Models.Enums;

#nullable disable

namespace SmartWMS.Migrations
{
    [DbContext(typeof(SmartwmsDbContext))]
    [Migration("20240820181238_IdentityMigration")]
    partial class IdentityMigration
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.8")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.HasPostgresEnum(modelBuilder, "action_type", new[] { "given", "taken" });
            NpgsqlModelBuilderExtensions.HasPostgresEnum(modelBuilder, "alert_type", new[] { "delivery_delay", "delivery_canceled", "delivery_returner", "product_shortage" });
            NpgsqlModelBuilderExtensions.HasPostgresEnum(modelBuilder, "level_type", new[] { "p0", "p1", "p2", "p3", "p4" });
            NpgsqlModelBuilderExtensions.HasPostgresEnum(modelBuilder, "order_name", new[] { "planned", "shipped", "realized", "cancelled" });
            NpgsqlModelBuilderExtensions.HasPostgresEnum(modelBuilder, "order_type", new[] { "delivery", "shipment" });
            NpgsqlModelBuilderExtensions.HasPostgresEnum(modelBuilder, "report_period", new[] { "day", "week", "month", "quarter", "year" });
            NpgsqlModelBuilderExtensions.HasPostgresEnum(modelBuilder, "report_type", new[] { "warehouse_state", "deliveries", "shipments", "returns", "tasks_submitted" });
            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasDatabaseName("RoleNameIndex");

                    b.ToTable("AspNetRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("text");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("text");

                    b.Property<string>("RoleId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("text");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("text");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasColumnType("text");

                    b.Property<string>("ProviderKey")
                        .HasColumnType("text");

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("text");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("text");

                    b.Property<string>("RoleId")
                        .HasColumnType("text");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("text");

                    b.Property<string>("LoginProvider")
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.Property<string>("Value")
                        .HasColumnType("text");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens", (string)null);
                });

            modelBuilder.Entity("SmartWMS.Models.Alert", b =>
                {
                    b.Property<int>("AlertId")
                        .HasColumnType("integer")
                        .HasColumnName("alert_id");

                    b.Property<DateTime>("AlertDate")
                        .HasColumnType("timestamp without time zone")
                        .HasColumnName("alert_date");

                    b.Property<AlertType>("AlertType")
                        .HasColumnType("alert_type")
                        .HasColumnName("alert_type");

                    b.Property<bool>("Seen")
                        .HasColumnType("boolean")
                        .HasColumnName("seen");

                    b.Property<int>("WarehousesWarehouseId")
                        .HasColumnType("integer")
                        .HasColumnName("warehouses_warehouse_id");

                    b.HasKey("AlertId")
                        .HasName("alerts_id_unique");

                    b.HasIndex("WarehousesWarehouseId");

                    b.ToTable("alerts", (string)null);
                });

            modelBuilder.Entity("SmartWMS.Models.Category", b =>
                {
                    b.Property<int>("CategoryId")
                        .HasColumnType("integer")
                        .HasColumnName("category_id");

                    b.Property<string>("CategoryName")
                        .IsRequired()
                        .HasMaxLength(45)
                        .HasColumnType("character varying(45)")
                        .HasColumnName("category_name");

                    b.HasKey("CategoryId")
                        .HasName("categories_id_unique");

                    b.ToTable("categories", (string)null);
                });

            modelBuilder.Entity("SmartWMS.Models.Country", b =>
                {
                    b.Property<int>("CountryId")
                        .HasColumnType("integer")
                        .HasColumnName("country_id");

                    b.Property<int>("CountryCode")
                        .HasColumnType("integer")
                        .HasColumnName("country_code");

                    b.Property<string>("CountryName")
                        .IsRequired()
                        .HasMaxLength(45)
                        .HasColumnType("character varying(45)")
                        .HasColumnName("country_name");

                    b.HasKey("CountryId")
                        .HasName("country_id_unique");

                    b.HasIndex(new[] { "CountryCode" }, "countrycode_unique")
                        .IsUnique();

                    b.HasIndex(new[] { "CountryName" }, "countryname_unique")
                        .IsUnique();

                    b.ToTable("countries", (string)null);
                });

            modelBuilder.Entity("SmartWMS.Models.OrderDetail", b =>
                {
                    b.Property<int>("OrderDetailId")
                        .HasColumnType("integer")
                        .HasColumnName("order_detail_id");

                    b.Property<int>("OrderHeadersOrdersHeaderId")
                        .HasColumnType("integer")
                        .HasColumnName("order_headers_orders_header_id");

                    b.Property<int>("ProductsProductId")
                        .HasColumnType("integer")
                        .HasColumnName("products_product_id");

                    b.Property<int>("Quantity")
                        .HasColumnType("integer")
                        .HasColumnName("quantity");

                    b.HasKey("OrderDetailId")
                        .HasName("order_detail_id_unique");

                    b.HasIndex("OrderHeadersOrdersHeaderId");

                    b.HasIndex("ProductsProductId");

                    b.ToTable("order_details", (string)null);
                });

            modelBuilder.Entity("SmartWMS.Models.OrderHeader", b =>
                {
                    b.Property<int>("OrdersHeaderId")
                        .HasColumnType("integer")
                        .HasColumnName("orders_header_id");

                    b.Property<DateTime?>("DeliveryDate")
                        .HasColumnType("timestamp without time zone")
                        .HasColumnName("delivery_date");

                    b.Property<string>("DestinationAddress")
                        .IsRequired()
                        .HasMaxLength(45)
                        .HasColumnType("character varying(45)")
                        .HasColumnName("destination_address");

                    b.Property<DateTime>("OrderDate")
                        .HasColumnType("timestamp without time zone")
                        .HasColumnName("order_date");

                    b.Property<OrderName>("StatusName")
                        .HasColumnType("order_name")
                        .HasColumnName("status_name");

                    b.Property<OrderType>("TypeName")
                        .HasColumnType("order_type")
                        .HasColumnName("type_name");

                    b.Property<int>("WaybillsWaybillId")
                        .HasColumnType("integer")
                        .HasColumnName("waybills_waybill_id");

                    b.HasKey("OrdersHeaderId")
                        .HasName("orders_id_unique");

                    b.HasIndex("WaybillsWaybillId");

                    b.ToTable("order_headers", (string)null);
                });

            modelBuilder.Entity("SmartWMS.Models.Product", b =>
                {
                    b.Property<int>("ProductId")
                        .HasColumnType("integer")
                        .HasColumnName("product_id");

                    b.Property<string>("Price")
                        .IsRequired()
                        .HasMaxLength(45)
                        .HasColumnType("character varying(45)")
                        .HasColumnName("price");

                    b.Property<string>("ProductDescription")
                        .HasMaxLength(45)
                        .HasColumnType("character varying(45)")
                        .HasColumnName("product_description");

                    b.Property<int>("ProductDetailsProductDetailId")
                        .HasColumnType("integer")
                        .HasColumnName("product_details_product_detail_id");

                    b.Property<string>("ProductName")
                        .IsRequired()
                        .HasMaxLength(45)
                        .HasColumnType("character varying(45)")
                        .HasColumnName("product_name");

                    b.Property<int>("SubcategoriesSubcategoryId")
                        .HasColumnType("integer")
                        .HasColumnName("subcategories_subcategory_id");

                    b.Property<int>("WarehousesWarehouseId")
                        .HasColumnType("integer")
                        .HasColumnName("warehouses_warehouse_id");

                    b.HasKey("ProductId")
                        .HasName("idprodukty_unique");

                    b.HasIndex("ProductDetailsProductDetailId");

                    b.HasIndex("SubcategoriesSubcategoryId");

                    b.HasIndex("WarehousesWarehouseId");

                    b.ToTable("products", (string)null);
                });

            modelBuilder.Entity("SmartWMS.Models.ProductDetail", b =>
                {
                    b.Property<int>("ProductDetailId")
                        .HasColumnType("integer")
                        .HasColumnName("product_detail_id");

                    b.Property<string>("Barcode")
                        .IsRequired()
                        .HasMaxLength(8)
                        .HasColumnType("character varying(8)")
                        .HasColumnName("barcode");

                    b.Property<int>("Quantity")
                        .HasColumnType("integer")
                        .HasColumnName("quantity");

                    b.HasKey("ProductDetailId")
                        .HasName("warehouse_state_id_unique");

                    b.ToTable("product_details", (string)null);
                });

            modelBuilder.Entity("SmartWMS.Models.ProductsHasTask", b =>
                {
                    b.Property<int>("ProductsProductId")
                        .HasColumnType("integer")
                        .HasColumnName("products_product_id");

                    b.Property<int>("TasksTaskId")
                        .HasColumnType("integer")
                        .HasColumnName("tasks_task_id");

                    b.Property<int>("QuantityAllocated")
                        .HasColumnType("integer")
                        .HasColumnName("quantity_allocated");

                    b.Property<int?>("QuantityCollected")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasDefaultValue(0)
                        .HasColumnName("quantity_collected");

                    b.HasKey("ProductsProductId", "TasksTaskId")
                        .HasName("products_has_tasks_pkey");

                    b.HasIndex("TasksTaskId");

                    b.ToTable("products_has_tasks", (string)null);
                });

            modelBuilder.Entity("SmartWMS.Models.Report", b =>
                {
                    b.Property<int>("ReportId")
                        .HasColumnType("integer")
                        .HasColumnName("report_id");

                    b.Property<DateTime>("ReportDate")
                        .HasColumnType("timestamp without time zone")
                        .HasColumnName("report_date");

                    b.Property<byte[]>("ReportFile")
                        .IsRequired()
                        .HasColumnType("bytea")
                        .HasColumnName("report_file");

                    b.Property<ReportPeriod>("ReportPeriod")
                        .HasColumnType("report_period")
                        .HasColumnName("report_period");

                    b.Property<ReportType>("ReportType")
                        .HasColumnType("report_type")
                        .HasColumnName("report_type");

                    b.Property<int>("WarehousesWarehouseId")
                        .HasColumnType("integer")
                        .HasColumnName("warehouses_warehouse_id");

                    b.HasKey("ReportId")
                        .HasName("report_id_unique");

                    b.HasIndex("WarehousesWarehouseId");

                    b.ToTable("reports", (string)null);
                });

            modelBuilder.Entity("SmartWMS.Models.Shelf", b =>
                {
                    b.Property<int>("WarehouseLocalizationId")
                        .HasColumnType("integer")
                        .HasColumnName("warehouse_localization_id");

                    b.Property<int>("CurrentQuant")
                        .HasColumnType("integer")
                        .HasColumnName("current_quant");

                    b.Property<string>("Lane")
                        .IsRequired()
                        .HasMaxLength(2)
                        .HasColumnType("character varying(2)")
                        .HasColumnName("lane");

                    b.Property<LevelType>("Level")
                        .HasColumnType("level_type")
                        .HasColumnName("level");

                    b.Property<int>("MaxQuant")
                        .HasColumnType("integer")
                        .HasColumnName("max_quant");

                    b.Property<int?>("ProductsProductId")
                        .HasColumnType("integer")
                        .HasColumnName("products_product_id");

                    b.Property<int>("Rack")
                        .HasColumnType("integer")
                        .HasColumnName("rack");

                    b.HasKey("WarehouseLocalizationId")
                        .HasName("warehouse_localization_id_unique");

                    b.HasIndex("ProductsProductId");

                    b.HasIndex(new[] { "Lane" }, "lane_unique")
                        .IsUnique();

                    b.ToTable("shelf", (string)null);
                });

            modelBuilder.Entity("SmartWMS.Models.Subcategory", b =>
                {
                    b.Property<int>("SubcategoryId")
                        .HasColumnType("integer")
                        .HasColumnName("subcategory_id");

                    b.Property<int>("CategoriesCategoryId")
                        .HasColumnType("integer")
                        .HasColumnName("categories_category_id");

                    b.Property<string>("SubcategoryName")
                        .IsRequired()
                        .HasMaxLength(45)
                        .HasColumnType("character varying(45)")
                        .HasColumnName("subcategory_name");

                    b.HasKey("SubcategoryId")
                        .HasName("subcategories_id_unique");

                    b.HasIndex("CategoriesCategoryId");

                    b.ToTable("subcategories", (string)null);
                });

            modelBuilder.Entity("SmartWMS.Models.Task", b =>
                {
                    b.Property<int>("TaskId")
                        .HasColumnType("integer")
                        .HasColumnName("task_id");

                    b.Property<string>("FinishDate")
                        .HasMaxLength(45)
                        .HasColumnType("character varying(45)")
                        .HasColumnName("finish_date");

                    b.Property<int>("OrderHeadersOrdersHeaderId")
                        .HasColumnType("integer")
                        .HasColumnName("order_headers_orders_header_id");

                    b.Property<int>("Priority")
                        .HasColumnType("integer")
                        .HasColumnName("priority");

                    b.Property<bool?>("Seen")
                        .HasColumnType("boolean")
                        .HasColumnName("seen");

                    b.Property<DateTime?>("StartDate")
                        .HasColumnType("timestamp without time zone")
                        .HasColumnName("start_date");

                    b.HasKey("TaskId")
                        .HasName("task_id_unique");

                    b.HasIndex("OrderHeadersOrdersHeaderId");

                    b.ToTable("tasks", (string)null);
                });

            modelBuilder.Entity("SmartWMS.Models.User", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("integer");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("text");

                    b.Property<string>("Email")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("boolean");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("boolean");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("ManagerId")
                        .HasColumnType("text")
                        .HasColumnName("manager_id");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("text");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("text");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("boolean");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("text");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("boolean");

                    b.Property<string>("UserName")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<int>("WarehousesWarehouseId")
                        .HasColumnType("integer")
                        .HasColumnName("warehouses_warehouse_id");

                    b.HasKey("Id");

                    b.HasIndex("ManagerId");

                    b.HasIndex("NormalizedEmail")
                        .HasDatabaseName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasDatabaseName("UserNameIndex");

                    b.HasIndex("WarehousesWarehouseId");

                    b.ToTable("users", (string)null);
                });

            modelBuilder.Entity("SmartWMS.Models.UsersHasTask", b =>
                {
                    b.Property<string>("UsersUserId")
                        .HasColumnType("text")
                        .HasColumnName("users_user_id");

                    b.Property<int>("TasksTaskId")
                        .HasColumnType("integer")
                        .HasColumnName("tasks_task_id");

                    b.Property<ActionType>("Action")
                        .HasColumnType("action_type")
                        .HasColumnName("action");

                    b.HasKey("UsersUserId", "TasksTaskId")
                        .HasName("users_has_tasks_pkey");

                    b.HasIndex("TasksTaskId");

                    b.ToTable("users_has_tasks", (string)null);
                });

            modelBuilder.Entity("SmartWMS.Models.Warehouse", b =>
                {
                    b.Property<int>("WarehouseId")
                        .HasColumnType("integer")
                        .HasColumnName("warehouse_id");

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasMaxLength(45)
                        .HasColumnType("character varying(45)")
                        .HasColumnName("address");

                    b.HasKey("WarehouseId")
                        .HasName("warehouses_pkey");

                    b.ToTable("warehouses", (string)null);
                });

            modelBuilder.Entity("SmartWMS.Models.Waybill", b =>
                {
                    b.Property<int>("WaybillId")
                        .HasColumnType("integer")
                        .HasColumnName("waybill_id");

                    b.Property<int>("CountriesCountryId")
                        .HasColumnType("integer")
                        .HasColumnName("countries_country_id");

                    b.Property<DateTime>("LoadingDate")
                        .HasColumnType("timestamp without time zone")
                        .HasColumnName("loading_date");

                    b.Property<string>("PostalCode")
                        .IsRequired()
                        .HasMaxLength(45)
                        .HasColumnType("character varying(45)")
                        .HasColumnName("postal_code");

                    b.Property<DateTime>("ShippingDate")
                        .HasColumnType("timestamp without time zone")
                        .HasColumnName("shipping_date");

                    b.Property<string>("SupplierName")
                        .IsRequired()
                        .HasMaxLength(45)
                        .HasColumnType("character varying(45)")
                        .HasColumnName("supplier_name");

                    b.HasKey("WaybillId")
                        .HasName("waybill_id_unique");

                    b.HasIndex("CountriesCountryId");

                    b.ToTable("waybills", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("SmartWMS.Models.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("SmartWMS.Models.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("SmartWMS.Models.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("SmartWMS.Models.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("SmartWMS.Models.Alert", b =>
                {
                    b.HasOne("SmartWMS.Models.Warehouse", "WarehousesWarehouse")
                        .WithMany("Alerts")
                        .HasForeignKey("WarehousesWarehouseId")
                        .IsRequired()
                        .HasConstraintName("fk_alerts_warehouses1");

                    b.Navigation("WarehousesWarehouse");
                });

            modelBuilder.Entity("SmartWMS.Models.OrderDetail", b =>
                {
                    b.HasOne("SmartWMS.Models.OrderHeader", "OrderHeadersOrdersHeader")
                        .WithMany("OrderDetails")
                        .HasForeignKey("OrderHeadersOrdersHeaderId")
                        .IsRequired()
                        .HasConstraintName("fk_order_details_order_headers1");

                    b.HasOne("SmartWMS.Models.Product", "ProductsProduct")
                        .WithMany("OrderDetails")
                        .HasForeignKey("ProductsProductId")
                        .IsRequired()
                        .HasConstraintName("fk_order_details_products1");

                    b.Navigation("OrderHeadersOrdersHeader");

                    b.Navigation("ProductsProduct");
                });

            modelBuilder.Entity("SmartWMS.Models.OrderHeader", b =>
                {
                    b.HasOne("SmartWMS.Models.Waybill", "WaybillsWaybill")
                        .WithMany("OrderHeaders")
                        .HasForeignKey("WaybillsWaybillId")
                        .IsRequired()
                        .HasConstraintName("fk_order_headers_waybills1");

                    b.Navigation("WaybillsWaybill");
                });

            modelBuilder.Entity("SmartWMS.Models.Product", b =>
                {
                    b.HasOne("SmartWMS.Models.ProductDetail", "ProductDetailsProductDetail")
                        .WithMany("Products")
                        .HasForeignKey("ProductDetailsProductDetailId")
                        .IsRequired()
                        .HasConstraintName("fk_products_product_details1");

                    b.HasOne("SmartWMS.Models.Subcategory", "SubcategoriesSubcategory")
                        .WithMany("Products")
                        .HasForeignKey("SubcategoriesSubcategoryId")
                        .IsRequired()
                        .HasConstraintName("fk_products_subcategories1");

                    b.HasOne("SmartWMS.Models.Warehouse", "WarehousesWarehouse")
                        .WithMany("Products")
                        .HasForeignKey("WarehousesWarehouseId")
                        .IsRequired()
                        .HasConstraintName("fk_products_warehouses1");

                    b.Navigation("ProductDetailsProductDetail");

                    b.Navigation("SubcategoriesSubcategory");

                    b.Navigation("WarehousesWarehouse");
                });

            modelBuilder.Entity("SmartWMS.Models.ProductsHasTask", b =>
                {
                    b.HasOne("SmartWMS.Models.Product", "ProductsProduct")
                        .WithMany("ProductsHasTasks")
                        .HasForeignKey("ProductsProductId")
                        .IsRequired()
                        .HasConstraintName("fk_products_has_tasks_products1");

                    b.HasOne("SmartWMS.Models.Task", "TasksTask")
                        .WithMany("ProductsHasTasks")
                        .HasForeignKey("TasksTaskId")
                        .IsRequired()
                        .HasConstraintName("fk_products_has_tasks_tasks1");

                    b.Navigation("ProductsProduct");

                    b.Navigation("TasksTask");
                });

            modelBuilder.Entity("SmartWMS.Models.Report", b =>
                {
                    b.HasOne("SmartWMS.Models.Warehouse", "WarehousesWarehouse")
                        .WithMany("Reports")
                        .HasForeignKey("WarehousesWarehouseId")
                        .IsRequired()
                        .HasConstraintName("fk_reports_warehouses1");

                    b.Navigation("WarehousesWarehouse");
                });

            modelBuilder.Entity("SmartWMS.Models.Shelf", b =>
                {
                    b.HasOne("SmartWMS.Models.Product", "ProductsProduct")
                        .WithMany("Shelves")
                        .HasForeignKey("ProductsProductId")
                        .HasConstraintName("fk_shelf_products1");

                    b.Navigation("ProductsProduct");
                });

            modelBuilder.Entity("SmartWMS.Models.Subcategory", b =>
                {
                    b.HasOne("SmartWMS.Models.Category", "CategoriesCategory")
                        .WithMany("Subcategories")
                        .HasForeignKey("CategoriesCategoryId")
                        .IsRequired()
                        .HasConstraintName("fk_subcategories_categories1");

                    b.Navigation("CategoriesCategory");
                });

            modelBuilder.Entity("SmartWMS.Models.Task", b =>
                {
                    b.HasOne("SmartWMS.Models.OrderHeader", "OrderHeadersOrdersHeader")
                        .WithMany("Tasks")
                        .HasForeignKey("OrderHeadersOrdersHeaderId")
                        .IsRequired()
                        .HasConstraintName("fk_tasks_order_headers1");

                    b.Navigation("OrderHeadersOrdersHeader");
                });

            modelBuilder.Entity("SmartWMS.Models.User", b =>
                {
                    b.HasOne("SmartWMS.Models.User", "Manager")
                        .WithMany("InverseManager")
                        .HasForeignKey("ManagerId")
                        .HasConstraintName("fk_users_users1");

                    b.HasOne("SmartWMS.Models.Warehouse", "WarehousesWarehouse")
                        .WithMany("Users")
                        .HasForeignKey("WarehousesWarehouseId")
                        .IsRequired()
                        .HasConstraintName("fk_users_warehouses1");

                    b.Navigation("Manager");

                    b.Navigation("WarehousesWarehouse");
                });

            modelBuilder.Entity("SmartWMS.Models.UsersHasTask", b =>
                {
                    b.HasOne("SmartWMS.Models.Task", "TasksTask")
                        .WithMany("UsersHasTasks")
                        .HasForeignKey("TasksTaskId")
                        .IsRequired()
                        .HasConstraintName("fk_users_has_tasks_tasks1");

                    b.HasOne("SmartWMS.Models.User", "UsersUser")
                        .WithMany("UsersHasTasks")
                        .HasForeignKey("UsersUserId")
                        .IsRequired()
                        .HasConstraintName("fk_users_has_tasks_users1");

                    b.Navigation("TasksTask");

                    b.Navigation("UsersUser");
                });

            modelBuilder.Entity("SmartWMS.Models.Waybill", b =>
                {
                    b.HasOne("SmartWMS.Models.Country", "CountriesCountry")
                        .WithMany("Waybills")
                        .HasForeignKey("CountriesCountryId")
                        .IsRequired()
                        .HasConstraintName("fk_waybills_countries1");

                    b.Navigation("CountriesCountry");
                });

            modelBuilder.Entity("SmartWMS.Models.Category", b =>
                {
                    b.Navigation("Subcategories");
                });

            modelBuilder.Entity("SmartWMS.Models.Country", b =>
                {
                    b.Navigation("Waybills");
                });

            modelBuilder.Entity("SmartWMS.Models.OrderHeader", b =>
                {
                    b.Navigation("OrderDetails");

                    b.Navigation("Tasks");
                });

            modelBuilder.Entity("SmartWMS.Models.Product", b =>
                {
                    b.Navigation("OrderDetails");

                    b.Navigation("ProductsHasTasks");

                    b.Navigation("Shelves");
                });

            modelBuilder.Entity("SmartWMS.Models.ProductDetail", b =>
                {
                    b.Navigation("Products");
                });

            modelBuilder.Entity("SmartWMS.Models.Subcategory", b =>
                {
                    b.Navigation("Products");
                });

            modelBuilder.Entity("SmartWMS.Models.Task", b =>
                {
                    b.Navigation("ProductsHasTasks");

                    b.Navigation("UsersHasTasks");
                });

            modelBuilder.Entity("SmartWMS.Models.User", b =>
                {
                    b.Navigation("InverseManager");

                    b.Navigation("UsersHasTasks");
                });

            modelBuilder.Entity("SmartWMS.Models.Warehouse", b =>
                {
                    b.Navigation("Alerts");

                    b.Navigation("Products");

                    b.Navigation("Reports");

                    b.Navigation("Users");
                });

            modelBuilder.Entity("SmartWMS.Models.Waybill", b =>
                {
                    b.Navigation("OrderHeaders");
                });
#pragma warning restore 612, 618
        }
    }
}
