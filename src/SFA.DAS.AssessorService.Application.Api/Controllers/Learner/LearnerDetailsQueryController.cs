﻿using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.AssessorService.Application.Api.Properties.Attributes;
using SFA.DAS.AssessorService.Application.Handlers.Staff;

namespace SFA.DAS.AssessorService.Application.Api.Controllers
{
    [Authorize(Roles = "AssessorServiceInternalAPI")]
    [Route("api/v1/learnerdetails")]
    [ValidateBadRequest]
    public class LearnerDetailsQueryController : Controller
    {
        private readonly IMediator _mediator;

        public LearnerDetailsQueryController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> Get(int stdCode, long uln, bool allLogs = false)
        {
            return Ok(await _mediator.Send(new GetLearnerDetailRequest(stdCode, uln, allLogs)));
        }
    }
}