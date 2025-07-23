using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsWebsite.BBL.DTOs
{
    public class UserManagementDto
    {
        public string Id { get; set; } = null!;
        public string UserName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string DisplayName { get; set; } = null!;
        public string PhoneNumber { get; set; } = null!;
        public string NationalId { get; set; } = null!;
        public string? ImageUrl { get; set; } = null!;
        public List<string> Roles { get; set; } = new();
    }

}
