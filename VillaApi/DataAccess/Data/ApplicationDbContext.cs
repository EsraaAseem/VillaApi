using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using VillaApi.DataAccess.Config;
using VillaApi.Model;

namespace VillaApi.DataAccess.Data
{
    public class ApplicationDbContext :DbContext// IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {

        }
       public DbSet<Villa>villas { get; set; }
      public DbSet <VillaNumber>villasNumber { get; set;}
       protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(VillaConfiguration).Assembly);

        }
    }
}
