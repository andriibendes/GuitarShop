using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace MyWebApplication
{
    public partial class guitar_shopContext : DbContext
    {
        public guitar_shopContext()
        {
        }

        public guitar_shopContext(DbContextOptions<guitar_shopContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Brands> Brands { get; set; }
        public virtual DbSet<Comments> Comments { get; set; }
        public virtual DbSet<Forms> Forms { get; set; }
        public virtual DbSet<Guitarcomment> Guitarcomment { get; set; }
        public virtual DbSet<Guitarmusician> Guitarmusician { get; set; }
        public virtual DbSet<Guitars> Guitars { get; set; }
        public virtual DbSet<Materials> Materials { get; set; }
        public virtual DbSet<Musicians> Musicians { get; set; }
        public virtual DbSet<Orders> Orders { get; set; }
        public virtual DbSet<Types> Types { get; set; }
        public virtual DbSet<Users> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Server=WIN-MO7NEDDO8MB\\SQLEXPRESS; Database=guitar_shop; Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Brands>(entity =>
            {
                entity.ToTable("brands");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Country)
                    .IsRequired()
                    .HasColumnName("country")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name")
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Comments>(entity =>
            {
                entity.ToTable("comments");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Date)
                    .HasColumnName("date")
                    .HasColumnType("date");

                entity.Property(e => e.Dislikes).HasColumnName("dislikes");

                entity.Property(e => e.Info)
                    .IsRequired()
                    .HasColumnName("info")
                    .HasColumnType("ntext");

                entity.Property(e => e.Likes).HasColumnName("likes");

                entity.Property(e => e.UserId).HasColumnName("user_id");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Comments)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_comments_users");
            });

            modelBuilder.Entity<Forms>(entity =>
            {
                entity.ToTable("forms");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name")
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Guitarcomment>(entity =>
            {
                entity.ToTable("guitarcomment");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CommentId).HasColumnName("comment_id");

                entity.Property(e => e.GuitarId).HasColumnName("guitar_id");

                entity.HasOne(d => d.Comment)
                    .WithMany(p => p.Guitarcomment)
                    .HasForeignKey(d => d.CommentId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_guitarcomment_comments");

                entity.HasOne(d => d.Guitar)
                    .WithMany(p => p.Guitarcomment)
                    .HasForeignKey(d => d.GuitarId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_guitarcomment_guitars");
            });

            modelBuilder.Entity<Guitarmusician>(entity =>
            {
                entity.ToTable("guitarmusician");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.GuitarId).HasColumnName("guitar_id");

                entity.Property(e => e.MusicianId).HasColumnName("musician_id");

                entity.HasOne(d => d.Guitar)
                    .WithMany(p => p.Guitarmusician)
                    .HasForeignKey(d => d.GuitarId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_guitarmusician_guitars");

                entity.HasOne(d => d.Musician)
                    .WithMany(p => p.Guitarmusician)
                    .HasForeignKey(d => d.MusicianId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_guitarmusician_musicians");
            });

            modelBuilder.Entity<Guitars>(entity =>
            {
                entity.ToTable("guitars");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.BrandId).HasColumnName("brand_id");

                entity.Property(e => e.Cost).HasColumnName("cost");

                entity.Property(e => e.FormId).HasColumnName("form_id");

                entity.Property(e => e.Info)
                    .IsRequired()
                    .HasColumnName("info")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.MaterialId).HasColumnName("material_id");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.TypeId).HasColumnName("type_id");

                entity.Property(e => e.Year).HasColumnName("year");

                entity.HasOne(d => d.Brand)
                    .WithMany(p => p.Guitars)
                    .HasForeignKey(d => d.BrandId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_guitars_brands");

                entity.HasOne(d => d.Form)
                    .WithMany(p => p.Guitars)
                    .HasForeignKey(d => d.FormId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_guitars_forms");

                entity.HasOne(d => d.Material)
                    .WithMany(p => p.Guitars)
                    .HasForeignKey(d => d.MaterialId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_guitars_materials");

                entity.HasOne(d => d.Type)
                    .WithMany(p => p.Guitars)
                    .HasForeignKey(d => d.TypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_guitars_types");
            });

            modelBuilder.Entity<Materials>(entity =>
            {
                entity.ToTable("materials");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name")
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Musicians>(entity =>
            {
                entity.ToTable("musicians");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name")
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Orders>(entity =>
            {
                entity.ToTable("orders");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Address)
                    .IsRequired()
                    .HasColumnName("address")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.DeliveryCost)
                    .IsRequired()
                    .HasColumnName("delivery_cost")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.GuitarId).HasColumnName("guitar_id");

                entity.Property(e => e.UserId).HasColumnName("user_id");

                entity.HasOne(d => d.Guitar)
                    .WithMany(p => p.Orders)
                    .HasForeignKey(d => d.GuitarId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_orders_guitars");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Orders)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_orders_users");
            });

            modelBuilder.Entity<Types>(entity =>
            {
                entity.ToTable("types");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name")
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Users>(entity =>
            {
                entity.ToTable("users");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Login)
                    .IsRequired()
                    .HasColumnName("login")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasColumnName("password")
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
