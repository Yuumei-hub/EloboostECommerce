using EloboostCommerce.Services;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace EloboostCommerce.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUserService _userService;

        public AccountController(IUserService userService)
        {
            _userService = userService;
        }
        
        public IActionResult Index()
        {
            var currentUser = HttpContext.User;
            var userId = currentUser.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var userdata=_userService.GetById(int.Parse(userId));

            ViewBag.UserData = userdata;
            return View();
        }
    }
}
