using System.Text.Json.Serialization;
using ArticleManagement.BL;
using ArticleManagement.DAL;
using ArticleManagement.DAL.EF;
using AspNetCoreLiveMonitoring.Extensions;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

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
builder.Services.AddLiveMonitoring(); // KdG TI ASP.NET Live Monitoring
var app = builder.Build();

// database storage: EF Code First Trigger
using (var scope = app.Services.CreateScope())
{
    var articleDbContext = scope.ServiceProvider.GetService<ArticleDbContext>(); 
    const bool doDropDatabase = true;
    bool isDbCreated = articleDbContext.CreateDatabase(doDropDatabase);
    if (isDbCreated) DataSeeder.Seed(articleDbContext);
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

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();