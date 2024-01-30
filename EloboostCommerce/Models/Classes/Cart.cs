using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace EloboostCommerce.Models.Classes
{
    public partial class Cart
    {
        [Key]
        public int CartId { get; set; }

        public string UserId { get; set; } // Add this if you want to associate carts with users

        public ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();

    }
}
