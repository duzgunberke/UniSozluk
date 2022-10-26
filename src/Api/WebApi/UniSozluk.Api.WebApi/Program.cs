using FluentValidation.AspNetCore;
using UniSozluk.Api.Application.Extensions;
using UniSozluk.Infrastructure.Persistence.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers()
                .AddJsonOptions(opt =>
                {
                    opt.JsonSerializerOptions.PropertyNamingPolicy = null;
                })
                .AddFluentValidation();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddApplicationRegistration();
builder.Services.AddInfrastructureRegistration(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
