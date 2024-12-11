using Application.Databases.Relational;
using Application.Databases.Relational.Models;
using Application.Features.Internships.Mappers;
using Domain.Features.Comment.Entities;
using Domain.Features.Comment.Exceptions.Entities;
using Domain.Features.Comment.ValueObjects.CommentTypePart;
using Domain.Features.Comment.ValueObjects.Identificators;
using Domain.Features.User.ValueObjects.Identificators;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Internships.Commands.Comments.Interfaces
{
    public class CommentRepo : ICommentRepo
    {
        //Values
        private readonly DiplomaProjectContext _context;
        private readonly IInternshipMapper _mapper;

        //Constructor
        public CommentRepo(
            DiplomaProjectContext context,
            IInternshipMapper mapper
            )
        {
            _context = context;
            _mapper = mapper;
        }


        //=========================================================================================================
        //=========================================================================================================
        //=========================================================================================================
        //Public Methods
        public async Task<CommentSenderEnum> GetSenderRoleAsync(
            Guid intershipId,
            UserId userId,
            CancellationToken cancellation)
        {
            var intership = await _context.Internships
                .Include(x => x.Recruitment)
                .ThenInclude(x => x.BranchOffer)
                .ThenInclude(x => x.Branch)
                .Where(x => x.Id == intershipId)
                .AsNoTracking()
                .FirstOrDefaultAsync(cancellation);

            if (intership == null)
            {
                throw new CommentException(
                    Messages.Comment_Cmd_Intership_NotFound,
                    Domain.Shared.Templates.Exceptions.DomainExceptionTypeEnum.NotFound);
            }

            if (intership.Recruitment.PersonId == userId.Value)
            {
                return CommentSenderEnum.Person;
            }
            else if (intership.Recruitment.BranchOffer.Branch.CompanyId == userId.Value)
            {
                return CommentSenderEnum.Company;
            }
            else
            {
                throw new CommentException(
                    Messages.Comment_Cmd_Intership_NotFound,
                    Domain.Shared.Templates.Exceptions.DomainExceptionTypeEnum.NotFound);
            }
        }

        public async Task<DomainComment> CreateCommentAsync(
            DomainComment domain,
            CancellationToken cancellation)
        {
            try
            {
                var databse = MapComment(domain, null);
                await _context.Comments.AddAsync(databse, cancellation);
                await _context.SaveChangesAsync(cancellation);
                return _mapper.DomainComment(databse);
            }
            catch (System.Exception ex)
            {
                throw HandleException(ex);
            }
        }

        public async Task DeleteAsync(
            CommentId id,
            CancellationToken cancellation)
        {
            try
            {
                var item = await GetCommentAsync(id, cancellation);
                _context.Comments.Remove(item);
                await _context.SaveChangesAsync(cancellation);
            }
            catch (System.Exception ex)
            {
                throw HandleException(ex);
            }
        }

        //=========================================================================================================
        //=========================================================================================================
        //=========================================================================================================
        //Private Methods
        private async Task<Comment> GetCommentAsync(
            CommentId id,
            CancellationToken cancellation)
        {
            var item = await _context.Comments
                    .Where(x =>
                        x.InternshipId == id.IntershipId.Value &&
                        x.CommentTypeId == id.CommentTypeId &&
                        x.Created == id.Created)
                    .FirstOrDefaultAsync(cancellation);

            if (item == null)
            {
                throw new CommentException(
                    Messages.Comment_Cmd_Id_NotFound,
                    Domain.Shared.Templates.Exceptions.DomainExceptionTypeEnum.NotFound);
            }

            return item;
        }

        private Comment MapComment(DomainComment domain, Comment? database)
        {
            var db = database ?? new Comment();
            if (database == null)
            {
                db.InternshipId = domain.Id.IntershipId.Value;
                db.Created = domain.Id.Created;
                db.CommentTypeId = domain.Id.CommentTypeId;
            }
            db.Description = domain.Description;
            db.Evaluation = domain.Evaluation?.Value;
            return db;
        }

        private System.Exception HandleException(System.Exception ex)
        {
            if (ex is DbUpdateException dbEx && dbEx.InnerException is SqlException sqlEx)
            {
                var number = sqlEx.Number;
                var message = sqlEx.Message;

                switch (number)
                {
                    case 50003:
                        return new CommentException(Messages.Comment_Cmd_UnablePublish_EndContractNull);
                    case 50004:
                        return new CommentException($"{Messages.Comment_Cmd_UnablePublish_ContractHasntEnd}: {message}");
                    case 50005:
                        return new CommentException(Messages.Comment_Cmd_UnablePublish_DuplicateEndOpinion);
                    case 50006:
                        return new CommentException(Messages.Comment_Cmd_UnablePublish_DuplicateOfAllovedOfpublication);
                    case 50007:
                        return new CommentException($"{Messages.Comment_Cmd_UnablePublish_AfterDays}: {message}");
                    case 50008:
                        return new CommentException(Messages.Comment_Cmd_UnablePublis_ContractEnd);
                };
            }
            return ex;
        }
    }
}
