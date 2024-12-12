namespace Application.Shared.ExtensionMethods
{
    public static class EFExtensionMethods
    {
        public static IQueryable<T> Pagination<T>(
            this IQueryable<T> query,
            int maxItems,
            int page)
        {
            maxItems = maxItems < 10 ? 10 : maxItems;
            maxItems = maxItems > 100 ? 100 : maxItems;
            page = page < 1 ? 1 : page;

            return query.Skip((page - 1) * maxItems).Take(maxItems);
        }
    }
}
