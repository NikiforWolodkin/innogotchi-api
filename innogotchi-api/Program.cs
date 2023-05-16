using BusinessLayer.Interfaces;
using BusinessLayer.RequestDtos;
using BusinessLayer.Services;
using ClientLayer.Middleware;
using DataLayer.Data;
using DataLayer.Interfaces;
using DataLayer.Repositories;
using FluentValidation;
using InnogotchiApi.Validators;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using DataLayer.RequestDtos;
using DataLayer.Services;
using Swashbuckle.AspNetCore.Filters;
using System.Text;
using innogotchi_api.Validators;

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

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IAvatarRepository, AvatarRepository>();
builder.Services.AddScoped<ICollaborationRepository, CollaborationRepository>();
builder.Services.AddScoped<IFarmRepository, FarmRepository>();
builder.Services.AddScoped<IInnogotchiRepository, InnogotchiRepository>();

builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IFarmService, FarmService>();
builder.Services.AddScoped<IInnogotchiService, InnogotchiService>();
builder.Services.AddScoped<ICollaborationService, CollaborationService>();
builder.Services.AddScoped<IAvatarService, AvatarService>();    

builder.Services.AddScoped<IValidator<UserSignupDto>, UserSignupValidator>();
builder.Services.AddScoped<IValidator<FarmAddDto>, FarmAddValidator>();
builder.Services.AddScoped<IValidator<InnogotchiAddDto>, InnogotchiAddValidator>();
builder.Services.AddScoped<IValidator<UserUpdatePasswordDto>, UserUpdatePasswordValidator>();
builder.Services.AddScoped<IValidator<UserUpdateProfileDto>, UserUpdateProfileValidator>();

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
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        builder => builder.MigrationsAssembly("Api")
    );
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseErrorHandler();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
