using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APBD31.Middlewares
{
    public class LoggingMiddleware
    {
        private readonly RequestDelegate _next;
        public LoggingMiddleware(RequestDelegate next) { _next = next; }

        public async Task InvokeAsync(HttpContext context)
        {
            context.Request.EnableBuffering();
            string path = context.Request.Path;
            string method = context.Request.Method;
            string queryString = context.Request.QueryString.ToString();
            string bodyStr = "";

            using( var reader = new StreamReader(context.Request.Body, Encoding.UTF8, true, 1024, true))
            {
                bodyStr = await reader.ReadToEndAsync();
                context.Request.Body.Position = 0;
            }

            string logpath = Environment.CurrentDirectory+"\\requestsLog.txt";
            StreamWriter sw = File.AppendText(logpath);

            sw.WriteLine(method);
            sw.WriteLine(path);
            sw.WriteLine(bodyStr);
            sw.WriteLine(queryString);
            sw.Close();

           if(_next!=null) await _next(context);
        }
    }
}
