using Application.Shared.DTOs.Features.Companies.Responses;
using Domain.Features.Branch.Entities;

namespace Application.Features.Companies.Commands.CompanyBranches.DTOs.CommandsBranch.Update
{
    public class UpdateBranchesResp
    {
        //Values
        public BranchResp? Branch { get; set; } = null!;
        public string? Message { get; set; } = null;
        public bool IsCorrect { get; set; } = true;


        //Constructor
        public UpdateBranchesResp(DomainBranch domain, bool isDuplicate, bool isBeforeDb)
        {
            Branch = new BranchResp(domain);
            if (isDuplicate)
            {
                IsCorrect = false;
                Message = isBeforeDb ?
                    Messages.Branch_Cmd_UrlSegmet_InputDuplicate :
                    Messages.Branch_Cmd_UrlSegmet_DbDuplicate;
            }
        }

        public UpdateBranchesResp(DomainBranch domain)
        {
            Branch = new BranchResp(domain);
        }
    }
}
