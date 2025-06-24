using ExamService.Application.Interfaces;
using ExamService.Application.UseCases.ExamConfigs.Create;
using ExamService.Application.UseCases.ExamConfigs.Delete;
using ExamService.Application.UseCases.ExamConfigs.Read;
using ExamService.Application.UseCases.ExamConfigs.Update;
using ExamService.Application.UseCases.Questions.Create;
using ExamService.Application.UseCases.Questions.Delete;
using ExamService.Application.UseCases.Questions.Read;
using ExamService.Application.UseCases.Questions.Update;
using ExamService.Application.UseCases.Subjects.Create;
using ExamService.Application.UseCases.Subjects.Delete;
using ExamService.Application.UseCases.Subjects.Read;
using ExamService.Application.UseCases.Subjects.Update;
using ExamService.Infrastructure.Persistence;
using ExamService.Infrastructure.Repositories;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Shared.Interfaces;
using Shared.Services;
using System.Security.Claims;
using System.Text;
using System.Text.Json.Serialization;
namespace ExamService.API.DI;

public static class ExamServiceDI
{
    public static IServiceCollection AddExamServiceDI(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddOpenApi();
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new() { Title = "ExamService API", Version = "v1" });

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




        services.AddCors(options =>
        {
            options.AddPolicy("AllowAllOrigins",
                builder => builder.AllowAnyOrigin()
                                  .AllowAnyMethod()
                                  .AllowAnyHeader());
        });

        // Database Context
        services.AddDbContext<ExamDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("ExamServiceDbConnection"),
             sql => sql.EnableRetryOnFailure(5, TimeSpan.FromSeconds(10), null)));

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
            options.AddPolicy("AdminOnly", policy =>
                policy.RequireRole("admin"));
        });

        // Repositories
        services.AddScoped<ISubjectRepository, SubjectRepository>();
        services.AddScoped<IQuestionsBankRepository, QuestionsBankRepository>();
        services.AddScoped<IExamConfigRepository, ExamConfigRepository>();




        // Handlers

        #region Subject Handlers
        services.AddScoped<CreateSubjectHandler>();
        services.AddScoped<GetSubjectsHandler>();
        services.AddScoped<GetSubjectByIdHandler>();
        services.AddScoped<UpdateSubjectHandler>();
        //services.AddScoped<DeleteSubjectHandler>();
        services.AddScoped<SoftDeleteSubjectHandler>();

        #endregion

        #region Questions hanlders
        services.AddScoped<GetQuestionByIdHandler>();
        services.AddScoped<GetQuestionsHandler>();
        services.AddScoped<CreateQuestionHandler>();
        services.AddScoped<UpdateQuestionHandler>();
        //services.AddScoped<DeleteQuestionHandler>();
        services.AddScoped<SoftDeleteQuestionHandler>();
        services.AddScoped<GetQuestionsBySubjectHandler>();

        #endregion

        #region Exam Config handlers
        services.AddScoped<GetExamConfigsHandler>();
        services.AddScoped<GetExamConfigByIdHandler>();
        services.AddScoped<CreateExamConfigHandler>();
        services.AddScoped<UpdateExamConfigHandler>();
        services.AddScoped<DeleteExamConfigHandler>();
        services.AddScoped<GetExamWithQuestionsHandler>();
        services.AddScoped<GetPublicExamConfigByIdHandler>();

        #endregion

        // Application Services
        services.Configure<JsonOptions>(options =>
        {
            options.SerializerOptions.Converters.Add(new JsonStringEnumConverter());
        });

        services.AddValidatorsFromAssemblyContaining<CreateExamConfigValidator>();




        return services;
    }
}
