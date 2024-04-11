using Iyzipay.Model.V2.Subscription;

namespace EloboostCommerce.Models.Classes
{
    public class OrderItem
    {
        public int OrderItemId { get; set; }
        public Product product { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
    }
}
