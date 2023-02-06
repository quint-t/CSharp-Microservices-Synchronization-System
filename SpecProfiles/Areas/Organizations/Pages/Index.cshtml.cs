using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net.NetworkInformation;
using System.Threading.Tasks;
using System.Xml.Linq;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SpecProfiles.Data;

using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;

namespace SpecProfiles.Areas.Organizations
{
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public IndexModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<Organization> Organization { get; set; } = default!;

        public async Task OnGetAsync(IdentityUser user)
        {
            if (_context.Organization != null)
            {
                string ownerId = User.FindFirstValue("sub");
                Organization = await _context.Organization.Where(org => org.OwnerId == ownerId).ToListAsync();
            }
        }
    }
}
