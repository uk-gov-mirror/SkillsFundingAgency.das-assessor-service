﻿using System;
using System.Threading;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using SFA.DAS.AssessorService.Api.Types.Models.Certificates;
using SFA.DAS.AssessorService.Application.Handlers.Private;
using SFA.DAS.AssessorService.Application.Handlers.Staff;
using SFA.DAS.AssessorService.Application.Interfaces;
using SFA.DAS.AssessorService.Domain.Entities;
using Organisation = SFA.DAS.AssessorService.Domain.Entities.Organisation;

namespace SFA.DAS.AssessorService.Application.UnitTests.Handlers.Certificates.StartPrivateCertificateHandlerTests
{
    [TestFixture]
    public class When_called_and_no_existing_certificate
    {
        private Mock<ICertificateRepository> _certificateRepository;
        private StartPrivateCertificateHandler _startPrivateCertificateHandler;
        private Guid _organisationId;
        private string _endPointAssessorOrganisationId;
        private Certificate _returnedCertificate;

        [SetUp]
        public void Arrange()
        {
            _organisationId = Guid.NewGuid();
            _endPointAssessorOrganisationId = "EPA0001";

            _certificateRepository = new Mock<ICertificateRepository>();
            _certificateRepository.Setup(r => r.GetPrivateCertificate(1111111111, _endPointAssessorOrganisationId)).ReturnsAsync(default(Certificate));

            _certificateRepository.Setup(r => r.NewPrivate(It.IsAny<Certificate>(), _endPointAssessorOrganisationId))
                .ReturnsAsync(new Certificate() {CertificateReferenceId = 10000, IsPrivatelyFunded = true});

            var organisationQueryRepository = new Mock<IOrganisationQueryRepository>();

            organisationQueryRepository.Setup(r => r.GetByUkPrn(88888888)).ReturnsAsync(new Organisation() { Id = _organisationId, EndPointAssessorOrganisationId = _endPointAssessorOrganisationId });

            _startPrivateCertificateHandler = new StartPrivateCertificateHandler(_certificateRepository.Object, organisationQueryRepository.Object, new Mock<ILogger<StartCertificateHandler>>().Object);

            _returnedCertificate = _startPrivateCertificateHandler
                .Handle(
                    new StartCertificatePrivateRequest()
                    {
                        UkPrn = 88888888,
                        Uln = 1111111111,
                        LastName = "Smith",
                        EndPointAssessorOrganisationId = "EPA0001",
                        Username = "user"
                    }, new CancellationToken()).Result;
        }

        [Test]
        public void Then_a_new_certificate_is_created()
        {
            _certificateRepository.Verify(r => r.NewPrivate(It.Is<Certificate>(c =>
                c.Uln == 1111111111 && 
                c.OrganisationId == _organisationId && 
                c.CreatedBy == "user" && 
                c.Status == Domain.Consts.CertificateStatus.Draft &&
                c.CertificateReference == ""), _endPointAssessorOrganisationId));
        }

        [Test]
        public void Then_the_certificate_has_IsPrivatelyFunded_flag_set_to_true()
        {
            _returnedCertificate.IsPrivatelyFunded.Should().BeTrue();
        }
    }
}