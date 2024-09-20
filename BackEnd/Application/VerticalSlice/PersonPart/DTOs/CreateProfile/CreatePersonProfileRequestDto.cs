using Application.Database.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.VerticalSlice.PersonPart.DTOs.CreateProfile
{
    public class CreatePersonProfileRequestDto
    {

        public string? UrlSegment { get; set; }

        public string ContactEmail { get; set; } = null!;

        public string Name { get; set; } = null!;

        public string Surname { get; set; } = null!;

        public DateOnly? BirthDate { get; set; }

        public string? ContactPhoneNum { get; set; }

        public string? Description { get; set; }

        public string IsStudent { get; set; } = null!;

        public string IsPublicProfile { get; set; } = null!;

        public Guid? AddressId { get; set; }
       
    }
}
