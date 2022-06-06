using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace WaveShopAPIRest.Models
{
    public partial class WaveShopContext : DbContext
    {
        public WaveShopContext()
        {
        }

        public WaveShopContext(DbContextOptions<WaveShopContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Address> Addresses { get; set; } = null!;
        public virtual DbSet<Category> Categories { get; set; } = null!;
        public virtual DbSet<Comment> Comments { get; set; } = null!;
        public virtual DbSet<Favorite> Favorites { get; set; } = null!;
        public virtual DbSet<Order> Orders { get; set; } = null!;
        public virtual DbSet<Product> Products { get; set; } = null!;
        public virtual DbSet<ProductSelectedCart> ProductSelectedCarts { get; set; } = null!;
        public virtual DbSet<ProductSelectedOrder> ProductSelectedOrders { get; set; } = null!;
        public virtual DbSet<Product_Image> Product_Images { get; set; } = null!;
        public virtual DbSet<ShoppingCart> ShoppingCarts { get; set; } = null!;
        public virtual DbSet<User> Users { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=localhost;Database=WaveShop;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Address>(entity =>
            {
                entity.ToTable("Address");

                entity.Property(e => e.State).HasMaxLength(100);

                entity.Property(e => e.Street).HasMaxLength(500);

                entity.Property(e => e.Zip).HasMaxLength(100);

                entity.Property(e => e.city).HasMaxLength(100);

                entity.HasOne(d => d.IdUserNavigation)
                    .WithMany(p => p.Addresses)
                    .HasForeignKey(d => d.IdUser)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_dbo.Address.Account");
            });

            modelBuilder.Entity<Category>(entity =>
            {
                entity.ToTable("Category");

                entity.HasIndex(e => e.Name, "PK_dbo_Category_Name")
                    .IsUnique();

                entity.Property(e => e.Description).HasMaxLength(500);

                entity.Property(e => e.Name).HasMaxLength(100);

                entity.Property(e => e.Status).HasMaxLength(500);
            });

            modelBuilder.Entity<Comment>(entity =>
            {
                entity.ToTable("Comment");

                entity.Property(e => e.Content).HasMaxLength(500);

                entity.Property(e => e.OpinionResume).HasMaxLength(100);

                entity.Property(e => e.PhotoAddress).HasMaxLength(100);

                entity.Property(e => e.Published).HasColumnType("datetime");

                entity.Property(e => e.UserName).HasMaxLength(100);

                entity.Property(e => e.Visible).HasMaxLength(1);

                entity.HasOne(d => d.IdProductNavigation)
                    .WithMany(p => p.Comments)
                    .HasForeignKey(d => d.IdProduct)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_dbo.Comment.Product");
            });

            modelBuilder.Entity<Favorite>(entity =>
            {
                entity.HasKey(e => new { e.IdUser, e.IdProduct })
                    .HasName("PK__Favorite__E521B255AEA66E1B");

                entity.ToTable("Favorite");

                entity.Property(e => e.Creation).HasColumnType("datetime");

                entity.HasOne(d => d.IdProductNavigation)
                    .WithMany(p => p.Favorites)
                    .HasForeignKey(d => d.IdProduct)
                    .HasConstraintName("FK_dbo.Favorite.Product");

                entity.HasOne(d => d.IdUserNavigation)
                    .WithMany(p => p.Favorites)
                    .HasForeignKey(d => d.IdUser)
                    .HasConstraintName("FK_dbo.Favorite.User");
            });

            modelBuilder.Entity<Order>(entity =>
            {
                entity.ToTable("Order");

                entity.Property(e => e.Ordered).HasColumnType("datetime");

                entity.Property(e => e.Shipped).HasColumnType("datetime");

                entity.Property(e => e.Status).HasMaxLength(100);

                entity.HasOne(d => d.IdUserNavigation)
                    .WithMany(p => p.Orders)
                    .HasForeignKey(d => d.IdUser)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_dbo.Order.Account");
            });

            modelBuilder.Entity<Product>(entity =>
            {
                entity.ToTable("Product");

                entity.HasIndex(e => e.Name, "PK_dbo_Product_Name")
                    .IsUnique();

                entity.Property(e => e.Country).HasMaxLength(100);

                entity.Property(e => e.Description).HasMaxLength(500);

                entity.Property(e => e.LastUpdate).HasColumnType("datetime");

                entity.Property(e => e.Location).HasMaxLength(500);

                entity.Property(e => e.Name).HasMaxLength(500);

                entity.Property(e => e.Published).HasColumnType("datetime");

                entity.Property(e => e.Status).HasMaxLength(100);

                entity.Property(e => e.VendorUsername).HasMaxLength(500);
            });

            modelBuilder.Entity<ProductSelectedCart>(entity =>
            {
                entity.ToTable("ProductSelectedCart");

                entity.Property(e => e.Status).HasMaxLength(100);

                entity.HasOne(d => d.IdProductNavigation)
                    .WithMany(p => p.ProductSelectedCarts)
                    .HasForeignKey(d => d.IdProduct)
                    .HasConstraintName("FK_dbo.ProductSelected.Product");

                entity.HasOne(d => d.IdShoppingCartNavigation)
                    .WithMany(p => p.ProductSelectedCarts)
                    .HasForeignKey(d => d.IdShoppingCart)
                    .HasConstraintName("FK_dbo.ProductSelected.ShoppingCart");
            });

            modelBuilder.Entity<ProductSelectedOrder>(entity =>
            {
                entity.ToTable("ProductSelectedOrder");

                entity.Property(e => e.Status).HasMaxLength(100);

                entity.HasOne(d => d.IdOrderNavigation)
                    .WithMany(p => p.ProductSelectedOrders)
                    .HasForeignKey(d => d.IdOrder)
                    .HasConstraintName("FK_dbo.ProductSelected.Order");

                entity.HasOne(d => d.IdProductNavigation)
                    .WithMany(p => p.ProductSelectedOrders)
                    .HasForeignKey(d => d.IdProduct)
                    .HasConstraintName("FK_dbo.ProductSelected.Product.Order");
            });

            modelBuilder.Entity<Product_Image>(entity =>
            {
                entity.ToTable("Product.Image");

                entity.Property(e => e.LastUpdate).HasColumnType("datetime");

                entity.HasOne(d => d.IdProductNavigation)
                    .WithMany(p => p.Product_Images)
                    .HasForeignKey(d => d.IdProduct)
                    .HasConstraintName("FK_dbo.Image.Product");
            });

            modelBuilder.Entity<ShoppingCart>(entity =>
            {
                entity.ToTable("ShoppingCart");

                entity.Property(e => e.LastUpdate).HasColumnType("datetime");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("User");

                entity.HasIndex(e => e.Email, "PK_dbo_AdminAccount_email")
                    .IsUnique();

                entity.HasIndex(e => e.UserName, "PK_dbo_AdminAccount_userName")
                    .IsUnique();

                entity.Property(e => e.BirthDay).HasColumnType("datetime");

                entity.Property(e => e.Description).HasMaxLength(500);

                entity.Property(e => e.Email).HasMaxLength(500);

                entity.Property(e => e.LastLogin).HasColumnType("datetime");

                entity.Property(e => e.LastUpdate).HasColumnType("datetime");

                entity.Property(e => e.Password).HasMaxLength(100);

                entity.Property(e => e.Phone).HasMaxLength(100);

                entity.Property(e => e.Reputation).HasMaxLength(100);

                entity.Property(e => e.Status).HasMaxLength(100);

                entity.Property(e => e.UserName).HasMaxLength(100);

                entity.Property(e => e.UserType).HasMaxLength(500);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
