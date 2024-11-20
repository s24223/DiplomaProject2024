namespace Application.Shared.Services.OrderBy
{
    public interface IOrderBySvc
    {
        IEnumerable<string> UserUrls();
        IEnumerable<string> UserNotifications();
        IEnumerable<string> CoreOffers();
    }
}
