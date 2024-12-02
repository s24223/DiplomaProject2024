using Application.Shared.DTOs.Features.Companies.Responses;
using Domain.Features.Branch.Entities;

namespace Application.Features.Companies.Commands.CompanyBranches.DTOs.CommandsBranch.Create
{
    public class CreateBranchesResp
    {
        //Values
        public BranchResp? Branch { get; set; } = null!;
        public string? Message { get; set; } = null;
        public bool IsCorrect { get; set; } = true;


        //Constructor
        public CreateBranchesResp(DomainBranch domain, bool isDuplicate, bool isBeforeDb, bool hasDuplicates)
        {
            Branch = new BranchResp(domain);
            if (isDuplicate)
            {
                IsCorrect = false;
                Message = isBeforeDb ?
                    Messages.Branch_Cmd_UrlSegmet_InputDuplicate :
                    Messages.Branch_Cmd_UrlSegmet_DbDuplicate;
            }

            if (hasDuplicates)
            {
                Branch.Id = Guid.Empty;
            }
        }
    }
}
