using HomematicApp.Context.Context;
using HomematicApp.Domain.Abstractions;
using HomematicApp.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);


// Add services to the container.
builder.Services.AddControllersWithViews(); 
builder.Services.AddMvc();
builder.Services.AddControllers();
builder.Services.AddDbContext<HomematicContext>(options =>
    options.UseMySql(builder.Configuration.GetConnectionString("HomematicConnectionString"), ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("HomematicConnectionString"))));

builder.Services.AddScoped<IAuthenticationRepository, AuthenticationRepository>();
builder.Services.AddAutoMapper(typeof(Program));
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

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Authentication}/{action=Register}/{id?}");

app.Run();
