﻿using System;
using System.Collections.Generic;
using System.Threading;
using FluentAssertions;
using Moq;
using Newtonsoft.Json;
using NUnit.Framework;
using SFA.DAS.AssessorService.Api.Types.Models;
using SFA.DAS.AssessorService.Domain.Consts;
using SFA.DAS.AssessorService.Domain.Entities;
using SFA.DAS.AssessorService.Domain.JsonData;

namespace SFA.DAS.AssessorService.Application.UnitTests.Handlers.Search
{
    [TestFixture]
    public class When_search_completed_for_private_fundeded : SearchHandlerTestBase
    {
        [SetUp]
        public void Arrange()
        {
            Setup();

            var certificateId = Guid.NewGuid();
            var searchingEpaoOrgId = Guid.NewGuid();

            CertificateRepository.Setup(r => r.GetCompletedCertificatesFor(1111111111))
                .ReturnsAsync(new List<Certificate>
                {
                    new Certificate
                    {
                        Id = certificateId,
                        CertificateReference = "00010001",
                        StandardCode = 12,
                        CertificateData =
                            JsonConvert.SerializeObject(new CertificateData
                            {
                                OverallGrade = CertificateGrade.Distinction,
                                LearningStartDate = new DateTime(2015, 06, 01),
                                AchievementDate = new DateTime(2018, 06, 01)
                            }),
                        IsPrivatelyFunded = true,
                        CreatedBy = "username"
                    }
                });

            CertificateRepository.Setup(r => r.GetCertificateLogsFor(certificateId))
                .ReturnsAsync(new List<CertificateLog>());

            ContactRepository.Setup(cr => cr.GetContact("username"))
                .ReturnsAsync(new Contact() { DisplayName = "EPAO User from this EAPOrg", OrganisationId = searchingEpaoOrgId });

            IlrRepository.Setup(r => r.SearchForLearnerByUln(It.IsAny<long>()))
                .ReturnsAsync(new List<Ilr> { new Ilr() { StdCode = 12, FamilyName = "Lamora" } });
        }

        [Test]
        public void Then_a_response_is_returned_including_LearnStartDate()
        {
            var result =
                SearchHandler.Handle(
                    new SearchQuery() { Surname = "Lamora", EpaOrgId = "12345", Uln = 1111111111, Username = "username", IsPrivatelyFunded = true },
                    new CancellationToken()).Result;

            result[0].LearnStartDate.Should().Be(new DateTime(2015, 06, 01));
        }

        public void Then_a_response_is_returned_including_IsPrivatelyFunded()
        {
            var result =
                SearchHandler.Handle(
                    new SearchQuery() { Surname = "Lamora", EpaOrgId = "12345", Uln = 1111111111, Username = "username", IsPrivatelyFunded = true },
                    new CancellationToken()).Result;

            result[0].IsPrivatelyFunded.Should().Be(true);
        }

        [Test]
        public void Then_a_Search_Log_entry_is_created()
        {
            SearchHandler.Handle(
                    new SearchQuery() { Surname = "Lamora", EpaOrgId = "12345", Uln = 1111111111, Username = "username", IsPrivatelyFunded = true },
                    new CancellationToken()).Wait();

            IlrRepository.Verify(r => r.StoreSearchLog(It.Is<SearchLog>(l =>
                l.Username == "username" &&
                l.NumberOfResults == 1 &&
                l.Surname == "Lamora" &&
                l.Uln == 1111111111 &&
                l.SearchData.IsPrivatelyFunded == true)));
        }
    }
}