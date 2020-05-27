This entire folder defines types which belong to the RoatpService (das-roatp-service).

These should be in a seperate library within this repo and should be ideally imported 
via a Nuget package from das-roatp-service.

Some of these types are re-used via the Assessor Service Client Nuget package in 
the Admin Service (das-admin-service); this is wrong as the Admin Service should
also define these types ideally via a Nuget package from das-roatp-service.

When creating Assessor Service Client Nuget the decision was taken to continue
using the types which had previously been duplicated into a copy of the Assessor.Application.Api.Types
library in the Admin Service as there is no current Nuget package.

However the re-use of these types is undesirable in that it would be strange
to have to alter the Assessor repo to use updated Roatp types in the Admin Service, however since
there should only be one copy of the types anyway (in a Nuget package) it would
also be strange not to update the Assessor repo.

The solution is to introduce a Roatp Service Client Nuget package as soon as possible.
