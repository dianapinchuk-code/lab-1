using Domain.Rooms;
using Infrastructure.Persistence.Converters;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class RoomsConfiguration : IEntityTypeConfiguration<HotelRoom>
{
    public void Configure(EntityTypeBuilder<HotelRoom> builder)
    {
        builder.HasKey(x => x.Id);

        // Унікальний номер кімнати
        builder.Property(x => x.RoomNumber)
            .HasColumnType("varchar(50)")
            .IsRequired();

        builder.HasIndex(x => x.RoomNumber)
            .IsUnique();

        builder.Property(x => x.PricePerNight)
            .HasColumnType("numeric(10,2)")
            .IsRequired();

        builder.Property(x => x.Capacity)
            .HasColumnType("integer")
            .IsRequired();

        builder.Property(x => x.IsAvailable)
            .HasColumnType("boolean")
            .IsRequired();

        builder.Property(x => x.CreatedAt)
            .HasConversion(new DateTimeUtcConverter())
            .HasDefaultValueSql("timezone('utc', now())")
            .IsRequired();
    }
}