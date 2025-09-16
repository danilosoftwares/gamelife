using GameLifeModels.Interface.Repository;
using GameLifeModels.Interface.Services;
using GameLifeRepository;
using GameLifeServices;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

var dbPath = "gamelife.db";
var sqliteFactory = new SqliteConnectionFactory(dbPath);
DbInitializer.EnsureCreated(sqliteFactory);

// Configura o Serilog para ler do appsettings.json/appsettings.Development.json
builder.Host.UseSerilog((context, services, configuration) =>
{
    configuration
        .ReadFrom.Configuration(context.Configuration)
        .ReadFrom.Services(services);
});

builder.Services.AddSingleton(sqliteFactory);
builder.Services.AddScoped<IBoardRepository, BoardRepository>();
builder.Services.AddScoped<IBoardService, BoardService>();

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors();

app.UseAuthorization();

app.MapControllers();

app.Run();
