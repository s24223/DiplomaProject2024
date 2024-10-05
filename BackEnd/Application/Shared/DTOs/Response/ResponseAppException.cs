namespace Application.Shared.DTOs.Response
{
    public class ResponseAppException : Response
    {
        public required Guid ProblemId { get; set; }
    }
}
