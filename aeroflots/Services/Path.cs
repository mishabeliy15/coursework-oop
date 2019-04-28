using aeroflots.Models;
using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace aeroflots.Services
{
    public class Path
    {
        public string From { get; private set; }
        public string To { get; private set; }
        public DateTime Date { get; private set; }
        public Day DayFlag
        {
            get => (Day)(Date.DayOfWeek > 0 ? Math.Pow(2, (int)Date.DayOfWeek - 1) : 64);
        }
        private readonly Data.ApplicationDbContext _db;
        public List<FlightSchedule> FlightPlans { get; private set; }
        public List<Flight> Flights { get; private set; }
        public Path(Data.ApplicationDbContext db) => _db = db;
        public Path(Data.ApplicationDbContext db, string from, string to, DateTime date) : this(db)
            => (From, To, Date) = (from, to, date);
        public async Task GetDataFromBD()
        {
            FlightPlans = await _db.FlightSchedules.ToListAsync();
            Flights = await _db.Flights.ToListAsync();
        }
        public async Task<List<Ticket>> SearchDirectTickets()
        {
            (string a, string b) = (From.ToLower(), To.ToLower());
            List<FlightSchedule> flp = FlightPlans.Where(x =>
                x.Available &&
                x.Days.HasFlag(DayFlag) &&
                x.Departure.ToLower() == a &&
                x.Arrival.ToLower() == b).ToList();
            List<Flight> fl = Flights.Where(x =>
                x.Date.Equals(Date.Date) &&
                flp.Any(t => t.Id == x.Schedule.Id)).ToList();
            List<Flight> mustadd = new List<Flight>();
            foreach (var i in flp)
                if (!fl.Any(t => t.Schedule.Id == i.Id))
                {
                    Flight temp = new Flight() {
                        Date = Date,
                        Schedule = i,
                        FreeSeats = i.Seats
                    };
                    mustadd.Add(temp);
                }
            if (mustadd.Count > 0)
            {
                _db.Flights.AddRange(mustadd);
                await _db.SaveChangesAsync();
                fl.AddRange(mustadd);
            }
            return fl.Where(_ => _.FreeSeats > 0).Select(x => new Ticket()
            {
                Date = x.Date,
                Path = new List<Flight>() { x }
            }).ToList<Ticket>();
        }
        
        public List<Ticket> SearchTickets()
        {
            List<Ticket> tickets = new List<Ticket>();

            return tickets;
        }
    }
}
