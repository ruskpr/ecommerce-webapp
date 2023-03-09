using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SleekEcommerce.Data;
using SleekEcommerce.Models;

namespace SleekEcommerce.Pages.products
{
    public class DetailsModel : PageModel
    {
        private readonly SleekEcommerce.Data.SleekEcommerceContext _context;

        public DetailsModel(SleekEcommerce.Data.SleekEcommerceContext context)
        {
            _context = context;
        }

      public Product Product { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.Products == null)
            {
                return NotFound();
            }

            var product = await _context.Products.FirstOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }
            else 
            {
                Product = product;
            }
            return Page();
        }
    }
}
