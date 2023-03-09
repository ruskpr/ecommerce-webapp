using System.ComponentModel.DataAnnotations.Schema;

namespace SleekEcommerce.Models
{
    public class Product
    {
        public int Id { get; set; }
        [Column(TypeName = "varchar(100)")]
        public string Name { get; set; }
        public Category Category { get; set; }
        [Column(TypeName = "varchar(500)")]
        public string Desc { get; set; }
        [Column(TypeName = "varchar(8)")]
        public string SKU { get; set; }
        public int Quantity { get; set; }
        [Column(TypeName = "decimal")]
        public decimal Price { get; set; }
        public int Discount { get; set; }
        public DateTime DateCreated { get; set; }
        [Column(TypeName = "varchar(500)")]
        public string ImageLocation { get; set; }
    }
}
