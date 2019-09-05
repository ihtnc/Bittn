using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Bittn.Api.Config;
using Bittn.Api.Filters;
using Bittn.Api.Middlewares;

namespace Bittn.Api
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
            services
                .AddOptions()
                .Configure<HospitalConfig>(option => { option.Database = Configuration["ENV_HOSPITAL_DATABASE"]; })
                .Configure<BittnConfig>(option => { option.Database = Configuration["ENV_BITTN_DATABASE"]; })
                .Configure<ApiBehaviorOptions>(options => { options.SuppressModelStateInvalidFilter = true; });

            services.AddDependencies();

            services
                .AddCors()
                .AddMvc(config => { config.Filters.Add<ModelValidationFilter>(); })
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            services.AddApiDocumentation();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseDefaultFiles();
            app.UseStaticFiles();

            app.UseMiddleware<ExceptionHandlingMiddleware>();

            if (!env.IsDevelopment())
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseCors(builder =>
                builder.AllowAnyOrigin()
                       .AllowAnyHeader()
                       .AllowAnyMethod()
                       .AllowCredentials());

            app.UseApiDocumentation();

            app.UseMvc();
        }
    }
}
