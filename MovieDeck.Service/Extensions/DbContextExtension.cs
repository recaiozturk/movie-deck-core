using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MovieDeck.Repository.Shared;
using MovieDeck.Repository.Shared.Entities;

namespace MovieDeck.Service.Extensions
{
    public static class DbContextExtension
    {
        public static IServiceCollection AddEfCoreExt(this IServiceCollection services, IConfiguration configuration)
        {

            services.AddDbContext<AppDbContext>(x =>
            {
                var connectionString = configuration.GetConnectionString("SqlSever");
                x.UseSqlServer(connectionString);
            });



            services.AddIdentity<AppUser, AppRole>(options =>
            {
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequiredLength = 8;
                options.User.RequireUniqueEmail = true;
            })
            .AddEntityFrameworkStores<AppDbContext>();

            services.ConfigureApplicationCookie(opt =>
            {
                var cookieBuilder = new CookieBuilder
                {
                    Name = "MovieDeckCookie",
                    HttpOnly = true
                };

                    opt.LoginPath = new PathString("/Auth/Signin");
                    opt.AccessDeniedPath = new PathString("/Home/AccessDenied");
                    opt.Cookie = cookieBuilder;
                    opt.ExpireTimeSpan = TimeSpan.FromDays(60);
                    opt.SlidingExpiration = true;
            });

            return services;
        }
    }
}
