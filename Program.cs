using FluentValidation;
using innogotchi_api.Data;
using innogotchi_api.Dtos;
using innogotchi_api.Interfaces;
using innogotchi_api.Repositories;
using innogotchi_api.Services;
using innogotchi_api.Validators;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddAuthentication().AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        ValidateAudience = false,
        ValidateIssuer = false,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
            builder.Configuration.GetSection("Jwt:Token").Value!
        ))
    };
});

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddScoped<IValidator<UserRequestDto>, UserValidator>();

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IFarmRepository, FarmRepository>();
builder.Services.AddScoped<IInnogotchiRepository, InnogotchiRepository>();

builder.Services.AddScoped<IUserAdapter, UserAdapter>();
builder.Services.AddScoped<IFarmAdapter, FarmAdapter>();
builder.Services.AddScoped<IInnogotchiAdapter, InnogotchiAdapter>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey
    });

    options.OperationFilter<SecurityRequirementsOperationFilter>();
});

builder.Services.AddDbContext<DataContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

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
