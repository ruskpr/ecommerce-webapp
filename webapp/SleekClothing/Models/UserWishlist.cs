using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace SleekClothing.Models
{
    public class UserWishlist
    {

        [Key]
        [Required]
        public string UserId { get; set; }
        [Required]
        public string WishlistDataJSON { get; set; }
    }
}
