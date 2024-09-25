using Application.Database.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.VerticalSlice.RecrutmentPart.DTOs.CreateProfile
{
    public class CreateRecruitmentRequestDto
    {
        public Guid PersonId { get; set; }

        public Guid BranchId { get; set; }

        public Guid OfferId { get; set; }

        public DateTime Created { get; set; } = DateTime.Now;

        public DateTime ApplicationDate { get; set; }

        public string? PersonMessage { get; set; }

        public string? CompanyResponse { get; set; }

        public string? AcceptedRejected { get; set; }
    }
}
