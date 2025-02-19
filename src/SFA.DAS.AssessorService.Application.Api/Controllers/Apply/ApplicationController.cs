﻿using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SFA.DAS.AssessorService.Api.Types.Models.Apply;
using SFA.DAS.AssessorService.Application.Api.Middleware;
using SFA.DAS.AssessorService.Application.Api.Properties.Attributes;
using SFA.DAS.AssessorService.Domain.Consts;
using Swashbuckle.AspNetCore.Annotations;

namespace SFA.DAS.AssessorService.Application.Api.Controllers.Apply
{
    [Authorize(Roles = "AssessorServiceInternalAPI")]
    [Route("api/v1/applications")]
    [ValidateBadRequest]
    public class ApplicationController : Controller
    {
        private readonly ILogger<ApplicationController> _logger;
        private readonly IMediator _mediator;

        public ApplicationController(ILogger<ApplicationController> logger, IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }

        [HttpGet("{userId}/organisation-applications", Name = "GetOrganisationApplications")]
        [SwaggerResponse((int)HttpStatusCode.OK, Type = typeof(List<ApplicationResponse>))]
        [SwaggerResponse((int)HttpStatusCode.InternalServerError, Type = typeof(ApiResponse))]
        public async Task<ActionResult<List<ApplicationResponse>>> GetOrganisationApplications(string userId)
        {
            _logger.LogInformation($"Received request to retrieve organisation applications for UserId {userId}");
            return Ok(await _mediator.Send(new GetApplicationsRequest(Guid.Parse(userId), ApplicationTypes.Organisation)));
        }

        [HttpGet("{userId}/standard-applications", Name = "GetStandardApplications")]
        [SwaggerResponse((int)HttpStatusCode.OK, Type = typeof(List<ApplicationResponse>))]
        [SwaggerResponse((int)HttpStatusCode.InternalServerError, Type = typeof(ApiResponse))]
        public async Task<ActionResult<List<ApplicationResponse>>> GetStandardApplications(string userId)
        {
            _logger.LogInformation($"Received request to retrieve standard applications for UserId {userId}");
            return Ok(await _mediator.Send(new GetApplicationsRequest(Guid.Parse(userId), ApplicationTypes.Standard)));
        }

        [HttpGet("{userId}/withdrawal-applications", Name = "GetWithdrawalApplications")]
        [SwaggerResponse((int)HttpStatusCode.OK, Type = typeof(List<ApplicationResponse>))]
        [SwaggerResponse((int)HttpStatusCode.InternalServerError, Type = typeof(ApiResponse))]
        public async Task<ActionResult<List<ApplicationResponse>>> GetWithdrawalApplications(string userId)
        {
            _logger.LogInformation($"Received request to retrieve withdrawal application for UserId {userId}");
            return Ok(await _mediator.Send(new GetApplicationsRequest(Guid.Parse(userId), ApplicationTypes.Withdrawal)));
        }

        [HttpGet("{userId}/organisation-withdrawal-applications", Name = "GetOrganisationWithdrawalApplications")]
        [SwaggerResponse((int)HttpStatusCode.OK, Type = typeof(List<ApplicationResponse>))]
        [SwaggerResponse((int)HttpStatusCode.InternalServerError, Type = typeof(ApiResponse))]
        public async Task<ActionResult<List<ApplicationResponse>>> GetOrganisationWithdrawalApplications(string userId)
        {
            _logger.LogInformation($"Received request to retrieve organisation withdrawal application for UserId {userId}");
            return Ok(await _mediator.Send(new GetApplicationsRequest(Guid.Parse(userId), ApplicationTypes.OrganisationWithdrawal)));
        }

