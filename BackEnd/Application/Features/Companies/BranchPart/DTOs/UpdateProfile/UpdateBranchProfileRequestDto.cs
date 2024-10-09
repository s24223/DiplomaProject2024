using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Companies.BranchPart.DTOs.UpdateProfile
{
    public class UpdateBranchProfileRequestDto
    {
        public string? UrlSegment { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
    }
}
