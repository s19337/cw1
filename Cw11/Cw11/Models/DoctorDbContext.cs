using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cw11.Models
{
    public class DoctorDbContext : DbContext
    {
        public DbSet<Medicament> Medicaments { get; set; }
        public DbSet<Doctor> Doctors { get; set; }
        public DbSet<Patient> Patients { get; set; }
        public DbSet<Prescription> Prescriptions { get; set; }
        public DbSet<Prescription_Medicament> Prescription_Medicaments { get; set; }



        public DoctorDbContext() { }

        public DoctorDbContext(DbContextOptions option) : base(option)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Doctor>().HasKey(e => e.IdDoctor);
            modelBuilder.Entity<Medicament>().HasKey(e => e.IdMedicament);
            modelBuilder.Entity<Patient>().HasKey(e => e.IdPatient);
            modelBuilder.Entity<Prescription>().HasKey(e => e.IdPrescription);
            modelBuilder.Entity<Prescription_Medicament>().HasKey(e => new {e.IdMedicament , e.IdPrescription });

            var doctorsList = new List<Doctor>();
            doctorsList.Add(new Doctor { IdDoctor = 1, FirstName ="Anna", LastName = "Kostrzewska", Email = "akostrzew@onet.pl"});
            doctorsList.Add(new Doctor { IdDoctor = 2, FirstName = "Andrzej", LastName = "Malewski", Email = "malewski@wp.pl"});
            doctorsList.Add(new Doctor { IdDoctor = 3, FirstName = "Marcin", LastName = "Malędowski", Email = "moleda@wp.pl"});

            modelBuilder.Entity<Doctor>().HasData(doctorsList);
            }
    }
}
