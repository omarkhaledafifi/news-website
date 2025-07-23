using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using NewsWebsite.BBL.DTOs;
using NewsWebsite.BBL.DTOs.UserRequests;
using NewsWebsite.DAL.Etities.Identity;
using NewsWebsite.Services.Abstraction;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace NewsWebsite.Services.Services
{
    public class AuthenticationService(UserManager<ApplicationUser> userManager,
                                         IConfiguration configuration,
                                         IOptions<JwtSettings> jwtOptions) : IAuthenticationService
    {
        private readonly UserManager<ApplicationUser> _userManager = userManager;
        private readonly IConfiguration _configuration = configuration;
        private readonly JwtSettings _jwtSettings = jwtOptions.Value;

        public async Task<UserResponse> LoginAsync(LoginRequest request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email) ?? throw new FileNotFoundException($"No user with the email : {request.Email} was found");
            var isvalid = await _userManager.CheckPasswordAsync(user, request.Password);
            if (!isvalid)
            {
                throw new UnauthorizedAccessException("Invalid credentials");
            }
            return new(request.Email, user.DisplayName, await CreateTokenAsync(user));

        }

        private async Task<string> CreateTokenAsync(ApplicationUser user)
        {
            var userClaims = await _userManager.GetClaimsAsync(user);
            var roles = await _userManager.GetRolesAsync(user);

            var roleClaims = roles.Select(role => new Claim(ClaimTypes.Role, role));

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim("DisplayName", user.DisplayName ?? string.Empty)
            }
            .Union(userClaims)
            .Union(roleClaims);

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _jwtSettings.Issuer,
                audience: _jwtSettings.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(_jwtSettings.DurationInMinutes),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public Task<UserResponse> RegisterAsync(RegisterRequest request)
        {
            var user = new ApplicationUser
            {
                UserName = request.Email,
                Email = request.Email,
                DisplayName = request.DisplayName,
                PhoneNumber = request.PhoneNumber,
                NationalId = request.NationalId
            };
            var result = _userManager.CreateAsync(user, request.Password).Result;
            if (!result.Succeeded)
            {
                throw new Exception(string.Join(", ", result.Errors.Select(e => e.Description)));
            }
            //adding the role "User" to the user
            var roleResult = _userManager.AddToRoleAsync(user, "User").Result;
            if (!roleResult.Succeeded)
            {
                throw new Exception(string.Join(", ", roleResult.Errors.Select(e => e.Description)));
            }

            return Task.FromResult(new UserResponse(request.Email, user.DisplayName, CreateTokenAsync(user).Result));
        }

        public async Task<IEnumerable<UserManagementDto>> GetAllUsersAsync()
        {
            var users = _userManager.Users.ToList();

            var result = new List<UserManagementDto>();

            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);

                result.Add(new UserManagementDto
                {
                    Id = user.Id,
                    UserName = user.UserName,
                    Email = user.Email,
                    DisplayName = user.DisplayName ?? string.Empty,
                    PhoneNumber = user.PhoneNumber ?? string.Empty,
                    NationalId = user.NationalId ?? string.Empty,
                    ImageUrl = user.ImageUrl ?? string.Empty,
                    Roles = roles.ToList()
                });
            }

            return result;
        }

        public async Task<UserManagementDto> GetUserByIdAsync(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null) return null;

            var roles = await _userManager.GetRolesAsync(user);

            return new UserManagementDto
            {
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email,
                DisplayName = user.DisplayName ?? string.Empty,
                PhoneNumber = user.PhoneNumber ?? string.Empty,
                NationalId = user.NationalId ?? string.Empty,
                ImageUrl = user.ImageUrl ?? string.Empty,
                Roles = roles.ToList()
            };
        }

        public async Task<bool> UpdateUserAsync(UserManagementDto updatedUser)
        {
            var user = await _userManager.FindByIdAsync(updatedUser.Id);
            if(user == null) return false;

            user.UserName = updatedUser.UserName;
            user.Email = updatedUser.Email;
            user.DisplayName = updatedUser.DisplayName;
            user.PhoneNumber = updatedUser.PhoneNumber;
            user.NationalId = updatedUser.NationalId;
            user.ImageUrl = updatedUser.ImageUrl;

            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
                throw new Exception(string.Join(", ", result.Errors.Select(e => e.Description)));

            return true;
        }

        public async Task<bool> UpdateUserRolesAsync(string id, List<string> roles)
        {
            var user = await _userManager.FindByIdAsync(id);
            if(user == null) return false;

            var currentRoles = await _userManager.GetRolesAsync(user);

            var removeResult = await _userManager.RemoveFromRolesAsync(user, currentRoles);
            if (!removeResult.Succeeded)
                return false;

            var addResult = await _userManager.AddToRolesAsync(user, roles);
            if (!addResult.Succeeded)
            {
                await _userManager.AddToRolesAsync(user, currentRoles);
                return false;
            }
            return true;
        }

        public async Task<UserManagementDto> CreateUserAsync(RegisterRequest request, List<string> roles)
        {
            var user = new ApplicationUser
            {
                UserName = request.UserName,
                Email = request.Email,
                DisplayName = request.DisplayName,
                PhoneNumber = request.PhoneNumber,
                NationalId = request.NationalId,
            };

            var result = await _userManager.CreateAsync(user, request.Password);
            if (!result.Succeeded)
            {
                throw new Exception(string.Join(", ", result.Errors.Select(e => e.Description)));
            }

            var roleResult = await _userManager.AddToRolesAsync(user, roles);
            if (!roleResult.Succeeded)
            {
                throw new Exception(string.Join(", ", roleResult.Errors.Select(e => e.Description)));
            }

            return new UserManagementDto
            {
                Id = user.Id,
                UserName = user.UserName!,
                Email = user.Email!,
                DisplayName = user.DisplayName!,
                PhoneNumber = user.PhoneNumber!,
                NationalId = user.NationalId!,
                ImageUrl = user.ImageUrl!,
                Roles = roles
            };
        }

        public async Task<bool> DeleteUserAsync(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null) return false;

            var result = await _userManager.DeleteAsync(user);

            if (!result.Succeeded) return false;

            return true;
        }


    }
}
