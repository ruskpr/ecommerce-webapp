using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SleekClothing.Data;
using SleekClothing.Helpers;
using SleekClothing.Models;

namespace SleekClothing.Pages.checkout
{
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public List<Product> Products { get; set; } = new List<Product>();
        public decimal CartTotal { get; private set; }

        public IndexModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public void OnGet()
        {
            // redirect user to login if they are not logged in
            //if (User.Identity.IsAuthenticated == false)
            //{
            //    Response.Redirect("/login");
            //    return;
            //}

            var user = UsersHelper.GetUser(_context, User);
            Products = CartHelper.GetGroupedCartItemsDb(user.Id, _context);

            CartTotal = CartHelper.GetCartTotalDb(user.Id, _context);

        }
    }
}
