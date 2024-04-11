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

namespace EloboostCommerce.Controllers
{
    public class CheckoutController : Controller
    {
        private readonly Context _context;
        private string _conversationId = Guid.NewGuid().ToString();

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

            Options options = new Options
            {
                ApiKey = "sandbox-Ikx8vXkZKX6YJDDlVWGDlMn4DYnyRNek",
                SecretKey = "sandbox-yhvcanALA1mdYI5mMAUkoizZJ4eSu5Sr",
                BaseUrl = "https://sandbox-api.iyzipay.com"
            };

            CreatePaymentRequest request = new CreatePaymentRequest
            {
                Locale = Locale.EN.ToString(),
                ConversationId = _conversationId,
                Price = CalculatePrice().ToString(),
                PaidPrice = CalculatePrice().ToString(),
                Currency = Currency.USD.ToString(),
                Installment = 1,
                BasketId = cart.CartId.ToString(),
                PaymentChannel = PaymentChannel.WEB.ToString(),
                PaymentGroup = PaymentGroup.PRODUCT.ToString()
            };
            request.CallbackUrl = "https://localhost:44385/Checkout/PaymentCallback";

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
            request.Buyer = buyer;

            Address billingAddress = new Address
            {
                ContactName = "Jane Doe",
                City = "Istanbul",
                Country = "Turkey",
                Description = "Nidakule Göztepe, Merdivenköy Mah. Bora Sok. No:1",
                ZipCode = "34742"
            };
            request.BillingAddress = billingAddress;

            List<BasketItem> basketItems = new List<BasketItem>();

            foreach (var cartItem in cart.CartItems)
            {
                BasketItem basketItemx = new BasketItem
                {
                    Id = cartItem.CartItemId.ToString(),
                    Name = cartItem.GameName,
                    Category1 = "Eloboost",
                    Category2 = "category2",
                    ItemType = BasketItemType.VIRTUAL.ToString(),
                    Price = cartItem.Price.ToString(),
                };
                 basketItems.Add(basketItemx);
            }
            request.BasketItems = basketItems;

            CreateCheckoutFormInitializeRequest cfrequest = new CreateCheckoutFormInitializeRequest()
            {
                Locale = request.Locale,
                ConversationId = request.ConversationId,
                Price = request.Price,
                PaidPrice = request.PaidPrice,
                Currency = request.Currency,
                EnabledInstallments = new List<int> { 2, 3, 6, 9 },
                BasketId = request.BasketId,
                PaymentGroup = PaymentGroup.PRODUCT.ToString(),
                Buyer = request.Buyer,
                BasketItems = request.BasketItems,
                BillingAddress = request.BillingAddress,
                CallbackUrl = request.CallbackUrl,
                PosOrderId = request.PosOrderId,
                PaymentSource = request.PaymentSource,
            };

            CheckoutFormInitialize checkoutFormInitialize = CheckoutFormInitialize.Create(cfrequest, options);
            return Redirect(checkoutFormInitialize.PaymentPageUrl);
        }

        [HttpPost]
        public async Task<IActionResult> PaymentCallback()
        {
            using (var httpClient = new HttpClient())
            {
                var body = await new StreamReader(Request.Body).ReadToEndAsync();
                var token = Regex.Match(body, @"(?<=token=)[a-zA-Z0-9\-]+").Value;
                var pwiRequest = new
                {
                    token = token,
                    conversationId = _conversationId,
                    locale = "en",
                };
                var pwiJson = JsonConvert.SerializeObject(pwiRequest);
                var baseUrl = "https://sandbox-api.iyzipay.com";
                try
                {
                    HttpResponseMessage response = await httpClient.PostAsync(baseUrl, new StringContent(pwiJson, Encoding.UTF8, "application/json"));

                    if (response.IsSuccessStatusCode)
                    {
                        var responseContent = await response.Content.ReadAsStringAsync();
                        return Ok(responseContent);
                        // Başarılı PWI Sorgulama yanıtını işle
                    }
                    else
                    {
                        // Başarısız PWI Sorgulama yanıtını işle
                        return BadRequest("PWI Sorgulama başarısız! Hata kodu: " + response.StatusCode);
                    }
                }
                catch (Exception ex)
                {
                    // Hata durumunda işle
                    return BadRequest("PWI Sorgulama sırasında bir hata oluştu: " + ex.Message);
                }
            }

        }
    }
}
