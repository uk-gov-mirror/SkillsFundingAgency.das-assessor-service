﻿using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using SFA.DAS.AssessorService.Api.Types.Models.Certificates;
using SFA.DAS.AssessorService.Application.Handlers.Staff;
using SFA.DAS.AssessorService.Application.Interfaces;
using SFA.DAS.AssessorService.Application.Logging;
using SFA.DAS.AssessorService.Domain.Entities;
using SFA.DAS.AssessorService.Domain.JsonData;
using CertificateStatus = SFA.DAS.AssessorService.Domain.Consts.CertificateStatus;

namespace SFA.DAS.AssessorService.Application.Handlers.Private
{
    public class StartPrivateCertificateHandler : IRequestHandler<StartCertificatePrivateRequest, Certificate>
    {
        private readonly ICertificateRepository _certificateRepository;             
        private readonly IOrganisationQueryRepository _organisationQueryRepository;
        private readonly ILogger<StartCertificateHandler> _logger;

        public StartPrivateCertificateHandler(ICertificateRepository certificateRepository,
            IOrganisationQueryRepository organisationQueryRepository, ILogger<StartCertificateHandler> logger)
        {
            _certificateRepository = certificateRepository;            
            _organisationQueryRepository = organisationQueryRepository;
            _logger = logger;
        }

        public async Task<Certificate> Handle(StartCertificatePrivateRequest request, CancellationToken cancellationToken)
        {
            var organisation = await _organisationQueryRepository.GetByUkPrn(request.UkPrn);

            var certificate = await _certificateRepository.GetPrivateCertificate(request.Uln,
                organisation.EndPointAssessorOrganisationId);
            if (certificate != null)
            {
                if (certificate.Status == Domain.Consts.CertificateStatus.Deleted)
                {
                    var certData = JsonConvert.DeserializeObject<CertificateData>(certificate.CertificateData);
                    certData.LearnerFamilyName = request.LastName;
                    certificate.CertificateData = JsonConvert.SerializeObject(certData);
                    certificate.IsPrivatelyFunded = true;
                    await _certificateRepository.Update(certificate, request.Username, string.Empty, false);
                }
                return certificate;
            }

            return await CreateNewCertificate(request);
        }

        private async Task<Certificate> CreateNewCertificate(StartCertificatePrivateRequest request)
        {
            try
            {
                var organisation = await _organisationQueryRepository.GetByUkPrn(request.UkPrn);

                var certData = new CertificateData()
                {
                    LearnerFamilyName = request.LastName,
                    EpaDetails = new EpaDetails { Epas = new List<EpaRecord>() }
                };

                _logger.LogInformation("CreateNewCertificate Before create new Certificate");
                var newCertificate = await _certificateRepository.NewPrivate(
                    new Certificate()
                    {
                        Uln = request.Uln,
                        OrganisationId = organisation.Id,
                        CreatedBy = request.Username,
                        CertificateData = JsonConvert.SerializeObject(certData),
                        Status = CertificateStatus.Draft,
                        CertificateReference = string.Empty,
                        CreateDay = DateTime.UtcNow.Date,
                        IsPrivatelyFunded = true
                    }, organisation.EndPointAssessorOrganisationId);

                _logger.LogInformation(LoggingConstants.CertificateStarted);
                _logger.LogInformation($"Certificate with ID: {newCertificate.Id} Started with reference of {newCertificate.CertificateReference}");

                return newCertificate;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }

    public interface ICommitmentsApi
    {
        CommitmentEmployerDetails GetCommitmentEmployerDetails(long providerId, long commitmentId);
    }

    public class CommitmentEmployerDetails
    {
        public string LegalEntityName { get; set; }
        public string LegalEntityAddress { get; set; }
    }
}
