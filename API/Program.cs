using API.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddApplicationServices(builder.Configuration);
builder.Services.AddIdentityServices(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
//order here is important !
app.UseCors(x => x.AllowAnyHeader().AllowAnyMethod()
.WithOrigins("http://localhost:4200", "https://localhost:4200")); // cors policy to allow angular to connect to api

app.UseAuthentication(); // authentication middleware to validate token need to put before authorization middleware
app.UseAuthorization(); // authorization middleware to authorize user
app.MapControllers();

app.Run();
