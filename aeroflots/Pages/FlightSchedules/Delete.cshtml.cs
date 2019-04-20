using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using aeroflots.Data;
using aeroflots.Models;

namespace aeroflots.Pages.FlightSchedules
{
    public class DeleteModel : PageModel
    {
        private readonly aeroflots.Data.ApplicationDbContext _context;

        public DeleteModel(aeroflots.Data.ApplicationDbContext context)
        => _context = context;

        [BindProperty]
        public FlightSchedule FlightSchedule { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
                return NotFound();
            
            FlightSchedule = await _context.FlightSchedules.FirstOrDefaultAsync(m => m.Id == id);

            if (FlightSchedule == null)
                return NotFound();
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
                return NotFound();

            FlightSchedule = await _context.FlightSchedules.FindAsync(id);

            if (FlightSchedule != null)
            {
                _context.FlightSchedules.Remove(FlightSchedule);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
