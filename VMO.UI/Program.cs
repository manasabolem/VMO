using VMO.UI.Services;
using VMO.UI.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);


// Add services to the container.
builder.Services.AddControllersWithViews();


builder.Services.AddHttpClient<ISalesApiService, SalesApiService>((client) => {
    var vmoApiServiceUrl = builder.Configuration.GetValue<string>("VmoApi:BaseUrl");
    if (string.IsNullOrEmpty(vmoApiServiceUrl))
    {
        throw new ArgumentNullException("Base URL for the API is not configured.");
    }

    client.BaseAddress = new Uri(vmoApiServiceUrl);
});



var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Sales/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Sales}/{action=Index}/{id?}");

app.Run();
