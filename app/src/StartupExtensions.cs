using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Bittn.Api.Http;
using Bittn.Api.Repositories;
using Bittn.Api.Repositories.AzureRepository;
using Bittn.Api.Services;

namespace Bittn.Api
{
    public static class StartupExtensions
    {
        public static IServiceCollection AddDependencies(this IServiceCollection services)
        {
            return services
                    .AddHttpClient()

                    .AddTransient<IBittnService, BittnService>()

                    .AddTransient<IHospitalRepository, AzureRepository>()
                    .AddTransient<IBittnRepository, LiteDbRepository>()

                    .AddTransient<ILiteDbProvider, LiteDbProvider>()

                    .AddTransient<IApiClient, ApiClient>()
                    .AddTransient<IApiRequestProvider, ApiRequestProvider>();
        }

        public static IServiceCollection AddApiDocumentation(this IServiceCollection services)
        {
            return services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("V1", new OpenApiInfo
                {
                    Title = "Bittn API",
                    Version = "V1",
                    Description = "Backend API for the Bittn app",
                    Contact = new OpenApiContact
                    {
                        Name = "Art Amurao"
                    }
                });
            });
        }

        public static IApplicationBuilder UseApiDocumentation(this IApplicationBuilder app)
        {
            return app.UseSwagger()
                      .UseSwaggerUI(c => { c.SwaggerEndpoint($"/swagger/V1/swagger.json", $"Bittn V1"); });
        }
    }
}