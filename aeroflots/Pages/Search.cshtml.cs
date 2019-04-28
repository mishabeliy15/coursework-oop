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
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace aeroflots.Pages
{
    public class SearchModel : PageModel
    {
        private readonly Data.ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        //public SearchModel(Data.ApplicationDbContext context) => _context = context;
        public SearchModel(Data.ApplicationDbContext context, UserManager<IdentityUser> userManager)
            => (_context, _userManager) = (context, userManager);
        public List<Flight> Flight { get; set; }        
        public async Task<IActionResult> OnGet(string from, string to, DateTime date)
        {
            /*string id = _userManager.GetUserId(User);
            Ticket t = new Ticket() { OwnerId = id, Date = date, Seats = 2 };
            _context.Tickets.Add(t);
            _context.SaveChanges();*/
            if (string.IsNullOrWhiteSpace(from) || string.IsNullOrWhiteSpace(from))
                return Redirect("./Index");
            Path p = new Path(_context, from, to, date);
            //await p.SearchDirectFlights();
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