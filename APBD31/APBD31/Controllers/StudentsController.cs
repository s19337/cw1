using System;
using APBD31.DAL;
using APBD31.Models;
using Microsoft.AspNetCore.Mvc;

namespace APBD31.Controllers
{
    [Route("api/students")]
    [ApiController]
    public class StudentsController : ControllerBase
    {
        private readonly IDbService _dbService;

        public StudentsController(IDbService dbService)
        {
            _dbService = dbService;
        }

        [HttpGet]
        public IActionResult GetStudents(string orderBy)
        {
            
            return Ok(_dbService.GetStudents());
        }

        [HttpGet("{id}")]
        public IActionResult GetStudent(int id)
        {
            if (id == 1)
                return Ok("Jan");

            else return NotFound("Ya");
        }

        [HttpPost]
        public IActionResult CreateStudent(Student student)
        {
            student.IndexNumber = $"s{new Random().Next(1, 20000)}";

            return Ok(student);
        }


        [HttpPut("{id}")]
        public IActionResult PutStudent(int id)
        { 
                return Ok("Ąktualizacja dokończona");
        }



        [HttpDelete("{id}")]
        public IActionResult DeleteStudent(int id)
        {
                return Ok("Usuwanie ukończone");

        }

    }
}