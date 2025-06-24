using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Shared.Contracts.ExamService;
using SubmissionService.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static MassTransit.ValidationResultExtensions;

namespace SubmissionService.Infrastructure.Services;

public class CachedExamServiceClient : ICachedExamServiceClient
{
    private readonly IExamServiceClient _inner;
    private readonly IMemoryCache _cache;
    private readonly ILogger<CachedExamServiceClient> _logger;

    public CachedExamServiceClient(
        IExamServiceClient inner,
        IMemoryCache cache,
        ILogger<CachedExamServiceClient> logger)
    {
        _inner = inner;
        _cache = cache;
        _logger = logger;
    }

    public async Task<ExamConfigResult> GetExamConfigAsync(Guid examId)
    {
        var cacheKey = $"exam-config:{examId}";

        if (_cache.TryGetValue(cacheKey, out ExamConfigDto cached))
        {
            _logger.LogInformation("Cache hit for exam config {ExamId}", examId);
            return new ExamConfigResult
            {
                Status = ExamConfigFetchStatus.Ok,
                Config = cached
            };
        }

        try
        {
            var config = await _inner.GetExamConfigAsync(examId);
            if (config != null)
            {
                _cache.Set(cacheKey, config, TimeSpan.FromMinutes(10));
                _logger.LogInformation("Getting data into the cache for exam config {ExamId}", examId);
                return new ExamConfigResult { Status = ExamConfigFetchStatus.Ok, Config = config };
            }

            return new ExamConfigResult { Status = ExamConfigFetchStatus.NotFound };
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Failed to fetch ExamConfig from ExamService for {ExamId}", examId);
            return new ExamConfigResult { Status = ExamConfigFetchStatus.Unavailable };
        }
    }
}
