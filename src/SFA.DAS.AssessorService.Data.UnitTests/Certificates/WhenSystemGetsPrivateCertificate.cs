﻿using System.Collections.Generic;
using System.Data;
using System.Linq;
using FizzWare.NBuilder;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using SFA.DAS.AssessorService.Application.Interfaces;
using SFA.DAS.AssessorService.Domain.Entities;

namespace SFA.DAS.AssessorService.Data.UnitTests.Certificates
{
    public class WhenSystemGetsPrivateCertificates
    {
        private CertificateRepository _certificateRepository;
        private Mock<AssessorDbContext> _mockDbContext;
        private Mock<IUnitOfWork> _mockUnitOfWork;
        private Certificate _result;

        [SetUp]
        public void Arrange()
        {
            MappingBootstrapper.Initialize();

            var mockSet = CreateCertificateMockDbSet();
            _mockDbContext = CreateMockDbContext(mockSet);
            _mockUnitOfWork = new Mock<IUnitOfWork>();

            _certificateRepository = new CertificateRepository(_mockUnitOfWork.Object, _mockDbContext.Object);
        
          _result = _certificateRepository.GetPrivateCertificate(1111111111, "EPA0001").Result;
        }

        [Test]
        public void ItShouldReturnResult()
        {
            _result.Uln.Should().Be(1111111111);
        }

        private Mock<DbSet<Certificate>> CreateCertificateMockDbSet()
        {
            var certificates = Builder<Certificate>.CreateListOfSize(10)
                .All()
                .With(x => x.Organisation = Builder<Organisation>.CreateNew().Build())
                .With(x => x.Uln = 1111111111)
                .With(x => x.Organisation.EndPointAssessorOrganisationId = "EPA0001")
                .With(x => x.IsPrivatelyFunded = true)
                .Build()                
                .AsQueryable();

            var mockSet = new Mock<DbSet<Certificate>>();

            mockSet.As<IAsyncEnumerable<Certificate>>()
                .Setup(m => m.GetEnumerator())
                .Returns(new TestAsyncEnumerator<Certificate>(certificates.GetEnumerator()));

            mockSet.As<IQueryable<Certificate>>()
                .Setup(m => m.Provider)
                .Returns(new TestAsyncQueryProvider<Certificate>(certificates.Provider));

            mockSet.As<IQueryable<Certificate>>().Setup(m => m.Expression).Returns(certificates.Expression);
            mockSet.As<IQueryable<Certificate>>().Setup(m => m.ElementType).Returns(certificates.ElementType);
            mockSet.As<IQueryable<Certificate>>().Setup(m => m.GetEnumerator()).Returns(() => certificates.GetEnumerator());

            return mockSet;
        }        

        private Mock<AssessorDbContext> CreateMockDbContext(Mock<DbSet<Certificate>> certificateMockDbSet)
        {
            var mockDbContext = new Mock<AssessorDbContext>();
            mockDbContext.Setup(c => c.Certificates).Returns(certificateMockDbSet.Object);
            return mockDbContext;
        }
    }
}

