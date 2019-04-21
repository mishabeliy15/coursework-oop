using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using aeroflots.Models;
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
                return BadRequest();
            (string a, string b) = (from.ToLower(), to.ToLower());
            Day day = ConvertToFlag((int)date.DayOfWeek);
            List<FlightSchedule> FlightSchedule = (await _context.FlightSchedules.ToListAsync()).
                Where(x => x.Available && x.Days.HasFlag(day) && x.Departure.ToLower() == a && x.Arrival.ToLower() == b).ToList();
            List<Flight> flights = (await _context.Flights.ToListAsync()).
                Where(x => x.Date.Equals(date.Date) && FlightSchedule.Any(t => t.Id == x.Schedule.Id)).ToList();
            List<Flight> mustadd = new List<Flight>();
            foreach (var i in FlightSchedule)
                if (!flights.Any(t => t.Schedule.Id == i.Id))
                {
                    Flight temp = new Flight();
                    temp.Date = date;
                    temp.Schedule = i;
                    temp.FreeSeats = i.Seats;
                    mustadd.Add(temp);
                }
            if (mustadd.Count > 0)
            {
                _context.Flights.AddRange(mustadd);
                await _context.SaveChangesAsync();
                flights.AddRange(mustadd);
            }
            ViewData["From"] = flights.Count > 0 ? flights[0].Schedule.Departure : FirstCapitalLetter(a);
            ViewData["To"] = flights.Count > 0 ? flights[0].Schedule.Arrival : FirstCapitalLetter(b);
            ViewData["Date"] = date;
            Flight = flights;
            return Page();
        }
        public static string FirstCapitalLetter(string s)
          => (s[0] - 32).ToString() + s.Substring(1);
        public static Day ConvertToFlag(int day)
            => (Day)(day > 0 ? Math.Pow(2, --day) : 64);
    }
}