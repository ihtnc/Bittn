using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using QckMox;
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

            services.AddControllers();
            services.AddDependencies();

            services
                .AddCors()
                .AddMvc(config => { config.Filters.Add<ModelValidationFilter>(); });

            services.AddApiDocumentation();

            services.AddQckMox(Configuration);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseDefaultFiles();
            app.UseStaticFiles();

            app.UseMiddleware<ExceptionHandlingMiddleware>();
            app.UseQckMox();

            if (!env.IsDevelopment())
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseRouting();
            app.UseCors(builder =>
                    builder.AllowAnyHeader()
                           .AllowAnyMethod()
                           .AllowCredentials()
                           .SetIsOriginAllowed(hostName => true));

            app.UseEndpoints(endpoints => endpoints.MapControllers());

            app.UseApiDocumentation();
        }
    }
}
