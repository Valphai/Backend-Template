using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Project.Domain.Security;
using System.Text;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.OpenApi.Models;

namespace Project.WebApi.Extensions;

public static class BuilderExtension
{
    public static Configuration AddConfiguration(this WebApplicationBuilder builder)
    {
        var config = new Configuration();
        
        builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
        builder.Configuration.Bind(config);

        builder.Services.AddSingleton(config);
        
        return config;
    }

    public static void AddJwtAuthentication(this WebApplicationBuilder builder, string jwtPrivateKey)
    {
        builder.Services
            .AddAuthentication(x => {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(x => {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters {
                    IssuerSigningKey =
                        new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtPrivateKey)),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });

        builder.Services.AddAuthorization();
    }

    public static void UseCustomExceptionHandler(this IApplicationBuilder applicationBuilder)
    {
        applicationBuilder.UseExceptionHandler(errorApp => {
            errorApp.Run(async context => {
                context.Response.StatusCode = 500;
                context.Response.ContentType = "application/json";

                var logger = context.RequestServices
                    .GetRequiredService<ILoggerFactory>()
                    .CreateLogger("[Global]");

                var exceptionFeature = context.Features.Get<IExceptionHandlerPathFeature>();
                var exception = exceptionFeature?.Error;

                logger.LogError(exception, "Unhandled exception occurred.");

                await context.Response.WriteAsync("Internal server error.");
            });
        });
    }

    public static void RequestJwtTokenInSwagger(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddSwaggerGen(options => {
            options.SwaggerDoc("v1", new OpenApiInfo { Title = "Demo API", Version = "v1" });
            options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme {
                In = ParameterLocation.Header,
                Description = "Please enter a valid token",
                Name = "Authorization",
                Type = SecuritySchemeType.Http,
                BearerFormat = "JWT",
                Scheme = "Bearer"
            });
            options.AddSecurityRequirement(new OpenApiSecurityRequirement {
                {
                    new OpenApiSecurityScheme {
                        Reference = new OpenApiReference {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    []
                }
            });

            options.CustomSchemaIds(type => type.ToString());
        });
    }
}