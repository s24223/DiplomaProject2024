using Application.Databases.Relational.Models;
using Domain.Features.Company.ValueObjects;
using Domain.Features.Offer.ValueObjects;
using Domain.Features.User.ValueObjects.Identificators;
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

        public static IQueryable<BranchOffer> BranchOfferFilter(
            this IQueryable<BranchOffer> query,
            DateTime? from = null,
            DateTime? to = null)
        {
            if (from.HasValue)
            {
                query = query.Where(x => x.PublishStart >= from.Value);
            }
            if (to.HasValue)
            {
                query = query.Where(x =>
                    x.PublishEnd <= to.Value ||
                    x.PublishEnd == null);
            }
            return query;
        }

        public static IQueryable<BranchOffer> BranchOfferOrderBy(
            this IQueryable<BranchOffer> query,
            string orderBy = "publishstart",
            bool ascending = true)
        {
            orderBy = orderBy.ToLower();
            switch (orderBy)
            {
                case "lastupdate":
                    return ascending ?
                        query
                            .OrderBy(x => x.LastUpdate)
                            .ThenBy(x => x.PublishStart) :
                        query
                            .OrderByDescending(x => x.LastUpdate)
                            .ThenByDescending(x => x.PublishStart);
                case "publishstart":
                    return ascending ?
                        query
                            .OrderBy(x => x.PublishStart)
                            .ThenBy(x => x.LastUpdate) :
                        query
                            .OrderByDescending(x => x.PublishStart)
                            .ThenByDescending(x => x.LastUpdate);

                default:
                    return ascending ?
                        query.OrderBy(x => x.PublishStart) :
                        query.OrderByDescending(x => x.PublishStart);
            }
        }

        public static IQueryable<BranchOffer> Filter(
            this IQueryable<BranchOffer> query,
            IEnumerable<int> characteristics,
            IEnumerable<int> divisionIds,
            int? wojId = null,
            string? streetName = null,//-
            UserId? userId = null,
            string? companyName = null,
            Regon? regon = null,
            string? searchText = null,
            DateTime? publishFrom = null,
            DateTime? publishTo = null,
            DateTime? workFrom = null,
            DateTime? workTo = null,
            Money? minSalary = null,
            Money? maxSalary = null,
            bool? isForStudents = null,
            bool? isNegotiatedSalary = null)
        {
            if (divisionIds.Any())
            {
                query = query.Where(x => divisionIds.Any(y =>
                    x.Branch.Address != null &&
                    x.Branch.Address.Division.Id == y
                ));

                if (streetName != null)
                {
                    var searchTerms = streetName
                    .Split(' ', StringSplitOptions.RemoveEmptyEntries);

                    query = query.Where(x =>
                     x.Branch.Address != null &&
                     x.Branch.Address.Street != null &&
                     searchTerms.Any(st => x.Branch.Address.Street.Name.Contains(st))
                     );
                }
            }
            else
            {
                if (wojId.HasValue)
                {
                    query = query.Where(x =>
                        x.Branch.Address != null &&
                        x.Branch.Address.Division.PathIds != null &&
                        x.Branch.Address.Division.PathIds.Contains($"{wojId.Value}-"));
                }
            }

            if (characteristics.Any())
            {
                query = query.Where(x =>
                    x.Offer.OfferCharacteristics
                    .Any(y => characteristics.Contains(y.CharacteristicId))
                    );
            }

            if (userId != null)
            {
                query = query.Where(x => !x.Recruitments
                    .Where(y => y.BranchOfferId == x.Id).Any()
                    );
            }

            if (!string.IsNullOrWhiteSpace(companyName))
            {
                var searchTerms = companyName
                    .Split(' ', StringSplitOptions.RemoveEmptyEntries);

                query = query.Where(x =>
                     (x.Branch.Company.Name != null &&
                     searchTerms.Any(st => x.Branch.Company.Name.Contains(st)))
                     );
            }
            if (regon != null)
            {
                query = query.Where(x => x.Branch.Company.Regon == regon.Value);
            }
            if (!string.IsNullOrWhiteSpace(searchText))
            {
                var searchTerms = searchText
                    .Split(' ', StringSplitOptions.RemoveEmptyEntries);

                query = query.Where(x =>
                     (x.Branch.Company.Name != null &&
                     searchTerms.Any(st => x.Branch.Company.Name.Contains(st))) ||
                     (x.Branch.Company.Description != null &&
                     searchTerms.Any(st => x.Branch.Company.Description.Contains(st)) ||
                     (x.Branch.Name != null &&
                     searchTerms.Any(st => x.Branch.Name.Contains(st))) ||
                     (x.Branch.Description != null &&
                     searchTerms.Any(st => x.Branch.Description.Contains(st))) ||
                     (x.Offer.Name != null &&
                     searchTerms.Any(st => x.Offer.Name.Contains(st))) ||
                     (x.Offer.Description != null &&
                     searchTerms.Any(st => x.Offer.Description.Contains(st)))
                     ));
            }

            if (publishFrom.HasValue)
            {
                query = query.Where(x => x.PublishStart >= publishFrom.Value);
            }
            if (publishTo.HasValue)
            {
                query = query.Where(x =>
                    x.PublishEnd == null ||
                    (x.PublishEnd != null && x.PublishEnd <= publishTo.Value));
            }
            if (workFrom.HasValue)
            {
                var dateOnly = DateOnly.FromDateTime(workFrom.Value);
                query = query.Where(x =>
                    x.WorkStart != null && x.WorkStart >= dateOnly);
            }
            if (workTo.HasValue)
            {
                var dateOnly = DateOnly.FromDateTime(workTo.Value);
                query = query.Where(x =>
                    x.WorkEnd == null ||
                    (x.WorkEnd != null && x.WorkEnd <= dateOnly));
            }
            if (minSalary != null && minSalary.Value > 0)
            {
                query = query.Where(x =>
                    x.Offer.MinSalary != null &&
                    x.Offer.MinSalary >= minSalary.Value);
            }
            if (maxSalary != null && maxSalary.Value > 0)
            {
                query = query.Where(x =>
                    x.Offer.MaxSalary != null &&
                    x.Offer.MaxSalary <= maxSalary.Value);
            }
            if (isForStudents != null && isForStudents == true)
            {
                query = query.Where(x => x.Offer.IsForStudents == _dbTrue);
            }
            if (isNegotiatedSalary != null && isNegotiatedSalary == true)
            {
                query = query.Where(x =>
                    x.Offer.IsNegotiatedSalary != null &&
                    x.Offer.IsNegotiatedSalary == _dbTrue);
            }
            return query;
        }

        public static IQueryable<BranchOffer> BranchOfferOrderBy(
            this IQueryable<BranchOffer> query,
            IEnumerable<int> characteristics,
            string orderBy = "publishstart",// characteristics
            bool ascending = true)
        {
            orderBy = orderBy.ToLower();

            if (orderBy == "characteristics" && characteristics.Any())
            {
                return query = ascending ?
                    query.Select(x => new
                    {
                        Item = x,
                        MatchCount = x.Offer.OfferCharacteristics
                            .Count(c => characteristics.Contains(c.CharacteristicId))
                    })
                    .OrderBy(x => x.MatchCount)
                    .ThenBy(x => x.Item.PublishStart)
                    .Select(x => x.Item).AsQueryable() :
                    query.Select(x => new
                    {
                        Item = x,
                        MatchCount = x.Offer.OfferCharacteristics
                            .Count(c => characteristics.Contains(c.CharacteristicId))
                    })
                    .OrderByDescending(x => x.MatchCount)
                    .ThenByDescending(x => x.Item.PublishStart)
                    .Select(x => x.Item).AsQueryable();
            }

            switch (orderBy)
            {
                case "maxsalary":
                    return ascending ?
                        query.OrderBy(x => x.Offer.MaxSalary)
                            .ThenBy(x => x.PublishStart) :
                        query.OrderByDescending(x => x.Offer.MaxSalary)
                            .ThenByDescending(x => x.PublishStart);

                case "minsalary":
                    return ascending ?
                        query.OrderBy(x => x.Offer.MinSalary)
                            .ThenBy(x => x.PublishStart) :
                        query.OrderByDescending(x => x.Offer.MinSalary)
                            .ThenByDescending(x => x.PublishStart);

                case "workend":
                    return ascending ?
                        query.OrderBy(x => x.WorkEnd)
                            .ThenBy(x => x.PublishStart) :
                        query.OrderByDescending(x => x.WorkEnd)
                            .ThenByDescending(x => x.PublishStart);

                case "workstart":
                    return ascending ?
                        query.OrderBy(x => x.WorkStart)
                            .ThenBy(x => x.PublishStart) :
                        query.OrderByDescending(x => x.WorkStart)
                            .ThenByDescending(x => x.PublishStart);

                case "publishend":
                    return ascending ?
                        query.OrderBy(x => x.PublishEnd)
                            .ThenBy(x => x.PublishStart) :
                        query.OrderByDescending(x => x.PublishEnd)
                            .ThenByDescending(x => x.PublishStart);

                default:// "publishstart": 
                    return ascending ?
                        query.OrderBy(x => x.PublishStart) :
                        query.OrderByDescending(x => x.PublishStart);

            }
        }


        public static IQueryable<Company> Filter(
            this IQueryable<Company> query,
            IEnumerable<int> characteristics,
            string? companyName = null,
            Regon? regon = null,
            string? searchText = null)
        {
            if (regon != null)
            {
                query = query.Where(x => x.Regon == regon.Value);
            }
            if (!string.IsNullOrWhiteSpace(companyName))
            {
                var searchTerms = companyName
                    .Split(' ', StringSplitOptions.RemoveEmptyEntries);

                query = query.Where(x =>
                     (x.Name != null &&
                     searchTerms.Any(st => x.Name.Contains(st)))
                     );
            }
            if (!string.IsNullOrWhiteSpace(searchText))
            {
                var searchTerms = searchText
                    .Split(' ', StringSplitOptions.RemoveEmptyEntries);

                query = query.Where(x =>
                     (x.Name != null &&
                     searchTerms.Any(st => x.Name.Contains(st))) ||
                     (x.Description != null &&
                     searchTerms.Any(st => x.Description.Contains(st))) ||
                     (x.Branches.Any() &&
                     searchTerms.Any(st =>
                        x.Branches.Any(b => b.Description != null && b.Description.Contains(st)) ||
                        x.Branches.Any(b => b.Name != null && b.Name.Contains(st))
                        ))
                     );
            }
            if (characteristics.Any())
            {
                query = query.Where(c => c.Branches
                    .Any(b => b.BranchOffers
                        .Any(bo => bo.Offer.OfferCharacteristics
                            .Any(och => characteristics.Contains(och.CharacteristicId))
                            )
                        )
                    );
            }
            return query;
        }
    }
}
