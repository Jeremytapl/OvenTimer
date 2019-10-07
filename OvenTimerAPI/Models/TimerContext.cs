using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace OvenTimerAPI
{
    public class TimerContext : DbContext
    {
        private readonly IConfiguration _config;
        
        public virtual DbSet<TimeSet> TimeSets { get; set; }
        public virtual DbSet<TimeSetDefaults> TimeSetDefaults { get; set; }

        public TimerContext([FromServices] IConfiguration config)
        {
            _config = config;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // SuppressForeignKeyEnforcement allows removal of items by ignoring empty dependencies as foreign keys
            optionsBuilder.UseSqlite(_config.GetConnectionString("DefaultDB"),
                x => x.SuppressForeignKeyEnforcement());
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<TimeSet>().HasKey(t => t.Id);
            builder.Entity<TimeSetDefaults>();

            base.OnModelCreating(builder);
        }
    }
}