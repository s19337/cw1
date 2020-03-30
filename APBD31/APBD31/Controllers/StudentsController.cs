using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using APBD31.DAL;
using APBD31.Models;
using Microsoft.AspNetCore.Mvc;

namespace APBD31.Controllers
{
    [Route("api/students")]
    [ApiController]
    public class StudentsController : ControllerBase
    {
        private const string ConString = "Data Source=db-mssql;Initial Catalog=s19337;Integrated Security=True";
        private readonly IDbService _dbService;


        public StudentsController(IDbService dbService)
        {
            _dbService = dbService;
        }

        [HttpGet]
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
                // com.CommandText = "select * from student where indexNumber=@index";

                com.CommandText = "select name, semester, startdate from Enrollment, Studies, Student " +
                     "where Enrollment.IdStudy = Studies.IdStudy and Student.IdEnrollment = Enrollment.IdEnrollment " +
                    "and indexNumber=@index";

                com.Parameters.AddWithValue("index", indexNumber);               
                con.Open();

                string response = "";
                SqlDataReader dr = com.ExecuteReader();

                if (dr.Read())
                {
                  /*  var st = new Student();
                    st.IndexNumber = dr["IndexNumber"].ToString();
                    st.FirstName = dr["FirstName"].ToString();
                    st.LastName = dr["LastName"].ToString();
                    return Ok(st); */
                    response+="Studies name: "+ dr["Name"].ToString()+
                               "\nSemester: "+ dr["Semester"].ToString()+
                               "\nStart date: " + dr["StartDate"].ToString();
                    return Ok(response);

                }


            }

            return NotFound();
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