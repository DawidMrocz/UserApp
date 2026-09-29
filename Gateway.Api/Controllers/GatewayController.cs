using Common.Filters;
using Microsoft.AspNetCore.Mvc;

namespace Gateway.Api.Controllers
{
    [CsrfJwtProtection]
    //[ServerExceptionTypeFilter]
    [ModelStateCheck]
    public abstract class GatewayController : ControllerBase { }
}
