using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace GroceryListAPI.Models;

public partial class GroceryListsDbContext : DbContext
{
    public GroceryListsDbContext()
    {
    }

    public GroceryListsDbContext(DbContextOptions<GroceryListsDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<AppUser> AppUsers { get; set; }

    public virtual DbSet<AppUserSetting> AppUserSettings { get; set; }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<GroceryList> GroceryLists { get; set; }

    public virtual DbSet<Item> Items { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=.\\SQLEXPRESS;Database=GroceryListsDB;Trusted_Connection=True; TrustServerCertificate=true;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AppUser>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__AppUser__3213E83FD629EEC6");

            entity.ToTable("AppUser");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Email)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("email");
            entity.Property(e => e.FirstName)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("firstName");
            entity.Property(e => e.LastName)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("lastName");
            entity.Property(e => e.PasswordHash).HasColumnName("passwordHash");
            entity.Property(e => e.PasswordSalt).HasColumnName("passwordSalt");
            entity.Property(e => e.Username)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("username");
        });

        modelBuilder.Entity<AppUserSetting>(entity =>
        {
            entity.HasKey(e => e.AppUserId).HasName("PK__AppUserS__3DC789401E10698C");

            entity.ToTable("AppUserSetting");

            entity.Property(e => e.AppUserId)
                .ValueGeneratedNever()
                .HasColumnName("appUserId");
            entity.Property(e => e.DarkMode).HasColumnName("darkMode");
            entity.Property(e => e.ShowCustom).HasColumnName("showCustom");

            entity.HasOne(d => d.AppUser).WithOne(p => p.AppUserSetting)
                .HasForeignKey<AppUserSetting>(d => d.AppUserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__AppUserSe__appUs__300424B4");
        });

        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Category__3213E83FFDC32EC6");

            entity.ToTable("Category");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AppUserId).HasColumnName("appUserId");
            entity.Property(e => e.IsCustom).HasColumnName("isCustom");
            entity.Property(e => e.Name)
                .HasMaxLength(60)
                .IsUnicode(false)
                .HasColumnName("name");
            entity.Property(e => e.PhotoUrl)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("photoUrl");

            entity.HasOne(d => d.AppUser).WithMany(p => p.Categories)
                .HasForeignKey(d => d.AppUserId)
                .HasConstraintName("FK_Category_AppUser");
        });

        modelBuilder.Entity<GroceryList>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__GroceryL__3213E83FE13A2BE9");

            entity.ToTable("GroceryList");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AppUserId).HasColumnName("appUserId");
            entity.Property(e => e.ItemsJson)
                .HasMaxLength(4000)
                .IsUnicode(false)
                .HasColumnName("itemsJson");
            entity.Property(e => e.Name)
                .HasMaxLength(60)
                .IsUnicode(false)
                .HasColumnName("name");
            entity.Property(e => e.ShowCrossedOff).HasColumnName("showCrossedOff");

            entity.HasOne(d => d.AppUser).WithMany(p => p.GroceryLists)
                .HasForeignKey(d => d.AppUserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__GroceryLi__appUs__286302EC");
        });

        modelBuilder.Entity<Item>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Item__3213E83FC6058A43");

            entity.ToTable("Item");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CategoryId).HasColumnName("categoryId");
            entity.Property(e => e.IsCustom).HasColumnName("isCustom");
            entity.Property(e => e.Name)
                .HasMaxLength(60)
                .IsUnicode(false)
                .HasColumnName("name");
            entity.Property(e => e.PhotoUrl)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("photoUrl");
            entity.Property(e => e.Unit)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("unit");

            entity.HasOne(d => d.Category).WithMany(p => p.Items)
                .HasForeignKey(d => d.CategoryId)
                .HasConstraintName("FK__Item__categoryId__2D27B809");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
