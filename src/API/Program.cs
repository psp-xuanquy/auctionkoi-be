using CleanArchitecture.Configuration;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using KoiAuction.API.Configuration;
using KoiAuction.Configuration;
using KoiAuction.API.Filters;
using KoiAuction.Infrastructure.Persistences;
using Microsoft.OpenApi.Models;
using System.Reflection;
using KoiAuction.Application;
using KoiAuction.Infrastructure;
using KoiAuction.API.Services;
using KoiAuction.Application.Common.Interfaces;
using KoiAuction.Application.Common.Email;
using Newtonsoft.Json.Converters;
using KoiAuction.Domain.Entities;

namespace KoiAuction
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            ConfigureServices(builder.Services, builder.Configuration);


            var app = builder.Build();

            // Configure the HTTP request pipeline.
            Configure(app, app.Environment, builder.Configuration);

            app.Run();
        }

        private static void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            // Add CORS policy
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                    builder => builder.AllowAnyOrigin()
                                      .AllowAnyMethod()
                                      .AllowAnyHeader());
            });

            // Add controllers with filters
            services.AddControllers(opt =>
            {

                opt.Filters.Add<ExceptionFilter>();
            }).AddNewtonsoftJson(options =>
            {
                // Add StringEnumConverter to convert enum to string
                options.SerializerSettings.Converters.Add(new StringEnumConverter());
            }); ;
            


            // Add Identity services
            services.AddIdentity<AspNetUser, IdentityRole>()
             .AddEntityFrameworkStores<ApplicationDbContext>()
             .AddDefaultTokenProviders();


            // Additional service configurations
            services.AddHttpContextAccessor();
            services.AddEndpointsApiExplorer();
            services.ConfigureApplicationSecurity(configuration);
            services.ConfigureProblemDetails();
            services.ConfigureApiVersioning();
            services.ConfigureSwagger(configuration);

            //Add Application and Infrastructure (Important)
            services.AddApplication(configuration);
            services.AddInfrastructure(configuration);

            services.Configure<EmailSettings>(configuration.GetSection("EmailSettings"));

            services.AddTransient<IEmailService, EmailService>();

            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
        }

        private static void Configure(IApplicationBuilder app, IWebHostEnvironment env, IConfiguration configuration)
        {
            // Environment-specific settings
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();
            app.UseCors("CorsPolicy");
            app.UseExceptionHandler();
            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                //endpoints.MapDefaultHealthChecks();
                endpoints.MapControllers();
            });
            app.UseSwashbuckle(configuration);
        }
    }
}
