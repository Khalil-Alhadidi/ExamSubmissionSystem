using Microsoft.Extensions.Logging;
using Shared.Contracts.Events;
using Shared.Contracts.ExamService;
using SubmissionService.Application.DTOs;
using SubmissionService.Application.Interfaces;
using SubmissionService.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SubmissionService.Application.UseCases.Create;

public class SubmitExamHandler
{
    private readonly ISubmissionRepository _repository;
    private readonly IEventPublisher _eventPublisher;
    private readonly ILogger<SubmitExamHandler> _logger;


    public SubmitExamHandler(ISubmissionRepository repository, IEventPublisher eventPublisher)
    {
        _repository = repository;
        _eventPublisher = eventPublisher;
    }

    public async Task<Guid> HandleAsync(SubmitExamRequest request, Guid studentId, Guid examId, List<PublicQuestionDto> configQuestions)
    {
        
        // Check for duplicate submission
        if (await _repository.ExistsAsync(studentId, examId))
            throw new InvalidOperationException("Submission already exists for this exam and student.");

        // Validate that all questions in config are answered and no extra answers are submitted
        var configQuestionIds = configQuestions.Select(q => q.Id).ToHashSet();
        var submittedQuestionIds = request.Answers.Select(a => a.QuestionId).ToHashSet();

        if (!configQuestionIds.SetEquals(submittedQuestionIds))
            throw new InvalidOperationException("Submitted answers do not match the exam questions.");

        // Validate all submitted questions exist in config
        foreach (var answer in request.Answers)
        {
            var configQuestion = configQuestions.FirstOrDefault(q => q.Id == answer.QuestionId);
            if (configQuestion == null)
                throw new InvalidOperationException($"Question not found in exam config: {answer.QuestionId}.");
        }

        var submission = new Submission
        {
            StudentId = studentId,
            ExamId = examId,
            SubmittedAtUtc = DateTimeOffset.UtcNow,
            Answers = request.Answers.Select(a => {
                var configQuestion = configQuestions.First(q => q.Id == a.QuestionId);
                return new Answer
                { 
                    QuestionId = a.QuestionId,
                    QuestionType = configQuestion.Type, // Use type from config
                    AnswerValue = a.AnswerValue
                };
            }).ToList()
        };

        await _repository.AddAsync(submission);
        
        try
        {
            //publish an event 
            var evt = new AnswersSubmittedEvent
            {
                SubmissionId = submission.Id,
                StudentId = submission.StudentId,
                ExamId = submission.ExamId,
                SubmittedAtUtc = submission.SubmittedAtUtc
            };

            await _eventPublisher.PublishAsync(evt);
        }
        catch (Exception ee)
        {
            _logger.LogError(ee, "Failed to publish AnswersSubmittedEvent for submission {SubmissionId}", submission.Id);
        }
        
        await _repository.SaveChanges();


        return submission.Id;
    }
}