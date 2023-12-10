using AlgorizaProject.DAL.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;


namespace AlgorizaProject.DAL.DbContext
{
    public class VezeetaDbContext : IdentityDbContext<AppUser>
    {
        public VezeetaDbContext(DbContextOptions<VezeetaDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
         
            base.OnModelCreating(builder);

            builder.Entity<Doctor>().ToTable("Doctors");
            builder.Entity<Patient>().ToTable("Patients");

        }

        public DbSet<Appoitment> Appoitments { get; set; }
        public DbSet<Doctor> Doctors { get; set; }
        public DbSet<Request> Requests { get; set; }
        public DbSet<Discount> Discounts { get; set; }
        public DbSet<AppoitmentTime> AppoitmentTimes { get; set; }
        public DbSet<Specialization> Specializations { get; set; }
        public DbSet<Patient> Patients { get; set; }
    }
}
