using Microsoft.AspNetCore.Http;

namespace Application.Features.Internships.Commands.Recrutments.DTOs
{
    public class CreateRecruitmentReq
    {
        public IFormFile? File { get; init; } = null;
        public string? PersonMessage { get; set; }
    }
}
