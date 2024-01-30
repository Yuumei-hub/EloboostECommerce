using EloboostCommerce.Models.Classes;
using Microsoft.AspNetCore.Mvc;
using System.Web;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;

namespace EloboostCommerce.Controllers
{
    public class LoginController : Controller
    {
        private readonly Context _context;
        public LoginController(Context context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult LoginForm( User user)
        {
            var userfromDb= _context.Users.FirstOrDefault(x=>x.Email==user.Email&& x.Password==user.Password);
            if (userfromDb == null)
            {
                ViewBag.ErrorMessage = "Invalid email or password.";
                return View("Index","Login");
            }
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name,userfromDb.Username),
                new Claim(ClaimTypes.Email,userfromDb.Email),
                new Claim(ClaimTypes.NameIdentifier,userfromDb.UserId.ToString())
            };
            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var authProperties = new AuthenticationProperties { };
            HttpContext.SignInAsync(new ClaimsPrincipal(claimsIdentity), authProperties);
            return RedirectToAction("Index", "Default");
        }

        /*
        [HttpGet]
        public PartialViewResult Partial1()
        {
            return PartialView();
        }
        [HttpPost]
        public IActionResult RegisterForm(User user)
        {
            using (_context)
            {
                bool userExists = _context.Users.Any(u => u.Username == user.Username || u.Email==user.Email);

                if (userExists)
                {
                    ModelState.AddModelError("", "This username or email already exist.");
                    return PartialView();
                }
                else
                {
                    user.IsBooster = false;
                    _context.Users.Add(user);
                    _context.SaveChanges();
                    return View("Index");
                }
            }
        }

        [HttpGet]
        public PartialViewResult Partial2()
        {
            return PartialView();
        }
        [HttpPost]
        public IActionResult LoginForm(User user)
        {
            using (_context)
            {
                bool userExists = _context.Users.Any(u => u.Username == user.Username && u.Password == user.Password);
                if (userExists)
                {
                    if (user.IsBooster)
                        return RedirectToAction("Index","Booster");
                    else
                        return RedirectToAction("Index","Default");
                }
                else
                    return PartialView();
            }
        }
        */
    }
}
