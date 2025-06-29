using Microsoft.EntityFrameworkCore;
using RevenueRecognition.API.Middlewares;
using RevenueRecognition.Application.Services.Client;
using RevenueRecognition.Application.Services.Contract;
using RevenueRecognition.Application.Services.Currency;
using RevenueRecognition.Application.Services.Discount;
using RevenueRecognition.Application.Services.Revenue;
using RevenueRecognition.Application.Services.Software;
using RevenueRecognition.Application.Services.Subscription;
using RevenueRecognition.Infrastructure.DAL;
using RevenueRecognition.Infrastructure.Repositories.Client;
using RevenueRecognition.Infrastructure.Repositories.Contract;
using RevenueRecognition.Infrastructure.Repositories.Discount;
using RevenueRecognition.Infrastructure.Repositories.Software;
using RevenueRecognition.Infrastructure.Repositories.Subscription;
using RevenueRecognition.Infrastructure.Repositories.UnitOfWork;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IRevenueService, RevenueService>();
builder.Services.AddScoped<IDiscountService, DiscountService>();
builder.Services.AddScoped<IClientService, ClientService>();
builder.Services.AddScoped<ISubscriptionService, SubscriptionService>();
builder.Services.AddScoped<ISoftwareService, SoftwareService>();
builder.Services.AddScoped<IContractService, ContractService>();


builder.Services.AddScoped<ISoftwareRepository, SoftwareRepository>();
builder.Services.AddScoped<IClientRepository, ClientRepository>();
builder.Services.AddScoped<IDiscountRepository, DiscountRepository>();
builder.Services.AddScoped<IContractRepository, ContractRepository>();
builder.Services.AddScoped<ISubscriptionRepository, SubscriptionRepository>();

builder.Services.AddHttpClient<ICurrencyConverterService, CurrencyConverterService>();

var connectionString = builder.Configuration.GetConnectionString("DefaultDatabase");
builder.Services.AddDbContext<CompanyDbContext>(options => options
    .UseSqlServer(connectionString)
);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseMiddleware<ExceptionHandlerMiddleware>();

app.MapControllers();

app.Run();