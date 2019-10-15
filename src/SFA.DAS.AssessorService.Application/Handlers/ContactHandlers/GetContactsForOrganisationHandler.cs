﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.AssessorService.Api.Types.Models;
using SFA.DAS.AssessorService.Application.Interfaces;
using SFA.DAS.AssessorService.Domain.Consts;
using AutoMapper;


namespace SFA.DAS.AssessorService.Application.Handlers.ContactHandlers
{
    public class GetContactsForOrganisationHandler : IRequestHandler<GetContactsForOrganisationRequest, List<ContactResponse>>
    {
        private readonly IContactQueryRepository _contactQueryRepository;
        public GetContactsForOrganisationHandler(IContactQueryRepository contactQueryRepository)
        {
            _contactQueryRepository = contactQueryRepository;
        }

        public async Task<List<ContactResponse>> Handle(GetContactsForOrganisationRequest request, CancellationToken cancellationToken)
        {
            var response = new List<ContactResponse>();
            var contacts = await _contactQueryRepository.GetContactsForOrganisation(request.OrganisationId);
            if (contacts == null)
                return response;
            return Mapper.Map<List<ContactResponse>>(
                contacts.Where(x => x.Status == ContactStatus.Live));
        }
    }
}
