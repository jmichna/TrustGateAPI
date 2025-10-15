using Microsoft.EntityFrameworkCore;
using TrustGateAPI.Configurations;
using TrustGateSqlLiteService.Db;

var builder = WebApplication.CreateBuilder(args);

JsonManagerConfig.ConfigureJsonSettings(builder.Services, builder.Configuration);

builder.Services.AddDbContext<SqlDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();

ServiceConfig.AddProjectService(builder.Services);

builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
