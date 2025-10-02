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
        // public DbSet<CreditScore> CreditScores { get; set; } // Закомментируем пока

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Конфигурация Borrower
            modelBuilder.Entity<Borrower>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.FirstName).IsRequired().HasMaxLength(100);
                entity.Property(e => e.LastName).IsRequired().HasMaxLength(100);
                entity.Property(e => e.PassportNumber).IsRequired().HasMaxLength(20);
                entity.Property(e => e.PassportSeries).IsRequired().HasMaxLength(10);
                entity.Property(e => e.PhoneNumber).HasMaxLength(20);
                entity.Property(e => e.Email).HasMaxLength(255);
                
                // Уникальный индекс по паспорту
                entity.HasIndex(e => new { e.PassportSeries, e.PassportNumber }).IsUnique();
            });

            // Конфигурация Lender
            modelBuilder.Entity<Lender>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(255);
                entity.Property(e => e.LicenseNumber).IsRequired().HasMaxLength(50);
                entity.Property(e => e.PhoneNumber).HasMaxLength(20);
                entity.Property(e => e.Email).HasMaxLength(255);
                
                entity.HasIndex(e => e.LicenseNumber).IsUnique();
            });

            // Конфигурация CreditHistory
            modelBuilder.Entity<CreditHistory>(entity =>
            {
                entity.HasKey(e => e.Id);
                
                entity.Property(e => e.LoanAmount).HasColumnType("decimal(18,2)");
                entity.Property(e => e.InterestRate).HasColumnType("decimal(5,2)");
                entity.Property(e => e.RemainingDebt).HasColumnType("decimal(18,2)");
                entity.Property(e => e.Status).IsRequired().HasMaxLength(50);
                
                // Внешние ключи - временно закомментируем навигационные свойства
                /*
                entity.HasOne(e => e.Borrower)
                      .WithMany(b => b.CreditHistories)
                      .HasForeignKey(e => e.BorrowerId)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(e => e.Lender)
                      .WithMany(l => l.CreditHistories)
                      .HasForeignKey(e => e.LenderId)
                      .OnDelete(DeleteBehavior.Restrict);
                */
            });

            // Конфигурация CreditScore - временно закомментируем
            /*
            modelBuilder.Entity<CreditScore>(entity =>
            {
                entity.HasKey(e => e.Id);
                
                entity.Property(e => e.Rating).HasMaxLength(50);
                entity.Property(e => e.Factors).HasMaxLength(1000);
                
                entity.HasOne(e => e.Borrower)
                      .WithMany()
                      .HasForeignKey(e => e.BorrowerId)
                      .OnDelete(DeleteBehavior.Cascade);
                      
                entity.HasIndex(e => e.BorrowerId);
            });
            */
        }
    }
}