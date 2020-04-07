using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.SpaServices.AngularCli;
using medpermapp.api.Data;
using Microsoft.EntityFrameworkCore;

namespace medpermapp.api
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
            services.AddControllers();
            services.AddEntityFrameworkNpgsql().AddDbContext<DataContext>(opt => opt.UseNpgsql(Configuration.GetConnectionString("MyConnectionString")));   
            services.AddCors();
            // services.AddSpaStaticFiles(config => config.RootPath = "wwwroot");
            // services.AddSingleton(typeof(PatientService), new PatientService());
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            // app.UseStaticFiles();
            // app.UseSpaStaticFiles();

            app.UseCors(x => x.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
 
            app.UseHttpsRedirection();
 
            app.UseRouting();
 
            app.UseAuthorization();
 
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
 
            // app.UseWebSockets();
            // app.Use(async (context, next) =>
            // {
            //     if (context.Request.Path == "/ws")
            //     {
            //         if (context.WebSockets.IsWebSocketRequest)
            //         {
            //             var socket = await context.WebSockets.AcceptWebSocketAsync();
            //             var squareService = (PatientService)app.ApplicationServices.GetService(typeof(PatientService));
            //             await squareService.AddUser(socket);
            //             while (socket.State == WebSocketState.Open)
            //             {
            //                 await Task.Delay(TimeSpan.FromMinutes(1));
            //             }
            //         }
            //         else
            //         {
            //             context.Response.StatusCode = 400;
            //         }
            //     }
            //     else
            //     {
            //         await next();
            //     }
            // });
            // app.UseSpa(config =>
            // {
            //     config.Options.SourcePath = "client-app";
            //     if (env.IsDevelopment())
            //     {
            //         config.UseAngularCliServer("start");
            //     }
            // });
        }
    }
}