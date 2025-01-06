using System;
using API.Data;
using API.Interfaces;
using API.Services;
using Microsoft.EntityFrameworkCore;

namespace API.Extensions;

public static class ApplicationServiceExtensions //static aloows use methos with out needing to create a new instance of the class
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration config)
    {
        services.AddControllers();
        services.AddDbContext<DataContext>(opt =>
        {
            opt.UseSqlite(config.GetConnectionString("DefaultConnection")); // configuration file to get connection string from appsettings
        });

        services.AddCors();
        services.AddScoped<ITokenService, TokenService>(); //dependency injection for token service should do this for interface and implement method
        return services;
    }
}
