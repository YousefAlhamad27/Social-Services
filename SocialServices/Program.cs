using clsSocialServicesBussiness;
using clsSocialServicesDataAccess;
using clsSocialServicesDataAccess.Feedback;
using clsSocialServicesDataAccess.Posts;
using clsSocialServicesDataAccess.Services;
using dotenv.net;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Text;





var builder = WebApplication.CreateBuilder(args);
clsConfigurations config = new clsConfigurations(builder.Configuration);
// Add services to the container.

//services block 
{
    builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(
        clsConfigurations.ConnectionString, // Your connection string
        sqlServerOptions =>
        {
            // This dynamically gets the correct assembly name "clsSocialServicesDataAccess"
            sqlServerOptions.MigrationsAssembly(typeof(AppDbContext).Assembly.FullName);
        }
    ));
  builder.Services.AddScoped<ServiceApplicationRepository>();
    builder.Services.AddScoped<FeedbackRepository>();
    builder.Services.AddScoped<PostRepository>();
    builder.Services.AddScoped<UserRepository>();
    builder.Services.AddScoped<PersonRepository>();
    builder.Services.AddScoped<clsPerson>();
    builder.Services.AddScoped<clsPost>();
    builder.Services.AddScoped<clsUser>();
    builder.Services.AddScoped<clsServiceApplication>();
    builder.Services.AddScoped<clsFeedBack>();

    builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        // 1. Configure the options for the JWT Bearer Handler
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true, // Validate the server that created the token
            ValidateAudience = true, // Validate the recipient of the token is authorized
            ValidateLifetime = true, // Check if the token is expired
            ValidateIssuerSigningKey = true, // Validate the signature key

            // 2. Set the actual values (these should be retrieved from a configuration file/vault)
            ValidIssuer = builder.Configuration["Jwt:Issuer"], // The issuer (server)
            ValidAudience = builder.Configuration["Jwt:Audience"], // The audience (API resource)
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"] ?? throw new InvalidOperationException("JWT Key not configured."))
            ),
            // Optional: Set a clock skew to tolerate slight time differences between servers
            ClockSkew = TimeSpan.Zero
        };

    });
    builder.Services.AddSwaggerGen(options =>
    {
        // Define the security scheme
        options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
        {
            In = ParameterLocation.Header,
            Description = "Please enter 'Bearer [jwt]'",
            Name = "Authorization",
            Type = SecuritySchemeType.ApiKey,
            BearerFormat = "JWT",
            Scheme = "Bearer"
        });

        // Add the security requirement
        options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
    });

    builder.Services.AddAuthorization();

}

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthentication();
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();







//{
//    "username": "Maher_Ahmad",
//  "firstName": "Yousef",
//  "secondName": "Ahmad",
//  "lastName": "AlHasan",
//  "email": "Maher@g.com",
//  "phone": "45521",
//  "age": 18,
//  "imagepath": ""
//}
