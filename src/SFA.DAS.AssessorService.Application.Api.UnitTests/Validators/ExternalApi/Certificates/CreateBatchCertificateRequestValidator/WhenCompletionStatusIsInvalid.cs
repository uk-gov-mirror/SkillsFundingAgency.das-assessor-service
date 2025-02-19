﻿using FizzWare.NBuilder;
using FluentAssertions;
using FluentValidation.Results;
using NUnit.Framework;
using SFA.DAS.AssessorService.Api.Types.Models.ExternalApi.Certificates;
using SFA.DAS.AssessorService.Domain.Consts;
using SFA.DAS.AssessorService.Domain.JsonData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SFA.DAS.AssessorService.Application.Api.UnitTests.Validators.ExternalApi.Certificates.CreateBatchCertificateRequestValidator
{
    public class WhenCompletionStatusIsInvalid : CreateBatchCertificateRequestValidatorTestBase
    {
        private ValidationResult _validationResult;

        [Test]
        public async Task ThenValidationResultShouldBeFalse()
        {

            var request = CreateInvalidRequest();

            _validationResult = await Validator.ValidateAsync(request);

            _validationResult.IsValid.Should().BeFalse();
            _validationResult.Errors.Count.Should().Be(1);
        }

        private CreateBatchCertificateRequest CreateInvalidRequest()
        {
            CreateBatchCertificateRequest request = Builder<CreateBatchCertificateRequest>.CreateNew()
               .With(i => i.Uln = 1234567891)
               .With(i => i.StandardCode = 1)
               .With(i => i.StandardReference = null)
               .With(i => i.UkPrn = 12345678)
               .With(i => i.FamilyName = "Test")
               .With(i => i.CertificateReference = null)
               .With(i => i.CertificateData = Builder<CertificateData>.CreateNew()
                               .With(cd => cd.ContactPostCode = "AA11AA")
                               .With(cd => cd.AchievementDate = DateTime.UtcNow)
                               .With(cd => cd.OverallGrade = CertificateGrade.Pass)
                               .With(cd => cd.CourseOption = null)
                               .Build())
               .Build();

            return request;
        }
    }
}
