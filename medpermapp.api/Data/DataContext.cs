using medpermapp.api.Models;
using Microsoft.EntityFrameworkCore;

namespace medpermapp.api.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) {}

        public DbSet<Patient> Patients { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<City> Cities { get; set; }
        public DbSet<County> Counties { get; set; }
        public DbSet<Country> Countries { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
        modelBuilder.Entity<Patient>()
            .HasOne(p => p.Address)
            .WithOne(addr => addr.Patient)
            .HasForeignKey<Address>(addr => addr.Id);
        }
    }
}