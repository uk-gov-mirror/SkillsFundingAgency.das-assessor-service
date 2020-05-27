﻿using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using SFA.DAS.AssessorService.Api.Types.Models;
using SFA.DAS.AssessorService.Application.Interfaces;
using SFA.DAS.AssessorService.Domain.DTOs;

namespace SFA.DAS.AssessorService.Application.Handlers.ao
{
    public class GetAssessmentOrganisationByIdHandler : IRequestHandler<GetAssessmentOrganisationByIdRequest, EpaOrganisation>
    {
        private readonly IRegisterQueryRepository _registerQueryRepository;
        private readonly ILogger<GetAssessmentOrganisationByIdHandler> _logger;

        public GetAssessmentOrganisationByIdHandler(IRegisterQueryRepository registerQueryRepository, ILogger<GetAssessmentOrganisationByIdHandler> logger)
        {
            _registerQueryRepository = registerQueryRepository;
            _logger = logger;
        }

        public async Task<EpaOrganisation> Handle(GetAssessmentOrganisationByIdRequest request, CancellationToken cancellationToken)
        {
            _logger.LogInformation($@"Handling AssessmentOrganisation Request for [{request.Id}]");
            var org = await _registerQueryRepository.GetEpaOrganisationById(request.Id);

            return org ?? null;
        }  
    }
}
