using GymManagementSystem.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

public class AuthService
{
    private readonly AppDbContext _db;
    private readonly IConfiguration _configuration;
    private readonly PasswordHasher _hasher;

    public AuthService (AppDbContext db , IConfiguration configuration , PasswordHasher passwordHasher)
    {
        _db = db;
        _configuration = configuration;
        _hasher = passwordHasher;
    }

    public async Task <string?>LoginAsync(string  username, string password)
    {
        //if Not Mathced , THEN return NULL ---> SO WHY NULL and not warning Error ? 
        //the caller can’t tell “no such user” from “wrong password”,
        //so attackers can’t discover which usernames exist.
        var user = await _db.Users.FirstOrDefaultAsync(u => u.UserName == username);
        if (user is null || !_hasher.Verify(password, user.PasswordHash)) 
        return null;


        //from configuration: why: the signing secret never goes in the code or on GitHub.
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:key"]!));




        //expires: +12 hours: why: a stolen token stops working on its own, and 12 hours covers one gym day.
        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.UserName),
            new Claim(ClaimTypes.Role , user.Role.ToString())
        };


        //SignWith
        //... HmacSha256: the server signs the token.
        //Why: if anyone edits the claims, for example to make themselves an owner,
        //the signature no longer matches and the token is rejected.

        var token = new JwtSecurityToken
            (
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddHours(12),
            signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256)

            );

        return new JwtSecurityTokenHandler().WriteToken(token);

    }

}