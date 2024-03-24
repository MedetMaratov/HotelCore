using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RoomService.Models;

namespace RoomService.DataAccess.EntityConfigs;

public class ReservationEntityTypeConfig : IEntityTypeConfiguration<Reservation>
{
    public void Configure(EntityTypeBuilder<Reservation> builder)
    {
        builder.Property(r => r.ReservationCreatorId).IsRequired();
        builder.Property(r => r.MadeBy).IsRequired();
        builder.Property(r => r.DateIn).IsRequired();
        builder.Property(r => r.DateOut).IsRequired();
        builder.Property(r => r.Status).IsRequired();
    }
}