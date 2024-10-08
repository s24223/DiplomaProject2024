using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Company.OfferPart.DTOs.Update
{
    public class UpdateOfferRequestDto
    {
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public decimal? MinSalary { get; set; }
        public decimal? MaxSalary { get; set; }
        public string? NegotiatedSalary { get; set; }
        public string ForStudents { get; set; } = null!;
    }
}
