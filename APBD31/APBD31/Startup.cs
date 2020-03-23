using APBD31.DAL;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;


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
            services.AddTransient<IDbService, MockDbService>();
            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
          //  if (env.IsDevelopment())
          //  {
          //      app.UseDeveloperExceptionPage();
          //  }

           // app.UseDeveloperExceptionPage();
           // app.UseHttpsRedirection();

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
