using ExamService.Domain.Entities;

namespace ExamService.Application.Interfaces;

public interface ISubjectRepository
{
    Task<List<Subject>> GetAllAsync();
    Task<Subject?> GetByIdAsync(Guid id);
    Task CreateAsync(Subject subject);
    Task UpdateAsync(Subject subject);
    Task DeleteAsync(Guid id);
}
