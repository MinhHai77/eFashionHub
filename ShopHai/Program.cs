using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer;
using ShopHai.Models;
using ShopHai.Repository;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddHttpContextAccessor();
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = "Cookies";
    options.DefaultSignInScheme = "Cookies";
    options.DefaultChallengeScheme = "Cookies";
})
.AddCookie("Cookies", options =>
{
    options.Cookie.Name = "YourCookieName";
    options.Cookie.HttpOnly = true;
    options.ExpireTimeSpan = TimeSpan.FromMinutes(30); // Thời gian hết hạn của cookie
});
builder.Services.AddAuthorization();
builder.Services.AddSession();

//Dependency Injection
builder.Services.AddDbContext<ShopClothesContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("ShopClothesDB"));
});
//DI
builder.Services.AddTransient<IProductRepository, ProductRepository>();
builder.Services.AddTransient<ICategoryRepository, CategoryRepository>();
builder.Services.AddTransient<IUserRepository, UserRepository>();
builder.Services.AddTransient<IInvoiceRepository, InvoiceRepository>();



var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();
app.UseSession();

app.MapAreaControllerRoute(
    name: "MyAreas",
    areaName: "Admin",
    pattern: "Admin/{action=Index}/{id?}",
    defaults: new { controller = "Home", action = "Index" });
app.MapAreaControllerRoute(
    name: "AdminProduct",
    areaName: "Admin",
    pattern: "Admin/Product/{action=ViewAllProduct}/{id?}",
    defaults: new { controller = "Product" });
app.MapAreaControllerRoute(
    name: "AdminUser",
    areaName: "Admin",
    pattern: "Admin/User/{action=ViewAllUser}/{id?}",
    defaults: new { controller = "User" });
app.MapAreaControllerRoute(
    name: "AdminInvoice",
    areaName: "Admin",
    pattern: "Admin/Invoice/{action=ViewAllInvoice}/{id?}",
    defaults: new { controller = "Invoice" });
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapControllerRoute(
    name: "logout",
    pattern: "{controller=Home}/{action=Logout}");
app.Run();
