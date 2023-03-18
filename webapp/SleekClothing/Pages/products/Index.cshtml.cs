using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SleekClothing.Data;
using SleekClothing.Helpers;
using SleekClothing.Models;

namespace SleekClothing.Pages.products
{
    public class IndexModel : PageModel
    {
        private readonly SleekClothing.Data.ApplicationDbContext _context;

        public IndexModel(SleekClothing.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<Product> Products { get; set; }

        public string SearchTerm { get; set; }

        public async Task OnGetAsync()
        {
            if (_context.Products != null)
            {

                Products = await _context.Products.ToListAsync();


            }
        }


        public IActionResult OnPostAddToCart(int productId)
        {
            Product product = _context.Products.First(x => x.Id == productId);

            var u = UsersHelper.GetUser(_context, this.User);
            CartHelper.AddToCartDb(product, _context, User);

            Products = _context.Products.ToList();
            return Page();
        }


    }
}
