using Microsoft.AspNetCore.Mvc;

namespace Logick.Test.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TestController : Controller
    {
        [HttpGet]
        public IActionResult Get() => Ok();
    }
}