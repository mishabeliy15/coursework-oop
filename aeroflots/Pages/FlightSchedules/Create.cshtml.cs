using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using aeroflots.Data;
using aeroflots.Models;
using Microsoft.AspNetCore.Authorization;

namespace aeroflots.Pages.FlightSchedules
{
    [Authorize]
    public class CreateModel : PageModel
    {
        private readonly aeroflots.Data.ApplicationDbContext _context;

        public CreateModel(aeroflots.Data.ApplicationDbContext context)
            => _context = context;

        public IActionResult OnGet() => Page();        

        [BindProperty]
        public FlightSchedule FlightSchedule { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
                return Page();

            _context.FlightSchedules.Add(FlightSchedule);
            await _context.SaveChangesAsync();
            return RedirectToPage("./Index");
        }
    }
}