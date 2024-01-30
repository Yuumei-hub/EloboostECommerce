using System.ComponentModel.DataAnnotations;

namespace EntityLayer.Concrete
{
    public class Game
    {
        [Key]
        public int GameId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
    }
}