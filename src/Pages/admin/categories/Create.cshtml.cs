using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using SleekClothing.Data;
using SleekClothing.Models;

namespace SleekClothing.Pages.admin.categories
{
    public class CreateModel : PageModel
    {
        private readonly SleekClothing.Data.ApplicationDbContext _context;
        public CreateModel(SleekClothing.Data.ApplicationDbContext context)
        {
            _context = context;
        }
        public IActionResult OnGet()
        {
            ViewData["ParentCategoryId"] = new SelectList(_context.Categories, "Id", "Name");
            return Page();
        }
        [BindProperty]
        public Category Category { get; set; }
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            _context.Categories.Add(Category);
            await _context.SaveChangesAsync();
            return RedirectToPage("./Index");
        }
    }
}   
