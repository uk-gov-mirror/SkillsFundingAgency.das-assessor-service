using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using AutoMapper;
using MediatR;
using Moq;
using NUnit.Framework;
using SFA.DAS.AssessorService.Api.Types.Models;
using SFA.DAS.AssessorService.Api.Types.Models.AO;
using SFA.DAS.AssessorService.Application.Api.AutoMapperProfiles;
using SFA.DAS.AssessorService.Application.Api.Controllers;
using SFA.DAS.AssessorService.Application.Api.Infrastructure;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.AssessorService.Application.Api.UnitTests.Controllers.OrganisationSearch.OrganisationSearchPagedTests
{
    [TestFixture]
    public class WhenSearchTermIsMatchedEpaoId
    {
        private readonly string _searchTerm = "EPA1234";

        [SetUp]
        public void Initialize()
        {
            Mapper.Reset();
            Mapper.Initialize(cfg => 
            {
                cfg.AddProfile<AssessorServiceOrganisationProfile>(); 
                cfg.AddProfile<AssessorServiceOrganisationAddressProfile>(); 
            });
        }

        [Test, MoqAutoData]
        public async Task ThenInvokeRoatpApiWithUkprn(
            [Frozen] Mock<IMediator> mediator,
            [Frozen] Mock<IRoatpApiClient> roatpApiClient,
            AssessmentOrganisationSummary mediatorResponse,
            OrganisationSearchController sut)
        {
            mediator
                .Setup(m => m.Send(
                    It.IsAny<SearchAssessmentOrganisationsRequest>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<AssessmentOrganisationSummary> { mediatorResponse });

            await sut.OrganisationSearchPaged(_searchTerm, 1, 1);

            roatpApiClient.Verify(c => c.SearchOrganisationByUkprn(It.IsAny<int>()));
        }
    }
}