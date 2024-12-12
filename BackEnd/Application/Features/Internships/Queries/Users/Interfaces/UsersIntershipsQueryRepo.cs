using Application.Databases.Relational;
using Application.Databases.Relational.Models;
using Application.Features.Companies.Mappers;
using Application.Features.Internships.ExtensionMethods;
using Application.Features.Internships.Mappers;
using Application.Features.Persons.Mappers;
using Application.Shared.DTOs.Features.Internships;
using Application.Shared.ExtensionMethods;
using Application.Shared.Interfaces.SqlClient;
using Domain.Features.Comment.Entities;
using Domain.Features.Intership.Entities;
using Domain.Features.Intership.Exceptions.Entities;
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
            var items = await PrepareQueryInternshipForPerson(personId)
                .InternshipFilter(searchText, from, to)
                .InternshipOrderBy(orderBy, ascending)
                .Pagination(maxItems, page)
                .ToListAsync(cancellation);
            var totalCount = await PrepareQueryInternshipForPerson(personId)
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


        public async Task<(IEnumerable<(DomainIntership Intership, InternshipDetailsResp Details)> Items,
                        int TotalCount)> GetInternshipsForCompanyAsync(
           UserId companyId,
           CancellationToken cancellation,
           string? searchText = null,
           DateTime? from = null,
           DateTime? to = null,
           string orderBy = "created", // ContractStartDate
           bool ascending = true,
           int maxItems = 100,
           int page = 1)
        {
            var items = await PrepareQueryInternshipForCompany(companyId)
                .InternshipFilter(searchText, from, to)
                .InternshipOrderBy(orderBy, ascending)
                .Pagination(maxItems, page)
                .ToListAsync(cancellation);
            var totalCount = await PrepareQueryInternshipForCompany(companyId)
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

                    .Include(x => x.Recruitment)
                    .ThenInclude(x => x.Person)
                    .ThenInclude(x => x.PersonCharacteristics)

                    .AsQueryable();
        }

        private IQueryable<Internship> PrepareQueryInternshipForPerson(UserId personId)
        {
            return PrepareQueryInternship()
                .Where(x => x.Recruitment.PersonId == personId.Value);
        }

        private IQueryable<Internship> PrepareQueryInternshipForCompany(UserId companyId)
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
    }
}