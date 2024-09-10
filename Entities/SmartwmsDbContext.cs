using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SmartWMS.Entities.Enums;
using SmartWMS.Models;
using Task = SmartWMS.Entities.Task;

namespace SmartWMS.Entities;

public partial class SmartwmsDbContext : IdentityDbContext<User>
{
    public SmartwmsDbContext(DbContextOptions<SmartwmsDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Alert> Alerts { get; set; }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<Country> Countries { get; set; }

    public virtual DbSet<OrderDetail> OrderDetails { get; set; }

    public virtual DbSet<OrderHeader> OrderHeaders { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<ProductDetail> ProductDetails { get; set; }

    //public virtual DbSet<ProductsHasTask> ProductsHasTasks { get; set; }
    
    public virtual DbSet<UsersHasTask> UsersHasTasks { get; set; }

    public virtual DbSet<Report> Reports { get; set; }

    public virtual DbSet<Shelf> Shelves { get; set; }

    public virtual DbSet<Subcategory> Subcategories { get; set; }

    public virtual DbSet<Task> Tasks { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<Warehouse> Warehouses { get; set; }

    public virtual DbSet<Waybill> Waybills { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        modelBuilder
            .HasPostgresEnum<ActionType>() //Done
            .HasPostgresEnum<AlertType>() //Done
            .HasPostgresEnum<LevelType>() //Done
            .HasPostgresEnum<OrderName>() //Done
            .HasPostgresEnum<OrderType>() //Done
            .HasPostgresEnum<ReportPeriod>() //Done
            .HasPostgresEnum<ReportType>(); //Done

        modelBuilder.Entity<Alert>(entity =>
        {
            entity.HasKey(e => e.AlertId).HasName("alerts_id_unique");

            entity.ToTable("alerts");

            entity.Property(e => e.AlertId)
                //.ValueGeneratedNever()
                .HasColumnName("alert_id");
            entity.Property(e => e.AlertDate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("alert_date");
            entity.Property(e => e.Seen).HasColumnName("seen");
            entity.Property(e => e.WarehousesWarehouseId).HasColumnName("warehouses_warehouse_id");

            entity.HasOne(d => d.WarehousesWarehouse).WithMany(p => p.Alerts)
                .HasForeignKey(d => d.WarehousesWarehouseId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_alerts_warehouses1");

            entity.Property(e => e.AlertType)
                .HasColumnName("alert_type")
                .HasColumnType("alert_type");
        });

        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.CategoryId).HasName("categories_id_unique");

            entity.ToTable("categories");

            entity.Property(e => e.CategoryId)
                //.ValueGeneratedNever()
                .HasColumnName("category_id");
            entity.Property(e => e.CategoryName)
                .HasMaxLength(45)
                .HasColumnName("category_name");
        });

        modelBuilder.Entity<Country>(entity =>
        {
            entity.HasKey(e => e.CountryId).HasName("country_id_unique");

            entity.ToTable("countries");

            entity.HasIndex(e => e.CountryCode, "countrycode_unique").IsUnique();

            entity.HasIndex(e => e.CountryName, "countryname_unique").IsUnique();

            entity.Property(e => e.CountryId)
                //.ValueGeneratedNever()
                .HasColumnName("country_id");
            entity.Property(e => e.CountryCode).HasColumnName("country_code");
            entity.Property(e => e.CountryName)
                .HasMaxLength(45)
                .HasColumnName("country_name");
        });

        modelBuilder.Entity<OrderDetail>(entity =>
        {
            entity.HasKey(e => e.OrderDetailId).HasName("order_detail_id_unique");

            entity.ToTable("order_details");

            entity.Property(e => e.OrderDetailId)
                //.ValueGeneratedNever()
                .HasColumnName("order_detail_id");
            entity.Property(e => e.OrderHeadersOrdersHeaderId).HasColumnName("order_headers_orders_header_id");
            entity.Property(e => e.ProductsProductId).HasColumnName("products_product_id");
            entity.Property(e => e.Quantity).HasColumnName("quantity");

            entity.HasOne(d => d.OrderHeadersOrdersHeader).WithMany(p => p.OrderDetails)
                .HasForeignKey(d => d.OrderHeadersOrdersHeaderId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_order_details_order_headers1");

            entity.HasOne(d => d.ProductsProduct).WithMany(p => p.OrderDetails)
                .HasForeignKey(d => d.ProductsProductId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_order_details_products1");

            entity.HasOne<Task>(d => d.TasksTask).WithOne(p => p.OrderDetailsOrderDetail)
                .HasForeignKey<Task>(d => d.OrderDetailsOrderDetailId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("fk_order_details_tasks1");
        });

        modelBuilder.Entity<OrderHeader>(entity =>
        {
            entity.HasKey(e => e.OrdersHeaderId).HasName("orders_id_unique");

            entity.ToTable("order_headers");

            entity.Property(e => e.OrdersHeaderId)
                //.ValueGeneratedNever()
                .HasColumnName("orders_header_id");
            entity.Property(e => e.DeliveryDate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("delivery_date");
            entity.Property(e => e.DestinationAddress)
                .HasMaxLength(45)
                .HasColumnName("destination_address");
            entity.Property(e => e.OrderDate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("order_date");
            entity.Property(e => e.TypeName)
                .HasColumnName("type_name")
                .HasColumnType("order_type");
            entity.Property(e => e.StatusName)
                .HasColumnName("status_name")
                .HasColumnType("order_name");
            entity.HasOne<Waybill>(d => d.WaybillsWaybill).WithOne(p => p.OrderHeadersOrderHeader)
                .HasForeignKey<Waybill>(d => d.OrderHeadersOrderHeaderId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_order_headers_waybills1");
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.ProductId).HasName("idprodukty_unique");

            entity.ToTable("products");

            entity.Property(e => e.ProductId)
                //.ValueGeneratedNever()
                .HasColumnName("product_id");
            entity.Property(e => e.Price)
                .HasMaxLength(45)
                .HasColumnName("price");
            entity.Property(e => e.ProductDescription)
                .HasMaxLength(45)
                .HasColumnName("product_description");
            entity.Property(e => e.ProductDetailsProductDetailId).HasColumnName("product_details_product_detail_id");
            entity.Property(e => e.ProductName)
                .HasMaxLength(45)
                .HasColumnName("product_name");
            entity.Property(e => e.SubcategoriesSubcategoryId).HasColumnName("subcategories_subcategory_id");
            entity.Property(e => e.WarehousesWarehouseId).HasColumnName("warehouses_warehouse_id");

            entity.HasOne(d => d.SubcategoriesSubcategory).WithMany(p => p.Products)
                .HasForeignKey(d => d.SubcategoriesSubcategoryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_products_subcategories1");

            entity.HasOne(d => d.WarehousesWarehouse).WithMany(p => p.Products)
                .HasForeignKey(d => d.WarehousesWarehouseId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_products_warehouses1");
        });
        
        modelBuilder.Entity<ProductDetail>(entity =>
        {
            entity.HasKey(e => e.ProductDetailId).HasName("warehouse_state_id_unique");

            entity.ToTable("product_details");

            entity.Property(e => e.ProductDetailId)
                //.ValueGeneratedNever()
                .HasColumnName("product_detail_id");
            entity.Property(e => e.Barcode)
                .HasMaxLength(8)
                .HasColumnName("barcode");
            entity.Property(e => e.Quantity).HasColumnName("quantity");
            entity.HasOne<Product>(d => d.ProductsProduct).WithOne(p => p.ProductDetailsProductDetail)
                .HasForeignKey<Product>(d => d.ProductDetailsProductDetailId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_products_product_details1");
        });
        /*
        modelBuilder.Entity<ProductsHasTask>(entity =>
        {
            entity.HasKey(e => new { e.ProductsProductId, e.TasksTaskId }).HasName("products_has_tasks_pkey");

            entity.ToTable("products_has_tasks");

            entity.Property(e => e.ProductsProductId).HasColumnName("products_product_id");
            entity.Property(e => e.TasksTaskId).HasColumnName("tasks_task_id");
            entity.Property(e => e.QuantityAllocated).HasColumnName("quantity_allocated");
            entity.Property(e => e.QuantityCollected)
                .HasDefaultValue(0)
                .HasColumnName("quantity_collected");

            entity.HasOne(d => d.ProductsProduct).WithMany(p => p.ProductsHasTasks)
                .HasForeignKey(d => d.ProductsProductId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_products_has_tasks_products1");

            entity.HasOne(d => d.TasksTask).WithMany(p => p.ProductsHasTasks)
                .HasForeignKey(d => d.TasksTaskId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_products_has_tasks_tasks1");
        });
        */

        modelBuilder.Entity<UsersHasTask>(entity =>
        {
            entity.HasKey(e => new { e.UsersUserId, e.TasksTaskId }).HasName("users_has_tasks_pkey");

            entity.ToTable("users_has_tasks");

            entity.Property(e => e.UsersUserId).HasColumnName("users_user_id");
            entity.Property(e => e.TasksTaskId).HasColumnName("tasks_task_id");
            entity.Property(e => e.Action)
                .HasColumnName("action")
                .HasColumnType("action_type");

            entity.HasOne(d => d.UsersUser).WithMany(p => p.UsersHasTasks)
                .HasForeignKey(d => d.UsersUserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_users_has_tasks_users1");

            entity.HasOne(d => d.TasksTask).WithMany(p => p.UsersHasTasks)
                .HasForeignKey(d => d.TasksTaskId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_users_has_tasks_tasks1");

        });

        modelBuilder.Entity<Report>(entity =>
        {
            entity.HasKey(e => e.ReportId).HasName("report_id_unique");

            entity.ToTable("reports");

            entity.Property(e => e.ReportId)
                //.ValueGeneratedNever()
                .HasColumnName("report_id");
            entity.Property(e => e.ReportType)
                .HasColumnName("report_type")
                .HasColumnType("report_type");
            entity.Property(e => e.ReportPeriod)
                .HasColumnName("report_period")
                .HasColumnType("report_period");
            entity.Property(e => e.ReportDate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("report_date");
            entity.Property(e => e.ReportFile).HasColumnName("report_file");
            entity.Property(e => e.WarehousesWarehouseId).HasColumnName("warehouses_warehouse_id");

            entity.HasOne(d => d.WarehousesWarehouse).WithMany(p => p.Reports)
                .HasForeignKey(d => d.WarehousesWarehouseId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_reports_warehouses1");
        });

        modelBuilder.Entity<Shelf>(entity =>
        {
            entity.HasKey(e => e.ShelfId).HasName("shelf_id_unique");

            entity.ToTable("shelf");

            //entity.HasIndex(e => e.Lane, "lane_unique").IsUnique();

            entity.Property(e => e.ShelfId)
                //.ValueGeneratedNever()
                .HasColumnName("shelf_id");
            entity.Property(e => e.CurrentQuant).HasColumnName("current_quant");
            entity.Property(e => e.Lane)
                .HasMaxLength(3)
                .HasColumnName("lane");
            entity.Property(e => e.MaxQuant).HasColumnName("max_quant");
            entity.Property(e => e.ProductsProductId).HasColumnName("products_product_id");
            entity.Property(e => e.Rack).HasColumnName("rack");
            entity.Property(e => e.Level).HasColumnName("level")
                .HasColumnType("level_type");

            entity.HasOne(d => d.ProductsProduct).WithMany(p => p.Shelves)
                .HasForeignKey(d => d.ProductsProductId)
                .HasConstraintName("fk_shelf_products1");
        });

        modelBuilder.Entity<Subcategory>(entity =>
        {
            entity.HasKey(e => e.SubcategoryId).HasName("subcategories_id_unique");

            entity.ToTable("subcategories");

            entity.Property(e => e.SubcategoryId)
                //.ValueGeneratedNever()
                .HasColumnName("subcategory_id");
            entity.Property(e => e.CategoriesCategoryId).HasColumnName("categories_category_id");
            entity.Property(e => e.SubcategoryName)
                .HasMaxLength(45)
                .HasColumnName("subcategory_name");

            entity.HasOne(d => d.CategoriesCategory).WithMany(p => p.Subcategories)
                .HasForeignKey(d => d.CategoriesCategoryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_subcategories_categories1");
        });

        modelBuilder.Entity<Task>(entity =>
        {
            entity.HasKey(e => e.TaskId).HasName("task_id_unique");

            entity.ToTable("tasks");

            entity.Property(e => e.TaskId)
                //.ValueGeneratedNever()
                .HasColumnName("task_id");
            entity.Property(e => e.FinishDate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("finish_date");
            //entity.Property(e => e.OrderHeadersOrdersHeaderId).HasColumnName("order_headers_orders_header_id");
            entity.Property(e => e.Priority).HasColumnName("priority");
            entity.Property(e => e.Seen).HasColumnName("seen");
            entity.Property(e => e.StartDate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("start_date");
            entity.Property(e => e.QuantityCollected).HasColumnName("quantity_collected");
            entity.Property(e => e.QuantityAllocated).HasColumnName("quantity_allocated");
            entity.Property(e => e.OrderDetailsOrderDetailId).HasColumnName("orderDetails_orderDetail_id");
            /*
            entity.HasOne(d => d.OrderHeadersOrdersHeader).WithMany(p => p.Tasks)
                .HasForeignKey(d => d.OrderHeadersOrdersHeaderId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_tasks_order_headers1");
                */
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.ToTable("users");
            
            entity.Property(e => e.ManagerId).HasColumnName("manager_id");
            entity.Property(e => e.WarehousesWarehouseId).HasColumnName("warehouses_warehouse_id");

            entity.HasOne(d => d.Manager).WithMany(p => p.InverseManager)
                .HasForeignKey(d => d.ManagerId)
                .HasConstraintName("fk_users_users1");

            entity.HasOne(d => d.WarehousesWarehouse).WithMany(p => p.Users)
                .HasForeignKey(d => d.WarehousesWarehouseId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_users_warehouses1");
        });

        modelBuilder.Entity<Warehouse>(entity =>
        {
            entity.HasKey(e => e.WarehouseId).HasName("warehouses_pkey");

            entity.ToTable("warehouses");

            entity.Property(e => e.WarehouseId)
                //.ValueGeneratedNever()
                .HasColumnName("warehouse_id");
            entity.Property(e => e.Address)
                .HasMaxLength(45)
                .HasColumnName("address");
        });

        modelBuilder.Entity<Waybill>(entity =>
        {
            entity.HasKey(e => e.WaybillId).HasName("waybill_id_unique");

            entity.ToTable("waybills");

            entity.Property(e => e.WaybillId)
                //.ValueGeneratedNever()
                .HasColumnName("waybill_id");
            entity.Property(e => e.CountriesCountryId).HasColumnName("countries_country_id");
            entity.Property(e => e.LoadingDate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("loading_date");
            entity.Property(e => e.PostalCode)
                .HasMaxLength(45)
                .HasColumnName("postal_code");
            entity.Property(e => e.ShippingDate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("shipping_date");
            entity.Property(e => e.SupplierName)
                .HasMaxLength(45)
                .HasColumnName("supplier_name");

            entity.HasOne(d => d.CountriesCountry).WithMany(p => p.Waybills)
                .HasForeignKey(d => d.CountriesCountryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_waybills_countries1");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
