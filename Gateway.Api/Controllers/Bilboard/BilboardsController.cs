using Common.Extensions;
using Common.Models.Token;
using Gateway.Api.Service;
using Microsoft.AspNetCore.Mvc;
using Models.Clients.Bilboards;
using Models.Clients.Tasks;
using Newtonsoft.Json;

namespace Gateway.Api.Controllers.Bilboard
{
    [ApiController]
    [Route("[controller]")]
    public class BilboardsController : GatewayController
    {
        private readonly ITokenService _tokenService;
        private readonly HttpClient _bilboardClient;

        public BilboardsController(IHttpClientFactory httpClientFactory, ITokenService tokenService)
        {
            _bilboardClient = httpClientFactory.CreateClient("BilboardApi");
            _tokenService = tokenService;
        }

        /// <summary>
        /// Widok tablicy
        /// </summary>
        /// <returns>Widok tablicy</returns>
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var result = await _bilboardClient.Get<BilboardGetResponse>($"/bilboards", _tokenService.GetToken());
            return Ok(result);
        }

        /// <summary>
        /// Usuniecia pozycji z tablicy
        /// </summary>
        /// <param name="bilboardItemId"></param>
        /// <returns>Usuniecia pozycji z tablicy</returns>
        [HttpDelete("{bilboardItemId}")]
        public async Task<IActionResult> Delete([FromRoute] int bilboardItemId)
        {
            await _bilboardClient.Delete($"/BilboardItems/{bilboardItemId}", _tokenService.GetToken());

            return Ok("Pomyœlnie usuniêto zadanie z tablicy.");
        }

        /// <summary>
        /// Zmiana statusu pozycji w tablicy
        /// </summary>
        /// <param name="bilboardItemId"></param>
        /// <param name="request"></param>
        /// <returns>Zmiana statusu pozycji w tablicy</returns>
        [HttpPut("{bilboardItemId}/change-status")]
        public async Task<IActionResult> ChangeStatus([FromRoute] int bilboardItemId, [FromBody] TaskChangeStatusRequest request)
        {
            await _bilboardClient.Update($"/BilboardItems/{bilboardItemId}/change-status", request, _tokenService.GetToken());

            return Ok("Pomyœlnie zmieniono status zadania.");
        }
    }
}
