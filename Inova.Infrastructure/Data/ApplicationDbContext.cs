using Microsoft.EntityFrameworkCore;
using Inova.Domain.Entities;

namespace Inova.Infrastructure.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    // DbSets - ALL our tables
    public DbSet<User> Users { get; set; }
    public DbSet<Customer> Customers { get; set; }
    public DbSet<Consultant> Consultants { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Specialization> Specializations { get; set; }
    public DbSet<Session> Sessions { get; set; }
    public DbSet<Payment> Payments { get; set; }
    public DbSet<ChatMessage> ChatMessages { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // User Configuration
        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(u => u.Id);

            entity.Property(u => u.Email)
                .IsRequired()
                .HasMaxLength(255);

            entity.Property(u => u.PasswordHash)
                .IsRequired()
                .HasMaxLength(500);

            entity.Property(u => u.Role)
                .IsRequired()
                .HasMaxLength(50);

            entity.Property(u => u.ProfileImageUrl)
                .HasMaxLength(500);

            entity.HasIndex(u => u.Email).IsUnique();
        });

        // Customer Configuration
        modelBuilder.Entity<Customer>(entity =>
        {
            entity.HasKey(c => c.Id);

            entity.Property(c => c.FullName)
                .IsRequired()
                .HasMaxLength(200);

            entity.Property(c => c.PhoneNumber)
                .HasMaxLength(20);

            entity.HasOne(c => c.User)
                .WithMany()
                .HasForeignKey(c => c.UserId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        // Consultant Configuration
        modelBuilder.Entity<Consultant>(entity =>
        {
            entity.HasKey(c => c.Id);

            entity.Property(c => c.FullName)
                .IsRequired()
                .HasMaxLength(200);

            entity.Property(c => c.PhoneNumber)
                .HasMaxLength(20);

            entity.Property(c => c.Bio)
                .HasMaxLength(2000);

            entity.Property(c => c.HourlyRate)
                .HasColumnType("decimal(18,2)");

            entity.Property(c => c.ApprovalStatus)
                .IsRequired()
                .HasMaxLength(50);

            entity.Property(c => c.ProfileImageUrl)
                .HasMaxLength(500);

            entity.Property(c => c.CoverImageUrl)
                .HasMaxLength(500);

            entity.Property(c => c.CertificateImageUrl)
                .HasMaxLength(500);

            entity.HasOne(c => c.User)
                .WithMany()
                .HasForeignKey(c => c.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(c => c.Specialization)
                .WithMany(s => s.Consultants)
                .HasForeignKey(c => c.SpecializationId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        // Category Configuration
        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(c => c.Id);

            entity.Property(c => c.NameAr)
                .IsRequired()
                .HasMaxLength(100);

            entity.Property(c => c.NameEn)
                .IsRequired()
                .HasMaxLength(100);

            entity.Property(c => c.Description)
                .HasMaxLength(500);

            entity.Property(c => c.CoverImageUrl)
                .HasMaxLength(500);

            entity.Property(c => c.IconUrl)
                .HasMaxLength(500);
        });

        // Specialization Configuration
        modelBuilder.Entity<Specialization>(entity =>
        {
            entity.HasKey(s => s.Id);

            entity.Property(s => s.NameAr)
                .IsRequired()
                .HasMaxLength(100);

            entity.Property(s => s.NameEn)
                .IsRequired()
                .HasMaxLength(100);

            entity.Property(s => s.Description)
                .HasMaxLength(500);

            entity.Property(s => s.IconUrl)
                .HasMaxLength(500);

            entity.HasOne(s => s.Category)
                .WithMany(c => c.Specializations)
                .HasForeignKey(s => s.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        // Session Configuration
        modelBuilder.Entity<Session>(entity =>
        {
            entity.HasKey(s => s.Id);

            entity.Property(s => s.DurationHours)
                .HasColumnType("decimal(3,1)");

            entity.Property(s => s.TotalAmount)
                .HasColumnType("decimal(18,2)");

            entity.Property(s => s.Status)
                .IsRequired()
                .HasMaxLength(50);

            entity.HasOne(s => s.Customer)
                .WithMany(c => c.Sessions)
                .HasForeignKey(s => s.CustomerId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(s => s.Consultant)
                .WithMany(c => c.Sessions)
                .HasForeignKey(s => s.ConsultantId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        // Payment Configuration
        modelBuilder.Entity<Payment>(entity =>
        {
            entity.HasKey(p => p.Id);

            entity.Property(p => p.Amount)
                .HasColumnType("decimal(18,2)");

            entity.Property(p => p.Status)
                .IsRequired()
                .HasMaxLength(50);

            entity.HasOne(p => p.Session)
                .WithOne(s => s.Payment)
                .HasForeignKey<Payment>(p => p.SessionId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        // ChatMessage Configuration
        modelBuilder.Entity<ChatMessage>(entity =>
        {
            entity.HasKey(cm => cm.Id);

            entity.Property(cm => cm.MessageText)
                .IsRequired()
                .HasMaxLength(2000);

            entity.HasOne(cm => cm.Session)
                .WithMany(s => s.ChatMessages)
                .HasForeignKey(cm => cm.SessionId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(cm => cm.Sender)
                .WithMany()
                .HasForeignKey(cm => cm.SenderId)
                .OnDelete(DeleteBehavior.Restrict);
        });
    }
}