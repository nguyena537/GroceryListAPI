using GroceryListAPI.Data;
using GroceryListAPI.Models;
using GroceryListAPI.Models.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GroceryListAPI.Controllers
{
    [ApiController]
    [Authorize]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _userRepo;

        public UserController(IUserRepository userRepo)
        {
            _userRepo = userRepo;
        }

        [HttpPost("UserRegister")]
        public async Task<ActionResult<ServiceResponse<int>>> UserRegister(UserRegisterDto request)
        {
            var response = await _userRepo.UserRegister(
                new AppUser
                {
                    Username = request.Username,
                    FirstName = request.FirstName,
                    LastName = request.LastName,
                    Email = request.Email,
                }, request.Password
            );
            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }

        [HttpPost("UserLogin")]
        public async Task<ActionResult<ServiceResponse<int>>> UserLogin(UserLoginDto request)
        {
            var response = await _userRepo.UserLogin(request.Username, request.Password);
            return Ok(response);
        }
    }
}
