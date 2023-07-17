using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VillaApi.Model;

namespace VillaApi.DataAccess.Config
{
    public class villaNumberConfiguration : IEntityTypeConfiguration<VillaNumber>
    {
        public void Configure(EntityTypeBuilder<VillaNumber> builder)
        {
            builder.HasKey(x => x.villaNbId);
            builder.Property(x => x.villaNbId).ValueGeneratedNever();
            builder.Property(x=>x.SpecialDetails).IsRequired();
            builder.HasOne(x=>x.villa).WithMany().HasForeignKey(x => x.VillaId).IsRequired();
        }
    }
}
