using Coinbase.Exchange.API;
using Coinbase.Exchange.API.ExchangeHub;
using Coinbase.Exchange.Infrastructure;
using Coinbase.Exchange.Infrastructure.Data;
using Coinbase.Exchange.Logic;
using Coinbase.Exchange.SharedKernel.Models.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<ExchangeDbContext>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddServiceInfrastructureServices(builder.Configuration);
builder.Services.AddApplicationServices();
builder.Services.Configure<ApiConfiguration>(builder.Configuration.GetSection("AppSettings"));
builder.Services.AddAPIServices(builder.Configuration);
builder.Services.AddSignalR();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapHub<ExchangeHub>("/exchangesubscription");

app.MapControllers();

app.Run();
