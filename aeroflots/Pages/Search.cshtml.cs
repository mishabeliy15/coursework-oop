using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using aeroflots.Models;
using aeroflots.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace aeroflots.Pages
{
    public class SearchModel : PageModel
    {
        private readonly Data.ApplicationDbContext _context;
        public SearchModel(Data.ApplicationDbContext context)
            => _context = context;
        public List<Flight> Flight { get; set; }
        public async Task<IActionResult> OnGet(string from, string to, DateTime date)
        {
            if (string.IsNullOrWhiteSpace(from) || string.IsNullOrWhiteSpace(from))
                return Redirect("./Index");
            Path p = new Path(_context, from, to, date);
            await p.SearchDirectFlights();
            ViewData["From"] = p.Flights.Count > 0 ? p.Flights[0].Schedule.Departure : from;
            ViewData["To"] = p.Flights.Count > 0 ? p.Flights[0].Schedule.Arrival : to;
            ViewData["Date"] = date;
            Flight = p.Flights;
            return Page();
        }
        public static string FirstCapitalLetter(string s)
            => ((char)(s[0] - 32)).ToString() + s.Substring(1);
    }
}