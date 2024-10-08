using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Company.BranchPart.DTOs.CreateProfile
{
    public class CreateBranchProfileRequestDto
    {
        public Guid CompanyId { get; set; }

        public Guid AddressId { get; set; }

        public Guid Id { get; set; }

        public string? UrlSegment { get; set; }

        public string Name { get; set; } = null!;

        public string? Description { get; set; }
    }
}
