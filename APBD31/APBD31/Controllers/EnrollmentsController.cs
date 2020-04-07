using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using APBD31.DTOs.Requests;
using APBD31.DTOs.Responses;
using APBD31.Models;
using APBD31.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace APBD31.Controllers
{
    [Route("api/enrollments")]
    [ApiController]
    public class EnrollmentsController : ControllerBase
    {
        private IStudentsDbService _service;

        public EnrollmentsController(IStudentsDbService service)
        {
            _service = service;
        }

        [HttpPost]
        public IActionResult EnrollStudent(EnrollStudentRequest request)
        {
            var response = new EnrollStudentResponse();
            response = _service.EnrollStudent(request);

            if (response != null)
                return Ok(response);

            else return BadRequest("");
        }

        [HttpPost("promotions")]
        public IActionResult PromoteStudent(int semester, string studies)
        {
            var response = new EnrollStudentResponse();
            response = _service.PromoteStudent(semester, studies);

            if (response != null)
                return Ok(response);

            else return BadRequest();
        }

    }
}