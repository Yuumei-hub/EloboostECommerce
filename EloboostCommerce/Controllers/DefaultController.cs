using EloboostCommerce.Models.Classes;
using Microsoft.AspNetCore.Mvc;

namespace EloboostCommerce.Controllers
{
    public class DefaultController : Controller
    {
        private readonly Context _context;
        public DefaultController(Context context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            var games = _context.Games.ToList();
            var testimonials = _context.Testimonials.ToList();
            var viewModel = new Tuple<List<Game>, List<Testimonial>>(games, testimonials);

            return View(viewModel);
        }
        public IActionResult ToLogin()
        {
            return RedirectToAction("Index","Login");
        }

    }
}
