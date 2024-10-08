using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Company.CompanyPart.DTOs.UpdateProfile
{
    internal class UpdateCompanyProfileRequestDto
    {
        public string? UrlSegment { get; set; }
        public string ContactEmail { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string Regon { get; set; } = null!;
        public string? Description { get; set; }
    }
}
