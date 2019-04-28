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
        {
            _context = context;
        }

        public IList<Ticket> Ticket { get;set; }

        public async Task OnGetAsync(string from, string to, DateTime date)
        {
            Path t = new Path(_context, from, to, date);
            await t.GetDataFromBD();
            Ticket = await t.SearchDirectTickets();
        }
    }
}
