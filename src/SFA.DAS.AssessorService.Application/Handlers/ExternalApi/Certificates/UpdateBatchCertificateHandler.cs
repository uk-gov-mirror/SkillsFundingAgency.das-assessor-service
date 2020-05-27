﻿using MediatR;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using SFA.DAS.AssessorService.Api.Types.Models.ExternalApi.Certificates;
using SFA.DAS.AssessorService.Application.Handlers.ExternalApi._HelperClasses;
using SFA.DAS.AssessorService.Application.Interfaces;
using SFA.DAS.AssessorService.Domain.Consts;
using SFA.DAS.AssessorService.Domain.Entities;
using SFA.DAS.AssessorService.Domain.JsonData;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using NotFound = SFA.DAS.AssessorService.Domain.Exceptions.NotFound;

namespace SFA.DAS.AssessorService.Application.Handlers.ExternalApi.Certificates
{
    public class UpdateBatchCertificateHandler : IRequestHandler<UpdateBatchCertificateRequest, Certificate>
    {
        private readonly ICertificateRepository _certificateRepository;
        private readonly IContactQueryRepository _contactQueryRepository;
        private readonly ILogger<UpdateBatchCertificateHandler> _logger;
        private readonly IStandardService _standardService;

        public UpdateBatchCertificateHandler(ICertificateRepository certificateRepository, IContactQueryRepository contactQueryRepository, ILogger<UpdateBatchCertificateHandler> logger, IStandardService standardService)
        {
            _certificateRepository = certificateRepository;
            _contactQueryRepository = contactQueryRepository;
            _logger = logger;
            _standardService = standardService;
        }

        public async Task<Certificate> Handle(UpdateBatchCertificateRequest request, CancellationToken cancellationToken)
        {
            return await UpdateCertificate(request);
        }

        private async Task<Certificate> UpdateCertificate(UpdateBatchCertificateRequest request)
        {
            _logger.LogInformation("UpdateCertificate Before Get Standard from API");
            var standard = await _standardService.GetStandard(request.StandardCode);

            _logger.LogInformation("UpdateCertificate Before Get Certificate from db");
            var certificate = await _certificateRepository.GetCertificate(request.Uln, request.StandardCode);

            if (standard != null && certificate != null)
            {
                var certData = CombineCertificateData(JsonConvert.DeserializeObject<CertificateData>(certificate.CertificateData), request.CertificateData, standard);

                _logger.LogInformation("UpdateCertificate Before Update CertificateData");

                // need to update EPA Reference too
                certData.EpaDetails.EpaReference = certificate.CertificateReference;
                certificate.CertificateData = JsonConvert.SerializeObject(certData);

                // adjust Status appropriately
                if (certificate.Status == CertificateStatus.Deleted)
                {
                    certificate.Status = CertificateStatus.Draft;
                }

                _logger.LogInformation("UpdateCertificate Before Update Cert in db");
                await _certificateRepository.Update(certificate, ExternalApiConstants.ApiUserName, CertificateActions.Amend);

                return await CertificateHelpers.ApplyStatusInformation(_certificateRepository, _contactQueryRepository, certificate);
            }
            else
            {
                _logger.LogWarning($"UpdateCertificate Did not find Certificate for Uln {request.Uln} and StandardCode {request.StandardCode}");
                throw new NotFound();
            }
        }

        private CertificateData CombineCertificateData(CertificateData certData, CertificateData requestData, StandardCollation standard)
        {
            var epaDetails = certData.EpaDetails ?? new EpaDetails();
            if (epaDetails.Epas is null) epaDetails.Epas = new List<EpaRecord>();

            var epaOutcome = certData.OverallGrade == CertificateGrade.Fail ? EpaOutcome.Fail : EpaOutcome.Pass;
            if (requestData.AchievementDate != null && !epaDetails.Epas.Any(rec => rec.EpaDate == requestData.AchievementDate.Value && rec.EpaOutcome == epaOutcome))
            {
                var record = new EpaRecord { EpaDate = requestData.AchievementDate.Value, EpaOutcome = epaOutcome };
                epaDetails.Epas.Add(record);

                // sort pass outcomes before fail outcomes as pass is the final state even if earlier than the fail
                var latestRecord = epaDetails.Epas
                            .OrderByDescending(epa => epa.EpaOutcome != EpaOutcome.Fail ? 1 : 0)
                            .ThenByDescending(epa => epa.EpaDate)
                            .First();

                epaDetails.LatestEpaDate = latestRecord.EpaDate;
                epaDetails.LatestEpaOutcome = latestRecord.EpaOutcome;
            }

            return new CertificateData()
            {
                LearnerGivenNames = certData.LearnerGivenNames,
                LearnerFamilyName = certData.LearnerFamilyName,
                LearningStartDate = certData.LearningStartDate,
                StandardReference = certData.StandardReference,
                StandardName = certData.StandardName,
                StandardLevel = certData.StandardLevel,
                StandardPublicationDate = certData.StandardPublicationDate,
                FullName = certData.FullName,
                ProviderName = certData.ProviderName,

                ContactName = requestData.ContactName,
                ContactOrganisation = requestData.ContactOrganisation,
                Department = requestData.Department,
                ContactAddLine1 = requestData.ContactAddLine1,
                ContactAddLine2 = requestData.ContactAddLine2,
                ContactAddLine3 = requestData.ContactAddLine3,
                ContactAddLine4 = requestData.ContactAddLine4,
                ContactPostCode = requestData.ContactPostCode,
                Registration = requestData.Registration,
                AchievementDate = requestData.AchievementDate,
                CourseOption = CertificateHelpers.NormalizeCourseOption(standard, requestData.CourseOption),
                OverallGrade = CertificateHelpers.NormalizeOverallGrade(requestData.OverallGrade),

                EpaDetails = epaDetails
            };
        }
    }
}
