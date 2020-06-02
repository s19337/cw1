using Cw11.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cw11.Services
{
   public interface IDoctorDbService
    {
        public IEnumerable<Doctor> GetDoctors();
        public void AddDoctor(Doctor doctor);
        public void UpdateDoctor(Doctor doctor);
        public void DeleteDoctor(Doctor doctor);
    }
}
