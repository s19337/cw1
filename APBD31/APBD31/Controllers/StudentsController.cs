using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using APBD31.DTOs.Requests;
using APBD31.Models;
using APBD31.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace APBD31.Controllers
{
    [Route("api/students")]
    [ApiController]
    public class StudentsController : ControllerBase
    {
        private const string ConString = "Data Source=db-mssql;Initial Catalog=s19337;Integrated Security=True";
        public IConfiguration Configuration { get; set; }
        private IStudentsDbService _service;
        public StudentsController(IConfiguration configuration, IStudentsDbService service)
        {
            Configuration = configuration;
            _service = service;
        }

        [HttpGet("refresh")]
        public IActionResult Refresh()
        {
            var token = " ";

            var request = _service.foundToken(token);
            if (request == null) return NotFound();

            return Login(request);
        }



        [HttpPost]
        public IActionResult Login(LoginRequest request)
        {
            var st = _service.foundStudent(request);
            if (st == null) return NotFound();

            var claims = new[]
{
                new Claim(ClaimTypes.NameIdentifier, st.IndexNumber),
                new Claim(ClaimTypes.Name, st.FirstName),
                new Claim(ClaimTypes.Role, "admin"),
                new Claim(ClaimTypes.Role, "student")
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["SecretKey"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken
            (
                issuer: "Gakko",
                audience: "Students",
                claims: claims,
                expires: DateTime.Now.AddMinutes(1),
                signingCredentials: creds
            );

             var refreshTokenn = Guid.NewGuid();
            _service.setToken(request, refreshTokenn.ToString());
            

            return Ok(new
            {
                token = new JwtSecurityTokenHandler().WriteToken(token),
                refreshToken = refreshTokenn
        });
        }


        [HttpGet]
        [Authorize]
        public IActionResult GetStudents()
        {

              var list = new List<Student>();

               using (SqlConnection con = new SqlConnection(ConString))
               using ( SqlCommand com = new SqlCommand())
               {
                   com.Connection = con;
                   com.CommandText = "select * from student";

                   con.Open();


                   SqlDataReader dr = com.ExecuteReader();

                   while (dr.Read())
                   {
                       var st = new Student();
                       st.IndexNumber = dr["IndexNumber"].ToString();
                       st.FirstName = dr["FirstName"].ToString();
                       st.LastName = dr["LastName"].ToString();
                       st.BirthDate = dr["BirthDate"].ToString();
                       list.Add(st);
                   }
               }
               return Ok(list);
  
        }



        [HttpGet("{indexNumber}")]
        public IActionResult GetStudent(string indexNumber)
        {
            using (SqlConnection con = new SqlConnection(ConString))
            using (SqlCommand com = new SqlCommand())
            {
                com.Connection = con;

                com.CommandText = "select name, semester, startdate from Enrollment, Studies, Student " +
                     "where Enrollment.IdStudy = Studies.IdStudy and Student.IdEnrollment = Enrollment.IdEnrollment " +
                    "and indexNumber=@index";

                com.Parameters.AddWithValue("index", indexNumber);               
                con.Open();

                string response = "";
                SqlDataReader dr = com.ExecuteReader();

                if (dr.Read())
                {

                    response+="Studies name: "+ dr["Name"].ToString()+
                               "\nSemester: "+ dr["Semester"].ToString()+
                               "\nStart date: " + dr["StartDate"].ToString();
                    return Ok(response);

                }


            }

            return NotFound();
        }

       /* [HttpPost]
        public IActionResult CreateStudent(Student student)
        {
            student.IndexNumber = $"s{new Random().Next(1, 20000)}";

            return Ok(student);
        }*/


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