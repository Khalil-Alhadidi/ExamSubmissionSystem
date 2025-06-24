using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SubmissionService.API.Endpoints;

public static class DevTokens
{
    public static IEndpointRouteBuilder MapDevTokenEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapPost("/dev-token", () =>
        {

            // so that I can test the cached submissions
            string studentId = Guid.NewGuid().ToString();//"B046912D-D09D-4BCE-B95F-0F0D0FBC358A";
            var claims = new[]
                {
            new Claim(ClaimTypes.NameIdentifier, "user"),
            new Claim(ClaimTypes.Role, "user"),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(ClaimTypes.Name, studentId) // I know this is not ideal, but for testing purposes given that the whole appraoch is to mimic the user service

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
