using System.ComponentModel.DataAnnotations;

namespace EloboostCommerce.Models.Classes
{
    public class Order
    {
        [Key]
        public int GameId { get; set; }
        public string? ToRankName { get; set; }
        public string? FromRankName { get; set; }
        public int? ToRankValue { get; set; }
        public int? FromRankValue { get; set; }
        public int? Price { get; set; }
    }
}
