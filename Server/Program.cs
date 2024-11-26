using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Server.Database;
using Server.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

builder.Services.AddDbContext<ReadingSpeedDbContext>(optionsBuilder =>
    optionsBuilder.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add session services to the container.
builder.Services.AddDistributedMemoryCache(); // Optional, for in-memory session storage
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Set session timeout (optional)
    options.Cookie.HttpOnly = true;  // Ensures that the session cookie is accessible only by the server
    options.Cookie.IsEssential = true; // Marks the cookie as essential for the application
    options.Cookie.SameSite = SameSiteMode.Strict; // Configures cookie SameSite policy
    options.Cookie.Name = "SessionCookie";
});

builder.Services.AddHttpContextAccessor(); // Required for accessing HttpContext

// Enable CORS to allow API calls
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(builder =>
    {
        builder.WithOrigins("https://localhost:5050") // Blazor app URL
               .AllowAnyHeader()
               .AllowAnyMethod();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseWebAssemblyDebugging();

}

app.UseHttpsRedirection();

app.UseBlazorFrameworkFiles();
app.UseStaticFiles();

// Add session middleware.
app.UseSession();

app.UseAuthorization();

app.UseCors();
app.UseRouting();

app.MapRazorPages();
app.MapControllers();
app.MapFallbackToFile("index.html");

app.Run();
