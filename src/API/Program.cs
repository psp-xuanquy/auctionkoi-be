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
            });

            // Configure Swagger
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Koi Auction API", Version = "v1" });

                //var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                //var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                //c.IncludeXmlComments(xmlPath);
            });

            // Add Identity services
            services.AddIdentity<UserEntity, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            // Additional service configurations
            services.ConfigureApplicationSecurity(configuration);
            services.ConfigureProblemDetails();
            services.ConfigureApiVersioning();

            // Add Application and Infrastructure (Important)
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
            app.UseAuthentication();
            app.UseAuthorization();

            // Kích hoạt Swagger
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Koi Auction API V1");
                //c.RoutePrefix = string.Empty; 
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
