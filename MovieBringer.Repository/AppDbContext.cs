using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MovieBringer.Core.Entities;
using System.Reflection;

namespace MovieBringer.Repository
{
    public class AppDbContext : IdentityDbContext<AppUser, AppRole, string>
    {

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<MovieList> MovieLists { get; set; }
        public DbSet<MovieListHistory> MovieListHistories { get; set; }

        public DbSet<VoteMovieList> VoteMovieLists { get; set; }

        //entitylerde create date ve update date tek bir yerden yapmak için
        public override int SaveChanges()
        {
            UpdateChangeTracker();
            return base.SaveChanges();
        }
        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            UpdateChangeTracker();
            return base.SaveChangesAsync(cancellationToken);
        }

        public void UpdateChangeTracker()
        {
            foreach (var item in ChangeTracker.Entries())
            {
                if (item.Entity is BaseEntity entityReference)
                {
                    switch (item.State)
                    {
                        case EntityState.Added:
                            {
                                Entry(entityReference).Property(x => x.UpdatedDate).IsModified = false;
                                entityReference.CreatedDate = DateTime.Now;
                                break;
                            }
                        case EntityState.Modified:
                            {
                                Entry(entityReference).Property(x => x.CreatedDate).IsModified = false;

                                entityReference.UpdatedDate = DateTime.Now;
                                break;
                            }
                    }
                }

            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            //bu assymble deki tüm configire ettiğimiz dosyaları uygula ,fluent api ile uyguladıgımız ayarlar
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            //tek tek de asagıdak igibi yapılır
            //modelBuilder.ApplyConfiguration(new ProductConfiguration());

            base.OnModelCreating(modelBuilder);
        }
    }
}
