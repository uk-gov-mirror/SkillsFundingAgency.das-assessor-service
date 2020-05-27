using System.Collections.Generic;

namespace SFA.DAS.AssessorService.Domain.DTOs
{
    public class ReportDetails
    {
        public string Name { get; set; }
        public List<WorksheetDetails> Worksheets { get; set; }
    }
}
