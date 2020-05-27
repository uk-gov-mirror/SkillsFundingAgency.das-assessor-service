using MediatR;
using SFA.DAS.AssessorService.Domain.DTOs;

namespace SFA.DAS.AssessorService.Api.Types.Models
{
    public class GetEpaContactByEmailRequest : IRequest<EpaContact>
    {
        public string Email { get; set; }
    }
}