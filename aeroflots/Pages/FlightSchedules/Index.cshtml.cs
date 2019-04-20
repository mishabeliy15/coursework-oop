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
    public class IndexModel : PageModel
    {
        private readonly aeroflots.Data.ApplicationDbContext _context;

        public IndexModel(aeroflots.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<FlightSchedule> FlightSchedule { get;set; }

        public async Task OnGetAsync()
        {
            FlightSchedule = await _context.FlightSchedules.ToListAsync();
        }
    }
}
