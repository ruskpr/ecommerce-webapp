using System.ComponentModel.DataAnnotations.Schema;

namespace SleekEcommerce.Models
{
    public class Product
    {
        public int Id { get; set; }

        [Column(TypeName = "varchar(100)")] public string Name { get; set; }

        public virtual Category Category { get; set; } // Foreign Key of Categories table

        [Column(TypeName = "varchar(500)")] public string Desc { get; set; }

        [Column(TypeName = "varchar(8)")] public string SKU { get; set; }

        public int Quantity { get; set; } 
        
        [Column(TypeName = "decimal(18, 2)")] public decimal Price { get; set; }

        public int Discount { get; set; }

        public DateTime DateCreated { get; set; }

        [Column(TypeName = "varchar(500)")] public string ImageLocation { get; set; }

        #region not included in EF migrations

        [NotMapped]
        public decimal PriceAfterDiscount
        {
            get => Price - (Price * Convert.ToDecimal($"0.{Discount.ToString()}"));
        }

        [NotMapped]
        public bool IsOutOfStock
        {
            get { return Quantity == 0; }
        }

        [NotMapped]
        public bool HasDiscount
        {
            get { return Discount > 0; }
        }

        #endregion
    }
}
