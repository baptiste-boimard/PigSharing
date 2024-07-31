namespace PigSharing.Client.Models;

public class StateManager
{
    // Permet de surveiller les changement des valeurs du State
    public event Action OnChange;
    
    private bool _userConnected= false;
    private bool _toggleAllImagePrivate = false;
    private bool _togglePublics = false;
    private Picture[] _publics = new Picture[] {};
    private Picture[] _allImages = new Picture[] {};
    
    
    // public bool UserConnected { get => _userConnected; set
    //     {if (_userConnected != value)
    //         {
    //             _userConnected = value;
    //             NotifyStateChanged();
    //         }
    //     }
    // }

    public bool UserConnected
    {
        get => _userConnected;
        set => SetField(ref _userConnected, value);
    }
    
    public Picture[] Publics
    {
        get => _publics;
        set => SetField(ref _publics, value);
    }

    public Picture[] AllImages
    {
        get => _allImages;
        set => SetField(ref _allImages, value);
    }

    public bool TogglePublics
    {
        get => _togglePublics;
        set => SetField(ref _togglePublics, value);
    }

    public bool ToggleAllImagePrivate
    {
        get => _toggleAllImagePrivate;
        set => SetField(ref _toggleAllImagePrivate, value);
    }

    // En cas de changement d'une des valeurs du StateManager
    // NotifyStateChanged est appelé
    private void SetField<T>(ref T field, T value)
    {
        if (!EqualityComparer<T>.Default.Equals(field, value))
        {
            field = value;
            NotifyStateChanged();
        }
    }
    
    public void NotifyStateChanged() => OnChange?.Invoke();
}