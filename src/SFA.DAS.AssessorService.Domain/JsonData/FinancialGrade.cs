using System;
using System.Collections.Generic;
using System.Text;

namespace SFA.DAS.AssessorService.Domain.JsonData
{
    public class FinancialGrade
    {
        public string ApplicationReference { get; set; }
        public string SelectedGrade { get; set; }
        public string InadequateMoreInformation { get; set; }
        public DateTime? FinancialDueDate { get; set; }
        public string GradedBy { get; set; }
        public DateTime GradedDateTime { get; set; }
        public List<FinancialEvidence> FinancialEvidences { get; set; }
    }
}
