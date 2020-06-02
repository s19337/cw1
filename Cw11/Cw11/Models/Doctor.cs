using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Cw11.Models
{
    public class Doctor
    {
     public int IdDoctor { get; set; }
     public string FirstName { get; set; }
     public string LastName { get; set; }
     public string Email { get; set; }

     public ICollection<Prescription> Prescriptions { get; set; }
    }
}
