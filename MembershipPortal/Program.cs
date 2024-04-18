using MembershipPortal.Data;
using MembershipPortal.IRepositories;
using MembershipPortal.Repositories;
using MembershipPortal.IServices;
using MembershipPortal.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);


// Add services to the container.
var configuration = builder.Configuration;
var connectionString = configuration.GetConnectionString("connectionStringHemant");

builder.Services.AddDbContext<MembershipPortalDbContext>(options => options.UseSqlServer(connectionString));

builder.Services.AddScoped<IGenderService, GenderService>();
builder.Services.AddScoped<IGenderRepository, GenderRepository>();

builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();


builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IUserRepository, UserRepository>();

builder.Services.AddScoped<ISubscriberService, SubscriberService>();
builder.Services.AddScoped<ISubscriberRepository, SubscriberRepository>();


builder.Services.AddScoped<ISubscriptionService, SubscriptionService>();
builder.Services.AddScoped<ISubscriptionRepository, SubscriptionRepository>();

builder.Services.AddScoped<ITaxRepository, TaxRepository>();
builder.Services.AddScoped<ITaxService, TaxService>();

builder.Services.AddScoped<IDiscountRepository, DiscountRepository>();
builder.Services.AddScoped<IDiscountService, DiscountService>();


builder.Services.AddControllers();

builder.Services.AddCors(options =>
{
    options.AddPolicy("EnableCORS", builder =>
    {
        builder.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader();
    });
});


var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseAuthorization();
app.UseCors("EnableCORS");
app.MapControllers();

app.Run();
