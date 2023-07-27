using Blogsiotoh.Models;
using Blogsiotoh.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

var builder = WebApplication.CreateBuilder(args);

var provider = builder.Services.BuildServiceProvider();
var _configuration = provider.GetRequiredService<IConfiguration>();


// Add services to the container.
builder.Services.AddControllersWithViews();

var connectionSection = _configuration.GetConnectionString("WebApiDatabase");
builder.Services.AddDbContext<BlogContext>(opt => opt.UseSqlite(connectionSection));

builder.Services.AddMvc();
builder.Services.AddScoped<IAuthServices, AuthServices>();
builder.Services.AddScoped<IPostServices, PostServices>();

builder.Services.AddDistributedMemoryCache();

builder.Services.AddMvc().AddSessionStateTempDataProvider();

builder.Services.AddSession();

builder.Services.AddHttpContextAccessor();

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromSeconds(1000);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

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
app.UseSession();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();

