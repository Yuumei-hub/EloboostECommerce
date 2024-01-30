using System.ComponentModel.DataAnnotations;

namespace EloboostCommerce.Models.Classes
{
    public class Game
    {
        [Key]
        public int GameId { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public string? ImageUrl { get; set; }
        public string? ImageUrl2 { get; set; }
    }
}