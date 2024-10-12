using System.Reflection;
using Hestia.API.Extensions;
using Hestia.Application.Profiles.Auth;
using MyCSharp.HttpUserAgentParser.DependencyInjection;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpUserAgentParser();
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
builder.Services.AddAutoMapper(Assembly.GetAssembly(typeof(SessionProfile)));

WebApplication app = builder.Build();

app.UseHestiaMiddleware();
app.MapControllers();

if (app.Environment.IsDevelopment())
{
    app.UseHestiaSwagger();
}

app.UseHestiaCors();
app.UseHttpsRedirection();
app.UseHestiaAuthentication();
app.UseHestiaDatabase();

app.Run();
