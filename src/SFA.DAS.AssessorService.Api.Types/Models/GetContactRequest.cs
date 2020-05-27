using MediatR;
using SFA.DAS.AssessorService.Domain.DTOs;

namespace SFA.DAS.AssessorService.Api.Types.Models
{
    public class GetContactRequest: IRequest<AssessmentOrganisationContact>
    {
        public string ContactId;
        
    }
}