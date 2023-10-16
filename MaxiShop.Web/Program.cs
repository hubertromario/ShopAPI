using MaxiShop.InfraStructure;
using MaxiShop.InfraStructure.DbContexts;
using Microsoft.EntityFrameworkCore;
using MaxiShop.Application;
using MaxiShop.InfraStructure.Common;
using MaxiShop.Web.MiddleWare;
using Microsoft.AspNetCore.Identity;
using MaxiShop.Application.Common;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.OpenApi.Models;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Mvc;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container    
builder.Services.AddApplicationServices();
builder.Services.AddInfraStructureServices();
#region CORS Configuration
builder.Services.AddCors(options=>
{
    options.AddPolicy("CustomPolicy",x=>x.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
}) ;
#endregion
#region Database Connectivity
builder.Services.AddDbContext<MaxiShopDbContext>(
    options=>options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
    options.SignIn.RequireConfirmedEmail = false;
    options.User.RequireUniqueEmail = true;
}).AddEntityFrameworkStores<MaxiShopDbContext>();
#endregion
#region Configuration for Seeding Data to Database
static async void UpdateDatabaseAsync(IHost host)
{
    using (var scope = host.Services.CreateScope())
    {
        var services = scope.ServiceProvider;
        try
        {
            var context = services.GetRequiredService<MaxiShopDbContext>();

            if (context.Database.IsSqlServer())
            {
                context.Database.Migrate();
            }

            await SeedData.SeedDataAsync(context);
        }
        catch (Exception ex){
            var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();

            logger.LogError(ex, "An error Occured while migrating or seeding the database");
            
        }
    }
}
#endregion

builder.Services.AddResponseCaching();

builder.Services.AddApiVersioning(options =>
{
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.DefaultApiVersion = new ApiVersion(1, 0);
    options.ReportApiVersions = true;

});

builder.Services.AddVersionedApiExplorer(options =>
{
    options.GroupNameFormat = "'v'VVV";
    options.SubstituteApiVersionInUrl = true;
});
builder.Services.AddControllers(options=>
{
    options.CacheProfiles.Add("Default", new CacheProfile
    {
        Duration = 20
    });
});

builder.Host.UseSerilog((context, config) =>
{
    config.WriteTo.File("Logs/log.txt", rollingInterval: RollingInterval.Day);
    if (context.HostingEnvironment.IsProduction() == false)
    {
        config.WriteTo.Console();
    }
});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

}
    ).AddJwtBearer(options =>
    {
        options.RequireHttpsMetadata = false;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            ValidateIssuer = true,
            ValidateAudience = true,
            ClockSkew = TimeSpan.Zero,
            ValidIssuer = builder.Configuration["JwtSettings:Issuer"],
            ValidAudience = builder.Configuration["JwtSettings:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JwtSettings:Key"]))
         
        };
    });
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Maxishop API version 1",
        Description = "Developed by Romario",
        Version = "v1.0"
    });

    options.SwaggerDoc("v2", new OpenApiInfo
    {
        Title = "Maxishop API version 2",
        Description = "Developed by Romario",
        Version = "v2.0"
    });

    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        Description = @"JWT Authorization header using the bearer scheme.
                        Enter 'Bearer' [space] and then your token in the input below.
                        Example:'Bearer 12345abcdef'",
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                },
                Scheme ="Oauth2",
                Name = "Bearer",
                In = ParameterLocation.Header
            },
            new List<string>()
        }
    });
});

var app = builder.Build();
app.UseMiddleware<ExceptionMiddleware>();

UpdateDatabaseAsync(app);

var serviceProvider = app.Services;
await SeedData.SeedRoles(serviceProvider);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "Maxishop_V1");
        options.SwaggerEndpoint("/swagger/v2/swagger.json", "Maxishop_V2");
    });
}

app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "Maxishop_V1");
    options.SwaggerEndpoint("/swagger/v2/swagger.json", "Maxishop_V2");
    options.RoutePrefix = string.Empty;
});

app.UseCors("CustomPolicy");



app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
