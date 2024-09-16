namespace Application.Shared.DTOs.Response
{
    public class Response
    {
        public bool IsSuccess { get; set; }
        public bool IsUserFault { get; set; }
        public bool IsServerFault { get; set; }
        public string MessageForUser { get; set; } = null!;
        public string MessageForAdmin { get; set; } = null!;
    }
}
