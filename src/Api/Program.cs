using Api.Filters;
using Application.UseCases.Attendees;
using Application.UseCases.Events;
using Domain.Interfaces;
using Infrastructure.AutoMapper;
using Infrastructure.Context;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<PassInDbContext>(options =>
{
    var path = Directory.GetCurrentDirectory().Split("src");
    var dbPath = Path.Combine(path[0], "PassInDb.db");

    options.UseSqlite($"Data Source={dbPath}");
});

builder.Services.AddMvc(options => options.Filters.Add(typeof(ExceptionFilter)));

builder.Services.AddScoped<RegisterEventUseCase>();
builder.Services.AddScoped<GetEventByIdUseCase>();
builder.Services.AddScoped<RegisterAttendeeOnEventUseCase>();
builder.Services.AddScoped<GetAllByEventIdUseCase>();
builder.Services.AddScoped<CheckInAttendeeOnEventUseCase>();

builder.Services.AddScoped<IEventRepository, EventRepository>();
builder.Services.AddScoped<IAttendeeRepository, AttendeeRepository>();
builder.Services.AddScoped<ICheckInRepository, CheckInRepository>();

//builder.Services.AddScoped<>();

builder.Services.AddHttpContextAccessor();

builder.Services.AddAutoMapper(typeof(AutoMapperProfile));
builder.Services.AddAutoMapper(typeof(CheckInMapper));

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