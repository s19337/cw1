using APBD31.DAL;
using APBD31.Middlewares;
using APBD31.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

namespace APBD31
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddTransient<IStudentsDbService, SqlServerDbService>();
            services.AddControllers();

            services.AddSwaggerGen(config => 

              config.SwaggerDoc( "v1", new OpenApiInfo {Title="Studemts app API", Version="v1" }));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IStudentsDbService service)
        {
          /*  if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }*/

            app.UseSwagger();
            app.UseSwaggerUI( config =>
            {
                config.SwaggerEndpoint("/swagger/v1/swagger.json", "Studemts api API");
            } );

            app.Use(async(context, next) =>
            {

                if (!context.Request.Headers.ContainsKey("Index"))
                {
                    context.Response.StatusCode = Microsoft.AspNetCore.Http.StatusCodes.Status401Unauthorized;
                    await context.Response.WriteAsync("Nie podano numeru indeksu");
                    return;
                }
                else
                {
                    string index = context.Request.Headers["Index"].ToString();
                    if (!service.IsStudentExist(index))
                    {
                        context.Response.StatusCode = Microsoft.AspNetCore.Http.StatusCodes.Status401Unauthorized;
                        await context.Response.WriteAsync("Student z indeksem "+index+" nie istnieje");
                        return;
                    }
                }
                await next();
            } );
            app.UseMiddleware<LoggingMiddleware>();
            app.UseDeveloperExceptionPage();
            app.UseRouting();

            app.Use(async (context, c) =>
            {
                context.Response.Headers.Add("Secret", "1234");
                await c.Invoke();
            }

            );

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
