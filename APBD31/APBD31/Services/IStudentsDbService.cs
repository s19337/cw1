using APBD31.DTOs.Requests;
using APBD31.DTOs.Responses;
using APBD31.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APBD31.Services
{
    public interface IStudentsDbService
    {
        Boolean IsStudentExist(String index);
        EnrollStudentResponse EnrollStudent(EnrollStudentRequest request);
        EnrollStudentResponse PromoteStudent(int semester, string studies);
        Student foundStudent(LoginRequest request);
        LoginRequest foundToken(String token);
        void setToken(LoginRequest request, String token);


    }
}
