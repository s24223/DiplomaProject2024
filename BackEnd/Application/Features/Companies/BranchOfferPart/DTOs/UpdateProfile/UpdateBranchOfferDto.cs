using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Companies.BranchOfferPart.DTOs.UpdateProfile
{
    internal class UpdateBranchOfferDto
    {
        public DateTime PublishStart { get; set; }
        public DateTime? PublishEnd { get; set; }
        public DateOnly? WorkStart { get; set; }
        public DateOnly? WorkEnd { get; set; }
        public DateTime Created { get; } = DateTime.Now;
        public DateTime LastUpdate { get; } = DateTime.Now;
    }
}
