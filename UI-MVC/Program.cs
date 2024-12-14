using ArticleManagement.BL;
using ArticleManagement.DAL;
using ArticleManagement.DAL.EF;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<CatalogueDbContext>(optionsBuillder =>
{
    optionsBuillder.UseSqlite("Data Source=../CatalogueDatabase.db");
});
builder.Services.AddScoped<IRepository, EfRepository>();
builder.Services.AddScoped<IManager, Manager>();
builder.Services.AddControllersWithViews();

var app = builder.Build();

// database storage: EF Code First Trigger
using (var scope = app.Services.CreateScope())
{
    var catalogueDbContext = scope.ServiceProvider.GetService<CatalogueDbContext>(); 
    const bool doDropDatabase = true;
    bool isDbCreated = catalogueDbContext.CreateDatabase(doDropDatabase);
    if (isDbCreated) DataSeeder.Seed(catalogueDbContext);
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

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();