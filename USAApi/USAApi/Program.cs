using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using USAApi.Filters;
using USAApi.Models;
using Microsoft.EntityFrameworkCore;
using USAApi;
using Microsoft.EntityFrameworkCore.Internal;
using USAApi.Services;
using USAApi.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using USA_REST_Api.Models;
using Microsoft.AspNetCore.Identity;
using USA_REST_Api.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.Configure<HotelInfo>(
    builder.Configuration.GetSection("Info"));
builder.Services.Configure<HotelOptions>(builder.Configuration);
builder.Services.Configure<PagingOptions>(
    builder.Configuration.GetSection("DefaultPagingOptions"));

builder.Services.AddScoped<IRoomService, RoomService>();
builder.Services.AddScoped<IBookingService, BookingService>();
builder.Services.AddScoped<IOpeningService, OpeningService>();
builder.Services.AddScoped<IDateLogicService, DateLogicService>();
builder.Services.AddScoped<IUserService, UserService>();

builder.Services.AddRouting(opt => opt.LowercaseUrls = true); // lower case urls settings in middleware
builder.Services.AddApiVersioning(opt => {
    opt.DefaultApiVersion = new ApiVersion(1, 0); //providing default API version
    opt.ApiVersionReader = new MediaTypeApiVersionReader(); // using media type api reader
    opt.AssumeDefaultVersionWhenUnspecified = true; // it will assume default version when not specified.
    opt.ReportApiVersions = true; // report api version to the browser
    opt.ApiVersionSelector = new CurrentImplementationApiVersionSelector(opt); //defines the behavior of how an API version is selected for a given request context. 
});

builder.Services.AddMvc(opt =>
{
    opt.CacheProfiles.Add("Static", new CacheProfile
    {
        Duration = 86400 // 1 Day
    });
    opt.Filters.Add<JsonExceptionFilter>();
    opt.Filters.Add<RequireHttpsOrCloseAttribute>();
    opt.Filters.Add<LinkRewritingFilter>();
});

builder.Services.AddCors(opt =>
{
    //opt.AddPolicy("AllowMyApp", policy => policy.WithOrigins("https://example.com")); //to add cors policy with specific origins policy.
    opt.AddPolicy("AllowMyApp", policy => policy.AllowAnyOrigin()); //to add cors policy with any origins, use for developers testing.
});

// Use in-memory database for quick dev and testing
// TODO: Swap out for a real database
builder.Services.AddDbContext<HotelApiDbContext>(options =>
{
    options.UseInMemoryDatabase("usadb"); // we might need to install package - Microsoft.EntityFrameworkCore.InMemory
});

//Add ASP.NET Core Identity
builder.Services.AddIdentity<UserEntity, UserRoleEntity>()
    .AddEntityFrameworkStores<HotelApiDbContext>()
    .AddDefaultTokenProviders();
//AddIdentityCoreServices(builder.Services);

builder.Services.AddAutoMapper(opt => opt.AddProfile<MappingProfile>());
builder.Services.Configure<ApiBehaviorOptions>(opt =>
{
    opt.InvalidModelStateResponseFactory = context =>
    {
        var errorResponse = new ApiErrors(context.ModelState);
        return new BadRequestObjectResult(errorResponse);
    };
});

builder.Services.AddResponseCaching();


var app = builder.Build();

SeedDatabase();

// Configure the HTTP request pipeline.
if(app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseResponseCaching();
//app.UseHttpsRedirection();
app.UseCors("AllowMyApp");
app.UseAuthorization();

app.MapControllers();

app.Run();



void SeedDatabase()
{
    using(var scope = app.Services.CreateScope())
    {
        var services = scope.ServiceProvider;
        try
        {
            SeedData.Ininialize(services).Wait();
        }
        catch(Exception ex)
        {
            var logger = services.GetRequiredService<ILogger<Program>>();
            logger.LogError(ex, "Error while seeding the database");
        }
    }
}

//static void AddIdentityCoreServices(IServiceCollection services)
//{
//    var builder = services.AddIdentityCore<UserEntity>();
//    builder = new IdentityBuilder(builder.UserType, typeof(UserRoleEntity), builder.Services);
//    builder.AddRoles<UserRoleEntity>().AddEntityFrameworkStores<HotelApiDbContext>()
//        .AddDefaultTokenProviders()
//        .AddSignInManager<SignInManager<UserEntity>>();
//}
