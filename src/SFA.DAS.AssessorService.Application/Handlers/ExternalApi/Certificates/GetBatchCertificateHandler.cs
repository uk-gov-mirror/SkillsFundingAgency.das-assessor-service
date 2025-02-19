﻿using MediatR;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using SFA.DAS.AssessorService.Api.Types.Models.ExternalApi.Certificates;
using SFA.DAS.AssessorService.Application.Handlers.ExternalApi._HelperClasses;
using SFA.DAS.AssessorService.Application.Interfaces;
using SFA.DAS.AssessorService.Domain.Consts;
using SFA.DAS.AssessorService.Domain.Entities;
using SFA.DAS.AssessorService.Domain.JsonData;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.AssessorService.Application.Handlers.ExternalApi.Certificates
{
    public class GetBatchCertificateHandler : IRequestHandler<GetBatchCertificateRequest, Certificate>
    {
        private readonly ICertificateRepository _certificateRepository;
        private readonly IContactQueryRepository _contactQueryRepository;
        private readonly ILogger<GetBatchCertificateHandler> _logger;
        private readonly IStandardRepository _standardRepository;
        private readonly IOrganisationQueryRepository _organisationQueryRepository;

        public GetBatchCertificateHandler(ICertificateRepository certificateRepository, IContactQueryRepository contactQueryRepository, IStandardRepository standardRepository, IOrganisationQueryRepository organisationQueryRepository, ILogger<GetBatchCertificateHandler> logger)
        {
            _certificateRepository = certificateRepository;
            _contactQueryRepository = contactQueryRepository;
            _standardRepository = standardRepository;
            _organisationQueryRepository = organisationQueryRepository;
            _logger = logger;
        }

        public async Task<Certificate> Handle(GetBatchCertificateRequest request, CancellationToken cancellationToken)
        {
            return await GetCertificate(request);
        }

        private async Task<Certificate> GetCertificate(GetBatchCertificateRequest request)
        {
            _logger.LogInformation("GetCertificate Before Get Certificate from db");
            var certificate = await _certificateRepository.GetCertificate(request.Uln, request.StandardCode);

            _logger.LogInformation("GetCertificate Before Get Searching Organisation from db");
            var searchingOrganisation = await _organisationQueryRepository.GetByUkPrn(request.UkPrn);

            var allowedCertificateStatus = new[] { CertificateStatus.Draft, CertificateStatus.Submitted }.Concat(CertificateStatus.PrintProcessStatus);

            if (certificate != null && searchingOrganisation != null && allowedCertificateStatus.Contains(certificate.Status))
            {
                var certData = JsonConvert.DeserializeObject<CertificateData>(certificate.CertificateData);

                if(string.Equals(certData.LearnerFamilyName, request.FamilyName, StringComparison.InvariantCultureIgnoreCase))
                {
                    certificate = await CertificateHelpers.ApplyStatusInformation(_certificateRepository, _contactQueryRepository, certificate);

                    if (!EpaOutcome.Pass.Equals(certData.EpaDetails?.LatestEpaOutcome, StringComparison.InvariantCultureIgnoreCase))
                    {
                        // As EPA has not passed, only give access to basic information & EPA Details
                        certificate = RedactCertificateInformation(certificate, true);
                    }

                    if (certificate.OrganisationId != searchingOrganisation.Id)
                    {
                        var providedStandards = await _standardRepository.GetEpaoRegisteredStandards(searchingOrganisation.EndPointAssessorOrganisationId, int.MaxValue, 1);

                        if (providedStandards.PageOfResults.Any(s => s.StandardCode == certificate.StandardCode))
                        {
                            // Shared standard but not the EPAO who created the certificate
                            certificate = RedactCertificateInformation(certificate, false);
                        }
                        else
                        {
                            certificate = null;
                        }
                    }
                }
                else
                {
                    certificate = null;
                }
            }
            else
            {
                certificate = null;
            }

            return certificate;
        }

        private Certificate RedactCertificateInformation(Certificate certificate, bool showEpaDetails)
        {
            // certificate is track-able entity. So we have to do this in order to stop it from updating in the database
            var json = JsonConvert.SerializeObject(certificate);
            var cert = JsonConvert.DeserializeObject<Certificate>(json);
            var certData = JsonConvert.DeserializeObject<CertificateData>(certificate.CertificateData);

            CertificateData redactedData = new CertificateData
            {
                LearnerGivenNames = certData.LearnerGivenNames,
                LearnerFamilyName = certData.LearnerFamilyName,
                StandardReference = certData.StandardReference,
                StandardName = certData.StandardName,
                StandardLevel = certData.StandardLevel,
                StandardPublicationDate = certData.StandardPublicationDate,
                EpaDetails = showEpaDetails ? certData.EpaDetails : null
            };

            cert.CertificateData = JsonConvert.SerializeObject(redactedData);
            cert.CertificateReference = null;
            cert.CertificateReferenceId = null;
            cert.CreateDay = DateTime.MinValue;
            cert.CreatedAt = DateTime.MinValue;
            cert.CreatedBy = null;
            cert.UpdatedAt = null;
            cert.UpdatedBy = null;
            cert.DeletedBy = null;
            cert.DeletedAt = null;

            return cert;
        }
    }
}
