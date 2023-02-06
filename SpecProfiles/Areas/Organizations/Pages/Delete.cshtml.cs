using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SpecProfiles.Data;

namespace SpecProfiles.Areas.Organizations
{
    public class DeleteModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public DeleteModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Organization Organization { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.Organization == null)
            {
                return NotFound();
            }

            string ownerId = User.FindFirstValue("sub");
            var organization = await _context.Organization.FirstOrDefaultAsync(m => m.Id == id && m.OwnerId == ownerId);

            if (organization == null)
            {
                return NotFound();
            }
            else
            {
                Organization = organization;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null || _context.Organization == null)
            {
                return NotFound();
            }

            string ownerId = User.FindFirstValue("sub");
            var organization = await _context.Organization.FirstOrDefaultAsync(m => m.Id == id && m.OwnerId == ownerId);
            if (organization != null)
            {
                Organization = organization;
                _context.Organization.Remove(Organization);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
