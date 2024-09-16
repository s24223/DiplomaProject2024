namespace Application.Shared.DTOs.Response
{
    public class ItemsResponse<T> : Response
    {
        public List<T> Items { get; set; } = new List<T>();
        public int Count { get; set; } = 0;
    }
}
