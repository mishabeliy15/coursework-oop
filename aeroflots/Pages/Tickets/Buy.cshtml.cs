using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using aeroflots.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace aeroflots.Pages.Tickets
{
    [Authorize]
    public class BuyModel : PageModel
    {
        private readonly aeroflots.Data.ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public BuyModel(aeroflots.Data.ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [BindProperty]
        public Ticket Ticket { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null) return NotFound();
            Ticket = await _context.Tickets.Include(t => t.Path).FirstOrDefaultAsync(m => m.Id == id);
            if (Ticket == null) return NotFound();
            int min = Ticket.Path.Min(p => p.FreeSeats);
            if (min <= 0)
                return RedirectToPage("../Index");
            ViewData["MaxTickets"] = min;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            Ticket order = await _context.Tickets.Include(t => t.Path).FirstOrDefaultAsync(m => m.Id == Ticket.Id);
            if (Ticket == null) return NotFound();
            string user_id = _userManager.GetUserId(User);
            order.OwnerId = user_id;
            order.Seats = Ticket.Seats;
            order.Purchased = true;
            Ticket = order;
            bool overseats = Ticket.Path.Any(x => x.FreeSeats < Ticket.Seats);
            if (!ModelState.IsValid || overseats)
                return Page();

            foreach (var t in Ticket.Path)
                t.FreeSeats -= Ticket.Seats;

            _context.UpdateRange(Ticket.Path);
            _context.Attach(order).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TicketExists(Ticket.Id))
                    return NotFound();
                else
                    throw;
            }

            return RedirectToPage("./MyTickets");
        }

        private bool TicketExists(int id) =>
            _context.Tickets.Any(e => e.Id == id);
    }
}
