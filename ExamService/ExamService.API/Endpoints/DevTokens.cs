using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ExamService.API.Endpoints;

public static class DevTokens
{
    public static IEndpointRouteBuilder MapDevTokenEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapPost("/dev-token", () =>
        {

            
            var claims = new[]
                {
                new Claim(ClaimTypes.NameIdentifier, "admin"),
                new Claim(ClaimTypes.Role, "admin")
            };


            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("This will be 256 bit secret and it should be 512 if I go with HmacSha512, it must be read from a secure location, this !s for Testing only0_0"));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var jwt = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.UtcNow.AddHours(10),
                signingCredentials: creds);

            var token = new JwtSecurityTokenHandler().WriteToken(jwt);
            return Results.Ok(new { token });
        });

        return app;
    }

}
