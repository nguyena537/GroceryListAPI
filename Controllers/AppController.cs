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

        [HttpGet("ViewItemsContainingPhrase/{phrase}")]
        public async Task<ActionResult<ServiceResponse<List<ItemDto>>>> ViewItemsContainingPhrase(string phrase)
        {
            return Ok(await _appService.ViewItemsContainingPhrase(phrase));
        }

        [HttpGet("ViewItemsInList/{listId}")]
        public async Task<ActionResult<ServiceResponse<List<ListItemDto>>>> ViewItemsInList(int listId)
        {
            return Ok(await _appService.ViewItemsInList(listId));
        }

        [HttpGet("ViewAllCategories/{userId}")]
        public async Task<ActionResult<ServiceResponse<List<ListItemDto>>>> ViewAllCategories(int userId)
        {
            return Ok(await _appService.ViewAllCategories(userId));
        }

        [HttpPut("CreateList/{listName}-{userId}")]
        public async Task<ActionResult<ServiceResponse<List<ListItemDto>>>> CreateList(int userId, string listName)
        {
            return Ok(await _appService.CreateList(userId, listName));
        }



        [HttpPost("AddExistingItemToList/{listId}-{itemId}")]
        public async Task<ActionResult<ServiceResponse<List<ListItemDto>>>> AddExistingItemToList(int listId, int itemId)
        {
            return Ok(await _appService.AddExistingItemToList(listId, itemId));
        }

        [HttpPost("AddCustomItemToList/{listId}-{itemId}")]
        public async Task<ActionResult<ServiceResponse<List<ListItemDto>>>> AddCustomItemToList(int listId, string itemName)
        {
            return Ok(await _appService.AddCustomItemToList(listId, itemName));
        }

        [HttpPost("EditItemInList/{listId}-{itemId}-{elementToEdit}-{newInfo}")]
        public async Task<ActionResult<ServiceResponse<List<ListItemDto>>>> EditItemInList(int listId, int itemId, char elementToEdit, string newInfo)
        {
            return Ok(await _appService.EditItemInList(listId, itemId, elementToEdit, newInfo));
        }

        [HttpPost("RemoveItemFromList/{listId}-{itemId}")]
        public async Task<ActionResult<ServiceResponse<List<ListItemDto>>>> RemoveItemFromList(int listId, int itemId)
        {
            return Ok(await _appService.RemoveItemFromList(listId, itemId));
        }

        [HttpPost("ToggleCrossedOff/{listId}-{itemId}")]
        public async Task<ActionResult<ServiceResponse<List<ListItemDto>>>> ToggleCrossedOff(int listId, int itemId)
        {
            return Ok(await _appService.ToggleCrossedOff(listId, itemId));
        }

        [HttpPost("ToggleShowCustom/{userId}")]
        public async Task<ActionResult<ServiceResponse<AppUserSettingDto>>> ToggleShowCustom(int userId)
        {
            return Ok(await _appService.ToggleShowCustom(userId));
        }

        [HttpPost("ToggleDarkMode/{userId}")]
        public async Task<ActionResult<ServiceResponse<AppUserSettingDto>>> ToggleDarkMode(int userId)
        {
            return Ok(await _appService.ToggleDarkMode(userId));
        }

        [HttpDelete("RemoveCategory/{categoryId}")]
        public async Task<ActionResult<ServiceResponse<List<CategoryDto>>>> RemoveCategory(int categoryId)
        {
            return Ok(await _appService.RemoveCategory(categoryId));
        }

    }
}
