using System.Collections.Generic;
using MediatR;
using SFA.DAS.AssessorService.Domain.DTOs;

namespace SFA.DAS.AssessorService.Api.Types.Models
{
    public class SearchAssessmentOrganisationsRequest : IRequest<List<AssessmentOrganisationSummary>>
    {
        public string SearchTerm { get; set; }
    }
}