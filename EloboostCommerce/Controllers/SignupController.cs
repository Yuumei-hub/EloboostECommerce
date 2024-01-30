using EloboostCommerce.Models.Classes;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Mvc;

namespace EloboostCommerce.Controllers
{
    public class SignupController : Controller
    {
        private readonly Context _context;
        public SignupController(Context context)
        {
            _context = context;
        }

        [HttpPost]
        public  IActionResult Register(User user)
        {
            if(_context.Users.Any(u => u.Email == user.Email || u.Username==user.Username))
            {
                ModelState.AddModelError("Email", "Email is already in use.");
                return RedirectToAction("Index","Login");
            }
            else
            {
                _context.Users.Add(user);
                _context.SaveChanges();
                return RedirectToAction("Index","Login");
            }
        }
    }
}
