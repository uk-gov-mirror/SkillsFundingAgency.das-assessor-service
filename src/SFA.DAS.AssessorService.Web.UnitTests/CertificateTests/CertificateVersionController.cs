﻿using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Newtonsoft.Json;
using NUnit.Framework;
using SFA.DAS.AssessorService.Api.Types.Models.Certificates;
using SFA.DAS.AssessorService.Api.Types.Models.Standards;
using SFA.DAS.AssessorService.Application.Api.Client.Clients;
using SFA.DAS.AssessorService.Domain.Consts;
using SFA.DAS.AssessorService.Domain.JsonData;
using SFA.DAS.AssessorService.Web.Controllers;
using SFA.DAS.AssessorService.Web.Infrastructure;
using SFA.DAS.AssessorService.Web.ViewModels.Certificate;
using SFA.DAS.Testing.AutoFixture;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SFA.DAS.AssessorService.Web.UnitTests.CertificateTests
{
    public class CertificateVersionControllerTests
    {
        private Mock<IStandardServiceClient> _mockStandardServiceClient;
        private Mock<IStandardVersionClient> _mockStandardVersionClient;
        private Mock<ICertificateApiClient> _mockCertificateApiClient;
        private Mock<IHttpContextAccessor> _mockContextAccessor;
        private Mock<ISessionService> _mockSessionService;
        private CertificateVersionController _certificateVersionController;

        private const int Ukprn = 123456;
        private const string Username = "TestProviderUsername";
        private Guid CertificateId = Guid.NewGuid();

        [SetUp]
        public void SetUp()
        {
            _mockStandardServiceClient = new Mock<IStandardServiceClient>();
            _mockStandardVersionClient = new Mock<IStandardVersionClient>();
            _mockCertificateApiClient = new Mock<ICertificateApiClient>();
            _mockContextAccessor = new Mock<IHttpContextAccessor>();
            _mockSessionService = new Mock<ISessionService>();

            _mockContextAccessor.Setup(s => s.HttpContext.User.FindFirst("http://schemas.portal.com/ukprn")).Returns(new Claim("", Ukprn.ToString()));
            _mockContextAccessor.Setup(s => s.HttpContext.User.FindFirst("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/upn")).Returns(new Claim("", Username));
            _mockContextAccessor.Setup(s => s.HttpContext.Request.Query).Returns(Mock.Of<IQueryCollection>());

            var certData = new CertificateData();
            var certDataString = JsonConvert.SerializeObject(certData);
            _mockCertificateApiClient.Setup(s => s.GetCertificate(It.IsAny<Guid>())).ReturnsAsync(
                new Domain.Entities.Certificate
                {
                    Id = CertificateId,
                    CertificateData = certDataString
                });

            _certificateVersionController = new CertificateVersionController(
                Mock.Of<ILogger<CertificateController>>(),
                _mockContextAccessor.Object,
                _mockCertificateApiClient.Object,
                _mockStandardVersionClient.Object,
                _mockStandardServiceClient.Object,
                _mockSessionService.Object);

        }

        [Test, MoqAutoData]
        public async Task WhenSelectingAVersion_WhenNoSession_RedirectsBackToSearch()
        {
            var result = await _certificateVersionController.Version(false) as RedirectToActionResult;

            result.ControllerName.Should().Be("Search");
            result.ActionName.Should().Be("Index");
        }

        [Test, MoqAutoData]
        public async Task WhenSelectingAVersion_WhenLoadingModel_StoresStandardVersions(CertificateSession session, IEnumerable<StandardVersion> standardVersions)
        {
            var sessionString = JsonConvert.SerializeObject(session);
            _mockSessionService.Setup(s => s.Get("CertificateSession")).Returns(sessionString);
            
            _mockStandardVersionClient.Setup(s => s.GetStandardVersionsByLarsCode(session.StandardCode)).ReturnsAsync(standardVersions);

            var result = await _certificateVersionController.Version(false) as ViewResult;

            var versionModel = ((CertificateVersionViewModel)result.Model);

            versionModel.Should().NotBeNull();
            versionModel.Id.Should().Be(CertificateId);
            versionModel.Versions.Should().BeEquivalentTo(standardVersions.Select(s => new CertificateVersionViewModel.StandardVersion
            {
                StandardUId = s.StandardUId,
                Title = s.Title,
                Version = s.Version
            }));
        }

        [Test, MoqAutoData]
        public async Task WhenSelectingAVersion_WhenLoadingModel_WithOneVersion_RedirectsToDeclaration(CertificateSession session, StandardVersion standardVersion)
        {
            standardVersion.StandardUId = session.StandardUId;
            var sessionString = JsonConvert.SerializeObject(session);
            _mockSessionService.Setup(s => s.Get("CertificateSession")).Returns(sessionString);

            _mockStandardVersionClient.Setup(s => s.GetStandardVersionsByLarsCode(session.StandardCode)).ReturnsAsync(new List<StandardVersion> { standardVersion });
            _mockStandardServiceClient.Setup(s => s.GetStandardOptions(session.StandardUId)).ReturnsAsync(new StandardOptions());

            var result = await _certificateVersionController.Version(false) as RedirectToActionResult;

            result.ControllerName.Should().Be("CertificateDeclaration");
            result.ActionName.Should().Be("Declare");
        }

        [Test, MoqAutoData]
        public async Task WhenSelectingAVersion_WhenLoadingModel_WithOneVersion_RedirectsToOptionPage(CertificateSession session, StandardVersion standardVersion, StandardOptions options)
        {
            standardVersion.StandardUId = session.StandardUId;
            var sessionString = JsonConvert.SerializeObject(session);
            _mockSessionService.Setup(s => s.Get("CertificateSession")).Returns(sessionString);

            _mockStandardVersionClient.Setup(s => s.GetStandardVersionsByLarsCode(session.StandardCode)).ReturnsAsync(new List<StandardVersion> { standardVersion });
            _mockStandardServiceClient.Setup(s => s.GetStandardOptions(session.StandardUId)).ReturnsAsync(options);

            var result = await _certificateVersionController.Version(false) as RedirectToActionResult;

            result.ControllerName.Should().Be("CertificateOption");
            result.ActionName.Should().Be("Option");

            _mockSessionService.Verify(s => s.Set("CertificateSession", It.Is<CertificateSession>(c => c.Options.SequenceEqual(options.CourseOption.ToList()) && c.StandardUId == standardVersion.StandardUId)));
        }

        [Test, MoqAutoData]
        public async Task WhenPostingToSelectAVersion_WhenSavingModel_UpdatesCertificateWithStandardUId(CertificateVersionViewModel vm, StandardVersion standardVersion, CertificateSession session)
        {
            var sessionString = JsonConvert.SerializeObject(session);
            _mockSessionService.Setup(s => s.Get("CertificateSession")).Returns(sessionString);
            _mockStandardVersionClient.Setup(s => s.GetStandardVersionByStandardUId(vm.StandardUId)).ReturnsAsync(standardVersion);
            _mockStandardServiceClient.Setup(s => s.GetStandardOptions(vm.StandardUId)).ReturnsAsync(new StandardOptions());

            var result = await _certificateVersionController.Version(vm) as RedirectToActionResult;

            _mockCertificateApiClient.Verify(v => v.UpdateCertificate(It.Is<UpdateCertificateRequest>(c =>
                c.Certificate.StandardUId == vm.StandardUId)));

            result.ControllerName.Should().Be("CertificateDeclaration");
            result.ActionName.Should().Be("Declare");
        }

        [Test, MoqAutoData]
        public async Task WhenPostingToSelectAVersion_WhenSavingModel_IfVersionHasOptions_RedirectToOptionsPage(CertificateVersionViewModel vm, StandardVersion standardVersion, StandardOptions options, CertificateSession session)
        {
            var sessionString = JsonConvert.SerializeObject(session);
            _mockSessionService.Setup(s => s.Get("CertificateSession")).Returns(sessionString);
            standardVersion.StandardUId = vm.StandardUId;
            _mockStandardVersionClient.Setup(s => s.GetStandardVersionByStandardUId(vm.StandardUId)).ReturnsAsync(standardVersion);
            _mockStandardServiceClient.Setup(s => s.GetStandardOptions(vm.StandardUId)).ReturnsAsync(options);

            var result = await _certificateVersionController.Version(vm) as RedirectToActionResult;

            result.ControllerName.Should().Be("CertificateOption");
            result.ActionName.Should().Be("Option");
        }

        [Test, MoqAutoData]
        public async Task WhenPostingToSelectAVersion_WhenSavingModel_IfVersionHasOptions_RedirectToOptionsPageWithRedirectToCheck(CertificateVersionViewModel vm, StandardVersion standardVersion, StandardOptions options, CertificateSession session)
        {
            var sessionString = JsonConvert.SerializeObject(session);
            _mockSessionService.Setup(s => s.Get("CertificateSession")).Returns(sessionString);
            _mockStandardVersionClient.Setup(s => s.GetStandardVersionByStandardUId(vm.StandardUId)).ReturnsAsync(standardVersion);
            _mockStandardServiceClient.Setup(s => s.GetStandardOptions(vm.StandardUId)).ReturnsAsync(options);
            _mockSessionService.Setup(s => s.Exists("redirecttocheck")).Returns(true);
            _mockSessionService.Setup(s => s.Get("redirecttocheck")).Returns("true");

            var result = await _certificateVersionController.Version(vm) as RedirectToActionResult;
                    
            result.ControllerName.Should().Be("CertificateOption");
            result.ActionName.Should().Be("Option");
            result.RouteValues.Should().ContainKey("redirecttocheck");
            result.RouteValues.Should().ContainValue(true);
        }

        [Test, MoqAutoData]
        public async Task WhenPostingToSelectAVersion_WhenSavingModel_IfVersionHasChanged_ClearOptionSessionCache(CertificateVersionViewModel vm, StandardVersion standardVersion, CertificateSession session)
        {
            _mockStandardVersionClient.Setup(s => s.GetStandardVersionByStandardUId(vm.StandardUId)).ReturnsAsync(standardVersion);
            _mockStandardServiceClient.Setup(s => s.GetStandardOptions(vm.StandardUId)).ReturnsAsync(new StandardOptions());

            var sessionString = JsonConvert.SerializeObject(session);
            _mockSessionService.Setup(s => s.Get("CertificateSession")).Returns(sessionString);
        
            var result = await _certificateVersionController.Version(vm) as RedirectToActionResult;

            _mockSessionService.Verify(s => s.Set("CertificateSession", It.Is<CertificateSession>(v => v.Options == null && v.StandardUId == vm.StandardUId)));
        }
    }
}
