using Application.Databases.Relational.Models;
using Domain.Features.Comment.Exceptions.ValueObjects;
using Domain.Features.Comment.ValueObjects.CommentTypePart;
using Domain.Shared.Templates.Exceptions;

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


        //Comments
        public static IQueryable<Comment> CommentsFilter(
            this IQueryable<Comment> query,
            string? searchText = null,
            int? commentType = null,
            DateTime? from = null,
            DateTime? to = null)
        {
            if (
                commentType.HasValue &&
                !Enum.IsDefined<CommentTypeEnum>((CommentTypeEnum)commentType.Value))
            {
                throw new CommentTypeException(
                    $"{Messages.CommentType_Query_Enum_IdNotFound}: {(int)commentType.Value}",
                    DomainExceptionTypeEnum.NotFound);
            }

            if (!string.IsNullOrWhiteSpace(searchText))
            {
                var searchTerms = searchText
                    .Split(' ', StringSplitOptions.RemoveEmptyEntries);
                query = query.Where(c =>
                    searchTerms.Any(t => c.Description.Contains(t)));
            }

            if (commentType != null)
            {
                query = query.Where(x =>
                    x.CommentTypeId == commentType.Value || x.CommentTypeId == (commentType.Value + 1));
            }

            if (from.HasValue && to.HasValue && from > to)
            {
                var dateTime = from.Value;
                from = to.Value;
                to = dateTime;
            }

            if (from.HasValue)
            {
                query = query.Where(x => x.Created >= from.Value);
            }
            if (to.HasValue)
            {
                query = query.Where(x => x.Created <= to.Value);
            }
            return query;
        }

        public static IQueryable<Comment> CommentsOrderBy(
        this IQueryable<Comment> query,
        string orderBy = "created", // CommentTypeId
        bool ascending = false)
        {
            orderBy = orderBy.ToLower();
            switch (orderBy)
            {
                case "commenttypeid":
                    query = ascending ?
                        query.OrderBy(x => x.CommentTypeId) :
                        query.OrderByDescending(x => x.CommentTypeId);
                    break;
                default:
                    query = ascending ?
                        query.OrderBy(x => x.Created) :
                        query.OrderByDescending(x => x.Created);
                    break;
            };
            return query;
        }

    }
}
