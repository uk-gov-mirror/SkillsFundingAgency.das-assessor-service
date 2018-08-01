﻿using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SFA.DAS.AssessorService.Application.Api.Client.Clients;
using SFA.DAS.AssessorService.Domain.Consts;
using SFA.DAS.AssessorService.Web.Staff.Infrastructure;
using SFA.DAS.AssessorService.Web.Staff.ViewModels;

namespace SFA.DAS.AssessorService.Web.Staff.Controllers
{
    [Authorize]
    [Route("certificate/grade")]
    public class CertificateGradeController : CertificateBaseController
    {
        public CertificateGradeController(ILogger<CertificateAmmendController> logger,
            IHttpContextAccessor contextAccessor,
            ApiClient apiClient)
            : base(logger, contextAccessor, apiClient)
        { }

        [HttpGet]
        public async Task<IActionResult> Grade(Guid certificateid)
        {
            return await LoadViewModel<CertificateGradeViewModel>(certificateid, "~/Views/CertificateAmmend/Grade.cshtml");
        }

        [HttpPost(Name = "Grade")]
        public async Task<IActionResult> Grade(CertificateGradeViewModel vm)
        {
            return await SaveViewModel(vm,
                returnToIfModelNotValid: "~/Views/CertificateAmmend/Grade.cshtml",
                nextAction: RedirectToAction("Check", "CertificateAmmend"), action: CertificateActions.Grade);
        }
    }
}