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
using System.Linq;
using Microsoft.AspNetCore.Authentication.Google;
using MedicinePlanner.WebApi.Configs;
using Microsoft.AspNetCore.Authentication.Cookies;
using MedicinePlanner.WebApi.Auth.Services.Interfaces;
using MedicinePlanner.WebApi.Auth.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using MedicinePlanner.WebApi.Auth.Configs;
using System.Text;

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

            services.AddCors(options =>
            {
                options.AddPolicy("CORS", builder =>
                {
                    builder
                        .WithOrigins(
                            Configuration["App:CorsOrigins"]
                                .Split(",", StringSplitOptions.RemoveEmptyEntries)
                                .ToArray()
                        )
                        .SetIsOriginAllowedToAllowWildcardSubdomains()
                        .WithExposedHeaders("Location", "Upload-Offset", "Upload-Length")
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowCredentials();
                });
            });

            services.AddControllers().AddNewtonsoftJson(s =>
            {
                s.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            });

            services.Configure<GoogleSecretsOptions>(Configuration.GetSection("Google"));

            GoogleSecretsOptions googleOptions = Configuration.GetSection("Google").Get<GoogleSecretsOptions>();

            
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme )
                .AddJwtBearer(options =>
                {
                    options.RequireHttpsMetadata = false;
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidIssuer = Configuration.GetSection("Jwt").GetValue<string>("issuer"),

                        ValidateAudience = true,
                        ValidAudience = Configuration.GetSection("Jwt").GetValue<string>("audience"),
                        ValidateLifetime = true,

                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Configuration.GetSection("Jwt")
                                .GetValue<string>("key"))),
                        ValidateIssuerSigningKey = true,
                    };
                })
                .AddGoogle(options =>
                {
                    options.ClientId = googleOptions.clientId;
                    options.ClientSecret = googleOptions.clientSecret;
                    options.SaveTokens = true;
                });

            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            services.AddScoped<AuthConfigsManager>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IMedicineRepo, MedicineRepo>();
            services.AddScoped<IMedicineService, MedicineService>();
            services.AddScoped<IFoodRelationRepo, FoodRelationRepo>();
            services.AddScoped<IFoodRelationService, FoodRelationService>();
            services.AddScoped<IPharmaceuticalFormRepo, PharmaceuticalFormRepo>();
            services.AddScoped<IPharmaceuticalFormService, PharmaceuticalFormService>();
            services.AddScoped<IUserRepo, UserRepo>();
            services.AddScoped<IUserService, UserService>();

        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCors("CORS");

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
