using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.SqlServer.Server;
using TrustGateAPI.Configurations;
using TrustGateSqlLiteService.Db;

var builder = WebApplication.CreateBuilder(args);

JsonManagerConfig.ConfigureJsonSettings(builder.Services, builder.Configuration);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<SqlDbContext>(options =>
    options.UseSqlite(connectionString)
);

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
