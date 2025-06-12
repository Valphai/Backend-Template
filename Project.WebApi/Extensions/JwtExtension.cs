using Microsoft.IdentityModel.Tokens;
using Project.Application.DataTransferObjects;
using Project.Domain.Security;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Project.WebApi.Extensions;

public static class JwtExtension
{
    public static string Generate(UserResponseDTO data)
    {
        var handler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(Configuration.Secrets.JwtPrivateKey);
        var credentials = new SigningCredentials(new SymmetricSecurityKey(key),
            SecurityAlgorithms.HmacSha256Signature);

        var tokenDescriptor = new SecurityTokenDescriptor {
            Subject = GenerateClaims(data),
            Expires = DateTime.UtcNow.AddHours(2),
            SigningCredentials = credentials,
        };
        
        var token = handler.CreateToken(tokenDescriptor);
        return handler.WriteToken(token);
    }

    private static ClaimsIdentity GenerateClaims(UserResponseDTO user)
    {
        var claims = new ClaimsIdentity();
        
        claims.AddClaim(new Claim(ClaimTypes.Email, user.Email));
        foreach (var role in user.Roles)
            claims.AddClaim(new Claim(ClaimTypes.Role, role.Name));

        return claims;
    }
}