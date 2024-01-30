using System.ComponentModel.DataAnnotations;

namespace EloboostCommerce.Models.Classes
{
    public class Testimonial
    {
        [Key]
        public int TestimonialId { get; set; }
        public string Name { get; set; }
        public string Comment { get; set; }
        public string ImageUrl { get; set; }
        public string Date { get; set; }
    }
}
