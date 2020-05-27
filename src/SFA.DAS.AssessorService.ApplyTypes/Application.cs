using SFA.DAS.AssessorService.Domain.Entities;
using SFA.DAS.AssessorService.Domain.JsonData;
using System;

namespace SFA.DAS.AssessorService.ApplyTypes
{
    public class Application : ApplyTypeBase
    {
        public Organisation ApplyingOrganisation { get; set; }
        public Guid ApplyingOrganisationId { get; set; }
        public DateTime WithdrawnAt { get; set; }
        public string WithdrawnBy { get; set; }
        public string ApplicationStatus { get; set; }
        public ApplicationData ApplicationData { get; set; }
    }

    public class ApplicationData
    {
        public string OrganisationReferenceId { get; set; }
        public string OrganisationName { get; set; }
        public string ReferenceNumber { get; set; }
        public string StandardName { get; set; }
        public string OrganisationType { get; set; }
        public int? StandardCode { get; set; }
        public string TradingName { get; set; }
        public bool UseTradingName { get; set; }
        public string ContactGivenName { get; set; }

        public CompaniesHouseSummary CompanySummary { get; set; }
        public CharityCommissionSummary CharitySummary { get; set; }
    }

    public class StandardApplicationData
    {
        public string StandardName { get; set; }
        public int StandardCode { get; set; }
        public Guid UserId { get; set; }
        public string UserEmail { get; set; }
    }   

    public class Feedback
    {
        public DateTime? Feedbackdate { get; set; }
        public string FeedbackBy { get; set; }
        public bool FeedbackAnswered { get; set; }
        public DateTime? FeedbackAnsweredDate { get; set; }
        public string FeedbackAnsweredBy { get; set; }
    }   
}
