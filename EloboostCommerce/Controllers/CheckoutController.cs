using Microsoft.AspNetCore.Mvc;
using Braintree;
using EloboostCommerce.Models.Classes;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace EloboostCommerce.Controllers
{
    public class CheckoutController : Controller
    {
        private readonly BraintreeGateway _gateway;
        private readonly Context _context;
        public CheckoutController(Context context)
        {
            _gateway = new BraintreeGateway {
                Environment = Braintree.Environment.SANDBOX,
                MerchantId = "c9rs7fgqwjgk2r5m",
                PublicKey= "grr83dzzx7knhhbd",
                PrivateKey= "66d4114467e41e1af39a46ae7f22924e"
            };
            _context = context;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            //test commment
            var cart = _context.Carts.Include(c => c.CartItems)
                .FirstOrDefault(c => c.UserId == userId);
            decimal totalPrice = 0;
            foreach (var item in cart.CartItems)
            {
                totalPrice += Convert.ToDecimal(item.Price) * item.Quantity;
            }
            ViewBag.TotalPrice = totalPrice;
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ProcessPayment(string cardNumber, string expiryDate, string cvv, string holderName ,string nonce, decimal amount)
        {
            var request = new TransactionRequest { 
                Amount=amount,
                PaymentMethodNonce=nonce,
                CreditCard=new TransactionCreditCardRequest
                {
                    Number=cardNumber,
                    CardholderName=holderName,
                    ExpirationDate=expiryDate,
                    CVV=cvv,
                },
                Options= new TransactionOptionsRequest
                {
                    SubmitForSettlement=true
                }
            };

            var result = await _gateway.Transaction.SaleAsync(request);

            if (result.IsSuccess())
            {
                var transactionId = result.Target.Id;
                return RedirectToAction("PaymentSuccess", new { transactionId });
            }
            else
            {
                var errorMessage = result.Message;
                return RedirectToAction("PaymentError", new { errorMessage });
            }
        }
    
        public ActionResult PaymentSuccess(string transactionId)
        {
            ViewBag.TransactionId = transactionId;
            return View();
        }
        public ActionResult PaymentError(string errorMessage)
        {
            ViewBag.ErrorMessage = errorMessage;
            return View();
        }
    }
}
