﻿using System;
using System.Data;
using System.Data.SqlClient;
using Dapper;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Transactions;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using SFA.DAS.AssessorService.Application.Interfaces;
using SFA.DAS.AssessorService.Domain.Entities.AssessmentOrganisations;
using SFA.DAS.AssessorService.Settings;

namespace SFA.DAS.AssessorService.Data
{
    public class AssessmentOrgsRepository : IAssessmentOrgsRepository
    {
        private readonly IWebConfiguration _configuration;
        private readonly ILogger<AssessmentOrgsRepository> _logger;


        public AssessmentOrgsRepository(IWebConfiguration configuration, ILogger<AssessmentOrgsRepository> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }

        public string TearDownData()
        {
            var progressStatus = new StringBuilder();
            try
            {
                var connectionString = _configuration.SqlConnectionString;

                progressStatus.Append("Teardown: Opening connection to database; ");
                using (var connection = new SqlConnection(connectionString))
                {
                    if (connection.State != ConnectionState.Open)
                        connection.Open();

                    progressStatus.Append("Teardown: DELETING all items in [OrganisationStandardDeliveryArea]; ");
                    connection.Execute("DELETE FROM [OrganisationStandardDeliveryArea]");
                    progressStatus.Append("Teardown: DELETING all items in [OrganisationStandard]; ");
                    connection.Execute("DELETE FROM [OrganisationStandard]");
                    

                    // MFCMFC FIrst point of deletion with logic
                    progressStatus.Append("Teardown: DELETING selected [Contacts]; ");
                    connection.Execute("DELETE FROM [contacts] WHERE username LIKE 'unknown%'");
                    progressStatus.Append("Teardown: DELETING selected [Organisations]; ");
                    // MFCMFC Second point of deleteion with logic
                    connection.Execute("DELETE FROM [organisations] where OrganisationTypeId is not null and Id not in (select organisationid from [contacts])");

                }
            }
            catch (Exception e)
            {
                progressStatus.Append("Teardown: DELETION Error; ");
                _logger.LogError($"Progress status: {progressStatus}",e);

                throw;
            }

            progressStatus.Append("Teardown: Complete; ");
            _logger.LogInformation($"Progress status: {progressStatus}");

            return progressStatus.ToString();

        }

        public void WriteDeliveryAreas(List<DeliveryArea> deliveryAreas)
        {
            // MFCMFC sanity check
            var connectionString = _configuration.SqlConnectionString;
            using (var connection = new SqlConnection(connectionString))
            {
                if (connection.State != ConnectionState.Open)
                    connection.Open();

                var currentNumber = connection.ExecuteScalar("select count(0) from [DeliveryArea]").ToString();
                if (currentNumber == "0")
                {
                    IDbTransaction transaction = connection.BeginTransaction();
                    connection.Execute(
                        "set identity_insert [DeliveryArea] ON; INSERT INTO [DeliveryArea] ([id], [Area],[Status]) VALUES (@Id, @Area, @Status); set identity_insert[DeliveryArea] OFF; ",
                        deliveryAreas, transaction);
                    transaction.Commit();
                }

                connection.Close();
            }
        }

        public void WriteOrganisationTypes(List<TypeOfOrganisation> organisationTypes)
        {
            // MFCMFC sanity check
            var connectionString = _configuration.SqlConnectionString;
            using (var connection = new SqlConnection(connectionString))
            {
                if (connection.State != ConnectionState.Open)
                    connection.Open();

                var organisationTypesToInsert = new List<TypeOfOrganisation>();

                foreach (var organisationType in organisationTypes)
                {
                    var currentNumber = connection
                        .ExecuteScalar(
                            "select count(0) from [OrganisationType] where Type = @Type",
                            organisationType).ToString();
                    if (currentNumber == "0")
                    {
                        organisationTypesToInsert.Add(organisationType);
                    }
                }

                IDbTransaction transaction = connection.BeginTransaction();
                connection.Execute(
                    "set identity_insert [OrganisationType] ON; INSERT INTO [OrganisationType] (Id, [Type], [Status]) VALUES (@Id, @Type, @Status); set identity_insert [OrganisationType] OFF; ",
                    organisationTypesToInsert, transaction);
                transaction.Commit();
             
               connection.Close();
            }
        }

