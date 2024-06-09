using FluentValidation;
using FluentValidation.AspNetCore;
using MicroElements.Swashbuckle.FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Serilog;
using TASK3_Business.Exceptions;
using TASK3_Business.Services.Implementations;
using TASK3_Business.Services.Interfaces;
using TASK3_Business.Validators.StudentValidators;
using TASK3_DataAccess;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers().ConfigureApiBehaviorOptions(options => {
  options.InvalidModelStateResponseFactory = context => {
    var errors = context.ModelState.Where(x => x.Value.Errors.Count > 0)
    .Select(x => new RestExceptionError(x.Key, x.Value.Errors.First().ErrorMessage)).ToList();
    return new BadRequestObjectResult(new { message = "", errors });
  };
});

builder.Services.AddDbContext<AppDbContext>(option => {
  option.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});
//Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//Fluent Validation
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddFluentValidationClientsideAdapters();
builder.Services.AddValidatorsFromAssemblyContaining<StudentCreateOneDtoValidator>();

//Custom Services
builder.Services.AddScoped<IStudentService, StudentService>();
builder.Services.AddScoped<IGroupService, GroupService>();

//Serilog
builder.Host.UseSerilog((hostingContext, loggerConfiguration) => {
  loggerConfiguration
  .ReadFrom.Configuration(hostingContext.Configuration);
});

//Microelements
builder.Services.AddFluentValidationRulesToSwagger();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment()) {
  app.UseSwagger();
  app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

