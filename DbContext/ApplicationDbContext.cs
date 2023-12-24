using AldhamrimediaApi.Models;


using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AldhamrimediaApi.DbContext
{
    public class ApplicationDbContext : IdentityDbContext<User, Role, int>
    {

        public ApplicationDbContext(DbContextOptions Options) : base(Options)
        {


        }
        protected override void OnModelCreating(ModelBuilder builder)

        {
            base.OnModelCreating(builder);
            //new UserEntityConfig().Configure(builder.Entity<User>());
            //new DepartmentEntityConfig().Configure(builder.Entity<Department>());




            builder.Entity<Notifications>()
           .HasOne(u => u.User)
           .WithMany(c => c.Notifications)
           .HasForeignKey(c => c.userId);

            builder.Entity<SubService>()
           .HasOne(m => m.Utilitie)
           .WithMany(p => p.subServices)
           .HasForeignKey(m => m.utilitieId);

            builder.Entity<Purchases>()
          .HasOne(u => u.User)
          .WithMany(c => c.Purchases)
          .HasForeignKey(c => c.userId);


        }

        public DbSet<utilitie> utilities { get; set; }
        public DbSet<SubService> subServices { get; set; }
        public DbSet<Notifications> notifications { get; set; }
        public DbSet<Purchases> Purchases { get; set; }



    }
}
