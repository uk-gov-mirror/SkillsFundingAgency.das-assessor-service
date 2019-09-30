﻿using SFA.DAS.AssessorService.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SFA.DAS.AssessorService.Application.Interfaces
{
    public interface IOppFinderRepository
    {
        Task<bool> CreateExpressionOfInterest(string standardReference, string email, string organisationName, string contactName, string contactNumber);

        Task<OppFinderFilterStandardsResult> GetOppFinderFilterStandards(string searchTerm, string sectorFilters, string levelFilters);
        Task<OppFinderApprovedStandardsResult> GetOppFinderApprovedStandards(string searchTerm, string sectorFilters, string levelFilters, string sortColumn, int sortAscending, int pageSize, int pageIndex);
        Task<OppFinderApprovedStandardDetailsResult> GetOppFinderApprovedStandardDetails(int standardCode);
        Task<OppFinderNonApprovedStandardsResult> GetOppFinderNonApprovedStandards(string searchTerm, string sectorFilters, string levelFilters, string sortColumn, int sortAscending, int pageSize, int pageIndex, string nonApprovedType);

        Task UpdateStandardSummary();       
    }

    public class OppFinderFilterStandardsResult
    {
        public IEnumerable<OppFinderSectorFilterResult> MatchingSectorFilterResults { get; set; }
        public IEnumerable<OppFinderLevelFilterResult> MatchingLevelFilterResults { get; set; }
    }

    public class OppFinderApprovedStandardsResult
    {
        public IEnumerable<OppFinderApprovedStandard> PageOfResults { get; set; }
        public int TotalCount { get; set; }
    }

    public class OppFinderNonApprovedStandardsResult
    {
        public IEnumerable<OppFinderNonApprovedStandard> PageOfResults { get; set; }
        public int TotalCount { get; set; }
    }

    public class OppFinderApprovedStandardDetailsResult
    {
        public OppFinderApprovedStandardOverviewResult OverviewResult { get; set; }
        public List<OppFinderApprovedStandardRegionResult> RegionResults { get; set; }
    }

    public class OppFinderApprovedStandardOverviewResult
    {
        public string StandardName { get; set; }
        public string OverviewOfRole { get; set; }
        public int StandardLevel { get; set; }
        public string StandardReference { get; set; }
        public int TotalActiveApprentices { get; set; }
        public int TotalCompletedAssessments { get; set; }
        public string Sector { get; set; }
        public int TypicalDuration { get; set; }
        public string ApprovedForDelivery { get; set; }
        public string MaxFunding { get; set; }
        public string Trailblazer { get; set; }
        public string StandardPageUrl { get; set; }
        public string EqaProviderName { get; set; }
        public string EqaProviderContactName { get; set; }
        public string EqaProviderContactEmail { get; set; }
        public string EqaProviderWebLink { get; set; }
    }

    public class OppFinderApprovedStandardRegionResult
    {
        public string Region { get; set; }
        public string EndPointAssessorsNames { get; set; }
        public int EndPointAssessors { get; set; }
        public int ActiveApprentices { get; set; }
        public int CompletedAssessments { get; set; }
    }
}
