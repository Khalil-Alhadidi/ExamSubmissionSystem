namespace Shared.Contracts.ExamService
{
    public class PublicQuestionDto
    {
        public Guid Id { get; set; }
        public string Text { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
    }
}
