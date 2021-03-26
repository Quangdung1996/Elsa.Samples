using Elsa.Activities.Console.Extensions;
using Elsa.Activities.Email.Extensions;
using Elsa.Activities.Http.Extensions;
using Elsa.Activities.Timers.Extensions;
using Elsa.Dashboard.Extensions;
using Elsa.Extensions;
using Elsa.Persistence.MongoDb.Extensions;
using Elsa.Samples.Extensions;
using Elsa.Samples.Handlers;
using Elsa.Samples.Interfaces;
using Elsa.Samples.Models;
using Elsa.Samples.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Elsa.Samples
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddRazorPages();
            services.AddServerSideBlazor();
            services.AddElsa(
                      elsa =>
                      {
                          elsa.AddMongoDbStores(Configuration, databaseName: "UserRegistration", connectionStringName: "MongoDb");
                      })
                  .AddElsaDashboard()
                  .AddEmailActivities(options => options.Bind(Configuration.GetSection("Elsa:Smtp")))
                  .AddHttpActivities(options => options.Bind(Configuration.GetSection("Elsa:Http")))
                  .AddTimerActivities(options => options.Bind(Configuration.GetSection("Elsa:Timers")))
                  .AddUserActivities()
                  .AddSingleton<IPasswordHasher, PasswordHasher>()
                  .AddMongoDbCollection<User>("Users")
                .AddNotificationHandlers(typeof(LiquidConfigurationHandler));

            services.AddConsoleActivities();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseStaticFiles();
            // Add Elsa's middleware to handle HTTP requests to workflows.
            app.UseHttpActivities();
            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapBlazorHub();
                endpoints.MapFallbackToPage("/_Host");

                // Attribute-based routing stuff.
                endpoints.MapControllers();
            });
        }
    }
}