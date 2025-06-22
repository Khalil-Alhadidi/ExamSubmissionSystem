using ExamService.Application.Interfaces;
using ExamService.Application.UseCases.ExamConfig;
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
using ExamService.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;

namespace ExamService.API;

public static class ExamServiceDI
{
    public static IServiceCollection AddExamServiceDI(this IServiceCollection services,IConfiguration configuration)
    {

        services.AddDbContext<ExamDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("ExamServiceDbConnection")));

        // Repositories
        services.AddScoped<ISubjectRepository, SubjectRepository>();
        services.AddScoped<IQuestionsBankRepository, QuestionsBankRepository>();
        services.AddScoped<IExamReadService, ExamReadService>();

        // Handlers
        services.AddScoped<GetExamWithQuestionsHandler>();



        //Subjects handlers
        services.AddScoped<CreateSubjectHandler>();
        services.AddScoped<GetSubjectsHandler>();
        services.AddScoped<GetSubjectByIdHandler>();
        services.AddScoped<UpdateSubjectHandler>();
        services.AddScoped<DeleteSubjectHandler>();

        // Questions hanlders
        services.AddScoped<GetQuestionByIdHandler>();
        services.AddScoped<GetQuestionsHandler>();
        services.AddScoped<CreateQuestionHandler>();
        services.AddScoped<UpdateQuestionHandler>();
        services.AddScoped<DeleteQuestionHandler>();
        services.AddScoped<GetQuestionsBySubjectHandler>();




        return services;
    }
}
