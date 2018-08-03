﻿using System.Collections.Generic;
using System.Threading.Tasks;
using SFA.DAS.AssessorService.Api.Types.Models.AO;

namespace SFA.DAS.AssessorService.Application.Interfaces
{
    public interface IRegisterQueryRepository
    {
        Task<IEnumerable<OrganisationType>> GetOrganisationTypes();
        Task<IEnumerable<DeliveryArea>> GetDeliveryAreas();
        Task<IEnumerable<AssessmentOrganisationSummary>> GetAssessmentOrganisations();
        Task<AssessmentOrganisationDetails> GetAssessmentOrganisation(string organisationId);
        Task<IEnumerable<AssessmentOrganisationDetails>> GetAssessmentOrganisationsByStandardId(int standardId);
        Task<IEnumerable<AssessmentOrganisationAddress>> GetAssessmentOrganisationAddresses(string organisationId);
        Task<IEnumerable<AssessmentOrganisationContact>> GetAssessmentOrganisationContacts(string organisationId);
        Task<AssessmentOrganisationContact> GetPrimaryOrFirstContact(string organisationId);

        Task<IEnumerable<OrganisationStandardSummary>> GetOrganisationStandardByOrganisationId(string organisationId);

        Task<IEnumerable<OrganisationStandardPeriod>> GetOrganisatonStandardPeriodsByOrganisationStandard(string organisationId, int standardId);
    }
}
