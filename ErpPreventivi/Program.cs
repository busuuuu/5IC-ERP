using Microsoft.EntityFrameworkCore;
using ErpPreventivi.Data;

var builder = WebApplication.CreateBuilder(args);

// Aggiunge MVC
builder.Services.AddControllersWithViews();

// Configura EF Core con SQLite (database locale)
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite("Data Source=erp_preventivi.db"));

var app = builder.Build();

// Crea il database automaticamente all'avvio (se non esiste)
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.EnsureCreated();
}

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
    pattern: "{controller=Preventivi}/{action=Index}/{id?}");

app.Run();
