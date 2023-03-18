using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SleekClothing.Data;
using SleekClothing.Helpers;
using SleekClothing.Models;

namespace SleekClothing.Pages.cart
{
    public class IndexModel : PageModel
    {
        private readonly SleekClothing.Data.ApplicationDbContext _context;

        public ApplicationDbContext Context { get; set; }

        public string CartTotal { get; set; }

        public IndexModel(SleekClothing.Data.ApplicationDbContext context)
        {
            _context = context;

            var user = UsersHelper.GetUser(_context, this.User);
            CartTotal = CartHelper.GetCartTotalDb(user.Id, _context).ToString("c2");
        }

        public IList<Product> Products { get; set; } = default!;
        

        public async Task OnGetAsync()
        {
            if (_context.Products != null)
                Products = await _context.Products.ToListAsync();
        }

        public IActionResult OnPostAddToCart(int productId)
        {
            Product product = _context.Products.First(x => x.Id == productId);
            CartHelper.AddToCartDb(product, _context, this.User);

            Products = _context.Products.ToList();
            return Page();
        }
    }
}
