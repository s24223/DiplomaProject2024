namespace Application.Shared.Interfaces.Email
{
    public interface IEmailService
    {
        Task SendAsync(string email, string title, string message);
    }
}
