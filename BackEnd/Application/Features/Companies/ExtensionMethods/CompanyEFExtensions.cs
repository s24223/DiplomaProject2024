using Application.Databases.Relational.Models;
using Domain.Shared.ValueObjects;

namespace Application.Features.Companies.ExtensionMethods
{
    public static class CompanyEFExtensions
    {
        //Values
        private static readonly string _dbTrue = new DatabaseBool(true).Code;
        private static readonly string _dbFalse = new DatabaseBool(false).Code;

        //===================================================================================================
        //===================================================================================================
        //===================================================================================================
        //Public Methods
        public static IQueryable<Branch> BranchFilter(
            this IQueryable<Branch> query,
            int? divisionId = null,
            int? streetId = null)
        {
            if (divisionId.HasValue)
            {
                string divIdString = divisionId.Value.ToString();
                query = query.Where(x =>
                x.Address != null &&
                x.Address.Division != null &&
                x.Address.Division.PathIds != null && (
                x.Address.Division.Id == divisionId ||
                x.Address.Division.PathIds.Contains($"-{divIdString}-") ||
                x.Address.Division.PathIds.Contains($"-{divIdString}") ||
                x.Address.Division.PathIds.Contains($"{divIdString}-")
                ));

            }
            if (streetId.HasValue)
            {
                query = query.Where(x =>
                x.Address != null &&
                x.Address.StreetId != null &&
                x.Address.StreetId != streetId);
            }
            return query;
        }

        public static IQueryable<Branch> BranchOrderBy(
            this IQueryable<Branch> query,
            string orderBy = "hierarchy",
            bool ascending = true)
        {
            query = ascending ?
                query.OrderBy(x => x.Address.Division.PathIds)
                .ThenBy(x => x.Address.Division.Name)
                .ThenBy(x => x.Address.Street.Name)
                .ThenBy(x => x.Address.DivisionId) :
                query.OrderByDescending(x => x.Address.Division.PathIds)
                .ThenByDescending(x => x.Address.Division.Name)
                .ThenByDescending(x => x.Address.Street.Name)
                .ThenByDescending(x => x.Address.Division.Id);

            return query;
        }


        //Offer
        public static IQueryable<Offer> OfferFilter(
            this IQueryable<Offer> query,
            IEnumerable<int> characteristics,
            string? searchText = null,
            bool? isNegotiatedSalary = null,
            bool? isForStudents = null,
            decimal? minSalary = null,
            decimal? maxSalary = null)
        {
            if (characteristics.Any())
            {
                query = query.Where(x =>
                    x.OfferCharacteristics
                    .Any(y => characteristics.Contains(y.CharacteristicId))
                    );
            }

            if (!string.IsNullOrWhiteSpace(searchText))
            {
                var searchTerms = searchText
                    .Split(' ', StringSplitOptions.RemoveEmptyEntries);

                query = query.Where(x =>
                     (x.Name != null && searchTerms.Any(st => x.Name.Contains(st))) ||
                     searchTerms.Any(st => x.Description.Contains(st))
                     );
            }
            if (isNegotiatedSalary == true)
            {
                query = query.Where(x => x.IsNegotiatedSalary == _dbTrue);
            }
            if (isForStudents == true)
            {
                query = query.Where(x => x.IsForStudents == _dbTrue);
            }
            if (minSalary.HasValue)
            {
                query = query.Where(x => x.MinSalary >= minSalary);
            }
            if (maxSalary.HasValue)
            {
                query = query.Where(x => x.MinSalary <= maxSalary);
            }
            return query;
        }


        ///IF CHANGE HERE OBLIGATORY CHANGE IN 
        ///"IOrderByService"
        public static IQueryable<Offer> OfferOrderBy(
            this IQueryable<Offer> query,
            IEnumerable<int> characteristics,
            string orderBy = "created",
            bool ascending = true)
        {
            switch (orderBy)
            {
                case "characteristics":
                    if (characteristics.Any())
                    {
                        query = ascending ?
                            query.OrderBy(x => x.OfferCharacteristics
                            .Select(x => x.CharacteristicId)
                                .Intersect(characteristics).Count()
                                )
                                .ThenBy(x => x.MinSalary)
                                .ThenBy(x => x.MaxSalary) :
                            query.OrderByDescending(x => x.OfferCharacteristics
                            .Select(x => x.CharacteristicId)
                                .Intersect(characteristics).Count()
                                )
                                .ThenByDescending(x => x.MinSalary)
                                .ThenByDescending(x => x.MaxSalary);
                    }
                    else
                    {
                        query = ascending ?
                            query.OrderBy(x => x.OfferCharacteristics.Count())
                                .ThenBy(x => x.MinSalary)
                                .ThenBy(x => x.MaxSalary) :
                            query.OrderByDescending(x => x.OfferCharacteristics.Count())
                                .ThenByDescending(x => x.MinSalary)
                                .ThenByDescending(x => x.MaxSalary);
                    }
                    break;
                case "minSalary":
                    query = ascending ?
                        query.OrderBy(x => x.MinSalary)
                            .ThenBy(x => x.MaxSalary) :
                        query.OrderByDescending(x => x.MinSalary)
                            .ThenByDescending(x => x.MaxSalary);
                    break;
                case "maxSalary":
                    query = ascending ?
                        query.OrderBy(x => x.MaxSalary)
                            .ThenBy(x => x.MinSalary) :
                        query.OrderByDescending(x => x.MaxSalary)
                            .ThenByDescending(x => x.MinSalary);
                    break;
                default:
                    //created
                    query = ascending ?
                        query.OrderBy(x => x.BranchOffers
                            .OrderBy(x => x.Created).First().Created) :
                        query.OrderByDescending(x => x.BranchOffers
                            .OrderBy(x => x.Created).First().Created);
                    break;
            };
            return query;
        }
    }
}