        public void WriteOrganisations(List<EpaOrganisation> organisations)
        {
            //MFCMFC sanity check  -- only inserting new ones, only updating existing ones with new data
            var connectionString = _configuration.SqlConnectionString;
            using (var connection = new SqlConnection(connectionString))
            {
                if (connection.State != ConnectionState.Open)
                    connection.Open();

                var organisationsToInsert = new List<EpaOrganisation>();
                var organisationsToUpdate = new List<EpaOrganisation>();

                foreach (var organisation in organisations)
                {
                    var currentCount = connection
                        .ExecuteScalar(
                            "select count(0) from [Organisations] where EndPointAssessorOrganisationId = @EndPointAssessorOrganisationId", organisation)
                            .ToString();

                    var organisationData = new OrganisationData
                    {
                        WebsiteLink = MakeStringSuitableForJson(organisation.OrganisationData?.WebsiteLink),
                        LegalName = MakeStringSuitableForJson(organisation.OrganisationData?.LegalName),
                        Address1 = MakeStringSuitableForJson(organisation.OrganisationData?.Address1),
                        Address2 = MakeStringSuitableForJson(organisation.OrganisationData?.Address2),
                        Address3 = MakeStringSuitableForJson(organisation.OrganisationData?.Address3),
                        Address4 = MakeStringSuitableForJson(organisation.OrganisationData?.Address4),
                        Postcode = MakeStringSuitableForJson(organisation.OrganisationData?.Postcode)
                    };

                    organisation.OrganisationData = organisationData;

                    if (currentCount == "0")
                    {
                      
                        organisationsToInsert.Add(organisation);
                    }
                    else
                    {
                        organisationsToUpdate.Add(organisation);
                    }
                }
              
                var sql = new StringBuilder();

                foreach (var org in organisationsToInsert)
                {
                    var id = ConvertStringToSqlValueString(org.Id.ToString());
                    var organisationData = JsonConvert.SerializeObject(org.OrganisationData);
                    var ukprn = ConvertIntToSqlValueString(org.EndPointAssessorUkprn);
                    var endPointAssessorName = ConvertStringToSqlValueString(org.EndPointAssessorName);

                    var sqlToAppend =
                        "INSERT INTO [Organisations] ([Id] ,[CreatedAt] ,[DeletedAt],[EndPointAssessorName] ,[EndPointAssessorOrganisationId], " +
                        "[EndPointAssessorUkprn],[PrimaryContact],[Status],[UpdatedAt],[OrganisationTypeId],[OrganisationData]) VALUES (" +
                        $@" {id}, getdate(), null, {endPointAssessorName}, '{org.EndPointAssessorOrganisationId}'," +
                        $@"{ukprn}, null, '{org.Status}', null,  {org.OrganisationTypeId}, '{organisationData}' ); ";
                    sql.Append(sqlToAppend);
                }

                foreach (var org in organisationsToUpdate)
                {      
                    var organisationData = JsonConvert.SerializeObject(org.OrganisationData);
                
                    var sqlToAppend =
                        $@"UPDATE [Organisations] SET [OrganisationTypeId] = {org.OrganisationTypeId}," +
                        $@"[OrganisationData] = '{organisationData}' "+
                        $@"WHERE EndPointAssessorOrganisationId = '{org.EndPointAssessorOrganisationId}'; ";
                    sql.Append(sqlToAppend);
                }
                connection.Execute(sql.ToString());                           
                connection.Close();
            }
        }

