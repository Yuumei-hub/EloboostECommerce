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
        public JsonResult AddCartItem(int quantity,double price,string fromSkillRating, string toSkillRating, int gameId)
        {
            Game game = _context.Games.FirstOrDefault(g=>g.GameId==gameId);
            //creates the cart item obj
            CartItem cartItem = new CartItem {
                GameName = game.Title,
                GameImgUrl= game.ImageUrl,
                Quantity=quantity,
                Price = price, 
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
            //int userId = 3;
            //check login
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
            {
                // Redirect to login page or take appropriate action
                throw new InvalidOperationException("User is not authenticated");
            }

            //checks if there is any cart that belongs to the user then adds the item
            var cart = _context.Carts
                .Include(c => c.CartItems)
                .FirstOrDefault(c => c.UserId == userId.ToString());

            //if no cart's found creates cart
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
