using EventEase.Models;
using Microsoft.EntityFrameworkCore;

namespace EventEase.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Venue> Venues { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<Booking> Bookings { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Map to plural table names (matching Azure SQL database created by EF migrations)
            modelBuilder.Entity<Venue>().ToTable("Venues");
            modelBuilder.Entity<Event>().ToTable("Events");
            modelBuilder.Entity<Booking>().ToTable("Bookings");

            // Configure relationships and constraints
            modelBuilder.Entity<Venue>(entity =>
            {
                entity.Property(v => v.VenueName).IsRequired().HasMaxLength(100);
                entity.Property(v => v.Location).IsRequired().HasMaxLength(200);
                entity.Property(v => v.ImageUrl).HasMaxLength(500);
            });

            modelBuilder.Entity<Event>(entity =>
            {
                entity.Property(e => e.EventName).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Description).HasMaxLength(500);
                entity.HasOne(e => e.Venue)
                      .WithMany(v => v.Events)
                      .HasForeignKey(e => e.VenueId)
                      .IsRequired()
                      .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<Booking>(entity =>
            {
                entity.Property(b => b.CustomerName).HasMaxLength(100);
                entity.Property(b => b.CustomerEmail).HasMaxLength(100);
                entity.Property(b => b.CustomerPhone).HasMaxLength(20);
                entity.Property(b => b.BookingStatus).HasMaxLength(20);

                entity.HasOne(b => b.Event)
                      .WithMany(e => e.Bookings)
                      .HasForeignKey(b => b.EventId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(b => b.Venue)
                      .WithMany(v => v.Bookings)
                      .HasForeignKey(b => b.VenueId)
                      .OnDelete(DeleteBehavior.Cascade);
            });
        }
    }
}
