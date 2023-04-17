using JSONanalyser.Middleware;
using JSONanalyser.Service;
using Serilog;
using Serilog.Extensions.Logging;

var builder = WebApplication.CreateBuilder(args);

// Add Serilog functionality
builder.Host.UseSerilog((hostContext, loggerConfiguration) =>
_ = loggerConfiguration.ReadFrom.Configuration(builder.Configuration));

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
// Register Services for dependency injection
//A new instance of a class that implements the Unit interface will be created every time the API is called, for each separate user.
builder.Services.AddHttpClient<DataService>();
builder.Services.AddMemoryCache();
builder.Services.AddTransient<DataService>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",builder =>
    {
        builder.AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseMiddleware<ExceptionMiddleware>();

app.UseCors("AllowAll");

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
