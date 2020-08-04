using System.Threading.Tasks;
namespace SFA.DAS.AssessorService.Application.Api.Infrastructure
{
    public interface ICharityCommissionApiClient
    {
        Task<AssessorService.Api.Types.CharityCommission.Charity> GetCharity(int charityNumber);
        Task<bool> IsCharityActivelyTrading(int charityNumber);
    }
}
