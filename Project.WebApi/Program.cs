using Project.Application.Extensions;
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

builder.Services.RequestJwtTokenInSwagger();

builder.Services.AddLogging();
builder.Services.ConfigureCorsPolicy();

// Custom extensions
var config = builder.AddConfiguration();
builder.AddJwtAuthentication(config.Secrets.JwtPrivateKey!);

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

app.UseCustomExceptionHandler();

app.Run();