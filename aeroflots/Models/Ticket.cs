using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace aeroflots.Models
{
    public class Ticket
    {
        public int Id { get; set; }
        [ForeignKey("User")]
        public string OwnerId { get; set; }
        public DateTime Date { get; set; }
        public int Seats { get; set; }
        public List<Flight> Path { get; set; }
        [Display(Name = "Cost")]
        public int GetCost() => Path.Sum(x => x.Schedule.Cost);
    }
}
