using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NewsWebsite.BBL.DTOs;
using NewsWebsite.BBL.DTOs.UserRequests;
using NewsWebsite.Services.Abstraction;

namespace NewsWebApp.Controllers
{
    [Authorize(Roles = "SuperAdmin")]
    public class SuperAdminController(IServiceManager serviceManager) : BaseController
    {
        private readonly IAuthenticationService _authService = serviceManager.AuthenticationService;

        // GET: api/users
        [HttpGet()]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _authService.GetAllUsersAsync();
            return Ok(users);
        }

        // GET: api/users/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(string id)
        {
            var user = await _authService.GetUserByIdAsync(id);
            if (user == null) return NotFound();
            return Ok(user);
        }

        // POST: api/users
        [HttpPost()]
        public async Task<IActionResult> CreateUser([FromBody] SuperAdminCreateUserRequest request)
        {
            var user = await _authService.CreateUserAsync(request.ToRegisterRequest(), request.Roles);
            return Ok(user);
        }

        // PUT: api/users/update-user
        [HttpPut("update-user")]
        public async Task<IActionResult> UpdateUser([FromBody] UserManagementDto updatedUser)
        {
            var success = await _authService.UpdateUserAsync(updatedUser);
            if (success) return NoContent();
            return BadRequest();
        }

        [HttpPut("update-roles")]
        public async Task<IActionResult> UpdateUserRoles([FromBody] UpdateUserRolesRequest request)
        {
            var success = await _authService.UpdateUserRolesAsync(request.UserId, request.Roles);
            if (success) return NoContent(); // 204 No Content if successful
            return BadRequest();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(string id)
        {
            var success = await _authService.DeleteUserAsync(id);
            if (success) return NoContent(); // 204 response
            return BadRequest();
        }

    }
}
