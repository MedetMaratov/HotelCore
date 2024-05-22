using GuestOrderingService.Models;
using GuestOrderingService.Models.Amenity;
using GuestOrderingService.Models.Service;
using Microsoft.EntityFrameworkCore;

namespace GuestOrderingService.DataAccess;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        :base(options) { }
    
    public DbSet<Order> Orders { get; set; }
    public DbSet<Service> Services { get; set; }
    public DbSet<Amenity> Amenities { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {

    }
}