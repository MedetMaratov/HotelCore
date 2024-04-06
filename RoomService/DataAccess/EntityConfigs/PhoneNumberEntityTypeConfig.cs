using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RoomService.Models;

namespace RoomService.DataAccess.EntityConfigs;

public class PhoneNumberEntityTypeConfig : IEntityTypeConfiguration<PhoneNumber>
{
    public void Configure(EntityTypeBuilder<PhoneNumber> builder)
    {

    }
}