﻿using System.Collections.Generic;

namespace SFA.DAS.AssessorService.Data.IntegrationTests.Models
{
    public class StandardCollationModel : TestModel
    {
        public int? StandardId { get; set; }
        public string ReferenceNumber { get; set; }
        public string Title { get; set; }
        public string StandardData { get; set; }
        public int IsLive { get; set; }
        public List<OptionDataModel> Options { get; set; }
    }
}
