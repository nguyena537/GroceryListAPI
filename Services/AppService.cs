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

        public async Task<ServiceResponse<List<ListItemDto>>> AddCustomItemToList(int listId, string itemName)
        {
            throw new NotImplementedException();
        }

        public async Task<ServiceResponse<List<ListItemDto>>> ToggleCrossedOff(int listId, int itemId)
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

                    if (itemList != null)
                    {
                        for (int i = 0; i < itemList.Count; i++)
                        {
                            if (itemList[i].ItemId == itemId)
                            {
                                itemList[i].CrossedOff = !itemList[i].CrossedOff;
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

        public async Task<ServiceResponse<List<ListItemDto>>> EditItemInList(int listId, int itemId, char elementToEdit, string newInfo)
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

                    if (itemList != null)
                    {
                        for (int i = 0; i < itemList.Count; i++)
                        {
                            if (itemList[i].ItemId == itemId)
                            {
                                itemFound = true;
                                if (elementToEdit == 'c')
                                {
                                    itemList[i].Category = newInfo;
                                }
                                else if (elementToEdit == 'q')
                                {
                                    itemList[i].Quantity = Convert.ToInt32(newInfo);
                                }
                                else if (elementToEdit == 'n')
                                {
                                    itemList[i].Note = newInfo;
                                }
                                else
                                {
                                    response.Success = false;
                                    response.Message = "Invalid elementToEdit. Must be c, q, or n.";
                                    break;
                                }
                                list.ItemsJson = JsonConvert.SerializeObject(itemList);
                                
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


        public async Task<ServiceResponse<AppUserSettingDto>> ToggleDarkMode(int userId)
        {
            var response = new ServiceResponse<AppUserSettingDto>();

            try
            {
                var appUserSetting = await _context.AppUserSettings
                    .FirstOrDefaultAsync(u => u.AppUserId == userId);

                if (appUserSetting != null)
                {
                    appUserSetting.DarkMode = !appUserSetting.DarkMode;
                    var newUserSetting = new AppUserSettingDto();
                    newUserSetting.AppUserId = userId;
                    newUserSetting.DarkMode = appUserSetting.DarkMode;
                    newUserSetting.ShowCustom = appUserSetting.ShowCustom;
                    response.Data = newUserSetting;

                    await _context.SaveChangesAsync();
                }
                else
                {
                    response.Success = false;
                    response.Message = "User id does not exist in the database.";
                }

                
                
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }

        public async Task<ServiceResponse<AppUserSettingDto>> ToggleShowCustom(int userId)
        {
            var response = new ServiceResponse<AppUserSettingDto>();

            try
            {
                var appUserSetting = await _context.AppUserSettings
                    .FirstOrDefaultAsync(u => u.AppUserId == userId);

                if (appUserSetting != null)
                {
                    appUserSetting.ShowCustom = !appUserSetting.ShowCustom;
                    var newUserSetting = new AppUserSettingDto();
                    newUserSetting.AppUserId = userId;
                    newUserSetting.DarkMode = appUserSetting.DarkMode;
                    newUserSetting.ShowCustom = appUserSetting.ShowCustom;
                    response.Data = newUserSetting;

                    await _context.SaveChangesAsync();
                }
                else
                {
                    response.Success = false;
                    response.Message = "User id does not exist in the database.";
                }



            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
            }


            return response;
        }

        public async Task<ServiceResponse<List<CategoryDto>>> ViewAllCategories(int userId)
        {
            var response = new ServiceResponse<List<CategoryDto>>();

            try
            {
                var categories = _context.Categories
                    .ToList();
                var currUser = await _context.AppUserSettings
                    .FirstOrDefaultAsync(u => u.AppUserId == userId);
                var showCustom = false;

                if (currUser != null)
                {
                    showCustom = currUser.ShowCustom;
                }
                else
                { 
                    throw new Exception("User does not exist");
                }

                var categoryDtos = new List<CategoryDto>();

                foreach (var category in categories)
                {
                    if (category.AppUserId == null || category.AppUserId == userId || showCustom)
                    {
                        var newCategoryDto = new CategoryDto();
                        newCategoryDto.Id = category.Id;
                        newCategoryDto.Name = category.Name;
                        newCategoryDto.PhotoUrl = category.PhotoUrl;
                        newCategoryDto.IsCustom = category.IsCustom;

                        categoryDtos.Add(newCategoryDto);
                    }
                    
                }

                response.Data = categoryDtos;
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
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
