
var builder = WebApplication.CreateBuilder(args);

// Register MVC
builder.Services.AddControllersWithViews();

// Register HTTP services
builder.Services.AddHttpClient();
builder.Services.AddHttpContextAccessor();

// Register ApiService
builder.Services.AddScoped<RaceDayMvc.Services.ApiService>();

// Register session services
builder.Services.AddDistributedMemoryCache();

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

var app = builder.Build();

// Configure the HTTP request pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// Session must run before controllers access it
app.UseSession();

app.UseAuthorization();

// Default MVC route
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
