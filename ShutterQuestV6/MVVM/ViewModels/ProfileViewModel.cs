using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.Maui.Controls;
using ShutterQuestV6.Services;
using ShutterQuestV6.MVVM.Models;
using ShutterQuestV6;

namespace ShutterQuest.MVVM.ViewModels
{
    public class ProfileViewModel : INotifyPropertyChanged
    {
        private readonly DatabaseService _databaseService;

        public ProfileViewModel()
        {
            _databaseService = new DatabaseService(Path.Combine(FileSystem.AppDataDirectory, "shutterquest.db3"));
            LoadUserDataAsync();

            EditDisplayNameCommand = new Command(async () => await EditDisplayNameAsync());
        }

        private string _username;
        public string Username
        {
            get => _username;
            set
            {
                _username = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(UsernameWithAt)); // Trigger update for the formatted username
            }
        }

        public string UsernameWithAt => $"@{Username}";

        private string _displayName;
        public string DisplayName
        {
            get => _displayName;
            set
            {
                _displayName = value;
                OnPropertyChanged();
            }
        }

        private string _email;
        public string Email
        {
            get => _email;
            set
            {
                _email = value;
                OnPropertyChanged();
            }
        }

        public string DisplayNameOrUsername => string.IsNullOrEmpty(DisplayName) ? Username : DisplayName;

        public ICommand EditDisplayNameCommand { get; }

        private async void LoadUserDataAsync()
        {
            try
            {
                var userId = App.LoggedInUserId;
                var user = await _databaseService.GetByIdAsync<User>(userId);

                if (user != null)
                {
                    Username = user.Username;
                    Email = user.Email;
                    DisplayName = user.DisplayName;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error loading user data: {ex.Message}");
            }
        }

        private async Task EditDisplayNameAsync()
        {
            string newDisplayName = await Application.Current.MainPage.DisplayPromptAsync(
                "Edit Display Name",
                "Enter a new display name (max 20 characters):",
                "Save",
                "Cancel",
                initialValue: DisplayName,
                maxLength: 20);

            if (!string.IsNullOrEmpty(newDisplayName))
            {
                DisplayName = newDisplayName;

                // Update the database
                var user = await _databaseService.GetByIdAsync<User>(App.LoggedInUserId);
                if (user != null)
                {
                    user.DisplayName = newDisplayName;
                    await _databaseService.UpdateAsync(user);
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
