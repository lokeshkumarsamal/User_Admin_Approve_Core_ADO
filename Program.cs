using User_Admin_Approve_Core_ADO.Repository;
using User_Admin_Approve_Core_ADO.Services;
using User_Admin_Approve_Core_ADO.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();


builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserServices,UserService>();


builder.Services.AddMemoryCache();

// Add session services
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(15); // Set session timeout
    options.Cookie.HttpOnly = true; // Make the session cookie HttpOnly
    options.Cookie.IsEssential = true; // Make the session cookie essential
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseSession();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=User}/{action=Index}/{id?}");

app.Run();
