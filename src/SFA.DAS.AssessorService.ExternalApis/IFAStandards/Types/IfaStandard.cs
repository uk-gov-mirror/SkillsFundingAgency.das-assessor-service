﻿using System;
using System.Collections.Generic;

namespace SFA.DAS.AssessorService.ExternalApis.IFAStandards.Types
{
    public class IfaStandard
    {
        public int LarsCode { get; set; }
        public string ReferenceNumber { get; set; }
        public double? Version { get; set; }
        public string Title { get; set; }
        public string Status { get; set; }
        public string Url { get; set; }
        public string OverviewOfRole { get; set; }
        public int Level { get; set; }
        public int? TypicalDuration { get; set; }
        public int? MaxFunding { get; set; }
        public string Route { get; set; }
        public string AssessmentPlanUrl { get; set; }
        public string Ssa1 { get; set; }
        public string Ssa2 { get; set; }
        public List<IfaStandardOption> Options { get; set; }
        public string[] OptionsUnstructuredTemplate { get; set; }
        public DateTime? ApprovedForDelivery { get; set; }
        public string IntegratedDegree { get; set; }
        public string TbMainContact { get; set; }
        public IfaStandardEqaProvider EqaProvider { get; set; }
        public string StandardPageUrl { get; set; }
        public bool IsPublished { get; set; }

        public List<string> GetOptionTitles()
        {
            var options = new List<string>();

            // the options are being migrated from the unstructured array to a list however EPAO only requires the title 
            if ((OptionsUnstructuredTemplate?.Length ?? 0) > 0)
            {
                options.AddRange(OptionsUnstructuredTemplate);
            }
            else if ((Options?.Count ?? 0) > 0)
            {
                options.AddRange(Options.ConvertAll(p => p.Title));
            }

            return options;
        }
    }

    public class IfaStandardEqaProvider
    {
        public string ProviderName { get; set; }
        public string ContactName { get; set; }
        public string ContactAddress { get; set; }
        public string ContactEmail { get; set; }
        public string WebLink { get; set; }
    }

    public class IfaStandardOption
    {
        public string Title { get; set; }
    }
}