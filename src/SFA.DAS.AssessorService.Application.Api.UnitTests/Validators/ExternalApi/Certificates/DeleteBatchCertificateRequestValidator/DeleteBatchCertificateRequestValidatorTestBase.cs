﻿using Microsoft.Extensions.Localization;
using Moq;

namespace SFA.DAS.AssessorService.Application.Api.UnitTests.Validators.ExternalApi.Certificates.DeleteBatchCertificateRequestValidator
{
    public class DeleteBatchCertificateRequestValidatorTestBase : BatchCertificateRequestValidatorTestBase
    {
        protected Api.Validators.ExternalApi.Certificates.DeleteBatchCertificateRequestValidator Validator;

        public DeleteBatchCertificateRequestValidatorTestBase () : base()
        {
            var stringLocalizerMock = new Mock<IStringLocalizer<Api.Validators.ExternalApi.Certificates.DeleteBatchCertificateRequestValidator>>();

            Validator = new Api.Validators.ExternalApi.Certificates.DeleteBatchCertificateRequestValidator(stringLocalizerMock.Object, OrganisationQueryRepositoryMock.Object, IlrRepositoryMock.Object, CertificateRepositoryMock.Object, StandardServiceMock.Object);
        }

    }
}
