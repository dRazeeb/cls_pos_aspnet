using Serilog;
using GCTL.UI.Core.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.ConfigureContext(builder.Configuration);
builder.Services.ConfigureServices();
builder.Services.ConfigureMapper();
builder.Services.AddControllersWithViews();

builder.Services.ConfigureSession();

builder.Services.ReadConfiguration(builder.Configuration);

Log.Logger = new LoggerConfiguration().CreateBootstrapLogger();
builder.Host.UseSerilog((ctx, lc) =>
{
    lc.MinimumLevel.Warning();
    lc.ReadFrom.Configuration(ctx.Configuration);
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseSerilogRequestLogging();

// PICK YOUR FLAVOR.

//app.UseErrorPage(ErrorPageOptions.ShowAll);
//app.UseStatusCodePages(); // There is a default response but any of the following can be used to change the behavior.

// app.UseStatusCodePages(context => context.HttpContext.Response.SendAsync("Handler, status code: " + context.HttpContext.Response.StatusCode, "text/plain"));
// app.UseStatusCodePages("text/plain", "Response, status code: {0}");
// app.UseStatusCodePagesWithRedirects("~/errors/{0}"); // PathBase relative
// app.UseStatusCodePagesWithRedirects("/base/errors/{0}"); // Absolute
// app.UseStatusCodePages(builder => builder.UseWelcomePage());
//app.UseStatusCodePagesWithReExecute("/errors/{0}");

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.UseSession();

app.MapControllerRoute(
    name: "preview",
    pattern: "Preview/{name}",
    defaults: new { controller = "Preview", action = "Viewer", name = "" });

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
