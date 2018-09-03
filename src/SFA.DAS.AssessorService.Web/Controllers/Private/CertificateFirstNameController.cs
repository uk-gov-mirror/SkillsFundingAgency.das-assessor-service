﻿using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SFA.DAS.AssessorService.Application.Api.Client.Clients;
using SFA.DAS.AssessorService.Domain.Consts;
using SFA.DAS.AssessorService.Web.Infrastructure;
using SFA.DAS.AssessorService.Web.ViewModels.Certificate;

namespace SFA.DAS.AssessorService.Web.Controllers
{
    [Authorize]
    [Route("certificate/firstname")]
    public class CertificateFirstNameController : CertificateBaseController
    {
        public CertificateFirstNameController(ILogger<CertificateController> logger, IHttpContextAccessor contextAccessor, ICertificateApiClient certificateApiClient, ISessionService sessionService)
            : base(logger, contextAccessor, certificateApiClient, sessionService)
        { }

        [HttpGet]
        public async Task<IActionResult> FirstName(bool? redirectToCheck = false)
        {
            CertificateFirstNameViewModel vm = new  CertificateFirstNameViewModel();
            return View("~/Views/Certificate/FirstName.cshtml", vm);
            //return await LoadViewModel<CertificateFirstNameViewModel>("~/Views/Certificate/Grade.cshtml");
        }

        [HttpPost(Name = "FirstName")]
        public async Task<IActionResult> FirstName(CertificateGradeViewModel vm)
        {
            return await SaveViewModel(vm,
                returnToIfModelNotValid: "~/Views/Certificate/Grade.cshtml",
                nextAction: RedirectToAction("Option", "CertificateOption"), action: CertificateActions.Grade);
        }
    }
}