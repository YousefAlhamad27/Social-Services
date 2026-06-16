using clsSocialDataAccess.Posts.Preferances;
using clsSocialDataAccess.Professions;
using clsSocialDataAccess.Volunteers;
using clsSocialServicesBussiness;
using clsSocialServicesDataAccess;
using clsSocialServicesDataAccess.Admin;
using clsSocialServicesDataAccess.Counties___Cities;
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
using SocialServices.Classes;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System.Text.Json.Serialization;
using static clsSocialServicesBussiness.UtilLibrary;

var builder = WebApplication.CreateBuilder(args);



clsConfigurations config = new clsConfigurations(builder.Configuration);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// CORS Configuration
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowViteFrontend",
        policy =>
        {
            // Make sure the Vite port is actually in here!
            policy.WithOrigins("http://localhost:5173")
                  .AllowAnyHeader()
                  .AllowAnyMethod();
        });
});

builder.Services.AddSwaggerGen(c =>
{
    c.OperationFilter<FileUploadOperationFilter>();
});

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        // This single line ensures all enums are serialized as strings instead of numbers
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(
        clsConfigurations.ConnectionString,
        sqlServerOptions =>
        {
            sqlServerOptions.MigrationsAssembly(typeof(AppDbContext).Assembly.FullName);
        }
    ));


builder.Services.AddScoped<IServiceApplicationRepository,ServiceApplicationRepository>();
builder.Services.AddScoped<IFeedbackRepository,FeedbackRepository>();
builder.Services.AddScoped<IPostRepository,PostRepository>();
builder.Services.AddScoped<IUserRepository,UserRepository>();
builder.Services.AddScoped<IPersonRespository,PersonRepository>();
builder.Services.AddScoped<CountyCityRepository>();
builder.Services.AddScoped<clsPerson>();
builder.Services.AddScoped<clsPost>();
builder.Services.AddScoped<clsUser>();
builder.Services.AddScoped<clsServiceApplication>();
builder.Services.AddScoped<clsFeedBack>();
builder.Services.AddScoped<clsCountiesCities>();

builder.Services.AddScoped<clsVolunteer>();
builder.Services.AddScoped<IVolunteerRepository,VolunteerRepository>();
builder.Services.AddScoped<IAdminRepository,AdminRepository>();
builder.Services.AddScoped<clsAdminService>();
builder.Services.AddScoped<clsAiRecommendationService>();
builder.Services.AddScoped<ILogRepository,LogRepository>();
builder.Services.AddScoped<ILogViewRepository, LogViewRepository>();
builder.Services.AddScoped<IProfessionRepository, ProfessionRepository>();
builder.Services.AddScoped<clsProfession>();



// Authentication Configuration
builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"] ?? throw new InvalidOperationException("JWT Key not configured."))
            ),
            ClockSkew = TimeSpan.Zero
        };
    });

builder.Services.AddAuthorization();

// Swagger Configuration
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter 'Bearer [jwt]'",
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        BearerFormat = "JWT",
        Scheme = "Bearer"
    });

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


var app = builder.Build();

FileOperations.RootPath = app.Environment.ContentRootPath;
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new Microsoft.Extensions.FileProviders.PhysicalFileProvider(
        Path.Combine(Directory.GetCurrentDirectory(), "PostsImages")),
    RequestPath = "/PostsImages"
});

app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new Microsoft.Extensions.FileProviders.PhysicalFileProvider(
        Path.Combine(Directory.GetCurrentDirectory(), "UserImages")),
    RequestPath = "/UserImages"
});

app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new Microsoft.Extensions.FileProviders.PhysicalFileProvider(
        Path.Combine(Directory.GetCurrentDirectory(), "VolunteerImages")),
    RequestPath = "/VolunteerImages"
});

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();


app.UseRouting();


app.UseCors("AllowViteFrontend");


app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();


app.Run();