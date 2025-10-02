using Microsoft.EntityFrameworkCore;
using CreditBureau.Core.Interfaces;
using CreditBureau.Infrastructure.Repositories;
using CreditBureau.Infrastructure.Data;
using CreditBureau.Services;
using CreditBureau.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Database Configuration
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseInMemoryDatabase("CreditBureauDB"));

// Dependency Injection
builder.Services.AddScoped<IBorrowerRepository, BorrowerRepository>();
builder.Services.AddScoped<ILenderRepository, LenderRepository>();
builder.Services.AddScoped<ICreditHistoryRepository, CreditHistoryRepository>();

builder.Services.AddScoped<IBorrowerService, BorrowerService>();
builder.Services.AddScoped<ILenderService, LenderService>();
builder.Services.AddScoped<ICreditHistoryService, CreditHistoryService>();

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

// Ensure database is created
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    dbContext.Database.EnsureCreated();
    Console.WriteLine("In-Memory Database created successfully!");
}

app.Run();