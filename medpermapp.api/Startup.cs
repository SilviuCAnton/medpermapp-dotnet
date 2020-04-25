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
using Microsoft.AspNetCore.SpaServices.AngularCli;
using medpermapp.api.Data;
using Microsoft.EntityFrameworkCore;
using System.Net.WebSockets;
using medpermapp.api.Services;
using System.Threading;
using Thrift.Server;
using Thrift.Transport;

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
            services.AddSpaStaticFiles(config => config.RootPath = "/./wwwroot");
            // services.AddSingleton<PatientService>();
            services.AddSingleton<PatientHandler>();
        }

        private void RunServer(PatientServiceThrift.Processor processor) 
        {
            TServerTransport serverTransport = new TServerSocket(9090);
            TServer server = new TSimpleServer(processor, serverTransport);
            System.Console.WriteLine("Started server...");
            server.Serve();
            server.Stop();
        }
        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseStaticFiles();
            app.UseSpaStaticFiles();

            app.UseCors(x => x.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
 
            app.UseHttpsRedirection();
 
            app.UseRouting();
 
            app.UseAuthorization();
 
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });


            PatientHandler handler = (PatientHandler)app.ApplicationServices.GetService(typeof(PatientHandler));
            PatientServiceThrift.Processor processor = new PatientServiceThrift.Processor(handler);

            new Thread(() => RunServer(processor)).Start();

            // app.UseWebSockets();

            // app.Use(async (context, next) =>
            // {
            //     if (context.Request.Path == "/ws")
            //     {
            //         if (context.WebSockets.IsWebSocketRequest)
            //         {
            //             var socket = await context.WebSockets.AcceptWebSocketAsync();
            //             var patientService = (PatientService)app.ApplicationServices.GetService(typeof(PatientService));
            //             await patientService.AddUser(socket);
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
            //     config.Options.SourcePath = "/./medpermapp-spa";
            //     if (env.IsDevelopment())
            //     {
            //         config.UseAngularCliServer("start");
            //     }
            // });
        }
    }
}
