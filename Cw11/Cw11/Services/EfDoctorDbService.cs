using Cw11.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cw11.Services
{
    public class EfDoctorDbService : IDoctorDbService
    {
        private readonly DoctorDbContext _context;
        public EfDoctorDbService(DoctorDbContext context)
        {
            _context = context;
        }



        public void UpdateDoctor(Doctor doctor)
        {
            _context.Attach(doctor);
            _context.Entry(doctor).State = EntityState.Modified;
            _context.SaveChanges();
        }

        public void DeleteDoctor(Doctor doctor)
        {
            _context.Attach(doctor);
            _context.Doctors.Remove(doctor);
            _context.SaveChanges();

        }

        public IEnumerable<Doctor> GetDoctors()
        {
            return _context.Doctors.ToList();
        }

        public void AddDoctor(Doctor doctor)
        {
            _context.Doctors.Add(doctor);
            _context.SaveChanges();
        }
    }
}
