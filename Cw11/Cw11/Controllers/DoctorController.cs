using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cw11.Models;
using Cw11.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Cw11.Controllers
{
    [Route("api/doctors")]
    [ApiController]
    public class DoctorController : ControllerBase
    {
        private readonly IDoctorDbService _service;
        public DoctorController(IDoctorDbService service)
        {
            _service = service;
        }

        [HttpGet]
        public IActionResult GetDoctors()
        {
            return Ok(_service.GetDoctors());
        }

        [Route("add")]
        [HttpPost]
        public IActionResult AddDoctor(Doctor doctor)
        {
            _service.AddDoctor(doctor);
            return Ok();
        }


        [Route("update")]
        [HttpPost]
        public IActionResult UpdateDoctors(Doctor doctor)
        {
            _service.UpdateDoctor(doctor);
            return Ok();
        }

        [Route("delete")]
        [HttpPost]
        public IActionResult DeleteDoctor(Doctor doctor)
        {
            _service.DeleteDoctor(doctor);
            return Ok();
        }


    }
}
