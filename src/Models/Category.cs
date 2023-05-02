using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;

namespace SleekClothing.Models
{
    public class Category
    {
        public int Id { get; set; }
        [Column(TypeName = "varchar(100)")]
        public string Name { get; set; }

    }

    
}
