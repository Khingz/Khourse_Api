using DotNetEnv;
using Khourse.Api.Common;
using Khourse.Api.Configs;
using Khourse.Api.Data;
using Khourse.Api.Extensions;
using Khourse.Api.Filters;
using Khourse.Api.Middlewares;
using Khourse.Api.Models;
using Khourse.Api.Repositories;
using Khourse.Api.Repositories.IRepositories;
using Khourse.Api.Services;
using Khourse.Api.Services.Email;
using Khourse.Api.Services.Email.IEmail;
using Khourse.Api.Services.IServices;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Converters;

Env.Load();

var builder = WebApplication.CreateBuilder(args);


// Variable that hols builder config
var config = builder.Configuration;

// Merge env variables into Iconfiguration
config.AddJsonFile("appsettings.json", optional: true)
        .AddEnvironmentVariables();

// Maps smtp values in env/appsettings.json into SmtpSettings class to be available for DI
builder.Services.Configure<SmtpSettings>(config.GetSection("Smtp"));

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddControllers().AddNewtonsoftJson(options =>
    {
        // Converts response data to snake case ==> we are using a resuable JsonConfig class
        options.SerializerSettings.ContractResolver = JsonConfig.SnakeCaseSettings.ContractResolver;

        // Ignores reference loops
        options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;

        // Serializes enums as strings
        options.SerializerSettings.Converters.Add(new StringEnumConverter());
    });

// Handles custom validation errors in Dtos
builder.Services.AddCustomValidationResponses();

builder.Services.AddOpenApi();

// Register Database service 
var connectionString = config["Db_Connection"];
builder.Services.AddDbContext<AppDbContext>(options => options.UseNpgsql(connectionString));

// Identity user setup == Remeber to bring in authentication and authorization middleware below (after app builds)
builder.Services.AddIdentity<AppUser, IdentityRole>(options =>
{
    options.Password.RequireDigit = true;
    options.Password.RequiredLength = 6;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireLowercase = false;
}).AddEntityFrameworkStores<AppDbContext>();

// Register other services
builder.Services.AddScoped<ICourseRepository, CourseRepository>();
builder.Services.AddScoped<IModuleRepository, ModuleRepository>();
builder.Services.AddScoped<IAccountRepository, AccountRepository>();
builder.Services.AddScoped<ILessonRepository, LessonRepository>();
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IAccountService, AccountService>();
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddSingleton<IEmailQueue, EmailQueue>();
builder.Services.AddHostedService<EmailBackgroundService>();
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<ICurrentUserService, CurrentUserService>();
builder.Services.AddCustomJwtAuthentication(builder.Configuration);
builder.Services.AddScoped<CourseExistFilter>();
builder.Services.AddScoped<ModuleExistFilter>();


// Register API Versioning 
builder.Services.AddApiVersioning(options =>
{
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.DefaultApiVersion = new ApiVersion(1, 0);
    options.ReportApiVersions = true;
});

// Build app
var app = builder.Build();

// Seed data defined in Data/DbSeeder
using (var scope = app.Services.CreateScope())
{
    var service = scope.ServiceProvider;
    await DbSeeder.SeedRolesAsync(service);
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}


if (!app.Environment.IsDevelopment())
{
    app.UseHttpsRedirection();
}

app.UseAuthentication();
app.UseAuthorization();
app.UseCustomMethodNotFoundHandler();
app.UseMiddleware<GlobalExceptionHandlerMiddleware>();
app.MapControllers();
app.Run();