using EloboostCommerce.Models.Classes;
using Humanizer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace EloboostCommerce.Controllers
{
    public class CartController : Controller
    {
        private readonly Context _context;
        public CartController(Context context)
        {
            _context = context;
        }

        [HttpPost]
        public JsonResult AddCartItem(int quantity,string gameImgUrl, string gameName,double price,string fromSkillRating, string toSkillRating)
        {
            CartItem cartItem = new CartItem { 
                
                Quantity=quantity,
                Price = price, 
                GameImgUrl=gameImgUrl,
                GameName=gameName,
                FromSkillRating=fromSkillRating,
                ToSkillRating=toSkillRating
            };
            if (ModelState.IsValid)
            {
                Cart cart = GetOrCreateCartForUser();
                var existingItem = cart.CartItems.FirstOrDefault(item => item.FromSkillRating == cartItem.FromSkillRating &&
                item.ToSkillRating==cartItem.ToSkillRating&&
                item.Price==cartItem.Price&&
                item.GameName==cartItem.GameName&&
                item.GameImgUrl==cartItem.GameImgUrl);
                if (existingItem != null)
                    existingItem.Quantity += cartItem.Quantity;
                else
                    cart.CartItems.Add(cartItem);
                _context.SaveChanges();
                return Json(new { success = true, message = "Successful data" });
            }
            return Json(new { success = false, message = "Invalid data" });
        }
        public IActionResult Index()
        {
            Cart cart;
            try
            {
                cart = GetOrCreateCartForUser();
            }
            catch (InvalidOperationException ex)
            {
                return RedirectToAction("Index", "Login");
            }

            return View(cart);
        }

        public IActionResult DeleteCartItem(int cartItemid)
        {
            Cart cart;

            try
            {
                cart = GetOrCreateCartForUser();
            }
            catch (InvalidOperationException ex)
            {
                return RedirectToAction("Index","Login");
            }

            CartItem cartItem = _context.CartItems.FirstOrDefault(item => item.CartItemId == cartItemid);
            if(cartItem != null)
            {
                cart.CartItems.Remove(cartItem);
                _context.Remove(cartItem);
                _context.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        private Cart GetOrCreateCartForUser()
        {
            int userId = 3;
            /*
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;*/
            if (userId == null)
            {
                // Redirect to login page or take appropriate action
                throw new InvalidOperationException("User is not authenticated");
            }

            var cart = _context.Carts
                .Include(c => c.CartItems)
                .FirstOrDefault(c => c.UserId == userId.ToString());

            if (cart == null)
            {
                cart = new Cart { UserId = userId.ToString() };
                _context.Carts.Add(cart);
                _context.SaveChanges();
            }
            return cart;
        }
    }
}
