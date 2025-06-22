using ExamService.Application.DTOs;
using ExamService.Application.DTOs.ExamConfigDtos;
using FluentValidation;

public class CreateExamConfigValidator : AbstractValidator<CreateExamConfigDto>
{
    public CreateExamConfigValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Exam name is required.")
            .MaximumLength(100);

        RuleFor(x => x.TotalQuestions)
            .GreaterThan(0).WithMessage("Total questions must be greater than 0.");

        RuleFor(x => x.QuestionIds)
            .NotNull()
            .Must((dto, list) => list.Count == dto.TotalQuestions)
            .WithMessage("Question count must match TotalQuestions.");

        RuleFor(x => x.EndDate)
            .GreaterThan(x => x.StartDate)
            .WithMessage("EndDate must be after StartDate.");
    }
}
