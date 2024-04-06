using GuestOrderingService.Models;
using GuestOrderingService.Models.Service;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GuestOrderingService.DataAccess.EntityConfigs;

public class ServiceEntityTypeConfig : IEntityTypeConfiguration<Service>
{
    public void Configure(EntityTypeBuilder<Service> builder)
    {
        builder.Property(o => o.Name).HasMaxLength(150);
        builder.Property(o => o.Description).HasMaxLength(500);
    }
}