using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsWebsite.BBL.DTOs.UserRequests
{
    public class SuperAdminCreateUserRequest
    {
        public string Email { get; set; } = null!;
        public string DisplayName { get; set; } = null!;
        public string PhoneNumber { get; set; } = null!;
        public string NationalId { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string UserName { get; set; } = null!;
        public string? ImageUrl { get; set; }
        public List<string> Roles { get; set; } = [];

        public RegisterRequest ToRegisterRequest()
        {
            return new RegisterRequest(Email, Password, UserName, DisplayName, PhoneNumber, NationalId);
        }

    }
}