        public List<EpaOrganisationStandard> WriteEpaOrganisationStandards(List<EpaOrganisationStandard> orgStandards)
        {
            //MFCMFC sanity check only insert standards if none exist? Should this be cleverer?
            var connectionString = _configuration.SqlConnectionString;
            var organisationStandardsFromDatabase = new List<EpaOrganisationStandard>();

            using (var connection = new SqlConnection(connectionString))
            {
                if (connection.State != ConnectionState.Open)
                    connection.Open();
                var sql = new StringBuilder();

                var currentNumber = connection.ExecuteScalar("select count(0) from [OrganisationStandard]").ToString();
                if (currentNumber == "0")
                {
                   
                        foreach (var organisationStandard in orgStandards)
                        {

                            var comments = ConvertStringToSqlValueString(organisationStandard.Comments);
                            var effectiveFrom = ConvertDateToSqlValueString(organisationStandard.EffectiveFrom);
                            var effectiveTo = ConvertDateToSqlValueString(organisationStandard.EffectiveTo);
                            var dateStandardApprovedOnRegister =
                                ConvertDateToSqlValueString(organisationStandard.DateStandardApprovedOnRegister);


                            var sqlToInsert = "INSERT INTO [OrganisationStandard] ([EndPointAssessorOrganisationId],[StandardCode],[EffectiveFrom],[EffectiveTo],[DateStandardApprovedOnRegister],[Comments],[Status])" +
                                $"VALUES ('{organisationStandard.EndPointAssessorOrganisationId}' ,'{organisationStandard.StandardCode}' ,{effectiveFrom} ,{effectiveTo} ,{dateStandardApprovedOnRegister} ,{comments} ,'{organisationStandard.Status}'); ";

                            sql.Append(sqlToInsert);
                        }
                    connection.Execute(sql.ToString());
                    organisationStandardsFromDatabase = connection.QueryAsync<EpaOrganisationStandard>("select * from [OrganisationStandard]").Result.ToList();                
                    connection.Close();
                }
            }

            return organisationStandardsFromDatabase.ToList();
        }

