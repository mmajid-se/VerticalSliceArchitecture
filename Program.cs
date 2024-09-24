using Microsoft.Extensions.Options;
using Carter;
using MeesageService.InfraStructure;
using MeesageService.Data.Interfaces;
using MeesageService.Shared.ViewModels;
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.RegisterDependencies();

builder.Services.Configure<KafkaSettings>(builder.Configuration.GetSection(KafkaSettings.Key));
builder.Services.AddSingleton<IKafkaSettings>(provider => provider.GetRequiredService<IOptions<KafkaSettings>>().Value);

var app = builder.Build();
app.MapCarter();
await NativeInjectorBootstrapper.EnsureTopicsExistAsync(builder.Services);

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
