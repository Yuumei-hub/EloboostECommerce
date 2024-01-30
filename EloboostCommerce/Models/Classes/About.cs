using System.ComponentModel.DataAnnotations;

namespace EloboostCommerce.Models.Classes
{
    public class About
    {
        [Key]
        public int AboutId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
    }
}
