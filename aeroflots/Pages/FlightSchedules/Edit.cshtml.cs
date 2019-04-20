using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using aeroflots.Data;
using aeroflots.Models;

namespace aeroflots.Pages.FlightSchedules
{
    public class EditModel : PageModel
    {
        private readonly aeroflots.Data.ApplicationDbContext _context;

        public EditModel(aeroflots.Data.ApplicationDbContext context)
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
            ViewData["Days"] = (int) FlightSchedule.Days;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
                return Page();

            _context.Attach(FlightSchedule).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FlightScheduleExists(FlightSchedule.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool FlightScheduleExists(int id)
        => _context.FlightSchedules.Any(e => e.Id == id);
    }
}
