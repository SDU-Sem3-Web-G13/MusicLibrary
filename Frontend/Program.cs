using Microsoft.AspNetCore.Mvc;
using Backend;
using Frontend.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddMvc().AddRazorPagesOptions(o =>
            o.Conventions.ConfigureFilter(new IgnoreAntiforgeryTokenAttribute()));
builder.Services.AddSession();
builder.Services.AddMemoryCache();

// Register backend services
builder.Services.AddBackendServices();

// Register frontend models
builder.Services.AddScoped<ILoginRegisterModel, LoginRegisterModel>();
builder.Services.AddScoped<IAlbumsModel, AlbumsModel>();
builder.Services.AddScoped<IAdministrationModel, AdministrationModel>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();
app.UseSession();
app.MapRazorPages();

app.Run();