using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MovieBringer.Core.Entities;



namespace MovieBringer.Repository.Configurations
{
    internal class MovieListConfiguration : IEntityTypeConfiguration<MovieList>
    {
        public void Configure(EntityTypeBuilder<MovieList> builder)
        {
            builder.HasKey(x => x.Id); 
            builder.Property(x => x.Id).UseIdentityColumn(); 
            builder.Property(x => x.ListName).IsRequired().HasMaxLength(50);

            builder.ToTable("MovieLists"); 

        }
    }
}
