using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Shared.Interfaces;
using Shared.Services;
using System.Security.Claims;
using System.Text;

namespace SubmissionService.API.DI;

public static class SubmissionServiceInfraDI
{
    public static IServiceCollection AddSubmissionServiceDI(this IServiceCollection services, IConfiguration configuration)
    {

        #region Swagger and OpenAPI
        services.AddOpenApi();
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new() { Title = "SubmissionService API", Version = "v1" });

            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Name = "Authorization",
                Type = SecuritySchemeType.Http,
                Scheme = "bearer",
                BearerFormat = "JWT",
                In = ParameterLocation.Header,
                Description = "Enter your JWT like this: eyJ..."
            });

            c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        Array.Empty<string>()
                    }
                });
                    });
        #endregion

        #region Security (Authentication and Authorization)

        services.AddCors(options =>
        {
            options.AddPolicy("AllowAllOrigins",
                builder => builder.AllowAnyOrigin()
                                  .AllowAnyMethod()
                                  .AllowAnyHeader());
        });

        // To mimic the behavior of a real user, in real world this should be coming from UserService
        services.AddHttpContextAccessor();
        services.AddScoped<ICurrentUserService, CurrentUserService>();



        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
            .AddJwtBearer(options =>
            {
                //options.Authority = "https://your-auth-provider"; // external or future UserService


                options.TokenValidationParameters = new TokenValidationParameters
                {
                    // future UserService
                    ValidateIssuer = false,
                    ValidateAudience = false,

                    ValidateLifetime = true,

                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes("This will be 256 bit secret and it should be 512 if I go with HmacSha512, it must be read from a secure location, this !s for Testing only0_0") // 
            ),

                    NameClaimType = ClaimTypes.NameIdentifier,//"sub"
                    RoleClaimType = ClaimTypes.Role //"role" 
                };
            });


        services.AddAuthorization(options =>
        {
            options.AddPolicy("StudentOnly", policy =>
                policy.RequireRole("user"));
        });

        #endregion

        return services;
    }
}
