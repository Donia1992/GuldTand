using Guldtand.Domain.Repositories;
using Guldtand.Domain.Services;
using Guldtand.Data;
using Guldtand.Domain;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Guldtand.Domain.Helpers;
using AutoMapper;

namespace GuldtandApi
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
            services.AddDbContext<DataContext>(options => options.UseSqlServer(Configuration.GetConnectionString("AzureGuldtand"), sqlServerOptionsAction: x => x.MigrationsAssembly("Guldtand.Data")));

            services.AddScoped<ICustomerRepository, CustomerRepository>();

            services.AddScoped<IEmployeeService, EmployeeService>();
            services.AddScoped<IEmployeeRepository, EmployeeRepository>();

            services.AddScoped<IBlobService, BlobService>();
            services.AddScoped<IBlobRepository, BlobRepository>();

            services.AddScoped<IUserService, UserService>();

            services.Configure<BlobSettings>(Configuration.GetSection("BlobSettings"));
            services.Configure<AppSettings>(Configuration.GetSection("AppSettings"));
            
            services.AddCors();
            services.AddAutoMapper();
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            env.EnvironmentName = EnvironmentName.Development;
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/error");
                //app.UseHsts();
            }

            app.UseCors(builder =>
                builder
                .AllowAnyOrigin()
                .AllowAnyHeader()
                .AllowAnyMethod()
             );

            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}
