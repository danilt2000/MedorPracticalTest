using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace MedorPracticalTest.WebAPI.Configuration
{
        /// <summary>
        /// SwaggerConfiguration allows application to dynamically generate Swagger Docs for every available API version 
        /// </summary>
        public class SwaggerConfiguration : IConfigureNamedOptions<SwaggerGenOptions>
        {
                private readonly IApiVersionDescriptionProvider _provider;

                /// <summary>
                /// Constructor for <see cref="SwaggerConfiguration"/>
                /// </summary>
                /// <param name="provider">IApiVersionDescriptionProvider</param>
                public SwaggerConfiguration(IApiVersionDescriptionProvider provider)
                {
                        _provider = provider;
                }

                /// <summary>
                /// Configure SwaggerDocs
                /// </summary>
                /// <param name="options"></param>
                public void Configure(SwaggerGenOptions options)
                {
                        foreach (var apiVersionDescription in _provider.ApiVersionDescriptions)
                        {
                                options.SwaggerDoc(apiVersionDescription.GroupName, GenerateApiVersionInfo(apiVersionDescription));
                        }
                }

                /// <summary>
                /// Configure implementation of IConfigureNamedOptions
                /// </summary>
                /// <param name="name"></param>
                /// <param name="options"></param>
                public void Configure(string? name, SwaggerGenOptions options)
                {
                        Configure(options);
                }

                /// <summary>
                /// Helper to create API version info
                /// </summary>
                /// <param name="description"></param>
                /// <returns></returns>
                private static OpenApiInfo GenerateApiVersionInfo(ApiVersionDescription description)
                {
                        var info = new OpenApiInfo
                        {
                                Title = "MedorPracticalTest API",
                                Version = description.ApiVersion.ToString()
                        };

                        return info;
                }
        }
}
