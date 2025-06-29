using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using RevenueRecognition.API.Middlewares;
using RevenueRecognition.Application.Services.Auth;
using RevenueRecognition.Application.Services.Client;
using RevenueRecognition.Application.Services.Contract;
using RevenueRecognition.Application.Services.Currency;
using RevenueRecognition.Application.Services.Discount;
using RevenueRecognition.Application.Services.Revenue;
using RevenueRecognition.Application.Services.Software;
using RevenueRecognition.Application.Services.Subscription;
using RevenueRecognition.Application.Services.Token;
using RevenueRecognition.Application.Services.Token.JwtOptions;
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

var jwtConfigData = builder.Configuration.GetSection("JwtConfig");
builder.Services.Configure<JwtOptions>(jwtConfigData);

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IRevenueService, RevenueService>();
builder.Services.AddScoped<IDiscountService, DiscountService>();
builder.Services.AddScoped<IClientService, ClientService>();
builder.Services.AddScoped<ISubscriptionService, SubscriptionService>();
builder.Services.AddScoped<ISoftwareService, SoftwareService>();
builder.Services.AddScoped<IContractService, ContractService>();
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IAuthService, AuthService>();


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
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(opt =>
{
    opt.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,   //by who
        ValidateAudience = true, //for whom
        ValidateLifetime = true,
        ClockSkew = TimeSpan.FromMinutes(1),
        ValidIssuer = jwtConfigData["Issuer"], //should come from configuration
        ValidAudience = jwtConfigData["Audience"], //should come from configuration
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtConfigData["Key"]))
    };
        
    opt.Events = new JwtBearerEvents
    {
        OnAuthenticationFailed = context =>
        {
            if (context.Exception.GetType() == typeof(SecurityTokenExpiredException))
            {
                context.Response.Headers.Add("Token-expired", "true");
            }
            return Task.CompletedTask;
        }
    };
});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// default credentials for db
// admin admin123
// user admin123
app.UseAuthentication();
app.UseAuthorization();

app.UseMiddleware<ExceptionHandlerMiddleware>();

app.MapControllers();

app.Run();