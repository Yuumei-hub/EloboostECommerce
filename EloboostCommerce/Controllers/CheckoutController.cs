using Microsoft.AspNetCore.Mvc;
using EloboostCommerce.Models.Classes;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Iyzipay.Model;
using Iyzipay;
using System.Linq;
using Newtonsoft.Json.Linq;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using EloboostCommerce.Services;
using System.Collections.Specialized;
using Iyzipay.Request;
using System.Security.Cryptography.X509Certificates;
using Options = Iyzipay.Options;
using Payment = Iyzipay.Model.Payment;
using CreatePaymentRequest = Iyzipay.Request.CreatePaymentRequest;
using System.Net;
using Newtonsoft.Json;
using System.Text;
using System.Text.RegularExpressions;
using HttpClient = System.Net.Http.HttpClient;
using System.Globalization;
using NuGet.ContentModel;
using System.Net.Http;
using OrderItem = Iyzipay.Model.OrderItem;

namespace EloboostCommerce.Controllers
{
    public class CheckoutController : Controller
    {
        private readonly Context _context;
        private string _conversationId = Guid.NewGuid().ToString();
        Options options = new Options
        {
            ApiKey = "sandbox-Ikx8vXkZKX6YJDDlVWGDlMn4DYnyRNek",
            SecretKey = "sandbox-yhvcanALA1mdYI5mMAUkoizZJ4eSu5Sr",
            BaseUrl = "https://sandbox-api.iyzipay.com"
        };
        public CheckoutController(Context context)
        {
            _context = context;
        }

        public decimal CalculatePrice()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var cart = _context.Carts.Include(c => c.CartItems).FirstOrDefault(c => c.UserId == userId);

            decimal price = 0;
            foreach (var item in cart.CartItems)
            {
                price += Convert.ToDecimal(item.Price) * item.Quantity;
            }
            return price;
        }
        //----------------------------

        //CF Başlatma
        public IActionResult PaymentProcess()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var cart = _context.Carts.Include(c => c.CartItems)
                .FirstOrDefault(c => c.UserId == userId);

            Buyer buyer = new Buyer
            {
                Id = "userId",
                Name = "username",
                Surname = "surname",
                GsmNumber = "+905350000000",
                Email = "email@email.com",
                IdentityNumber = "12332112332",
                RegistrationAddress = "address",
                City = "cityname",
                Country = "countryname",
                ZipCode = ""
            };

            Address billingAddress = new Address
            {
                ContactName = "Jane Doe",
                City = "Istanbul",
                Country = "Turkey",
                Description = "Nidakule Göztepe, Merdivenköy Mah. Bora Sok. No:1",
                ZipCode = "34742"
            };

            List<BasketItem> basketItems = new List<BasketItem>();

            foreach (var cartItem in cart.CartItems)
            {
                BasketItem basketItemx = new BasketItem
                {
                    Id = cartItem.CartItemId.ToString(),
                    Name = cartItem.GameName,
                    Category1 = "Eloboost",
                    Category2 = "Online Game Items",
                    ItemType = BasketItemType.VIRTUAL.ToString(),
                    Price = cartItem.Price.ToString(),
                };
                 basketItems.Add(basketItemx);
            }
            CreatePaymentRequest request = new CreatePaymentRequest
            {
                Locale = Locale.EN.ToString(),
                ConversationId = _conversationId,
                Price = CalculatePrice().ToString(CultureInfo.InvariantCulture),
                PaidPrice = (CalculatePrice() + CalculatePrice() / 10).ToString(CultureInfo.InvariantCulture),
                Currency = Currency.USD.ToString(),
                Installment = 1,
                BasketId = cart.CartId.ToString(),
                PaymentChannel = PaymentChannel.WEB.ToString(),
                PaymentGroup = PaymentGroup.PRODUCT.ToString()
            };
            CreateCheckoutFormInitializeRequest cfrequest = new CreateCheckoutFormInitializeRequest()
            {
                Locale = Locale.EN.ToString(),
                ConversationId = _conversationId,
                Price = CalculatePrice().ToString(CultureInfo.InvariantCulture),
                PaidPrice = (CalculatePrice()+CalculatePrice()/10).ToString(CultureInfo.InvariantCulture),
                Currency = Currency.USD.ToString(),
                EnabledInstallments = new List<int> { 2, 3, 6, 9 },
                BasketId = cart.CartId.ToString(),
                PaymentGroup = PaymentGroup.PRODUCT.ToString(),
                Buyer = buyer,
                BasketItems = basketItems,
                BillingAddress = billingAddress,
                CallbackUrl = "https://localhost:44385/Checkout/PaymentCallback",
            };

            CheckoutFormInitialize checkoutFormInitialize = CheckoutFormInitialize.Create(cfrequest, options);
            return Redirect(checkoutFormInitialize.PaymentPageUrl);
        }

        [HttpPost]
        public async Task<IActionResult> PaymentCallback()
        {
            using (var httpClient= new HttpClient())
            {
                var body = await new StreamReader(Request.Body).ReadToEndAsync();
                var token = Regex.Match(body, @"(?<=token=)[a-zA-Z0-9\-]+").Value;
                RetrievePayWithIyzicoRequest pwiRequest = new RetrievePayWithIyzicoRequest();
                pwiRequest.ConversationId = _conversationId;
                pwiRequest.Token = token;
                pwiRequest.Locale=Locale.EN.ToString();

                PayWithIyzico payWithIyzicoResult = PayWithIyzico.Retrieve(pwiRequest, options);

                if (payWithIyzicoResult.Status == "success")
                {
                    var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                    User user = _context.Users.FirstOrDefault(u => u.UserId.ToString() == userId);
                    Order order = new Order()
                    {
                        OrderId = Guid.NewGuid().ToString(),
                        OrderDate = DateTime.Now,
                        Amount = decimal.Parse(payWithIyzicoResult.Price),
                        PaymentMethod = payWithIyzicoResult.CardAssociation + " " + payWithIyzicoResult.CardType,
                        Status=payWithIyzicoResult.Status,
                        User=user,
                        Description=""
                    };
                    _context.Orders.Add(order);
                    _context.SaveChangesAsync();

                    return RedirectToAction("Index","Order");
                }
                else
                    return Redirect("PaymentError");
            }

        }
    }
}
