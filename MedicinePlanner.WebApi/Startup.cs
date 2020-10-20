using MedicinePlanner.Core.Repositories.Interfaces;
using MedicinePlanner.Core.Repositories;
using MedicinePlanner.Core.Services.Interfaces;
using MedicinePlanner.Core.Services;
using MedicinePlanner.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json.Serialization;
using AutoMapper;
using System;

namespace MedicinePlanner.WebApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ApplicationContext>(options =>
                 options.UseSqlServer
                 (
                    Configuration.GetConnectionString("DefaultConnection")
                 ));
            services.AddControllers().AddNewtonsoftJson(s =>
            {
                s.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            });

            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            services.AddScoped<IMedicineRepo, MedicineRepo>();
            services.AddScoped<IMedicineService, MedicineService>();
            services.AddScoped<IFoodRelationRepo, FoodRelationRepo>();
            services.AddScoped<IFoodRelationService, FoodRelationService>();
            services.AddScoped<IPharmaceuticalFormRepo, PharmaceuticalFormRepo>();
            services.AddScoped<IPharmaceuticalFormService, PharmaceuticalFormService>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
