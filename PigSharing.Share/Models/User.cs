namespace PigSharing.Share.Models;

public class User
{
    private bool _userConnected;
    public bool UserConnected
    {
        get => _userConnected;
        set
        {
            if (_userConnected != value)
            {
                _userConnected = value;
                NotifyStateChanged();
            }
        }
    }

    public event Action OnChange;

    private void NotifyStateChanged() => OnChange?.Invoke();
    
}