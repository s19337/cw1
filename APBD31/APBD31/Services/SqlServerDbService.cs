using APBD31.DTOs.Requests;
using APBD31.DTOs.Responses;
using APBD31.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Data.SqlClient;

namespace APBD31.Services
{
    public class SqlServerDbService : IStudentsDbService
    {
        private const string ConString = "Data Source=db-mssql;Initial Catalog=s19337;Integrated Security=True";

        public Boolean IsStudentExist(string index)
        {
            using (var con = new SqlConnection(ConString))
            using (var com = new SqlCommand())
            {
                com.Connection = con;
                con.Open();

                com.CommandText = "select * from student where IndexNumber=@index";
                com.Parameters.AddWithValue("index", index);


                var dr = com.ExecuteReader();
                if (!dr.Read())
                {
                    return false;
                }
                else return true;

            }
        }


        public EnrollStudentResponse EnrollStudent(EnrollStudentRequest request)
        {
            using (var con = new SqlConnection(ConString))
            using (var com = new SqlCommand())
            {
                var response = new EnrollStudentResponse();
                response.Name = request.Studies;
                response.Semester = 1;

                com.Connection = con;
                con.Open();
                var tran = con.BeginTransaction();
                com.Transaction = tran;
                try
                {

                    com.CommandText = "select idStudy from studies where name=@name";
                    com.Parameters.AddWithValue("name", request.Studies);


                    var dr = com.ExecuteReader();
                    if (!dr.Read())
                    {
                        dr.Close();
                        tran.Rollback();
                        return null;
                    }

                    int idstudies = (int)dr["idStudy"]; //IdStudies 
                    dr.Close();

                    com.CommandText = "select IdEnrollment, StartDate from enrollment where idStudy=@idStudies and semester=1 order by StartDate desc";
                    com.Parameters.AddWithValue("idStudies", idstudies);

                    int idEnrollment = 10;    //idEnrollment


                    var dr2 = com.ExecuteReader();
                    if (!dr2.Read())
                    {
                        dr2.Close();
                        com.CommandText = "select IdEnrollment from enrollment order by IdEnrollment desc";
                        dr2 = com.ExecuteReader();

                        if (dr2.Read()) idEnrollment = (int)dr2["IdEnrollment"] + 1;
                        dr2.Close();

                        com.CommandText = "INSERT INTO Enrollment(IdEnrollment, Semester, IdStudy, StartDate)" +
                            " VALUES(@IdEnrollment, @Semester, @IdStudy, @StartDate)";

                        com.Parameters.AddWithValue("IdEnrollment", idEnrollment);
                        com.Parameters.AddWithValue("Semester", 1);
                        com.Parameters.AddWithValue("IdStudy", idstudies);
                        com.Parameters.AddWithValue("StartDate", DateTime.Today);
                        com.ExecuteNonQuery();

                        response.StartDate = DateTime.Today;

                    }
                    else
                    {
                        idEnrollment = (int)dr2["IdEnrollment"];
                        response.StartDate = (DateTime)dr2["StartDate"];
                    }
                    dr2.Close();


                    com.CommandText = "select * from student where indexNumber=@IndexNumber";
                    com.Parameters.AddWithValue("IndexNumber", request.IndexNumber);


                    var dr3 = com.ExecuteReader();
                    if (!dr3.Read())
                    {
                        dr3.Close();

                        com.CommandText = "INSERT INTO Student(IndexNumber, FirstName, LastName, BirthDate, IdEnrollment)" +
                                " VALUES(@Index, @Fname, @LastName, @BirthDate, @IdEnrollment)";
                        com.Parameters.AddWithValue("Index", request.IndexNumber);
                        com.Parameters.AddWithValue("FirstName", request.FirstName);
                        com.Parameters.AddWithValue("LastName", request.LastName);
                        com.Parameters.AddWithValue("BirthDate", DateTime.Parse(request.BirthDate));
                        com.Parameters.AddWithValue("IdEnrollment", idEnrollment);
                        com.ExecuteNonQuery();

                    }
                    else
                    {
                        dr3.Close();
                        tran.Rollback();
                        return null;
                    }

                    tran.Commit();
                    return response;

                }
                catch (SqlException exc)
                {
                    tran.Rollback();
                }
            }
            return null;
        }

        public EnrollStudentResponse PromoteStudent(int semester, string studies)
        {
            using (var con = new SqlConnection(ConString))
            using (var com = new SqlCommand())
            {
                var response = new EnrollStudentResponse();
                com.Connection = con;
                con.Open();

                try
                {
                    com.CommandText = "EXEC PromoteStudents @Studies, @Semester;";
                    com.Parameters.AddWithValue("Studies", studies);
                    com.Parameters.AddWithValue("Semester", semester);
                    com.ExecuteNonQuery();

                    response.Name = studies;
                    response.Semester = semester + 1;
                    response.StartDate = DateTime.Today;
                    return response;

                }catch(SqlException exc)
                {
                    return null;
                }
            }
        }
    }
}