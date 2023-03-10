using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SleekEcommerce.Models;

namespace SleekEcommerce.Pages
{
    public class IndexModel : PageModel
    {
        private readonly SleekEcommerce.Data.SleekEcommerceContext _context;

        public IndexModel(SleekEcommerce.Data.SleekEcommerceContext context)
        {
            _context = context;
        }

        public IList<Product> Products { get; set; } = default!;
        public IList<Product> NewestProducts
        {
            get
            {
                return Products.OrderBy(x => x.DateCreated).Take(8).ToList();
            }
        }


        public async Task OnGetAsync()
        {
            if (_context.Products != null)
            {

                Products = await _context.Products.ToListAsync();

            }
        }
    }
}
