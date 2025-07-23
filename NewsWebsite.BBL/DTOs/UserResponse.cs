using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsWebsite.BBL.DTOs
{
    public record UserResponse(string Email, string DisplayName, string Token);
}
