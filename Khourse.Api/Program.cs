using DotNetEnv;
using Khourse.Api.Data;
using Khourse.Api.Repositories;
using Khourse.Api.Repositories.IRepositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

Env.Load();

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddControllers();
builder.Services.AddOpenApi();



// Register Database service 
var connectionString = Environment.GetEnvironmentVariable("DB_CONNECTION_STRING") ??
    throw new InvalidOperationException("Database connection string is not set in the environment variables.");

builder.Services.AddDbContext<AppDbContext>(options => options.UseNpgsql(connectionString));

// Register other services
builder.Services.AddScoped<ICourseRepository, CourseRepository>();

// Register API Versioning 
builder.Services.AddApiVersioning(options =>
{
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.DefaultApiVersion = new ApiVersion(1, 0);
    options.ReportApiVersions = true;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}


if (!app.Environment.IsDevelopment())
{
    app.UseHttpsRedirection();
}

app.MapControllers();
app.Run();