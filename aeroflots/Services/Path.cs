using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using aeroflots.Models;

namespace aeroflots.Services
{
    public class Path
    {
        public string From { get; private set; }
        public string To { get; private set; }
        public DateTime Date { get; private set; }
        public Day DayFlag
        {
            get => DateToDay(Date);
        }
        public Day DateToDay(DateTime date)
            => (Day)(date.DayOfWeek > 0 ? Math.Pow(2, (int)date.DayOfWeek - 1) : 64);
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
            List<Flight> fl = (await GetRaiceOnDay(Date)).Where(x => 
                x.Schedule.Departure.ToLower() == a &&
                x.Schedule.Arrival.ToLower() == b).ToList();
            return fl.Select(x => new Ticket()
            {
                Date = x.Date,
                Purchased = false,
                Path = new List<Flight>(){ x }
            }).ToList();
        }
        
        public async Task<List<Ticket>> SearchTransitiveTickets()
        {
            List<Ticket> tickets = new List<Ticket>();
            DateTime date = Date;
            List<Flight> fl = new List<Flight>();
            for(int i = 0;i < 2; i++)
            {
                fl.AddRange(await GetRaiceOnDay(date));
                date = date.AddDays(1);
            }
            var cities = fl.GroupBy(g => g.Schedule.Departure).
                Select(x => new
                {
                    Departure = x.Key,
                    Counts = x.Count(),
                    Edges = x.Select(p => p).ToList()
                }).ToList();
            (string a, string b) = (From.ToLower(), To.ToLower());
            int s = cities.FindIndex(x => x.Departure.ToLower() == a);
            int dest = cities.FindIndex(x => x.Departure.ToLower() == b);
            if (s != -1)
            {
                int n = cities.Count;
                long[] d = new long[n];
                FlInd[] p = new FlInd[n];
                Array.Fill(d, long.MaxValue);
                Array.Fill(p, new FlInd());
                d[s] = 0;
                bool[] u = new bool[n];
                Array.Fill(u, false);
                FlInd t_last = new FlInd();
                for(int i = 0;i < n && t_last.Idx == -1; i++)
                {
                    int v = -1;
                    for (int j = 0; j < n; ++j)
                        if (!u[j] && (v == -1 || d[j] < d[v]))
                            v = j;
                    if (d[v] == long.MaxValue)
                        break;
                    u[v] = true;
                    for (int j = 0; j < cities[v].Counts; ++j)
                    {
                        int to = cities.FindIndex(temp => temp.Departure == cities[v].Edges[j].Schedule.Arrival);
                        long deptime = cities[v].Edges[j].Date.ToFileTimeUtc() + (long)cities[v].Edges[j].Schedule.DepartureTime.TotalMilliseconds * 6,
                            arrtime = cities[v].Edges[j].Date.ToFileTimeUtc() + (long)cities[v].Edges[j].Schedule.ArrivalTime.TotalMilliseconds * 6;
                        if (arrtime < deptime) arrtime += 864000000000; // 1 day in UTC
                        if (to == -1)
                            if (cities[v].Edges[j].Schedule.Arrival.ToLower() == b &&
                                arrtime > d[v])
                            {
                                t_last = new FlInd(cities[v].Edges[j], v);
                                break;
                            }
                        else
                            continue;
                        if (d[v] < deptime && arrtime < d[to])
                        {
                            d[to] = arrtime;
                            p[to] = new FlInd(cities[v].Edges[j], v);
                        }
                    }
                }
                Ticket t = new Ticket();
                t.Path = new List<Flight>();
                if (dest == -1)
                    if (t_last.Idx != -1)
                    {
                        dest = t_last.Idx;
                        t.Path.Add(t_last.Fl);
                    }
                    else return tickets;
                for (int v = dest; v != -1 && v != s; v = p[v].Idx)
                    t.Path.Add(p[v].Fl);
                t.Path.Reverse();
                t.Date = t.Path[0].Date;
                t.Purchased = false;
                tickets.Add(t);
            }
            return tickets;
        }

        public async Task<List<Ticket>> SearchTickets()
        {
            List<Ticket> tickets = await SearchDirectTickets();
            if (tickets.Count < 1) tickets = await SearchTransitiveTickets();
            return tickets;
        }
        public async Task<List<Flight>> GetRaiceOnDay(DateTime date)
        {
            List<FlightSchedule> flp = FlightPlans.Where(x =>
                x.Available && x.Days.HasFlag(DateToDay(date))).ToList();
            List<Flight> fl = Flights.Where(x =>
                x.Date.Equals(Date.Date) && flp.Any(t => t.Id == x.Schedule.Id)).ToList();
            List<Flight> mustadd = new List<Flight>();
            foreach (var i in flp)
                if (!fl.Any(t => t.Schedule.Id == i.Id))
                {
                    Flight temp = new Flight()
                    {
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
                Flights.AddRange(mustadd);
            }
            return fl.Where(x => x.FreeSeats > 0).ToList();
        }
    }
}

class FlInd
{
    public Flight Fl { get; set; }
    public int Idx { get; set; } = -1;
    public FlInd() { }
    public FlInd(Flight f, int i)
    {
        Fl = f;
        Idx = i;
    }
}