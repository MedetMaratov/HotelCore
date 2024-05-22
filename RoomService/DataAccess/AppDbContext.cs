using Microsoft.EntityFrameworkCore;
using RoomService.Models;

namespace RoomService.DataAccess;

public class AppDbContext: DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options) {}
    
    public DbSet<Room> Rooms { get; set; }
    public DbSet<Reservation> Reservations { get; set; }
    public DbSet<RoomType> RoomsTypes { get; set; }
    public DbSet<OccupiedRoom> OccupiedRooms { get; set; }
    public DbSet<Amenity> Amenities { get; set; }
    public DbSet<HotelBranch> HotelBranches { get; set; }
    public DbSet<ImageForRoomType> ImagesForRoomTypes { get; set; }
    public DbSet<Guest> Guests { get; set; }
    public DbSet<Reservator> Reservators { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
    }

}