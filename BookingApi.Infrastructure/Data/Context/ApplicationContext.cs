using BookingApi.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace BookingApi.Infrastructure.Data.Context
{
    public class ApplicationContext : DbContext
    {
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<Reservation> Reservations { get; set; }

        public ApplicationContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<Room>()
                .HasData(new Room(101) { Description = "The very last room of the hotel." });

            modelBuilder
                .Entity<Customer>()
                .HasKey(c => c.Id);
        }
    }
}
