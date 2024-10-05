namespace Application.Shared.DTOs.Response
{
    public class ResponseItem<T> : Response
    {
        public T? Item { get; set; }
    }
}
