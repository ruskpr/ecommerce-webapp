using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SleekClothing.Data;
using SleekClothing.Models;

namespace shopping_cart_russ.Pages.myorders
{
    public class IndexModel : PageModel
    {
        private readonly SleekClothing.Data.ApplicationDbContext _context;

        public IndexModel(SleekClothing.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<Order> Orders { get;set; } = new List<Order>();

        public async Task OnGetAsync()
        {
            if (!User.Identity.IsAuthenticated) return;

            if (_context.Orders != null)
            {
                Orders = await _context.Orders.OrderByDescending(o => o.DateOrdered).Where(o => o.Email == User.Identity.Name).ToListAsync();
            }
        }
    }
}
