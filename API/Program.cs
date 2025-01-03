using API.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

builder.Services.AddDbContext<DataContext>(opt =>
{
    opt.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")); // configuration file to get connection string from appsettings
});

builder.Services.AddCors();

var app = builder.Build();

// Configure the HTTP request pipeline.
//order here is important !
app.UseCors(x => x.AllowAnyHeader().AllowAnyMethod()
.WithOrigins("http://localhost:4200", "https://localhost:4200")); // cors policy to allow angular to connect to api


app.MapControllers();

app.Run();
