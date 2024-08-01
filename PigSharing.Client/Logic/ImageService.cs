using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using PigSharing.Client.Models;

namespace PigSharing.Client.Logic;

public class ImageService
{
    private readonly HttpClient _client;
    private readonly ILocalStorageService _localStorage;
    private readonly NavigationManager _navigation;
    private readonly StateManager _stateManager;

    public ImageService(
        HttpClient client,
        ILocalStorageService localStorage,
        NavigationManager navigation,
        StateManager stateManager )
    {
        _client = client;
        _localStorage = localStorage;
        _navigation = navigation;
        _stateManager = stateManager;
    }

    // Méthode pour demander l'upload de l'image
    public async Task<bool> UploadImage(IBrowserFile file, Account account)
    {
        var accountContent = new StringContent(
            JsonSerializer.Serialize(account),
            Encoding.UTF8,
            "application/json");
     
        var content = new MultipartFormDataContent();
     
        var streamContent = new StreamContent(file.OpenReadStream());
        streamContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(file.ContentType);
     
        content.Add(streamContent, "file", file.Name);
        content.Add(accountContent, "account");
     
        var response = await _client.PostAsync("/api/picture/upload", content);

        if (response.IsSuccessStatusCode)
        {
            return true;
        }

        return false;
    }
    
    // Permet d'obtenir les images publiques
    public async Task GetPublics()
    {
        try
        {
           // _stateManager.Publics  = await _client.GetFromJsonAsync<Picture[]>("/api/picture/getallpublics");
           var response  = await _client.GetAsync("/api/picture/getallpublics");
           
           Console.WriteLine(response);

           var responseString = await response.Content.ReadAsStringAsync();
           
           Console.WriteLine(responseString);
           
           
           _stateManager.Publics = await response.Content.ReadFromJsonAsync<Picture[]>();
           

           // foreach (var picture in _stateManager.Publics)
           // {
           //     Console.WriteLine($"{picture.Created}");
           // }
           // Console.WriteLine();

        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
    
    // Permet d'obtenir les images privates
    public async Task GetAllImages(Account account)
    {
        try
        {
            _stateManager.AllImages = await _client.GetFromJsonAsync<Picture[]>($"/api/picture/getallimages/{account.ConnectionToken}");
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
    
    
}
    
    