using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace ContactManagementApi.OutputDirectory;

public partial class ContactDbContext : DbContext
{
    public ContactDbContext()
    {
    }

    public ContactDbContext(DbContextOptions<ContactDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Contact> Contacts { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)

        => optionsBuilder.UseSqlServer("Server=DOSYSNET006;Database=ContactDB;Trusted_Connection=True;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Contact>(entity =>
        {
            entity.HasKey(e => e.ContactId).HasName("PK__Contacts__5C66259BDF240392");

            entity.HasIndex(e => e.Email, "UQ__Contacts__A9D10534E3C9A896").IsUnique();

            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Email).HasMaxLength(100);
            entity.Property(e => e.ModifiedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Name).HasMaxLength(100);
            entity.Property(e => e.Phone).HasMaxLength(15);

            entity.HasOne(d => d.User).WithMany(p => p.Contacts)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__Contacts__UserId__5EBF139D");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.RoleId).HasName("PK__Role__8AFACE1AA812E31A");

            entity.ToTable("Role");

            entity.HasIndex(e => e.RoleName, "UQ__Role__DA15413EC4601833").IsUnique();

            entity.Property(e => e.RoleName).HasMaxLength(50);
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__Users__1788CC4CEEB25C3F");

            entity.HasIndex(e => e.Username, "UQ__Users__536C85E460FF24D5").IsUnique();

            entity.HasIndex(e => e.Email, "UQ__Users__A9D10534E2723FAD").IsUnique();

            entity.Property(e => e.Email).HasMaxLength(100);
            entity.Property(e => e.PasswordHash).HasMaxLength(256);
            entity.Property(e => e.Username).HasMaxLength(50);

            entity.HasMany(d => d.Roles).WithMany(p => p.Users)
                .UsingEntity<Dictionary<string, object>>(
                    "UserRole",
                    r => r.HasOne<Role>().WithMany()
                        .HasForeignKey("RoleId")
                        .HasConstraintName("FK__UserRole__RoleId__0B91BA14"),
                    l => l.HasOne<User>().WithMany()
                        .HasForeignKey("UserId")
                        .HasConstraintName("FK__UserRole__UserId__0A9D95DB"),
                    j =>
                    {
                        j.HasKey("UserId", "RoleId").HasName("PK__UserRole__AF2760AD5499943A");
                        j.ToTable("UserRole");
                    });
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
