namespace Application.Shared.DTOs.Response
{
    public class ItemResponse<T> : Response
    {
        public T? Item { get; set; }
    }
}
