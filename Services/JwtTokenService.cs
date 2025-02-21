using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace QlNhanSu_Backend.Services;

public interface IJwtTokenService
{
    public string GenerateJwtToken(string userId, bool isAccess = true);

}
public class JwtTokenService : IJwtTokenService
{

    private readonly IConfiguration _configuration;

    public JwtTokenService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public string GenerateJwtToken(string userId, bool isAccess = true)
    {
        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, userId),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()), // JTI mới cho mỗi token
            new Claim(JwtRegisteredClaimNames.Iat, DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString()), // Thêm thời gian phát hành
        };

        var keyValue = _configuration["Jwt:Key"];

        if (string.IsNullOrEmpty(keyValue))
        {
            Console.WriteLine("Jwt:Key cannot be null or empty");
        }


        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(keyValue));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(Convert.ToDouble(_configuration[isAccess ? "Jwt:ExpireMinutesAccess" : "Jwt:ExpireMinutesRefesh"])),
            signingCredentials: creds);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

}
