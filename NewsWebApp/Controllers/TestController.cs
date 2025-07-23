using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace NewsWebApp.Controllers
{

    public class TestController : BaseController
    {
        [HttpGet]
        [Authorize]
        public IActionResult Get() => Ok("Test OK");
    }
}
