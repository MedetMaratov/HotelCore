namespace RoomService.DataAccess;

public class DbInitializer
{
    public static void Initialize(AppDbContext context)
    {
        context.Database.EnsureDeleted();
        context.Database.EnsureCreated();
        context.Amenities.AddRange(DbSeeder.GetAmenities());
        context.RoomsTypes.AddRange(DbSeeder.GetRoomTypes());
        context.HotelBranches.AddRange(DbSeeder.GetHotelBranches());
        context.Rooms.AddRange(DbSeeder.GetRooms());
        context.SaveChanges();
    }
}