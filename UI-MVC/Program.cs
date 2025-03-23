using System.Text.Json.Serialization;
using ArticleManagement.BL;
using ArticleManagement.DAL;
using ArticleManagement.DAL.EF;
using ArticleManagement.UI.Web;
using AspNetCoreLiveMonitoring.Extensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("ArticleDbContextConnection") ?? throw new InvalidOperationException("Connection string 'ArticleDbContextConnection' not found.");

// Add services to the container.
builder.Services.AddDbContext<ArticleDbContext>(optionsBuilder =>
{
    optionsBuilder.UseSqlite("Data Source=../ArticleDatabase.db");
});

builder.Services.AddScoped<IRepository, EfRepository>();
builder.Services.AddScoped<IManager, Manager>();
builder.Services.AddControllersWithViews()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    });

// ASP.NET Identity
builder.Services.AddDefaultIdentity<IdentityUser>(/*options => options.SignIn.RequireConfirmedAccount = false*/) // RequireConfirmedAccount = false by default
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ArticleDbContext>();

// Authorization
builder.Services.AddAuthorization();

// Web API: fix auth responses
builder.Services.ConfigureApplicationCookie(cfg =>
{
    cfg.Events.OnRedirectToLogin += ctx =>
    {
        if (ctx.Request.Path.StartsWithSegments("/api"))
        {
            ctx.Response.StatusCode = 401; // unauthorized ( = unauthenticated)
        }
        return Task.CompletedTask;
    };
    cfg.Events.OnRedirectToAccessDenied += ctx =>
    {
        if (ctx.Request.Path.StartsWithSegments("/api"))
        {
            ctx.Response.StatusCode = 403; // forbidden ( = actually unauthorized for the particular role/user)
        }
        return Task.CompletedTask;
    };
});

// KdG TI ASP.NET Live Monitoring
builder.Services.AddLiveMonitoring(); 

var app = builder.Build();

// database storage: EF Code First Trigger
using (var scope = app.Services.CreateScope())
{
    var articleDbContext = scope.ServiceProvider.GetService<ArticleDbContext>(); 
    const bool doDropDatabase = true;
    bool isDbCreated = articleDbContext.CreateDatabase(doDropDatabase);
    if (isDbCreated)
    {
        // ASP.NET Identity
        var userManager = scope.ServiceProvider.GetService<UserManager<IdentityUser>>();
        var roleManager = scope.ServiceProvider.GetService<RoleManager<IdentityRole>>();
        IdentitySeeder identitySeeder = new IdentitySeeder(userManager, roleManager);
        await identitySeeder.AsyncSeed();
        // dummy data
        DataSeeder.Seed(articleDbContext);
    }
}

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
//KdG TI ASP.NET Live Monitoring
app.UseAndMapLiveMonitoring();

app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();

public partial class Program {}
