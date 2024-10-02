using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SchoolConnect_DomainLayer.Data;
using SchoolConnect_DomainLayer.Models;
using SchoolConnect_RepositoryLayer.Interfaces;
using SchoolConnect_RepositoryLayer.Repositories;
using SchoolConnect_ServiceLayer.IServerSideServices;
using SchoolConnect_ServiceLayer.ServerSideServices;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<SchoolConnectDbContext>(options
    => options.UseSqlite(builder.Configuration.GetConnectionString("SQLiteConnectionString"),
    b => b.MigrationsAssembly(nameof(SchoolConnect_WebAPI))));
builder.Services.AddDbContext<SignInDbContext>(options
    => options.UseSqlite(builder.Configuration.GetConnectionString("SQLiteConnectionString"),
    b => b.MigrationsAssembly(nameof(SchoolConnect_WebAPI))));
builder.Services.AddDefaultIdentity<CustomIdentityUser>(options
    => options.SignIn.RequireConfirmedAccount = false)
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<SignInDbContext>();
builder.Services.AddScoped<ISchool, SchoolRepository>();
builder.Services.AddScoped<ISchoolService, SchoolService>();
builder.Services.AddScoped<ISysAdmin, SystemAdminRepository>();
builder.Services.AddScoped<ISystemAdminService, AdminService>();
builder.Services.AddScoped<IPrincipal, PrincipalRepository>();
builder.Services.AddScoped<IPrincipalService, PrincipalService>();
builder.Services.AddScoped<ILearner, LearnerRepository>();
builder.Services.AddScoped<ILearnerService, LearnerService>();
builder.Services.AddScoped<ITeacher, TeacherRepository>();
builder.Services.AddScoped<ITeacherService, TeacherService>();
builder.Services.AddScoped<IParent, ParentRepository>();
builder.Services.AddScoped<IParentService, ParentService>();
builder.Services.AddScoped<PasswordValidator<CustomIdentityUser>>();
builder.Services.AddScoped<ISignInRepo, SignInRepository>();
builder.Services.AddScoped<ISignInService, SignInService>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", builder => builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
});


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

app.UseCors("AllowAll");

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    var roles = new[] { "System Admin", "Principal", "Teacher", "Parent" };
    foreach (var role in roles)
    {
        if (!await roleManager.RoleExistsAsync(role))
        {
            await roleManager.CreateAsync(new IdentityRole(role));
        }
    }

    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<CustomIdentityUser>>();
    string email = "Takatso@gmail.com",
        password = "TakiPassword123!";

    string email1 = "Lukhanyomayekiso98@gmail.com",
        password1 = "Lukhanyo12345!";

    var user = new CustomIdentityUser
    {
        UserName = email,
        Email = email,
        PhoneNumber = "0789512589",
    };

    var user1 = new CustomIdentityUser
    {
        UserName = email1,
        Email = email1,
        PhoneNumber = "0739002497",
    };

    if (await userManager.FindByEmailAsync(email) == null)
        await userManager.CreateAsync(user, password);

    if (await userManager.FindByEmailAsync(email1) == null)
        await userManager.CreateAsync(user1, password1);

    await userManager.AddToRoleAsync(user, "System Admin");
    await userManager.AddToRoleAsync(user1, "System Admin");
}

app.Run();
