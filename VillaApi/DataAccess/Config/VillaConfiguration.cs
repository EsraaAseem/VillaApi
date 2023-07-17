using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VillaApi.Model;

namespace VillaApi.DataAccess.Config
{
    public class VillaConfiguration : IEntityTypeConfiguration<Villa>
    {
        public void Configure(EntityTypeBuilder<Villa> builder)
        {
            builder.HasKey(x => x.villaId);
            builder.Property(x => x.villaId).ValueGeneratedOnAdd();
            builder.Property(x=>x.Name).HasColumnType("VARCHAR").HasMaxLength(30).IsRequired();
            builder.Property(x => x.Rate).IsRequired();
            SeedDate(builder);
        }
        private void SeedDate(EntityTypeBuilder<Villa>builder)
        {
            builder.HasData(
                new Villa
                {
                    villaId=Guid.NewGuid(),
                    Name = "Royal Villa",
                    Details = "Fusce 11 tincidunt maximus leo, sed scelerisque massa auctor sit amet. Donec ex mauris, hendrerit quis nibh ac, efficitur fringilla enim.",
                    ImageUrl = "https://dotnetmastery.com/bluevillaimages/villa3.jpg",
                    Rate = 200,
                    Sqft = 550,
                    Amenity = "",
                    CreatedDate = DateTime.Now
                },
             new Villa
             {
                 villaId = Guid.NewGuid(),
                 Name = "Premium Pool Villa",
                 Details = "Fusce 11 tincidunt maximus leo, sed scelerisque massa auctor sit amet. Donec ex mauris, hendrerit quis nibh ac, efficitur fringilla enim.",
                 ImageUrl = "https://dotnetmastery.com/bluevillaimages/villa1.jpg",
                 Rate = 300,
                 Sqft = 550,
                 Amenity = "",
                 CreatedDate = DateTime.Now
             });
        }
    }
}
