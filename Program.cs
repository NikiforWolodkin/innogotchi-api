using innogotchi_api.Data;
using innogotchi_api.Interfaces;
using innogotchi_api.Repositories;
using innogotchi_api.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IFarmRepository, FarmRepository>();
builder.Services.AddScoped<IInnogotchiRepository, InnogotchiRepository>();

builder.Services.AddScoped<IUserAdapter, UserAdapter>();
builder.Services.AddScoped<IFarmAdapter, FarmAdapter>();
builder.Services.AddScoped<IInnogotchiAdapter, InnogotchiAdapter>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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
