using GroceryListAPI.Models;
using GroceryListAPI.Models.DTOs;
using GroceryListAPI.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GroceryListAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    
    public class AppController : ControllerBase
    {
        private readonly IAppService _appService;

        public AppController(IAppService appService)
        {
            _appService = appService;
        }

        [HttpGet("ViewItemsInList/{listId}")]
        public async Task<ActionResult<ServiceResponse<List<ListItemDto>>>> ViewItemsInList(int listId)
        {
            return Ok(await _appService.ViewItemsInList(listId));
        }

        [HttpPut("CreateList/{listName}-{userId}")]
        public async Task<ActionResult<ServiceResponse<List<ListItemDto>>>> CreateList(int userId, string listName)
        {
            return Ok(await _appService.CreateList(userId, listName));
        }

        [HttpPost("AddExistingItemToList/{itemId}-{listId}")]
        public async Task<ActionResult<ServiceResponse<List<ListItemDto>>>> AddExistingItemToList(int listId, int itemId)
        {
            return Ok(await _appService.AddExistingItemToList(listId, itemId));
        }

        [HttpPost("RemoveItemFromList/{itemId}-{listId}")]
        public async Task<ActionResult<ServiceResponse<List<ListItemDto>>>> RemoveItemFromList(int listId, int itemId)
        {
            return Ok(await _appService.RemoveItemFromList(listId, itemId));
        }
    }
}
