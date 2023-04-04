using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using SleekClothing.Data;
using SleekClothing.Models;

namespace SleekClothing.Pages.admin.products
{
    public class CreateModel : PageModel
    {
        private readonly SleekClothing.Data.ApplicationDbContext _context;

        public CreateModel(SleekClothing.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public List<Category> Categories { get; set; }

        public IActionResult OnGet()
        {
            Categories = _context.Categories.GroupBy(x => x.Name).Select(x => x.First()).ToList();

            return Page();
        }

        [BindProperty]
        public Product Product { get; set; }
        
        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            Categories = _context.Categories.GroupBy(x => x.Name).Select(x => x.First()).ToList();

            
            Product.Category = Categories[0];

            if (!ModelState.IsValid)
            {
                TempData["error"] = "Failed to submit.";
                return Page();
            }

            _context.Products.Add(Product);
            await _context.SaveChangesAsync();

            return Page();
            return RedirectToPage("./Index");
        }
    }
}
