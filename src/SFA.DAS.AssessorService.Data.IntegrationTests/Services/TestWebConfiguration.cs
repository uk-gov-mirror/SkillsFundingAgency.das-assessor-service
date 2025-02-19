﻿using System;
using SFA.DAS.AssessorService.Settings;

namespace SFA.DAS.AssessorService.Data.IntegrationTests.Services
{

    public class TestWebConfiguration : IWebConfiguration
    {
        public ApiAuthentication ApiAuthentication { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public AzureApiAuthentication AzureApiAuthentication { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public ClientApiAuthentication AssessorApiAuthentication { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public NotificationsApiClientConfiguration NotificationsApiClientConfiguration { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public string AssessmentOrgsApiClientBaseUrl { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public string IfaApiClientBaseUrl { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public string IFATemplateStorageConnectionString { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public string SqlConnectionString { get; set; }
        public string SpecflowDBTestConnectionString { get; set; }
        public string SessionRedisConnectionString { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public ClientApiAuthentication QnaApiAuthentication { get; set; }
        public string ServiceLink { get; set; }
        public LoginServiceConfig LoginService { get; set; }
        public ClientApiAuthentication RoatpApiAuthentication { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public ReferenceDataApiAuthentication ReferenceDataApiAuthentication { get; set; }
        public CompaniesHouseApiAuthentication CompaniesHouseApiAuthentication { get; set; }
        public CharityCommissionApiAuthentication CharityCommissionApiAuthentication { get; set; }
        public string ReferenceFormat { get; set; }
        public string FeedbackUrl { get; set; }
        
        #region For External API Sandbox
        public string SandboxSqlConnectionString { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public ApiAuthentication SandboxApiAuthentication { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public ClientApiAuthentication SandboxClientApiAuthentication { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        #endregion

        public string ZenDeskSnippetKey { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public string ZenDeskSectionId { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public string ZenDeskCobrowsingSnippetKey { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public OuterApiConfiguration OuterApi { get; set; }
    }
}
