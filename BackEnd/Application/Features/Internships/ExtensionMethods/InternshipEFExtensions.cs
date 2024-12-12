using Application.Databases.Relational.Models;
using Domain.Features.Comment.Exceptions.ValueObjects;
using Domain.Features.Comment.ValueObjects.CommentTypePart;
using Domain.Shared.Templates.Exceptions;
using Domain.Shared.ValueObjects;

namespace Application.Features.Internships.ExtensionMethods
{
    public static class InternshipEFExtensions
    {
        //Values 
        private static readonly string _databaseTrue = new DatabaseBool(true).Code;
        private static readonly string _databaseFalse = new DatabaseBool(false).Code;

        //================================================================================================
        //Comments
        public static IQueryable<Comment> CommentsFilter(
            this IQueryable<Comment> query,
            string? searchText = null,
            int? commentType = null,
            DateTime? from = null,
            DateTime? to = null)
        {
            if (commentType.HasValue)
            {
                if (!Enum.IsDefined<CommentTypeEnum>((CommentTypeEnum)commentType.Value))
                {
                    throw new CommentTypeException(
                    $"{Messages.CommentType_Query_Enum_IdNotFound}: {(int)commentType.Value}",
                    DomainExceptionTypeEnum.NotFound);
                }
                query = query.Where(x =>
                    x.CommentTypeId == commentType.Value || x.CommentTypeId == (commentType.Value + 1));
            }

            if (!string.IsNullOrWhiteSpace(searchText))
            {
                var searchTerms = searchText
                    .Split(' ', StringSplitOptions.RemoveEmptyEntries);
                query = query.Where(c =>
                    searchTerms.Any(t => c.Description.Contains(t)));
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

        //================================================================================================
        //Internship
        public static IQueryable<Internship> InternshipFilter(
           this IQueryable<Internship> query,
           string? searchText = null,
           DateTime? from = null,
           DateTime? to = null)
        {
            if (!string.IsNullOrWhiteSpace(searchText))
            {
                var searchTerms = searchText
                    .Split(' ', StringSplitOptions.RemoveEmptyEntries);

                query = query.Where(c =>
                    searchTerms.Any(t => c.ContractNumber.Contains(t)));
            }

            if (from.HasValue && to.HasValue && from > to)
            {
                var dateTime = from.Value;
                from = to.Value;
                to = dateTime;
            }

            if (from.HasValue)
            {
                var date = DateOnly.FromDateTime(from.Value);
                query = query.Where(x => x.ContractStartDate >= date);
            }
            if (to.HasValue)
            {
                var date = DateOnly.FromDateTime(to.Value);
                query = query.Where(x => x.ContractEndDate == null || x.ContractEndDate <= date);
            }

            return query;
        }

        public static IQueryable<Internship> InternshipOrderBy(
          this IQueryable<Internship> query,
          string orderBy = "created", // ContractStartDate
          bool ascending = true)
        {
            orderBy = orderBy.ToLower();
            switch (orderBy)
            {
                case "created":
                    query = ascending ?
                        query.OrderBy(x => x.Created) :
                        query.OrderByDescending(x => x.Created);
                    break;
                case "contractstartdate":
                    query = ascending ?
                        query.OrderBy(x => x.ContractStartDate) :
                        query.OrderByDescending(x => x.ContractStartDate);
                    break;
                default:
                    query = ascending ?
                        query.OrderBy(x => x.ContractNumber)
                            .ThenBy(x => x.ContractStartDate) :
                        query.OrderByDescending(x => x.ContractNumber)
                            .ThenByDescending(x => x.ContractStartDate);
                    break;
            };

            return query;
        }

        //================================================================================================
        //Recruitment
        public static IQueryable<Recruitment> RecruitmentFilter(
            this IQueryable<Recruitment> query,
            string? searchText = null,
            DateTime? from = null,
            DateTime? to = null,
            bool filterStatus = false,
            bool? status = null
            )
        {
            if (!string.IsNullOrWhiteSpace(searchText))
            {
                var searchTerms = searchText
                    .Split(' ', StringSplitOptions.RemoveEmptyEntries);

                query = query.Where(c =>
                    searchTerms.Any(t =>
                    (c.PersonMessage != null && c.PersonMessage.Contains(t)) ||
                    (c.CompanyResponse != null && c.CompanyResponse.Contains(t))
                    ));
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

            if (filterStatus)
            {
                if (status == true)
                {
                    query = query.Where(x => x.IsAccepted == _databaseTrue);
                }
                else if (status == false)
                {
                    query = query.Where(x => x.IsAccepted == _databaseFalse);
                }
                else
                {
                    query = query.Where(x => x.IsAccepted == null);
                }
            }

            return query;
        }

        public static IQueryable<Recruitment> RecruitmentOrderBy(
            this IQueryable<Recruitment> query,
            string orderBy = "created", // isaccepted
            bool ascending = true
            )
        {
            switch (orderBy)
            {
                case "isaccepted":
                    query = ascending ?
                        query.OrderBy(x => x.IsAccepted)
                            .ThenBy(x => x.Created) :
                        query.OrderByDescending(x => x.IsAccepted)
                            .ThenByDescending(x => x.Created);
                    break;
                default:
                    query = ascending ?
                        query.OrderBy(x => x.Created) :
                        query.OrderByDescending(x => x.Created);
                    break;
            }

            return query;
        }

    }
}
