﻿using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Newtonsoft.Json;
using NUnit.Framework;
using SFA.DAS.AssessorService.Application.Api.Client.Clients;
using SFA.DAS.AssessorService.Domain.Entities;
using SFA.DAS.AssessorService.Domain.JsonData;
using SFA.DAS.AssessorService.Web.Controllers;
using SFA.DAS.AssessorService.Web.Infrastructure;
using SFA.DAS.AssessorService.Web.ViewModels.Certificate;
using SFA.DAS.AssessorService.Web.ViewModels.Shared;
using SFA.DAS.Testing.AutoFixture;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SFA.DAS.AssessorService.Web.UnitTests.CertificateOptionsTests
{
    [TestFixture]
    public class WhenRequestingCertificateOptionsPageNew
    {
        private Mock<ICertificateApiClient> _mockCertificateApiClient;
        private Mock<IHttpContextAccessor> _mockContextAccessor;
        private Mock<ISessionService> _mockSessionService;
        private CertificateOptionController _certificateOptionController;

        private const int Ukprn = 123456;
        private const string Username = "TestProviderUsername";
        private Guid CertificateId = Guid.NewGuid();
        private const string EpaoId = "EPAO123";

        [SetUp]
        public void SetUp()
        {
            _mockCertificateApiClient = new Mock<ICertificateApiClient>();
            _mockContextAccessor = new Mock<IHttpContextAccessor>();
            _mockSessionService = new Mock<ISessionService>();

            _mockContextAccessor.Setup(s => s.HttpContext.User.FindFirst("http://schemas.portal.com/ukprn")).Returns(new Claim("", Ukprn.ToString()));
            _mockContextAccessor.Setup(s => s.HttpContext.User.FindFirst("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/upn")).Returns(new Claim("", Username));
            _mockContextAccessor.Setup(s => s.HttpContext.User.FindFirst("http://schemas.portal.com/epaoid")).Returns(new Claim("", EpaoId));
            _mockContextAccessor.Setup(s => s.HttpContext.Request.Query).Returns(Mock.Of<IQueryCollection>());
            var certData = new CertificateData();
            var certDataString = JsonConvert.SerializeObject(certData);
            _mockCertificateApiClient.Setup(s => s.GetCertificate(It.IsAny<Guid>())).ReturnsAsync(
                new Certificate
                {
                    Id = CertificateId,
                    CertificateData = certDataString
                });

            _certificateOptionController = new CertificateOptionController(Mock.Of<ILogger<CertificateController>>(),
                _mockContextAccessor.Object,
                _mockCertificateApiClient.Object,
                _mockSessionService.Object);
        }

        [Test, MoqAutoData]
        public async Task Then_IfNoSession_RedirectsToSearchIndex()
        {
            var result = await _certificateOptionController.Option() as RedirectToActionResult;

            result.ControllerName.Should().Be("Search");
            result.ActionName.Should().Be("Index");
        }

        [Test, MoqAutoData]
        public async Task WhenOneOptionInSession_RedirectsToDeclaration(CertificateSession session, string option)
        {
            session.Options = new List<string> { option };
            var sessionString = JsonConvert.SerializeObject(session);
            _mockSessionService.Setup(s => s.Get(nameof(CertificateSession))).Returns(sessionString);

            var result = await _certificateOptionController.Option() as RedirectToActionResult;

            result.ControllerName.Should().Be("CertificateDeclaration");
            result.ActionName.Should().Be("Declare");
        }

        [Test, MoqAutoData]
        public async Task WhenOptionsInSession_LoadsOptionView(CertificateSession session, List<string> options)
        {
            session.Options = options;
            var sessionString = JsonConvert.SerializeObject(session);
            _mockSessionService.Setup(s => s.Get(nameof(CertificateSession))).Returns(sessionString);

            var result = await _certificateOptionController.Option() as ViewResult;

            result.ViewName.Should().Be("~/Views/Certificate/Option.cshtml");
            result.Model.Should().BeOfType<CertificateOptionViewModel>();
        }

        [Test, MoqAutoData]
        public async Task WhenNoOptionsInSession_ThenRedirectsToSearchIndex(CertificateSession session)
        {
            session.Options = null;
            var sessionString = JsonConvert.SerializeObject(session);
            _mockSessionService.Setup(s => s.Get(nameof(CertificateSession))).Returns(sessionString);

            var result = await _certificateOptionController.Option() as RedirectToActionResult;

            result.ControllerName.Should().Be("Search");
            result.ActionName.Should().Be("Index");
        }

        [Test, MoqAutoData]
        public async Task WhenPostingBackToOptionsPage_RedirectsToCertificateDeclaration(CertificateSession session, CertificateOptionViewModel model)
        {
            session.Options = null;
            var sessionString = JsonConvert.SerializeObject(session);
            _mockSessionService.Setup(s => s.Get(nameof(CertificateSession))).Returns(sessionString);

            var result = await _certificateOptionController.Option(model) as RedirectToActionResult;

            result.ControllerName.Should().Be("CertificateDeclaration");
            result.ActionName.Should().Be("Declare");
        }               

        [Test, MoqAutoData]
        public void AndClickingBack_RedirectsToSearchIndex_IfNoSession()
        {
            var result = _certificateOptionController.Back() as RedirectToActionResult;

            result.ControllerName.Should().Be("Search");
            result.ActionName.Should().Be("Index");
        }

        [Test, MoqAutoData]
        public void AndClickingBack_RedirectsToSearchIndex_IfNoVersionsInSession(CertificateSession session)
        {
            session.Versions = null;
            var sessionString = JsonConvert.SerializeObject(session);
            _mockSessionService.Setup(s => s.Get(nameof(CertificateSession))).Returns(sessionString);
            var result = _certificateOptionController.Back() as RedirectToActionResult;

            result.ControllerName.Should().Be("Search");
            result.ActionName.Should().Be("Index");
        }

        [Test, MoqAutoData]
        public void AndClickingBack_RedirectsToCertificateVersion_IfMultipleVersionsInSession_AndRedirectFromVersionSet(CertificateSession session, List<StandardVersionViewModel> standardVersions)
        {
            session.Versions = standardVersions;
            var sessionString = JsonConvert.SerializeObject(session);
            _mockSessionService.Setup(s => s.Get(nameof(CertificateSession))).Returns(sessionString);

            var expectedValue = true;
            _mockSessionService.Setup(s => s.TryGet<bool>("redirecttocheck", out expectedValue));
            _mockSessionService.Setup(s => s.TryGet<bool>("redirectedfromversion", out expectedValue));

            var result = _certificateOptionController.Back() as RedirectToActionResult;

            result.ControllerName.Should().Be("CertificateVersion");
            result.ActionName.Should().Be("Version");
            result.RouteValues.ContainsKey("redirecttocheck").Should().BeTrue();
        }

        [Test, MoqAutoData]
        public void AndClickingBack_RedirectsToCheckPage_IfHasVersions_AndRedirectToCheckSet(CertificateSession session, List<StandardVersionViewModel> standardVersions)
        {
            session.Versions = standardVersions;
            var sessionString = JsonConvert.SerializeObject(session);
            _mockSessionService.Setup(s => s.Get(nameof(CertificateSession))).Returns(sessionString);
            var expectedValue = true;
            _mockSessionService.Setup(s => s.TryGet<bool>("redirecttocheck", out expectedValue));

            var result = _certificateOptionController.Back() as RedirectToActionResult;

            result.ControllerName.Should().Be("CertificateCheck");
            result.ActionName.Should().Be("Check");
        }

        [Test, MoqAutoData]
        public void AndClickingBack_RedirectsToSearchResultPage_IfHasOneVersion(CertificateSession session, StandardVersionViewModel standardVersion)
        {
            session.Versions = new List<StandardVersionViewModel> { standardVersion };
            var sessionString = JsonConvert.SerializeObject(session);
            _mockSessionService.Setup(s => s.Get(nameof(CertificateSession))).Returns(sessionString);
            
            var result = _certificateOptionController.Back() as RedirectToActionResult;

            result.ControllerName.Should().Be("Search");
            result.ActionName.Should().Be("Result");
        }

        [Test, MoqAutoData]
        public void AndClickingBack_RedirectsToVersionSelectPage_IfHasMoreThanOneVersion(CertificateSession session, List<StandardVersionViewModel> standardVersions)
        {
            session.Versions = standardVersions;
            var sessionString = JsonConvert.SerializeObject(session);
            _mockSessionService.Setup(s => s.Get(nameof(CertificateSession))).Returns(sessionString);

            var result = _certificateOptionController.Back() as RedirectToActionResult;

            result.ControllerName.Should().Be("CertificateVersion");
            result.ActionName.Should().Be("Version");
        }
    }
}
