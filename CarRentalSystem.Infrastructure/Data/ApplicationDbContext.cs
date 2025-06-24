// CarRentalSystem.Infrastructure/Data/ApplicationDbContext.cs
using CarRentalSystem.Core.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CarRentalSystem.Infrastructure.Data
{
    public class ApplicationDbContext : IdentityDbContext<AppUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Car> Cars { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Rental> Rentals { get; set; }
        public DbSet<Service> Services { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            List<IdentityRole> roles = new List<IdentityRole>
            {
                new IdentityRole
                {
                    Name= "Admin",
                    NormalizedName = "ADMIN"
                },
                new IdentityRole
                {
                    Name= "User",
                    NormalizedName = "USER"
                }
            };

            modelBuilder.Entity<IdentityRole>().HasData(roles);

            // Configure Car entity
            modelBuilder.Entity<Car>(entity =>
            {
                entity.HasKey(c => c.Id);
                entity.Property(c => c.Type).IsRequired().HasMaxLength(50);
                entity.Property(c => c.Model).IsRequired().HasMaxLength(100);
                entity.Property(c => c.LastServiceDate).IsRequired();

                // Configure 1-to-many relationship with Rentals
                entity.HasMany(c => c.Rentals)
                      .WithOne(r => r.Car)
                      .HasForeignKey(r => r.CarId)
                      .OnDelete(DeleteBehavior.Restrict); // Prevent deleting car if rentals exist

                // Configure 1-to-many relationship with Services
                entity.HasMany(c => c.Services)
                      .WithOne(s => s.Car)
                      .HasForeignKey(s => s.CarId)
                      .OnDelete(DeleteBehavior.Cascade); // Delete services if car is deleted
            });

            // Configure Customer entity
            modelBuilder.Entity<Customer>(entity =>
            {
                entity.HasKey(c => c.Id);
                entity.HasIndex(c => c.CustomerIdString).IsUnique(); // Ensure CustomerIdString is unique
                entity.Property(c => c.CustomerIdString).IsRequired().HasMaxLength(50);
                entity.Property(c => c.FullName).IsRequired().HasMaxLength(200);
                entity.Property(c => c.Address).IsRequired().HasMaxLength(500);

                // Configure 1-to-many relationship with Rentals
                entity.HasMany(c => c.Rentals)
                      .WithOne(r => r.Customer)
                      .HasForeignKey(r => r.CustomerId)
                      .OnDelete(DeleteBehavior.Restrict); // Prevent deleting customer if rentals exist
            });

            // Configure Rental entity
            modelBuilder.Entity<Rental>(entity =>
            {
                entity.HasKey(r => r.Id);
                entity.Property(r => r.StartDate).IsRequired();
                entity.Property(r => r.EndDate).IsRequired();

                // Foreign key relationships defined by navigation properties in Core models
                // CustomerId and CarId are already defined as properties in Rental.
                // EF Core will automatically detect these as foreign keys if navigation properties are set up.
            });

            // Configure Service entity
            modelBuilder.Entity<Service>(entity =>
            {
                entity.HasKey(s => s.Id);
                entity.Property(s => s.ScheduledDate).IsRequired();
                entity.Property(s => s.DurationDays).IsRequired();
                entity.Property(s => s.IsCompleted).IsRequired();
            });
        }

        // Override SaveChanges and SaveChangesAsync to automatically set IDs if needed (already done in model ctor)
        // Or to handle auditable properties like Created/Modified dates.
        public override int SaveChanges()
        {
            SetDates();
            return base.SaveChanges();
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            SetDates();
            return base.SaveChangesAsync(cancellationToken);
        }

        private void SetDates()
        {
            // Example: If you had 'CreatedAt' or 'UpdatedAt' properties
            // foreach (var entry in ChangeTracker.Entries())
            // {
            //     if (entry.State == EntityState.Added && entry.Entity is IAuditable auditable)
            //     {
            //         auditable.CreatedAt = DateTime.UtcNow;
            //     }
            //     else if (entry.State == EntityState.Modified && entry.Entity is IAuditable auditable)
            //     {
            //         auditable.UpdatedAt = DateTime.UtcNow;
            //     }
            // }
        }
    }
}