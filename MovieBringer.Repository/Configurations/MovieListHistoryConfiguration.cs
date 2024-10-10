using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MovieBringer.Core.Entities;


namespace MovieBringer.Repository.Configurations
{
    internal class MovieListHistoryConfiguration : IEntityTypeConfiguration<MovieListHistory>
    {
        public void Configure(EntityTypeBuilder<MovieListHistory> builder)
        {
            builder.HasKey(x => x.Id); 
            builder.Property(x => x.Id).UseIdentityColumn(); 

            builder.ToTable("MovieListHistorys"); 
        }
    }
}
