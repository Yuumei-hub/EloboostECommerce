using Microsoft.AspNetCore.Mvc;

namespace EloboostCommerce.Controllers
{
    public class CheckoutController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult ProcessPayment()
        {

            return View();
        }
    }
}
