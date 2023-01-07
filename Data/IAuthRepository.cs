using GroceryListAPI.Models;

namespace GroceryListAPI.Data
{
    public interface IAuthRepository
    {
        Task<ServiceResponse<string>> Authenticate(string username, string password);
    }
}
