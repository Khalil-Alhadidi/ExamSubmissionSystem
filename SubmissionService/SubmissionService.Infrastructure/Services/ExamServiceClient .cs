using Shared.Contracts.ExamService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace SubmissionService.Infrastructure.Services;

public class ExamServiceClient : IExamServiceClient
{
    private readonly HttpClient _httpClient;

    public ExamServiceClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<ExamConfigDto?> GetExamConfigAsync(Guid examId)
    {
        var response = await _httpClient.GetAsync($"/api/v1/exam-configs/public/{examId}");

        if (!response.IsSuccessStatusCode)
            return null;

        return await response.Content.ReadFromJsonAsync<ExamConfigDto>();
    }
}
