using System;
using System.Collections.Generic;

namespace SFA.DAS.AssessorService.Domain.JsonData
{
    public class Apply
    {
        public string ReferenceNumber { get; set; }
        public int? StandardCode { get; set; }
        public string StandardReference { get; set; }
        public string StandardName { get; set; }
        public List<Submission> InitSubmissions { get; set; }
        public int InitSubmissionsCount { get; set; }
        public DateTime? LatestInitSubmissionDate { get; set; }
        public DateTime? InitSubmissionFeedbackAddedDate { get; set; }
        public DateTime? InitSubmissionClosedDate { get; set; }
        public List<Submission> StandardSubmissions { get; set; }
        public int StandardSubmissionsCount { get; set; }
        public DateTime? LatestStandardSubmissionDate { get; set; }
        public DateTime? StandardSubmissionFeedbackAddedDate { get; set; }
        public DateTime? StandardSubmissionClosedDate { get; set; }
    }
}
