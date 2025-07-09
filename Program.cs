using System.Globalization;
using System.Text.Json;
using VigilanceClearance.Data.Account;

using VigilanceClearance.DataAccessLayer.Ministry_Service;
using VigilanceClearance.DataAccessLayer.PESB_Service;
using VigilanceClearance.DataAccessLayer.PESB_Service;
using VigilanceClearance.DataAccessLayer.Ministry_Service;
using VigilanceClearance.Interface.Account;
using VigilanceClearance.Interface.Ministry;
using VigilanceClearance.Interface.PESB;
using VigilanceClearance.Services;

var time = 30;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddHttpContextAccessor();
builder.Services.AddSingleton<CaptchaService>();
builder.Services.AddHttpClient();


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
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IPESB, PESB_Services>();
builder.Services.AddScoped<IMinistry, Ministry_Service>();

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
//Added as on date 30-06-2025

//var cultureInfo = new CultureInfo("en-GB"); // or "en-CA" for yyyy-MM-dd
//cultureInfo.DateTimeFormat.ShortDatePattern = "yyyy-MM-dd";
//cultureInfo.DateTimeFormat.LongDatePattern = "yyyy-MM-dd";

//CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
//CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;


var app = builder.Build();

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

app.UseAuthorization();



app.MapControllerRoute(
    name: "default",

  pattern: "{controller=Account}/{action=Login}/{id?}");



app.Run();
