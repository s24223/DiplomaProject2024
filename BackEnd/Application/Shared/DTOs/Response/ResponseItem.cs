namespace Application.Shared.DTOs.Response
{
    /// <summary>
    /// Default Success
    /// </summary>
    public class ResponseItem<T> : Response
    {
        public T? Item { get; set; }
    }
}
