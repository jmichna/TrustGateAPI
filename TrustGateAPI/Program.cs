using TrustGateAPI.Configurations;

var builder = WebApplication.CreateBuilder(args);

JsonManagerConfig.ConfigureJsonSettings(builder.Services, builder.Configuration);

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
