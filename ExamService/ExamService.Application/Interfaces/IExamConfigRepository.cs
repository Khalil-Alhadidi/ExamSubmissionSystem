using ExamService.Application.DTOs.ExamConfigDtos;
using ExamService.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExamService.Application.Interfaces;

public interface IExamConfigRepository
{
    Task<List<ExamConfig>> GetAllAsync();
    Task<ExamConfig?> GetByIdAsync(Guid id);
    Task CreateAsync(ExamConfig config);
    Task UpdateAsync(ExamConfig config);
    Task DeleteAsync(Guid id);
    Task<ExamWithQuestionsDto?> GetWithQuestionsAsync(Guid examId);

}
