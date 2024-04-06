using Microsoft.EntityFrameworkCore;
using RoomService.DataAccess;
using RoomService.Interfaces;
using RoomService.Services;
using Serilog;


var builder = WebApplication.CreateBuilder(args);

var logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.FromLogContext()
    .CreateLogger();
builder.Logging.ClearProviders();
builder.Logging.AddSerilog(logger);

builder.Services.AddScoped<IRoomService, RoomService.Services.RoomService>();
builder.Services.AddScoped<IRoomTypeService, RoomTypeService>();
builder.Services.AddScoped<IAmenityService, AmenityService>();
builder.Services.AddScoped<IOccupiedRoomService, OccupiedRoomService>();
builder.Services.AddScoped<IReservationService, ReservationService>();
builder.Services.AddScoped<IHotelBranchService, HotelBranchService>();
builder.Services.AddScoped<IRoomFinderService, RoomFinderService>();
builder.Services.AddControllers();
builder.Services.AddStackExchangeRedisCache(redisOptions =>
{
    var connection = builder.Configuration
        .GetConnectionString("Redis");
    redisOptions.Configuration = connection;

});
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors(option =>
{
    if (!string.IsNullOrEmpty(builder.Configuration["ClientUrl"]))
    {
        option.AddPolicy("AllowClient", policy =>
        {
            var clientUrl = builder.Configuration["ClientUrl"];
            if (clientUrl != null) policy.WithOrigins(clientUrl);
            policy.AllowAnyHeader();
            policy.AllowAnyMethod();
        });
    }
});

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("AppDbConnection")));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    context.Database.Migrate();
    DbInitializer.Initialize(context);
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();