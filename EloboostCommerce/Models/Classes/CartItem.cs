using System.ComponentModel.DataAnnotations;

namespace EloboostCommerce.Models.Classes
{
    public class CartItem
    {
        [Key]
        public int CartItemId { get; set; }

        [Required]
        public int Quantity { get; set; }

        [Required]
        public string GameImgUrl { get; set; }

        [Required]
        public string GameName { get; set; }

        [Required]
        public double Price { get; set; }

        [Required]
        public string FromSkillRating { get; set; }

        [Required]
        public string ToSkillRating { get; set; }

    }
}
