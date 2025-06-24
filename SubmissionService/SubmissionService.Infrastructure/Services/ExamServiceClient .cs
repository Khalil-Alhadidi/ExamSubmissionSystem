using Shared.Contracts.ExamService;
using System.Net;
using System.Net.Http.Json;

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
        var request = new HttpRequestMessage(HttpMethod.Get, $"/api/v1/exam-configs/public/{examId}");
        request.Headers.Add("X-Internal-Api-Key", CommunicationKey.ApiKey);

        var response = await _httpClient.SendAsync(request);

        if (response.StatusCode == HttpStatusCode.NotFound)
            return null;

        if (!response.IsSuccessStatusCode)
            throw new HttpRequestException($"ExamService returned status {response.StatusCode}");

        return await response.Content.ReadFromJsonAsync<ExamConfigDto>();
    }
}
