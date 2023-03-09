using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SleekEcommerce.Data;
using SleekEcommerce.Models;

namespace SleekEcommerce.Pages.categories
{
    public class DetailsModel : PageModel
    {
        private readonly SleekEcommerce.Data.SleekEcommerceContext _context;

        public DetailsModel(SleekEcommerce.Data.SleekEcommerceContext context)
        {
            _context = context;
        }

      public Category Category { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.Categories == null)
            {
                return NotFound();
            }

            var category = await _context.Categories.FirstOrDefaultAsync(m => m.Id == id);
            if (category == null)
            {
                return NotFound();
            }
            else 
            {
                Category = category;
            }
            return Page();
        }
    }
}
