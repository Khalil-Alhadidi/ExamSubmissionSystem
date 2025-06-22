using ExamService.Domain.Entities;

namespace ExamService.Application.DTOs.ExamConfigDtos;

public class ExamWithQuestionsDto
{
    public ExamConfig Exam { get; set; }
    public List<QuestionsBank> Questions { get; set; }
}
