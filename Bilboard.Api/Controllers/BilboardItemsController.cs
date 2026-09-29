using Bilboard.Api.DTO.BilboardItem;
using Bilboard.Api.Services.Bilboards.BilboardItem;
using Microsoft.AspNetCore.Mvc;
using Models.Clients.Tasks;

namespace Bilboard.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BilboardItemsController : AuthroizedController
    {
        private readonly IBilboardItemService _bilboardItemService;

        public BilboardItemsController(IBilboardItemService bilboardItemService)
        {
            _bilboardItemService = bilboardItemService;
        }

        /// <summary>
        /// Akcja usuwajaca zadanie z tablicy użytkownika
        /// </summary>
        /// <param name="bilboardItemId"></param>
        /// <returns>Akcja usuwajaca zadanie z tablicy użytkownika</returns>
        [HttpDelete("{bilboardItemId}")]
        public async Task<IActionResult> Delete([FromRoute] int bilboardItemId)
        {
            await _bilboardItemService.Delete(bilboardItemId, AuthorizedUserId!.Value);
            return Ok("Pomyślnie usunięto zadanie z tablicy.");
        }

        /// <summary>
        /// Akcja zmieniajaca status zadania na tablicy użytkownika
        /// </summary>
        /// <param name="bilboardItemId"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPut("{bilboardItemId}/change-status")]
        public async Task<IActionResult> ChangeStatus([FromRoute] int bilboardItemId, [FromBody] TaskChangeStatusRequest request)
        {
            await _bilboardItemService.ChangeStatus(bilboardItemId, request, AuthorizedUserId!.Value);
            return Ok("Pomyślnie zmieniono status zadania.");
        }
    }
}
