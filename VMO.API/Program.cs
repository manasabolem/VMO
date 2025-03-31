using VMO.API.Services.Interfaces;
using VMO.API.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Register the SalesService for Dependency Injection
builder.Services.AddScoped<ISalesService, SalesService>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Enable serving static files from wwwroot
app.UseStaticFiles();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
