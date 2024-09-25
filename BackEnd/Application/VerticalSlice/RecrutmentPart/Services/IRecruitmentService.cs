using Application.Shared.DTOs.Response;
using Application.VerticalSlice.RecrutmentPart.DTOs.CreateProfile;
using Application.VerticalSlice.RecrutmentPart.Interfaces;
using Domain.Factories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.VerticalSlice.RecrutmentPart.Services
{
    public interface IRecruitmentService
    {
        Task<Response> CreateRecruitmentAsync(CreateRecruitmentRequestDto dto, CancellationToken cancellation);
    }
}
