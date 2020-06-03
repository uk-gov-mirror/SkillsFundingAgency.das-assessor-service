﻿using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using SFA.DAS.AssessorService.Api.Types.Models;
using SFA.DAS.AssessorService.Application.Interfaces;

namespace SFA.DAS.AssessorService.Application.Handlers.BatchLogs
{
    public class GetBatchFromPeriodAndBatchNumberHandler : IRequestHandler<GetBatchFromBatchNumberRequest, BatchLogResponse>
    {
        private readonly IBatchLogQueryRepository _batchLogQueryRepository;

        public GetBatchFromPeriodAndBatchNumberHandler(IBatchLogQueryRepository batchLogQueryRepository)
        {
            _batchLogQueryRepository = batchLogQueryRepository;
        }

        public async Task<BatchLogResponse> Handle(GetBatchFromBatchNumberRequest request, CancellationToken cancellationToken)
        {
            var batchLog = await _batchLogQueryRepository.Get(int.Parse(request.BatchNumber));
            return Mapper.Map<BatchLogResponse>(batchLog);
        }
    }
}
