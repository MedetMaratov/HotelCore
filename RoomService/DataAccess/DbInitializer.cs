namespace RoomService.DataAccess;

public class DbInitializer
{
    public static void Initialize(AppDbContext context)
    {
        context.Database.EnsureCreated();
        context.SaveChanges();
    }
}