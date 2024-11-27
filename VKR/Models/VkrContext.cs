using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace VKR.Models;

public partial class VkrContext : DbContext
{
    public VkrContext()
    {
    }

    public VkrContext(DbContextOptions<VkrContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Deliveryinfo> Deliveryinfos { get; set; }

    public virtual DbSet<Dish> Dishes { get; set; }

    public virtual DbSet<Dishcategory> Dishcategories { get; set; }

    public virtual DbSet<Dishreview> Dishreviews { get; set; }

    public virtual DbSet<Ingredient> Ingredients { get; set; }

    public virtual DbSet<Ingredientamount> Ingredientamounts { get; set; }

    public virtual DbSet<Loyaltysystem> Loyaltysystems { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<Orderitem> Orderitems { get; set; }

    public virtual DbSet<Payment> Payments { get; set; }

    public virtual DbSet<Paymenttype> Paymenttypes { get; set; }

    public virtual DbSet<Reservation> Reservations { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql("Server=localhost;Database=vkr;Username=postgres;Password=2003;Persist Security Info=True")
        .LogTo(Console.WriteLine, LogLevel.Information);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        /*foreach (var entity in modelBuilder.Model.GetEntityTypes())
        {
            // Настраиваем имя таблицы
            entity.SetTableName(entity.GetTableName().ToLowerInvariant());

            // Настраиваем имена колонок
            foreach (var property in entity.GetProperties())
            {
                property.SetColumnName(property.Name.ToLowerInvariant());
            }
        }*/
        modelBuilder.HasDefaultSchema("restaurant");
        modelBuilder.Entity<Deliveryinfo>(entity =>
        {
            entity.HasKey(e => e.Deliveryinfoid).HasName("deliveryinfo_pkey");

            entity.ToTable("deliveryinfo", "restaurant");

            entity.Property(e => e.Deliveryinfoid).HasColumnName("deliveryinfoid");
            entity.Property(e => e.Courierid).HasColumnName("courierid");
            entity.Property(e => e.Deliverycost)
                .HasPrecision(10, 2)
                .HasColumnName("deliverycost");
            entity.Property(e => e.Deliverystatus)
                .HasMaxLength(255)
                .HasColumnName("deliverystatus");
            entity.Property(e => e.Estimateddeliverytime)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("estimateddeliverytime");
            entity.Property(e => e.Orderid).HasColumnName("orderid");

            entity.HasOne(d => d.Order).WithMany(p => p.Deliveryinfos)
                .HasForeignKey(d => d.Orderid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_deliveryinfo_order");
        });

        modelBuilder.Entity<Dish>(entity =>
        {
            entity.ToTable("dishes", "restaurant");
            entity.HasKey(e => e.Dishid);
            entity.Property(e => e.Dishid).ValueGeneratedOnAdd();
            entity.HasOne(d => d.Dishcategory)
                  .WithMany(c => c.Dishes)
                  .HasForeignKey(d => d.Dishcategoryid)
                  .OnDelete(DeleteBehavior.Cascade); // или другое поведение при удалении
        });


        modelBuilder.Entity<Dishcategory>(entity =>
        {
            entity.ToTable("dishcategories", "restaurant");
            entity.HasKey(e => e.Dishcategoryid);
        });

        modelBuilder.Entity<Dishreview>(entity =>
        {
            entity.HasKey(e => e.Reviewid).HasName("dishreviews_pkey");

            entity.ToTable("dishreviews", "restaurant");

            entity.Property(e => e.Reviewid).HasColumnName("reviewid");
            entity.Property(e => e.Comment).HasColumnName("comment");
            entity.Property(e => e.Dishid).HasColumnName("dishid");
            entity.Property(e => e.Rating).HasColumnName("rating");
            entity.Property(e => e.Userid).HasColumnName("userid");

            entity.HasOne(d => d.Dish).WithMany(p => p.Dishreviews)
                .HasForeignKey(d => d.Dishid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_dishreviews_dish");

            entity.HasOne(d => d.User).WithMany(p => p.Dishreviews)
                .HasForeignKey(d => d.Userid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_dishreviews_user");
        });

        modelBuilder.Entity<Ingredient>(entity =>
        {
            entity.HasKey(e => e.Ingredientid).HasName("ingredients_pkey");

            entity.ToTable("ingredients", "restaurant");

            entity.Property(e => e.Ingredientid).HasColumnName("ingredientid");
            entity.Property(e => e.Ingredientname)
                .HasMaxLength(255)
                .HasColumnName("ingredientname");
            entity.Property(e => e.Quantity).HasColumnName("quantity");
        });

        modelBuilder.Entity<Ingredientamount>(entity =>
        {
            entity.HasKey(e => new { e.Dishid, e.Ingredientid }).HasName("ingredientamounts_pkey");

            entity.ToTable("ingredientamounts", "restaurant");

            entity.Property(e => e.Dishid).HasColumnName("dishid");
            entity.Property(e => e.Ingredientid).HasColumnName("ingredientid");
            entity.Property(e => e.Amount).HasColumnName("amount");

            entity.HasOne(d => d.Dish).WithMany(p => p.Ingredientamounts)
                .HasForeignKey(d => d.Dishid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_ingredientamounts_dish");

            entity.HasOne(d => d.Ingredient).WithMany(p => p.Ingredientamounts)
                .HasForeignKey(d => d.Ingredientid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_ingredientamounts_ingredient");
        });

        modelBuilder.Entity<Loyaltysystem>(entity =>
        {
            entity.HasKey(e => e.Loyaltyid).HasName("loyaltysystem_pkey");

            entity.ToTable("loyaltysystem", "restaurant");

            entity.Property(e => e.Loyaltyid).HasColumnName("loyaltyid");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.Points)
                .HasDefaultValue(0)
                .HasColumnName("points");
            entity.Property(e => e.Userid).HasColumnName("userid");
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.Orderid).HasName("orders_pkey");

            entity.ToTable("orders", "restaurant");

            entity.Property(e => e.Orderid).HasColumnName("orderid");
            entity.Property(e => e.Orderdatetime)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("orderdatetime");
            entity.Property(e => e.Orderstatus)
                .HasMaxLength(255)
                .HasColumnName("orderstatus");
            entity.Property(e => e.Totalamount)
                .HasPrecision(10, 2)
                .HasColumnName("totalamount");
            entity.Property(e => e.Userid).HasColumnName("userid");
        });

        modelBuilder.Entity<Orderitem>(entity =>
        {
            entity.HasKey(e => e.Orderitemid).HasName("orderitems_pkey");

            entity.ToTable("orderitems", "restaurant");

            entity.Property(e => e.Orderitemid).HasColumnName("orderitemid");
            entity.Property(e => e.Dishid).HasColumnName("dishid");
            entity.Property(e => e.Dishstatus)
                .HasMaxLength(255)
                .HasColumnName("dishstatus");
            entity.Property(e => e.Orderid).HasColumnName("orderid");
            entity.Property(e => e.Quantity).HasColumnName("quantity");

            entity.HasOne(d => d.Dish).WithMany(p => p.Orderitems)
                .HasForeignKey(d => d.Dishid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_orderitems_dish");

            entity.HasOne(d => d.Order).WithMany(p => p.Orderitems)
                .HasForeignKey(d => d.Orderid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_orderitems_order");
        });

        modelBuilder.Entity<Payment>(entity =>
        {
            entity.HasKey(e => e.Paymentid).HasName("payments_pkey");

            entity.ToTable("payments", "restaurant");

            entity.Property(e => e.Paymentid).HasColumnName("paymentid");
            entity.Property(e => e.Orderid).HasColumnName("orderid");
            entity.Property(e => e.Paymentamount)
                .HasPrecision(10, 2)
                .HasColumnName("paymentamount");
            entity.Property(e => e.Paymentdatetime)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("paymentdatetime");
            entity.Property(e => e.Paymentstatus)
                .HasMaxLength(255)
                .HasColumnName("paymentstatus");
            entity.Property(e => e.Paymenttypeid).HasColumnName("paymenttypeid");

            entity.HasOne(d => d.Order).WithMany(p => p.Payments)
                .HasForeignKey(d => d.Orderid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_payment_order");

            entity.HasOne(d => d.Paymenttype).WithMany(p => p.Payments)
                .HasForeignKey(d => d.Paymenttypeid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_payment_paymenttype");
        });

        modelBuilder.Entity<Paymenttype>(entity =>
        {
            entity.HasKey(e => e.Paymenttypeid).HasName("paymenttypes_pkey");

            entity.ToTable("paymenttypes", "restaurant");

            entity.Property(e => e.Paymenttypeid).HasColumnName("paymenttypeid");
            entity.Property(e => e.Paymenttypename)
                .HasMaxLength(255)
                .HasColumnName("paymenttypename");
        });

        modelBuilder.Entity<Reservation>(entity =>
        {
            entity.HasKey(e => e.Reservationid).HasName("reservations_pkey");

            entity.ToTable("reservations", "restaurant");

            entity.Property(e => e.Reservationid).HasColumnName("reservationid");
            entity.Property(e => e.Guestcount).HasColumnName("guestcount");
            entity.Property(e => e.Reservationdatetime)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("reservationdatetime");
            entity.Property(e => e.Tablenumber).HasColumnName("tablenumber");
            entity.Property(e => e.Userid).HasColumnName("userid");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Userid).HasName("users_pkey");

            entity.ToTable("users", "restaurant");

            entity.HasIndex(e => e.Email, "users_email_key").IsUnique();

            entity.Property(e => e.Userid).HasColumnName("userid");
            entity.Property(e => e.Address).HasColumnName("address");
            entity.Property(e => e.Email)
                .HasMaxLength(255)
                .HasColumnName("email");
            entity.Property(e => e.Fullname)
                .HasMaxLength(255)
                .HasColumnName("fullname");
            entity.Property(e => e.Loyaltypoints)
                .HasDefaultValue(0)
                .HasColumnName("loyaltypoints");
            entity.Property(e => e.Passwordhash)
                .HasMaxLength(255)
                .HasColumnName("passwordhash");
            entity.Property(e => e.Phonenumber)
                .HasMaxLength(255)
                .HasColumnName("phonenumber");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
