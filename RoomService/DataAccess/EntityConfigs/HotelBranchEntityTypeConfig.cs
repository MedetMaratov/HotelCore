using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RoomService.Models;

namespace RoomService.DataAccess.EntityConfigs;

public class HotelBranchEntityTypeConfig : IEntityTypeConfiguration<HotelBranch>
{
    public void Configure(EntityTypeBuilder<HotelBranch> builder)
    {
        builder.Property(hb => hb.Name).IsRequired();
        builder.Property(hb => hb.Name).HasMaxLength(100);
    }
}