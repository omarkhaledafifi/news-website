using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsWebsite.BBL.DTOs.UserRequests
{
    public class UpdateUserRolesRequest
    {
        public string UserId { get; set; } = null!;
        public List<string> Roles { get; set; } = new();
    }

}
