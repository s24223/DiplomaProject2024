namespace Application.Shared.DTOs.Response
{
    /// <summary>
    /// Default Success
    /// </summary>
    public class ResponseAppException : Response
    {
        public required Guid ProblemId { get; set; }
    }
}
