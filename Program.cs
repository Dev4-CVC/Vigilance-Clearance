using Microsoft.AspNetCore.Authentication.Cookies;
using System.Globalization;
using System.Text.Json;
using VigilanceClearance.Data.Account;
using VigilanceClearance.DataAccessLayer.Ministry_Service;
using VigilanceClearance.DataAccessLayer.Ministry_Service;
using VigilanceClearance.DataAccessLayer.PESB_Service;
using VigilanceClearance.DataAccessLayer.PESB_Service;
using VigilanceClearance.Interface.Account;
using VigilanceClearance.Interface.Ministry;
using VigilanceClearance.Interface.PESB;
using VigilanceClearance.Middleware;
using VigilanceClearance.Services;
using VigilanceClearance.Interface.Admin;
using VigilanceClearance.DataAccessLayer.Admin_Service;
using VigilanceClearance.DataAccessLayer.Dealing_Hand_Service;
using VigilanceClearance.Interface.Dealing_Hand;

var time = 30;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddHttpContextAccessor();
builder.Services.AddSingleton<CaptchaService>();
builder.Services.AddHttpClient();


//Added ason date 17_07_2025
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Account/Login";
        options.LogoutPath = "/Account/Logout";
    });
//Added ason date 17_07_2025 End

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(time);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
    options.Cookie.SecurePolicy = CookieSecurePolicy.None;
    options.Cookie.SameSite = SameSiteMode.Lax;
});


builder.Services.AddSession();
builder.Services.AddHttpContextAccessor();

//interface services
builder.Services.AddScoped<IAdmin, Admin_Service>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IPESB, PESB_Services>();
builder.Services.AddScoped<IMinistry, Ministry_Service>();
builder.Services.AddScoped<IDealingHand, Dealing_Hand_Service>();

var cultureInfo = new CultureInfo("en-GB");
cultureInfo.DateTimeFormat.ShortDatePattern = "yyyy-MM-dd";
cultureInfo.DateTimeFormat.LongDatePattern = "yyyy-MM-dd";

CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;



//Added as on date 30-06-2025
builder.Services.AddControllers().AddJsonOptions(opts =>
{
    opts.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
});
//Added as on date 30-06-2025

//Added as on date 30-06-2025
builder.Services.AddControllers().AddJsonOptions(opts =>
{
    opts.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
});


var app = builder.Build();

#region Added as on date 17_07_2025

app.Use(async (context, next) =>
{
    context.Response.Headers["Cache-Control"] = "no-cache, no-store, must-revalidate";
    context.Response.Headers["Pragma"] = "no-cache";
    context.Response.Headers["Expires"] = "0";
    await next();
});
#endregion

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseSession();


app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();


app.UseMiddleware<JwtSessionAuthMiddleware>(); // Added as on date 17_07_2025
app.UseAuthentication(); // Added as on date 17_07_2025

app.UseAuthorization();

app.UseDeveloperExceptionPage();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Account}/{action=Login}/{id?}");


//app.UseEndpoints(endpoints =>
//{
//    endpoints.MapControllerRoute(
//        name: "default",
//        pattern: "{controller=Account}/{action=Login}/{id?}");
//});

app.Run();
