using Application.Databases.Relational;
using Application.Databases.Relational.Models;
using Application.Features.Companies.Mappers;
using Application.Features.Internships.Mappers;
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
        private readonly DiplomaProjectContext _context;
        private readonly ISqlClientRepo _sql;


        //Constructor
        public UsersInternshipsRepo(
            IInternshipMapper mapper,
            ICompanyMapper companyMapper,
            DiplomaProjectContext context,
            ISqlClientRepo sql)
        {
            _internshipMapper = mapper;
            _companyMapper = companyMapper;
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


        //================================================================================================
        //================================================================================================
        //================================================================================================
        //Private Methods
        private IQueryable<Internship> PrepareQueryInternshipForComment(
            UserId userId,
            RecrutmentId internshipId)
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

                .Where(x => x.Id == internshipId.Value)
                .Where(x =>
                    x.Recruitment.PersonId == userId.Value ||
                    x.Recruitment.BranchOffer.Branch.CompanyId == userId.Value)
                .AsQueryable();
        }

        private async Task<DomainIntership> MapInternship(Internship database, CancellationToken cancellation)
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
    }
}