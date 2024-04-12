using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EloboostCommerce.Models.Classes
{
    public class Order
    {
        [Key]
        public string OrderId { get; set; }
        [Required]
        public User User { get; set; } //to define which User it belongs to
        [Column(TypeName ="decimal(18,2)")]
        public decimal Amount { get; set; }//Price
        public string PaymentMethod { get; set; }
        public string Status { get; set; }//ACTIVE,CANCELLED,COMPLETED
        public DateTime OrderDate { get; set; }
        public string Description { get; set; }
    }

}
