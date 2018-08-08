﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.FileProviders.Physical;
using Microsoft.Extensions.Logging;
using SFA.DAS.AssessorService.Api.Types.Models.AO;
using SFA.DAS.AssessorService.Api.Types.Models.Register;
using SFA.DAS.AssessorService.Application.Api.Middleware;
using SFA.DAS.AssessorService.Application.Api.Properties.Attributes;
using SFA.DAS.AssessorService.Application.Exceptions;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace SFA.DAS.AssessorService.Application.Api.Controllers
{

    [Authorize]
    [Route("api/ao/assessment-organisations")]
    [ValidateBadRequest]
    public class RegisterController : Controller
    {
        private readonly ILogger<RegisterController> _logger;
        private readonly IMediator _mediator;

        public RegisterController(IMediator mediator, ILogger<RegisterController> logger
        )
        {
            _mediator = mediator;
            _logger = logger;
        }


        [HttpPost(Name = "CreateEpaOrganisation")]
        [SwaggerResponse((int)HttpStatusCode.OK, Type = typeof(EpaOrganisation))]
        [SwaggerResponse((int)HttpStatusCode.Conflict, Type = typeof(AlreadyExistsException))]
        [SwaggerResponse((int)HttpStatusCode.BadRequest, typeof(IDictionary<string, string>))]
        [SwaggerResponse((int)HttpStatusCode.InternalServerError, Type = typeof(ApiResponse))]
        public async Task<IActionResult> CreateOrganisation([FromBody] CreateEpaOrganisationRequest request)
        {
            try
            {
                var result = await _mediator.Send(request);

                return Ok(result);
            }
            
            catch (AlreadyExistsException ex)
            {
                return Conflict(ex.Message);
            }
            catch (BadRequestException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest();
            }

        }
    }
}
