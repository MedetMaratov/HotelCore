using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RoomService.Models;

namespace RoomService.DataAccess.EntityConfigs;

public class RoomEntityTypeConfig : IEntityTypeConfiguration<Room>
{
    public void Configure(EntityTypeBuilder<Room> builder)
    {
        builder.Property(r => r.Number).IsRequired();
        builder.Property(r => r.Type).IsRequired();
    }
}