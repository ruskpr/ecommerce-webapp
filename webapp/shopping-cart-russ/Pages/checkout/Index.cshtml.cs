using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
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

        [BindProperty]
        public Order Order { get; set; }

        public decimal CartTotalAfterGst { 
            get
            {
                return CartTotal * 1.05m;
            }
        }

        public IndexModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public void OnGet()
        {
            //redirect user to login if they are not logged in
            if (User.Identity.IsAuthenticated == false)
            {
                Response.Redirect("/Identity/Account/Login");
                return;
            }

            var user = UsersHelper.GetUser(_context, User);
            Products = CartHelper.GetGroupedCartItemsDb(user.Id, _context);

            CartTotal = CartHelper.GetCartTotalDb(user.Id, _context);

        }

        public async Task<IActionResult> OnPostCheckout()
        {
            var user = UsersHelper.GetUser(_context, User);
            Products = CartHelper.GetGroupedCartItemsDb(user.Id, _context);
            CartTotal = CartHelper.GetCartTotalDb(user.Id, _context);

            if (!ModelState.IsValid)
            {
                TempData["error"] = "Failed to submit.";
                return Page();
            }

            Order.Email = user.Email;
            Order.ProductDataAsJson = JsonConvert.SerializeObject(Products);
            Order.DateOrdered = DateTime.UtcNow;
            _context.Orders.Add(Order);
            await _context.SaveChangesAsync();
            ////clear cart
            CartHelper.ClearCartDb(User, _context);
            TempData["success"] = "Your order has been placed!";
            return Redirect("/myorders");
        }

    }
}
