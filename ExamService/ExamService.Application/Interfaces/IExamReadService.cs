using ExamService.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExamService.Application.Interfaces;

public interface IExamReadService
{
    Task<List<QuestionsBank>> GetQuestionsAsync(Guid subjectId);
    Task<ExamConfig?> GetExamWithQuestionsAsync(Guid examId);
}
