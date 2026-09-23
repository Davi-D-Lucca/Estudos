using Microsoft.AspNetCore.Mvc;
using PrimeiraApi.Model;
using PrimeiraApi.Services;

namespace PrimeiraApi.Controllers
{
    public class AuthController : Controller
    {
        [HttpPost]
        public IActionResult Auth(string username, string password)
        {
            if(username == "admin" && password == "123")
            {
                var token = TokenService.GenerateToken(new Employee());
            }

            return BadRequest("Username or password invalid");
        }
    }
}
