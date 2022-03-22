var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

builder.Services.AddHealthChecksUI(async setup =>
{
    using var provider = builder.Services.BuildServiceProvider();
    using var scope = provider.CreateScope();

    await Task.Delay(3);

})
    .AddInMemoryStorage();

var app = builder.Build();

app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapHealthChecksUI();

    // endpoints.MapControllerRoute(
    //    name: "default",
    //    pattern: "{controller=Home}/{action=Index}/{id?}");
});

app.Run();
