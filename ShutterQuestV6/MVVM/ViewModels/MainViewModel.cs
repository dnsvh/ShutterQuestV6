using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;
using ShutterQuestV6;
using ShutterQuestV6.MVVM.Models;

public class MainViewModel : INotifyPropertyChanged
{
    private readonly DatabaseService _databaseService;

    private string _pointsDisplay;
    public string PointsDisplay
    {
        get => _pointsDisplay;
        set
        {
            _pointsDisplay = value;
            OnPropertyChanged();
        }
    }

    public MainViewModel()
    {
        _databaseService = new DatabaseService(Path.Combine(FileSystem.AppDataDirectory, "shutterquest.db3"));
        Task.Run(async () => await LoadUserPointsAsync());
    }

    private async Task LoadUserPointsAsync()
    {
        // Use the static property from App to get the logged-in user ID
        var userId = App.LoggedInUserId;

        // Fetch user from the database
        var user = await _databaseService.GetByIdAsync<User>(userId);

        // Update PointsDisplay
        PointsDisplay = $"{user.Points} [icon]";
    }

    public event PropertyChangedEventHandler PropertyChanged;

    protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}

