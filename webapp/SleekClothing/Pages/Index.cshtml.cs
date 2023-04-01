using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SleekClothing.Helpers;
using SleekClothing.Models;

namespace SleekClothing.Pages
{
    public class IndexModel : PageModel
    {
        private readonly SleekClothing.Data.ApplicationDbContext _context;

        public IndexModel(SleekClothing.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public List<Product> Products { get; set; } = default!;


        public async Task OnGetAsync()
        {
            if (_context.Products != null)
            {
                Products = await _context.Products.ToListAsync(); // get from db
                Products = Products.OrderByDescending(x => x.DateCreated).Take(8).ToList(); // order by newest
            }

        }

        public IActionResult OnPostAddToCart(int productId)
        {
            Products = _context.Products.ToList();
            Products = Products.OrderBy(x => x.DateCreated).Take(8).ToList();

            Product product = _context.Products.First(x => x.Id == productId);

            //handle product out of stock

            if (product.IsOutOfStock) return Redirect("/");
            if (!User.Identity.IsAuthenticated)
            {
                CartHelper.AddToCartCookie(product, HttpContext);
            }
            else
            {
                CartHelper.AddToCartDb(product, _context, this.User);
            }

            return Redirect("/");
        }

        public IActionResult OnPostAddToWishlist(int productId)
        {
            if (!User.Identity.IsAuthenticated)
                return Redirect("/Identity/Account/Login");

            Product product = _context.Products.First(x => x.Id == productId);
            WishlistHelper.AddToWishlist(product, _context, this.User);

            Products = _context.Products.ToList();
            Products = Products.OrderBy(x => x.DateCreated).Take(8).ToList();
            return Redirect("/");
        }
    }
}
