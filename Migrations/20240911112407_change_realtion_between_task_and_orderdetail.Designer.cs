﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using SmartWMS.Entities;
using SmartWMS.Entities.Enums;

#nullable disable

namespace SmartWMS.Migrations
{
    [DbContext(typeof(SmartwmsDbContext))]
    [Migration("20240911112407_change_realtion_between_task_and_orderdetail")]
    partial class change_realtion_between_task_and_orderdetail
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

            modelBuilder.Entity("SmartWMS.Entities.Alert", b =>
                {
                    b.Property<int>("AlertId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("alert_id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("AlertId"));

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

            modelBuilder.Entity("SmartWMS.Entities.Category", b =>
                {
                    b.Property<int>("CategoryId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("category_id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("CategoryId"));

                    b.Property<string>("CategoryName")
                        .IsRequired()
                        .HasMaxLength(45)
                        .HasColumnType("character varying(45)")
                        .HasColumnName("category_name");

                    b.HasKey("CategoryId")
                        .HasName("categories_id_unique");

                    b.ToTable("categories", (string)null);
                });

            modelBuilder.Entity("SmartWMS.Entities.Country", b =>
                {
                    b.Property<int>("CountryId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("country_id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("CountryId"));

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

            modelBuilder.Entity("SmartWMS.Entities.OrderDetail", b =>
                {
                    b.Property<int>("OrderDetailId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("order_detail_id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("OrderDetailId"));

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

            modelBuilder.Entity("SmartWMS.Entities.OrderHeader", b =>
                {
                    b.Property<int>("OrdersHeaderId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("orders_header_id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("OrdersHeaderId"));

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

                    b.HasKey("OrdersHeaderId")
                        .HasName("orders_id_unique");

                    b.ToTable("order_headers", (string)null);
                });

            modelBuilder.Entity("SmartWMS.Entities.Product", b =>
                {
                    b.Property<int>("ProductId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("product_id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("ProductId"));

                    b.Property<string>("Barcode")
                        .IsRequired()
                        .HasMaxLength(8)
                        .HasColumnType("character varying(8)")
                        .HasColumnName("barcode");

                    b.Property<decimal>("Price")
                        .HasColumnType("money")
                        .HasColumnName("price");

                    b.Property<string>("ProductDescription")
                        .HasMaxLength(45)
                        .HasColumnType("character varying(45)")
                        .HasColumnName("product_description");

                    b.Property<string>("ProductName")
                        .IsRequired()
                        .HasMaxLength(45)
                        .HasColumnType("character varying(45)")
                        .HasColumnName("product_name");

                    b.Property<int>("Quantity")
                        .HasColumnType("integer")
                        .HasColumnName("quantity");

                    b.Property<int>("SubcategoriesSubcategoryId")
                        .HasColumnType("integer")
                        .HasColumnName("subcategories_subcategory_id");

                    b.Property<int>("WarehousesWarehouseId")
                        .HasColumnType("integer")
                        .HasColumnName("warehouses_warehouse_id");

                    b.HasKey("ProductId")
                        .HasName("idprodukty_unique");

                    b.HasIndex("SubcategoriesSubcategoryId");

                    b.HasIndex("WarehousesWarehouseId");

                    b.ToTable("products", (string)null);
                });

            modelBuilder.Entity("SmartWMS.Entities.Report", b =>
                {
                    b.Property<int>("ReportId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("report_id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("ReportId"));

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

            modelBuilder.Entity("SmartWMS.Entities.Shelf", b =>
                {
                    b.Property<int>("ShelfId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("shelf_id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("ShelfId"));

                    b.Property<int>("CurrentQuant")
                        .HasColumnType("integer")
                        .HasColumnName("current_quant");

                    b.Property<string>("Lane")
                        .IsRequired()
                        .HasMaxLength(3)
                        .HasColumnType("character varying(3)")
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

                    b.HasKey("ShelfId")
                        .HasName("shelf_id_unique");

                    b.HasIndex("ProductsProductId");

                    b.ToTable("shelf", (string)null);
                });

            modelBuilder.Entity("SmartWMS.Entities.Subcategory", b =>
                {
                    b.Property<int>("SubcategoryId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("subcategory_id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("SubcategoryId"));

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

            modelBuilder.Entity("SmartWMS.Entities.Task", b =>
                {
                    b.Property<int>("TaskId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("task_id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("TaskId"));

                    b.Property<DateTime?>("FinishDate")
                        .HasColumnType("timestamp without time zone")
                        .HasColumnName("finish_date");

                    b.Property<int>("OrderDetailsOrderDetailId")
                        .HasColumnType("integer")
                        .HasColumnName("orderDetails_orderDetail_id");

                    b.Property<int>("Priority")
                        .HasColumnType("integer")
                        .HasColumnName("priority");

                    b.Property<int>("QuantityAllocated")
                        .HasColumnType("integer")
                        .HasColumnName("quantity_allocated");

                    b.Property<int>("QuantityCollected")
                        .HasColumnType("integer")
                        .HasColumnName("quantity_collected");

                    b.Property<bool?>("Seen")
                        .HasColumnType("boolean")
                        .HasColumnName("seen");

                    b.Property<DateTime?>("StartDate")
                        .HasColumnType("timestamp without time zone")
                        .HasColumnName("start_date");

                    b.HasKey("TaskId")
                        .HasName("task_id_unique");

                    b.HasIndex("OrderDetailsOrderDetailId");

                    b.ToTable("tasks", (string)null);
                });

            modelBuilder.Entity("SmartWMS.Entities.User", b =>
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

            modelBuilder.Entity("SmartWMS.Entities.UsersHasTask", b =>
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

            modelBuilder.Entity("SmartWMS.Entities.Warehouse", b =>
                {
                    b.Property<int>("WarehouseId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("warehouse_id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("WarehouseId"));

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasMaxLength(45)
                        .HasColumnType("character varying(45)")
                        .HasColumnName("address");

                    b.HasKey("WarehouseId")
                        .HasName("warehouses_pkey");

                    b.ToTable("warehouses", (string)null);
                });

            modelBuilder.Entity("SmartWMS.Entities.Waybill", b =>
                {
                    b.Property<int>("WaybillId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("waybill_id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("WaybillId"));

                    b.Property<int>("CountriesCountryId")
                        .HasColumnType("integer")
                        .HasColumnName("countries_country_id");

                    b.Property<DateTime>("LoadingDate")
                        .HasColumnType("timestamp without time zone")
                        .HasColumnName("loading_date");

                    b.Property<int>("OrderHeadersOrderHeaderId")
                        .HasColumnType("integer");

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

                    b.HasIndex("OrderHeadersOrderHeaderId")
                        .IsUnique();

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
                    b.HasOne("SmartWMS.Entities.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("SmartWMS.Entities.User", null)
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

                    b.HasOne("SmartWMS.Entities.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("SmartWMS.Entities.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("SmartWMS.Entities.Alert", b =>
                {
                    b.HasOne("SmartWMS.Entities.Warehouse", "WarehousesWarehouse")
                        .WithMany("Alerts")
                        .HasForeignKey("WarehousesWarehouseId")
                        .IsRequired()
                        .HasConstraintName("fk_alerts_warehouses1");

                    b.Navigation("WarehousesWarehouse");
                });

            modelBuilder.Entity("SmartWMS.Entities.OrderDetail", b =>
                {
                    b.HasOne("SmartWMS.Entities.OrderHeader", "OrderHeadersOrdersHeader")
                        .WithMany("OrderDetails")
                        .HasForeignKey("OrderHeadersOrdersHeaderId")
                        .IsRequired()
                        .HasConstraintName("fk_order_details_order_headers1");

                    b.HasOne("SmartWMS.Entities.Product", "ProductsProduct")
                        .WithMany("OrderDetails")
                        .HasForeignKey("ProductsProductId")
                        .IsRequired()
                        .HasConstraintName("fk_order_details_products1");

                    b.Navigation("OrderHeadersOrdersHeader");

                    b.Navigation("ProductsProduct");
                });

            modelBuilder.Entity("SmartWMS.Entities.Product", b =>
                {
                    b.HasOne("SmartWMS.Entities.Subcategory", "SubcategoriesSubcategory")
                        .WithMany("Products")
                        .HasForeignKey("SubcategoriesSubcategoryId")
                        .IsRequired()
                        .HasConstraintName("fk_products_subcategories1");

                    b.HasOne("SmartWMS.Entities.Warehouse", "WarehousesWarehouse")
                        .WithMany("Products")
                        .HasForeignKey("WarehousesWarehouseId")
                        .IsRequired()
                        .HasConstraintName("fk_products_warehouses1");

                    b.Navigation("SubcategoriesSubcategory");

                    b.Navigation("WarehousesWarehouse");
                });

            modelBuilder.Entity("SmartWMS.Entities.Report", b =>
                {
                    b.HasOne("SmartWMS.Entities.Warehouse", "WarehousesWarehouse")
                        .WithMany("Reports")
                        .HasForeignKey("WarehousesWarehouseId")
                        .IsRequired()
                        .HasConstraintName("fk_reports_warehouses1");

                    b.Navigation("WarehousesWarehouse");
                });

            modelBuilder.Entity("SmartWMS.Entities.Shelf", b =>
                {
                    b.HasOne("SmartWMS.Entities.Product", "ProductsProduct")
                        .WithMany("Shelves")
                        .HasForeignKey("ProductsProductId")
                        .HasConstraintName("fk_shelf_products1");

                    b.Navigation("ProductsProduct");
                });

            modelBuilder.Entity("SmartWMS.Entities.Subcategory", b =>
                {
                    b.HasOne("SmartWMS.Entities.Category", "CategoriesCategory")
                        .WithMany("Subcategories")
                        .HasForeignKey("CategoriesCategoryId")
                        .IsRequired()
                        .HasConstraintName("fk_subcategories_categories1");

                    b.Navigation("CategoriesCategory");
                });

            modelBuilder.Entity("SmartWMS.Entities.Task", b =>
                {
                    b.HasOne("SmartWMS.Entities.OrderDetail", "OrderDetailsOrderDetail")
                        .WithMany("TasksTask")
                        .HasForeignKey("OrderDetailsOrderDetailId")
                        .OnDelete(DeleteBehavior.SetNull)
                        .IsRequired()
                        .HasConstraintName("fk_order_details_tasks1");

                    b.Navigation("OrderDetailsOrderDetail");
                });

            modelBuilder.Entity("SmartWMS.Entities.User", b =>
                {
                    b.HasOne("SmartWMS.Entities.User", "Manager")
                        .WithMany("InverseManager")
                        .HasForeignKey("ManagerId")
                        .HasConstraintName("fk_users_users1");

                    b.HasOne("SmartWMS.Entities.Warehouse", "WarehousesWarehouse")
                        .WithMany("Users")
                        .HasForeignKey("WarehousesWarehouseId")
                        .IsRequired()
                        .HasConstraintName("fk_users_warehouses1");

                    b.Navigation("Manager");

                    b.Navigation("WarehousesWarehouse");
                });

            modelBuilder.Entity("SmartWMS.Entities.UsersHasTask", b =>
                {
                    b.HasOne("SmartWMS.Entities.Task", "TasksTask")
                        .WithMany("UsersHasTasks")
                        .HasForeignKey("TasksTaskId")
                        .IsRequired()
                        .HasConstraintName("fk_users_has_tasks_tasks1");

                    b.HasOne("SmartWMS.Entities.User", "UsersUser")
                        .WithMany("UsersHasTasks")
                        .HasForeignKey("UsersUserId")
                        .IsRequired()
                        .HasConstraintName("fk_users_has_tasks_users1");

                    b.Navigation("TasksTask");

                    b.Navigation("UsersUser");
                });

            modelBuilder.Entity("SmartWMS.Entities.Waybill", b =>
                {
                    b.HasOne("SmartWMS.Entities.Country", "CountriesCountry")
                        .WithMany("Waybills")
                        .HasForeignKey("CountriesCountryId")
                        .IsRequired()
                        .HasConstraintName("fk_waybills_countries1");

                    b.HasOne("SmartWMS.Entities.OrderHeader", "OrderHeadersOrderHeader")
                        .WithOne("WaybillsWaybill")
                        .HasForeignKey("SmartWMS.Entities.Waybill", "OrderHeadersOrderHeaderId")
                        .IsRequired()
                        .HasConstraintName("fk_order_headers_waybills1");

                    b.Navigation("CountriesCountry");

                    b.Navigation("OrderHeadersOrderHeader");
                });

            modelBuilder.Entity("SmartWMS.Entities.Category", b =>
                {
                    b.Navigation("Subcategories");
                });

            modelBuilder.Entity("SmartWMS.Entities.Country", b =>
                {
                    b.Navigation("Waybills");
                });

            modelBuilder.Entity("SmartWMS.Entities.OrderDetail", b =>
                {
                    b.Navigation("TasksTask");
                });

            modelBuilder.Entity("SmartWMS.Entities.OrderHeader", b =>
                {
                    b.Navigation("OrderDetails");

                    b.Navigation("WaybillsWaybill")
                        .IsRequired();
                });

            modelBuilder.Entity("SmartWMS.Entities.Product", b =>
                {
                    b.Navigation("OrderDetails");

                    b.Navigation("Shelves");
                });

            modelBuilder.Entity("SmartWMS.Entities.Subcategory", b =>
                {
                    b.Navigation("Products");
                });

            modelBuilder.Entity("SmartWMS.Entities.Task", b =>
                {
                    b.Navigation("UsersHasTasks");
                });

            modelBuilder.Entity("SmartWMS.Entities.User", b =>
                {
                    b.Navigation("InverseManager");

                    b.Navigation("UsersHasTasks");
                });

            modelBuilder.Entity("SmartWMS.Entities.Warehouse", b =>
                {
                    b.Navigation("Alerts");

                    b.Navigation("Products");

                    b.Navigation("Reports");

                    b.Navigation("Users");
                });
#pragma warning restore 612, 618
        }
    }
}
