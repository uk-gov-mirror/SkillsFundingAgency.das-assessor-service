﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SFA.DAS.AssessorService.Api.Types.Models.Certificates;
using SFA.DAS.AssessorService.Domain.DTOs;
using SFA.DAS.AssessorService.Domain.Entities;
using SFA.DAS.AssessorService.Domain.Paging;

namespace SFA.DAS.AssessorService.Application.Interfaces
{
    public interface ICertificateRepository
    {
        Task<Certificate> New(Certificate certificate);
        Task<Certificate> NewPrivate(Certificate certificate, string endpointOrganisationId);
        
        Task<Certificate> GetCertificate(Guid id);
        Task<Certificate> GetCertificate(long uln, int standardCode);
        Task<Certificate> GetCertificate(string certificateReference);
        Task<Certificate> GetPrivateCertificate(long uln, string endpointOrganisationId);
        Task<Certificate> GetCertificateByOrgIdLastname(long uln, string endpointOrganisationId, string lastName);
        Task<Certificate> GetCertificateByUlnLastname(long uln, string lastName);
        Task<Certificate> GetCertificate(string certificateReference, string lastName, DateTime? achievementDate);

        Task<List<Certificate>> GetCompletedCertificatesFor(long uln);
        Task<List<Certificate>> GetCertificates(List<string> statuses);
        
        Task<int> GetCertificatesReadyToPrintCount(string[] excludedOverallGrades, string[] includedStatus);
        Task<Guid[]> GetCertificatesReadyToPrint(int numberOfCertifictes, string[] excludedOverallGrades, string[] includedStatus);
        Task<List<CertificatePrintSummary>> GetCertificatesForBatch(int batchNumber);
        Task UpdateCertificatesReadyToPrintInBatch(Guid[] certificateIds, int batchNumber);

        Task<Certificate> Update(Certificate certificate, string username, string action, bool updateLog = true, string reasonForChange = null);
        Task Delete(long uln, int standardCode, string username, string action, bool updateLog = true, string reasonForChange = null, string incidentNumber = null);
        Task<Certificate> UpdateProviderName(Guid id, string providerName);

        Task UpdatePrintStatus(Certificate certificate, int batchNumber, string status, DateTime statusAt, string reasonForChange, bool isLatestChange);

        Task UpdatePrivatelyFundedCertificatesToBeApproved();
        
        Task<List<CertificateLog>> GetCertificateLogsFor(Guid certificateId);
        Task<PaginatedList<Certificate>> GetCertificateHistory(string endPointAssessorOrganisationId, int pageIndex, int pageSize, List<string> statuses);
        Task<string> GetPreviousProviderName(int providerUkPrn);
        Task<CertificateAddress> GetContactPreviousAddress(string username, bool requestIsPrivatelyFunded);
        Task ApproveCertificates(List<ApprovalResult> approvalResults, string username);
        Task<PaginatedList<Certificate>> GetCertificatesForApproval(int pageIndex, int pageSize,string status, string privatelyFundedStatus);
        Task<bool> CertifciateExistsForUln(long uln);
        Task<Certificate> GetCertificateDeletedByUln(long uln);
    }
}