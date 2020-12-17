using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Products.Core.Helper;
using Products.Core.Interfaces;
using Products.Core.Services;
using Products.Infrastructure.Data;
using Serilog;

namespace Products.API
{
    public class Startup
    {
        private readonly ILogger<Startup> _logger;

        public Startup(IWebHostEnvironment env)
        {            
            //Configuration = configuration;

            var builder = new ConfigurationBuilder()
            .SetBasePath(env.ContentRootPath)
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
            .AddEnvironmentVariables();

            Configuration = builder.Build();

            //Initialize Serilog on Startup scope (.net core 3.1 and above)
            Log.Logger = new LoggerConfiguration()
                            .ReadFrom.Configuration(Configuration)
                            .CreateLogger();

            Log.Information("Starting up");
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers().AddNewtonsoftJson();

            //Cors
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                            builder => builder.AllowAnyOrigin()
                                .AllowAnyMethod()
                                .AllowAnyHeader());
            });

            #region Application Services
            services.AddDbContext<ERPContext>(options =>
               options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
            services.AddTransient<IRepository, EFRepository>();
            services.AddTransient<IProductsService, ProductsService>();
            #endregion

            services.AddSwaggerDocument(config =>
            {
                config.PostProcess = document =>
                {
                    document.Info.Version = "v0.90.1";
                    document.Info.Title = "Products Management API";
                    document.Info.Description = "APIs to access the Products Database";
                    document.Info.TermsOfService = "None";
                    document.Info.Contact = new NSwag.OpenApiContact
                    {
                        Name = "Products.API",
                        Email = string.Empty,
                        Url = "https://example.com/contact"
                    };
                    document.Info.License = new NSwag.OpenApiLicense
                    {
                        Name = "Terms of Use",
                        Url = "https://example.com/license"
                    };
                };
            });

            //Prepare system parameters
            services.Configure<Settings>(options =>
            {
                //Verify the environment. In case of production, the connection string will be get from environment variable
                if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT").Equals("Production") || Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT").Equals("Staging"))
                {
                    if (String.IsNullOrEmpty(Environment.GetEnvironmentVariable("PRODUCT_IMAGE_MAX_SIZE"))) {
                        Log.Error("Environment variable PRODUCT_IMAGE_MAX_SIZE not initialized. The application will die.");
                        Environment.Exit(-1);
                    }
                    options.ProductImageMaxSize = Environment.GetEnvironmentVariable("PRODUCT_IMAGE_MAX_SIZE");
                }
                else
                {
                    options.ProductImageMaxSize = Configuration.GetSection("PRODUCT_IMAGE_MAX_SIZE").Value;
                }
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            // Shows UseCors with named policy.
            app.UseCors("CorsPolicy");

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSerilogRequestLogging();

            app.UseHttpsRedirection();

            app.UseOpenApi();
            app.UseSwaggerUi3();


            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

        }
    }
}
