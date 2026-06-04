using clsSocialServicesDataAccess;
using Microsoft.EntityFrameworkCore;
using clsSocialServicesDataAccess.Counties___Cities;
using clsSocialServicesDataAccess.Posts;
using clsSocialServicesDataAccess.Feedback;
using clsSocialServicesDataAccess.Services;
using clsSocialServicesDataAccess.Feedback;
using clsSocialServicesDataAccess.Admin;

namespace clsSocialServicesDataAccess
{
    
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

       
        public DbSet<PersonEntity> People { get; set; }
        public DbSet<UserEntity> Users { get; set; }
        public DbSet<RefreshToken> Tokens { get; set; }

        public DbSet<CountyEntity> Counties { get; set; }
        public DbSet<CityEntity> Cities { get; set; }
        public DbSet<PostEntity> Posts { get; set; }
        public DbSet<PostTypeEntity> PostTypes { get; set; }


        public DbSet<ServiceApplicationEntity> ServiceApplications { get; set; }
        public DbSet<FeedbackEntity> Feedbacks { get; set; }
        public DbSet<ProfessionEntity> Professions { get; set; }

        public DbSet<AdminEntity> Admins { get; set; }

        public DbSet<LogEntity> Logs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<AdminEntity>(entity =>
                {
                    entity.HasKey(a => a.AdminID);
                    // Primary Key
                    entity.HasOne<PersonEntity>()         
                          .WithMany()                   
                          .HasForeignKey(a => a.PersonID) 
                          .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity<FeedbackEntity>(entity =>
                {
                    entity.HasKey(p => p.FeedbackID); // Primary Key
                    entity.HasOne<UserEntity>()         
                          .WithMany()                   
                          .HasForeignKey(p => p.UserID) 
                          .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity<PersonEntity>()
                .HasOne(p => p.User)
                .WithOne(u => u.Person)
                .HasForeignKey<UserEntity>(u => u.PersonID);
            
            modelBuilder.Entity<ServiceApplicationEntity>(entity =>
            {
                entity.HasKey(sa => sa.ServiceApplicationID); // Primary Key
                
                entity.HasOne<UserEntity>()         
                      .WithMany()                   
                      .HasForeignKey(sa => sa.UserID) 
                      .OnDelete(DeleteBehavior.Restrict); 
                
                entity.HasOne<PostEntity>()         
                      .WithMany()                   
                      .HasForeignKey(sa => sa.PostID) 
                      .OnDelete(DeleteBehavior.Restrict); 
            });



            modelBuilder.Entity<CityEntity>(entity =>
            {
                entity.HasKey(c => c.CityID); // Primary Key
                entity.Property(c => c.CityName).IsRequired().HasMaxLength(100);
            });

            
            

            
            modelBuilder.Entity<CountyEntity>(entity =>
            {
                entity.HasKey(c => c.CountyID); // Primary Key
                entity.Property(c => c.CountyName).IsRequired().HasMaxLength(100);

                
                entity.HasOne<CityEntity>()         
                      .WithMany()                   
                      .HasForeignKey(c => c.CityID) 
                      .OnDelete(DeleteBehavior.Restrict); 
            });
            modelBuilder.Entity<ProfessionEntity>(
                entity =>
                {
                    entity.HasKey(pro => pro.ProfessionID);
                   
                });

            modelBuilder.Entity<PostTypeEntity>(entity =>
            {
                entity.HasKey(pt => pt.PostTypeID);
                entity.Property(pt => pt.TypeTitle).IsRequired().HasMaxLength(50);
            });

            // =========================================================
            // 4. PostEntity Configuration
            // =========================================================
            modelBuilder.Entity<PostEntity>(entity =>
            {
                entity.HasKey(p => p.PostID); // Primary Key

                // --- Relationship: Post -> PostType ---
                entity.HasOne<PostTypeEntity>()
                      .WithMany()
                      .HasForeignKey(p => p.PostTypeID)
                      .OnDelete(DeleteBehavior.Restrict);

                // --- Relationship: Post -> County ---
                entity.HasOne<CountyEntity>()
                      .WithMany()
                      .HasForeignKey(p => p.CountyID)
                      .OnDelete(DeleteBehavior.Restrict);

                // --- Relationship: Post -> User (Assuming you have a User class) ---
                // If your User entity is named 'User', use that here.
                entity.HasOne<UserEntity>()
                      .WithMany()
                      .HasForeignKey(p => p.UserID)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne<ProfessionEntity>().
                WithMany().
                HasForeignKey(p => p.ProfessionID).
                OnDelete(DeleteBehavior.Restrict);
            });
        }
    }
}
