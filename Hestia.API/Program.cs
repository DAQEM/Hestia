using Hestia.API.Extensions;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpClient();
builder.Services.AddHestiaCors();
builder.Services.AddHestiaJson();
builder.Services.AddHestiaSwagger();
builder.Services.AddHestiaServices();
builder.Services.AddHestiaRepositories();
builder.Services.AddControllers().AddHestiaJson();
builder.Services.AddHestiaOptions(builder.Configuration);
builder.Services.AddHestiaDatabase(builder.Configuration);
builder.Services.AddHestiaAuthentication(builder.Configuration);

WebApplication app = builder.Build();

app.MapControllers();

if (app.Environment.IsDevelopment())
{
    app.UseHestiaSwagger();
}

app.UseHestiaCors();
app.UseHttpsRedirection();
app.UseHestiaAuthentication();
app.UseHestiaDatabase();
app.UseHestiaMiddleware();

app.Run();
