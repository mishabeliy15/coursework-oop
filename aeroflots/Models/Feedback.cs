using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace aeroflots.Models
{
    public class Feedback
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Comment { get; set; }
        public DateTime Date { get; set; }
    }
}
