using Microsoft.EntityFrameworkCore;
using SchoolConnect_DomainLayer.Data;
using SchoolConnect_Web_App.IServices;
using SchoolConnect_Web_App.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<SchoolConnectDbContext>(options 
    => options.UseSqlite(builder.Configuration.GetConnectionString("SQLiteConnectionString")));
//builder.Services.AddDbContext<SignInDbContext>(options
//    => options.UseSqlite(builder.Configuration.GetConnectionString("SQLiteConnectionString"), 
//    b => b.MigrationsAssembly(nameof(SchoolConnect_WebAPI))));
//builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = false)
//    .AddRoles<IdentityRole>()
//    .AddEntityFrameworkStores<SignInDbContext>();
builder.Services.AddHttpClient<ISchoolService, SchoolService>(c =>
c.BaseAddress = new Uri("https://localhost:7091"));
builder.Services.AddScoped<ISystemAdminService, SystemAdminService>();
builder.Services.AddScoped<ISignInService, SignInService>();
builder.Services.AddScoped<ISchoolService, SchoolService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

//using (var scope = app.Services.CreateScope())
//{
//    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
//    var roles = new[] { "System Admin", "Principal", "Teacher", "Parent" };
//    foreach (var role in roles)
//    {
//        if (!await roleManager.RoleExistsAsync(role))
//        {
//            await roleManager.CreateAsync(new IdentityRole(role));
//        }
//    }
//}

app.Run();
