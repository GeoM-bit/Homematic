using HomematicApp.Context.Context;
using HomematicApp.Domain.Abstractions;
using HomematicApp.Repositories;
using HomematicApp.Service.Services;
using HomematicApp.Service.Services.EmailService;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);


// Add services to the container.
builder.Services.AddControllersWithViews(); 
builder.Services.AddMvc();
builder.Services.AddControllers();
builder.Services.AddOptions();
builder.Services.AddDbContext<HomematicContext>(options =>
    options.UseMySql(builder.Configuration.GetConnectionString("HomematicConnectionString"), ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("HomematicConnectionString"))));

builder.Services.Configure<SenderOptions>(options => builder.Configuration.GetSection("SenderOptions").Bind(options));
builder.Services.AddScoped<IAuthenticationRepository, AuthenticationRepository>();
builder.Services.AddScoped<IHashService, HashService>();
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddTransient<IEmailSender, EmailSender>();
builder.Services.AddScoped<ITemplateFillerService, TemplateFillerService>();
builder.Services.AddAutoMapper(typeof(Program));

//builder.Services.ConfigureApplicationCookie(options =>
//{
//    options.Cookie.Name = "IdentityCookie";
//    options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
//    options.LoginPath = "/Authentication/Login";
//    options.ReturnUrlParameter = CookieAuthenticationDefaults.ReturnUrlParameter;
//    options.SlidingExpiration = true;
//});

builder.Services.AddSession();
builder.Services.AddDistributedMemoryCache();
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new
        SymmetricSecurityKey
        (Encoding.UTF8.GetBytes
        (builder.Configuration["Jwt:Key"]))
    };
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
app.UseSession();
app.Use(async (context, next) =>
{
    var token = context.Session.GetString("Token");
    if (!string.IsNullOrEmpty(token))
    {
        context.Request.Headers.Add("Authorization", "Bearer " + token);
    }
    await next();
});
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Authentication}/{action=Register}/{id?}");

app.Run();
