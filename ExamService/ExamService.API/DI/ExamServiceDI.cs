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
        #region Db Context
        services.AddDbContext<ExamDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("ExamServiceDbConnection"),
             sql => sql.EnableRetryOnFailure(5, TimeSpan.FromSeconds(10), null)));
        #endregion

        #region Rpositories
        services.AddScoped<ISubjectRepository, SubjectRepository>();
        services.AddScoped<IQuestionsBankRepository, QuestionsBankRepository>();
        services.AddScoped<IExamConfigRepository, ExamConfigRepository>();

        #endregion

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

        #region Simple Validtion Class for the ExamConfig with FluentValidation
        services.AddValidatorsFromAssemblyContaining<CreateExamConfigValidator>();
        #endregion

        return services;
    }
}