        [HttpGet("{userId}/standard-withdrawal-applications", Name = "GetStandardWithdrawalApplications")]
        [SwaggerResponse((int)HttpStatusCode.OK, Type = typeof(List<ApplicationResponse>))]
        [SwaggerResponse((int)HttpStatusCode.InternalServerError, Type = typeof(ApiResponse))]
        public async Task<ActionResult<List<ApplicationResponse>>> GetStandardWithdrawalApplications(string userId)
        {
            _logger.LogInformation($"Received request to retrieve standard withdrawal applications for UserId {userId}");
            return Ok(await _mediator.Send(new GetApplicationsRequest(Guid.Parse(userId), ApplicationTypes.StandardWithdrawal)));
        }

        [HttpGet("{Id}/application", Name = "GetApplication")]
        [SwaggerResponse((int)HttpStatusCode.OK, Type = typeof(ApplicationResponse))]
        [SwaggerResponse((int)HttpStatusCode.InternalServerError, Type = typeof(ApiResponse))]
        public async Task<ActionResult<ApplicationResponse>> GetApplication(string Id)
        {
            _logger.LogInformation($"Received request to retrieve application for ApplicationId {Id}");
            return Ok(await _mediator.Send(new GetApplicationRequest(Guid.Parse(Id))));
        }

        [HttpPost("createApplication", Name = "CreateApplication")]
        [SwaggerResponse((int)HttpStatusCode.Created, Type = typeof(Guid))]
        [SwaggerResponse((int)HttpStatusCode.BadRequest, Type = typeof(IDictionary<string, string>))]
        [SwaggerResponse((int)HttpStatusCode.InternalServerError, Type = typeof(ApiResponse))]
        public async Task<ActionResult<Guid>> CreateApplication(
            [FromBody] CreateApplicationRequest createApplicationRequest)
        {
            _logger.LogInformation("Received Create Application Request");

            var applicationResponse = await _mediator.Send(createApplicationRequest);

            return CreatedAtRoute("CreateApplication",
                applicationResponse);
        }

        [HttpPost("submitApplicationSequence", Name = "SubmitApplicationSequence")]
        [SwaggerResponse((int)HttpStatusCode.Created, Type = typeof(bool))]
        [SwaggerResponse((int)HttpStatusCode.BadRequest, Type = typeof(IDictionary<string, string>))]
        [SwaggerResponse((int)HttpStatusCode.InternalServerError, Type = typeof(ApiResponse))]
        public async Task<ActionResult<Guid>> SubmitApplicationSequence(
            [FromBody] SubmitApplicationSequenceRequest submitApplicationSequenceRequest)
        {
            _logger.LogInformation("Received Submit Application Sequence Request");

            var response = await _mediator.Send(submitApplicationSequenceRequest);

            return CreatedAtRoute("SubmitApplicationSequence", response);
        }

        [HttpPost("updateStandardData", Name = "UpdateStandardData")]
        [SwaggerResponse((int)HttpStatusCode.OK, Type = typeof(bool))]
        [SwaggerResponse((int)HttpStatusCode.BadRequest, Type = typeof(IDictionary<string, string>))]
        [SwaggerResponse((int)HttpStatusCode.InternalServerError, Type = typeof(ApiResponse))]
        public async Task<ActionResult> UpdateStandardData(
            [FromBody] UpdateStandardDataRequest updateStandardDataRequest)
        {
            _logger.LogInformation("Received Update Application Standard Request");

            return Ok(await _mediator.Send(updateStandardDataRequest));
        }

        [HttpPost("resetApplicationToStage1", Name = "ResetApplicationToStage1")]
        [SwaggerResponse((int)HttpStatusCode.OK, Type = typeof(bool))]
        [SwaggerResponse((int)HttpStatusCode.BadRequest, Type = typeof(IDictionary<string, string>))]
        [SwaggerResponse((int)HttpStatusCode.InternalServerError, Type = typeof(ApiResponse))]
        public async Task<ActionResult> ResetApplicationToStage1(
            [FromBody] ResetApplicationToStage1Request resetApplicationToStage1Request)
        {
            _logger.LogInformation("Received Reset Application To Stage 1 Request");

            return Ok(await _mediator.Send(resetApplicationToStage1Request));
        }
    }
}