using GuestOrderingService.Models;
using GuestOrderingService.Models.Amenity;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GuestOrderingService.DataAccess.EntityConfigs;

public class AmenityEntityTypeConfig
{
    public void Configure(EntityTypeBuilder<Amenity> builder)
    {
        builder.Property(a => a.Name).HasMaxLength(150);
        builder.Property(a => a.Description).HasMaxLength(500);
    }
}