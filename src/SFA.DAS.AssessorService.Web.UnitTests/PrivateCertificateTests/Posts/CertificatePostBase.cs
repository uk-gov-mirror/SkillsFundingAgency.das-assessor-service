﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FizzWare.NBuilder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;
using Moq;
using Newtonsoft.Json;
using SFA.DAS.AssessorService.Api.Types.Models.Standards;
using SFA.DAS.AssessorService.Application.Api.Client.Clients;
using SFA.DAS.AssessorService.Domain.Entities;
using SFA.DAS.AssessorService.Domain.JsonData;
using SFA.DAS.AssessorService.Web.Controllers;
using SFA.DAS.AssessorService.Web.Infrastructure;
using SFA.DAS.AssessorService.Web.UnitTests.MockedObjects;
using Organisation = SFA.DAS.AssessorService.Domain.Entities.Organisation;

namespace SFA.DAS.AssessorService.Web.UnitTests.PrivateCertificateTests.Posts
{
    public class CertificatePostBase
    {
        protected Mock<ILogger<CertificateController>> MockLogger;
        protected Mock<IHttpContextAccessor> MockHttpContextAccessor;
        protected ICertificateApiClient MockCertificateApiClient;
        protected IOrganisationsApiClient MockOrganisationApiClient;
        protected Mock<IStandardServiceClient> MockStandardServiceClient;

        protected Mock<ISessionService> MockSession;

        protected Certificate Certificate;
        protected CertificateData CertificateData;

        public CertificatePostBase()
        {
            Certificate = SetupCertificate();

            MockLogger = new Mock<ILogger<CertificateController>>();
            var mockedApiClientLogger = new Mock<ILogger<CertificateApiClient>>();
            var mockedOrgApiClientLogger = new Mock<ILogger<OrganisationsApiClient>>();
            MockSession = new Mock<ISessionService>();
            MockStandardServiceClient = new Mock<IStandardServiceClient>();

            var standards = new List<StandardCollation>
            {
                new StandardCollation
                {
                    StandardId = 91,
                    StandardData = new StandardData{Level = 2},
                    Title = "Test Title 1"
                },
                new StandardCollation
                {
                    StandardId = 92,
                    StandardData = new StandardData{Level = 3},
                    Title = "Test Title 2"
                },
                new StandardCollation
                {
                    StandardId = 93,
                    StandardData = new StandardData{Level = 5},
                    Title = "Test Title 3"
                },
                new StandardCollation
                {
                    StandardId = 94,
                    StandardData = new StandardData{Level = 2},
                    Title = "Test Title 4"
                },
                new StandardCollation
                {
                    StandardId = 95,
                    StandardData = new StandardData{Level = 2},
                    Title = "Test Title 5"
                },
            };

            MockStandardServiceClient.Setup(s => s.GetAllStandards()).Returns(Task.FromResult(standards.AsEnumerable()));

            MockHttpContextAccessor = MockedHttpContextAccessor.Setup();
            MockCertificateApiClient = MockedCertificateApiClient.Setup(Certificate, mockedApiClientLogger);

            MockOrganisationApiClient = MockedOrganisationsApiClient.Setup(mockedOrgApiClientLogger);

            CertificateData = JsonConvert.DeserializeObject<CertificateData>(Certificate.CertificateData);
        }

        private Certificate SetupCertificate()
        {
            var certificate = new Builder().CreateNew<Certificate>()
                .With(q => q.CertificateData = JsonConvert.SerializeObject(new Builder()
                    .CreateNew<CertificateData>()
                    .With(x => x.AchievementDate = DateTime.Now)
                    .Build()))
                .Build();

            var organisaionId = Guid.NewGuid();
            certificate.OrganisationId = organisaionId;

            var organisation = new Builder().CreateNew<Organisation>()
                .Build();

            certificate.Organisation = organisation;

            return certificate;
        }

        protected void SetupSession()
        {
            var certificateSession = Builder<CertificateSession>
                .CreateNew()
                .With(q => q.CertificateId = Certificate.Id)
                .Build();

            var serialisedCertificateSession
                = JsonConvert.SerializeObject(certificateSession);

            MockSession.Setup(q => q.Get("CertificateSession"))
                .Returns(serialisedCertificateSession);

            MockSession.Setup(q => q.Get("EndPointAsessorOrganisationId"))
                .Returns("EPA00001");
        }

        protected void AddRedirectCheck()
        {
            var queryString = new Dictionary<string, StringValues> { { "redirecttocheck", "true" } };

            MockHttpContextAccessor.Object.HttpContext.Request.Query =
                new QueryCollection(queryString);

            MockSession.Setup(q => q.Exists("redirecttocheck"))
                .Returns(true);

            MockSession.Setup(q => q.Get("redirecttocheck"))
                .Returns("true");
        }
    }
}