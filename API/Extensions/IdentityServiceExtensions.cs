using System;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace API.Extensions;

public static class IdentityServiceExtensions
{
    public static IServiceCollection AddIdentityServices(this IServiceCollection services, IConfiguration config)
    {
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
   .AddJwtBearer(options =>
   {
       var tokenKey = config["TokenKey"] ?? throw new Exception("TokenKey not found in appsettings.json"); // get token key from appsettings.json

       options.TokenValidationParameters = new TokenValidationParameters // token validation parameters to validate token
       {
           ValidateIssuerSigningKey = true, // set to true to validate issuer signing key if not all keys are approved
           IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenKey)), // convert token key to bytes and create symmetric security key for encryption
           ValidateIssuer = false, // validate issuer , not using it
           ValidateAudience = false // validate audience , not using it
       };
   });
        return services;
    }
}
