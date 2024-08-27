using Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Microsoft.Win32;
using System.Text;
using System;
using ToDo.Services;
using ToDo.Repositories.Interfaces;
using ToDo.Repositories;
using ToDo.Services.Interfaces;
using ToDoBackend.Services;



var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Add CORS policy
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins",
        builder =>
        {
            builder.AllowAnyOrigin()
                   .AllowAnyMethod()
                   .AllowAnyHeader();
        });
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c => {
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "API",
        Version = "v1"
    });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 1safsfsdfdfd\"",
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement {
        {
            new OpenApiSecurityScheme {
                Reference = new OpenApiReference {
                    Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});

// Adding SQL service
var server = builder.Configuration["server"] ?? "localhost";
// var server =  "localhost";

var database = builder.Configuration["database"] ?? "ToDoDb";
var port = builder.Configuration["port"] ?? "1433";
var pass = builder.Configuration["password"] ?? "MohammedSaeed123$";
var user = builder.Configuration["dbuser"] ?? "SA";

// "Server=DESKTOP-PNPH01G\\SQLEXPRESS;Database=ToDoDb;Trusted_Connection=true;TrustServerCertificate=true;"

var connectionString = $"Server={server},{port};Initial Catalog={database};User ID={user};Password={pass};Trusted_Connection=false;TrustServerCertificate=true;";

builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlServer(connectionString);

    // options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

// Adding services
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IUserTaskService, UserTaskService>();
builder.Services.AddScoped<ITaskService, TaskService>();
builder.Services.AddScoped<IRegistrationService, RegistrationService>();
builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();
builder.Services.AddScoped<ITokenBlacklistService, TokenBlacklistService>();
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<IPasswordResetService, PasswordResetService>();
builder.Services.AddScoped<IConfirmationService, ConfirmationService>();

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserTaskRepository, UserTaskRepository>();

builder.Services.AddSingleton<IScopeFactory, ScopeFactory>();
builder.Services.AddScoped<IEmailConfirmationService, EmailConfirmationService>();

// Configure JWT authentication
var jwtSecret = builder.Configuration["Jwt:Secret"];
if (string.IsNullOrEmpty(jwtSecret))
{
    throw new ArgumentNullException("Jwt:Secret", "The JWT Secret is not set in the configuration.");
}
var key = Encoding.ASCII.GetBytes(jwtSecret);

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = false,
        ValidateAudience = false
    };
});

var app = builder.Build();

DataManagementService.MigrationInitialization(app);
// Configure the HTTP request pipeline.
// if (app.Environment.IsDevelopment())
// {
    app.UseSwagger();
    app.UseSwaggerUI();
// }

app.UseHttpsRedirection();

// Use CORS policy
app.UseCors("AllowAllOrigins");

app.UseAuthentication();
app.UseAuthorization();

app.UseLoginInterceptor(); // Use the extension method to add middleware
app.UseUserOperationsMiddleware();

app.MapControllers();

app.Run();