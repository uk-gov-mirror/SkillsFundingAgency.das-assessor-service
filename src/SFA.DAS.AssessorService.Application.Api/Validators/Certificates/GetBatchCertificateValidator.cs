﻿using FluentValidation;
using FluentValidation.Results;
using Microsoft.Extensions.Localization;
using SFA.DAS.AssessorService.Api.Types.Models.Certificates.Batch;
using SFA.DAS.AssessorService.Application.Interfaces;
using System;
using System.Linq;

namespace SFA.DAS.AssessorService.Application.Api.Validators.Certificates
{
    public class GetBatchCertificateRequestValidator : AbstractValidator<GetBatchCertificateRequest>
    {
        public GetBatchCertificateRequestValidator(IStringLocalizer<GetBatchCertificateRequestValidator> localiser, IOrganisationQueryRepository organisationQueryRepository, IIlrRepository ilrRepository, ICertificateRepository certificateRepository, IStandardService standardService)
        {
            RuleFor(m => m.UkPrn).InclusiveBetween(10000000, 99999999).WithMessage("The UKPRN should contain exactly 8 numbers");
            RuleFor(m => m.Email).NotEmpty();

            RuleFor(m => m.FamilyName).NotEmpty().WithMessage("Enter the apprentice's last name");
            RuleFor(m => m.StandardCode).GreaterThan(0).WithMessage("A Standard should be selected").DependentRules(() =>
            {
                RuleFor(m => m).CustomAsync(async (m, context, cancellation) =>
                {
                    if (!string.IsNullOrEmpty(m.StandardReference))
                    {
                        var collatedStandard = await standardService.GetStandard(m.StandardReference);
                        if (m.StandardCode != collatedStandard?.StandardId)
                        {
                            context.AddFailure("StandardReference and StandardCode relate to different standards");
                        }
                    }
                });
            });

            RuleFor(m => m.Uln).InclusiveBetween(1000000000, 9999999999).WithMessage("The apprentice's ULN should contain exactly 10 numbers").DependentRules(() =>
            {
                When(m => m.StandardCode > 0 && !string.IsNullOrEmpty(m.FamilyName), () =>
                {
                    RuleFor(m => m).CustomAsync(async (m, context, cancellation) =>
                    {
                        // TODO: FUTURE WORK - consider comment below. Currently we're making the Certificate & ILR record both mandatory
                        var requestedIlr = await ilrRepository.Get(m.Uln, m.StandardCode);
                        var sumbittingEpao = await organisationQueryRepository.GetByUkPrn(m.UkPrn);

                        if (requestedIlr is null || !string.Equals(requestedIlr.FamilyName, m.FamilyName, StringComparison.InvariantCultureIgnoreCase))
                        {
                            context.AddFailure(new ValidationFailure("Uln", "Cannot find apprentice with the specified Uln, FamilyName & Standard"));
                        }
                        else if (sumbittingEpao is null)
                        {
                            context.AddFailure(new ValidationFailure("UkPrn", "Cannot find EPAO for specified UkPrn"));
                        }
                        else
                        {
                            var providedStandards = await standardService.GetEpaoRegisteredStandards(sumbittingEpao.EndPointAssessorOrganisationId);

                            if (!providedStandards.Any(s => s.StandardCode == m.StandardCode))
                            {
                                context.AddFailure(new ValidationFailure("StandardCode", "EPAO is not registered for this Standard"));
                            }
                        }
                    });

                    RuleFor(m => m).CustomAsync(async (m, context, cancellation) =>
                    {
                        var existingCertificate = await certificateRepository.GetCertificate(m.Uln, m.StandardCode);

                        if (existingCertificate is null)
                        {
                            // TODO: FUTURE WORK - Do Alan's Certificate Search THEN the ILR Search (which may be the validation down below)
                        }
                        else if (!existingCertificate.CertificateData.Contains(m.FamilyName, StringComparison.InvariantCultureIgnoreCase))
                        {
                            context.AddFailure(new ValidationFailure("FamilyName", $"Invalid family name"));
                        }
                    });
                });
            });
        }
    }
}
