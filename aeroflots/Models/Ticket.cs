using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace aeroflots.Models
{
    public class Ticket
    {
        public int Id { get; set; }
        public List<Flight> Path { get; set; }
    }
}
