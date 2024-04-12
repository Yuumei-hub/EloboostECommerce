using EloboostCommerce.Models.Classes;
using Microsoft.AspNetCore.Mvc;

namespace EloboostCommerce.Controllers
{
    public class OrderController : Controller
    {
        private readonly Context _context;
        public OrderController(Context context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            var orders = _context.Orders.ToList();
            return View(orders);
        }

        public IActionResult PaymentSuccessful()
        {
            return View();
        }

        public IActionResult Details(string orderId)
        {
            var order = _context.Orders.FirstOrDefault(o => o.OrderId == orderId);
            if (order == null)
                return NotFound();
            return View(order);
        }

        public IActionResult CreateOrder()
        {
            return View();
        }

        //GET
        public IActionResult EditOrder(string orderId)
        {
            var order = _context.Orders.FirstOrDefault(o=>o.OrderId==orderId);
            if(order==null)
                return NotFound();

            return View(order);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditOrder(string orderId, Order order)
        {
            if (orderId != order.OrderId)
                return BadRequest();
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Orders.Update(order);
                    _context.SaveChanges();
                }
                catch (Exception)
                {
                    return NotFound();
                }
                return RedirectToAction(nameof(Index));
            }
            return View(order);
        }

        public IActionResult Delete(string orderId)
        {
            var order = _context.Orders.FirstOrDefault(o=>o.OrderId==orderId);
            if (order == null)
            {
                return NotFound();
            }
            return View(order);
        
        }


        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(string orderId)
        {
            var order = _context.Orders.FirstOrDefault(o => o.OrderId == orderId);
            if (order == null)
            {
                return NotFound();
            }

            _context.Orders.Remove(order);
            _context.SaveChanges();

            return View(order);
        }

    }
}
