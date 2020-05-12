﻿using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FizzWare.NBuilder;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Moq;
using NUnit.Framework;
using SFA.DAS.AssessorService.Domain.Entities;
using SFA.DAS.AssessorService.Domain.Consts;
using SFA.DAS.AssessorService.Domain.JsonData;
using Newtonsoft.Json;

namespace SFA.DAS.AssessorService.Data.UnitTests.Certificates
{
    public class WhenSystemDeleteCertificate
    {
        private CertificateRepository _certificateRepository;        
        private Mock<AssessorDbContext> _mockDbContext;
        private Mock<IDbConnection> _mockDbConnection;
        private Exception _exception;        
        private Guid _certificateId;
        private string _incidentNumber;
        private string _reasonForChange;

        [SetUp]
        public void Arrange()
        {
            _certificateId = Guid.NewGuid();
            _incidentNumber = "INC12345";
            _reasonForChange = "Test Text Reason For Change";

            var mockCertificate = MockDbSetCreateCertificate();
            var mockCertificateLog = MockDbSetCreateCertificateLog();
            
            _mockDbContext = CreateMockDbContext(mockCertificate, mockCertificateLog);

            _mockDbConnection = new Mock<IDbConnection>();

            _certificateRepository = new CertificateRepository(_mockDbContext.Object,  _mockDbConnection.Object);
        }
        

        [Test]
        public async Task Then_Delete()
        {
            //Act
            try
            {
                await _certificateRepository.Delete(1111111111, 93, "UserName", CertificateActions.Delete);
            }
            catch (Exception exception)
            {
                _exception = exception;
            }


            //Assert
            _exception.Should().BeNull();            
        }

        [Test]
        public async Task Then_Delete_With_ReasonForChange()
        {
            //Act           
            await _certificateRepository.Delete(1111111111, 93, "UserName", CertificateActions.Delete, reasonForChange: _reasonForChange);
           
            //Assert
            var result = _certificateRepository.GetCertificateLogsFor(_certificateId);
            Assert.AreEqual(result.Result.First().ReasonForChange, _reasonForChange);
        }

        [Test]
        public async Task Then_Delete_With_IncidentNumber()
        {
            //Act
            await _certificateRepository.Delete(1111111111, 93, "UserName", CertificateActions.Delete, incidentNumber: _incidentNumber);

            //Assert
            var certificate =  _certificateRepository.GetCertificate(_certificateId);            
            var certificateData = JsonConvert.DeserializeObject<CertificateData>(certificate.Result.CertificateData);
            Assert.AreEqual(certificateData.IncidentNumber, _incidentNumber);
        }

        private Mock<DbSet<Certificate>> MockDbSetCreateCertificate()
        {
            var certificates = Builder<Certificate>.CreateListOfSize(2)
                .TheFirst(1)
                .With(x => x.Id = _certificateId)
                .With(x => x.Organisation = Builder<Organisation>.CreateNew().Build())                
                .With(x => x.CertificateData = GetCertificateData())
                .With(x => x.Uln = 1111111111)
                .With(x => x.StandardCode = 93)
                .With(x => x.Organisation.EndPointAssessorOrganisationId = "EPA0001")
                .With(x => x.IsPrivatelyFunded = true)
                .TheNext(1)
                .With(x => x.Organisation = Builder<Organisation>.CreateNew().Build())
                .With(x => x.Uln = 1111111111)
                .With(x => x.Organisation.EndPointAssessorOrganisationId = "EPA0001")
                .With(x => x.StandardCode = 100)
                .With(x => x.IsPrivatelyFunded = true)
                .Build()
                .AsQueryable();

            var mockCertificate = new Mock<DbSet<Certificate>>();

            mockCertificate.As<IQueryable<Certificate>>()
                .Setup(m => m.Provider)
                .Returns(new TestAsyncQueryProvider<Certificate>(certificates.Provider));

            mockCertificate.As<IQueryable<Certificate>>()
                .Setup(m => m.Expression)
                .Returns(certificates.Expression);

            mockCertificate.As<IQueryable<Certificate>>()
                .Setup(m => m.ElementType)
                .Returns(certificates.ElementType);

            mockCertificate.As<IQueryable<Certificate>>()
                .Setup(m => m.GetEnumerator())
                .Returns(() => certificates.GetEnumerator());

            return mockCertificate;
        }

        private Mock<DbSet<CertificateLog>> MockDbSetCreateCertificateLog()
        {
            var certificateLog = Builder<CertificateLog>.CreateListOfSize(1)
                .TheFirst(1)
                .With(x => x.Id = Guid.NewGuid())
                .With(x => x.CertificateId = _certificateId)
                .With(x => x.Action = CertificateActions.Delete)
                .With(x => x.EventTime = DateTime.UtcNow)
                .With(x => x.ReasonForChange = _reasonForChange)
                .Build()
                .AsQueryable();

            var mockCertificateLog = new Mock<DbSet<CertificateLog>>();

            mockCertificateLog.As<IQueryable<CertificateLog>>()
                .Setup(m => m.Provider)
                .Returns(new TestAsyncQueryProvider<CertificateLog>(certificateLog.Provider));

            mockCertificateLog.As<IQueryable<CertificateLog>>()
                .Setup(m => m.Expression)
                .Returns(certificateLog.Expression);

            mockCertificateLog.As<IQueryable<CertificateLog>>()
                .Setup(m => m.ElementType)
                .Returns(certificateLog.ElementType);


            mockCertificateLog.As<IQueryable<CertificateLog>>()
                .Setup(m => m.GetEnumerator())
                .Returns(() => certificateLog.GetEnumerator());

            return mockCertificateLog;
        }

        private Mock<AssessorDbContext> CreateMockDbContext(Mock<DbSet<Certificate>> certificateMockDbSet, Mock<DbSet<CertificateLog>> CreateCertificateLogMockDbSet)
        {
            var mockDbContext = new Mock<AssessorDbContext>();
            mockDbContext.Setup(c => c.Certificates).Returns(certificateMockDbSet.Object);
            mockDbContext.Setup(c => c.CertificateLogs).Returns(CreateCertificateLogMockDbSet.Object);
            return mockDbContext;
        }

        private string GetCertificateData()
        {
            var certData = new CertificateData
            {
                ContactName = "ContactName"
            };
            return JsonConvert.SerializeObject(certData);
        }
    }
}
