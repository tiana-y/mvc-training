using BulkyWebRazor.Data;
using BulkyWebRazor.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace BulkyWebRazor.Pages.Categories
{
    public class DeleteModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        [BindProperty]
        public Category Category { get; set; }

        public DeleteModel(ApplicationDbContext context)
        {
            _context = context;
        }
        public void OnGet(int? id)
        {
            if (id is not null && id != 0)
            {
                Category = _context.Categories.Find(id);
            }
        }

        public IActionResult OnPost()
        {
            if (ModelState.IsValid)
            {
                _context.Categories.Remove(Category);
                _context.SaveChanges();
                TempData["success"] = "Category deleted successfully";
                return RedirectToPage("Index");
            }
            return Page();
        }
    }
}
