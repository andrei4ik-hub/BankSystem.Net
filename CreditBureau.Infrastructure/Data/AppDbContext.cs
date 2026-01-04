using Microsoft.EntityFrameworkCore;
using CreditBureau.Core.Models;

namespace CreditBureau.Infrastructure.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Borrower> Borrowers { get; set; }
        public DbSet<Lender> Lenders { get; set; }
        public DbSet<CreditHistory> CreditHistories { get; set; }
        public DbSet<CreditScore> CreditScores { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Конфигурация Borrower
            modelBuilder.Entity<Borrower>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.FirstName).IsRequired().HasMaxLength(100);
                entity.Property(e => e.LastName).IsRequired().HasMaxLength(100);
                entity.Property(e => e.MiddleName).HasMaxLength(100);
                entity.Property(e => e.PassportNumber).IsRequired().HasMaxLength(20);
                entity.Property(e => e.PassportSeries).IsRequired().HasMaxLength(10);
                entity.Property(e => e.PhoneNumber).HasMaxLength(20);
                entity.Property(e => e.Email).HasMaxLength(255);
                
                // ИСПРАВЛЕНИЕ: Используем timestamp without time zone для PostgreSQL
                entity.Property(e => e.BirthDate).HasColumnType("timestamp without time zone");
                entity.Property(e => e.CreatedAt).HasColumnType("timestamp without time zone");
                entity.Property(e => e.UpdatedAt).HasColumnType("timestamp without time zone");
                
                // Уникальный индекс по паспорту
                entity.HasIndex(e => new { e.PassportSeries, e.PassportNumber }).IsUnique();

                // Навигационное свойство
                entity.HasMany(e => e.CreditHistories)
                      .WithOne(e => e.Borrower)
                      .HasForeignKey(e => e.BorrowerId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            // Конфигурация Lender
            modelBuilder.Entity<Lender>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(255);
                entity.Property(e => e.LicenseNumber).IsRequired().HasMaxLength(50);
                entity.Property(e => e.Address).HasMaxLength(500);
                entity.Property(e => e.PhoneNumber).HasMaxLength(20);
                entity.Property(e => e.Email).HasMaxLength(255);
                
                // ИСПРАВЛЕНИЕ: Используем timestamp without time zone для PostgreSQL
                entity.Property(e => e.CreatedAt).HasColumnType("timestamp without time zone");
                entity.Property(e => e.UpdatedAt).HasColumnType("timestamp without time zone");
                
                entity.HasIndex(e => e.LicenseNumber).IsUnique();

                // Навигационное свойство
                entity.HasMany(e => e.CreditHistories)
                      .WithOne(e => e.Lender)
                      .HasForeignKey(e => e.LenderId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            // Конфигурация CreditHistory
            modelBuilder.Entity<CreditHistory>(entity =>
            {
                entity.HasKey(e => e.Id);
                
                entity.Property(e => e.LoanAmount).HasColumnType("decimal(18,2)");
                entity.Property(e => e.InterestRate).HasColumnType("decimal(5,2)");
                entity.Property(e => e.RemainingDebt).HasColumnType("decimal(18,2)");
                entity.Property(e => e.Status).IsRequired().HasMaxLength(50);
                
                // ИСПРАВЛЕНИЕ: Используем timestamp without time zone для PostgreSQL
                entity.Property(e => e.LoanIssueDate).HasColumnType("timestamp without time zone");
                entity.Property(e => e.LoanEndDate).HasColumnType("timestamp without time zone");
                entity.Property(e => e.CreatedAt).HasColumnType("timestamp without time zone");
                entity.Property(e => e.UpdatedAt).HasColumnType("timestamp without time zone");
                
                // Внешние ключи
                entity.HasOne(e => e.Borrower)
                      .WithMany(b => b.CreditHistories)
                      .HasForeignKey(e => e.BorrowerId)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(e => e.Lender)
                      .WithMany(l => l.CreditHistories)
                      .HasForeignKey(e => e.LenderId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            // Конфигурация CreditScore
            modelBuilder.Entity<CreditScore>(entity =>
            {
                entity.HasKey(e => e.Id);
                
                entity.Property(e => e.Score).IsRequired();
                entity.Property(e => e.Rating).HasMaxLength(50);
                entity.Property(e => e.Factors).HasMaxLength(1000);
                
                // ИСПРАВЛЕНИЕ: Используем timestamp without time zone для PostgreSQL
                entity.Property(e => e.CalculationDate).HasColumnType("timestamp without time zone");
                entity.Property(e => e.CreatedAt).HasColumnType("timestamp without time zone");
                
                entity.HasOne(e => e.Borrower)
                      .WithMany()
                      .HasForeignKey(e => e.BorrowerId)
                      .OnDelete(DeleteBehavior.Cascade);
                      
                entity.HasIndex(e => e.BorrowerId);
            });
        }
    }
}