using Application.Features.Internship.InternshipPart.DTOs.Create;
using Application.Features.Internship.InternshipPart.DTOs.Update;
using Application.Shared.DTOs.Response;
using System.Security.Claims;

namespace Application.Features.Internship.InternshipPart.Services
{
    public interface IInternshipService
    {
        //DML
        Task<ResponseItem<CreateInternshipResponseDto>> CreateAsync
            (
            IEnumerable<Claim> claims,
            Guid branchId,
            Guid offerId,
            DateTime created,
            Guid personId,
            CreateInternshipRequestDto dto,
            CancellationToken cancellation
            );

        Task<Response> UpdateAsync
            (
            IEnumerable<Claim> claims,
            Guid idInternship,
            UpdateInternshipRequestDto dto,
            CancellationToken cancellation
            );

        //DQL
    }
}
