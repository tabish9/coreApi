using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// 🔥 Enable CORS for Angular
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngular",
        policy => policy.WithOrigins("http://localhost:4200")
                        .AllowAnyMethod()
                        .AllowAnyHeader());
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// 🚀 Enable CORS before HTTPS
app.UseCors("AllowAngular");
app.UseHttpsRedirection();

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

var leads = new List<Lead>();

// Existing /tt Route
app.MapGet("/tt", () =>
{
    var forecast = Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast
        (
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
        ))
        .ToArray();
    return forecast;
})
.WithName("GetWeatherForecast")
.WithOpenApi();

// New Route: Receive Lead (POST /leads)
app.MapPost("/leads", ([FromBody] Lead lead) =>
{
    if (string.IsNullOrEmpty(lead.Name) || string.IsNullOrEmpty(lead.PhoneNumber))
    {
        return Results.BadRequest("Name and Phone Number are required.");
    }

    lead.Id = leads.Count + 1; // Assign an ID
    leads.Add(lead);

    Console.WriteLine($"Simulating sending text/email to {lead.Name}");

    return Results.Ok(new { message = "Lead received", lead });
})
.WithName("ReceiveLead")
.WithOpenApi();

// New Route: Get All Leads (GET /leads)
app.MapGet("/leads", () =>
{
    return Results.Ok(leads);
})
.WithName("GetAllLeads")
.WithOpenApi();

// New Route: Get Lead by ID (GET /leads/{id})
app.MapGet("/leads/{id:int}", (int id) =>
{
    var lead = leads.Find(l => l.Id == id);
    return lead is null ? Results.NotFound() : Results.Ok(lead);
})
.WithName("GetLeadById")
.WithOpenApi();

app.Run();

public partial class Program { } // Needed for WebApplicationFactory in Tests

// Models
record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}

record Lead
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string PhoneNumber { get; set; }
    public string ZipCode { get; set; }
    public bool CanCommunicate { get; set; }
    public string? Email { get; set; }
}
