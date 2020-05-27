using MediatR;
using SFA.DAS.AssessorService.Domain.JsonData;
using System;

namespace SFA.DAS.AssessorService.Api.Types.Models.Apply.Financial.Review
{
    public class ReturnFinancialReviewRequest : IRequest
    {
        public Guid Id { get; }
        public FinancialGrade UpdatedGrade { get; }

        public ReturnFinancialReviewRequest(Guid id, FinancialGrade updatedGrade)
        {
            Id = id;
            UpdatedGrade = updatedGrade;
        }
    }
}
