using GroceryListAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace GroceryListAPI.Data
{
    public class UserRepository : IUserRepository
    {
        private readonly GroceryListsDbContext _context;
        private readonly IConfiguration _configuration;

        public UserRepository(GroceryListsDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public async Task<ServiceResponse<int>> UserRegister(AppUser user, string password)
        {
            var response = new ServiceResponse<int>();

            if (await UserExists(user.Username))
            {
                response.Success = false;
                response.Message = "User already exists.";
                return response;
            }

            CreatePasswordHash(password, out byte[] passwordHash, out byte[] passwordSalt);

            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;

            _context.AppUsers.Add(user);
            await _context.SaveChangesAsync();
            response.Data = user.Id;

            return response;
        }
        public async Task<ServiceResponse<int>> UserLogin(string username, string password)
        {
            var response = new ServiceResponse<int>();
            var user = await _context.AppUsers
                .FirstOrDefaultAsync(u => u.Username.ToLower().Equals(username.ToLower()));

            if (user == null)
            {
                response.Success = false;
                response.Message = "User not found.";
            }
            else if (!VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt))
            {
                response.Success = false;
                response.Message = "Incorrect password.";
            }
            else
            {
                response.Data = user.Id;
            }

            return response;
        }

        public async Task<bool> UserExists(string username)
        {
            if (await _context.AppUsers.AnyAsync(u => u.Username.ToLower().Equals(username.ToLower())))
            {
                return true;
            }

            return false;
        }

        

        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512(passwordSalt))
            {
                var computeHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                return computeHash.SequenceEqual(passwordHash);
            }
        }
    }
}
