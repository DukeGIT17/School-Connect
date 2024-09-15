using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SchoolConnect_DomainLayer.Data;
using SchoolConnect_RepositoryLayer.Interfaces;
using SchoolConnect_RepositoryLayer.Repositories;
using SchoolConnect_ServiceLayer.ISystemAdminServices;
using SchoolConnect_ServiceLayer.SystemAdminServices;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<SchoolConnectDbContext>(options 
    => options.UseSqlite(builder.Configuration.GetConnectionString("SQLiteConnectionString"),
    b => b.MigrationsAssembly(nameof(SchoolConnect_WebAPI))));
builder.Services.AddDbContext<SignInDbContext>(options
    => options.UseSqlite(builder.Configuration.GetConnectionString("SQLiteConnectionString"), 
    b => b.MigrationsAssembly(nameof(SchoolConnect_WebAPI))));
builder.Services.AddDefaultIdentity<IdentityUser>(options
    => options.SignIn.RequireConfirmedAccount = false)
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<SignInDbContext>();
builder.Services.AddScoped<ISchool, SchoolRepository>();
builder.Services.AddScoped<ISysAdmin, SystemAdminRepository>();
builder.Services.AddScoped<ISignInRepo, SignInRepository>();
builder.Services.AddScoped<ISystemAdminService, SystemAdminService>();


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.ConfigureSwaggerGen(setup =>
{
    setup.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "School Connect",
        Version = "v1"
    });
});

var app = builder.Build();

app.UseSwagger();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
