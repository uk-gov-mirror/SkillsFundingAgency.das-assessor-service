using MediatR;
using SFA.DAS.AssessorService.Domain.DTOs;

namespace SFA.DAS.AssessorService.Api.Types.Models
{
    public class GetAssessmentOrganisationByEmailRequest : IRequest<AssessmentOrganisationSummary>
    {
        public string Email { get; set; }
    }
}
