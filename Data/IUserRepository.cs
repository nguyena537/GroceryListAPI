using GroceryListAPI.Models;

namespace GroceryListAPI.Data
{
    public interface IUserRepository
    {
        Task<ServiceResponse<int>> UserRegister(AppUser user, string password);
        Task<ServiceResponse<int>> UserLogin(string username, string password);
        Task<bool> UserExists(string username);
    }
}