        public void WriteStandardDeliveryAreas(
            List<EpaOrganisationStandardDeliveryArea> organisationStandardDeliveryAreas,
            List<EpaOrganisationStandard> organisationStandards)
        {
            //MFCMFC only write if there are none present... does this need to be cleverer?
            var connectionString = _configuration.SqlConnectionString;
            var sql = new StringBuilder();

            using (var connection = new SqlConnection(connectionString))
            {
                if (connection.State != ConnectionState.Open)
                    connection.Open();

                var currentNumber = connection.ExecuteScalar("select count(0) from [OrganisationStandardDeliveryArea]")
                    .ToString();
                if (currentNumber == "0")
                {
                    foreach (var orgStandardDeliveryArea in organisationStandardDeliveryAreas)
                    {

                        var orgStandard = organisationStandards.FirstOrDefault(
                            x => x.EndPointAssessorOrganisationId ==
                                 orgStandardDeliveryArea.EndPointAssessorOrganisationId &&
                                 x.StandardCode == orgStandardDeliveryArea.StandardCode);

                        if (orgStandard != null)
                        {
                            orgStandardDeliveryArea.OrganisationStandardId = orgStandard.Id;
                        }
                    }
                }

                var orgStandardDeliveryAreasToProcess =
                    organisationStandardDeliveryAreas.Where(x => x.OrganisationStandardId != 0);
              
                foreach (var organisationStandardDeliveryArea in orgStandardDeliveryAreasToProcess)
                {
                    sql.Append($@"INSERT INTO [OrganisationStandardDeliveryArea]
                                        ([OrganisationStandardId]
                                        ,[DeliveryAreaId]
                                        ,[Comments]
                                        ,[Status])
                                    VALUES
                                        ('{organisationStandardDeliveryArea.OrganisationStandardId}'
                                        , {organisationStandardDeliveryArea.DeliveryAreaId}
                                        , '{organisationStandardDeliveryArea.Comments}'
                                        , '{organisationStandardDeliveryArea.Status}'); ");

                }
                connection.Execute(sql.ToString());
                connection.Close();
            }
        }

        public void WriteOrganisationContacts(List<OrganisationContact> contacts)
        {
            // MFC insert new records if email /organisationId isn't present, otherwise update phonenumber - does this need to be refined to only update if phonenumber is null???
            var connectionString = _configuration.SqlConnectionString;

            using (var connection = new SqlConnection(connectionString))
            {
                if (connection.State != ConnectionState.Open)
                    connection.Open();

                var contactsToInsert = new List<OrganisationContact>();
                var contactsToUpdate = new List<OrganisationContact>();

                foreach (var contact in contacts)
                {
                    var numberOfMatches = connection
                        .ExecuteScalar(
                            "select count(0) from [Contacts] where [EndPointAssessorOrganisationId] = @EndPointAssessorOrganisationId and email = @email",
                            contact).ToString();
                    if (numberOfMatches == "0")
                    {
                        contactsToInsert.Add(contact);
                    }
                    else
                    {
                        contactsToUpdate.Add(contact);
                    }
                }
               
                var sql = new StringBuilder();

                foreach (var contact in contactsToInsert)
                {
                    var displayName = ConvertStringToSqlValueString(contact.DisplayName);
                    var email = ConvertStringToSqlValueString(contact.Email);
                    var endPointAssessorOrganisationId =
                        ConvertStringToSqlValueString(contact.EndPointAssessorOrganisationId);
                    var organisationId = ConvertStringToSqlValueString(contact.OrganisationId.ToString());
                    var userName = ConvertStringToSqlValueString(contact.Username);
                    var phoneNumber = ConvertStringToSqlValueString(contact.PhoneNumber);

                    sql.Append(
                        "INSERT INTO [Contacts] ([Id] ,[CreatedAt] ,[DeletedAt] ,[DisplayName] ,[Email] ,[EndPointAssessorOrganisationId] ,[OrganisationId],  " +
                        "[Status], [UpdatedAt], [Username] ,[PhoneNumber]) VALUES " +
                        $@"(newid(), getdate(), null, {displayName}, {email}, {endPointAssessorOrganisationId}, {organisationId}, " +
                        $@"'{contact.Status}', getdate(), {userName}, {phoneNumber}); ");
                }

                foreach (var contact in contactsToUpdate)
                {
                    var email = ConvertStringToSqlValueString(contact.Email);
                    var endPointAssessorOrganisationId =
                        ConvertStringToSqlValueString(contact.EndPointAssessorOrganisationId);
                    var phoneNumber = ConvertStringToSqlValueString(contact.PhoneNumber);

                    sql.Append(
                        $@"UPDATE [Contacts] Set [PhoneNumber] = {phoneNumber} WHERE "+
                        $@"[EndPointAssessorOrganisationId] = {endPointAssessorOrganisationId} and email = {email}; "
                    );
                }

                connection.Execute(sql.ToString());
                connection.Close();
            }
        }

        private static string ConvertStringToSqlValueString(string stringToProcess)
        {
            return stringToProcess == null
                ? "null"
                : $@"'{stringToProcess.Replace("'","''")}'";
        }

        private static string MakeStringSuitableForJson(string stringToProcess)
        {
            return stringToProcess == null
                ? "null"
                : $@"{stringToProcess.Replace("'", "''")}";
        }

        private static string ConvertIntToSqlValueString(int? intToProcess)
        {
            return intToProcess == null
                ? "null"
                : $@"{intToProcess}";
        }

        private static string  ConvertDateToSqlValueString (DateTime? dateToProcess)
        {           
            if (dateToProcess == null)
                return  "null";

            return dateToProcess.Value< new DateTime(1980,1,1) 
                ? "null" 
                : $"'{dateToProcess.Value:yyyy-MM-dd}'";
        }   
    }
}
