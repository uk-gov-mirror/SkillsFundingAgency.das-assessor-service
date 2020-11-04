﻿using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.AssessorService.Api.Types.Models;
using SFA.DAS.AssessorService.Api.Types.Models.BatchLogs;
using SFA.DAS.AssessorService.Api.Types.Models.Validation;
using SFA.DAS.AssessorService.Application.Handlers.BatchLogs;
using SFA.DAS.AssessorService.Domain.Entities;
using SFA.DAS.AssessorService.Domain.JsonData.Printing;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.AssessorService.Application.UnitTests.Handlers.BatchLogs.UpdateBatchLogSentToPrinterHandlerTests
{
    public class When_called_and_batch_does_exist : UpdateBatchLogSentToPrinterHandlerTestBase
    {
        private ValidationResponse _response;
        private DateTime _utcNow = DateTime.UtcNow;

        [SetUp]
        public async Task Arrange()
        {
            // Arrange
            base.BaseArrange();

            // Act
            var sut = new UpdateBatchLogSentToPrinterHandler(_batchLogRepository.Object);
            _response = await sut.Handle(new UpdateBatchLogSentToPrinterRequest
            {
                BatchNumber = _validBatchNumber,
                BatchCreated = _utcNow,
                NumberOfCertificates = 1,
                NumberOfCoverLetters = 1,
                CertificatesFileName = "TestFileName",
                FileUploadStartTime = _utcNow,
                FileUploadEndTime = _utcNow
            }, new CancellationToken());
        }

        [Test]
        public void Then_validation_response_is_valid_true()
        {
            // Assert
            _response.IsValid.Should().Be(true);
            _response.Errors.Count.Should().Be(0);
        }

        [Test]
        public void Then_respository_is_called_to_update_batchlog()
        {
            // Assert
            _batchLogRepository.Verify(r => r.UpdateBatchLogSentToPrinter(
                _validBatchNumber, _utcNow, 1, 1, "TestFileName", _utcNow, _utcNow, 
                It.Is<BatchData>(p => p.BatchNumber == _validBatchNumber)), Times.Once);
        }
    }
}
