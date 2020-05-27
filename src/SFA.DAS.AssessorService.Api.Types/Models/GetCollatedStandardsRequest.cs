﻿using System.Collections.Generic;
using MediatR;
using SFA.DAS.AssessorService.Domain.Entities;

namespace SFA.DAS.AssessorService.Api.Types.Models
{
    public class GetCollatedStandardsRequest : IRequest<List<StandardCollation>>
    {
    }
}
