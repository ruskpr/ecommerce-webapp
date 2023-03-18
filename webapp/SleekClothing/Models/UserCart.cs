using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace SleekClothing.Models
{
    public class UserCart
    {

        [Key]
        [Required]
        public string UserId { get; set; }
        [Required]
        public string CartDataJSON { get; set; }
    }
}
