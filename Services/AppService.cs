using GroceryListAPI.Models;
using GroceryListAPI.Models.DTOs;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace GroceryListAPI.Services
{
    public class AppService : IAppService
    {
        private readonly GroceryListsDbContext _context;

        public AppService(GroceryListsDbContext context)
        {
            _context = context;
        }

        public Task<ServiceResponse<List<CategoryDto>>> AddCustomCategory(string categoryName, string photoUrl, int userId)
        {
            throw new NotImplementedException();
        }

        public async Task<ServiceResponse<List<ListItemDto>>> AddExistingItemToList(int listId, int itemId)
        {
            var response = new ServiceResponse<List<ListItemDto>>();

            try
            {
                var itemFound = false;

                var itemInDatabase = await _context.Items
                    .FirstOrDefaultAsync(i => i.Id == itemId);
                if (itemInDatabase != null)
                {
                    var newItem = new ListItemDto(itemId);
                    var list = await _context.GroceryLists
                        .FirstOrDefaultAsync(l => l.Id == listId);

                    if (list != null)
                    {
                        var itemList = JsonConvert.DeserializeObject<List<ListItemDto>>(list.ItemsJson);

                        if (itemList == null)
                        {
                            itemList = new List<ListItemDto>();
                        }

                        for (int i = 0; i < itemList.Count; i++)
                        {
                            if (itemList[i].ItemId == itemId)
                            {
                                response.Success = false;
                                response.Message = "Item is already in the list.";
                                itemFound = true;
                            }
                        }

                        if (!itemFound)
                        {
                            itemList.Add(newItem);
                        }
                        
                        list.ItemsJson = JsonConvert.SerializeObject(itemList);

                        response.Data = itemList;
                    }
                    else
                    {
                        response.Success = false;
                        response.Message = "List does not exist.";
                    }
                }
                else
                {
                    response.Success = false;
                    response.Message = "Item does not exist.";
                }
                

            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
            }

            await _context.SaveChangesAsync();
            
            return response;
        }

        public Task<ServiceResponse<List<ListItemDto>>> AddCustomItemToList(int listId, string itemName)
        { 
            throw new NotImplementedException();
        }

        public Task<ServiceResponse<ListItemDto>> CrossOffItem(int listId, int itemId)
        {
            throw new NotImplementedException();
        }

        public Task<ServiceResponse<ItemDto>> EditItemInList(int listId, int itemId, char elementToEdit, string newInfo)
        {
            throw new NotImplementedException();
        }

        public Task<ServiceResponse<List<CategoryDto>>> RemoveCategory(int categoryId)
        {
            throw new NotImplementedException();
        }

        public async Task<ServiceResponse<List<ListItemDto>>> RemoveItemFromList(int listId, int itemId)
        {
            var response = new ServiceResponse<List<ListItemDto>>();

            try
            {
                var itemFound = false;

                var list = await _context.GroceryLists
                        .FirstOrDefaultAsync(l => l.Id == listId);

                if (list != null)
                {
                    var itemList = JsonConvert.DeserializeObject<List<ListItemDto>>(list.ItemsJson);

                    if (itemList != null || itemList.Count == 0)
                    {
                        for (int i = 0; i < itemList.Count; i++)
                        {
                            if (itemList[i].ItemId == itemId)
                            {
                                itemList.RemoveAt(i);
                                list.ItemsJson = JsonConvert.SerializeObject(itemList);
                                itemFound = true;
                                break;
                            }
                        }

                        if (!itemFound)
                        {
                            response.Success = false;
                            response.Message = "Item is not in the list";
                        }
                    }
                    else
                    {
                        response.Success = false;
                        response.Message = "List is empty";
                    }
                    response.Data = itemList;
                }
                else
                {
                    response.Success = false;
                    response.Message = "List does not exist";
                }
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
            }

            await _context.SaveChangesAsync();
            
            return response;
        }


        public Task<ServiceResponse<AppUserSettingDto>> ToggleDarkMode(int userId)
        {
            throw new NotImplementedException();
        }

        public Task<ServiceResponse<AppUserSettingDto>> ToggleShowCustom(int userId)
        {
            throw new NotImplementedException();
        }

        public Task<ServiceResponse<List<CategoryDto>>> ViewAllCategories(int userId)
        {
            throw new NotImplementedException();
        }

        public Task<ServiceResponse<List<ItemDto>>> ViewItemsContainingPhrase(string phrase)
        {
            throw new NotImplementedException();
        }

        public async Task<ServiceResponse<List<ListItemDto>>> ViewItemsInList(int listId)
        {
            var response = new ServiceResponse<List<ListItemDto>>();

            try
            {
                var list = await _context.GroceryLists
                    .FirstOrDefaultAsync(l => l.Id == listId);

                if (list != null)
                {
                    var itemList = JsonConvert.DeserializeObject<List<ListItemDto>>(list.ItemsJson);
                    response.Data = itemList;
                }
                else
                {
                    response.Success = false;
                    response.Message = "List does not exist";
                }
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }

        public async Task<ServiceResponse<List<ListItemDto>>> CreateList(int userId, string listName)
        {
            var response = new ServiceResponse<List<ListItemDto>>();

            var newList = new GroceryList();
            var newItems = new List<ListItemDto>();
            var newItemsJson = JsonConvert.SerializeObject(newItems);

            newList.Name = listName;
            newList.ItemsJson = newItemsJson;
            newList.ShowCrossedOff = true;
            newList.AppUserId = userId;

            _context.GroceryLists.Add(newList);
            await _context.SaveChangesAsync();

            response.Data = newItems;

            return response;
        }
    }
}
