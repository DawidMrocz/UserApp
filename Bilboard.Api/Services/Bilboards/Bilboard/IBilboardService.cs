using Microsoft.AspNetCore.Authorization.Infrastructure;
using Models.Clients.Bilboards;
using System.Security.Claims;

namespace Bilboard.Api.Services.Bilboards.Bilboard
{
    public interface IBilboardService
    {
        Task<BilboardGetResponse> Get(ClaimsPrincipal user);
    }
}
