﻿using MediatR;
using SFA.DAS.AssessorService.Domain.Entities;
using SFA.DAS.AssessorService.Domain.JsonData;

namespace SFA.DAS.AssessorService.Api.Types.Models.ExternalApi.Certificates
{
    public class BatchCertificateRequest : IRequest<Certificate>
    {
        public string RequestId { get; set; }
        public long Uln { get; set; }
        public string FamilyName { get; set; }

        public int StandardCode { get; set; }
        public string StandardReference { get; set; }
        public int? StandardId { get; set; }
        public int UkPrn { get; set; }

        public string CertificateReference { get; set; }
        public CertificateData CertificateData { get; set; }
    }
}
