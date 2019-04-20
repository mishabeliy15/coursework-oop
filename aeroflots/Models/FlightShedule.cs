using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace aeroflots.Models
{
    public enum BookingClass
    {
        [Display(Name = "First Class")]
        First,
        Business,
        Premium,
        Economy
    }
    [Flags]
    public enum Day
    {
        Monday = 1,
        Tuesday = 2,
        Wednesday = 4,
        Thursday = 8,
        Friday = 16,
        Saturday = 32,
        Sunday = 64
    }
    public class FlightSchedule
    {
        public int Id { get; set; }
        [Required]
        public BookingClass Class { get;set; }
        public Day Days { get; set; }
        [Required]
        public string Departure { get; set; }
        public TimeSpan DepartureTime { get; set; }
        [Required]
        public string Arrival { get; set; }
        public TimeSpan ArrivalTime { get; set; }
        [Required]
        public int Seats { get; set; }
        public bool Available { get; set; }
        public int Cost { get; set; }
    }
}
