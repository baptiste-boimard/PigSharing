using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Hosting;
using PigSharing.Client;
using PigSharing.Share.Models;


var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("http://localhost:5248") });
// builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

// Service permet de savoir si le User est connecté
builder.Services.AddSingleton<User>();

// Ajout de BlazoredLocalStorage
builder.Services.AddBlazoredLocalStorage();

await builder.Build().RunAsync();
        
     



