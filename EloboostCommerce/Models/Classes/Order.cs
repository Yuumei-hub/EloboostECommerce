using Iyzipay.Model.V2.Subscription;
using System.ComponentModel.DataAnnotations;

namespace EloboostCommerce.Models.Classes
{
    public class Order
    {
        [Key]
        public int OrderId { get; set; }
        [Required]
        public User User { get; set; }
        public List<OrderItem> OrderItems { get; set; }
        public decimal Amount { get; set; }
        public string PaymentMethod { get; set; }
        public string Status { get; set; }
        public DateTime OrderDate { get; set; }
    }

}
