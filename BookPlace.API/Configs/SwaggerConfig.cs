using Microsoft.OpenApi.Models;

namespace BookPlace.API.Configs
{
    public static class SwaggerConfig
    {
        /// <summary>
        /// Add swagger config to Service Collection
        /// </summary>
        /// <param name="services">Instance of ServiceCollection received from Startup.cs</param>
        public static void Initialize(IServiceCollection services)
        {
            services.AddSwaggerGen(opts =>
            {
                opts.SwaggerDoc("v1", new OpenApiInfo { 
                    Title = "Book Place API",
                    Version = "v1",
                    Description = "Book your sport place",
                    Contact = new OpenApiContact
                    {
                        Name = "Fisnik Alidemi",
                        Email = "fisnik.alidemi@hotmail.com"
                    }
                });

                opts.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Specify authorization token here",
                    BearerFormat = "JWT",
                    Scheme = "Bearer",
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey
                });
                opts.AddSecurityRequirement(new OpenApiSecurityRequirement {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            },
                            In = ParameterLocation.Header,
                            Name = "Bearer"
                        },
                        new string[] { }
                    }
                });
            });
        }
        
        /// <summary>
        /// Setup swagger config after initialization. Sets the endpoints and registers it to the AppBuilder
        /// </summary>
        /// <param name="app">Instance of ApplicationBuilder received from Startup.cs</param>
        public static void Setup(IApplicationBuilder app)
        {
            app.UseSwagger();
            app.UseSwaggerUI(opts =>
            {
                opts.SwaggerEndpoint("/swagger/v1/swagger.json", "Book Place API");
            });
        }
    }
}
