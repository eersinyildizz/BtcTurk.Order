using BtcTurk.Order.Api.Configurations;
using BtcTurk.Order.Api.CurrentUserProvider;
using BtcTurk.Order.Api.Data;
using BtcTurk.Order.Api.Handlers;
using BtcTurk.Order.Api.MessageBrokers;
using BtcTurk.Order.Api.NotificationHistories;
using BtcTurk.Order.Api.Orders;
using BtcTurk.Order.Api.Services;
using BtcTurk.Order.Api.Swagger;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add Serilog
builder.AddSerilog();

builder.Services.Configure<OrderOptions>(builder.Configuration.GetSection(OrderOptions.SectionName));

// Add services to the container.
builder.Services.AddDbContext<OrderDbContext>(options 
    => options.UseInMemoryDatabase("Orders"));

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(opt =>
{
    opt.OperationFilter<UserIdOperationFilter>();
});

// Configure Services
builder.Services.AddServices();
builder.Services.AddCurrentUser();
builder.Services.AddHandlers();
// Configure MessageBrokers
builder.AddMessageBrokers();

// Configure rate limiting
builder.Services.AddRateLimiting();

// Configure OpenTelemetry
builder.AddOpenTelemetry();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseDeveloperExceptionPage();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseExceptionHandlerMiddleware();
app.UseRateLimiter();
await app.ConsumeAsync();

app.Map("/", () => Results.Redirect("/swagger"));

// Configure the APIs
app.MapOrders();
app.MapNotificationHistories();

app.MapControllers();

await app.RunAsync();