using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.Cookies;
using BlogApp.Data.Abstract;
using BlogApp.Data.Concrete.EfCore;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<BlogContext>(options =>
options.UseSqlServer(builder.Configuration.GetConnectionString("myconn")));


builder.Services.AddScoped<IArticleRepository, EfArticleRepository>();
builder.Services.AddScoped<ITagRepository, EfTagRepository>();
builder.Services.AddScoped<IUserRepository, EfUserRepository>();
builder.Services.AddScoped<ICommentRepository, EfCommentRepository>();


builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(options => {
    options.LoginPath = "/Users/Login";
});

var app = builder.Build();

SeedData.FillTheTestDatas(app);


app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllerRoute(
    name: "articles_details",
    pattern: "articles/details/{url}",
    defaults: new { controller = "Articles", action = "Details" }
);

app.MapControllerRoute(
    name: "articles_by_tag",
    pattern: "articles/tag/{tag}",
    defaults: new { controller = "Articles", action = "Index" }
);

app.MapControllerRoute(
    name: "user_profile",
    pattern: "profile/{username}",
    defaults: new { controller = "Users", action = "Profile" }
);

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Articles}/{action=Index}/{id?}");

app.Run();
