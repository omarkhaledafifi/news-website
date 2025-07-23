using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsWebsite.BBL.DTOs.UserRequests
{
    public record RegisterRequest
        ([EmailAddress] string Email, string Password, string UserName, string DisplayName, string PhoneNumber, string NationalId);
}
