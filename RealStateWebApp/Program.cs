using RealStateApp.Infrastructure.Identity;
using RealStateApp.Core.Application;
using RealStateApp.Infrastructure.Shared;
using RealStateApp.Infrastructure.Persistance;
using RealStateApp.Middlewares;
using RealStateApp.MiddleWare;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddSession();
builder.Services.AddIdentityLayerForWeb(builder.Configuration);
builder.Services.AddPersistanceLayer(builder.Configuration);
builder.Services.AddApplicationLayer();
builder.Services.AddSharedInfrastructure(builder.Configuration);

builder.Services.AddControllersWithViews();

builder.Services.AddScoped<LoginAuthorize>();
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddTransient<ValidateUserSession, ValidateUserSession>();

var app = builder.Build();

await app.AddIdentitySeedsForWeb();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseSession();
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
