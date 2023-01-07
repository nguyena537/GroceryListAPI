using GroceryListAPI.Models;
using GroceryListAPI.Models.DTOs;
using GroceryListAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;


namespace GroceryListAPI.Controllers
{
    [EnableCors("corsapp")]
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]

    public class AppController : ControllerBase
    {
        private readonly IAppService _appService;

        public AppController(IAppService appService)
        {
            _appService = appService;
        }

        [HttpGet("ViewItemsContainingPhrase/{phrase}/{listId}")]
        public async Task<ActionResult<ServiceResponse<List<ItemDto>>>> ViewItemsContainingPhrase(string phrase, int listId)
        {
            return Ok(await _appService.ViewItemsContainingPhrase(phrase, listId));
        }

        [HttpGet("ViewItemsInList/{listId}/{userId}")]
        public async Task<ActionResult<ServiceResponse<List<ListItemDto>>>> ViewItemsInList(int listId, int userId)
        {
            return Ok(await _appService.ViewItemsInList(listId, userId));
        }

        [HttpGet("ViewItemInfoInList/{listId}/{itemId}")]
        public async Task<ActionResult<ServiceResponse<ListItemDto>>> ViewItemInfoInList(int listId, int itemId)
        {
            return Ok(await _appService.ViewItemInfoInList(listId, itemId));
        }

        [HttpGet("ViewAllCategories/{userId}")]
        public async Task<ActionResult<ServiceResponse<List<ListItemDto>>>> ViewAllCategories(int userId)
        {
            return Ok(await _appService.ViewAllCategories(userId));
        }

        [HttpGet("ViewAllLists/{userId}")]
        public async Task<ActionResult<ServiceResponse<List<GroceryListDto>>>> ViewAllLists(int userId)
        {
            return Ok(await _appService.ViewAllLists(userId));
        }

        [HttpGet("ViewListInfo/{listId}")]
        public async Task<ActionResult<ServiceResponse<GroceryListDto>>> ViewListInfo(int listId)
        {
            return Ok(await _appService.ViewListInfo(listId));
        }

        [HttpPut("CreateList/{userId}/{listName}")]
        public async Task<ActionResult<ServiceResponse<List<GroceryListDto>>>> CreateList(int userId, string listName)
        {
            return Ok(await _appService.CreateList(userId, listName));
        }

        [HttpPut("AddCustomCategory/{categoryName}-{userId}")]
        public async Task<ActionResult<ServiceResponse<List<ListItemDto>>>> AddCustomCategory(string categoryName, int userId)
        {
            return Ok(await _appService.AddCustomCategory(categoryName, userId));
        }

        [HttpPost("EditCategory/{categoryId}-{userId}-{elementToEdit}-{newInfo}")]
        public async Task<ActionResult<ServiceResponse<List<ListItemDto>>>> EditCategory(int categoryId, int userId, char elementToEdit, string newInfo)
        {
            return Ok(await _appService.EditCategory(categoryId, userId, elementToEdit, newInfo));
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

        [HttpPost("EditItemInList/{listId}/{itemId}/{elementToEdit}/{newInfo}")]
        public async Task<ActionResult<ServiceResponse<ListItemDto>>> EditItemInList(int listId, int itemId, char elementToEdit, string newInfo)
        {
            return Ok(await _appService.EditItemInList(listId, itemId, elementToEdit, newInfo));
        }

        [HttpPost("EditListName/{listId}/{listName}")]
        public async Task<ActionResult<ServiceResponse<List<GroceryListDto>>>> EditListName(int listId, string listName)
        {
            return Ok(await _appService.EditListName(listId, listName));
        }

        [HttpPost("RemoveItemFromList/{listId}/{itemId}")]
        public async Task<ActionResult<ServiceResponse<List<ListItemDto>>>> RemoveItemFromList(int listId, int itemId)
        {
            return Ok(await _appService.RemoveItemFromList(listId, itemId));
        }

        
        [HttpPost("ToggleCrossedOff/{listId}/{itemId}")]
        public async Task<ActionResult<ServiceResponse<List<ListItemDto>>>> ToggleCrossedOff(int listId, int itemId)
        {
            return Ok(await _appService.ToggleCrossedOff(listId, itemId));
        }

        [HttpGet("ViewAppUserSetting/{userId}")]
        public async Task<ActionResult<ServiceResponse<AppUserSettingDto>>> ViewAppUserSetting(int userId)
        {
            return Ok(await _appService.ViewAppUserSetting(userId));
        }

        [HttpPost("ToggleShowCustom/{userId}")]
        public async Task<ActionResult<ServiceResponse<AppUserSettingDto>>> ToggleShowCustom(int userId)
        {
            return Ok(await _appService.ToggleShowCustom(userId));
        }

        [HttpPost("ToggleShowCrossedOff/{listId}")]
        public async Task<ActionResult<ServiceResponse<GroceryListDto>>> ToggleShowCrossedOff(int listId)
        {
            return Ok(await _appService.ToggleShowCrossedOff(listId));
        }

        [HttpPost("ToggleDarkMode/{userId}")]
        public async Task<ActionResult<ServiceResponse<AppUserSettingDto>>> ToggleDarkMode(int userId)
        {
            return Ok(await _appService.ToggleDarkMode(userId));
        }

        [HttpDelete("RemoveList/{userId}/{listId}")]
        public async Task<ActionResult<ServiceResponse<List<GroceryListDto>>>> RemoveList(int userId, int listId)
        {
            return Ok(await _appService.RemoveList(userId, listId));
        }
        [HttpDelete("RemoveCategory/{categoryId}-{userId}")]
        public async Task<ActionResult<ServiceResponse<List<CategoryDto>>>> RemoveCategory(int categoryId, int userId)
        {
            return Ok(await _appService.RemoveCategory(categoryId, userId));
        }

    }
}
