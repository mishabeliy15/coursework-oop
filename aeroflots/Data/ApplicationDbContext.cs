﻿using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using aeroflots.Models;

namespace aeroflots.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public DbSet<FlightSchedule> FlightSchedules { get; set; }
        public DbSet<Flight> Flights { get; set; }
        public DbSet<Ticket> Tickets { get; set; }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<aeroflots.Models.Feedback> Feedback { get; set; }
    }
}
