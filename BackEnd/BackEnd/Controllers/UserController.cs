using Application.Features.Companies.Commands.BranchOffers.DTOs.CommandsBranchOffer.CreateBranchOffer.Request;
using Application.Features.Companies.Commands.BranchOffers.DTOs.CommandsBranchOffer.UpdateBranchOffer.Request;
using Application.Features.Companies.Commands.BranchOffers.DTOs.CommandsOffer.CreateOffer.Request;
using Application.Features.Companies.Commands.BranchOffers.DTOs.CommandsOffer.UpdateOffer.Request;
using Application.Features.Companies.Commands.BranchOffers.Services;
using Application.Features.Companies.Commands.CompanyBranches.DTOs.CommandsBranch.Create;
using Application.Features.Companies.Commands.CompanyBranches.DTOs.CommandsBranch.Update;
using Application.Features.Companies.Commands.CompanyBranches.DTOs.CommandsCompany.Create;
using Application.Features.Companies.Commands.CompanyBranches.DTOs.CommandsCompany.Update;
using Application.Features.Companies.Commands.CompanyBranches.Services;
using Application.Features.Companies.Queries.QueriesUser.Services;
using Application.Features.Internships.Commands.Internships.DTOs;
using Application.Features.Internships.Commands.Internships.Services;
using Application.Features.Internships.Commands.Recrutments.DTOs;
using Application.Features.Internships.Commands.Recrutments.Services;
using Application.Features.Persons.Commands.DTOs;
using Application.Features.Persons.Commands.Services;
using Application.Features.Users.Commands.Notifications.DTOs.Create;
using Application.Features.Users.Commands.Notifications.Services;
using Application.Features.Users.Commands.Urls.DTOs;
using Application.Features.Users.Commands.Urls.DTOs.Update;
using Application.Features.Users.Commands.Urls.Services;
using Application.Features.Users.Commands.Users.DTOs.Create;
using Application.Features.Users.Commands.Users.DTOs.LoginIn;
using Application.Features.Users.Commands.Users.DTOs.Refresh;
using Application.Features.Users.Commands.Users.DTOs.UpdateLogin;
using Application.Features.Users.Commands.Users.DTOs.UpdatePassword;
using Application.Features.Users.Commands.Users.Services;
using Application.Features.Users.Queries.QueriesUser.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace BackEnd.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        //Values
        //User Servises
        private readonly IUrlCmdSvc _urlService;
        private readonly IUserCommandService _userService;
        private readonly INotificationCmdSvc _notificationService;
        private readonly IUserQuerySvc _userQueryService;
        //Person Servises
        private readonly IPersonCmdSvc _personService;
        //Company Servises
        private readonly ICompanyBranchCmdSvc _companyBranchService;
        private readonly IBranchOfferCommandService _branchOfferService;
        private readonly IUserCompanySvc _userCompanySvc;
        //Intership Servises
        private readonly IInternshipCmdSvc _internshipService;
        private readonly IRecruitmentCmdSvc _recruitmentService;


        //Cosntructors
        public UserController
            (
            //User Servises
            IUrlCmdSvc urlService,
            IUserCommandService userService,
            INotificationCmdSvc notificationService,
            IUserQuerySvc userQueryService,
            //Person Servises
            IPersonCmdSvc personService,
            //Company Servises
            ICompanyBranchCmdSvc companyBranchService,
            IBranchOfferCommandService branchOfferService,
            IUserCompanySvc userCompanySvc,
            //Intership Servises
            IInternshipCmdSvc internshipService,
            IRecruitmentCmdSvc recruitmentService
            )
        {
            //User Servises
            _urlService = urlService;
            _userService = userService;
            _userQueryService = userQueryService;
            _notificationService = notificationService;
            //Person Servises
            _personService = personService;
            //Company Servises
            _companyBranchService = companyBranchService;
            _branchOfferService = branchOfferService;
            _userCompanySvc = userCompanySvc;
            //Intership Servises
            _internshipService = internshipService;
            _recruitmentService = recruitmentService;
        }


        //================================================================================================================
        //================================================================================================================
        //================================================================================================================
        //Public Methods

        [AllowAnonymous]
        [HttpPost()]
        public async Task<IActionResult> CreateUserProfileAsync
            (
            CreateUserRequestDto dto,
            CancellationToken cancellation
            )
        {
            var result = await _userService.CreateAsync(dto, cancellation);
            return StatusCode(201, result);
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetDataAsync(CancellationToken cancellation)
        {
            var claims = User.Claims.ToList();
            var result = await _userQueryService.GetUserDataAsync(claims, cancellation);
            return Ok(result);
        }


        //================================================================================================================
        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> LoginInAsync
            (
            LoginInRequestDto dto,
            CancellationToken cancellation
            )
        {
            var result = await _userService.LoginInAsync(dto, cancellation);
            return Ok(result.Item);
        }

        [Authorize]
        [HttpPut("login")]
        public async Task<IActionResult> UpdateLoginAsync
            (
            UpdateLoginRequestDto dto,
            CancellationToken cancellation
            )
        {
            var claims = User.Claims.ToList();
            var result = await _userService.UpdateLoginAsync(claims, dto, cancellation);
            return Ok(result);
        }

        //================================================================================================================
        [Authorize]
        [HttpPost("logOut")]
        public async Task<IActionResult> LogOutAsync
            (
            CancellationToken cancellation
            )
        {
            var claims = User.Claims.ToList();
            var result = await _userService.LogOutAsync(claims, cancellation);
            return Ok(result);
        }

        //================================================================================================================
        [AllowAnonymous]
        [HttpPost("refresh")]
        public async Task<IActionResult> RefreshAsync
            (
            RefreshRequestDto dto,
            CancellationToken cancellation
            )
        {
            if (Request.Headers.TryGetValue("Authorization", out var authorizationHeader))
            {
                var jwt = authorizationHeader.ToString().Replace("Bearer ", "");
                var result = await _userService.RefreshTokenAsync(jwt, dto, cancellation);
                return Ok(result.Item);
            }
            return StatusCode(401);
        }

        //================================================================================================================
        [Authorize]
        [HttpPut("password")]
        public async Task<IActionResult> UpdatePasswordAsync
            (
            UpdatePasswordRequestDto dto,
            CancellationToken cancellation
            )
        {
            var claims = User.Claims.ToList();
            var result = await _userService.UpdatePasswordAsync(claims, dto, cancellation);
            return Ok(result);
        }


        //================================================================================================================
        //================================================================================================================
        //UserProblem Part
        [AllowAnonymous]
        [HttpPost("notifications/unauthorized")]
        public async Task<IActionResult> CreateForUnauthorizedAsync
            (
            CreateUnAuthNotificationReq dto,
            CancellationToken cancellation
            )
        {
            var result = await _notificationService.CreateUnauthorizeAsync(dto, cancellation);
            return StatusCode(201, result);
        }

        //================================================================================================================
        [Authorize]
        [HttpPost("notifications/authorized")]
        public async Task<IActionResult> CreateForAuthorizedAsync
            (
            CreateAuthNotificationReq dto,
            CancellationToken cancellation
            )
        {
            var claims = User.Claims.ToList();
            var result = await _notificationService.CreateAuthorizeAsync(claims, dto, cancellation);
            return StatusCode(201, result);
        }

        [Authorize]
        [HttpGet("notifications/authorized")]
        public async Task<IActionResult> GetNotificationsAsync
           (
            CancellationToken cancellation,
            string? searchText = null,
            bool? hasReaded = null,
            int? senderId = null,
            int? statusId = null,
            DateTime? createdStart = null,
            DateTime? createdEnd = null,
            DateTime? completedStart = null,
            DateTime? completedEnd = null,
            string orderBy = "created", //completed
            bool ascending = true,
            int itemsCount = 100,
            int page = 1
           )
        {
            var claims = User.Claims.ToList();
            var result = await _userQueryService.GetNotificationsAsync
                (
                claims,
                cancellation,
                searchText,
                hasReaded,
                senderId,
                statusId,
                createdStart,
                createdEnd,
                completedStart,
                completedEnd,
                orderBy,
                ascending,
                itemsCount,
                page
                );
            return StatusCode(200, result);
        }

        //================================================================================================================
        [Authorize]
        [HttpPut("notifications/authorized/{notificationId:guid}/annul")]
        public async Task<IActionResult> AnnulForAuthorizedAsync
           (
            Guid notificationId,
            CancellationToken cancellation
           )
        {
            var claims = User.Claims.ToList();
            var result = await _notificationService.AnnulAsync(claims, notificationId, cancellation);
            return StatusCode(200, result);
        }

        [Authorize]
        [HttpPut("notifications/authorized/{notificationId:guid}/read")]
        public async Task<IActionResult> ReadForAuthorizedAsync
           (
            Guid notificationId,
            CancellationToken cancellation
           )
        {
            var claims = User.Claims.ToList();
            var result = await _notificationService.ReadAsync(claims, notificationId, cancellation);
            return StatusCode(200, result);
        }

        //================================================================================================================
        //================================================================================================================
        //Url Part
        //DML
        [Authorize]
        [HttpPost("urls")]
        public async Task<IActionResult> CreateAsync
            (
            IEnumerable<CreateUrlReq> dtos,
            CancellationToken cancellation
            )
        {
            var claims = User.Claims.ToList();
            var result = await _urlService.CreateAsync(claims, dtos, cancellation);
            return StatusCode(201, result);
        }

        [Authorize]
        [HttpPut("urls")]
        public async Task<IActionResult> UpdateAsync
            (
            IEnumerable<UpdateUrlReq> dtos,
            CancellationToken cancellation
            )
        {
            var claims = User.Claims.ToList();
            var result = await _urlService.UpdateAsync(claims, dtos, cancellation);
            return StatusCode(200, result);
        }

        [Authorize]
        [HttpDelete("urls")]
        public async Task<IActionResult> DeleteAsync
            (
            IEnumerable<DeleteUrlReq> dtos,
            CancellationToken cancellation
            )
        {
            var claims = User.Claims.ToList();
            var result = await _urlService.DeleteAsync(claims, dtos, cancellation);
            return StatusCode(200, result);
        }

        [Authorize]
        [HttpGet("urls")]
        public async Task<IActionResult> GetUrlsAsync
            (
            CancellationToken cancellation,
            string? searchText = null,
            string orderBy = "created", //typeId, name
            [Required] bool ascending = true,
            int itemsCount = 100,
            int page = 1
            )
        {
            var claims = User.Claims.ToList();
            var result = await _userQueryService
                .GetUrlsAsync(claims, cancellation, searchText, orderBy, ascending, itemsCount, page);
            return StatusCode(200, result);
        }

        //================================================================================================================
        //================================================================================================================
        //Person Part
        [Authorize]
        [HttpPost("person")]
        public async Task<IActionResult> CreateAsync
            (
            CreatePersonReq dto,
            CancellationToken cancellation
            )
        {
            var claims = User.Claims.ToList();
            var result = await _personService.CreateAsync(claims, dto, cancellation);
            return StatusCode(201, result);
        }

        [Authorize]
        [HttpPut("person")]
        public async Task<IActionResult> UpdateAsync
            (
            UpdatePersonReq dto,
            CancellationToken cancellation
            )
        {
            var claims = User.Claims.ToList();
            var result = await _personService.UpdateAsync(claims, dto, cancellation);
            return StatusCode(201, result);
        }


        //================================================================================================================
        //================================================================================================================
        //Company Part
        [Authorize]
        [HttpPost("company")]
        public async Task<IActionResult> CreateCompanyAsync
            (
            CreateCompanyReq dto,
            CancellationToken cancellation
            )
        {
            var claims = User.Claims.ToList();
            var result = await _companyBranchService.CreateAsync(claims, dto, cancellation);
            return StatusCode(201, result);
        }

        [Authorize]
        [HttpPut("company")]
        public async Task<IActionResult> UpdateCompanyAsync
            (
            UpdateCompanyReq dto,
            CancellationToken cancellation
            )
        {
            var claims = User.Claims.ToList();
            var result = await _companyBranchService.UpdateCompanyAsync(claims, dto, cancellation);
            return StatusCode(200, result);
        }

        //================================================================================================================
        [Authorize]
        [HttpPost("company/branches")]
        public async Task<IActionResult> CreateBranchAsync
            (
            IEnumerable<CreateBranchReq> dtos,
            CancellationToken cancellation
            )
        {
            var claims = User.Claims.ToList();
            var result = await _companyBranchService.CreateBranchesAsync(claims, dtos, cancellation);
            return StatusCode(201, result);
        }

        [Authorize]
        [HttpPut("company/branches")]
        public async Task<IActionResult> UpdateBranchAsync
            (
            IEnumerable<UpdateBranchRequestDto> dtos,
            CancellationToken cancellation
            )
        {
            var claims = User.Claims.ToList();
            var result = await _companyBranchService.UpdateBranchesAsync(claims, dtos, cancellation);
            return StatusCode(200, result);
        }

        [Authorize]
        [HttpGet("company/branches")]
        public async Task<IActionResult> UpdateBranchAsync
            (
            CancellationToken cancellation,
            int? divisionId = null,
            int? streetId = null,
            bool ascending = true,
            int itemsCount = 100,
            int page = 1
            )
        {
            var claims = User.Claims.ToList();
            var result = await _userCompanySvc.GetCoreBranchesAsync
                (
                claims,
                cancellation,
                divisionId,
                streetId,
                ascending,
                itemsCount,
                page
                );
            return StatusCode(200, result);
        }
        //================================================================================================================
        [Authorize]
        [HttpPost("company/offers")]
        public async Task<IActionResult> CreateOffersAsync
           (
           IEnumerable<CreateOfferRequestDto> dtos,
           CancellationToken cancellation
           )
        {
            var claims = User.Claims.ToList();
            var result = await _branchOfferService.CreateOffersAsync(claims, dtos, cancellation);
            return StatusCode(201, result);
        }

        [Authorize]
        [HttpPut("company/offers")]
        public async Task<IActionResult> UpdateOffersAsync
            (
            IEnumerable<UpdateOfferRequestDto> dtos,
            CancellationToken cancellation
            )
        {
            var claims = User.Claims.ToList();
            var result = await _branchOfferService.UpdateOffersAsync(claims, dtos, cancellation);
            return StatusCode(200, result);
        }

        [Authorize]
        [HttpGet("company/offers/core")]
        public async Task<IActionResult> UpdateOffersAsync
            (
            [FromHeader]
            IEnumerable<int> characteristics,
            CancellationToken cancellation,
            string? searchText = null,
            bool? isNegotiatedSalary = null,
            bool? isForStudents = null,
            decimal? minSalary = null,
            decimal? maxSalary = null,
            string orderBy = "created",
            bool ascending = true,
            int itemsCount = 100,
            int page = 1
            )
        {
            var claims = User.Claims.ToList();
            var result = await _userCompanySvc.GetCoreOffersAsync
                (
                claims,
                characteristics,
                cancellation,
                searchText,
                isNegotiatedSalary,
                isForStudents,
                minSalary,
                maxSalary,
                orderBy,
                ascending,
                itemsCount,
                page
                );
            return StatusCode(200, result);
        }
        //================================================================================================================
        //branches&offers
        [Authorize]
        [HttpPost("company/branches&offers")]
        public async Task<IActionResult> CreateBranchOfferConnectionsAsync
           (
           IEnumerable<CreateBranchOfferRequestDto> dtos,
           CancellationToken cancellation
           )
        {
            var claims = User.Claims.ToList();
            var result = await _branchOfferService.CreateBranchOffersAsync
                (
                claims,
                dtos,
                cancellation
                );
            return StatusCode(201, result);
        }

        [Authorize]
        [HttpPut("company/branches&offers")]
        public async Task<IActionResult> UpdateBranchOfferConnectionsAsync
            (
            IEnumerable<UpdateBranchOfferRequestDto> dtos,
            CancellationToken cancellation
            )
        {
            var claims = User.Claims.ToList();
            var result = await _branchOfferService.UpdateBranchOffersAsync
                (
                claims,
                dtos,
                cancellation
                );
            return StatusCode(200, result);
        }


        //================================================================================================================
        //Recrutment Part

        [Authorize]
        [HttpPost("recruitment")]
        public async Task<IActionResult> CreateRecruitmentByPersonAsync
            (
            [Required] Guid branchOfferId,
            CreateRecruitmentReq dto,
            CancellationToken cancellation
            )
        {
            var claims = User.Claims.ToList();
            var result = await _recruitmentService.CreateByPersonAsync
                (
                claims,
                branchOfferId,
                dto,
                cancellation
                );
            return StatusCode(201, result);
        }

        [Authorize]
        [HttpPut("recruitment/answer")]
        public async Task<IActionResult> SetAnswerRecruitmentByCompanyAsync
            (
            [Required] Guid id,
            SetAnswerReq dto,
            CancellationToken cancellation
            )
        {
            var claims = User.Claims.ToList();
            var result = await _recruitmentService.SetAnswerByCompanyAsync
                (
                claims,
                id,
                dto,
                cancellation
                );
            return StatusCode(200, result);
        }

        //Intership Part
        [Authorize]
        [HttpPost("internship/")]
        public async Task<IActionResult> CreateInternshipAsync
        (
            [Required] Guid recrutmentId,
            CreateInternshipReq dto,
            CancellationToken cancellation
            )
        {
            var claims = User.Claims.ToList();
            var result = await _internshipService.CreateAsync
                (
                claims,
                recrutmentId,
                dto,
                cancellation
                );
            return StatusCode(201, result);
        }

        [Authorize]
        [HttpPut("internship/{idInternship:guid}")]
        public async Task<IActionResult> UpdateInternshipAsync
            (
            Guid idInternship,
            UpdateInternshipReq dto,
            CancellationToken cancellation
            )
        {
            var claims = User.Claims.ToList();
            var result = await _internshipService.UpdateAsync
                (
                claims,
                idInternship,
                dto,
                cancellation
                );
            return StatusCode(200, result);
        }


        //================================================================================================================
        //================================================================================================================
        //Private Methods
    }
}
