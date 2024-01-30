using EloboostCommerce.Models.Classes;
using Microsoft.AspNetCore.Mvc;

namespace EloboostCommerce.Controllers
{
    public class GameController : Controller
    {
        private readonly Context _context;
        public GameController(Context context)
        {
            _context=context;
        }
        public IActionResult Index()
        {
            var games = _context.Games.ToList();

            return View(games);
        }

        public IActionResult Overwatch()
        {
            return View();
        }
    }
}
