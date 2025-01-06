using System;
using System.Security.Cryptography;
using System.Text;
using API.Data;
using API.DTOs;
using API.Entities;
using API.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers;

public class AccountController(DataContext context, ITokenService tokenService) : BaseApiController
{
    [HttpPost("register")] //account/register
    public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto) // registerDto is the data that is sent to the api
    {
        if (await UserExists(registerDto.Username)) return BadRequest("Username is taken");
        using var hmac = new HMACSHA512(); // using is a using statement that will automatically dispose of hmac when we are done with it
        var user = new AppUser
        {
            UserName = registerDto.Username.ToLower(),
            PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDto.Password)), // Encoding.UTF8.GetBytes(password) converts the password to bytes
            PasswordSalt = hmac.Key // hmac.Key is the salt
        };

        context.Users.Add(user);
        await context.SaveChangesAsync();

        return new UserDto
        {
            Username = user.UserName,
            Token = tokenService.CreateToken(user)
        };

    }
    [HttpPost("login")]
    public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
    {
        var user = await context.Users.FirstOrDefaultAsync(x =>
         x.UserName == loginDto.Username.ToLower());

        if (user == null) return Unauthorized("Invalid Username");

        using var hmac = new HMACSHA512(user.PasswordSalt); // using is a using statement that will automatically dispose of hmac when we are done with it

        var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDto.Password)); // Encoding.UTF8.GetBytes(password) converts the password to bytes

        for (int i = 0; i < computedHash.Length; i++) // going through the computed hash and checking if the password is correct
        {
            if (computedHash[i] != user.PasswordHash[i]) return Unauthorized("Invalid Password"); // if the password is not correct return unauthorized
        }
        return new UserDto
        {
            Username = user.UserName,
            Token = tokenService.CreateToken(user)
        };
    }

    private async Task<bool> UserExists(string username) // check if user exists
    {
        return await context.Users.AnyAsync(x => x.UserName.ToLower() == username.ToLower()); //Bob != bob lower case value check
    }
}
