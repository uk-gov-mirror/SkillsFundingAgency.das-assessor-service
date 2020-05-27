using MediatR;
using SFA.DAS.AssessorService.Domain.DTOs;
using System;

namespace SFA.DAS.AssessorService.Api.Types.Models
{
    public class GetAssessmentOrganisationRequest : IRequest<EpaOrganisation>
    {
        public string OrganisationId { get; set; }
    }

    public class GetAssessmentOrganisationByIdRequest : IRequest<EpaOrganisation>
    {
        public Guid Id { get; set; }
    }
}

