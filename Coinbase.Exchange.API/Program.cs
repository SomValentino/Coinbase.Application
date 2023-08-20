using Coinbase.Exchange.API;
using Coinbase.Exchange.Infrastructure;
using Coinbase.Exchange.Logic;
using Coinbase.Exchange.SharedKernel.Models.Configuration;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddServiceInfrastructureServices(builder.Configuration);
builder.Services.AddApplicationServices();
builder.Services.Configure<ApiConfiguration>(builder.Configuration.GetSection("ApiSettings"));
builder.Services.AddAPIServices(builder.Configuration);

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
