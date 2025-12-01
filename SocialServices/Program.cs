using clsSocialServicesBussiness;
using Microsoft.EntityFrameworkCore;
using dotenv.net;
using clsSocialServicesDataAccess;
using clsSocialServicesDataAccess;
using clsSocialServicesDataAccess;




var builder = WebApplication.CreateBuilder(args);
clsConfiguration config = new clsConfiguration(builder.Configuration);
// Add services to the container.

//services block 
{
    builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(clsConfiguration.ConnectionString));
    builder.Services.AddScoped<UserRepository>();
    builder.Services.AddScoped<PersonRepository>();
    builder.Services.AddScoped<clsPerson>();
    builder.Services.AddScoped<clsUser>();
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


app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
