using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using API.Entities;
using API.Interfaces;
using Microsoft.IdentityModel.Tokens;

namespace API.Services;

public class TokenService(IConfiguration config) : ITokenService
{
    public string CreateToken(AppUser user)
    {
        var tokenKey = config["TokenKey"] ?? throw new Exception("TokenKey not found in appsettings.json"); // get token key from appsettings.json
        if (tokenKey.Length < 64) throw new Exception("TokenKey must be at least 64 characters long"); // check if token key is at least 64 characters long
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenKey)); // convert token key to bytes and create symmetric security key for encryption

        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.UserName) // add username to claims
        };
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature); // create signing credentials using symmetric security key and HmacSha512Signature algorithm
        var tokenDescriptor = new SecurityTokenDescriptor // create security token descriptor
        {
            Subject = new ClaimsIdentity(claims), // create claims identity
            Expires = DateTime.UtcNow.AddDays(7), // set expiration date to 7 days from now
            SigningCredentials = creds // set signing credentials
        };
        var tokenHandler = new JwtSecurityTokenHandler(); // create jwt security token handler
        var token = tokenHandler.CreateToken(tokenDescriptor); // create token
        return tokenHandler.WriteToken(token); // write token to string and return it

    }
}
