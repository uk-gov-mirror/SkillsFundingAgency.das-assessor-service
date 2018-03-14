﻿// ------------------------------------------------------------------------------
//  <auto-generated>
//      This code was generated by SpecFlow (http://www.specflow.org/).
//      SpecFlow Version:2.3.0.0
//      SpecFlow Generator Version:2.3.0.0
// 
//      Changes to this file may cause incorrect behavior and will be lost if
//      the code is regenerated.
//  </auto-generated>
// ------------------------------------------------------------------------------
#region Designer generated code
#pragma warning disable
namespace SFA.DAS.AssessorService.Application.Api.Specflow.Tests.Organisations.Maintenance
{
    using TechTalk.SpecFlow;
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("TechTalk.SpecFlow", "2.3.0.0")]
    [System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [NUnit.Framework.TestFixtureAttribute()]
    [NUnit.Framework.DescriptionAttribute("Maintain Organisations through the SFA.DAS.AssessorService.Application.Api")]
    [NUnit.Framework.CategoryAttribute("maintainOrganisations")]
    public partial class MaintainOrganisationsThroughTheSFA_DAS_AssessorService_Application_ApiFeature
    {
        
        private TechTalk.SpecFlow.ITestRunner testRunner;
        
#line 1 "OrganisationMaintenance.feature"
#line hidden
        
        [NUnit.Framework.OneTimeSetUpAttribute()]
        public virtual void FeatureSetup()
        {
            testRunner = TechTalk.SpecFlow.TestRunnerManager.GetTestRunner();
            TechTalk.SpecFlow.FeatureInfo featureInfo = new TechTalk.SpecFlow.FeatureInfo(new System.Globalization.CultureInfo("en-US"), "Maintain Organisations through the SFA.DAS.AssessorService.Application.Api", "\tIn order to be able to Modify Organisations\r\n\tAs a System\r\n\tI want to be be able" +
                    " to maintain Organisations", ProgrammingLanguage.CSharp, new string[] {
                        "maintainOrganisations"});
            testRunner.OnFeatureStart(featureInfo);
        }
        
        [NUnit.Framework.OneTimeTearDownAttribute()]
        public virtual void FeatureTearDown()
        {
            testRunner.OnFeatureEnd();
            testRunner = null;
        }
        
        [NUnit.Framework.SetUpAttribute()]
        public virtual void TestInitialize()
        {
        }
        
        [NUnit.Framework.TearDownAttribute()]
        public virtual void ScenarioTearDown()
        {
            testRunner.OnScenarioEnd();
        }
        
        public virtual void ScenarioSetup(TechTalk.SpecFlow.ScenarioInfo scenarioInfo)
        {
            testRunner.OnScenarioStart(scenarioInfo);
        }
        
        public virtual void ScenarioCleanup()
        {
            testRunner.CollectScenarioErrors();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Create an Organisation With No Primary Contact")]
        public virtual void CreateAnOrganisationWithNoPrimaryContact()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Create an Organisation With No Primary Contact", ((string[])(null)));
#line 7
this.ScenarioSetup(scenarioInfo);
#line 8
 testRunner.Given("System Has access to the SFA.DAS.AssessmentOrgs.Api", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line hidden
            TechTalk.SpecFlow.Table table1 = new TechTalk.SpecFlow.Table(new string[] {
                        "EndPointAssessorName",
                        "EndPointAssessorOrganisationId",
                        "EndPointAssessorUKPRN"});
            table1.AddRow(new string[] {
                        "Test",
                        "99999999",
                        "10033333"});
#line 9
 testRunner.When("I Create an Organisation", ((string)(null)), table1, "When ");
#line 12
 testRunner.Then("the response http status should be Created", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 13
 testRunner.And("the Location Header should be set", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 14
 testRunner.And("the Organisation should be created", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 15
 testRunner.And("the Organisation Status should be set to New", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Create an Organisation With Primary Contact that exists")]
        public virtual void CreateAnOrganisationWithPrimaryContactThatExists()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Create an Organisation With Primary Contact that exists", ((string[])(null)));
#line 17
this.ScenarioSetup(scenarioInfo);
#line 18
 testRunner.Given("System Has access to the SFA.DAS.AssessmentOrgs.Api", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line hidden
            TechTalk.SpecFlow.Table table2 = new TechTalk.SpecFlow.Table(new string[] {
                        "EndPointAssessorName",
                        "EndPointAssessorOrganisationId",
                        "EndPointAssessorUKPRN"});
            table2.AddRow(new string[] {
                        "Test",
                        "99999987",
                        "10033333"});
#line 19
  testRunner.When("I Create an Organisation With Existing Primary Contact", ((string)(null)), table2, "When ");
#line 22
 testRunner.Then("the response http status should be Created", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 23
 testRunner.And("the Location Header should be set", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 24
 testRunner.And("the Organisation should be created", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 25
 testRunner.And("the Organisation Status should be set to Live", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Create an Organisation With Invalid UkPrn")]
        public virtual void CreateAnOrganisationWithInvalidUkPrn()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Create an Organisation With Invalid UkPrn", ((string[])(null)));
#line 27
this.ScenarioSetup(scenarioInfo);
#line 28
 testRunner.Given("System Has access to the SFA.DAS.AssessmentOrgs.Api", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line hidden
            TechTalk.SpecFlow.Table table3 = new TechTalk.SpecFlow.Table(new string[] {
                        "EndPointAssessorName",
                        "EndPointAssessorOrganisationId",
                        "EndPointAssessorUKPRN"});
            table3.AddRow(new string[] {
                        "Test",
                        "99999998",
                        "14"});
#line 29
    testRunner.When("I Create an Organisation", ((string)(null)), table3, "When ");
#line 32
 testRunner.Then("the response http status should be Bad Request", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 33
 testRunner.And("the response message should contain Request must contain a valid UKPRN as defined" +
                    " in the UK Register of Learning Providers (UKRLP) is 8 digits in the format 1000" +
                    "0000 – 99999999", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Create an Organisation Which Already Exists")]
        public virtual void CreateAnOrganisationWhichAlreadyExists()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Create an Organisation Which Already Exists", ((string[])(null)));
#line 35
this.ScenarioSetup(scenarioInfo);
#line 36
 testRunner.Given("System Has access to the SFA.DAS.AssessmentOrgs.Api", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line hidden
            TechTalk.SpecFlow.Table table4 = new TechTalk.SpecFlow.Table(new string[] {
                        "EndPointAssessorName",
                        "EndPointAssessorOrganisationId",
                        "EndPointAssessorUKPRN"});
            table4.AddRow(new string[] {
                        "Test",
                        "99999988",
                        "10033333"});
#line 37
  testRunner.When("I Create an Organisation", ((string)(null)), table4, "When ");
#line hidden
            TechTalk.SpecFlow.Table table5 = new TechTalk.SpecFlow.Table(new string[] {
                        "EndPointAssessorName",
                        "EndPointAssessorOrganisationId",
                        "EndPointAssessorUKPRN"});
            table5.AddRow(new string[] {
                        "Test",
                        "99999988",
                        "10033333"});
#line 40
    testRunner.When("I Create an Organisation", ((string)(null)), table5, "When ");
#line 43
 testRunner.Then("the response http status should be Bad Request", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 44
 testRunner.And("the response message should contain Organisation Has Already Been Created", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Update an Organisation Succesfully")]
        public virtual void UpdateAnOrganisationSuccesfully()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Update an Organisation Succesfully", ((string[])(null)));
#line 46
this.ScenarioSetup(scenarioInfo);
#line 47
 testRunner.Given("System Has access to the SFA.DAS.AssessmentOrgs.Api", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line hidden
            TechTalk.SpecFlow.Table table6 = new TechTalk.SpecFlow.Table(new string[] {
                        "EndPointAssessorName",
                        "EndPointAssessorOrganisationId",
                        "EndPointAssessorUKPRN"});
            table6.AddRow(new string[] {
                        "Test Name",
                        "99999999",
                        "10033670"});
#line 48
 testRunner.When("I Update an Organisation", ((string)(null)), table6, "When ");
#line 51
 testRunner.Then("the response http status should be No Content", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 52
 testRunner.And("the Update should have occured", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Update an Organisation That does Not Exist")]
        public virtual void UpdateAnOrganisationThatDoesNotExist()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Update an Organisation That does Not Exist", ((string[])(null)));
#line 54
this.ScenarioSetup(scenarioInfo);
#line 55
 testRunner.Given("System Has access to the SFA.DAS.AssessmentOrgs.Api", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line hidden
            TechTalk.SpecFlow.Table table7 = new TechTalk.SpecFlow.Table(new string[] {
                        "EndPointAssessorName",
                        "EndPointAssessorOrganisationId",
                        "EndPointAssessorUKPRN"});
            table7.AddRow(new string[] {
                        "Test Name",
                        "99999999",
                        "10005333"});
#line 56
 testRunner.When("I Update an Organisation With invalid Id", ((string)(null)), table7, "When ");
#line 59
 testRunner.Then("the response http status should be Bad Request", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Update an Organisation with invalid PrimaryContact")]
        public virtual void UpdateAnOrganisationWithInvalidPrimaryContact()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Update an Organisation with invalid PrimaryContact", ((string[])(null)));
#line 61
this.ScenarioSetup(scenarioInfo);
#line 62
 testRunner.Given("System Has access to the SFA.DAS.AssessmentOrgs.Api", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line hidden
            TechTalk.SpecFlow.Table table8 = new TechTalk.SpecFlow.Table(new string[] {
                        "EndPointAssessorName",
                        "EndPointAssessorOrganisationId",
                        "EndPointAssessorUKPRN"});
            table8.AddRow(new string[] {
                        "Test Name",
                        "99999999",
                        "14"});
#line 63
 testRunner.When("I Update an Organisation With Invalid Primary Contact", ((string)(null)), table8, "When ");
#line 66
 testRunner.Then("the response http status should be Bad Request", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Update an Organisation with valid PrimaryContact")]
        public virtual void UpdateAnOrganisationWithValidPrimaryContact()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Update an Organisation with valid PrimaryContact", ((string[])(null)));
#line 68
this.ScenarioSetup(scenarioInfo);
#line 69
 testRunner.Given("System Has access to the SFA.DAS.AssessmentOrgs.Api", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line hidden
            TechTalk.SpecFlow.Table table9 = new TechTalk.SpecFlow.Table(new string[] {
                        "EndPointAssessorName",
                        "EndPointAssessorOrganisationId",
                        "EndPointAssessorUKPRN"});
            table9.AddRow(new string[] {
                        "Test Name",
                        "1234",
                        "10033670"});
#line 70
 testRunner.When("I Update an Organisation With valid Primary Contact", ((string)(null)), table9, "When ");
#line 73
 testRunner.Then("the response http status should be No Content", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 74
 testRunner.And("the Organisation Status should be persisted as Live", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Delete an Organisation")]
        public virtual void DeleteAnOrganisation()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Delete an Organisation", ((string[])(null)));
#line 76
this.ScenarioSetup(scenarioInfo);
#line 77
 testRunner.Given("System Has access to the SFA.DAS.AssessmentOrgs.Api", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line hidden
            TechTalk.SpecFlow.Table table10 = new TechTalk.SpecFlow.Table(new string[] {
                        "EndPointAssessorName",
                        "EndPointAssessorOrganisationId",
                        "EndPointAssessorUKPRN"});
            table10.AddRow(new string[] {
                        "Test",
                        "99999777",
                        "10033444"});
#line 78
 testRunner.When("I Delete an Organisation", ((string)(null)), table10, "When ");
#line 81
 testRunner.Then("the response http status should be No Content", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 82
 testRunner.And("the Organisation should be deleted", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Repeat Deleting an Organisation")]
        public virtual void RepeatDeletingAnOrganisation()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Repeat Deleting an Organisation", ((string[])(null)));
#line 84
this.ScenarioSetup(scenarioInfo);
#line 85
 testRunner.Given("System Has access to the SFA.DAS.AssessmentOrgs.Api", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line hidden
            TechTalk.SpecFlow.Table table11 = new TechTalk.SpecFlow.Table(new string[] {
                        "EndPointAssessorName",
                        "EndPointAssessorOrganisationId",
                        "EndPointAssessorUKPRN"});
            table11.AddRow(new string[] {
                        "Test",
                        "99999778",
                        "10033444"});
#line 86
 testRunner.When("I Delete an Organisation Twice", ((string)(null)), table11, "When ");
#line 89
 testRunner.Then("the response http status should be No Content", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 90
 testRunner.And("the Organisation should be deleted", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            this.ScenarioCleanup();
        }
    }
}
#pragma warning restore
#endregion
