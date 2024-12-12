using Application.Databases.Relational;
using Application.Databases.Relational.Models;
using Application.Features.Companies.Mappers;
using Application.Features.Internships.ExtensionMethods;
using Application.Features.Internships.Mappers;
using Application.Features.Persons.Mappers;
using Application.Shared.DTOs.Features.Internships;
using Application.Shared.ExtensionMethods;
using Application.Shared.Interfaces.SqlClient;
using Domain.Features.BranchOffer.Entities;
using Domain.Features.BranchOffer.Exceptions.Entities;
using Domain.Features.BranchOffer.ValueObjects.Identificators;
using Domain.Features.Comment.Entities;
using Domain.Features.Intership.Entities;
using Domain.Features.Intership.Exceptions.Entities;
using Domain.Features.Recruitment.Entities;
using Domain.Features.Recruitment.ValueObjects.Identificators;
using Domain.Features.User.ValueObjects.Identificators;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace Application.Features.Internships.Queries.Users.Interfaces
{
    public class UsersInternshipsRepo : IUsersInternshipsRepo
    {
        //Values
        private readonly IInternshipMapper _internshipMapper;
        private readonly ICompanyMapper _companyMapper;
        private readonly IPersonMapper _personMapper;
        private readonly DiplomaProjectContext _context;
        private readonly ISqlClientRepo _sql;


        //Constructor
        public UsersInternshipsRepo(
            IInternshipMapper mapper,
            ICompanyMapper companyMapper,
            IPersonMapper personMapper,
            DiplomaProjectContext context,
            ISqlClientRepo sql)
        {
            _internshipMapper = mapper;
            _companyMapper = companyMapper;
            _personMapper = personMapper;
            _context = context;
            _sql = sql;
        }


        //================================================================================================
        //================================================================================================
        //================================================================================================
        //Public Methods
        public async Task<(InternshipDetailsResp Details, DomainIntership Intership, int TotalCount)>
            GetCommentsFirstPageAsync(
            UserId userId,
            RecrutmentId internshipId,
            CancellationToken cancellation,
            string? searchText = null,
            int? commentType = null,
            DateTime? from = null,
            DateTime? to = null,
            string orderBy = "created", // CommentTypeId
            bool ascending = false,
            int maxItems = 100)
        {
            var details = await _sql.GetStatisticDetailsByIntershipAsync(
                internshipId,
                userId,
                cancellation);

            var queryCore = PrepareQueryInternshipForComment(userId, internshipId);

            var internshipDb = await queryCore.FirstOrDefaultAsync(cancellation);
            var commentsDb = await queryCore.SelectMany(x => x.Comments)
                        .CommentsFilter(searchText, commentType, from, to)
                        .CommentsOrderBy(orderBy, ascending)
                        .Pagination(maxItems, 1)
                        .ToListAsync(cancellation);
            var totalCountDb = await queryCore.SelectMany(x => x.Comments)
                        .CommentsFilter(searchText, commentType, from, to)
                        .CommentsOrderBy(orderBy, ascending).CountAsync(cancellation);

            if (internshipDb == null)
            {
                throw new IntershipException(
                    "Impossible Pagination by procedure",
                    Domain.Shared.Templates.Exceptions.DomainExceptionTypeEnum.AppProblem);
            }

            var internship = await MapInternship(internshipDb, cancellation);
            var comments = commentsDb.Select(x => _internshipMapper.DomainComment(x));
            var totalCount = totalCountDb;
            internship.AddComments(comments);
            return (details, internship, totalCount);
        }

        public async Task<IEnumerable<DomainComment>> GetCommentsAsync(
           UserId userId,
           RecrutmentId internshipId,
           CancellationToken cancellation,
           string? searchText = null,
           int? commentType = null,
           DateTime? from = null,
           DateTime? to = null,
           string orderBy = "created", // CommentTypeId
           bool ascending = false,
           int maxItems = 100,
           int page = 1)
        {
            var details = await _sql.GetStatisticDetailsByIntershipAsync(
                internshipId,
                userId,
                cancellation);

            var queryCore = PrepareQueryInternshipForComment(userId, internshipId);

            var dbResponse = await queryCore.SelectMany(x => x.Comments)
                        .CommentsFilter(searchText, commentType)
                        .CommentsOrderBy(orderBy, ascending)
                        .Pagination(maxItems, page)
                        .ToListAsync(cancellation);

            var comments = dbResponse.Select(x => _internshipMapper.DomainComment(x));
            return comments;
        }


        public async Task<(IEnumerable<(DomainIntership Intership, InternshipDetailsResp Details)> Items,
                int TotalCount)> GetInternshipsForPersonAsync(
           UserId personId,
           CancellationToken cancellation,
           string? searchText = null,
           DateTime? from = null,
           DateTime? to = null,
           string orderBy = "created", // ContractStartDate
           bool ascending = true,
           int maxItems = 100,
           int page = 1)
        {
            var items = await PrepareQueryPersonInternships(personId)
                .InternshipFilter(searchText, from, to)
                .InternshipOrderBy(orderBy, ascending)
                .Pagination(maxItems, page)
                .ToListAsync(cancellation);
            var totalCount = await PrepareQueryPersonInternships(personId)
                .InternshipFilter(searchText, from, to)
                .CountAsync(cancellation);

            var tasks = items.Select(async x =>
            {
                var details = await _sql.GetStatisticDetailsByIntershipAsync(
                    new RecrutmentId(x.Recruitment.Id),
                    new UserId(x.Recruitment.PersonId),
                    cancellation);
                return new KeyValuePair<Internship, InternshipDetailsResp>(x, details);
            });
            var result = await Task.WhenAll(tasks);

            var list = new List<(DomainIntership Intership, InternshipDetailsResp Details)>();
            foreach (var item in result)
            {
                var domain = await MapInternshipForPerson(item.Key, cancellation);
                list.Add((domain, item.Value));
            }

            Console.WriteLine(totalCount);
            return (list, totalCount);
        }


        public async Task<(
            IEnumerable<(DomainIntership Intership, InternshipDetailsResp Details)> Items,
            int TotalCount
            )> GetInternshipsForCompanyAsync
            (
               UserId companyId,
               CancellationToken cancellation,
               string? searchText = null,
               DateTime? from = null,
               DateTime? to = null,
               string orderBy = "created", // ContractStartDate
               bool ascending = true,
               int maxItems = 100,
               int page = 1
            )
        {
            var items = await PrepareQueryCompanyInternships(companyId)
                .InternshipFilter(searchText, from, to)
                .InternshipOrderBy(orderBy, ascending)
                .Pagination(maxItems, page)
                .ToListAsync(cancellation);
            var totalCount = await PrepareQueryCompanyInternships(companyId)
                .InternshipFilter(searchText, from, to)
                .CountAsync(cancellation);

            var tasks = items.Select(async x =>
            {
                var details = await _sql.GetStatisticDetailsByIntershipAsync(
                    new RecrutmentId(x.Recruitment.Id),
                    new UserId(x.Recruitment.PersonId),
                    cancellation);
                return new KeyValuePair<Internship, InternshipDetailsResp>(x, details);
            });
            var result = await Task.WhenAll(tasks);

            var list = new List<(DomainIntership Intership, InternshipDetailsResp Details)>();
            foreach (var item in result)
            {
                var domain = await MapInternshipForCompany(item.Key, cancellation);
                list.Add((domain, item.Value));
            }

            Console.WriteLine(totalCount);
            return (list, totalCount);
        }


        public async Task<(IEnumerable<DomainRecruitment> Items, int TotalCount)>
            GetPersonRecruitmentsAsync(
            UserId personId,
            CancellationToken cancellation,
            string? searchText = null,
            DateTime? from = null,
            DateTime? to = null,
            bool filterStatus = false,
            bool? status = null, // true accepted, false denied
            string orderBy = "created", // ContractStartDate
            bool ascending = true,
            int maxItems = 100,
            int page = 1)
        {
            var items = await PrepareQueryPersonRecruitments(personId)
                .RecruitmentFilter(searchText, from, to, filterStatus, status)
                .RecruitmentOrderBy(orderBy, ascending)
                .Pagination(maxItems, page)
                .ToListAsync(cancellation);
            var totalCount = await PrepareQueryPersonRecruitments(personId)
                .RecruitmentFilter(searchText, from, to, filterStatus, status)
                .CountAsync(cancellation);

            var domains = new List<DomainRecruitment>();
            foreach (var item in items)
            {
                var domain = await MapRecruitmentForPerson(item, cancellation);
                domains.Add(domain);
            }
            return (domains, totalCount);
        }

        public async Task<(IEnumerable<DomainRecruitment> Items, int TotalCount)>
            GetCompanyRecruitmentsAsync(
            UserId companyId,
            CancellationToken cancellation,
            string? searchText = null,
            DateTime? from = null,
            DateTime? to = null,
            bool filterStatus = false,
            bool? status = null, // true accepted, false denied
            string orderBy = "created", // ContractStartDate
            bool ascending = true,
            int maxItems = 100,
            int page = 1)
        {
            var items = await PrepareQueryCompanyRecruitments(companyId)
                .RecruitmentFilter(searchText, from, to, filterStatus, status)
                .RecruitmentOrderBy(orderBy, ascending)
                .Pagination(maxItems, page)
                .ToListAsync(cancellation);
            var totalCount = await PrepareQueryPersonRecruitments(companyId)
                .RecruitmentFilter(searchText, from, to, filterStatus, status)
                .CountAsync(cancellation);

            var domains = new List<DomainRecruitment>();
            foreach (var item in items)
            {
                var domain = await MapRecruitmentForCompany(item, cancellation);
                domains.Add(domain);
            }
            return (domains, totalCount);
        }

        public async Task<(DomainBranchOffer BranchOffer, int TotalCount)>
            GetBranchOfferRecruitmentsFirstPageAsync(
            UserId companyId,
            BranchOfferId branchOfferId,
            CancellationToken cancellation,
            string? searchText = null,
            DateTime? from = null,
            DateTime? to = null,
            bool filterStatus = false,
            bool? status = null, // true accepted, false denied
            string orderBy = "created", // ContractStartDate
            bool ascending = true,
            int maxItems = 100)
        {
            var branchOffer = await PrepareQueryBranchOffers()
                .Where(x => x.Id == branchOfferId.Value)
                .FirstOrDefaultAsync(cancellation);

            if (branchOffer == null)
            {
                throw new BranchOfferException(
                    Messages.BranchOffer_Cmd_Ids_NotFound,
                    Domain.Shared.Templates.Exceptions.DomainExceptionTypeEnum.NotFound
                    );
            }
            var items = await PrepareQueryCompanyRecruitments(companyId)
                .Where(x => x.BranchOffer.Id == branchOfferId.Value)
                .RecruitmentFilter(searchText, from, to, filterStatus, status)
                .RecruitmentOrderBy(orderBy, ascending)
                .Pagination(maxItems, 1)
                .ToListAsync(cancellation);
            var totalCount = await PrepareQueryCompanyRecruitments(companyId)
                .Where(x => x.BranchOffer.Id == branchOfferId.Value)
                .RecruitmentFilter(searchText, from, to, filterStatus, status)
                .CountAsync(cancellation);

            var domainBranchOffer = await MapBranchOfferForRecruitment(branchOffer, cancellation);
            var domainRecruitments = await MapRecruitmentsForBranchOffer(items, cancellation);
            domainBranchOffer.AddRecrutments(domainRecruitments);

            return (domainBranchOffer, totalCount);
        }

        public async Task<(IEnumerable<DomainRecruitment> Items, int TotalCount)>
            GetBranchOfferRecruitmentsAsync(
            UserId companyId,
            BranchOfferId branchOfferId,
            CancellationToken cancellation,
            string? searchText = null,
            DateTime? from = null,
            DateTime? to = null,
            bool filterStatus = false,
            bool? status = null, // true accepted, false denied
            string orderBy = "created", // ContractStartDate
            bool ascending = true,
            int maxItems = 100,
            int page = 1)
        {
            var items = await PrepareQueryCompanyRecruitments(companyId)
                .Where(x => x.BranchOffer.Id == branchOfferId.Value)
                .RecruitmentFilter(searchText, from, to, filterStatus, status)
                .RecruitmentOrderBy(orderBy, ascending)
                .Pagination(maxItems, page)
                .ToListAsync(cancellation);
            var totalCount = await PrepareQueryCompanyRecruitments(companyId)
                .Where(x => x.BranchOffer.Id == branchOfferId.Value)
                .RecruitmentFilter(searchText, from, to, filterStatus, status)
                .CountAsync(cancellation);

            var domainRecruitments = await MapRecruitmentsForBranchOffer(items, cancellation);

            return (domainRecruitments, totalCount);
        }


        //================================================================================================
        //================================================================================================
        //================================================================================================
        //Private Methods

        private IQueryable<Internship> PrepareQueryInternship()
        {
            return _context.Internships
                    .Include(x => x.Comments)

                    .Include(x => x.Recruitment)
                    .ThenInclude(x => x.BranchOffer)
                    .ThenInclude(x => x.Branch)
                    .ThenInclude(x => x.Company)

                    .Include(x => x.Recruitment)
                    .ThenInclude(x => x.BranchOffer)
                    .ThenInclude(x => x.Offer)
                    .ThenInclude(x => x.OfferCharacteristics)

                    .Include(x => x.Recruitment)
                    .ThenInclude(x => x.Person)
                    .ThenInclude(x => x.PersonCharacteristics)

                    .AsNoTracking()
                    .AsQueryable();
        }

        private IQueryable<Internship> PrepareQueryPersonInternships(UserId personId)
        {
            return PrepareQueryInternship()
                .Where(x => x.Recruitment.PersonId == personId.Value);
        }

        private IQueryable<Internship> PrepareQueryCompanyInternships(UserId companyId)
        {
            return PrepareQueryInternship()
                .Where(x => x.Recruitment.BranchOffer.Branch.CompanyId == companyId.Value);
        }

        private IQueryable<Internship> PrepareQueryInternshipForComment(
            UserId userId,
            RecrutmentId internshipId)
        {
            return PrepareQueryInternship()
                .Where(x => x.Id == internshipId.Value)
                .Where(x =>
                    x.Recruitment.PersonId == userId.Value ||
                    x.Recruitment.BranchOffer.Branch.CompanyId == userId.Value);
        }

        private async Task<DomainIntership> MapInternship(Internship database, CancellationToken cancellation)
        {
            var internship = _internshipMapper.DomainIntership(database);
            var recruitment = _internshipMapper.DomainRecruitment(database.Recruitment);
            var branchOffer = _companyMapper.DomainBranchOffer(database.Recruitment.BranchOffer);
            var branch = _companyMapper.DomainBranchAsync(database.Recruitment.BranchOffer.Branch, cancellation);
            var offer = _companyMapper.DomainOffer(database.Recruitment.BranchOffer.Offer);
            var company = _companyMapper.DomainCompany(database.Recruitment.BranchOffer.Branch.Company);
            var person = _personMapper.DomainPerson(database.Recruitment.Person, cancellation);
            await Task.WhenAll(branch, person);

            internship.Recrutment = recruitment;
            recruitment.BranchOffer = branchOffer;
            branchOffer.Branch = branch.Result;
            branchOffer.Offer = offer;
            branch.Result.Company = company;
            recruitment.Person = person.Result;

            return internship;
        }

        private async Task<DomainIntership> MapInternshipForPerson(Internship database, CancellationToken cancellation)
        {
            var internship = _internshipMapper.DomainIntership(database);
            var recruitment = _internshipMapper.DomainRecruitment(database.Recruitment);
            var branchOffer = _companyMapper.DomainBranchOffer(database.Recruitment.BranchOffer);
            var branch = await _companyMapper.DomainBranchAsync(database.Recruitment.BranchOffer.Branch, cancellation);
            var offer = _companyMapper.DomainOffer(database.Recruitment.BranchOffer.Offer);
            var company = _companyMapper.DomainCompany(database.Recruitment.BranchOffer.Branch.Company);

            internship.Recrutment = recruitment;
            recruitment.BranchOffer = branchOffer;
            branchOffer.Branch = branch;
            branchOffer.Offer = offer;
            branch.Company = company;

            return internship;
        }

        private async Task<DomainIntership> MapInternshipForCompany(Internship database, CancellationToken cancellation)
        {
            var internship = _internshipMapper.DomainIntership(database);
            var recruitment = _internshipMapper.DomainRecruitment(database.Recruitment);
            var branchOffer = _companyMapper.DomainBranchOffer(database.Recruitment.BranchOffer);
            var branch = _companyMapper.DomainBranchAsync(database.Recruitment.BranchOffer.Branch, cancellation);
            var offer = _companyMapper.DomainOffer(database.Recruitment.BranchOffer.Offer);
            var person = _personMapper.DomainPerson(database.Recruitment.Person, cancellation);
            await Task.WhenAll(branch, person);

            internship.Recrutment = recruitment;
            recruitment.BranchOffer = branchOffer;
            branchOffer.Branch = branch.Result;
            branchOffer.Offer = offer;
            recruitment.Person = person.Result;

            return internship;
        }

        //=============================================================================================
        private IQueryable<Recruitment> PrepareQueryRecruitments()
        {
            return _context.Recruitments

                    .Include(x => x.BranchOffer)
                    .ThenInclude(x => x.Branch)
                    .ThenInclude(x => x.Company)

                    .Include(x => x.BranchOffer)
                    .ThenInclude(x => x.Offer)
                    .ThenInclude(x => x.OfferCharacteristics)

                    .Include(x => x.Internship)
                    .Include(x => x.Person)
                    .ThenInclude(x => x.PersonCharacteristics)

                    .AsNoTracking()
                    .AsQueryable();
        }

        private IQueryable<Recruitment> PrepareQueryPersonRecruitments(UserId personId)
        {
            return PrepareQueryRecruitments()
                .Where(x => x.PersonId == personId.Value);
        }
        private IQueryable<Recruitment> PrepareQueryCompanyRecruitments(UserId companyId)
        {
            return PrepareQueryRecruitments()
                .Where(x => x.BranchOffer.Branch.CompanyId == companyId.Value);
        }

        private async Task<DomainRecruitment> MapRecruitment(
            Recruitment database, CancellationToken cancellation)
        {
            var recruitment = _internshipMapper.DomainRecruitment(database);
            var branchOffer = _companyMapper.DomainBranchOffer(database.BranchOffer);
            var branch = _companyMapper.DomainBranchAsync(database.BranchOffer.Branch, cancellation);
            var offer = _companyMapper.DomainOffer(database.BranchOffer.Offer);
            var company = _companyMapper.DomainCompany(database.BranchOffer.Branch.Company);
            var person = _personMapper.DomainPerson(database.Person, cancellation);
            await Task.WhenAll(branch, person);

            recruitment.BranchOffer = branchOffer;
            branchOffer.Branch = branch.Result;
            branchOffer.Offer = offer;
            branch.Result.Company = company;
            recruitment.Person = person.Result;

            if (database.Internship != null)
            {
                var internship = _internshipMapper.DomainIntership(database.Internship);
                recruitment.Intership = internship;
            }
            return recruitment;
        }

        private async Task<DomainRecruitment> MapRecruitmentForPerson(
            Recruitment database, CancellationToken cancellation)
        {
            var recruitment = _internshipMapper.DomainRecruitment(database);
            var branchOffer = _companyMapper.DomainBranchOffer(database.BranchOffer);
            var branch = await _companyMapper.DomainBranchAsync(database.BranchOffer.Branch, cancellation);
            var offer = _companyMapper.DomainOffer(database.BranchOffer.Offer);
            var company = _companyMapper.DomainCompany(database.BranchOffer.Branch.Company);

            recruitment.BranchOffer = branchOffer;
            branchOffer.Branch = branch;
            branchOffer.Offer = offer;
            branch.Company = company;

            if (database.Internship != null)
            {
                var internship = _internshipMapper.DomainIntership(database.Internship);
                recruitment.Intership = internship;
            }
            return recruitment;
        }

        private async Task<DomainRecruitment> MapRecruitmentForCompany(
            Recruitment database, CancellationToken cancellation)
        {
            var recruitment = _internshipMapper.DomainRecruitment(database);
            var branchOffer = _companyMapper.DomainBranchOffer(database.BranchOffer);
            var branch = _companyMapper.DomainBranchAsync(database.BranchOffer.Branch, cancellation);
            var offer = _companyMapper.DomainOffer(database.BranchOffer.Offer);
            var person = _personMapper.DomainPerson(database.Person, cancellation);
            await Task.WhenAll(branch, person);

            recruitment.BranchOffer = branchOffer;
            branchOffer.Branch = branch.Result;
            branchOffer.Offer = offer;
            recruitment.Person = person.Result;

            if (database.Internship != null)
            {
                var internship = _internshipMapper.DomainIntership(database.Internship);
                recruitment.Intership = internship;
            }
            return recruitment;
        }


        private IQueryable<BranchOffer> PrepareQueryBranchOffers()
        {
            return _context.BranchOffers
                .Include(x => x.Offer)
                .ThenInclude(x => x.OfferCharacteristics)
                .Include(x => x.Branch)
                .AsNoTracking();
        }

        private IQueryable<Recruitment> PrepareQueryPersonRecruitment()
        {
            return _context.Recruitments
                .Include(x => x.Person)
                .ThenInclude(x => x.PersonCharacteristics)
                .Include(x => x.Internship)
                .AsNoTracking();
        }

        private async Task<DomainBranchOffer> MapBranchOfferForRecruitment(
            BranchOffer database, CancellationToken cancellation)
        {
            var branchOffer = _companyMapper.DomainBranchOffer(database);
            var branch = await _companyMapper.DomainBranchAsync(database.Branch, cancellation);
            var offer = _companyMapper.DomainOffer(database.Offer);

            branchOffer.Branch = branch;
            branchOffer.Offer = offer;

            return branchOffer;
        }

        private async Task<IEnumerable<DomainRecruitment>> MapRecruitmentsForBranchOffer(
            IEnumerable<Recruitment> recruitments, CancellationToken cancellation)
        {
            var result = new List<DomainRecruitment>();
            foreach (var database in recruitments)
            {
                var recruitment = _internshipMapper.DomainRecruitment(database);
                var person = await _personMapper.DomainPerson(database.Person, cancellation);

                recruitment.Person = person;

                if (database.Internship != null)
                {
                    var internship = _internshipMapper.DomainIntership(database.Internship);
                    recruitment.Intership = internship;
                }

                result.Add(recruitment);
            }
            return result;
        }

    }
}