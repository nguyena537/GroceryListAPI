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

        public List<CategoryDto> returnCategoryDtos(int userId)
        {
            var categories = _context.Categories
                .ToList();
            var currUser = _context.AppUserSettings
                .FirstOrDefault(u => u.AppUserId == userId);
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
                    newCategoryDto.AppUserId = category.AppUserId;

                    categoryDtos.Add(newCategoryDto);
                }

            }

            return categoryDtos;

        }

        public List<GroceryListDto> CreateListDtos(int userId)
        { 
            var lists = _context.GroceryLists
                    .Where(l => l.AppUserId == userId)
                    .ToList();
            var listDtos = new List<GroceryListDto>();

            foreach (var list in lists)
            {
                var listDto = new GroceryListDto();
                listDto.Name = list.Name;
                listDto.AppUserId = userId;
                listDto.Id = list.Id;
                listDto.ShowCrossedOff = list.ShowCrossedOff;
                var itemList = JsonConvert.DeserializeObject<List<ListItemDto>>(list.ItemsJson);
                if (itemList == null)
                {
                    listDto.NumItems = 0;
                }
                else
                {
                    listDto.NumItems = itemList.Count;
                }
                listDtos.Add(listDto);
            }

            return listDtos;
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
                            newItem.Quantity = 1;
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

        public async Task<ServiceResponse<ListItemDto>> EditItemInList(int listId, int itemId, char elementToEdit, string newInfo)
        {
            var response = new ServiceResponse<ListItemDto>();

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
                                    itemList[i].Category = newInfo.Replace('_', ' ');
                                }
                                else if (elementToEdit == 'q')
                                {
                                    itemList[i].Quantity = Convert.ToInt32(newInfo);
                                }
                                else if (elementToEdit == 'n')
                                {
                                    itemList[i].Note = newInfo.Replace('_', ' ');
                                }
                                else
                                {
                                    response.Success = false;
                                    response.Message = "Invalid elementToEdit. Must be c, q, or n.";
                                    break;
                                }
                                list.ItemsJson = JsonConvert.SerializeObject(itemList);

                                var item = itemList[i];
                                var currItem = await _context.Items
                                    .FirstOrDefaultAsync(i => i.Id == itemId);

                                if (currItem == null)
                                {
                                    throw new Exception("An item in the list does not exist in the database");
                                }

                                var currItemCategory = await _context.Categories
                                    .FirstOrDefaultAsync(c => c.Id == currItem.CategoryId);

                                if (currItemCategory == null && item.Category == null)
                                {
                                    item.Category = null;
                                }
                                else
                                {
                                    item.Category = item.Category == null ? currItemCategory.Name : item.Category;
                                }

                                item.Name = currItem.Name;
                                item.Unit = currItem.Unit;
                                item.PhotoUrl = currItem.PhotoUrl;
                                response.Data = item;
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

        public async Task<ServiceResponse<GroceryListDto>> ToggleShowCrossedOff(int listId)
        {
            var response = new ServiceResponse<GroceryListDto>();

            try
            {
                var list = await _context.GroceryLists
                    .FirstOrDefaultAsync(l => l.Id == listId);

                if (list != null)
                {
                    list.ShowCrossedOff = !list.ShowCrossedOff;
                    var newList = new GroceryListDto();
                    newList.Id = listId;
                    newList.Name = list.Name;
                    newList.AppUserId = list.AppUserId;
                    newList.ShowCrossedOff = list.ShowCrossedOff;
                    response.Data = newList;

                    await _context.SaveChangesAsync();
                }
                else
                {
                    response.Success = false;
                    response.Message = "List id does not exist in the database.";
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
                response.Data = returnCategoryDtos(userId);
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }

        public async Task<ServiceResponse<List<ItemDto>>> ViewItemsContainingPhrase(string phrase, int listId)
        {
            var response = new ServiceResponse<List<ItemDto>>();

            try
            {
                var itemsContainingPhrase = _context.Items
                    .Where(i => i.Name.Contains(phrase))
                    .ToList();

                var itemDtos = new List<ItemDto>();

                var list = await _context.GroceryLists
                    .FirstOrDefaultAsync(l => l.Id == listId);

                List<ListItemDto> itemList;

                if (list != null)
                {
                    itemList = JsonConvert.DeserializeObject<List<ListItemDto>>(list.ItemsJson);
                } 
                else
                {
                    throw new Exception("List is null");
                }
                foreach (var item in itemsContainingPhrase)
                {
                    var itemFound = itemList
                        .FirstOrDefault(i => i.ItemId == item.Id);
                    if (itemFound == null)
                    {
                        var itemCategory = await _context.Categories
                            .FirstOrDefaultAsync(c => c.Id == item.CategoryId);
                        var currItemDto = new ItemDto();
                        currItemDto.Id = item.Id;
                        currItemDto.Name = item.Name;
                        currItemDto.Unit = item.Unit;
                        currItemDto.CategoryName = itemCategory?.Name;
                        currItemDto.IsCustom = item.IsCustom;

                        itemDtos.Add(currItemDto);
                    }
                    
                }

                response.Data = itemDtos;
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }

        public async Task<ServiceResponse<List<ListItemDto>>> ViewItemsInList(int listId, int userId)
        {
            var response = new ServiceResponse<List<ListItemDto>>();

            try
            {
                var list = await _context.GroceryLists
                    .FirstOrDefaultAsync(l => l.Id == listId);

                if (list != null)
                {
                    if (list.AppUserId != userId)
                    {
                        throw new Exception("This list does not belong to the user.");
                    }

                    var itemList = JsonConvert.DeserializeObject<List<ListItemDto>>(list.ItemsJson);
                    if (itemList != null)
                    {
                        foreach (var item in itemList)
                        {
                            var currItem = await _context.Items
                                .FirstOrDefaultAsync(i => i.Id == item.ItemId);

                            if (currItem == null)
                            {
                                throw new Exception("An item in the list does not exist in the database");
                            }

                            var currItemCategory = await _context.Categories
                                .FirstOrDefaultAsync(c => c.Id == currItem.CategoryId);

                            if (currItemCategory == null && item.Category == null)
                            {
                                item.Category = null;
                            }
                            else
                            {
                                item.Category = item.Category == null ? currItemCategory.Name : item.Category;
                            }

                            item.Name = currItem.Name;
                            item.Unit = currItem.Unit;
                            item.PhotoUrl = currItem.PhotoUrl;
                        }
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

            return response;
        }

        public async Task<ServiceResponse<List<GroceryListDto>>> CreateList(int userId, string listName)
        {
            var response = new ServiceResponse<List<GroceryListDto>>();
            
            try
            {
                var newList = new GroceryList();
                var newItems = new List<ListItemDto>();
                var newItemsJson = JsonConvert.SerializeObject(newItems);

                newList.Name = listName.Replace('_', ' ');
                newList.ItemsJson = newItemsJson;
                newList.ShowCrossedOff = true;
                newList.AppUserId = userId;

                _context.GroceryLists.Add(newList);

                await _context.SaveChangesAsync();

                response.Data = CreateListDtos(userId);
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
            }


            return response;
        }

        public async Task<ServiceResponse<List<GroceryListDto>>> RemoveList(int userId, int listId)
        {
            var response = new ServiceResponse<List<GroceryListDto>>();

            try
            {
                var listToRemove = _context.GroceryLists
                    .FirstOrDefault(l => l.Id == listId);

                if (listToRemove != null)
                {
                    _context.GroceryLists.Remove(listToRemove); 
                    await _context.SaveChangesAsync();
                    response.Data = CreateListDtos(userId);
                }
                else
                {
                    throw new Exception("List not found");
                }
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }

        public async Task<ServiceResponse<List<CategoryDto>>> AddCustomCategory(string categoryName, int userId)
        {
            var response = new ServiceResponse<List<CategoryDto>>();

            try
            {
                var categories = _context.Categories
                    .Where(c => !c.IsCustom || c.AppUserId == userId)
                    .ToList();

                foreach (var category in categories)
                {
                    if (category.Name == categoryName)
                    {
                        throw new Exception("A category already exists with that name");
                    }
                }

                var newCategory = new Category();
                newCategory.Name = categoryName;
                newCategory.IsCustom = true;
                newCategory.AppUserId = userId;

                _context.Categories.Add(newCategory);
                await _context.SaveChangesAsync();

                response.Data = returnCategoryDtos(userId);
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }

        public async Task<ServiceResponse<List<CategoryDto>>> EditCategory(int categoryId, int userId, char elementToEdit, string newInfo)
        {
            var response = new ServiceResponse<List<CategoryDto>>();

            try
            {
                var categoryToEdit = await _context.Categories
                    .FirstOrDefaultAsync(c => c.Id == categoryId);

                if (categoryToEdit != null)
                {
                    if (elementToEdit == 'n')
                    {
                        categoryToEdit.Name = newInfo;
                    }
                    else if (elementToEdit == 'p')
                    {
                        categoryToEdit.PhotoUrl = newInfo;
                    }
                    else
                    {
                        throw new Exception("elementToEdit must be n or p");
                    }
                }

                await _context.SaveChangesAsync();

                response.Data = returnCategoryDtos(userId);
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }

        public async Task<ServiceResponse<List<CategoryDto>>> RemoveCategory(int categoryId, int userId)
        {
            var response = new ServiceResponse<List<CategoryDto>>();

            try
            {
                var categoryToRemove = await _context.Categories
                    .FirstOrDefaultAsync(c => c.Id == categoryId);

                if (categoryToRemove == null)
                {
                    throw new Exception("Category id not found in database.");
                }
                else if (!categoryToRemove.IsCustom)
                {
                    throw new Exception("Cannot remove default (non-custom) categories");
                }
                else if (categoryToRemove.AppUserId != userId)
                {
                    throw new Exception("This custom category does not belong to userId");
                }

                _context.Categories.Remove(categoryToRemove);
                await _context.SaveChangesAsync();
                response.Data = returnCategoryDtos(userId);
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;

        }

        public async Task<ServiceResponse<ListItemDto>> ViewItemInfoInList(int listId, int itemId)
        {
            var response = new ServiceResponse<ListItemDto>();

            try
            {
                var list = await _context.GroceryLists
                    .FirstOrDefaultAsync(l => l.Id == listId);

                if (list != null)
                {
                    var itemList = JsonConvert.DeserializeObject<List<ListItemDto>>(list.ItemsJson);
                    if (itemList != null)
                    {
                        foreach (var item in itemList)
                        {
                            if (item.ItemId == itemId)
                            {
                                var currItem = await _context.Items
                                    .FirstOrDefaultAsync(i => i.Id == item.ItemId);

                                if (currItem == null)
                                {
                                    throw new Exception("An item in the list does not exist in the database");
                                }

                                var currItemCategory = await _context.Categories
                                    .FirstOrDefaultAsync(c => c.Id == currItem.CategoryId);

                                if (currItemCategory == null && item.Category == null)
                                {
                                    item.Category = null;
                                }
                                else
                                {
                                    item.Category = item.Category == null ? currItemCategory.Name : item.Category;
                                }

                                item.Name = currItem.Name;
                                item.Unit = currItem.Unit;
                                item.PhotoUrl = currItem.PhotoUrl;
                                
                                response.Data = item;
                                break;
                            }
                            
                        }
                    }

                    
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

        public async Task<ServiceResponse<AppUserSettingDto>> ViewAppUserSetting(int userId)
        {
            var response = new ServiceResponse<AppUserSettingDto>();

            try
            {
                var appUserSetting = await _context.AppUserSettings
                    .FirstOrDefaultAsync(u => u.AppUserId == userId);

                if (appUserSetting != null)
                {
                    var appUserSettingDto = new AppUserSettingDto();
                    appUserSettingDto.AppUserId = userId;
                    appUserSettingDto.ShowCustom = appUserSetting.ShowCustom;
                    appUserSettingDto.DarkMode = appUserSetting.DarkMode;

                    response.Data = appUserSettingDto;
                }
                else
                {
                    throw new Exception("User id not found in database");
                }
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }

        public async Task<ServiceResponse<List<GroceryListDto>>> ViewAllLists(int userId)
        {
            var response = new ServiceResponse<List<GroceryListDto>>();
            
            try
            { 
                response.Data = CreateListDtos(userId);
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }

        public async Task<ServiceResponse<GroceryListDto>> ViewListInfo(int listId)
        {
            var response = new ServiceResponse<GroceryListDto>();
            try
            {
                var list = await _context.GroceryLists
                    .FirstOrDefaultAsync(l => l.Id == listId);

                if (list != null)
                {
                    var itemList = JsonConvert.DeserializeObject<List<ListItemDto>>(list.ItemsJson);

                    var newList = new GroceryListDto();
                    newList.Id = listId;
                    newList.Name = list.Name;
                    newList.AppUserId = list.AppUserId;
                    newList.ShowCrossedOff = list.ShowCrossedOff;
                    if (itemList == null)
                    {
                        newList.NumItems = 0;
                    }
                    else
                    {
                        newList.NumItems = itemList.Count;
                    }
                    
                    response.Data = newList;
                }
                else
                {
                    response.Success = false;
                    response.Message = "List id does not exist in the database.";
                }
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }

        public async Task<ServiceResponse<List<GroceryListDto>>> EditListName(int listId, string listName)
        {
            var response = new ServiceResponse<List<GroceryListDto>>();

            try
            {
                var list = await _context.GroceryLists
                    .FirstOrDefaultAsync(l => l.Id == listId);

                if (list != null)
                {
                    list.Name = listName.Replace('_', ' ');
                    await _context.SaveChangesAsync();

                    response.Data = CreateListDtos(list.AppUserId);
                }
                else
                {
                    throw new Exception("List not found");
                }
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }
    }
}
