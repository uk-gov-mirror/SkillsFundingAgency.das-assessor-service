using SFA.DAS.AssessorService.Domain.JsonData;

namespace SFA.DAS.AssessorService.Domain.Entities
{
    public class StandardNonApprovedCollation
    {
        public int Id { get; set; }
        public string ReferenceNumber { get; set; }
        public string Title { get; set; }
        public StandardNonApprovedData StandardData { get; set; }
    }
}
