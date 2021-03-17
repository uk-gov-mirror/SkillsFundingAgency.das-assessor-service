﻿using Microsoft.Extensions.Logging;
using SFA.DAS.AssessorService.Api.Types.Models.Standards;
using SFA.DAS.AssessorService.Application.Interfaces;
using SFA.DAS.AssessorService.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SFA.DAS.AssessorService.Application.Infrastructure.OuterApi;

namespace SFA.DAS.AssessorService.Application.Api.Services
{
    public class StandardService : IStandardService
    {
        private readonly CacheService _cacheService;
        private readonly IOuterApiClient _outerApiClient;
        private readonly ILogger<StandardService> _logger;
        private readonly IStandardRepository _standardRepository;

        public StandardService(CacheService cacheService, IOuterApiClient outerApiClient, ILogger<StandardService> logger, IStandardRepository standardRepository)
        {
            _cacheService = cacheService;
            _outerApiClient = outerApiClient;
            _logger = logger;
            _standardRepository = standardRepository;
        }

        public async Task LoadStandards(IEnumerable<GetStandardByIdResponse> standards)
        {
            Func<GetStandardByIdResponse, Standard> MapGetStandardsListItemToStandard = source => new Standard 
            {
                StandardUId = source.StandardUId,
                IfateReferenceNumber = source.IfateReferenceNumber,
                LarsCode = source.LarsCode,
                Title = source.Title,
                Version = source.Version,
                Level = source.Level,
                Status = source.Status,
                TypicalDuration = source.TypicalDuration,
                MaxFunding = source.MaxFunding,
                IsActive = source.IsActive,
                LastDateStarts = source.StandardDates?.LastDateStarts,
                EffectiveFrom = source.StandardDates?.EffectiveFrom,
                EffectiveTo = source.StandardDates?.EffectiveTo,
                VersionApprovedForDelivery = source.VersionDetail.ApprovedForDelivery,
                VersionEarliestStartDate = source.VersionDetail.EarliestStartDate,
                VersionLatestEndDate = source.VersionDetail.LatestEndDate,
                VersionLatestStartDate = source.VersionDetail.LatestStartDate,
                ProposedMaxFunding = source.VersionDetail.ProposedMaxFunding,
                ProposedTypicalDuration = source.VersionDetail.ProposedTypicalDuration
            };

            await _standardRepository.DeleteAll();

            var tasks = standards.Select(MapGetStandardsListItemToStandard).Select(_standardRepository.Insert);

            await Task.WhenAll(tasks);
        }

        public async Task UpsertStandardCollations(IEnumerable<GetStandardByIdResponse> standards)
        {
            Func<GetStandardByIdResponse, StandardCollation> MapGetStandardsListItemToStandard = source => new StandardCollation
            {
                StandardId = source.LarsCode,
                ReferenceNumber = source.IfateReferenceNumber,
                Title = source.Title,
                Options = source.Options,
                StandardData = new StandardData 
                {
                    Category = source.Route,
                    IfaStatus = source.Status,
                    //EqaProviderName = source.EqaProvider?.ProviderName,
                    EqaProviderContactName = source.EqaProvider?.ContactName,
                    //EqaProviderContactAddress = source.EqaProvider?.ContactAddress,
                    EqaProviderContactEmail = source.EqaProvider?.ContactEmail,
                    EqaProviderWebLink = source.EqaProvider?.WebLink,
                    IntegratedDegree = source.IntegratedDegree,
                    EffectiveFrom = source.StandardDates.EffectiveFrom,
                    EffectiveTo = source.StandardDates.EffectiveTo,
                    Level = source.Level,
                    LastDateForNewStarts = source.StandardDates.LastDateStarts,
                    IfaOnly = source.LarsCode == 0,
                    Duration = source.TypicalDuration,
                    MaxFunding = source.MaxFunding,
                    Trailblazer = source.TrailBlazerContact,
                    PublishedDate = source.VersionDetail.ApprovedForDelivery,
                    IsPublished = source.LarsCode > 0,
                    //Ssa1 = source.ssa1,
                    Ssa2 = source.SectorSubjectAreaTier2Description,
                    OverviewOfRole = source.OverviewOfRole,
                    IsActiveStandardInWin = source.IsActive,
                    FatUri = "",
                    //IfaUri = source.Url,
                    AssessmentPlanUrl = source.AssessmentPlanUrl,
                    StandardPageUrl = source.StandardPageUrl
                }
            };

            var standardCollations = standards.Select(MapGetStandardsListItemToStandard).ToList();

            await _standardRepository.UpsertApprovedStandards(standardCollations);
        }

        public async Task<IEnumerable<StandardCollation>> GetAllStandards()
        {
            var results = await _cacheService.RetrieveFromCache<IEnumerable<StandardCollation>>("StandardCollations");

            if (results != null)
                return results;

            var standardCollations = await _standardRepository.GetStandardCollations();

            await _cacheService.SaveToCache("StandardCollations", standardCollations, 8);
            return standardCollations;
        }

        public async Task<StandardCollation> GetStandard(int standardId)
        {
            StandardCollation standardCollation = null;

            try
            {
                standardCollation = await _standardRepository.GetStandardCollationByStandardId(standardId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"STANDARD COLLATION: Failed to get for standard id: {standardId}");
            }

            return standardCollation;
        }

        public async Task<StandardCollation> GetStandard(string referenceNumber)
        {
            StandardCollation standardCollation = null;

            try
            {
                standardCollation = await _standardRepository.GetStandardCollationByReferenceNumber(referenceNumber);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"STANDARD COLLATION: Failed to get for standard reference: {referenceNumber}");
            }

            return standardCollation;
        }


        public async Task<IEnumerable<StandardOptions>> GetStandardOptions()
        {
            try
            {
                var standardOptionsResponse = await _outerApiClient.Get<GetStandardOptionsListResponse>(new GetStandardOptionsRequest());

                return standardOptionsResponse.StandardOptions.Select(standard => new StandardOptions
                {
                    StandardUId = standard.StandardUId,
                    StandardCode = standard.LarsCode,
                    StandardReference = standard.IfateReferenceNumber,
                    Version = standard.Version,
                    CourseOption = standard.Options
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "STANDARD OPTIONS: Failed to get standard options");
            }

            return null;
        }

        public async Task<StandardOptions> GetStandardOptionsByStandardId(string id)
        {
            try
            {
                var standard = await _outerApiClient.Get<GetStandardByIdResponse>(new GetStandardByIdRequest(id));

                return new StandardOptions
                {
                    StandardUId = standard.StandardUId,
                    StandardCode = standard.LarsCode,
                    StandardReference = standard.IfateReferenceNumber,
                    Version = standard.Version,
                    CourseOption = standard.Options
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"STANDARD OPTIONS: Failed to get standard options for id {id}");
            }

            return null;
        }

        public async Task<IEnumerable<EPORegisteredStandards>> GetEpaoRegisteredStandards(string endPointAssessorOrganisationId)
        {
            var results = await _standardRepository.GetEpaoRegisteredStandards(endPointAssessorOrganisationId, int.MaxValue, 1);
            return results.PageOfResults;
        }
    }
}
