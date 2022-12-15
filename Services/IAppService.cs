using GroceryListAPI.Models;
using GroceryListAPI.Models.DTOs;

namespace GroceryListAPI.Services
{
    public interface IAppService
    {
        Task<ServiceResponse<List<ListItemDto>>> AddExistingItemToList(int listId, int itemId); //
        Task<ServiceResponse<List<ListItemDto>>> AddCustomItemToList(int listId, string itemName); //
        Task<ServiceResponse<List<ListItemDto>>> CreateList(int userId, string listName); //
        Task<ServiceResponse<List<ItemDto>>> ViewItemsContainingPhrase(string phrase); //
        Task<ServiceResponse<List<ListItemDto>>> ViewItemsInList(int listId); //
        Task<ServiceResponse<List<ListItemDto>>> EditItemInList(int listId, int itemId, char elementToEdit, string newInfo); //
        Task<ServiceResponse<List<ListItemDto>>> ToggleCrossedOff(int listId, int itemId); //
        Task<ServiceResponse<List<ListItemDto>>> RemoveItemFromList(int listId, int itemId); //

        Task<ServiceResponse<AppUserSettingDto>> ToggleShowCustom(int userId); //
        Task<ServiceResponse<AppUserSettingDto>> ToggleDarkMode(int userId); //

        Task<ServiceResponse<List<CategoryDto>>> AddCustomCategory(string categoryName, string photoUrl, int userId);
        Task<ServiceResponse<List<CategoryDto>>> ViewAllCategories(int userId); //
        Task<ServiceResponse<List<CategoryDto>>> RemoveCategory(int categoryId); //
    }
}
