using System.ComponentModel.DataAnnotations;

namespace EloboostCommerce.Models.Classes
{
    public class User
    {
        [Key]
        public int UserId { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public bool IsBooster { get; set; }

    }
}
