using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Project;
using Blazored.LocalStorage;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
builder.Services.AddBlazoredLocalStorage();  // Add  LocalStorage
builder.Services.AddScoped<ReadingTimeService>(); 
builder.Services.AddScoped<QuizScoreService>();


var host = builder.Build();

// Readingtimeservice is initialized before run
var readingTimeService = host.Services.GetRequiredService<ReadingTimeService>();
await readingTimeService.InitializeAsync();

await host.RunAsync();
