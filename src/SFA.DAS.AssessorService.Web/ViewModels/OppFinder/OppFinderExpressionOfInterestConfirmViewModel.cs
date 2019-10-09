﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using SFA.DAS.AssessorService.Api.Types.Models;

namespace SFA.DAS.AssessorService.Web.ViewModels.OppFinder
{
    public class OppFinderExpressionOfInterestConfirmViewModel
    {
        public string StandardName { get; internal set; }
        public string StandardReference { get; internal set; }
        public string StandardLevel { get; internal set; }
        public string StandardSector { get; internal set; }
        public StandardStatus StandardStatus { get; internal set; }
    }
}
