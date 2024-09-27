﻿using Application.Database;
using Application.Database.Models;
using Application.Shared.Exceptions.UserExceptions;
using Domain.Entities.CompanyPart;

using Microsoft.EntityFrameworkCore;


namespace Application.VerticalSlice.OfferPart.Interfaces
{
    public class OfferRepository : IOfferRepository
    {
        private readonly DiplomaProjectContext _context;

        public OfferRepository(DiplomaProjectContext context)
        {
            _context = context;
        }
        public DiplomaProjectContext Get_context()
        {
            return _context;
        }

        public async Task CreateOfferProfileAsync
            (
                DomainOffer offer,
                CancellationToken cancellation
            )
        {
            if (offer != null)
            {
                var databaseOffer = await _context.Offers
                    .Where(x => x.Id == offer.Id.Value).FirstOrDefaultAsync(cancellation);
                if (databaseOffer == null)
                {
                    throw new UnauthorizedUserException();
                }
            }
            await _context.Offers.AddAsync(new Offer
            {
                Name = offer.Name,
                MinSalary = (offer.MinSalary == null) ? null : offer.MinSalary.Value,
                MaxSalary = (offer.MaxSalary == null) ? null : offer.MaxSalary.Value,
                NegotiatedSalary = (offer.IsNegotiatedSalary == null) ? null : offer.IsNegotiatedSalary.Code,
                ForStudents = offer.ForStudents.Code

            }, cancellation);
            await _context.SaveChangesAsync(cancellation);
        }
    }
}