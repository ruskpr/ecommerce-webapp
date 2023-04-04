using Microsoft.Build.Framework;
using System.ComponentModel.DataAnnotations;
using RequiredAttribute = System.ComponentModel.DataAnnotations.RequiredAttribute;

namespace shopping_cart_russ.Models
{
    public class Order
    {

        [Key]
        [Required]
        public int Id { get; set; }

        [Required]
        [MaxLength(50, ErrorMessage = "First name must be less than 50 characters.")]
        [Display(Name = "first name")]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(50, ErrorMessage = "Last name must be less than 50 characters.")]
        [Display(Name = "last name")]
        public string LastName { get; set; }

        [Required]
        [MaxLength(100, ErrorMessage = "Address must be less than 100 characters.")]
        [Display(Name = "address")]
        public string Address { get; set; }

        [Required]
        [Display(Name = "address 2")]
        public string? Address2 { get; set; } = null;

        [Required]
        [Display(Name = "country")]
        public string Country { get; set; }

        [Required]
        [Display(Name = "province")]
        public string Province { get; set; }

        [Required]
        [MaxLength(10, ErrorMessage = "Postal code must be less that 10 characters.")]
        [Display(Name = "postal code")]
        public string PostalCode { get; set; }

        [Required]
        public DateTime DateOrdered { get; set; }

    }
}
