using EuroPlitka_DataAccess.Data;
using EuroPlitka_DataAccess.Extensions;
using EuroPlitka_DataAccess.Repository;
using EuroPlitka_DataAccess.Repository.IRepository;
using EuroPlitka_DataAccess.Repository.IReposotory;
using EuroPlitka_Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();


//connection to Db
builder.Services.AddDbContext<EuroPlitkaDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddScoped<IDbSeed, DbSeed>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<IProductTypeRepository, ProductTypeRepository>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IOrderDetailRepository, OrderDetailRepository>();
builder.Services.AddScoped<IOrderHeaderRepository, OrderHeaderRepository>();


//session
builder.Services.AddHttpContextAccessor();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(10);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});



//identity regiser
builder.Services.AddIdentity<AplicationUser, IdentityRole>()
                    .AddDefaultTokenProviders()
                    .AddDefaultUI()
                    .AddEntityFrameworkStores<EuroPlitkaDbContext>();

//identity Path
builder.Services.ConfigureApplicationCookie(option =>
{
    option.LoginPath = "/Account/Login";
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

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");



await app.InitializeDatabase<EuroPlitkaDbContext>().ConfigureAwait(false);//run migration(if exist) and create Db
                                                                          //DbSeed.InitializeData(app);

//Add service to conveyer (initializing role and Data(if exist))
var dbInitializer = app.Services.CreateScope();
var ini = dbInitializer.ServiceProvider.GetRequiredService<IDbSeed>();
ini.Initialize(); //Role
ini.InitializeData(app); //Data

//add session для обработки сеансов или сессий
app.UseSession();

app.Run();
