using GroceryListAPI.Data;
using GroceryListAPI.Models;
using GroceryListAPI.Models.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace GroceryListAPI.Controllers
{
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthRepository _authRepo;

        public AuthController(IAuthRepository authRepo)
        {
            _authRepo = authRepo;
        }

        [HttpPost("Authenticate")]
        public async Task<ActionResult<ServiceResponse<string>>> Authenticate(UserLoginDto request)
        {
            var response = await _authRepo.Authenticate(request.Username, request.Password);
            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }
    }
}
