using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using aeroflots.Models;
using Npgsql;

namespace aeroflots.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public DbSet<FlightSchedule> FlightSchedules { get; set; }
        public DbSet<Flight> Flights { get; set; }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
    }
}
