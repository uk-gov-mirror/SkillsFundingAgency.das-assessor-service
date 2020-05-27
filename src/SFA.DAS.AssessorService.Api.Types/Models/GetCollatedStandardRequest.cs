using MediatR;
using SFA.DAS.AssessorService.Domain.Entities;

namespace SFA.DAS.AssessorService.Api.Types.Models
{
    public class GetCollatedStandardRequest : IRequest<StandardCollation>
    {
        public int? StandardId;
        public string ReferenceNumber;
    }
}
