using clsSocialServicesDataAccess;
using clsSocialServicesDataAccess;
using Microsoft.EntityFrameworkCore;

namespace clsSocialServicesDataAccess
{
    
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

       
        public DbSet<PersonEntity> People { get; set; }
        public DbSet<UserEntity> Users { get; set; }
        //public DbSet<CountyEntity> Counties { get; set; }
        //public DbSet<CityEntity> Cities { get; set; }
        //public DbSet<PostEntity> Posts { get; set; }
        //public DbSet<PostTypeEntity> PostTypes { get; set; }
        //public DbSet<ServiceApplicationEntity> ServiceApplications { get; set; }
        //public DbSet<FeedbackEntity> Feedback { get; set; }
        //public DbSet<ProfessionEntity> Professions { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            

            modelBuilder.Entity<PersonEntity>()
                .HasOne(p => p.User)
                .WithOne(u => u.Person)
                .HasForeignKey<UserEntity>(u => u.PersonID);
        }
    }
}
