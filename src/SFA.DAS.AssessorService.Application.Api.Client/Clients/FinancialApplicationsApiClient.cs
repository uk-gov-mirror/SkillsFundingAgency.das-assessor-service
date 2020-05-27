using Microsoft.Extensions.Logging;
using SFA.DAS.AssessorService.Api.Types.Models.Apply.Review;
using SFA.DAS.AssessorService.ApplyTypes;
using SFA.DAS.AssessorService.Domain.JsonData;
using SFA.DAS.AssessorService.Domain.Paging;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace SFA.DAS.AssessorService.Application.Api.Client.Clients
{
    public class FinancialApplicationsApiClient : ApiClientBase, IFinancialApplicationsApiClient
    {
        public FinancialApplicationsApiClient(string baseUri, ITokenService tokenService,
            ILogger<FinancialApplicationsApiClient> logger) : base(baseUri, tokenService, logger)
        {
        }

        public FinancialApplicationsApiClient(HttpClient httpClient, ITokenService tokenService, ILogger<ApiClientBase> logger) 
            : base(httpClient, tokenService, logger)
        {
        }

        public async Task<List<FinancialApplicationSummaryItem>> GetOpenFinancialApplications()
        {
            using (var request = new HttpRequestMessage(HttpMethod.Get, $"/Financial/OpenApplications"))
            {
                return await RequestAndDeserialiseAsync<List<FinancialApplicationSummaryItem>>(request, $"Could not retrieve open financial applications");
            }
        }

        public async Task<List<FinancialApplicationSummaryItem>> GetFeedbackAddedFinancialApplications()
        {
            using (var request = new HttpRequestMessage(HttpMethod.Get, $"/Financial/FeedbackAddedApplications"))
            {
                return await RequestAndDeserialiseAsync<List<FinancialApplicationSummaryItem>>(request, $"Could not retrieve feedback added financial applications");
            }
        }

        public async Task<List<FinancialApplicationSummaryItem>> GetClosedFinancialApplications()
        {
            using (var request = new HttpRequestMessage(HttpMethod.Get, $"/Financial/ClosedApplications"))
            {
                return await RequestAndDeserialiseAsync<List<FinancialApplicationSummaryItem>>(request, $"Could not retrieve closed financial applications");
            }
        }

        public async Task StartFinancialReview(Guid applicationId, string reviewer)
        {
            using (var request = new HttpRequestMessage(HttpMethod.Post, $"/Financial/{applicationId}/StartReview"))
            {
                await PostPutRequest(request, new { reviewer });
            }
        }

        public async Task ReturnFinancialReview(Guid applicationId, FinancialGrade grade)
        {
            using (var request = new HttpRequestMessage(HttpMethod.Post, $"/Financial/{applicationId}/Return"))
            {
                await PostPutRequest(request, grade);
            }
        }
    }
}
