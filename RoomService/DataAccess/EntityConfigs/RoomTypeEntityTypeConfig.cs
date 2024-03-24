using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RoomService.Models;

namespace RoomService.DataAccess.EntityConfigs;

public class RoomTypeEntityTypeConfig : IEntityTypeConfiguration<RoomType>
{
    public void Configure(EntityTypeBuilder<RoomType> builder)
    {
        builder.Property(r => r.Title).IsRequired();
        builder.Property(r => r.Title).HasMaxLength(100);
        builder.Property(r => r.Description).IsRequired();
        builder.Property(r => r.MaxCapacity).IsRequired();
        builder.Property(r => r.NightlyRate).IsRequired();
    }
}