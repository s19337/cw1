using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Cw11.Models
{
    public class Patient
    {

        public int IdPatient { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        DateTime Birthdate { get; set; }
        public ICollection<Prescription> Prescriptions { get; set; }
    }
}
