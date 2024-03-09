using System.ComponentModel.DataAnnotations;

namespace EloboostCommerce.Models.Classes
{
    public class Order
    {
        [Key]
        public int OrderId { get; set; }
        [Required]
        public string CustomerName { get; set; }
        public int CustomerId { get; set; } //FK
        [Required]
        public string ProductName { get; set; }
        public int ProductId { get; set; }//FK
        [Required]
        public int? Price { get; set; }
        public DateTime OrderDate { get; set; }
    }
}
