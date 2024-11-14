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
using API.Services;
using Microsoft.Extensions.Hosting;
using Domain.IRepositories.IBaseRepositories;
using Infrastructure.Repositories.BaseRepositories;
using KoiAuction.Domain.IRepositories;
using KoiAuction.Domain.Repositories;
using KoiAuction.Infrastructure.Repositories;
using Application.Common.Library;

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
            //services.AddSwaggerGen(c =>
            //{
            //    //c.SwaggerDoc("v1", new OpenApiInfo { Title = "Koi Auction API", Version = "v1" });

            //    //var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            //    //var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
            //    //c.IncludeXmlComments(xmlPath);
            //});

            // Add Identity services
            services.AddIdentity<UserEntity, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            // PayOS service registration
            services.AddScoped<PayOSService>(provider =>
            {
                var payOSSettings = configuration.GetSection("payOS");
                var clientId = payOSSettings["clientId"];
                var apiKey = payOSSettings["apiKey"];
                var checksumKey = payOSSettings["checksumKey"];

                if (string.IsNullOrEmpty(clientId) || string.IsNullOrEmpty(apiKey) || string.IsNullOrEmpty(checksumKey))
                {
                    throw new InvalidOperationException("PayOS configuration is invalid.");
                }

                return new PayOSService(clientId, apiKey, checksumKey);
            });


            // Additional service configurations
            services.AddHttpContextAccessor();
            services.AddEndpointsApiExplorer();
            services.ConfigureApplicationSecurity(configuration);
            services.ConfigureProblemDetails();
            services.ConfigureApiVersioning();
            services.ConfigureSwagger(configuration);

            // Add Application and Infrastructure (Important)
            services.AddApplication(configuration);
            services.AddInfrastructure(configuration);

            services.Configure<EmailSettings>(configuration.GetSection("EmailSettings"));
            services.AddTransient<IEmailService, EmailService>();

            services.AddHostedService<AuctionExpiryService>();

            services.AddHttpContextAccessor();

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
            app.UseStaticFiles();   //wwwroot
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                //endpoints.MapDefaultHealthChecks();
                endpoints.MapControllers();
            });
            app.UseSwashbuckle(configuration);
        }
    }
}
