using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using MovieBringer.Core.Entities;
using MovieBringer.Core.Repositories;
using MovieBringer.Core.Services;
using MovieBringer.Core.UnitOfWorks;
using MovieBringer.Core.Util;
using MovieBringer.Core.Util.Concrate;
using MovieBringer.Repository;
using MovieBringer.Repository.Repositories;
using MovieBringer.Repository.UnitOfWorks;
using MovieBringer.Service.Mapping;
using MovieBringer.Service.Services;
using MovieBringer.Service.Validations;
using MovieBringer.WebApp.Services.Abstract;
using MovieBringer.WebApp.Services.Concrate;
using MovieBringer.WebApp.Util;
using MovieBringer.WebApp.Util.Abstract;
using Smidge;


var builder = WebApplication.CreateBuilder(args);

var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

//cors
builder.Services.AddCors(options =>
{
    options.AddPolicy(MyAllowSpecificOrigins,
    policy =>
    {
        policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
    });
});

//service swagger
builder.Services.AddSwaggerGen(option =>
{
    option.SwaggerDoc("v1", new OpenApiInfo { Title = "MovieBringer API", Version = "v1" });
    option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter a valid token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "Bearer"
    });
    option.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type=ReferenceType.SecurityScheme,
                    Id="Bearer"
                }
            },
            new string[]{}
        }
    });
});

builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddFluentValidationClientsideAdapters();
builder.Services.AddValidatorsFromAssemblyContaining<RegisterViewModelValidator>();
builder.Services.AddControllersWithViews().AddRazorRuntimeCompilation();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddHttpClient();


//Identity Options
builder.Services.Configure<IdentityOptions>(options =>
{
    options.User.RequireUniqueEmail = true;
    options.Lockout.MaxFailedAccessAttempts = 5;
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
    options.SignIn.RequireConfirmedEmail = true;
});

//DB Context
builder.Services.AddDbContext<AppDbContext>(options =>
{
    var config = builder.Configuration;
    var connectionString = config.GetConnectionString("database");
    options.UseSqlServer(connectionString);
});

builder.Services.AddIdentity<AppUser, AppRole>().AddEntityFrameworkStores<AppDbContext>().AddDefaultTokenProviders();

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));



//DI Controls
builder.Services.AddScoped<UserDTOService>();

builder.Services.AddScoped<IMovieListDTOService, MovieListDTOService>();
builder.Services.AddScoped<IMovieListHistoryDTOService, MovieListHistoryDTOService>();
builder.Services.AddScoped<IAuthDtoService, AuthDtoService>();

builder.Services.AddScoped<IMovieListRepository, MovieListRepository>();
builder.Services.AddScoped<IMovieListHistoryRepository, MovieListHistoryRepository>();

builder.Services.AddScoped<IMethods, Methods>();
builder.Services.AddSingleton<IMyTokenHandler, MyTokenHandler>();

//DI Web Controls
builder.Services.AddScoped<IMovieListService, MovieListService>();
builder.Services.AddScoped<IProfileWebService, ProfileWebService>();
builder.Services.AddScoped<IApiService, ApiService>();


//mail settings
builder.Services.AddScoped<IEmailSender, SmtpEmailSender>(i =>
new SmtpEmailSender(
    builder.Configuration["EmailSender:Host"],
    builder.Configuration.GetValue<int>("EmailSender:Port"),
    builder.Configuration.GetValue<bool>("EmailSender:EnableSSL"),
    builder.Configuration["EmailSender:UserName"],
    builder.Configuration["EmailSender:Password"])
);

builder.Services.AddAutoMapper(typeof(MapProfile));
builder.Services.AddScoped<IMovieService, MovieService>();

//smidge
builder.Services.AddSmidge(builder.Configuration.GetSection("smidge"));

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}
app.UseDeveloperExceptionPage();

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.UseSwagger();
app.UseSwaggerUI();


//smidge
//app.UseSmidge(bundle =>
//{
//    bundle.CreateJs("my-js-bundle", "~/js/sweetAlertCustom.js", "~/js/Views/MovieList", "~/js/Views/Profile");
//    bundle.CreateJs("my-js-bundle", "~/js/sweetAlertCustom.js", "~/js/Views/MovieList", "~/js/Views/Profile");
//    bundle.CreateJs("my-js-bundle", "~/js/sweetAlertCustom.js", "~/js/Views/MovieList", "~/js/Views/Profile");

//    //asagýdaki sistemde js lerde debug=true deyim enviroment secimi kullanamdan ,her serferinde app datayý bastan olusturur,tavisiye edilen enviroment ile kulalným
//    //bundle.CreateJs("my-js-bundle", "~/js/").WithEnvironmentOptions(BundleEnvironmentOptions.Create().ForDebug(builder => builder.EnableCompositeProcessing().EnableFileWatcher().SetCacheBusterType<AppDomainLifetimeCacheBuster>().CacheControlOptions(enableEtag: false, cacheControlMaxAge: 0)).Build());

//    //asagýdaki sekilde klasordkilerin hepsini bundle eder
//    //bundle.CreateJs("my-js-bundle", "~/js/"); 

//    //bundle.CreateCss("my-css-bundle", "~/css/site.css", "~/lib/bootstrap/dist/css/bootstrap.css");
//});

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapControllers();

app.Run();
