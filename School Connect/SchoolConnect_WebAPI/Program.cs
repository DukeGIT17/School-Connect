using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
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

// Main Functionality Services
builder.Services.AddScoped<ISchool, SchoolRepository>();
builder.Services.AddScoped<ISchoolService, SchoolService>();
builder.Services.AddScoped<IAnnouncement, AnnouncementRepo>();
builder.Services.AddScoped<IAnnouncementService, AnnouncementService>();
builder.Services.AddScoped<IGroupRepo, GroupRepository>();

// Actors Services
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

// Sign In Services
builder.Services.AddScoped<PasswordValidator<CustomIdentityUser>>();
builder.Services.AddScoped<ISignInRepo, SignInRepository>();
builder.Services.AddScoped<ISignInService, SignInService>();

builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
});

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
            await roleManager.CreateAsync(new IdentityRole(role));
    }

    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<CustomIdentityUser>>();
    var context = scope.ServiceProvider.GetRequiredService<SchoolConnectDbContext>();

    var systemAdmins = new SysAdmin[] {
        new()
        {
            ProfileImage = "Default Pic",
            Name = "Lukhanyo",
            Surname = "Mayekiso",
            Gender = "Male",
            Role = "System Admin",
            StaffNr = 14785,
            EmailAddress = "Lukhanyo@gmail.com",
            PhoneNumber = 739002497
        },
        new()
        {
            ProfileImage = "Default Pic",
            Name = "Takatso",
            Surname = "Senyatso",
            Gender = "Female",
            Role = "System Admin",
            StaffNr = 12365,
            EmailAddress = "Takatso@gmail.com",
            PhoneNumber = 789516542
        }
    };
    var admins = await context.SystemAdmins.ToListAsync();
    foreach (var systemAdmin in systemAdmins)
    {
        if (admins.FirstOrDefault(a => a.EmailAddress == systemAdmin.EmailAddress) == null)
            await context.AddAsync(systemAdmin);
    }
    await context.SaveChangesAsync();

    admins = await context.SystemAdmins.ToListAsync();
    if (!admins.IsNullOrEmpty())
    {
        foreach (var admin in admins)
        {
            if (await userManager.FindByEmailAsync(admin.EmailAddress) == null)
            {
                var newAdmin = new CustomIdentityUser
                {
                    Email = admin.EmailAddress,
                    UserName = admin.EmailAddress,
                    PhoneNumber = admin.PhoneNumber.ToString(),
                    ResetPassword = false
                };

                if (userManager.CreateAsync(newAdmin, "Default12345!").Result.Succeeded)
                    await userManager.AddToRoleAsync(newAdmin, admin.Role);
            }
        }
    }

    var schools = await context.Schools.Include("SchoolGroupsNP").ToListAsync();

    if (!schools.IsNullOrEmpty())
    {
        foreach (var school in schools)
        {
            if (!school.SchoolGroupsNP.Any())
            {
                Group allGroup = new()
                {
                    GroupName = "All",
                    GroupMemberIDs = [],
                    GroupActorNP = [],
                    SchoolID = school.Id
                };

                await context.AddAsync(allGroup);
            }
            else
                Console.WriteLine($"\n\nTHERE ARE ALREADY GROUPS IN THE SCHOOL {school.Name.ToUpper()}!!\n\n");
        }

        await context.SaveChangesAsync();
    }
    else
        Console.WriteLine("\n\nNO SCHOOLS IN THE DB YET!!\n\n");
}

app.Run();
