using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APBD31.DTOs.Responses
{
    public class EnrollStudentResponse
    {
        public string Name { get; set; }
        public int Semester { get; set; }
        public DateTime StartDate { get; set; }
    }
}
