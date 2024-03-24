using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RoomService.Models;

namespace RoomService.DataAccess.EntityConfigs;

public class OccupiedRoomEntityTypeConfig : IEntityTypeConfiguration<OccupiedRoom>
{
    public void Configure(EntityTypeBuilder<OccupiedRoom> builder)
    {
        builder.Property(r => r.CheckIn).IsRequired();
        builder.Property(r => r.CheckOut).IsRequired();
    }
}