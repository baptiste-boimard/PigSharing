using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using PigSharing.Client;
using PigSharing.Client.Logic;
using PigSharing.Client.Models;


var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

// builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("https://punky.chickenkiller:4245") });
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("http://localhost:5248") });

// Service permet de stocker les variables utiles pour le front
builder.Services.AddSingleton<StateManager>();

// Service regrouper les méthodes d'appel au back
builder.Services.AddScoped<ImageService>();

// Ajout de BlazoredLocalStorage
builder.Services.AddBlazoredLocalStorage();

await builder.Build().RunAsync();
        
     



