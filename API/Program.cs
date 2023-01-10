using Microsoft.EntityFrameworkCore;
using Persistence;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<DataContext>(opt=>{
    opt.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection"));
});

var app = builder.Build();

// Configure the HTTP request pipeline.
//Refer as Middleware:give us the abilty to handle the http request on the way in/out
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

//the using commang is just disposable way to create section, once and when is done it will destory this scope.
using var scope =app.Services.CreateScope(); 
var services=scope.ServiceProvider;

try
{
    var context=services.GetRequiredService<DataContext>();
    await context.Database.MigrateAsync();
    await Seed.SeedData(context);
}
catch (Exception e)
{
    var logger=services.GetRequiredService<ILogger<Program>>();
    logger.LogError(e,"An error occured during migration");
}     

app.Run();
