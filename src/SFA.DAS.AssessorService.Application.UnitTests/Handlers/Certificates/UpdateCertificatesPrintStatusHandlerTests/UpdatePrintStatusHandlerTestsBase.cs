﻿using MediatR;
using Microsoft.Extensions.Logging;
using Moq;
using SFA.DAS.AssessorService.Api.Types.Models;
using SFA.DAS.AssessorService.Application.Handlers.Certificates;
using SFA.DAS.AssessorService.Application.Interfaces;
using SFA.DAS.AssessorService.Domain.Consts;
using SFA.DAS.AssessorService.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.AssessorService.Application.UnitTests.Handlers.Certificates.UpdateCertificatesPrintStatusHandlerTests
{
    public class UpdateCertificatesPrintStatusHandlerTestsBase
    {
        protected UpdateCertificatesPrintStatusHandler _sut;

        protected Mock<ICertificateRepository> _certificateRepository;
        protected Mock<ICertificateBatchLogRepository> _certificateBatchLogRepository;
        protected Mock<IMediator> _mediator;
        protected Mock<ILogger<UpdateCertificatesPrintStatusHandler>> _logger;

        protected static int _batchNumber = 222;
        protected static DateTime _printedAt = DateTime.UtcNow;

        protected static string _certificateReference1 = "00000001";
        protected static string _certificateReference2 = "00000002";
        protected static string _certificateReference3 = "00000003";
        protected static string _certificateReference4 = "00000004";
        protected static string _certificateReference5 = "00000005";
        protected static string _certificateReference6 = "00000006";
        protected static string _certificateReferenceUpdateAfterPrinted = "00000007";
        protected static string _certificateReferenceDeletedAfterPrinted = "00000008";        

        protected static string _certificateNotDeliveredReason1 = "The address given did not match the building street address";

        protected BatchLog _batchLog = new BatchLog { Id = Guid.NewGuid(), BatchNumber = _batchNumber };
        
        public void BaseArrange()
        {
            _certificateRepository = new Mock<ICertificateRepository>();
            _certificateBatchLogRepository = new Mock<ICertificateBatchLogRepository>();
            _mediator = new Mock<IMediator>();

            _certificateRepository.Setup(r => r.GetCertificate(It.IsIn(_certificateReference1)))
                .Returns((string certificateReference) => Task.FromResult(
                    new Certificate
                    {
                        Id = Guid.NewGuid(),
                        CertificateReference = certificateReference,
                        UpdatedAt = DateTime.UtcNow.AddDays(-1),
                        BatchNumber = _batchNumber,
                        Status = CertificateStatus.SentToPrinter,
                        ToBePrinted = DateTime.UtcNow.AddDays(-1)
                    }));

            _certificateRepository.Setup(r => r.GetCertificate(It.IsIn(_certificateReference2, _certificateReference3)))
                .Returns((string certificateReference) => Task.FromResult(
                    new Certificate
                    {
                        Id = Guid.NewGuid(),
                        CertificateReference = certificateReference,
                        UpdatedAt = DateTime.UtcNow.AddDays(-1),
                        BatchNumber = _batchNumber,
                        Status = CertificateStatus.Printed,
                        ToBePrinted = DateTime.UtcNow.AddDays(-1)
                    }));

            _certificateRepository.Setup(r => r.GetCertificate(It.IsIn(_certificateReferenceUpdateAfterPrinted)))
                .Returns((string certificateReference) => Task.FromResult(
                    new Certificate
                    {
                        Id = Guid.NewGuid(),
                        CertificateReference = certificateReference,
                        UpdatedAt = DateTime.UtcNow.AddDays(1),
                        BatchNumber = null,
                        Status = CertificateStatus.Reprint,
                    }));

            _certificateRepository.Setup(r => r.GetCertificate(It.IsIn(_certificateReferenceDeletedAfterPrinted)))
                .Returns((string certificateReference) => Task.FromResult(
                    new Certificate
                    {
                        Id = Guid.NewGuid(),
                        CertificateReference = certificateReference,
                        Status = CertificateStatus.Deleted,
                        UpdatedAt = DateTime.UtcNow.AddDays(-1)
                    }));

            _certificateRepository.Setup(r => r.GetCertificate(It.IsIn(_certificateReference5)))
              .Returns((string certificateReference) => Task.FromResult(
                  new Certificate
                  {
                      Id = Guid.NewGuid(),
                      CertificateReference = certificateReference,
                      Status = CertificateStatus.Delivered,
                      UpdatedAt = DateTime.UtcNow.AddDays(1)
                  }));

            _certificateBatchLogRepository.Setup(r => r.GetCertificateBatchLog(It.IsIn(_certificateReference1), _batchNumber))
               .Returns((string certificateReference, int batchNumber) => Task.FromResult(
                    new CertificateBatchLog
                    {
                        Id = Guid.NewGuid(),
                        CertificateReference = certificateReference,
                        UpdatedAt = DateTime.UtcNow.AddDays(-1),
                        BatchNumber = batchNumber,
                        Status = CertificateStatus.SentToPrinter
                    }));

            _certificateBatchLogRepository.Setup(r => r.GetCertificateBatchLog(It.IsIn(_certificateReference2), _batchNumber))
             .Returns((string certificateReference, int batchNumber) => Task.FromResult(
                  new CertificateBatchLog
                  {
                      Id = Guid.NewGuid(),
                      CertificateReference = certificateReference,
                      UpdatedAt = DateTime.UtcNow.AddDays(-1),
                      BatchNumber = batchNumber,
                      Status = CertificateStatus.SentToPrinter
                  }));          

            _certificateBatchLogRepository.Setup(r => r.GetCertificateBatchLog(It.IsIn(_certificateReference3), _batchNumber))
             .Returns((string certificateReference, int batchNumber) => Task.FromResult(
                  new CertificateBatchLog
                  {
                      Id = Guid.NewGuid(),
                      CertificateReference = certificateReference,
                      UpdatedAt = DateTime.UtcNow.AddDays(-1),
                      BatchNumber = batchNumber,
                      Status = CertificateStatus.SentToPrinter
                  }));

            _certificateBatchLogRepository.Setup(r => r.GetCertificateBatchLog(It.IsIn(_certificateReferenceUpdateAfterPrinted), _batchNumber))
              .Returns((string certificateReference, int batchNumber) => Task.FromResult(
                  new CertificateBatchLog
                  {
                      Id = Guid.NewGuid(),
                      CertificateReference = certificateReference
                  }));

            _certificateBatchLogRepository.Setup(r => r.GetCertificateBatchLog(It.IsIn(_certificateReferenceDeletedAfterPrinted), _batchNumber))
                .Returns((string certificateReference, int batchNumber) => Task.FromResult(
                    new CertificateBatchLog
                    {
                        Id = Guid.NewGuid(),
                        CertificateReference = certificateReference,
                        Status = CertificateStatus.Delivered
                    }));

            _certificateBatchLogRepository.Setup(r => r.GetCertificateBatchLog(It.IsIn(_certificateReference5), _batchNumber))
              .Returns((string certificateReference, int batchNumber) => Task.FromResult(
                  new CertificateBatchLog
                  {
                      Id = Guid.NewGuid(),
                      CertificateReference = certificateReference,
                      Status = CertificateStatus.Delivered,                      
                      StatusAt = DateTime.UtcNow.AddDays(1)
                  }));

            _certificateRepository.Setup(r => r.GetCertificate(It.IsIn(_certificateReference4)))
                .Returns((string certificateReference) => Task.FromResult<Certificate>(null));           

            _certificateRepository.Setup(r => r.UpdateSentToPrinter(It.IsAny<Certificate>(), It.IsAny<int>(), It.IsAny<DateTime>()))
                .Returns(Task.CompletedTask);

            _mediator.Setup(r => r.Send(It.Is<GetForBatchNumberBatchLogRequest>(p => p.BatchNumber == _batchNumber), It.IsAny<CancellationToken>()))
                .Returns((GetForBatchNumberBatchLogRequest request, CancellationToken token) => Task.FromResult(new BatchLogResponse { BatchNumber = request.BatchNumber }));

            _mediator.Setup(r => r.Send(It.Is<GetForBatchNumberBatchLogRequest>(p => p.BatchNumber != _batchNumber), It.IsAny<CancellationToken>()))
                .Returns((GetForBatchNumberBatchLogRequest request, CancellationToken token) => Task.FromResult<BatchLogResponse>(null));

            _logger = new Mock<ILogger<UpdateCertificatesPrintStatusHandler>>();

            _sut = new UpdateCertificatesPrintStatusHandler(_certificateRepository.Object, _certificateBatchLogRepository.Object, _mediator.Object, _logger.Object);
        }
    }
}
