using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Moq;
using NUnit.Framework;
using SFA.DAS.AssessorService.Api.Types.Models;
using SFA.DAS.AssessorService.Application.Api.Controllers;
using SFA.DAS.AssessorService.Application.Api.Infrastructure;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.AssessorService.Application.Api.UnitTests.Controllers.OrganisationSearch.OrganisationSearchPagedTests
{
    [TestFixture]
    public class WhenSearchTermIsUnmatchedEpaoId
    {
        private readonly string _searchTerm = "EPA1234";

        [Test, MoqAutoData]
        public async Task ThenInvokeMediatorToGetAssessmentOrganisationSummaries(
            [Frozen] Mock<IMediator> mediator,
            OrganisationSearchController sut)
        {
            await sut.OrganisationSearchPaged(_searchTerm, 1, 1);

            mediator.Verify(m => m.Send(
                It.Is<SearchAssessmentOrganisationsRequest>(s => s.SearchTerm.Equals(_searchTerm)), 
                It.IsAny<CancellationToken>()));
        }

        [Test, MoqAutoData]
        public async Task ThenReturnEmptyList(
            [Frozen] Mock<IMediator> mediator,
            OrganisationSearchController sut)
        {
            var result = await sut.OrganisationSearchPaged(_searchTerm, 1, 1);
            result.TotalRecordCount.Should().Be(0);
        }

        [Test, MoqAutoData]
        public async Task ThenDoNotInvokeRoatApi(
            [Frozen] Mock<IRoatpApiClient> roatpApiClient,
            OrganisationSearchController sut)
        {
            await sut.OrganisationSearchPaged(_searchTerm, 1, 1);
            roatpApiClient.Verify(r => r.SearchOrganisationByUkprn(It.IsAny<int>()), Times.Never);
            roatpApiClient.Verify(r => r.SearchOrganisationByName(It.IsAny<string>(), false), Times.Never);
            roatpApiClient.Verify(r => r.SearchOrganisationByName(It.IsAny<string>(), true), Times.Never);
        }

        [Test, MoqAutoData]
        public async Task ThenDoNotInvokeProviderRegisterApi(
            [Frozen] Mock<IProviderRegisterApiClient> providerRegisterApiClient,
            OrganisationSearchController sut)
        {
            await sut.OrganisationSearchPaged(_searchTerm, 1, 1);
            providerRegisterApiClient.Verify(r => r.SearchOrganisationByUkprn(It.IsAny<int>()), Times.Never);
            providerRegisterApiClient.Verify(r => r.SearchOrganisationByName(It.IsAny<string>(), false), Times.Never);
            providerRegisterApiClient.Verify(r => r.SearchOrganisationByName(It.IsAny<string>(), true), Times.Never);
        }

        [Test, MoqAutoData]
        public async Task ThenDoNotInvokeReferenceDataApi(
            [Frozen] Mock<IProviderRegisterApiClient> providerRegisterApiClient,
            OrganisationSearchController sut)
        {
            await sut.OrganisationSearchPaged(_searchTerm, 1, 1);
            providerRegisterApiClient.Verify(r => r.SearchOrganisationByUkprn(It.IsAny<int>()), Times.Never);
            providerRegisterApiClient.Verify(r => r.SearchOrganisationByName(It.IsAny<string>(), false), Times.Never);
            providerRegisterApiClient.Verify(r => r.SearchOrganisationByName(It.IsAny<string>(), true), Times.Never);
        }
    }

}