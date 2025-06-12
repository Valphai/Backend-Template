using Microsoft.OpenApi.Models;
using Project.Application.Extensions;
using Project.Infrastructure.Config;
using Project.WebApi.Extensions;
using System.Text.Json.Serialization;
using Project.Infrastructure.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Insert this to migrations run
builder.Services.ConfigurePersistenceApp(builder.Configuration);

// Mediator
builder.Services.ConfigureApplicationApp();
builder.Services.AddMvc()
    .AddJsonOptions(x => x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

// Request JWT token in Swagger
builder.Services.AddSwaggerGen(options => {
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

builder.Services.AddLogging();
builder.Services.ConfigureCorsPolicy();

// Custom extensions
builder.AddConfiguration();
builder.AddJwtAuthentication();

var app = builder.Build();

// Allow CORS POLICY
app.UseCors();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapControllers();

app.Run();