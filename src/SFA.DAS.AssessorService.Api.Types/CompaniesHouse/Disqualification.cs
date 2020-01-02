﻿using System;

namespace SFA.DAS.AssessorService.Api.Types.CompaniesHouse
{
    public class Disqualification
    {
        public DateTime DisqualifiedFrom { get; set; }
        public DateTime DisqualifiedUntil { get; set; }
        public string CaseIdentifier { get; set; }
        public string Reason { get; set; }
        public string ReasonDescription { get; set; }
    }
}
