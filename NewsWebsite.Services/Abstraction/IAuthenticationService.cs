using NewsWebsite.BBL.DTOs;
using NewsWebsite.BBL.DTOs.UserRequests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsWebsite.Services.Abstraction
{
    public interface IAuthenticationService
    {
        Task<UserResponse> LoginAsync(LoginRequest request);
        Task<UserResponse> RegisterAsync(RegisterRequest request);

        // ✅ User Management Methods
        Task<IEnumerable<UserManagementDto>> GetAllUsersAsync();
        Task<UserManagementDto> GetUserByIdAsync(string id);
        Task<bool> UpdateUserAsync(UserManagementDto updatedUser);
        Task<bool> UpdateUserRolesAsync(string id, List<string> roles);
        Task<bool> DeleteUserAsync(string id);
        Task<UserManagementDto> CreateUserAsync(RegisterRequest request, List<string> roles);
    }

}
