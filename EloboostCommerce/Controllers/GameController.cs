using EloboostCommerce.Models.Classes;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

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
            Game overwatch = _context.Games.First(g => g.GameId == 1);
            // Serialize the model into JSON format
            var serializedModel = JsonConvert.SerializeObject(overwatch);

            // Pass the serialized JSON data to the view
            ViewBag.SerializedModel = serializedModel;
            return View(overwatch);
        }
    }
}
