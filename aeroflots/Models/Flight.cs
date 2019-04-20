using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace aeroflots.Models
{
    public class Flight
    {
        public int Id { get; set; }
        [Required]
        public FlightSchedule Schedule { get; set; }
        [Required]
        public DateTime Date { get; set; }
        public int FreeSeats { get; set; }
    }
}
