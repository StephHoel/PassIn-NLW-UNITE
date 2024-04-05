using Api.Filters;
using Application.UseCases.Attendees;
using Application.UseCases.Events;
using Domain.Interfaces;
using Infrastructure.AutoMapper;
using Infrastructure.Context;
using Infrastructure.Repositories;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddRouting(options => options.LowercaseUrls = true);

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

app.UseRequestLocalization(new RequestLocalizationOptions
{
    //DefaultRequestCulture = new RequestCulture("en-US"),
    DefaultRequestCulture = new RequestCulture("pt-BR"),
    SupportedCultures =
                [
                //new CultureInfo("en-US"),
                new CultureInfo("pt-BR")
                ]
});

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();