﻿using FluentValidation;
using Microsoft.Extensions.Localization;
using SFA.DAS.AssessorService.Web.ViewModels.Account;

namespace SFA.DAS.AssessorService.Web.Validators
{
  public class CreateAccountValidator : AbstractValidator<CreateAccountViewModel>
  {
    public CreateAccountValidator()
    {
      RuleFor(vm => vm.Email).EmailAddress().WithMessage("Email must be valid")
          .NotEmpty().WithMessage("Email must not be empty");
      RuleFor(vm => vm.FamilyName).NotEmpty().WithMessage("Family name must not be empty");
      RuleFor(vm => vm.GivenName).NotEmpty().WithMessage("Given name must not be empty");
    }
  }
}
