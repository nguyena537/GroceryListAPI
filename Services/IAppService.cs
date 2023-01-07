using GroceryListAPI.Models;
using GroceryListAPI.Models.DTOs;

namespace GroceryListAPI.Services
{
    public interface IAppService
    {
        Task<ServiceResponse<List<ListItemDto>>> AddExistingItemToList(int listId, int itemId); //
        Task<ServiceResponse<List<ListItemDto>>> AddCustomItemToList(int listId, string itemName); //
        Task<ServiceResponse<List<GroceryListDto>>> CreateList(int userId, string listName); //
        Task<ServiceResponse<List<ItemDto>>> ViewItemsContainingPhrase(string phrase, int listId); //
        Task<ServiceResponse<List<ListItemDto>>> ViewItemsInList(int listId, int userId); //
        Task<ServiceResponse<ListItemDto>> ViewItemInfoInList(int listId, int itemId); //
        Task<ServiceResponse<List<GroceryListDto>>> ViewAllLists(int userId); //
        Task<ServiceResponse<GroceryListDto>> ViewListInfo(int listId);

        Task<ServiceResponse<ListItemDto>> EditItemInList(int listId, int itemId, char elementToEdit, string newInfo); //
        Task<ServiceResponse<List<GroceryListDto>>> EditListName(int listId, string listName);
        Task<ServiceResponse<List<ListItemDto>>> ToggleCrossedOff(int listId, int itemId); //
        Task<ServiceResponse<List<ListItemDto>>> RemoveItemFromList(int listId, int itemId); //
        Task<ServiceResponse<List<GroceryListDto>>> RemoveList(int userId, int listId);

        Task<ServiceResponse<AppUserSettingDto>> ToggleShowCustom(int userId); //
        Task<ServiceResponse<AppUserSettingDto>> ToggleDarkMode(int userId); //
        Task<ServiceResponse<GroceryListDto>> ToggleShowCrossedOff(int listId); //
        Task<ServiceResponse<AppUserSettingDto>> ViewAppUserSetting(int userId); //

        Task<ServiceResponse<List<CategoryDto>>> AddCustomCategory(string categoryName, int userId); //
        Task<ServiceResponse<List<CategoryDto>>> EditCategory(int categoryId, int userId, char elementToEdit, string newInfo); //
        Task<ServiceResponse<List<CategoryDto>>> ViewAllCategories(int userId); //
        Task<ServiceResponse<List<CategoryDto>>> RemoveCategory(int categoryId, int userId);  //
    }
}
