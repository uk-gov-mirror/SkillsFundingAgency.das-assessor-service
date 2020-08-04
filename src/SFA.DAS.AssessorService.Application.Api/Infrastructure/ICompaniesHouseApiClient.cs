using System.Threading.Tasks;

namespace SFA.DAS.AssessorService.Application.Api.Infrastructure
{
    public interface ICompaniesHouseApiClient
    {
        Task<AssessorService.Api.Types.CompaniesHouse.Company> GetCompany(string companyNumber);
        Task<bool> IsCompanyActivelyTrading(string companyNumber);
    }
}
