using SearchScraper.Models;
using Microsoft.EntityFrameworkCore;
using SearchScraper.Services;

var builder = WebApplication.CreateBuilder(args);

// Add CORS policy
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigins",
        policy =>
        {
            policy.AllowAnyOrigin()
                  .AllowAnyHeader()
                  .AllowAnyMethod();
        });
});

var connectionString = builder.Configuration.GetConnectionString("SearchScraperDbContextConnection") ??
    throw new InvalidOperationException("Connection string 'SearchScraperDbContextConnection' not found");

builder.Services.AddDbContext<SearchScraperDbContext>(options => options.UseSqlServer(connectionString)); ;

builder.Services.AddControllersWithViews();

builder.Services.AddScoped<ISearchRepository, SearchRepository>();
builder.Services.AddScoped<SearchScraperService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

DbInitializer.Seed(app);
app.Run();
