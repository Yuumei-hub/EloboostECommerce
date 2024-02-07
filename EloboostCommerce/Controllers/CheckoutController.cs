using Microsoft.AspNetCore.Mvc;
using Braintree;
using EloboostCommerce.Models.Classes;

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
                MerchantId = "mzdfpt5xgs2rz5r6",
                PublicKey= "2r9qpyh724v86vyv",
                PrivateKey= "a803df595ad0007c06e390b5aeb607fa"
            };
            _context = context;
        }

        [HttpGet]
        public IActionResult Index()
        {
            Cart cart = new Cart();
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
        public async Task<IActionResult> ProcessPayment(string nonce, decimal amount)
        {
            var request = new TransactionRequest { 
                Amount=amount,
                PaymentMethodNonce=nonce,
                Options=new TransactionOptionsRequest
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
