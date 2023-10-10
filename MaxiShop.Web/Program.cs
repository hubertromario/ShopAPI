using MaxiShop.InfraStructure;
using MaxiShop.InfraStructure.DbContexts;
using Microsoft.EntityFrameworkCore;
using MaxiShop.Application;
using MaxiShop.InfraStructure.Common;
using MaxiShop.Web.MiddleWare;
using Microsoft.AspNetCore.Identity;
using MaxiShop.Application.Common;

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
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
app.UseMiddleware<ExceptionMiddleware>();

UpdateDatabaseAsync(app);

var serviceProvider = app.Services;
await SeedData.SeedRoles(serviceProvider);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("CustomPolicy");



app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
