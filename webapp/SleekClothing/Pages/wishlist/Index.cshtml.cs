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

        public IList<Product> ProductWishlist { get; set; } = new List<Product>();


        public async Task OnGetAsync()
        {
            if (User.Identity.IsAuthenticated)
            {
                var user = UsersHelper.GetUser(_context, this.User);
                CartTotal = CartHelper.GetCartTotalDb(user.Id, _context).ToString("c2");

                if (_context != null)
                {
                    ProductWishlist = await Task.Run(() => WishlistHelper.GetUserWishlist(User, _context));
                }
            }
               
        }

        public IActionResult OnPostAddToCart(int productId)
        {
            Product product = _context.Products.First(x => x.Id == productId);
            CartHelper.AddToCartDb(product, _context, this.User);

            // remove it from wishlist
            WishlistHelper.RemoveFromWishlist(product, _context, this.User);

            ProductWishlist = WishlistHelper.GetUserWishlist(User, _context);

            return Page();
        }

        public IActionResult OnPostRemoveFromWishlist(int productId)
        {
            Product product = _context.Products.First(x => x.Id == productId);
            WishlistHelper.RemoveFromWishlist(product, _context, this.User);

            ProductWishlist = WishlistHelper.GetUserWishlist(User, _context);
            return Page();
        }
    }
}
