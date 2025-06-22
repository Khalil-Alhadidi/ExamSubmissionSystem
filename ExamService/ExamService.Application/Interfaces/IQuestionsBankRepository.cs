using ExamService.Domain.Entities;

namespace ExamService.Application.Interfaces;

public interface IQuestionsBankRepository
{
    Task<List<QuestionsBank>> GetAllAsync();
    Task<QuestionsBank?> GetByIdAsync(Guid id);
    Task<List<QuestionsBank>> GetBySubjectIdAsync(Guid subjectId);
    Task CreateAsync(QuestionsBank question);
    Task UpdateAsync(QuestionsBank question);
    Task DeleteAsync(Guid id);
}
