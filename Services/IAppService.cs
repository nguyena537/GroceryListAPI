using GroceryListAPI.Models;
using GroceryListAPI.Models.DTOs;

namespace GroceryListAPI.Services
{
    public interface IAppService
    {
        Task<ServiceResponse<List<ListItemDto>>> AddItemToList(string itemName, int listId, bool isCustom);
        Task<ServiceResponse<List<ItemDto>>> ViewItemsContainingPhrase(string phrase);
        Task<ServiceResponse<List<ListItemDto>>> ViewItemsInList(int listId);
        Task<ServiceResponse<ItemDto>> EditItemInList(int listId, char elementToEdit, string newInfo);
        Task<ServiceResponse<ListItemDto>> CrossOffItem(int listId, int itemId);
        Task<ServiceResponse<ListItemDto>> RemoveItemFromList(int listId, int itemId);

        Task<ServiceResponse<AppUserSettingDto>> ToggleShowCustom(int userId);
        Task<ServiceResponse<AppUserSettingDto>> ToggleDarkMode(int userId);

        Task<ServiceResponse<List<CategoryDto>>> AddCustomCategory(string categoryName, string photoUrl, int userId);
        Task<ServiceResponse<List<CategoryDto>>> ViewAllCategories(int userId);
        Task<ServiceResponse<List<CategoryDto>>> RemoveCategory(int categoryId);
    }
}
