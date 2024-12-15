using Application.Databases.Relational.Models;
using Domain.Shared.ValueObjects;

namespace Application.Features.Users.ExtensionMethods
{
    public static class UserEFExtensions
    {
        //values
        private static readonly string _databaseBoolTrue = new DatabaseBool(true).Code;
        private static readonly string _databaseBoolFalse = new DatabaseBool(false).Code;


        //================================================================================================
        //================================================================================================
        //================================================================================================
        //Public Methods
        public static IQueryable<Url> UrlFilter(
            this IQueryable<Url> query,
            string? searchText = null) //typeId, name
        {
            if (!string.IsNullOrWhiteSpace(searchText))
            {
                var searchTerms = searchText
                    .Split(' ', StringSplitOptions.RemoveEmptyEntries);

                query = query.Where(x =>
                     (x.Name != null && searchTerms.Any(st => x.Name.Contains(st))) ||
                     searchTerms.Any(st => x.Path.Contains(st))
                     );
            }
            return query;
        }

        public static IQueryable<Url> UrlOrderBy(
           this IQueryable<Url> query,
           string orderBy = "created", //typeId, name
           bool ascending = true)
        {
            ///IF CHANGE HERE OBLIGATORY CHANGE IN 
            ///"IOrderByService"
            orderBy = orderBy.Trim().ToLower();
            switch (orderBy)
            {
                case "typeId":
                    query = ascending ?
                        query.OrderBy(x => x.UrlTypeId).ThenBy(x => x.Created) :
                    query.OrderByDescending(x => x.UrlTypeId).ThenByDescending(x => x.Created);
                    break;
                case "path":
                    query = ascending ?
                        query.OrderBy(x => x.Path).ThenBy(x => x.Created) :
                    query.OrderByDescending(x => x.Path).ThenByDescending(x => x.Created);
                    break;
                default:
                    query = ascending ?
                        query.OrderBy(x => x.Created) :
                    query.OrderByDescending(x => x.Created);
                    break;
            };
            return query;
        }

        public static IQueryable<Notification> NotificationFilter(
            this IQueryable<Notification> query,
            string? searchText = null,
            bool? hasReaded = null,
            int? senderId = null,
            int? statusId = null,
            DateTime? createdStart = null,
            DateTime? createdEnd = null,
            DateTime? completedStart = null,
            DateTime? completedEnd = null
            )
        {
            //Swap If Time is invalid
            if (createdStart.HasValue && createdEnd.HasValue && createdStart > createdEnd)
            {
                var start = createdStart.Value;
                createdStart = createdEnd.Value;
                createdEnd = start;
            }
            if (completedStart.HasValue && completedEnd.HasValue && completedStart > completedEnd)
            {
                var start = completedStart.Value;
                completedStart = completedEnd.Value;
                completedEnd = start;
            }

            if (!string.IsNullOrWhiteSpace(searchText))
            {
                var searchTerms = searchText
                    .Split(' ', StringSplitOptions.RemoveEmptyEntries);

                query = query.Where(x =>
                     (x.UserMessage != null && searchTerms.Any(st => x.UserMessage.Contains(st))) ||
                     (x.Response != null && searchTerms.Any(st => x.Response.Contains(st)))
                     );
            }

            if (hasReaded.HasValue)
            {
                query = hasReaded.Value ?
                    query.Where(x => x.IsReadedByUser == _databaseBoolTrue) :
                    query.Where(x => x.IsReadedByUser == _databaseBoolFalse);
            }
            if (senderId.HasValue)
            {
                query = query.Where(x => x.NotificationSenderId == senderId.Value);
            }
            if (statusId.HasValue)
            {
                query = query.Where(x => x.NotificationStatusId == statusId.Value);
            }
            //Time Validation
            if (createdStart.HasValue)
            {
                query = query.Where(x => x.Created >= createdStart.Value);
            }
            if (createdEnd.HasValue)
            {
                query = query.Where(x => x.Created <= createdEnd.Value);
            }
            if (completedStart.HasValue)
            {
                query = query.Where(x => x.Completed >= completedStart.Value);
            }
            if (completedEnd.HasValue)
            {
                query = query.Where(x => x.Completed <= completedEnd.Value);
            }
            return query;
        }

        public static IQueryable<Notification> NotificationOrderBy(
           this IQueryable<Notification> query,
           string orderBy = "created", //completed
            bool ascending = true)
        {
            ///IF CHANGE HERE OBLIGATORY CHANGE IN 
            ///"IOrderByService"
            orderBy = orderBy.Trim().ToLower();
            switch (orderBy)
            {
                case "completed":
                    query = ascending ?
                        query.OrderBy(x => x.Completed) :
                        query.OrderByDescending(x => x.Completed);
                    break;
                default:
                    //Created
                    query = ascending ?
                        query.OrderBy(x => x.Created) :
                        query.OrderByDescending(x => x.Created);
                    break;
            }
            return query;
        }

        //================================================================================================
        //================================================================================================
        //================================================================================================
        //Public Methods
    }
}
