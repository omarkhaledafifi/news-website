using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsWebsite.BBL.DTOs.UserRequests
{
    public record LoginRequest([EmailAddress] string Email, string Password);
}
