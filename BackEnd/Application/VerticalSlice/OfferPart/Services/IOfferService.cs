using Application.VerticalSlice.OfferPart.DTOs.CreateProfile;
using Domain.Entities.CompanyPart;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.VerticalSlice.OfferPart.Services
{
    public interface IOfferService
    {
        Task CreateOfferProfileAsync
             (
             CreateOfferRequestDto dto,
             CancellationToken cancellation
             );
    }
}
