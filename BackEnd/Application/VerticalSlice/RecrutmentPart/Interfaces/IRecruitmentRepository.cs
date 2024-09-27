using Domain.Entities.RecrutmentPart;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.VerticalSlice.RecrutmentPart.Interfaces
{
    public interface IRecruitmentRepository
    {
        Task CreateAsync(DomainRecruitment recruitment, CancellationToken cancellation);
    }
}
