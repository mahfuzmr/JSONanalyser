using JSONanalyser.Middleware;
using JSONanalyser.Service;
using Serilog;
using Serilog.Extensions.Logging;

var builder = WebApplication.CreateBuilder(args);

// Add Serilog functionality
builder.Host.UseSerilog((hostContext, loggerConfiguration) =>
_ = loggerConfiguration.ReadFrom.Configuration(builder.Configuration));
Log.Information("API starting....");

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

try
{
   
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
}
catch (Exception ex)
{
    Log.Fatal("API: API found error while Processing with following: Inner Exception {0}; exception:{1}; source: {2},stackTrace: {3} ", ex.InnerException?.Message ?? ex.Message, ex.Message, ex.Source == null ? "Empty" : ex.Source, ex.StackTrace == null ? "Empty" : ex.StackTrace);

}
finally
{
    Log.CloseAndFlush();
}




