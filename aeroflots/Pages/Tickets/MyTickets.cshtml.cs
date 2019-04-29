using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using aeroflots.Data;
using aeroflots.Models;
using Microsoft.AspNetCore.Identity;

namespace aeroflots.Pages.Tickets
{
    public class MyTicketsModel : PageModel
    {
        private readonly aeroflots.Data.ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public MyTicketsModel(aeroflots.Data.ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public IList<Ticket> Ticket { get;set; }

        public async Task OnGetAsync()
        {
            string userid = _userManager.GetUserId(User);
            Ticket = (await _context.Tickets.Include(t => t.Path).
                    ThenInclude(t => t.Schedule).
                ToListAsync()).Where(x =>x.Purchased && x.OwnerId == userid).ToList();
        }
    }
}
