using Bilboard.Api.Services.Bilboards.Bilboard;
using Microsoft.AspNetCore.Mvc;

namespace Bilboard.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BilboardsController : AuthroizedController
    {
        private readonly IBilboardService _bilboardService;

        public BilboardsController(IBilboardService bilboardService)
        {
            _bilboardService = bilboardService;
        }

        /// <summary>
        /// Pobranie tablicy u¿ytkownika
        /// </summary>
        /// <returns>Pobranie tablicy u¿ytkownika</returns>
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok(await _bilboardService.Get(AuthorizedUser!));
        }
    }
}
