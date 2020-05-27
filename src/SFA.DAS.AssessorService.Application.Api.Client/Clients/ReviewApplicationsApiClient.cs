using Microsoft.Extensions.Logging;
using SFA.DAS.AssessorService.Api.Types.Models.Apply.Review;
using SFA.DAS.AssessorService.ApplyTypes;
using SFA.DAS.AssessorService.Domain.Paging;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace SFA.DAS.AssessorService.Application.Api.Client.Clients
{
    public class ReviewApplicationsApiClient : ApiClientBase, IReviewApplicationsApiClient
    {
        public ReviewApplicationsApiClient(string baseUri, ITokenService tokenService,
            ILogger<ReviewApplicationsApiClient> logger) : base(baseUri, tokenService, logger)
        {
        }

        public ReviewApplicationsApiClient(HttpClient httpClient, ITokenService tokenService, ILogger<ApiClientBase> logger) 
            : base(httpClient, tokenService, logger)
        {
        }

        public async Task<ApplicationReviewStatusCounts> GetApplicationReviewStatusCounts()
        {
            using (var request = new HttpRequestMessage(HttpMethod.Get, $"/Review/ApplicationReviewStatusCounts"))
            {
                return await RequestAndDeserialiseAsync<ApplicationReviewStatusCounts>(request, $"Could not retrieve status counts");
            }
        }

        public async Task<PaginatedList<ApplicationSummaryItem>> GetOrganisationApplications(OrganisationApplicationsRequest organisationApplicationsRequest)
        {
            using (var request = new HttpRequestMessage(HttpMethod.Post, $"/Review/OrganisationApplications"))
            {
                return await PostPutRequestWithResponse<OrganisationApplicationsRequest, PaginatedList<ApplicationSummaryItem>>(request, organisationApplicationsRequest);   
            }
        }

        public async Task<PaginatedList<ApplicationSummaryItem>> GetStandardApplications(StandardApplicationsRequest standardApplicationsRequest)
        {
            using (var request = new HttpRequestMessage(HttpMethod.Post, $"/Review/StandardApplications"))
            {
                return await PostPutRequestWithResponse<StandardApplicationsRequest, PaginatedList<ApplicationSummaryItem>>(request, standardApplicationsRequest);
            }
        }

        public async Task StartApplicationSectionReview(Guid applicationId, int sequenceNo, int sectionNo, string reviewer)
        {
            using (var request = new HttpRequestMessage(HttpMethod.Post, $"/Review/Applications/{applicationId}/Sequences/{sequenceNo}/Sections/{sectionNo}/StartReview"))
            {
                await PostPutRequest(request, new { reviewer });
            }
        }

        public async Task EvaluateSection(Guid applicationId, int sequenceNo, int sectionNo, bool isSectionComplete, string evaluatedBy)
        {
            using (var request = new HttpRequestMessage(HttpMethod.Post, $"Review/Applications/{applicationId}/Sequences/{sequenceNo}/Sections/{sectionNo}/Evaluate"))
            {
                await PostPutRequest(request, new { isSectionComplete, evaluatedBy });
            }
        }

        public async Task ReturnApplicationSequence(Guid applicationId, int sequenceNo, string returnType, string returnedBy)
        {
            using (var request = new HttpRequestMessage(HttpMethod.Post, $"Review/Applications/{applicationId}/Sequences/{sequenceNo}/Return"))
            {
                await PostPutRequest(request, new { returnType, returnedBy });
            }
        }

        public async Task AddFeedback(Guid applicationId, int sequenceId, int sectionId, string pageId, Feedback feedback)
        {
            using (var request = new HttpRequestMessage(HttpMethod.Post, $"Review/Applications/{applicationId}/Sequences/{sequenceId}/Sections/{sectionId}/Pages/{pageId}/AddFeedback"))
            {
                await PostPutRequest(request, feedback);
            }
        }

        public async Task DeleteFeedback(Guid applicationId, int sequenceId, int sectionId, string pageId, Guid feedbackId)
        {
            using (var request = new HttpRequestMessage(HttpMethod.Post, $"Review/Applications/{applicationId}/Sequences/{sequenceId}/Sections/{sectionId}/Pages/{pageId}/DeleteFeedback"))
            {
                await PostPutRequest(request, feedbackId);
            }
        }
    }
}
