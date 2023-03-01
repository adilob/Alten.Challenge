using BookingApi.Extensions;
using BookingApi.Infrastructure.Data.Context;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
var corsConfigName = "default";

builder.Services.AddCors(options =>
{
    options.AddPolicy(
        name: corsConfigName,
        policy =>
        {
            policy.SetIsOriginAllowed(_ => true)
                .WithOrigins("http://localhost:8090", "http://localhost", "http://bookingapi.local");
        });
});

// Add services to the container.
builder.Services.AddDbContext(builder.Configuration);

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwagger();

builder.Services.AddManagers();
builder.Services.AddUnitOfWork();
builder.Services.AddRepositories();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
        options.RoutePrefix = string.Empty;
    });
}

app.UseHttpsRedirection();

app.UseCors(corsConfigName);

app.UseAuthorization();

app.MapControllers();

app.MigrateDatabase();

app.Run();
