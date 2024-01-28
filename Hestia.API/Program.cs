using System.Text.Json;
using Hestia.API.Extensions;
using Hestia.Infrastructure.Application;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<ApplicationSettings>(builder.Configuration.GetSection("Application"));

//Make sure the json response is snake_case
builder.Services.ConfigureHttpJsonOptions(options => options.SerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower);

builder.Services.AddControllers()
    //Make sure the json request is snake_case
    .AddJsonOptions(options => options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower);
builder.Services.AddHestiaDatabase(builder.Configuration);

builder.Services.AddHestiaRepositories();
builder.Services.AddHestiaServices();

builder.Services.AddHestiaAuthentication(builder.Configuration);

builder.Services.AddHestiaSwagger();

builder.Services.AddHestiaCors();

builder.Services.AddHttpClient();

WebApplication app = builder.Build();

app.MapControllers();

if (app.Environment.IsDevelopment())
{
    app.UseHestiaSwagger();
}

app.UseHttpsRedirection();

app.UseHestiaAuthentication();

app.UseHestiaCors();

app.Run();
