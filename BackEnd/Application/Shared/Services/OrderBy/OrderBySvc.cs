namespace Application.Shared.Services.OrderBy
{
    public class OrderBySvc : IOrderBySvc
    {
        public IEnumerable<string> UserUrls()
        {
            return new[]
            {
                "typeId",
                "path",
                "created",
            };
        }

        public IEnumerable<string> UserNotifications()
        {
            return new[]
            {
                "created",
                "completed",
            };
        }
    }
}
