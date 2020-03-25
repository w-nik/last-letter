using System;
using System.Collections.Generic;
using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace GameOfWords
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
            //services.AddMvc()
            //    .AddNewtonsoftJson()
            //    .AddControllersAsServices()
            //    .AddApplicationPart(typeof(MatchController).GetTypeInfo().Assembly);

            services.AddSingleton<MatchService>();
            
            services.AddControllers();
            services.AddSingleton<GameOfWordsHost>();
            //services.AddHostedService<GameOfWordsHost>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            //app.UseHttpsRedirection();

            var lt = app.ApplicationServices.GetService<IHostApplicationLifetime>();
            var host = app.ApplicationServices.GetService<GameOfWordsHost>();
            lt.ApplicationStarted.Register(host.StartAsync);
            lt.ApplicationStopped.Register(host.StopAsync);

            app.UseRouting();

            //app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
