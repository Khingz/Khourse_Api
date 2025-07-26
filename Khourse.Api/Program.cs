using DotNetEnv;
using Khourse.Api.Common;
using Khourse.Api.Data;
using Khourse.Api.Extensions;
using Khourse.Api.Middlewares;
using Khourse.Api.Models;
using Khourse.Api.Repositories;
using Khourse.Api.Repositories.IRepositories;
using Khourse.Api.Services;
using Khourse.Api.Services.IServices;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

Env.Load();

var builder = WebApplication.CreateBuilder(args);

// Merge env variables into Iconfiguration
builder.Configuration.AddEnvironmentVariables();

// Variab;e that hols builder config
var config = builder.Configuration;

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddControllers().AddNewtonsoftJson(options =>
    {
        // Converts response data to snake case ==> we are using a resuable JsonConfig class
        options.SerializerSettings.ContractResolver = JsonConfig.SnakeCaseSettings.ContractResolver;
        
        // Ignores reference loops
        options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
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

builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme =
    options.DefaultAuthenticateScheme =
    options.DefaultChallengeScheme =
    options.DefaultForbidScheme =
    options.DefaultSignInScheme =
    options.DefaultSignOutScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidIssuer = config["Jwt:Issuer"],
        ValidateAudience = true,
        ValidAudience = config["Jwt:Audience"],
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(
            System.Text.Encoding.UTF8.GetBytes(config["Jwt:Key"]!)
        ),

    };
});

// Register other services
builder.Services.AddScoped<ICourseRepository, CourseRepository>();
builder.Services.AddScoped<IModuleRepository, ModuleRepository>();
builder.Services.AddScoped<IAccountRepository, AccountRepository>();
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IAccountService, AccountService>();

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
app.MapControllers();
app.UseMiddleware<GlobalExceptionHandlerMiddleware>();
app.Run();