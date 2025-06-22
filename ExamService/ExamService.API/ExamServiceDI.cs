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
using Microsoft.AspNetCore.Http.Json;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

namespace ExamService.API;

public static class ExamServiceDI
{
    public static IServiceCollection AddExamServiceDI(this IServiceCollection services,IConfiguration configuration)
    {

        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new() { Title = "ExamService API", Version = "v1" });
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
            options.UseSqlServer(configuration.GetConnectionString("ExamServiceDbConnection")));

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
        services.AddScoped<DeleteSubjectHandler>();

        #endregion


        #region Questions hanlders
        services.AddScoped<GetQuestionByIdHandler>();
        services.AddScoped<GetQuestionsHandler>();
        services.AddScoped<CreateQuestionHandler>();
        services.AddScoped<UpdateQuestionHandler>();
        services.AddScoped<DeleteQuestionHandler>();
        services.AddScoped<GetQuestionsBySubjectHandler>();

        #endregion

        #region Exam Config handlers
        services.AddScoped<GetExamConfigsHandler>();
        services.AddScoped<GetExamConfigByIdHandler>();
        services.AddScoped<CreateExamConfigHandler>();
        services.AddScoped<UpdateExamConfigHandler>();
        services.AddScoped<DeleteExamConfigHandler>();
        services.AddScoped<GetExamWithQuestionsHandler>();

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
