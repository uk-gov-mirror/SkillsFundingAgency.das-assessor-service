using SFA.DAS.AssessorService.Api.Types.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SFA.DAS.AssessorService.Application.Api.Infrastructure
{
    public interface IProviderRegisterApiClient
    {
        Task<IEnumerable<OrganisationSearchResult>> SearchOrganisationByName(string name, bool exactMatch);
        Task<OrganisationSearchResult> SearchOrganisationByUkprn(int ukprn);
    }
}
