namespace Application.Shared.DTOs.Response
{
    /// <summary>
    /// Default Success
    /// </summary>
    public class ResponseItems<T> : Response
    {
        private List<T> _items = new List<T>();
        public List<T> Items
        {
            get
            {
                return _items;
            }
            set
            {
                Count = value.Count;
                _items = value;
            }
        }
        public int Count { get; private set; } = 0;
        public int TotalCount { get; set; } = 0;
    }
}
