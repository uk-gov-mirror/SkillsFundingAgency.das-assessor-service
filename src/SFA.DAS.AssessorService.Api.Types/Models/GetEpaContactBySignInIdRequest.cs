using MediatR;
using SFA.DAS.AssessorService.Domain.DTOs;

namespace SFA.DAS.AssessorService.Api.Types.Models
{
    public class GetEpaContactBySignInIdRequest : IRequest<EpaContact>
    {
        public string SignInId { get; set; }
    }
}