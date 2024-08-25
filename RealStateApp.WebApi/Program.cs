using RealStateApp.Infrastructure.Persistance;
using RealStateApp.Infrastructure.Identity;
using RealStateApp.WebApi.Extensions;
using RealStateApp.Core.Application;
using RealStateApp.Infrastructure.Shared;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddPersistanceLayer(builder.Configuration);
builder.Services.AddIdentityLayerForApi(builder.Configuration);
builder.Services.AddApplicationLayer();
builder.Services.AddSharedInfrastructure(builder.Configuration);
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

builder.Services.AddControllers(options =>
{
    options.Filters.Add(new ProducesAttribute("application/json"));

}).ConfigureApiBehaviorOptions(options =>
{
    options.SuppressInferBindingSourcesForParameters = true;
    options.SuppressMapClientErrors = true;

});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddHealthChecks();
builder.Services.AddSwaggerExtension();
builder.Services.AddApiVersioningExtension();

builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession();

//builder.Services.AddSwaggerGen();


var app = builder.Build();

await app.AddIdentitySeedsForApi();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}


app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();
app.UseSwaggerExtension();
app.UseHealthChecks("/health");
app.UseSession();

app.MapControllers();

app.Run();