using May1Homework.Data;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using System.Security.Claims;

namespace May1Homework.Web.Controllers
{
    public class AccountController : Controller
    {
        private string _connectionString;
        public AccountController(IConfiguration configuration)
        {
            _connectionString =configuration.GetConnectionString("ConStr");
        }

        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Signup()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Signup(string password, User user)
        {
            UserRepository repo = new(_connectionString);
            repo.SignUp(user, password);
            return Redirect("/account/login");
        }
        public IActionResult Login()
        {
            string message = (string)TempData["message"];

            return View(model: message);
        }
        [HttpPost]
        public IActionResult Login(string email, string password)
        {
            UserRepository repo = new(_connectionString);
            User u = repo.Login(email, password);
            if(u == null)
            {
                TempData["message"] = "Invalid Login";
                return Redirect("/account/login");
            }

            var claims = new List<Claim>
            {
                new Claim("user", email)
            };

            HttpContext.SignInAsync(new ClaimsPrincipal(
                new ClaimsIdentity(claims, "Cookies", "user", "role"))).Wait();

            return Redirect("/");
        }
        public IActionResult Logout()
        {
            HttpContext.SignOutAsync().Wait();
            return Redirect("/");
        }

    }
}
