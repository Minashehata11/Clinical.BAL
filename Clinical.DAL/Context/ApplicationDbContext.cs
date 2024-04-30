using Clinical.DAL.Entities;
using Clinical.DAL.Entities.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Conventions.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace Clinical.DAL.Context
{
    public class ApplicationDbContext:IdentityDbContext<ApplicationUser,ApplicationRole,string>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options):base(options)
        {
                
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<ApplicationUser>().Property(e => e.Gender)
           .HasConversion<string>();
            builder.Entity<Appointment>().HasKey(x => new { x.AppointemntId, x.DateReserve });
            builder.Entity<Appointment>()
           .Property(p => p.AppointemntId)
           .ValueGeneratedOnAdd(); // Let the database generate values

        }

        public DbSet<Doctor> Doctors { get; set; }
        public DbSet<Patient> Patients { get; set; }

        public DbSet<Appointment> Appointments { get; set; }

        

    }
}
