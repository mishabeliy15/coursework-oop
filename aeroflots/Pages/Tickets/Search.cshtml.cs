using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using aeroflots.Data;
using aeroflots.Models;
using aeroflots.Services;

namespace aeroflots.Pages.Tickets
{
    public class SearchModel : PageModel
    {
        private readonly aeroflots.Data.ApplicationDbContext _context;

        public SearchModel(aeroflots.Data.ApplicationDbContext context)
            => _context = context;

        [BindProperty]
        public List<Ticket> Ticket { get;set; }

        public async Task OnGetAsync(string from, string to, DateTime date, int mintransfer)
        {
            Path t = new Path(_context, from, to, date, mintransfer);
            await t.GetDataFromBD();
            Ticket = await t.SearchTickets();
            _context.Tickets.AddRange(Ticket);
            await _context.SaveChangesAsync();
            ViewData["Date"] = date.ToLongDateString();
            if (Ticket.Count == 0) ViewData["FromTo"] = $"{from} - {to}";
        }
    }
}
