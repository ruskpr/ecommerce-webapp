using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SleekClothing.Data;
using SleekClothing.Helpers;
using SleekClothing.Models;

namespace SleekClothing.Pages.wishlist
{
    public class IndexModel : PageModel
    {
        private readonly SleekClothing.Data.ApplicationDbContext _context;

        public ApplicationDbContext Context { get; set; }

        public string CartTotal { get; set; }

        public IndexModel(SleekClothing.Data.ApplicationDbContext context)
        {
            _context = context;

        }

        public IList<Product> ProductWishlist { get; set; } = default!;
        

        public async Task OnGetAsync()
        {
            var user = UsersHelper.GetUser(_context, this.User);
            CartTotal = CartHelper.GetCartTotalDb(user.Id, _context).ToString("c2");


            //if (_context.Products != null)
            //    Products = await _context.Products.ToListAsync();

            if (_context != null)
            {
                ProductWishlist = await Task.Run(() => CartHelper.GetGroupedCartItemsDb(user.Id, _context));
            }

        }

        public IActionResult OnPostAddToCart(int productId)
        {
            Product product = _context.Products.First(x => x.Id == productId);
            CartHelper.AddToCartDb(product, _context, this.User);

            //Products = _context.Products.ToList();
            return Redirect("/cart");
        }

        public IActionResult OnPostRemoveFromCart(int productId)
        {
            Product product = _context.Products.First(x => x.Id == productId);
            CartHelper.RemoveFromCartDb(product, _context, this.User);

            //Products = _context.Products.ToList();
            return Redirect("/cart");
            return Page();

        }
    }
}
